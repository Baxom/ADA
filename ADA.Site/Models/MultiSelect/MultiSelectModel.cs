using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ADA.Site.Models.MultiSelect
{
    public class MultiSelectModel<TValues, TSelectedValues>
    {
        public IEnumerable<TValues> Values { get; private set; }
        public IEnumerable<TSelectedValues> SelectedValues { get; private set; }

        public Func<TValues, TSelectedValues> FuncKeySelection { get; private set; }
        public Func<TValues, string> FuncDisplaySelection { get; private set; }

        public string Id { get { return "multi-select-" + Name; } }
        public string Name { get; private set; }

        public MultiSelectModel(IEnumerable<TValues> values, IEnumerable<TSelectedValues> selectedValues, 
            Func<TValues, TSelectedValues> funcKeySelection, Func<TValues, string> funcDisplaySelection,
            string name)
        {
            Values = values;
            SelectedValues = selectedValues;
            FuncKeySelection = funcKeySelection;
            FuncDisplaySelection = funcDisplaySelection;
            Name = name;
        }
    }
}