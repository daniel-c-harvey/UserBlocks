using DataAccess;
using DataBlocks.DataAccess;
using DataBlocks.DataAdapters;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using System.Net;
using System.Runtime.Intrinsics.X86;

namespace UserBlocks.Identity;

public static class Startup<TClient, TDatabase>
{
    private const string AUTH_COOKIE_NAME = ".TCBC.Auth";

    public static void ConfigureServerServices(IServiceCollection services, string connectionString, string databaseName)
    {
        IDataAccess<TDatabase> dataAccess = DataAccessFactory.Create<TClient, TDatabase>(connectionString, databaseName);
        IQueryBuilder<TDatabase> queryBuilder = QueryBuilderFactory.Create<TDatabase>();

        IDataAdapter<ApplicationUser> userAdapter =
            DataAdapterFactory.Create<TDatabase, ApplicationUser>(
                dataAccess,
                queryBuilder,
                DataSchema.Create<ApplicationUser>("users")
            );

        // IDataAdapter<ApplicationRole> roleAdapter = 
        services
            .AddSingleton(_ => userAdapter)
            // Add authentication
            .AddAuthentication(options =>
            {
                options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = CookieAuthenticationDefaults.AuthenticationScheme;
            })
            .AddCookie("Identity.Application", ConfigureAuthCookie);

        services
            // .AddScoped<AuthenticationStateProvider, CustomAuthStateProvider>()
            .AddIdentityCore<ApplicationUser>((options) =>
            {
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;
                options.Password.RequireLowercase = false;
                options.Password.RequireDigit = false;
            })
            // .AddRoles<ApplicationRole>()
            .AddUserStore<CustomUserStore>()
            // .AddRoleStore<CustomRoleStore>()
            .AddSignInManager()
            .AddDefaultTokenProviders();

   
            services.ConfigureApplicationCookie(ConfigureAuthCookie);

        services
            .AddAuthorizationCore();
    }

    private static void ConfigureAuthCookie(CookieAuthenticationOptions options)
    {
        options.Cookie.Name = AUTH_COOKIE_NAME;
        options.Cookie.HttpOnly = true;
        options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
        options.Cookie.SameSite = SameSiteMode.Strict;
        options.ExpireTimeSpan = TimeSpan.FromDays(30);
        options.SlidingExpiration = true;
        options.LoginPath = "/login";
        options.LogoutPath = "/logout";
    }

    public static void ConfigureClientServices(IServiceCollection services)
    {
        services
            .AddAuthorizationCore();
    }
}