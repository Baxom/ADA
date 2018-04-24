﻿using ADA.Domain.Indexation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ADA.Site.Models.Index
{
    public class GroupeIndexViewModel
    {
        public int Id { get; set; }
        public string Libelle { get; set; }
        public IEnumerable<IndexViewModel> Index { get; set; }

        public GroupeIndexViewModel()
        {
            
        }
    }
}