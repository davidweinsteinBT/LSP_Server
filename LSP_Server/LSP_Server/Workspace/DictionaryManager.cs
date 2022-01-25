using Microsoft.VisualBasic.FileIO;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace LSP_Server.Workspace
{
    public class DictionaryManager
    {
        private ILookup<string, string> _dictionary;

        public DictionaryManager()
        {
            /*
             * Dictionary sourced from Manas Sharma and Kai Saru
             * https://www.bragitoff.com/2016/03/english-dictionary-in-csv-format/
             * https://docs.google.com/spreadsheets/d/1vgNJpEWVppQv1CYPE8O_Z72mugHiqbMCvWbQPsEATcY/edit#gid=0
             */
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
                    definitions.AddRange(Regex.Split(fields[1], @"(?=[[])"));

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
