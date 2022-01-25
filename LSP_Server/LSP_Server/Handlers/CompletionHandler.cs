using OmniSharp.Extensions.LanguageServer.Protocol.Document;
using OmniSharp.Extensions.LanguageServer.Protocol.Client.Capabilities;
using OmniSharp.Extensions.LanguageServer.Protocol.Models;
using System.Threading.Tasks;
using System.Threading;
using System.Collections.Generic;

namespace LSP_Server.Handlers
{
    public class CompletionHandler : ICompletionHandler
    {

        public CompletionRegistrationOptions GetRegistrationOptions(CompletionCapability capability, ClientCapabilities clientCapabilities)
        {
            return new CompletionRegistrationOptions
            {
                DocumentSelector = DocumentSelector.ForLanguage("plaintext"),
                ResolveProvider = false
            };
        }

        public Task<CompletionList> Handle(CompletionParams request, CancellationToken cancellationToken) =>
            Task.FromResult(new CompletionList(
                new List<CompletionItem>()
                {
                    new CompletionItem
                    {
                        Label = "Try This",
                        Kind = CompletionItemKind.Text,
                        InsertText = "Try This"
                    }
                }
            , isIncomplete: false));
    }
}
