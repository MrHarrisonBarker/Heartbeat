using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace Heartbeat
{
    public class ServiceTracking
    {
        private readonly ILogger<ServiceTracking> Logger;
        public List<TrackedService> TrackedServices { get; }

        public ServiceTracking(ILogger<ServiceTracking> logger)
        {
            Logger = logger;
            TrackedServices = new List<TrackedService>();
        }

        public void Add(string containerId, string hostname, int? hostPort, ServiceStatus status, string name)
        {
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
    }
}