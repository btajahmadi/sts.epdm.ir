using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IdentityServer4.Models;
using IdentityServer4.Test;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace sts.epdm.ir
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddIdentityServer()
                .AddInMemoryClients(GetClients())
                .AddInMemoryIdentityResources(GetIdentityResources())
                .AddDeveloperSigningCredential()
                .AddTestUsers(TestUsers.Users);


            services.AddMvc();
        }

        private IEnumerable<IdentityResource> GetIdentityResources()
        {
            return new List<IdentityResource>()
            {
                new IdentityResources.OpenId(),
                new IdentityResources.Email(),
                new IdentityResources.Profile()
            };
        }

        private IEnumerable<Client> GetClients()
        {
            var clients = new List<Client>()
            {
                new Client()
                {
                    ClientId = "IS4MvcClient",
                    ClientName = "Identity Server 4 MVC Client",
                    AllowedGrantTypes = { GrantType.Implicit },
                    RedirectUris =  { "http://localhost:5100/signin-oidc" },
                    PostLogoutRedirectUris =  { "http://localhost:5100/" },
                    AllowedScopes = 
                    {
                        IdentityServer4.IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServer4.IdentityServerConstants.StandardScopes.Email,
                        IdentityServer4.IdentityServerConstants.StandardScopes.Profile
                    }
                }
            };

            return clients;
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {

                app.UseDeveloperExceptionPage();
            }

            app.UseIdentityServer();
            app.UseStaticFiles();
            app.UseMvcWithDefaultRoute();
            app.Run(async (context) =>
            {
                await context.Response.WriteAsync("Hello World!");
            });
        }
    }
}
