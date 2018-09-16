using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ADA.Infrastructure.Services.Interface.WordSearchParser
{
    public interface IWordSearchParser
    {
        List<Word> Parse(string inputSearch);
    }
}
