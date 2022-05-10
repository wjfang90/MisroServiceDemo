using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MicroService.Autheration.IdentityServer4
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {

            services.AddControllers();
            /*
             Identity Server4 认证服务配置
             1. 安装 IdentityServer4 包
             2. 配置验证方式
                services.AddIdentityServer()
                .AddDeveloperSigningCredential()//默认使用开发证书
                .AddInMemoryClients(AuthenticationInit.GetClients())
                .AddInMemoryApiScopes(AuthenticationInit.GetApiScopes())
                .AddInMemoryApiResources(AuthenticationInit.GetApiResources());
             3. 配置中间件
                 app.UseIdentityServer();//添加认证中间件

             4. 查看配置文档 http://localhost:7200/.well-known/openid-configuration
             */


            //配置验证方式
            services.AddIdentityServer()
                .AddDeveloperSigningCredential()//默认使用开发证书
                .AddInMemoryClients(AuthenticationInit.GetClients())
                .AddInMemoryApiScopes(AuthenticationInit.GetApiScopes())
                .AddInMemoryApiResources(AuthenticationInit.GetApiResources());
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseIdentityServer();//添加认证中间件

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
