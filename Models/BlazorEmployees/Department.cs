using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RadZenTutorial1.Models.BlazorEmployees
{
    [Table("Departments", Schema = "dbo")]
    public partial class Department
    {
        [Key]
        [Required]
        public int DepartmentId { get; set; }

        public string DepartmentName { get; set; }

        public ICollection<Employee> Employees { get; set; }

    }
}