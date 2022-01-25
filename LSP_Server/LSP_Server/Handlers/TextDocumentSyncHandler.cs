using LSP_Server.Workspace;
using MediatR;
using OmniSharp.Extensions.LanguageServer.Protocol;
using OmniSharp.Extensions.LanguageServer.Protocol.Client.Capabilities;
using OmniSharp.Extensions.LanguageServer.Protocol.Document;
using OmniSharp.Extensions.LanguageServer.Protocol.Models;
using OmniSharp.Extensions.LanguageServer.Protocol.Server.Capabilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace LSP_Server.Handlers
{
    public class TextDocumentSyncHandler : ITextDocumentSyncHandler
    {
        private readonly DocumentsManager _documentsManager;

        public TextDocumentSyncHandler(DocumentsManager documentsManager)
        {
            _documentsManager = documentsManager;
        }

        public TextDocumentChangeRegistrationOptions GetRegistrationOptions(SynchronizationCapability capability, ClientCapabilities clientCapabilities)
        {
            return new TextDocumentChangeRegistrationOptions()
            {
                DocumentSelector = DocumentSelector.ForLanguage("plaintext"),
                SyncKind = TextDocumentSyncKind.Full        
            };
        }

        TextDocumentOpenRegistrationOptions IRegistration<TextDocumentOpenRegistrationOptions, SynchronizationCapability>.GetRegistrationOptions(SynchronizationCapability capability, ClientCapabilities clientCapabilities)
        {
            return new TextDocumentOpenRegistrationOptions()
            {
                DocumentSelector = DocumentSelector.ForLanguage("plaintext"),
            };

        }

        public TextDocumentAttributes GetTextDocumentAttributes(DocumentUri uri)
        {
            return new TextDocumentAttributes(uri, "plaintext");
        }

        public Task<Unit> Handle(DidChangeTextDocumentParams request, CancellationToken cancellationToken)
        {
            var documentPath = request.TextDocument.Uri.ToString();
            var text = request.ContentChanges.FirstOrDefault()?.Text;

            _documentsManager.UpdateBuffer(documentPath, text);

            return Unit.Task;
        }

        public Task<Unit> Handle(DidOpenTextDocumentParams request, CancellationToken cancellationToken)
        {
            var documentPath = request.TextDocument.Uri.ToString();
            var text = request.TextDocument.Text;

            _documentsManager.UpdateBuffer(documentPath, text);

            return Unit.Task;
        }

        public Task<Unit> Handle(DidCloseTextDocumentParams request, CancellationToken cancellationToken)
        {
            return Unit.Task;
        }

        public Task<Unit> Handle(DidSaveTextDocumentParams request, CancellationToken cancellationToken)
        {
            return Unit.Task;
        }


        TextDocumentCloseRegistrationOptions IRegistration<TextDocumentCloseRegistrationOptions, SynchronizationCapability>.GetRegistrationOptions(SynchronizationCapability capability, ClientCapabilities clientCapabilities)
        {
            return new TextDocumentCloseRegistrationOptions();
        }

        TextDocumentSaveRegistrationOptions IRegistration<TextDocumentSaveRegistrationOptions, SynchronizationCapability>.GetRegistrationOptions(SynchronizationCapability capability, ClientCapabilities clientCapabilities)
        {
            return new TextDocumentSaveRegistrationOptions();
        }
    }
}
