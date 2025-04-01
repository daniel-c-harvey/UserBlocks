using Microsoft.Extensions.DependencyInjection;

namespace UserBlocksWeb.Client;

public static class Startup
{
    public static void ConfigureServices(IServiceCollection services)
    {
        services
            .AddAuthorizationCore()
            .AddCascadingAuthenticationState()
            .AddAuthenticationStateDeserialization();
    }
}