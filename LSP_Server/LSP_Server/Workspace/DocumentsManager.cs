using OmniSharp.Extensions.LanguageServer.Protocol;
using OmniSharp.Extensions.LanguageServer.Protocol.Models;
using OmniSharp.Extensions.LanguageServer.Protocol.Server;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace LSP_Server.Workspace
{
    public class DocumentsManager
    {
        private ConcurrentDictionary<string, string> _documents = new ConcurrentDictionary<string, string>();

        public void UpdateBuffer(DocumentUri documentUri, string documentText)
        {
            _documents.AddOrUpdate(documentUri.ToString(), documentText, (k, v) => documentText);
        }

        public string GetDocument(string documentPath)
        {
            return _documents.TryGetValue(documentPath, out var document) ? document : null;
        }

    }
}
