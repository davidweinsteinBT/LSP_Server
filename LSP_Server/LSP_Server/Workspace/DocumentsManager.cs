using System.Collections.Concurrent;

namespace LSP_Server.Workspace
{
    public class DocumentsManager
    {
        private ConcurrentDictionary<string, string> _documents = new ConcurrentDictionary<string, string>();

        public void UpdateBuffer(string documentPath, string documentText)
        {
            _documents.AddOrUpdate(documentPath, documentText, (k, v) => documentText);
        }

        public string GetDocument(string documentPath)
        {
            return _documents.TryGetValue(documentPath, out var document) ? document : null;
        }
    }
}
