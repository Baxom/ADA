using ADA.Infrastructure.Services.Interface.WordSearchParser;
using Sprache;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ADA.Infrastructure.Services.Core.WordSearchParser
{
    public class SprachWordSearchParser : IWordSearchParser
    {
        private static Parser<string> Text = Sprache.Parse.CharExcept('"').Many().Text();
        private static Parser<string> TextWord = Sprache.Parse.CharExcept("\" ").Many().Text();
        private static Parser<char> CompleteWordDelimiter = Sprache.Parse.Char('"');

        private static Parser<Word> CompleteWordParser =
            (from first in CompleteWordDelimiter
             from wordrest in Text
             from end in CompleteWordDelimiter
             select wordrest).Token().Select( b => new CompleteWord(b));


        public List<Word> Parse(string inputSearch)
        {
            if (inputSearch == null) inputSearch = String.Empty;

            return CompleteWordParser.XOr(TextWord.Token().Select(b => new Word(b))).Many().Parse(inputSearch).ToList();
        }
    }
}
