using LSP_Server.Handlers;
using Microsoft.Extensions.DependencyInjection;
using OmniSharp.Extensions.LanguageServer.Protocol.Models;
using OmniSharp.Extensions.LanguageServer.Server;
using System;
using System.Threading.Tasks;

namespace LSP_Server
{
    internal class Program
    {
        private static async Task Main(string[] args)
        {
            var server = await LanguageServer.From(
                options =>
                    options
                       .WithInput(Console.OpenStandardInput())
                       .WithOutput(Console.OpenStandardOutput())
                       .WithHandler<HoverHandler>()
                       .WithServices(
                            services =>
                            {
                                services.AddSingleton(
                                    new ConfigurationItem
                                    {
                                        Section = "plaintext"
                                    }
                                );
                            }
                       )
            );

            await server.WaitForExit;
        }
    }
}
