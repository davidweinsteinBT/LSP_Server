using OmniSharp.Extensions.LanguageServer.Protocol.Document;
using OmniSharp.Extensions.LanguageServer.Protocol.Client.Capabilities;
using OmniSharp.Extensions.LanguageServer.Protocol.Models;
using System.Threading.Tasks;
using System.Threading;
using System.Collections.Generic;
using LSP_Server.Workspace;
using System.Linq;

namespace LSP_Server.Handlers
{
    public class CompletionHandler : ICompletionHandler
    {

        private readonly DictionaryManager _dictionaryManager;
        public CompletionHandler(DictionaryManager dictionaryManager)
        {
            _dictionaryManager = dictionaryManager;
        }

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
                        Label = _dictionaryManager.GetDefinitions("Keep").FirstOrDefault(),
                        Kind = CompletionItemKind.Text,
                        InsertText = _dictionaryManager.GetDefinitions("Keep").FirstOrDefault()                    }
                }
            , isIncomplete: false));
    }
}
