using ADA.Domain.Fonds;
using ADA.Domain.Revues;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ADA.Site.Models.Menu
{
    public class MenuViewModel
    {
        public List<Revue> Revues { get; set; }
        public List<Fond> Fonds { get; set; }

        public MenuViewModel()
        {
            Revues = new List<Revue>();
            Fonds = new List<Fond>();
        }
    }
}
