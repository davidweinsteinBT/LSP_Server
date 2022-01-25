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
                    var recordExists = tempDict.TryGetValue(fields[0], out definitions);
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
                        tempDict.Add(fields[0], definitions);
                    }
                    else
                    {
                        tempDict[fields[0]] = definitions;
                    }
                }
            }
            _dictionary = tempDict.SelectMany(w => w.Value, (w, Value) => new { w.Key, Value }).ToLookup(d => d.Key, d => d.Value);
        }

        public IEnumerable<string> GetDefinitions(string word)
        {
            if (!_dictionary.Contains(word))
            {
                return null;
            }
            return _dictionary[word];
        }

    }
}
