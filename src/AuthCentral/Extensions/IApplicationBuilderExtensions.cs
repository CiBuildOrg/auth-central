using System;
using System.Collections.Generic;
using System.Globalization;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

using Microsoft.AspNet.Authentication;
using Microsoft.AspNet.Authentication.Cookies;
using Microsoft.AspNet.Authentication.OpenIdConnect;
using Microsoft.AspNet.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.PlatformAbstractions;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;

using BrockAllen.MembershipReboot;
using BrockAllen.MembershipReboot.Hierarchical;
using IdentityServer3.Core.Configuration;
using IdentityServer3.Core.Services;
using IdentityServer3.MembershipReboot;
using Owin;

using Fsw.Enterprise.AuthCentral.IdSvr;
using Fsw.Enterprise.AuthCentral.MongoStore;
using Fsw.Enterprise.AuthCentral.IdMgr;

using CookieOptions = IdentityServer3.Core.Configuration.CookieOptions;

namespace Fsw.Enterprise.AuthCentral.Extensions
{
    using DataProtectionProviderDelegate = Func<string[], Tuple<Func<byte[], byte[]>, Func<byte[], byte[]>>>;
    using DataProtectionTuple = Tuple<Func<byte[], byte[]>, Func<byte[], byte[]>>;

    public static class IApplicationBuilderExtensions
    {
        public static void UseIdentityServer(this IApplicationBuilder app, IApplicationEnvironment env, ILoggerFactory loggerFactory, EnvConfig config, StoreSettings idSvrStoreSettings)
        {
            var usrSrv = new Registration<IUserService, MembershipRebootUserService>();
            var idSvcFactory = new ServiceFactory(usrSrv, idSvrStoreSettings)
            {
                ViewService = new Registration<IViewService>(typeof(CustomViewService))
            };

            idSvcFactory.ConfigureCustomUserService(config.DB.MembershipReboot, env, loggerFactory, config);
            idSvcFactory.Register(new Registration<IApplicationEnvironment>(env));
            idSvcFactory.Register(
                new Registration<DefaultUserAccountServiceContainer>(resolver =>
                    new DefaultUserAccountServiceContainer
                    {
                        Service = new UserAccountService<HierarchicalUserAccount>(
                            MembershipRebootConfigFactory.GetDefaultConfig(env, loggerFactory, config),
                            resolver.Resolve<IUserAccountRepository<HierarchicalUserAccount>>())
                    }
                )
            );

            var options = new IdentityServerOptions
            {
                SiteName = config.AppName,
                PublicOrigin = config.Uri.IssuerUri,
                SigningCertificate = Crypto.Certificate.Get(config.Cert.StoreName, config.Cert.Thumbprint),
                IssuerUri = config.Uri.IssuerUri,
                RequireSsl = true,
                LoggingOptions = new LoggingOptions()
                {
                    EnableHttpLogging = true,
                    EnableKatanaLogging = config.IsDebug,
                    EnableWebApiDiagnostics = config.IsDebug,
                    WebApiDiagnosticsIsVerbose = config.IsDebug
                },
                Endpoints = new EndpointOptions()
                {
                    EnableCspReportEndpoint = true
                },
                Factory = idSvcFactory,
                AuthenticationOptions = new IdentityServer3.Core.Configuration.AuthenticationOptions()
                {
                    EnableLocalLogin = true,
                    EnableLoginHint = true,
                    RememberLastUsername = false,
                    CookieOptions = new CookieOptions()
                    {
                        ExpireTimeSpan = new TimeSpan(10, 0, 0),
                        IsPersistent = false,
                        SlidingExpiration = false,
                        AllowRememberMe = true,
                        RememberMeDuration = new TimeSpan(30, 0, 0, 0)
                    },
                    EnableSignOutPrompt = true,
                    EnablePostSignOutAutoRedirect = true,
                    SignInMessageThreshold = 5
                },
                CspOptions = new CspOptions()
                {
                    Enabled = true,
                    ScriptSrc = config.Csp.ScriptSrc,
                    StyleSrc = config.Csp.StyleSrc,
                    FontSrc = config.Csp.FontSrc
                },
                EnableWelcomePage = false
            };
            
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

        public static void UseOpenIdConnectAuthentication(this IApplicationBuilder app, EnvConfig config)
        {
            app.UseOpenIdConnectAuthentication(options =>
            {
                options.AuthenticationScheme = OpenIdConnectDefaults.AuthenticationScheme;
                options.SignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.ClientId = config.Client.Id;
                options.ClientSecret = config.Client.Secret;
                options.Authority = config.Uri.Authority;
                options.ResponseType = OpenIdConnectResponseTypes.Code;
                options.Scope.Add("fsw_platform");
                options.Scope.Add("profile");
                options.Scope.Add("email");
                options.Scope.Add("openid");

                options.Events = new OpenIdConnectEvents
                {
                    OnRedirectToEndSessionEndpoint = context =>
                    {
                        // set the post logout redirect uri, otherwise the
                        // first stored value associated with our client is use (not 
                        // always what we want)
                        context.Options.PostLogoutRedirectUri = config.Uri.IssuerUri;
                        return Task.FromResult(0);
                    },

                    OnRedirectToAuthenticationEndpoint = context =>
                    {
                        if (context.HttpContext.User.Identity.IsAuthenticated && context.ProtocolMessage.RequestType != OpenIdConnectRequestType.LogoutRequest)
                        {
                            context.HandleResponse();
                            context.HttpContext.Response.StatusCode = 403;
                        }

                        return Task.FromResult(0);
                    },

                    OnAuthenticationValidated = context =>
                    {
                        var id = new ClaimsIdentity("application", "given_name", "role");

                        var idToken = new JwtSecurityToken(context.TokenEndpointResponse.IdToken);
                        var token = new JwtSecurityToken(context.TokenEndpointResponse.AccessToken);

                        // get resource claims from auth token
                        IEnumerable<Claim> claims = token.Claims.Where(c => c.Type != "iss" &&
                                                                            c.Type != "aud" &&
                                                                            c.Type != "nbf" &&
                                                                            c.Type != "exp" &&
                                                                            c.Type != "iat" &&
                                                                            c.Type != "amr" &&
                                                                            c.Type != "jti" &&
                                                                            c.Type != "nonce" &&
                                                                            c.Type != "c_hash" &&
                                                                            c.Type != "at_hash");

                        // get identity claims from identity token
                        claims = claims.Concat(idToken.Claims.Where(c =>    c.Type != "iss" &&
                                                                            c.Type != "aud" &&
                                                                            c.Type != "nbf" &&
                                                                            c.Type != "exp" &&
                                                                            c.Type != "jti" &&
                                                                            c.Type != "amr" &&
                                                                            c.Type != "sub" &&
                                                                            c.Type != "idp" &&
                                                                            c.Type != "auth_time" &&
                                                                            c.Type != "iat" &&
                                                                            c.Type != "nonce" &&
                                                                            c.Type != "c_hash" &&
                                                                            c.Type != "at_hash"));

                        string expiration = token.Claims.First(c => c.Type == "exp").Value;

                        id.AddClaims(claims);
                        id.AddClaim(new Claim("expires_at",
                            new DateTime(1970, 1, 1).AddSeconds(Convert.ToDouble(expiration))
                                .ToString(CultureInfo.CurrentCulture)));

                        context.AuthenticationTicket = new AuthenticationTicket(
                            new ClaimsPrincipal(id),
                            context.AuthenticationTicket.Properties,
                            context.AuthenticationTicket.AuthenticationScheme);

                        return Task.FromResult(0);
                    },
                };
            });
        }

        public static void ConfigureLoggers(this IApplicationBuilder app, ILoggerFactory loggerFactory, bool isDebug)
        {
            if (isDebug)
            {
                loggerFactory.MinimumLevel = LogLevel.Verbose;
                app.UseDeveloperExceptionPage();
            }
            else
            {
                loggerFactory.MinimumLevel = LogLevel.Error;
                app.UseRuntimeInfoPage();
            }
        }
    }
}
