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
             Identity Server4 ��֤��������
             1. ��װ IdentityServer4 ��
             2. ������֤��ʽ
                services.AddIdentityServer()
                .AddDeveloperSigningCredential()//Ĭ��ʹ�ÿ���֤��
                .AddInMemoryClients(AuthenticationInit.GetClients())
                .AddInMemoryApiScopes(AuthenticationInit.GetApiScopes())
                .AddInMemoryApiResources(AuthenticationInit.GetApiResources());
             3. �����м��
                 app.UseIdentityServer();//�����֤�м��

             4. �鿴�����ĵ� http://localhost:7200/.well-known/openid-configuration
             */


            //������֤��ʽ
            services.AddIdentityServer()
                .AddDeveloperSigningCredential()//Ĭ��ʹ�ÿ���֤��
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

            app.UseIdentityServer();//�����֤�м��

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
