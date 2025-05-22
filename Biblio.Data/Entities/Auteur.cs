using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Biblio.Data.Entities
{
    public class Auteur
    {
        public int Id { get; set; }
        public string Nom { get; set; }
        public DateTime DateNaissance { get; set; }
        public string Nationalite { get; set; }
    }
}
