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
    public partial class Departments
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

        protected IEnumerable<RadZenTutorial1.Models.BlazorEmployees.Department> departments;

        protected RadzenDataGrid<RadZenTutorial1.Models.BlazorEmployees.Department> grid0;

        protected string search = "";

        [Inject]
        protected SecurityService Security { get; set; }

        protected async Task Search(ChangeEventArgs args)
        {
            search = $"{args.Value}";

            await grid0.GoToPage(0);

            departments = await BlazorEmployeesService.GetDepartments(new Query { Filter = $@"i => i.DepartmentName.Contains(@0)", FilterParameters = new object[] { search } });
        }
        protected override async Task OnInitializedAsync()
        {
            departments = await BlazorEmployeesService.GetDepartments(new Query { Filter = $@"i => i.DepartmentName.Contains(@0)", FilterParameters = new object[] { search } });
        }

        protected async Task AddButtonClick(MouseEventArgs args)
        {
            await DialogService.OpenAsync<AddDepartment>("Add Department", null);
            await grid0.Reload();
        }

        protected async Task EditRow(RadZenTutorial1.Models.BlazorEmployees.Department args)
        {
            await DialogService.OpenAsync<EditDepartment>("Edit Department", new Dictionary<string, object> { {"DepartmentId", args.DepartmentId} });
        }

        protected async Task GridDeleteButtonClick(MouseEventArgs args, RadZenTutorial1.Models.BlazorEmployees.Department department)
        {
            try
            {
                if (await DialogService.Confirm("Are you sure you want to delete this record?") == true)
                {
                    var deleteResult = await BlazorEmployeesService.DeleteDepartment(department.DepartmentId);

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
                    Detail = $"Unable to delete Department" 
                });
            }
        }
    }
}