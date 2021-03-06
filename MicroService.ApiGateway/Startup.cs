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


            //Ocelot╝»│╔IdentityServer4
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
             Ocelot ╠ß╣ę ┬ĚË╔╣Ž─▄úČ┐╔┼ńÍ├│Č╩▒╚█Â¤╗˙ÍĂúČ┴¸┴┐¤ŮÍĂúČĚ■╬˝ŻÁ╝Â, ┐╔┼ńÍ├Ş║ďěż¨║Ô(Consul+Polly)úČ┐╔═Ę╣řConsul╩Á¤ÍĚ■╬˝Îó▓ßĚó¤Í ,ăŰăˇż█║¤
             1.░▓Î░nuget░Ř Ocelot , Ocelot.Provider.Consul,  Ocelot.Provider.Polly
             2.ď┌ConfigureServices Íđ services.AddOcelot()
             3.ď┌ConfigureServices Íđ services.AddConsul()
             4.ď┌Configure Íđ app.UseOcelot
             5. ╠Ý╝Ëconfiguration.json ┼ńÍ├╬─╝■┼ńÍ├ÁŻ¤ţ─┐Íđ

            ┼ńÍ├╗║┤Š 
            1.Install-Package Ocelot.Cache.CacheManager
            2.services..AddCacheManager(x =>
                        {
                            x.WithDictionaryHandle();
                        })
            3. ocelot.json ╠Ý╝Ë┼ńÍ├¤ţ "FileCacheOptions": { "TtlSeconds": 15, "Region": "somename" }
            
            ┼ńÍ├ÎďÂĘĎň╗║┤Š
            1.ÂĘĎň╩Á¤Í IOcelotCache<CachedResponse>ŻË┐┌Á─╩Á¤Í└Ó
            2.services.AddSingleton<IOcelotCache<CachedResponse>, OcelotCustomCache>()
            */
            services.AddOcelot()
                    .AddCacheManager(t => t.WithDictionaryHandle()) //─Č╚¤ÎÍÁń╗║┤Š
                    .AddConsul();

            //─Č╚¤╗║┤Š╠Š╗╗│╔ÎďÂĘĎň╗║┤Š
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

            app.UseAuthentication();//╠Ý╝Ë╝°╚Ę

            app.UseAuthorization();

            //app.UseEndpoints(endpoints =>
            //{
            //    endpoints.MapControllers();
            //});

            app.UseOcelot();//┼ńÍ├OcelotÍđ╝ń╝■
        }
    }
}
