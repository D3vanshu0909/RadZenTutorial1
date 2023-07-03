using System;
using System.Linq;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

using RadZenTutorial1.Models.BlazorEmployees;

namespace RadZenTutorial1.Data
{
    public partial class BlazorEmployeesContext : DbContext
    {
        public BlazorEmployeesContext()
        {
        }

        public BlazorEmployeesContext(DbContextOptions<BlazorEmployeesContext> options) : base(options)
        {
        }

        partial void OnModelBuilding(ModelBuilder builder);

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<RadZenTutorial1.Models.BlazorEmployees.Employee>()
              .HasOne(i => i.Department)
              .WithMany(i => i.Employees)
              .HasForeignKey(i => i.DepartmentId)
              .HasPrincipalKey(i => i.DepartmentId);

            builder.Entity<RadZenTutorial1.Models.BlazorEmployees.Employee>()
              .Property(p => p.DateOfBirth)
              .HasColumnType("datetime2");
            this.OnModelBuilding(builder);
        }

        public DbSet<RadZenTutorial1.Models.BlazorEmployees.Department> Departments { get; set; }

        public DbSet<RadZenTutorial1.Models.BlazorEmployees.Employee> Employees { get; set; }

      


    
    }
}