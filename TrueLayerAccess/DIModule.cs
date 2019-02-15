using Microsoft.Extensions.DependencyInjection;

namespace TrueLayerAccess
{
    public static class DIModule
    {
        public static void Add(IServiceCollection services)
        {
            services.AddSingleton<ITrueLayerApi, TrueLayerApi>();
            services.AddSingleton<ITrueLayerAuthApi, TrueLayerAuthApi>();
        }
    }
}