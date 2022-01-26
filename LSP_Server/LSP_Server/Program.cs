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
            Log.Logger = new LoggerConfiguration()
                .Enrich.FromLogContext()
                .WriteTo.File("log.txt")
                .MinimumLevel.Verbose()
                .CreateLogger();

            var server = await LanguageServer.From(
                options =>
                    options
                       .WithInput(Console.OpenStandardInput())
                       .WithOutput(Console.OpenStandardOutput())
                       .ConfigureLogging(x => x.AddSerilog(Log.Logger)
                            .AddLanguageProtocolLogging()
                            .SetMinimumLevel(LogLevel.Trace)
                        )
                       .WithHandler<TextDocumentSyncHandler>()
                       .WithHandler<HoverHandler>()
                       .WithHandler<ReferencesHandler>()
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
                       .OnInitialize(
                            async (server, request, token) =>
                            {
                                var documentsManager = server.Services.GetService<DocumentsManager>();
                                var folderPaths = new List<string>();

                                if (request.Capabilities.Workspace.WorkspaceFolders)
                                {
                                    folderPaths.AddRange(request.WorkspaceFolders?.Select(f => f.Uri.GetFileSystemPath()) ?? new List<string>());
                                }

                                if (folderPaths == null)
                                {
                                    folderPaths = request.RootUri == null ? new List<string>() { request.RootUri.GetFileSystemPath() } : new List<string>();
                                }

                                documentsManager.InitializeWorkspace(folderPaths);
                            }
                       )

            );

            await server.WaitForExit;
        }
    }
}
