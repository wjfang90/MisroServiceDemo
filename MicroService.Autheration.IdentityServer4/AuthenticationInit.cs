using IdentityServer4.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MicroService.Autheration.IdentityServer4
{
    public class AuthenticationInit
    {
        /// <summary>
        /// 定义ApiResource   
        /// 这里的资源（Resources）指的就是管理的API
        /// </summary>
        /// <returns>多个ApiResource</returns>
        public static IEnumerable<ApiResource> GetApiResources()
        {
            return new[]
            {
                new ApiResource("UserApi", "用户获取API")
                {
                    Scopes = new [] { "user.read", "user.write" }
                },
                new ApiResource("fangtestApi", "fang test api")
            };
        }

        public static IEnumerable<ApiScope> GetApiScopes()
        {
            return new[]
            {
                new ApiScope("user","用户获取"),
                new ApiScope("fangtest","fang test")
            };
        }

        /// <summary>
        /// 定义验证条件的Client
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<Client> GetClients()
        {
            return new[]
            {
                new Client
                {
                    ClientId = "fang_client",//客户端惟一标识
                    ClientSecrets = new [] { new Secret("fang_autheration_pwd".Sha256()) },//客户端密码，进行了加密
                    AllowedGrantTypes = GrantTypes.ClientCredentials, //授权方式，客户端认证，只要ClientId+ClientSecrets                   
                    AllowedScopes = new [] { "user", "fangtest" },//允许访问的资源
                    //Claims=new List<Claim>(){
                    //    new Claim(IdentityModel.JwtClaimTypes,"Admin")
                    //}
                }
            };
        }
    }
}
