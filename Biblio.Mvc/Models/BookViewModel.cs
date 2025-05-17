using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Biblio.Data.Entities;

namespace Biblio.Mvc.Models
{
    public class BookViewModel
    {
        public List<Livre> Liste { get; set; }
        public Livre Livre { get; set; }
        public List<Avis> Avis { get; set; }

    }
}