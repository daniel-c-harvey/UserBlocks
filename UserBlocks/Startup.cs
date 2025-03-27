using DataBlocks.DataAccess;
using DataBlocks.DataAdapters;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using UserBlocks.Components.Account;
using UserBlocks.Identity;

namespace UserBlocks;

public static class Startup<TClient, TDatabase>
{
    private const string AUTH_COOKIE_NAME = ".TCBC.Auth";

    public static void ConfigureServices(IServiceCollection services, string connectionString, string databaseName)
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
            .AddCascadingAuthenticationState()
            .AddScoped<IdentityUserAccessor>()
            .AddScoped<IdentityRedirectManager>()
            .AddScoped<AuthenticationStateProvider, IdentityRevalidatingAuthenticationStateProvider>()
            .AddSingleton(_ => userAdapter)
            // Add authentication
            .AddAuthentication(options =>
            {
                options.DefaultScheme = IdentityConstants.ApplicationScheme;
                options.DefaultSignInScheme = IdentityConstants.ExternalScheme;
            })
            .AddIdentityCookies();

        services
            // .AddScoped<AuthenticationStateProvider, CustomAuthStateProvider>()
            .AddIdentityCore<ApplicationUser>((options) =>
            {
                options.SignIn.RequireConfirmedAccount = true;
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
            services.AddSingleton<IEmailSender<ApplicationUser>, IdentityNoOpEmailSender>();

        services
            .AddAuthorizationCore();
    }

    private static void ConfigureAuthCookie(CookieAuthenticationOptions options)
    {
        // options.Cookie.Name = AUTH_COOKIE_NAME;
        // options.Cookie.HttpOnly = true;
        // options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
        // options.Cookie.SameSite = SameSiteMode.Strict;
        options.ExpireTimeSpan = TimeSpan.FromDays(30);
        options.SlidingExpiration = true;
        // options.LoginPath = "/login";
        // options.LogoutPath = "/logout";
    }
}