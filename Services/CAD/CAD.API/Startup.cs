//using EventBus.Messages.Common;
using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.WsFederation;
//using MassTransit;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
//using Ordering.API.EventBusConsumer;
using Ordering.Application;
using Ordering.Infrastructure;
using Ordering.Infrastructure.Persistence;
using Sustainsys.Saml2;
using Sustainsys.Saml2.Metadata;

namespace Ordering.API
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
            services.AddApplicationServices();
            services.AddInfrastructureServices(Configuration);

            // MassTransit-RabbitMQ Configuration
            //services.AddMassTransit(config => {

            //    config.AddConsumer<BasketCheckoutConsumer>();

            //    config.UsingRabbitMq((ctx, cfg) => {
            //        cfg.Host(Configuration["EventBusSettings:HostAddress"]);
            //        cfg.UseHealthCheck(ctx);

            //        cfg.ReceiveEndpoint(EventBusConstants.BasketCheckoutQueue, c => {
            //            c.ConfigureConsumer<BasketCheckoutConsumer>(ctx);
            //        });
            //    });
            //});
            // services.AddMassTransitHostedService();

            // General Configuration
            //  services.AddScoped<BasketCheckoutConsumer>();
            //           services.AddAuthentication(sharedOptions =>
            //           {
            //               sharedOptions.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
            //               sharedOptions.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
            //               sharedOptions.DefaultChallengeScheme = "Saml2";
            //           }).AddSaml2(options =>
            //{
            //    //options.SPOptions.EntityId = new EntityId("https://localhost:44342/Saml2");
            //    //options.SPOptions.EntityId = new EntityId("https://localhost:44373/api/v1/CAdocument/GetCAdocument?businesspartner=test");
            //    options.SPOptions.EntityId = new EntityId("https://localhost:44373/api/v1/CAdocument/GetCAdocument");
            //    options.IdentityProviders.Add(
            //     new IdentityProvider(
            //       //new EntityId("https://sts.windows.net/63eb1bcb-f74f-4703-8243-6f73d78ebf52/"), options.SPOptions)
            //       new EntityId("https://sts.windows.net/b9165cc7-82cb-4b2f-86eb-f60b7e3809a5/"), options.SPOptions)
            //    {
            //        MetadataLocation = "https://login.microsoftonline.com/b9165cc7-82cb-4b2f-86eb-f60b7e3809a5/federationmetadata/2007-06/federationmetadata.xml?appid=042b32b5-ee63-414c-96e5-fd41a8abde54"
            //     });
            //})
            //.AddCookie();

            services.AddAuthentication(sharedOptions =>
            {
                sharedOptions.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                sharedOptions.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                sharedOptions.DefaultChallengeScheme = WsFederationDefaults.AuthenticationScheme;
            })
       .AddWsFederation(options =>
       {
           // this is where your AppID URI goes
          // options.Wtrealm = "https://microsoft.onmicrosoft.com/5b60c247-398e-4c00-9ba4-a8xxxxxxxx4d1";
           options.Wtrealm = "api://79d17639-e1f0-4b4d-9be5-624bf7738697";
           //options.MetadataAddress = "https://login.microsoftonline.com/72f988bf-xxxx-41af-91ab-xxxxxxxxxxdb47/federationmetadata/2007-06/federationmetadata.xml";
           options.MetadataAddress = "https://login.microsoftonline.com/b9165cc7-82cb-4b2f-86eb-f60b7e3809a5/federationmetadata/2007-06/federationmetadata.xml";
       })
       .AddCookie();

            services.AddMvc();
            services.AddAutoMapper(typeof(Startup));
            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Ordering.API", Version = "v1" });
            });

            services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy",
                    builder => builder.AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader());
                //.AllowCredentials());
            });
            services.AddMvc();

            services.AddHealthChecks().AddDbContextCheck<HSBCContext>();
            //services.AddDbContext<OrderContext>(ServiceLifetime.Scoped);

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Ordering.API v1"));
            }
            app.UseCors("CorsPolicy");
            app.UseAuthentication();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapHealthChecks("/hc", new HealthCheckOptions()
                {
                    Predicate = _ => true,
                    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
                });
            });
        }
    }
}
