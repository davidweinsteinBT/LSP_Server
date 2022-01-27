using LSP_Server.Workspace;
using OmniSharp.Extensions.LanguageServer.Protocol.Client.Capabilities;
using OmniSharp.Extensions.LanguageServer.Protocol.Document;
using OmniSharp.Extensions.LanguageServer.Protocol.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace LSP_Server.Handlers
{
    public class HoverHandler : IHoverHandler
    {
        private readonly DocumentsManager _documentsManager;
        private readonly DictionaryManager _dictionaryManager;
        private readonly PositionManager _positionManager;

        public HoverHandler(DocumentsManager documentsManager, DictionaryManager dictionaryManager, PositionManager positionManager)
        {
            _documentsManager = documentsManager;
            _dictionaryManager = dictionaryManager;
            _positionManager = positionManager;
        }

        public HoverRegistrationOptions GetRegistrationOptions(HoverCapability capability, ClientCapabilities clientCapabilities)
        {
            return new HoverRegistrationOptions()
            {
                DocumentSelector = new DocumentSelector(
                    new DocumentFilter()
                    {
                        Pattern = "**/*.txt"
                    },
                    new DocumentFilter()
                    {
                        Language = "plaintext"
                    }
                )
            };
        }

        public Task<Hover> Handle(HoverParams request, CancellationToken cancellationToken)
        {
            var document = _documentsManager.GetDocument(request.TextDocument.Uri.ToString());
            var wordAtPos = _positionManager.GetWordAtPosition(document, request.Position);
            var definitions = _dictionaryManager.GetDefinitions(wordAtPos);

            return Task.FromResult(new Hover()
            {
                Contents = new MarkedStringsOrMarkupContent(definitions.Select(d => new MarkedString(d)))
            });
        }
    }
}
