﻿using LSP_Server.Handlers;
using LSP_Server.Workspace;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using OmniSharp.Extensions.LanguageServer.Protocol.Models;
using OmniSharp.Extensions.LanguageServer.Server;
using System;
using System.IO;
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
                       .WithLoggerFactory(new LoggerFactory())
                       .AddDefaultLoggingProvider()
                       .WithHandler<TextDocumentSyncHandler>()
                       .WithHandler<CompletionHandler>()
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
                            }
                       )

            );

            await server.WaitForExit;
        }
    }
}
