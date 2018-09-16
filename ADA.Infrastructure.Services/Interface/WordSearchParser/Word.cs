using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ADA.Infrastructure.Services.Interface.WordSearchParser
{
    public class Word
    {
        public string Val { get; set; }
        public virtual string SearchVal { get { return Val; } }
        public bool WordBoundary { get; protected set; }

        public Word(string val)
        {
            Val = val;
            WordBoundary = false;
        }
    }

    public class CompleteWord : Word
    {
        public override string SearchVal { get { return String.Format("\"{0}\"",Val); } }

        public CompleteWord(string val) : base(val)
        {
            WordBoundary = true;
        }
    }
}
