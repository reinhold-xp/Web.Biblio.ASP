using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Biblio.Data.Entities
{
    public class Livre
    {
        public int Id { get; set; }
        public string Titre { get; set; }
        public int Pages { get; set; }
        public string Image { get; set; } 
        public string Resume { get; set; }
        public int Id_auteur { get; set; }
        
    }
}
