using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace RadZenTutorial1.Data.Migrations
{
    /// <inheritdoc />
    public partial class dev : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "5c1f32d2-c437-4643-9f93-a50b07f9b404", null, "Admin", "ADMIN" },
                    { "f3432f9f-2646-4502-82bf-53d1f72ca35b", null, "User", "USER" }
                });

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "Email", "EmailConfirmed", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[,]
                {
                    { "3f0cee53-de1b-49fc-97df-de3b8c1426ea", 0, "e3afc51d-9f3a-4650-a83c-fa5b94373d4a", "admin@example.com", true, false, null, "ADMIN@EXAMPLE.COM", "ADMIN", "AQAAAAIAAYagAAAAEG00dP9l7Be9NLhpSe/BxW26Z6UzZomJXGkQBiU3NhknOVqDEy2QhSd86OaZicZDvw==", null, false, "75054b19-d4cf-4c82-bace-eeba6e22f641", false, "admin" },
                    { "5cf91221-1512-4e09-93e9-a3ff76638856", 0, "6c305ec6-c297-4d96-a553-f31893ea764b", "user@example.com", true, false, null, "USER@EXAMPLE.COM", "USER", "AQAAAAIAAYagAAAAEN7DBNt4SXwNEDK9/b5aF9xjWvO8vR4FMQZeXERJIeyy3vyyC/DDe0SCHMXVyJkKug==", null, false, "6de2993a-c5fc-4c59-9a94-a768b34e56a3", false, "user" }
                });

            migrationBuilder.InsertData(
                table: "AspNetUserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[,]
                {
                    { "5c1f32d2-c437-4643-9f93-a50b07f9b404", "3f0cee53-de1b-49fc-97df-de3b8c1426ea" },
                    { "f3432f9f-2646-4502-82bf-53d1f72ca35b", "5cf91221-1512-4e09-93e9-a3ff76638856" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { "5c1f32d2-c437-4643-9f93-a50b07f9b404", "3f0cee53-de1b-49fc-97df-de3b8c1426ea" });

            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { "f3432f9f-2646-4502-82bf-53d1f72ca35b", "5cf91221-1512-4e09-93e9-a3ff76638856" });

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "5c1f32d2-c437-4643-9f93-a50b07f9b404");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "f3432f9f-2646-4502-82bf-53d1f72ca35b");

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "3f0cee53-de1b-49fc-97df-de3b8c1426ea");

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "5cf91221-1512-4e09-93e9-a3ff76638856");
        }
    }
}
