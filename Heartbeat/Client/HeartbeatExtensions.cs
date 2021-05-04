using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace Heartbeat.Client
{
    public static class HeartbeatExtensions
    {
        public static HeartbeatBuilder AddHeartbeat(this IServiceCollection services, string ekgHost, string serviceHostName, string serviceName)
            => new HeartbeatBuilder(services, ekgHost, serviceHostName, serviceName);

        public static UsingHeartbeatBuilder UseHeartbeat(this IApplicationBuilder app) => new UsingHeartbeatBuilder(app);
    }

    public class HeartbeatBuilder
    {
        public HeartbeatBuilder(IServiceCollection services, string ekgHost, string serviceHostName, string serviceName)
        {
            services.AddHostedService(provider => new HeartbeatService(ekgHost, serviceHostName, serviceName));
            services.AddHealthChecks();
        }
    }

    public class UsingHeartbeatBuilder
    {
        public UsingHeartbeatBuilder(IApplicationBuilder app)
        {
            // app.UseEndpoints(endpoints =>
            // {
            //     endpoints.MapHealthChecks("/hc");
            // });
        }
    }
}