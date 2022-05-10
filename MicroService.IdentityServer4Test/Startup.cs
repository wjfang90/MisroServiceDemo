using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MicroService.IdentityServer4Test
{
    public class Startup
    {

        private string DefaultScheme => Configuration.GetSection("Authenticate:IdentityServer4:DefaultScheme").Value;
        private string AuthUrl => Configuration.GetSection("Authenticate:IdentityServer4:AuthUrl").Value;

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {

            services.AddControllers()
                .AddJsonOptions(option =>
                    {
                        option.JsonSerializerOptions.PropertyNamingPolicy = null;//json序列化原样输出
                        option.JsonSerializerOptions.Encoder = System.Text.Encodings.Web.JavaScriptEncoder.Create(System.Text.Unicode.UnicodeRanges.All);//支持中文显示
                    });

            /*
              在服务实例上调用认证服务的服务配置
                1.安装 Microsoft.AspNetCore.Authentication.JwtBearer
                2. 配置认证方式
                      services.AddAuthentication(DefaultScheme)
                                .AddJwtBearer(DefaultScheme, options =>
                                {
                                    options.Authority = AuthUrl;
                                    options.RequireHttpsMetadata = false;
                                    options.TokenValidationParameters = new TokenValidationParameters
                                    {
                                        ValidateAudience = false
                                    };
                                });
                3.添加认证中间件
                  app.UseAuthentication();//添加鉴权

                4.在需要认证的Controller或Action上标记特殊 [Authorize]
                
                
                在postman 中使用使用token调用API方法

                headers 方式
                Authorization:Bearer token_value

                Authorization方式
                Type:BearerToken
                Token:token_value
             */

            services.AddAuthentication(DefaultScheme)
                    .AddJwtBearer(DefaultScheme, options =>
                    {
                        options.Authority = AuthUrl;
                        options.RequireHttpsMetadata = false;
                        options.TokenValidationParameters = new TokenValidationParameters
                        {
                            ValidateAudience = false
                        };
                    });


        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseAuthentication();//添加鉴权

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
