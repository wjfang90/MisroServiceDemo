using MicroService.Business;
using MicroService.Models;
using MicroServiceInstance.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MicroServiceInstance.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CheckController : ControllerBase
    {
        private IConfiguration _configuration;
        public CheckController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpGet]
        [Route("HealthCheck")]
        public OkResult HealthCheck()
        {
            Console.WriteLine($"health check {_configuration["ip"]}:{_configuration["port"]}");
            return Ok();
        }

    }
}
