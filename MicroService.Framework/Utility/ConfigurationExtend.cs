using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MicroService.Framework.Utility
{
    public class ConfigurationExtend
    {
        public static string GetConfigurationValue(string configFileName, string key)
        {
            var configurationBuilder = new ConfigurationBuilder();

            return configurationBuilder
                    .SetBasePath(Directory.GetCurrentDirectory())
                    .AddJsonFile(configFileName, true, false)
                    .Build()
                    .GetSection(key).Value;
        }
    }
}
