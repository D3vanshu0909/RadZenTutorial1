using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RadZenTutorial1.Models.BlazorEmployees
{
    [Table("Employees", Schema = "dbo")]
    public partial class Employee
    {
        [Key]
        [Required]
        public int EmployeeId { get; set; }

        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        public string Email { get; set; }

        [Column(TypeName="datetime2")]
        [Required]
        public DateTime DateOfBirth { get; set; }

        [Required]
        public int Gender { get; set; }

        [Required]
        public int DepartmentId { get; set; }

        public string PhotoPath { get; set; }

        public Department Department { get; set; }

    }
}