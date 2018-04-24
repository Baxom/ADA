﻿using ADA.Domain.Pretres;
using ADA.Domain.Revues;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ADA.Data.Configurations
{
    public class RevueGroupeIndexEntityConfiguration : EntityTypeConfiguration<RevueGroupeIndex>
    {
        public RevueGroupeIndexEntityConfiguration()
        {
            this.HasRequired(b => b.GroupeIndex);
        }
        
    }
}
