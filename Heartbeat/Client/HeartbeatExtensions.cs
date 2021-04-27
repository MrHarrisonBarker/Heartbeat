using System;
using Microsoft.Extensions.DependencyInjection;

namespace Heartbeat.Client
{
    public static class HeartbeatExtensions
    {
        public static HeartbeatBuilder AddHeartbeat(this IServiceCollection services, string ekgHost, string serviceHostName, string serviceName)
            => new HeartbeatBuilder(services, ekgHost, serviceHostName, serviceName);
    }

    public class HeartbeatBuilder
    {
        public HeartbeatBuilder(IServiceCollection services, string ekgHost, string serviceHostName, string serviceName)
        {
            services.AddHostedService(provider => new HeartbeatService(ekgHost, serviceHostName, serviceName));
        }
    }
}