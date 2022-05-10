using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MicroService.IdentityServer4Test.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TestController : ControllerBase
    {
        [HttpGet]
        [Route("Index")]
        public IActionResult Index()
        {
            Console.WriteLine($"{nameof(TestController)}.{nameof(TestController.Index)}");
            return new JsonResult(new { Id = 1, Name = "fang 测试", CreateTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff") });
        }

        [Authorize]
        [HttpGet]
        [Route("AuthenticateTest")]
        public IActionResult AuthenticateTest()
        {
            Console.WriteLine($"{nameof(TestController)}.{nameof(TestController.AuthenticateTest)}");
            return new JsonResult(new { Id = 1, Name = "fang 测试", CreateTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff") });
        }
    }
}
