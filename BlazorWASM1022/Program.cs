using System;
using System.Net.Http;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Text;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace BlazorWASM1022
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            builder.RootComponents.Add<App>("app");

            builder.Services.AddScoped<GraphAPIAuthorizationMessageHandler>();

            builder.Services.AddHttpClient("GraphAPI",
                    client => client.BaseAddress = new Uri("https://graph.microsoft.com"))
                .AddHttpMessageHandler<GraphAPIAuthorizationMessageHandler>();
            
            builder.Services.AddMsalAuthentication(options =>
            {
                builder.Configuration.Bind("AzureAd", options.ProviderOptions.Authentication);
                
                options.ProviderOptions.DefaultAccessTokenScopes.Add("User.Read");
                options.ProviderOptions.DefaultAccessTokenScopes.Add("Mail.Read");
            });

            await builder.Build().RunAsync();
        }
    }
}
