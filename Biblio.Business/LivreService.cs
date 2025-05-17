using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Biblio.Data;
using Biblio.Data.Entities;

namespace Biblio.Business
{
    class LivreService
    {
        private GenericRepository<Livre> dbHelper = new GenericRepository<Livre>();

        public List<Livre> GetFilteredOeuvres(int minPages)
        {
            var livres = dbHelper.GetAll();
            return livres.FindAll(o => o.Pages >= minPages);
        }
    }
}
