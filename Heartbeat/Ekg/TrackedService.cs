using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace Heartbeat
{
    public class TrackedService
    {
        // Container id
        public string Id { get; set; }
        public DateTime LastHeartBeat { get; set; }

        // last successful
        public DateTime LastHealthCheck { get; set; }
        public DateTime LastCheckAttempt { get; set; }
        public DateTime Discovered { get; set; }
        
        public string Hostname { get; set; }
        public int? Port { get; set; }
        public string ServiceName { get; set; }
        public ServiceStatus Status { get; set; }
        
        // Current
        public DateTime FallingEdge { get; set; }
        public TimeSpan Downtime { get; set; }
        
        // Aggregate of all downtime
        public TimeSpan TotalDowntime { get; set; }

        public async Task CheckHealth()
        {
            var client = new HttpClient();

            var baseUri = new UriBuilder(Hostname);

            if (Port.HasValue) baseUri.Port = Port.Value;

            var hcUri = new Uri(baseUri.Uri, "hc");

            try
            {
                var result = await client.GetAsync(hcUri);

                if (result.IsSuccessStatusCode)
                {
                    LastHealthCheck = DateTime.Now;
                    Downtime = new TimeSpan();
                    FallingEdge = new DateTime();
                    
                    var status = await result.Content.ReadAsStringAsync();

                    Status = status == "Healthy" ? ServiceStatus.Healthy : ServiceStatus.UnHealthy;
                }
                else
                {
                    ServiceNotFound();
                }
            }
            catch
            {
                Console.WriteLine($"Health check failed {Hostname}");
                ServiceNotFound();
            }

            LastCheckAttempt = DateTime.Now;
        }

        private void ServiceNotFound()
        {
            Console.WriteLine($"{Hostname} Can't be found");
            if (Status == ServiceStatus.NotFound)
            {
                Downtime = Downtime.Add(DateTime.Now - LastCheckAttempt);
                TotalDowntime = TotalDowntime.Add(Downtime);
            }
            else
            {
                Status = ServiceStatus.NotFound;
                FallingEdge = DateTime.Now;
            }
        }
    }

    public enum ServiceStatus
    {
        Healthy,
        UnHealthy,
        NotFound
    }
}