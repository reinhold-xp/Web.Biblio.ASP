using Biblio.Data;
using Biblio.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Biblio.Business.Services
{
    public class BookService
    {
        private GenericRepository<Livre> repos = new GenericRepository<Livre>();
        public List<Livre> GetFilteredBooks(int minPages)
        {
            var livres = repos.GetAll();
            return livres.FindAll(o => o.Pages >= minPages);
        }
    }
}
