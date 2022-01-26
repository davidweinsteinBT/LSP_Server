using LSP_Server.Handlers;
using LSP_Server.Workspace;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using OmniSharp.Extensions.LanguageServer.Protocol;
using OmniSharp.Extensions.LanguageServer.Protocol.Models;
using OmniSharp.Extensions.LanguageServer.Server;
using Serilog;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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
                       .WithHandler<TextDocumentSyncHandler>()
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
                                services.AddSingleton<DocumentsManager>();
                                services.AddSingleton<DictionaryManager>();
                                services.AddSingleton<PositionManager>();
                            }
                       )
            );

            await server.WaitForExit;
        }
    }
}
