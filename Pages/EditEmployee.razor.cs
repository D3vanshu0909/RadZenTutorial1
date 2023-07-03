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
    public partial class EditEmployee
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

        [Parameter]
        public int EmployeeId { get; set; }

        protected override async Task OnInitializedAsync()
        {
            employee = await BlazorEmployeesService.GetEmployeeByEmployeeId(EmployeeId);

            departmentsForDepartmentId = await BlazorEmployeesService.GetDepartments();
        }
        protected bool errorVisible;
        protected RadZenTutorial1.Models.BlazorEmployees.Employee employee;

        protected IEnumerable<RadZenTutorial1.Models.BlazorEmployees.Department> departmentsForDepartmentId;

        [Inject]
        protected SecurityService Security { get; set; }

        protected async Task FormSubmit()
        {
            try
            {
                await BlazorEmployeesService.UpdateEmployee(EmployeeId, employee);
                DialogService.Close(employee);
            }
            catch (Exception ex)
            {
                errorVisible = true;
            }
        }

        protected async Task CancelButtonClick(MouseEventArgs args)
        {
            DialogService.Close(null);
        }
    }
}