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

        private readonly PositionManager _positionManager;

        public DocumentsManager(PositionManager positionManager)
        {
            _positionManager = positionManager;
        }

        public void UpdateBuffer(DocumentUri documentUri, string documentText)
        {
            _documents.AddOrUpdate(documentUri.ToString(), documentText, (k, v) => documentText);
        }

        public string GetDocument(string documentPath)
        {
            return _documents.TryGetValue(documentPath, out var document) ? document : null;
        }

        public void InitializeWorkspace(IEnumerable<string> rootFolders)
        {
            List<string> filePaths = new List<string>();
            foreach (var rootFolder in rootFolders)
            {
                filePaths.AddRange(Directory.GetFiles(rootFolder, "*.txt", SearchOption.AllDirectories));
            }

            foreach (var filePath in filePaths)
            {
                UpdateBuffer(DocumentUri.From(filePath), System.IO.File.ReadAllText(filePath));
            }
        }

        public List<Location> GetWordLocations(string word)
        {
            List<Location> locations = new List<Location>();
            foreach(var documentKey in _documents.Keys)
            {
                var documentText = _documents[documentKey];
                var lines = documentText.Split('\n');
                for(var i = 0; i < lines.Length; i++)
                {
                    for(var j = 0; j < lines[i].Length; j++)
                    {
                        if(j == 0 || lines[i][j-1] == ' ')
                        {
                            var wordAtPos = _positionManager.GetWordAtPosition(documentText, new Position(i, j));
                            if(wordAtPos == word)
                            {
                                locations.Add(_positionManager.GetLocationOfWordAtPosition(DocumentUri.From(documentKey), documentText, new Position(i, j)));
                            }
                        }
                    }
                }
            }

            return locations;
        }
    }
}
