using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Radzen;
using Microsoft.EntityFrameworkCore;
using RadZenTutorial1.Data;
using Microsoft.AspNetCore.Identity;
using RadZenTutorial1.Models;
using Microsoft.AspNetCore.OData;
using Microsoft.OData.ModelBuilder;
using Microsoft.AspNetCore.Components.Authorization;

var builder = WebApplication.CreateBuilder(args);
// Create a new WebApplication using the CreateBuilder method

// Add services to the container
builder.Services.AddRazorPages(); // Adds support for Razor Pages
builder.Services.AddServerSideBlazor().AddHubOptions(o =>
{
    o.MaximumReceiveMessageSize = 10 * 1024 * 1024;
}); // Adds support for Blazor Server-Side and configures SignalR hub options

builder.Services.AddScoped<DialogService>(); // Registers the DialogService for displaying dialog boxes
builder.Services.AddScoped<NotificationService>(); // Registers the NotificationService for displaying notifications
builder.Services.AddScoped<TooltipService>(); // Registers the TooltipService for displaying tooltips
builder.Services.AddScoped<ContextMenuService>(); // Registers the ContextMenuService for displaying context menus

builder.Services.AddScoped<RadZenTutorial1.BlazorEmployeesService>(); // Registers the BlazorEmployeesService for interacting with the BlazorEmployeesContext

builder.Services.AddDbContext<RadZenTutorial1.Data.BlazorEmployeesContext>(options =>
{
    options.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
    options.UseSqlServer(builder.Configuration.GetConnectionString("BlazorEmployeesConnection"));
});
// Adds the BlazorEmployeesContext as a database context using Entity Framework Core

builder.Services.AddHttpClient("RadZenTutorial1").AddHeaderPropagation(o => o.Headers.Add("Cookie"));
builder.Services.AddHeaderPropagation(o => o.Headers.Add("Cookie"));
// Configures the HTTP client to propagate the "Cookie" header when making requests

builder.Services.AddAuthentication(); // Configures the authentication services
builder.Services.AddAuthorization(); // Configures the authorization services

builder.Services.AddScoped<RadZenTutorial1.SecurityService>(); // Registers the SecurityService for implementing security-related functionality

builder.Services.AddDbContext<ApplicationIdentityDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("BlazorEmployeesConnection"));
});
// Adds the ApplicationIdentityDbContext as a database context for identity-related functionality

builder.Services.AddIdentity<ApplicationUser, ApplicationRole>().AddEntityFrameworkStores<ApplicationIdentityDbContext>().AddDefaultTokenProviders();
// Configures the Identity services to use ApplicationUser as the user model, ApplicationRole as the role model,
// and sets up the database context and default token providers

builder.Services.AddControllers().AddOData(o =>
{
    var oDataBuilder = new ODataConventionModelBuilder();
    oDataBuilder.EntitySet<ApplicationUser>("ApplicationUsers"); // Adds ApplicationUser as an OData entity set
    var usersType = oDataBuilder.StructuralTypes.First(x => x.ClrType == typeof(ApplicationUser));
    usersType.AddProperty(typeof(ApplicationUser).GetProperty(nameof(ApplicationUser.Password))); // Adds a property to ApplicationUser entity
    usersType.AddProperty(typeof(ApplicationUser).GetProperty(nameof(ApplicationUser.ConfirmPassword))); // Adds a property to ApplicationUser entity
    oDataBuilder.EntitySet<ApplicationRole>("ApplicationRoles"); // Adds ApplicationRole as an OData entity set
    o.AddRouteComponents("odata/Identity", oDataBuilder.GetEdmModel())
        .Count().Filter().OrderBy().Expand().Select().SetMaxTop(null).TimeZone = TimeZoneInfo.Utc;
});
// Configures OData support, adds entity sets, includes additional properties, and sets OData query options

builder.Services.AddScoped<AuthenticationStateProvider, RadZenTutorial1.ApplicationAuthenticationStateProvider>();
// Registers the ApplicationAuthenticationStateProvider for managing the authentication state in a Blazor application

var app = builder.Build();
// Builds the WebApplication using the configured services

// Configure the HTTP request pipeline
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error"); // Handles exceptions and redirects to the "/Error" page
    app.UseHsts(); // Enables HTTP Strict Transport Security (HSTS)
}

app.UseHttpsRedirection(); // Redirects HTTP requests to HTTPS
app.UseStaticFiles(); // Serves static files
app.UseHeaderPropagation(); // Propagates headers in the HTTP request
app.UseRouting(); // Enables routing
app.UseAuthentication(); // Authenticates the request
app.UseAuthorization(); // Authorizes the request
app.MapControllers(); // Maps controllers
app.MapBlazorHub(); // Maps the Blazor hub
app.MapFallbackToPage("/_Host"); // Sets the fallback route to the Blazor "_Host.cshtml" page

app.Services.CreateScope().ServiceProvider.GetRequiredService<ApplicationIdentityDbContext>().Database.Migrate();
// Creates a scope for the application services and applies any pending migrations to the database

app.Run(); // Starts the application and listens for incoming requests
