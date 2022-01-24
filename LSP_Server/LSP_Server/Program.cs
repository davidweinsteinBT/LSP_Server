using OmniSharp.Extensions.LanguageServer.Server;
using System;
using System.Threading.Tasks;

namespace LSP_Server
{
    internal class Program
    {
        private static void Main(string[] args) => MainAsync(args).Wait();

        private static async Task MainAsync(string[] args)
        {

            var server = await LanguageServer.From(
                options =>
                    options
                       .WithInput(Console.OpenStandardInput())
                       .WithOutput(Console.OpenStandardOutput())
            ).ConfigureAwait(false);

            await server.WaitForExit.ConfigureAwait(false);
        }
    }
}
