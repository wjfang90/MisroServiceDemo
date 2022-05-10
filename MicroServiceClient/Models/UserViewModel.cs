using MicroService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MisroServiceClient.Models
{
    public class UserViewModel
    {
        public string IP { get; set; }
        public string Port { get; set; }
        public List<UserModel> UserList { get; set; }
        public UserModel UserInfo { get; set; }
    }
}
