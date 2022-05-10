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
using Ocelot.DependencyInjection;
using Ocelot.Provider.Consul;
using Ocelot.Middleware;
using Ocelot.Cache.CacheManager;
using Ocelot.Cache;
using MicroService.Framework.Ocelot;
using MicroService.Framework.Utility;
using Microsoft.IdentityModel.Tokens;

namespace MicroService.ApiGateway
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

            //services.AddControllers();


            //Ocelot集成IdentityServer4
            var defaultScheme = Configuration.GetValue<string>("Authenticate:IdentityServer4:DefaultScheme");
            var authenticationProviderKey = Configuration.GetValue<string>("Authenticate:IdentityServer4:AuthenticationProviderKey");
            var secret = System.Text.Encoding.UTF8.GetBytes(Configuration.GetValue<string>("Authenticate:IdentityServer4:ClientSecret"));
            //services.AddAuthentication(defaultScheme)
            services.AddAuthentication()
                .AddJwtBearer(authenticationProviderKey, options =>
                {
                    options.Authority = Configuration.GetValue<string>("Authenticate:IdentityServer4:AuthUrl");
                    options.RequireHttpsMetadata = false;
                    options.SaveToken = true;
                    
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(secret),
                        ValidateAudience = false,
                        ValidateIssuer = false

                    };
                });

            /*
             Ocelot 提供 路由功能，可配置超时熔断机制，流量限制，服务降级, 可配置负载均衡(Consul+Polly)，可通过Consul实现服务注册发现 ,请求聚合
             1.安装nuget包 Ocelot , Ocelot.Provider.Consul,  Ocelot.Provider.Polly
             2.在ConfigureServices 中 services.AddOcelot()
             3.在ConfigureServices 中 services.AddConsul()
             4.在Configure 中 app.UseOcelot
             5. 添加configuration.json 配置文件配置到项目中

            配置缓存 
            1.Install-Package Ocelot.Cache.CacheManager
            2.services..AddCacheManager(x =>
                        {
                            x.WithDictionaryHandle();
                        })
            3. ocelot.json 添加配置项 "FileCacheOptions": { "TtlSeconds": 15, "Region": "somename" }
            
            配置自定义缓存
            1.定义实现 IOcelotCache<CachedResponse>接口的实现类
            2.services.AddSingleton<IOcelotCache<CachedResponse>, OcelotCustomCache>()
            */
            services.AddOcelot()
                    .AddCacheManager(t => t.WithDictionaryHandle()) //默认字典缓存
                    .AddConsul();

            //默认缓存替换成自定义缓存
            services.AddSingleton<IOcelotCache<CachedResponse>, OcelotCustomCache>();



        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            //if (env.IsDevelopment())
            //{
            //    app.UseDeveloperExceptionPage();
            //}

            //app.UseRouting();

            app.UseAuthentication();//添加鉴权

            app.UseAuthorization();

            //app.UseEndpoints(endpoints =>
            //{
            //    endpoints.MapControllers();
            //});

            app.UseOcelot();//配置Ocelot中间件
        }
    }
}
