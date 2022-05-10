using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MicroServiceInstance.Models
{
    public class ResponseModel
    {
        public string IP { get; set; }
        public string Port { get; set; }

        public object Data { get; set; }
    }
}
