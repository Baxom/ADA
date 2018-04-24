using ADA.Data.Model;
using ADA.Domain.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ADA.Site.Models.Interface
{
    public interface ITriable 
    {
        TriModel Tri { get; set; }
        List<TriModel> Tris { get; }
    }
}
