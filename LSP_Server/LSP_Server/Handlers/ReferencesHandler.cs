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
    public class ReferencesHandler : IReferencesHandler
    {
        private static DocumentsManager _documentsManager;
        private static PositionManager _positionManager;
        public ReferencesHandler(DocumentsManager documentsManager, PositionManager positionManager)
        {
            _documentsManager = documentsManager;
            _positionManager = positionManager;
        }
        public ReferenceRegistrationOptions GetRegistrationOptions(ReferenceCapability capability, ClientCapabilities clientCapabilities)
        {
            return new ReferenceRegistrationOptions()
            {
                DocumentSelector = DocumentSelector.ForLanguage("plaintext")
            };
        }

        public Task<LocationContainer> Handle(ReferenceParams request, CancellationToken cancellationToken)
        {
            var documentText = _documentsManager.GetDocument(request.TextDocument.Uri.ToString());
            var word = _positionManager.GetWordAtPosition(documentText, request.Position);
            return Task.FromResult(new LocationContainer(_documentsManager.GetWordLocations(word)));
        }
    }
}
