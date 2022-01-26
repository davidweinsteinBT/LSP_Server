using OmniSharp.Extensions.LanguageServer.Protocol;
using OmniSharp.Extensions.LanguageServer.Protocol.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LSP_Server.Workspace
{
    public class PositionManager
    {
        public string GetWordAtPosition(string documentText, Position position)
        {
            try
            {
                var index = GetIndex(documentText, position);

                var (startIndex, endIndex) = GetWordRangeIndices(documentText, index);

                return documentText.Substring(startIndex, endIndex - startIndex);
            }
            catch
            {
                return string.Empty;
            }
        }

        public Location GetLocationOfWordAtPosition(DocumentUri documentUri, string documentText, Position position)
        {
            return new Location()
            {
                Uri = documentUri,
                Range = GetRangeOfWordAtPosition(documentText, position)
            };

        }

        private int GetIndex(string documentText, Position position)
        {
            var lineOffset = 0;
            for (var i = 0; i < position.Line; i++)
            {
                lineOffset = documentText.IndexOf('\n', lineOffset) + 1;
            }
            return lineOffset + position.Character;
        }

        private OmniSharp.Extensions.LanguageServer.Protocol.Models.Range GetRangeOfWordAtPosition(string documentText, Position position)
        {
            var lines = documentText.Split('\n');

            var (startIndex, endIndex) = GetWordRangeIndices(lines[position.Line], position.Character);
            
            return new OmniSharp.Extensions.LanguageServer.Protocol.Models.Range(position.Line, startIndex, position.Line, endIndex);
        }

        private (int startIndex, int endIndex) GetWordRangeIndices(string text, int index)
        {
            var delimiters = new char[] { ' ', '\n', '.', ',', '\'', '/', '\\', '"', ':', ';', '{', '}', '[', ']' };
            var endIndex = text.IndexOfAny(delimiters, index);
            var startIndex = text.LastIndexOfAny(delimiters, index) + 1;

            if(endIndex == -1)
            {
                endIndex = text.Length;
            }

            return (startIndex, endIndex);
        }
    }
}
