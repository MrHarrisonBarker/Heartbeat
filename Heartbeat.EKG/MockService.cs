using System;
using System.Collections.Generic;

namespace Heartbeat.EKG
{
    public class MockService
    {
        public List<string> Strings = new List<string>()
        {
            "Hello", "World ", "HelloWorld"
        };

        public List<TrackedService> Services = new List<TrackedService>()
        {
            new TrackedService()
            {
                Discovered = DateTime.Now,
                Downtime = TimeSpan.Zero,
                Hostname = "mockservice",
                Id = "11111111",
                Port = 5102,
                Status = ServiceStatus.Healthy,
                ServiceName = "MockService",
                TotalDowntime = TimeSpan.Zero,
                LastCheckAttempt = DateTime.Now,
                LastHealthCheck = DateTime.Now,
                LastHeartBeat = DateTime.Now
            }
        };

        public TrackedService Service(string id)
        {
            return Services.Find(x => x.Id == id);
        }
    }
}