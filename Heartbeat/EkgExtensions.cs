using Microsoft.Extensions.DependencyInjection;

namespace Heartbeat
{
    public static class EkgExtensions
    {
        public static EkgBuilder AddEkg(this IServiceCollection services)
            => new EkgBuilder(services);
    }
    
    public class EkgBuilder
    {
        public EkgBuilder(IServiceCollection services)
        {
            services.AddSingleton<ServiceTracking>();
            services.AddHostedService<ServiceTrackingWatchDog>();
            services.AddMvcCore().AddControllersAsServices();
        }
    }
}