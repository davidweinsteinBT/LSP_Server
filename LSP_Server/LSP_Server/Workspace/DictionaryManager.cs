using Microsoft.VisualBasic.FileIO;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace LSP_Server.Workspace
{
    public class DictionaryManager
    {
        private ILookup<string, string> _dictionary;

        public DictionaryManager()
        {
            string path = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), @"Resources\dictionary.csv");
            var tempDict = new Dictionary<string, List<string>>();
            using (TextFieldParser parser = new TextFieldParser(path))
            {
                parser.TextFieldType = FieldType.Delimited;
                parser.SetDelimiters(",");
                while (!parser.EndOfData)
                {
                    var fields = parser.ReadFields();
                    List<string> definitions;
                    var word = fields[0].ToLower();
                    var recordExists = tempDict.TryGetValue(word, out definitions);
                    if(!recordExists)
                    {
                        definitions = new List<string>();
                    }
                    var definition = new StringBuilder();
                    if(fields[1].Length > 0)
                    {
                        definition.Append(fields[1]).Append(' ');
                    }
                    definition.Append(fields[2]);
                    definitions.Add(definition.ToString());

                    if (!recordExists)
                    {
                        tempDict.Add(word, definitions);
                    }
                    else
                    {
                        tempDict[word] = definitions;
                    }
                }
            }
            _dictionary = tempDict.SelectMany(w => w.Value, (w, Value) => new { w.Key, Value }).ToLookup(d => d.Key, d => d.Value);
        }

        public IEnumerable<string> GetDefinitions(string word)
        {
            if (!_dictionary.Contains(word.ToLower()))
            {
                return new List<string>();
            }
            return _dictionary[word.ToLower()];
        }

    }
}
