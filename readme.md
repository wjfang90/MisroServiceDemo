MicroService 
===

--http请求 => Authentication(IdentityServer4) => ApiGateway (Ocelot服务治理) =>   MicroService (Consul实现服务注册和服务发现)

## Consul

--优点 ：提供服务注册、服务发现、服务健康检查功能，服务异常自动下线，注册的服务自动上线功能
--缺点 ：需要手动配置负载均衡策略 

 --Consul使用方式
 1.微服务上注册Consul服务
 2.客户端发现Consul服务
 3.客户端调用Consul服务


## Ocelot

 Ocelot 提供 路由功能，可配置超时熔断机制，流量限制，服务降级, 可配置负载均衡(Consul+Polly)，可通过Consul实现服务注册发现 ,请求聚合
     1.安装nuget包 Ocelot , Ocelot.Provider.Consul,  Ocelot.Provider.Polly
     2.在ConfigureServices 中 services.AddOcelot()
     3.在ConfigureServices 中 services.AddConsul()
     4.在Configure 中 app.UseOcelot
     5. 添加configuration.json 配置文件配置到项目中

    --配置缓存 
    1.Install-Package Ocelot.Cache.CacheManager
    2.services..AddCacheManager(x =>
		{
		    x.WithDictionaryHandle();
		})
    3. ocelot.json 添加配置项 "FileCacheOptions": { "TtlSeconds": 15, "Region": "somename" }
    
    --配置自定义缓存
    1.定义实现 IOcelotCache<CachedResponse>接口的实现类
    2.services.AddSingleton<IOcelotCache<CachedResponse>, OcelotCustomCache>()


    --todo 配置IdentityServer4



 

## IdentityServer4

 --Identity Server4 认证服务配置
     1. 安装 IdentityServer4 包
     2. 配置验证方式
	services.AddIdentityServer()
	.AddDeveloperSigningCredential()//默认使用开发证书
	.AddInMemoryClients(AuthenticationInit.GetClients())
	.AddInMemoryApiResources(AuthenticationInit.GetApiResources());
     3. 配置中间件
	 app.UseIdentityServer();//添加认证中间件

     4. 查看配置文档 http://localhost:7200/.well-known/openid-configuration

--Client 认证
	IdentityModel 提供了集成认证

--API 认证	
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
                
                
## 在postman 中使用使用token调用API方法

   --headers 方式
	Authorization:Bearer token_value

   --Authorization方式
	Type:BearerToken
	Token:token_value



