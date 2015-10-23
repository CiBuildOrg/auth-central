using System;
using IdentityServer3.Core.Configuration;
using System.Threading.Tasks;
using System.Collections.Generic;
using Owin;
using Microsoft.Framework.DependencyInjection;
using IdentityManager.Configuration;

namespace Microsoft.AspNet.Builder
{
    using Microsoft.Owin.Security.Cookies;
    using DataProtectionProviderDelegate = Func<string[], Tuple<Func<byte[], byte[]>, Func<byte[], byte[]>>>;
    using DataProtectionTuple = Tuple<Func<byte[], byte[]>, Func<byte[], byte[]>>;

    public static class IApplicationBuilderExtensions
    {
        public static void UseIdentityServer(this IApplicationBuilder app, IdentityServerOptions options)
        {
            app.UseOwin(addToPipeline =>
            {
                addToPipeline(next =>
                {
                    var builder = new Microsoft.Owin.Builder.AppBuilder();
                    var provider = app.ApplicationServices.GetService<Microsoft.AspNet.DataProtection.IDataProtectionProvider>();

                    builder.Properties["security.DataProtectionProvider"] = new DataProtectionProviderDelegate(purposes =>
                    {
                        var dataProtection = provider.CreateProtector(String.Join(",", purposes));
                        return new DataProtectionTuple(dataProtection.Protect, dataProtection.Unprotect);
                    });

                    builder.UseIdentityServer(options);

                    var appFunc = builder.Build(typeof(Func<IDictionary<string, object>, Task>)) as Func<IDictionary<string, object>, Task>;
                    return appFunc;
                });
            });
        }

        public static void UseIdentityManager(this IApplicationBuilder app, IdentityManagerOptions options)
        {
            app.UseOwin(addToPipeline =>
            {
                addToPipeline(next =>
                {
                    var builder = new Microsoft.Owin.Builder.AppBuilder();

                    // This might never be used...?
                    var provider = app.ApplicationServices.GetService<Microsoft.AspNet.DataProtection.IDataProtectionProvider>();
                    builder.Properties["security.DataProtectionProvider"] = new DataProtectionProviderDelegate(purposes =>
                    {
                        var dataProtection = provider.CreateProtector(String.Join(",", purposes));
                        return new DataProtectionTuple(dataProtection.Protect, dataProtection.Unprotect);
                    });

                    builder.UseIdentityManager(options);

                    var appFunc = builder.Build(typeof(Func<IDictionary<string, object>, Task>)) as Func<IDictionary<string, object>, Task>;
                    return appFunc;
                });
            });
        }

        //public static void UseOpenIdConnectAuthentication(this IApplicationBuilder app, Microsoft.Owin.Security.OpenIdConnect.OpenIdConnectAuthenticationOptions options)
        //{
        //    app.UseOwin(addToPipeline =>
        //    {
        //        addToPipeline(next =>
        //        {
        //            var builder = new Microsoft.Owin.Builder.AppBuilder();

        //            // This might never be used...?
        //            var provider = app.ApplicationServices.GetService<Microsoft.AspNet.DataProtection.IDataProtectionProvider>();
        //            builder.Properties["security.DataProtectionProvider"] = new DataProtectionProviderDelegate(purposes =>
        //            {
        //                var dataProtection = provider.CreateProtector(String.Join(",", purposes));
        //                return new DataProtectionTuple(dataProtection.Protect, dataProtection.Unprotect);
        //            });

        //            builder.UseOpenIdConnectAuthentication(options);

        //            var appFunc = builder.Build(typeof(Func<IDictionary<string, object>, Task>)) as Func<IDictionary<string, object>, Task>;
        //            return appFunc;
        //        });
        //    });
        //}
    }
}