using DataAccess;
using DataBlocks.DataAccess;
using DataBlocks.DataAdapters;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

namespace UserBlocks.Identity;

public static class Startup<TClient, TDatabase>
{
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
            .AddSingleton<IDataAdapter<ApplicationUser>>(_ => userAdapter)
            .AddAuthentication(options => {
                options.DefaultScheme = IdentityConstants.ApplicationScheme;
            })
            .AddCookie(IdentityConstants.ApplicationScheme, options => {
                options.LoginPath = "/login";
                options.LogoutPath = "/logout";
            });
        services
            // .AddScoped<AuthenticationStateProvider, CustomAuthStateProvider>()
            .AddIdentityCore<ApplicationUser>()
            // .AddRoles<ApplicationRole>()
            .AddUserStore<CustomUserStore>()
            // .AddRoleStore<CustomRoleStore>()
            .AddSignInManager()
            .AddDefaultTokenProviders();

        services
            .AddAuthorizationCore();
    }

    public static void ConfigureClientServices(IServiceCollection services)
    {
        services
            .AddAuthorizationCore();
    }
}