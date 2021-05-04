using System;
using System.Collections.Generic;
using Heartbeat.Hubs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;

namespace Heartbeat
{
    public class ServiceTracking
    {
        private readonly ILogger<ServiceTracking> Logger;
        private readonly List<TrackedService> TrackedServices = new();
        private readonly IHubContext<EkgHub> EkgHub;

        public ServiceTracking(ILogger<ServiceTracking> logger, IHubContext<EkgHub> ekgHub)
        {
            Logger = logger;
            EkgHub = ekgHub;
        }

        public void Add(string containerId, string hostname, int? hostPort, ServiceStatus status, string name)
        {
            Logger.LogInformation($"total tracked services -> {TrackedServices.Count}");
            var service = TrackedServices.Find(x => x.Id == containerId);

            if (service != null)
            {
                Logger.LogInformation("updating service already being tracked");

                service.LastHeartBeat = DateTime.Now;
                service.Status = status;

                return;
            }

            Logger.LogInformation("adding new service to tracking");

            var trackedService = new TrackedService()
            {
                Id = containerId,
                Discovered = DateTime.Now,
                Hostname = hostname,
                Port = hostPort,
                Status = status,
                LastHeartBeat = DateTime.Now,
                ServiceName = name
            };

            TrackedServices.Add(trackedService);
        }

        public List<TrackedService> Services()
        {
            return TrackedServices;
        }

        public TrackedService Service(string id)
        {
            return TrackedServices.Find(x => x.Id == id);
        }
    }
}