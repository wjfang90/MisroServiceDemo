using Consul;
using MicroService.Framework.Utility;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MicroService.Framework.Consul
{
    /// <summary>
    /// Consul 
    /// 优点 ：提供服务注册、服务发现、服务检查功能，服务异常自动下线，注册的服务自动上线功能
    /// 缺点 ：需要手动配置负载均衡策略
    /// </summary>
    public static class ConsulExtend
    {

        private static int ConsulRoundTurnIndex;

        /// <summary>
        /// 注册Consul服务
        /// </summary>
        /// <param name="configuration"></param>
        public static void ConsulRegist(this IConfiguration configuration)
        {
            //命令行参数  配置优先级: Kestrel方法 > 命令行参数方法 >  配置文件中urls的配置 > UseUrls方法 > 默认值
            var ip = configuration["ip"] ?? GetHostSettingHost(configuration);
            var port = configuration["port"] ?? GetHostSettingPort(configuration)?.ToString();
            var weight = configuration["weight"] ?? "1";

            var url = configuration.GetSection("Consul:Url").Value;
            var name = configuration.GetSection("Consul:Name").Value;
            var checkUrl = configuration.GetSection("Consul:Check:Url").Value;
            var checkTimeout = configuration.GetSection("Consul:Check:Timeout").Value;
            var checkTimeDelete = configuration.GetSection("Consul:Check:TimeDelete").Value;

            var consulClient = new ConsulClient(config =>
            {
                config.Address = new Uri(url);
                config.Datacenter = "DataCenter fang";
            });

            var consulName = ConfigurationExtend.GetConfigurationValue("appsettings.json", "Consul:Name");

            consulClient.Agent.ServiceRegister(new AgentServiceRegistration()
            {
                ID = $"{consulName}-{ip}:{port}", //服务唯一标志
                Name = name, //分组名称
                Address = ip,
                Port = int.Parse(port),
                Tags = new[] { weight },//标签
                Check = new AgentServiceCheck()
                {
                    Interval = TimeSpan.FromSeconds(10),
                    HTTP = string.Format(checkUrl, ip, port),
                    Timeout = TimeSpan.FromSeconds(double.Parse(checkTimeout)),//超时时间
                    DeregisterCriticalServiceAfter = TimeSpan.FromSeconds(double.Parse(checkTimeDelete))//失败后删除时间
                }
            });

            //命令行参数获取
            Console.WriteLine($"{ip}:{port}--weight:{weight}");
        }

        /// <summary>
        /// 调用Consul服务并根据配置负载均衡策略返回微服务地址
        /// </summary>
        /// <param name="requestConsulUrl"></param>
        /// <param name="consulLoadBanlaceType"></param>
        /// <returns></returns>
        public static string GetMicroServerUrlByConsul(string requestConsulUrl, ConsulLoadBanlaceType consulLoadBanlaceType = ConsulLoadBanlaceType.RoundTurn)
        {
            var consulUrl = ConfigurationExtend.GetConfigurationValue("appsettings.json", "Consul:Url");
            var consulName = ConfigurationExtend.GetConfigurationValue("appsettings.json", "Consul:Name");

            var consulClient = new ConsulClient(config =>
            {
                config.Address = new Uri(consulUrl);
                config.Datacenter = consulName;
            });

            var responseDict = consulClient.Agent.Services().Result.Response;

            var agentServerDict = responseDict.Where(t => t.Value.Service.SequenceEqual(consulName)).ToArray();

            //consul 需要手动写负载均衡策略
            AgentService agentServer;
            switch (consulLoadBanlaceType)
            {
                default:
                case ConsulLoadBanlaceType.RoundTurn:
                    {
                        agentServer = agentServerDict[ConsulRoundTurnIndex++ % agentServerDict.Length].Value;
                        break;
                    }
                case ConsulLoadBanlaceType.Random:
                    {
                        var randomIndex = new Random().Next(0, agentServerDict.Length);
                        agentServer = agentServerDict[randomIndex].Value;
                        break;

                    }
                case ConsulLoadBanlaceType.Weight:
                    {
                        var weightAgentServerDict = new List<KeyValuePair<string, AgentService>>();
                        agentServerDict.ToList().ForEach(s =>
                        {
                            for (var i = 0; i < int.Parse(s.Value.Tags.FirstOrDefault()); i++)
                            {
                                weightAgentServerDict.Add(new KeyValuePair<string, AgentService>(s.Key, s.Value));
                            }
                        });
                        var randomIndex = new Random().Next(0, weightAgentServerDict.Count);
                        agentServer = weightAgentServerDict[randomIndex].Value;
                        break;
                    }
            }

            var requestUrl = new Uri(requestConsulUrl);
            return $"{requestUrl.Scheme}://{agentServer?.Address}:{agentServer?.Port}{requestUrl.PathAndQuery}";
        }

        private static string GetHostSettingHost(IConfiguration configuration)
        {
            var hostUrlsStr = configuration.GetSection("urls").Value ?? ConfigurationExtend.GetConfigurationValue("hostsettings.json", "urls");
            var hostUrlList = hostUrlsStr.Split(';');

            if (hostUrlList == null || !hostUrlList.Any()) return null;

            var host = new Uri(hostUrlList.FirstOrDefault().Replace("*", "127.0.0.1")).Host;
            Console.WriteLine($"--hostsetting.urls.host:{host}");

            return host;
        }

        private static int? GetHostSettingPort(IConfiguration configuration)
        {
            var hostUrlsStr = configuration.GetSection("urls").Value ?? ConfigurationExtend.GetConfigurationValue("hostsettings.json", "urls");
            var hostUrlList = hostUrlsStr.Split(';');

            if (hostUrlList == null || !hostUrlList.Any()) return null;

            var port = new Uri(hostUrlList.FirstOrDefault().Replace("*", "127.0.0.1")).Port;
            Console.WriteLine($"--hostsetting.urls.port:{port}");

            return port;
        }
    }

    public enum ConsulLoadBanlaceType
    {
        /// <summary>
        /// 轮徇
        /// </summary>
        RoundTurn,
        /// <summary>
        /// 随机
        /// </summary>
        Random,
        /// <summary>
        /// 权重
        /// </summary>
        Weight
    }
}
