using IdentityManager.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using VikoServiceManager.Authorize;

namespace VikoServiceManager
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // DB setup
            var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
            builder.Services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(connectionString));

            /*            builder.Services.AddIdentity<IdentityUser, IdentityRole>(options => options.SignIn.RequireConfirmedAccount = true).AddEntityFrameworkStores<ApplicationDbContext>();*/
            builder.Services.AddIdentity<IdentityUser, IdentityRole>().AddEntityFrameworkStores<ApplicationDbContext>().AddDefaultTokenProviders().AddDefaultUI();
            builder.Services.Configure<IdentityOptions>(options =>
            {
                options.Password.RequiredLength = 5;
                options.Password.RequireLowercase = true;
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromSeconds(30);
                options.Lockout.MaxFailedAccessAttempts = 5;
            });
            // custom path if access is denied
            /*            builder.Services.ConfigureApplicationCookie(options =>
                        {
                            options.AccessDeniedPath = new Microsoft.AspNetCore.Http.PathString("/Home/AccessDenied");
                        });*/

            // policy configuration
            builder.Services.AddAuthorization(options =>
            {
                options.AddPolicy("Admin", policy => policy.RequireRole("Admin"));
                options.AddPolicy("UserAndAdmin", policy => policy.RequireRole("Admin").RequireRole("User"));
                options.AddPolicy("AdminCreateAccess", policy => policy.RequireRole("Admin").RequireClaim("create", "True"));
                options.AddPolicy("AdminCreateEditDeleteAccess", policy => policy.RequireRole("Admin")
                    .RequireClaim("create", "True")
                    .RequireClaim("edit", "True")
                    .RequireClaim("delete", "True"));

                // funcion with complex rules
                options.AddPolicy("AdminCreateEditDeleteAccessOrSuperAdmin", policy => policy.RequireAssertion(context => AuthorizeAdminWithClaimsOrSuperAdmin(context)));
                // custom policy handler from Authorize/OnlySuperAdminChecker.cs
                options.AddPolicy("OnlySuperAdminChecker", policy => policy.Requirements.Add(new OnlySuperAdminChecker()));
                options.AddPolicy("AdminWithMoreThanThousandDays", policy => policy.Requirements.Add(new AdminWithMoreThanThousandDaysRequirement(1000)));
            });

            builder.Services.AddScoped<IAuthorizationHandler, AdminWithThousandDaysHandler>();
            builder.Services.AddScoped<INumberOfDaysForAccount, NumberOfDaysForAccount>();

            // Add services to the container.
            builder.Services.AddControllersWithViews();
            builder.Services.AddRazorPages();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication(); // must be before Authorization

            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}",
                app.MapRazorPages()); // enables razor scaffolding




            app.Run();
        }

        // TODO: should be moved to seprate class for all the access functions
        private static bool AuthorizeAdminWithClaimsOrSuperAdmin(AuthorizationHandlerContext handlerContext)
        {
            return (
                handlerContext.User.IsInRole("Admin") && handlerContext.User.HasClaim(c => c.Type == "Create" && c.Value == "True")
                    && handlerContext.User.HasClaim(c => c.Type == "Edit" && c.Value == "True")
                    && handlerContext.User.HasClaim(c => c.Type == "Delete" && c.Value == "True")
                ) || handlerContext.User.IsInRole("SuperAdmin");
        }
    }
}