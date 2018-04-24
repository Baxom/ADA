using ADA.Domain.Revues;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ADA.Site.Models.MultiSelect
{
    public class MultiSelectRevueModel : MultiSelectModel<Revue, int>
    {
        public MultiSelectRevueModel(IEnumerable<Revue> values, IEnumerable<int> selectedValues,
            Func<Revue, int> funcKeySelection, Func<Revue, string> funcDisplaySelection,
            string name)
            : base(values, selectedValues, funcKeySelection, funcDisplaySelection, name)
        {

        }
    }
}