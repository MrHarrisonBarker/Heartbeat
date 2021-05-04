#nullable enable
using System;
using System.Threading;
using System.Threading.Tasks;
using Heartbeat.Hubs;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Heartbeat
{
    public class ServiceTrackingWatchDog : IHostedService, IDisposable
    {
        private readonly ILogger<ServiceTrackingWatchDog> Logger;
        private readonly ServiceTracking ServiceTracking;
        private readonly IHubContext<EkgHub> EkgHub;

        private Timer? Timer;
        private readonly TimeSpan CheckInterval = TimeSpan.FromSeconds(10);

        public ServiceTrackingWatchDog(ILogger<ServiceTrackingWatchDog> logger, ServiceTracking serviceTracking, IHubContext<EkgHub> ekgHub)
        {
            ServiceTracking = serviceTracking;
            EkgHub = ekgHub;
            Logger = logger;
        }

        private Task Process()
        {
            Logger.LogInformation("Checking tracked services");

            ServiceTracking.Services().ForEach(async service =>
            {
                await service.CheckHealth();
                await EkgHub.Clients.All.SendAsync("pulse", service);
            });

            return Task.CompletedTask;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            Logger.LogInformation("Starting service tracking watch dog");
            Timer = new Timer(Process, null, TimeSpan.Zero, CheckInterval);
            return Task.CompletedTask;
        }

        private void Process(object? state)
        {
            Process();
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            Logger.LogInformation("Stopping service tracking watch dog");
            Timer?.Change(Timeout.Infinite, 0);
            return Task.CompletedTask;
        }

        public void Dispose()
        {
            Timer?.Dispose();
        }
    }
}