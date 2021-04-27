using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Heartbeat
{
    [ApiController]
    [Route("[controller]")]
    public class EkgController : ControllerBase
    {
        private readonly ILogger<EkgController> Logger;
        private readonly ServiceTracking ServiceTracking;

        public EkgController(ILogger<EkgController> logger, ServiceTracking serviceTracking)
        {
            Logger = logger;
            ServiceTracking = serviceTracking;
        }

        [HttpGet("heartbeat")]
        public ActionResult Heartbeat(string serviceName)
        {
            Logger.LogInformation($"Got heartbeat pulse {serviceName}");
            Logger.LogInformation($"host -> {string.Join(",", Request.Host.Host)}");
            Logger.LogInformation($"host -> {string.Join(",", Request.Headers["ContainerId"].ToString())}");

            ServiceTracking.Add(Request.Headers["ContainerId"].ToString(), Request.Host.Host, Request.Host.Port, ServiceStatus.Healthy, serviceName);

            return Ok();
        }

        [HttpGet("status")]
        public List<TrackedService> Status()
        {
            return ServiceTracking.TrackedServices;
        }
    }
}