using Microsoft.AspNet.Builder;
using Microsoft.AspNet.Mvc;
using Microsoft.Dnx.Runtime;
using Microsoft.Framework.DependencyInjection;
using System.Security.Cryptography.X509Certificates;

using IdentityServer3.Core.Configuration;

namespace Fsw.Enterprise.AuthCentral
{
    public class Startup
    {

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDataProtection();
            services.AddMvc();
        }

        public void Configure(IApplicationBuilder app, IApplicationEnvironment env)
        {
            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "account/{controller}/{action}/{id?}",
                    defaults: new { controller = "Home", action = "Index" }
                );
            });

            app.UseIISPlatformHandler();
            app.UseDeveloperExceptionPage();
            var certFile = env.ApplicationBasePath + "\\idsrv3test.pfx";
            var idsrvOptions = new IdentityServerOptions
            {
                Factory = new IdentityServerServiceFactory()
                                .UseInMemoryUsers(Users.Get())
                                .UseInMemoryClients(Clients.Get())
                                .UseInMemoryScopes(Scopes.Get()),

                SigningCertificate = new X509Certificate2(certFile, "idsrv3test"),
                AuthenticationOptions = new AuthenticationOptions
                {
                    EnablePostSignOutAutoRedirect = true
                }
            };

            app.UseIdentityServer(idsrvOptions);
        }
    }
}