using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.JSInterop;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Radzen;
using Radzen.Blazor;

namespace RadZenTutorial1.Pages
{
    public partial class Employees
    {
        [Inject]
        protected IJSRuntime JSRuntime { get; set; }

        [Inject]
        protected NavigationManager NavigationManager { get; set; }

        [Inject]
        protected DialogService DialogService { get; set; }

        [Inject]
        protected TooltipService TooltipService { get; set; }

        [Inject]
        protected ContextMenuService ContextMenuService { get; set; }

        [Inject]
        protected NotificationService NotificationService { get; set; }

        [Inject]
        public BlazorEmployeesService BlazorEmployeesService { get; set; }

        protected IEnumerable<RadZenTutorial1.Models.BlazorEmployees.Employee> employees;

        protected RadzenDataGrid<RadZenTutorial1.Models.BlazorEmployees.Employee> grid0;

        protected string search = "";

        [Inject]
        protected SecurityService Security { get; set; }

        protected async Task Search(ChangeEventArgs args)
        {
            search = $"{args.Value}";

            await grid0.GoToPage(0);

            employees = await BlazorEmployeesService.GetEmployees(new Query { Filter = $@"i => i.FirstName.Contains(@0) || i.LastName.Contains(@0) || i.Email.Contains(@0)", FilterParameters = new object[] { search }, Expand = "Department" });
        }
        protected override async Task OnInitializedAsync()
        {
            employees = await BlazorEmployeesService.GetEmployees(new Query { Filter = $@"i => i.FirstName.Contains(@0) || i.LastName.Contains(@0) || i.Email.Contains(@0)", FilterParameters = new object[] { search }, Expand = "Department" });
        }

        protected async Task AddButtonClick(MouseEventArgs args)
        {
            await DialogService.OpenAsync<AddEmployee>("Add Employee", null);
            await grid0.Reload();
        }

        protected async Task EditRow(RadZenTutorial1.Models.BlazorEmployees.Employee args)
        {
            await DialogService.OpenAsync<EditEmployee>("Edit Employee", new Dictionary<string, object> { {"EmployeeId", args.EmployeeId} });
        }

        protected async Task GridDeleteButtonClick(MouseEventArgs args, RadZenTutorial1.Models.BlazorEmployees.Employee employee)
        {
            try
            {
                if (await DialogService.Confirm("Are you sure you want to delete this record?") == true)
                {
                    var deleteResult = await BlazorEmployeesService.DeleteEmployee(employee.EmployeeId);

                    if (deleteResult != null)
                    {
                        await grid0.Reload();
                    }
                }
            }
            catch (Exception ex)
            {
                NotificationService.Notify(new NotificationMessage
                { 
                    Severity = NotificationSeverity.Error,
                    Summary = $"Error", 
                    Detail = $"Unable to delete Employee" 
                });
            }
        }
    }
}