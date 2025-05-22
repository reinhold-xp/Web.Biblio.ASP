using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Biblio.Data.Entities;

namespace Biblio.WebUI.Models
{
    public class BookViewModel
    {
        public Livre Livre { get; set; }
        public List<Avis> Avis { get; set; }
        public double Note { get; set; }
        public List<Livre> Collection { get; set; }

    }
}