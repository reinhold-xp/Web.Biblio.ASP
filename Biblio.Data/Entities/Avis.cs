using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Biblio.Data.Entities
{
    public class Avis
    {
        public int Id { get; set; }
        public int LivreId { get; set; }
        public string User { get; set; }
        public string Commentaire { get; set; }
        public DateTime Date { get; set; }
        public double Note { get; set; }
    }
}
