using System;
using System.Data;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Components;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Radzen;

using RadZenTutorial1.Data;

namespace RadZenTutorial1
{
    public partial class BlazorEmployeesService
    {
        BlazorEmployeesContext Context
        {
           get
           {
             return this.context;
           }
        }

        private readonly BlazorEmployeesContext context;
        private readonly NavigationManager navigationManager;

        public BlazorEmployeesService(BlazorEmployeesContext context, NavigationManager navigationManager)
        {
            this.context = context;
            this.navigationManager = navigationManager;
        }

        public void Reset() => Context.ChangeTracker.Entries().Where(e => e.Entity != null).ToList().ForEach(e => e.State = EntityState.Detached);

        public void ApplyQuery<T>(ref IQueryable<T> items, Query query = null)
        {
            if (query != null)
            {
                if (!string.IsNullOrEmpty(query.Filter))
                {
                    if (query.FilterParameters != null)
                    {
                        items = items.Where(query.Filter, query.FilterParameters);
                    }
                    else
                    {
                        items = items.Where(query.Filter);
                    }
                }

                if (!string.IsNullOrEmpty(query.OrderBy))
                {
                    items = items.OrderBy(query.OrderBy);
                }

                if (query.Skip.HasValue)
                {
                    items = items.Skip(query.Skip.Value);
                }

                if (query.Top.HasValue)
                {
                    items = items.Take(query.Top.Value);
                }
            }
        }


        public async Task ExportDepartmentsToExcel(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/blazoremployees/departments/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/blazoremployees/departments/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        public async Task ExportDepartmentsToCSV(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/blazoremployees/departments/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/blazoremployees/departments/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        partial void OnDepartmentsRead(ref IQueryable<RadZenTutorial1.Models.BlazorEmployees.Department> items);

        public async Task<IQueryable<RadZenTutorial1.Models.BlazorEmployees.Department>> GetDepartments(Query query = null)
        {
            var items = Context.Departments.AsQueryable();


            if (query != null)
            {
                if (!string.IsNullOrEmpty(query.Expand))
                {
                    var propertiesToExpand = query.Expand.Split(',');
                    foreach(var p in propertiesToExpand)
                    {
                        items = items.Include(p.Trim());
                    }
                }

                ApplyQuery(ref items, query);
            }

            OnDepartmentsRead(ref items);

            return await Task.FromResult(items);
        }

        partial void OnDepartmentGet(RadZenTutorial1.Models.BlazorEmployees.Department item);
        partial void OnGetDepartmentByDepartmentId(ref IQueryable<RadZenTutorial1.Models.BlazorEmployees.Department> items);


        public async Task<RadZenTutorial1.Models.BlazorEmployees.Department> GetDepartmentByDepartmentId(int departmentid)
        {
            var items = Context.Departments
                              .AsNoTracking()
                              .Where(i => i.DepartmentId == departmentid);

 
            OnGetDepartmentByDepartmentId(ref items);

            var itemToReturn = items.FirstOrDefault();

            OnDepartmentGet(itemToReturn);

            return await Task.FromResult(itemToReturn);
        }

        partial void OnDepartmentCreated(RadZenTutorial1.Models.BlazorEmployees.Department item);
        partial void OnAfterDepartmentCreated(RadZenTutorial1.Models.BlazorEmployees.Department item);

        public async Task<RadZenTutorial1.Models.BlazorEmployees.Department> CreateDepartment(RadZenTutorial1.Models.BlazorEmployees.Department department)
        {
            OnDepartmentCreated(department);

            var existingItem = Context.Departments
                              .Where(i => i.DepartmentId == department.DepartmentId)
                              .FirstOrDefault();

            if (existingItem != null)
            {
               throw new Exception("Item already available");
            }            

            try
            {
                Context.Departments.Add(department);
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(department).State = EntityState.Detached;
                throw;
            }

            OnAfterDepartmentCreated(department);

            return department;
        }

        public async Task<RadZenTutorial1.Models.BlazorEmployees.Department> CancelDepartmentChanges(RadZenTutorial1.Models.BlazorEmployees.Department item)
        {
            var entityToCancel = Context.Entry(item);
            if (entityToCancel.State == EntityState.Modified)
            {
              entityToCancel.CurrentValues.SetValues(entityToCancel.OriginalValues);
              entityToCancel.State = EntityState.Unchanged;
            }

            return item;
        }

        partial void OnDepartmentUpdated(RadZenTutorial1.Models.BlazorEmployees.Department item);
        partial void OnAfterDepartmentUpdated(RadZenTutorial1.Models.BlazorEmployees.Department item);

        public async Task<RadZenTutorial1.Models.BlazorEmployees.Department> UpdateDepartment(int departmentid, RadZenTutorial1.Models.BlazorEmployees.Department department)
        {
            OnDepartmentUpdated(department);

            var itemToUpdate = Context.Departments
                              .Where(i => i.DepartmentId == department.DepartmentId)
                              .FirstOrDefault();

            if (itemToUpdate == null)
            {
               throw new Exception("Item no longer available");
            }

            Reset();

            Context.Attach(department).State = EntityState.Modified;

            Context.SaveChanges();

            OnAfterDepartmentUpdated(department);

            return department;
        }

        partial void OnDepartmentDeleted(RadZenTutorial1.Models.BlazorEmployees.Department item);
        partial void OnAfterDepartmentDeleted(RadZenTutorial1.Models.BlazorEmployees.Department item);

        public async Task<RadZenTutorial1.Models.BlazorEmployees.Department> DeleteDepartment(int departmentid)
        {
            var itemToDelete = Context.Departments
                              .Where(i => i.DepartmentId == departmentid)
                              .FirstOrDefault();

            if (itemToDelete == null)
            {
               throw new Exception("Item no longer available");
            }

            OnDepartmentDeleted(itemToDelete);

            Reset();

            Context.Departments.Remove(itemToDelete);

            try
            {
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(itemToDelete).State = EntityState.Unchanged;
                throw;
            }

            OnAfterDepartmentDeleted(itemToDelete);

            return itemToDelete;
        }
    
        public async Task ExportEmployeesToExcel(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/blazoremployees/employees/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/blazoremployees/employees/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        public async Task ExportEmployeesToCSV(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/blazoremployees/employees/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/blazoremployees/employees/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        partial void OnEmployeesRead(ref IQueryable<RadZenTutorial1.Models.BlazorEmployees.Employee> items);

        public async Task<IQueryable<RadZenTutorial1.Models.BlazorEmployees.Employee>> GetEmployees(Query query = null)
        {
            var items = Context.Employees.AsQueryable();

            items = items.Include(i => i.Department);

            if (query != null)
            {
                if (!string.IsNullOrEmpty(query.Expand))
                {
                    var propertiesToExpand = query.Expand.Split(',');
                    foreach(var p in propertiesToExpand)
                    {
                        items = items.Include(p.Trim());
                    }
                }

                ApplyQuery(ref items, query);
            }

            OnEmployeesRead(ref items);

            return await Task.FromResult(items);
        }

        partial void OnEmployeeGet(RadZenTutorial1.Models.BlazorEmployees.Employee item);
        partial void OnGetEmployeeByEmployeeId(ref IQueryable<RadZenTutorial1.Models.BlazorEmployees.Employee> items);


        public async Task<RadZenTutorial1.Models.BlazorEmployees.Employee> GetEmployeeByEmployeeId(int employeeid)
        {
            var items = Context.Employees
                              .AsNoTracking()
                              .Where(i => i.EmployeeId == employeeid);

            items = items.Include(i => i.Department);
 
            OnGetEmployeeByEmployeeId(ref items);

            var itemToReturn = items.FirstOrDefault();

            OnEmployeeGet(itemToReturn);

            return await Task.FromResult(itemToReturn);
        }

        partial void OnEmployeeCreated(RadZenTutorial1.Models.BlazorEmployees.Employee item);
        partial void OnAfterEmployeeCreated(RadZenTutorial1.Models.BlazorEmployees.Employee item);

        public async Task<RadZenTutorial1.Models.BlazorEmployees.Employee> CreateEmployee(RadZenTutorial1.Models.BlazorEmployees.Employee employee)
        {
            OnEmployeeCreated(employee);

            var existingItem = Context.Employees
                              .Where(i => i.EmployeeId == employee.EmployeeId)
                              .FirstOrDefault();

            if (existingItem != null)
            {
               throw new Exception("Item already available");
            }            

            try
            {
                Context.Employees.Add(employee);
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(employee).State = EntityState.Detached;
                throw;
            }

            OnAfterEmployeeCreated(employee);

            return employee;
        }

        public async Task<RadZenTutorial1.Models.BlazorEmployees.Employee> CancelEmployeeChanges(RadZenTutorial1.Models.BlazorEmployees.Employee item)
        {
            var entityToCancel = Context.Entry(item);
            if (entityToCancel.State == EntityState.Modified)
            {
              entityToCancel.CurrentValues.SetValues(entityToCancel.OriginalValues);
              entityToCancel.State = EntityState.Unchanged;
            }

            return item;
        }

        partial void OnEmployeeUpdated(RadZenTutorial1.Models.BlazorEmployees.Employee item);
        partial void OnAfterEmployeeUpdated(RadZenTutorial1.Models.BlazorEmployees.Employee item);

        public async Task<RadZenTutorial1.Models.BlazorEmployees.Employee> UpdateEmployee(int employeeid, RadZenTutorial1.Models.BlazorEmployees.Employee employee)
        {
            OnEmployeeUpdated(employee);

            var itemToUpdate = Context.Employees
                              .Where(i => i.EmployeeId == employee.EmployeeId)
                              .FirstOrDefault();

            if (itemToUpdate == null)
            {
               throw new Exception("Item no longer available");
            }

            Reset();
            employee.Department = null;

            Context.Attach(employee).State = EntityState.Modified;

            Context.SaveChanges();

            OnAfterEmployeeUpdated(employee);

            return employee;
        }

        partial void OnEmployeeDeleted(RadZenTutorial1.Models.BlazorEmployees.Employee item);
        partial void OnAfterEmployeeDeleted(RadZenTutorial1.Models.BlazorEmployees.Employee item);

        public async Task<RadZenTutorial1.Models.BlazorEmployees.Employee> DeleteEmployee(int employeeid)
        {
            var itemToDelete = Context.Employees
                              .Where(i => i.EmployeeId == employeeid)
                              .FirstOrDefault();

            if (itemToDelete == null)
            {
               throw new Exception("Item no longer available");
            }

            OnEmployeeDeleted(itemToDelete);

            Reset();

            Context.Employees.Remove(itemToDelete);

            try
            {
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(itemToDelete).State = EntityState.Unchanged;
                throw;
            }

            OnAfterEmployeeDeleted(itemToDelete);

            return itemToDelete;
        }
        }
}