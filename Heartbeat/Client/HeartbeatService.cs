using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Hosting;

namespace Heartbeat.Client
{
    public interface IHeartbeatService : IHostedService, IDisposable
    {
    }

    public class HeartbeatService : IHeartbeatService
    {
        private readonly Uri HeartbeatUri;
        private readonly string ServiceName;

        private readonly HttpClient HttpClient;
        private Timer Timer;

        public HeartbeatService(string ekgHost, string serviceHostName, string serviceName)
        {
            HttpClient = new HttpClient();
            HttpClient.DefaultRequestHeaders.Host = serviceHostName;
            HttpClient.DefaultRequestHeaders.Add("ContainerId", Dns.GetHostName());

            ServiceName = serviceName;

            var baseUri = new Uri(new UriBuilder(ekgHost).Uri, "ekg/heartbeat");
            var uriBuilder = new UriBuilder(baseUri) {Query = $"serviceName={serviceName}"};
            HeartbeatUri = uriBuilder.Uri;
        }

        private void SendPulse(object state)
        {
            Console.WriteLine("Sending heartbeat pulse");

            foreach (var header in HttpClient.DefaultRequestHeaders)
            {
                Console.WriteLine($"header {header.Key} -> {string.Join(",", header.Value)}");
            }

            var message = new HttpRequestMessage(HttpMethod.Get, HeartbeatUri);

            try
            {
                HttpClient.Send(message);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            Console.WriteLine("Sending heartbeat service");
            Timer = new Timer(SendPulse, null, TimeSpan.Zero, TimeSpan.FromSeconds(5));
            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            Timer?.Change(Timeout.Infinite, 0);
            return Task.CompletedTask;
        }

        public void Dispose()
        {
            HttpClient.Dispose();
            Timer.Dispose();
        }
    }
}