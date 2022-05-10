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
    public class UserController : ControllerBase
    {
        private IUserService _userService;
        private IConfiguration _configuration;
        public UserController(IConfiguration configuration, IUserService userService)
        {
            _configuration = configuration;
            _userService = userService;
        }
        [HttpGet]
        [Route("users")]
        public async Task<JsonResult> GetUsers()
        {
            var userList = await _userService.GetList();
            var ip = _configuration["ip"] ?? string.Empty;
            var port = _configuration["port"] ?? string.Empty;

            var data = new ResponseModel { Data = userList, IP = ip, Port = port };
            
            return new JsonResult(data);
        }

        [HttpGet]
        [Route("userInfo")]
        public async Task<JsonResult> Get(string id)
        {
            var ip = _configuration["ip"] ?? string.Empty;
            var port = _configuration["port"] ?? string.Empty;
            var userInfo = await _userService.Get(id);
            var data = new ResponseModel { Data = userInfo, IP = ip, Port = port };

            return new JsonResult(data);
        }

        [HttpGet]
        [Route("timeout")]
        public JsonResult Timeout()
        {
            var ip = _configuration["ip"] ?? string.Empty;
            var port = _configuration["port"] ?? string.Empty;
            System.Threading.Thread.Sleep(5000);
            
            Console.WriteLine($"This is User.Timeout Invoke Timeout {ip}:{port}");

            return new JsonResult(new { IP = ip, Port = port });
        }
    }
}
