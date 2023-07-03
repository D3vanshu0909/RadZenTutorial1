using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;

using RadZenTutorial1.Models;

namespace RadZenTutorial1.Data
{
    public partial class ApplicationIdentityDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, string>
    {
        public ApplicationIdentityDbContext(DbContextOptions<ApplicationIdentityDbContext> options) : base(options)
        {
        }

        public ApplicationIdentityDbContext()
        {
        }

        partial void OnModelBuilding(ModelBuilder builder);

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<ApplicationUser>()
                   .HasMany(u => u.Roles)
                   .WithMany(r => r.Users)
                   .UsingEntity<IdentityUserRole<string>>();

            SeedRoles(builder); // Call the method to seed roles and users

            this.OnModelBuilding(builder);
        }

        private void SeedRoles(ModelBuilder builder)
        {
            // Seed an admin role
            var adminRoleId = Guid.NewGuid().ToString();
            builder.Entity<ApplicationRole>().HasData(new ApplicationRole { Id = adminRoleId, Name = "Admin", NormalizedName = "ADMIN" });

            // Seed a user role
            var userRoleId = Guid.NewGuid().ToString();
            builder.Entity<ApplicationRole>().HasData(new ApplicationRole { Id = userRoleId, Name = "User", NormalizedName = "USER" });

            // Seed an admin user with password
            var adminUserId = Guid.NewGuid().ToString();
            var adminUser = new ApplicationUser
            {
                Id = adminUserId,
                UserName = "admin",
                NormalizedUserName = "ADMIN",
                Email = "admin@example.com",
                NormalizedEmail = "ADMIN@EXAMPLE.COM",
                EmailConfirmed = true,
                PasswordHash = new PasswordHasher<ApplicationUser>().HashPassword(null, "adminpassword")
            };
            builder.Entity<ApplicationUser>().HasData(adminUser);

            // Assign the admin user to the admin role
            builder.Entity<IdentityUserRole<string>>().HasData(new IdentityUserRole<string> { UserId = adminUserId, RoleId = adminRoleId });

            // Seed a regular user with password
            var regularUserId = Guid.NewGuid().ToString();
            var regularUser = new ApplicationUser
            {
                Id = regularUserId,
                UserName = "user",
                NormalizedUserName = "USER",
                Email = "user@example.com",
                NormalizedEmail = "USER@EXAMPLE.COM",
                EmailConfirmed = true,
                PasswordHash = new PasswordHasher<ApplicationUser>().HashPassword(null, "userpassword")
            };
            builder.Entity<ApplicationUser>().HasData(regularUser);

            // Assign the regular user to the user role
            builder.Entity<IdentityUserRole<string>>().HasData(new IdentityUserRole<string> { UserId = regularUserId, RoleId = userRoleId });
        }
    }
}
