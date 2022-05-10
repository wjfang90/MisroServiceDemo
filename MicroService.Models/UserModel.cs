using System;

namespace MicroService.Models
{
    public class UserModel
    {
        public string Id { get; set; }
        public int? Age { get; set; }
        public string Name { get; set; }

        public string CreateTime => DateTime.Now.ToString("yyyy-mm-dd HH:mm:ss:fff");
    }
}
