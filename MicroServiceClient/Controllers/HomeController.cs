using MicroService.Framework.Consul;
using MicroService.Framework.Utility;
using MicroService.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using MisroServiceClient.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace MisroServiceClient.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private IUtility _iutility;
        private IConfiguration _iConfiguration;

        public HomeController(ILogger<HomeController> logger,IUtility iutility, IConfiguration iConfiguration)
        {
            _logger = logger;
            _iutility = iutility;
            _iConfiguration = iConfiguration;
        }

        public IActionResult Index()
        {
            #region 直接调用MicroService intstance
            //var url = "http://localhost:7000/api/user/users";
            #endregion

            #region  nginx 集群配置方式
            //var url = "http://localhost:7777/api/user/users";
            #endregion

            #region Consul 配置MicroServer 方式

            //var consulName = _iConfiguration.GetSection("Consul:Name").Value;
            //var requestUrl = $"http://{consulName}/api/user/users";

            //var url = ConsulExtend.GetMicroServerUrlByConsul(requestUrl, ConsulLoadBanlaceType.Random);

            #endregion

            #region ocelot+consul 配置API Gateway方式

            var gatewayUrl = _iConfiguration.GetSection("ApiGatewayUrl").Value;

            var url = $"{gatewayUrl}/user/users";
            #endregion

            var content = _iutility.GetResponse(url);
            var usersResponse = Newtonsoft.Json.JsonConvert.DeserializeObject<dynamic>(content);
            var userViewModel = new UserViewModel();
            userViewModel.IP = usersResponse.IP;
            userViewModel.Port = usersResponse.Port;
            userViewModel.UserList = Newtonsoft.Json.JsonConvert.DeserializeObject<List<UserModel>>(Newtonsoft.Json.JsonConvert.SerializeObject(usersResponse.Data));
            return View(userViewModel);
        }

        public IActionResult Privacy()
        {
            #region 直接调用MicroService intstance
            //var url = "http://localhost:7000/api/user/userinfo";
            #endregion

            #region  nginx 集群配置方式
            //var url = "http://localhost:7777/api/user/userinfo";
            #endregion

            #region Consul 配置MicroServer 方式

            //var consulName = _iConfiguration.GetSection("Consul:Name").Value;
            //var requestUrl = $"http://{consulName}/api/user/userinfo";

            //var url = ConsulExtend.GetMicroServerUrlByConsul(requestUrl, ConsulLoadBanlaceType.Weight);

            #endregion

            #region ocelot+consul 配置API Gateway方式

            var gatewayUrl = _iConfiguration.GetSection("ApiGatewayUrl").Value;

            var url = $"{gatewayUrl}/user/userinfo";
            #endregion

            var content = _iutility.GetResponse(url);
            var userInfoResponse = Newtonsoft.Json.JsonConvert.DeserializeObject<dynamic>(content);
            var userViewModel = new UserViewModel();
            userViewModel.IP = userInfoResponse.IP;
            userViewModel.Port = userInfoResponse.Port;
            userViewModel.UserInfo = Newtonsoft.Json.JsonConvert.DeserializeObject<UserModel>(Newtonsoft.Json.JsonConvert.SerializeObject(userInfoResponse.Data));
            return View(userViewModel);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
