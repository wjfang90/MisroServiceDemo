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
                        option.JsonSerializerOptions.PropertyNamingPolicy = null;//json���л�ԭ�����
                        option.JsonSerializerOptions.Encoder = System.Text.Encodings.Web.JavaScriptEncoder.Create(System.Text.Unicode.UnicodeRanges.All);//֧��������ʾ
                    });

            /*
              �ڷ���ʵ���ϵ�����֤����ķ�������
                1.��װ Microsoft.AspNetCore.Authentication.JwtBearer
                2. ������֤��ʽ
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
                3.�����֤�м��
                  app.UseAuthentication();//��Ӽ�Ȩ

                4.����Ҫ��֤��Controller��Action�ϱ������ [Authorize]
                
                
                ��postman ��ʹ��ʹ��token����API����

                headers ��ʽ
                Authorization:Bearer token_value

                Authorization��ʽ
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

            app.UseAuthentication();//��Ӽ�Ȩ

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
