using Biblio.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Biblio.WebUI.Models
{
    public class HomeViewModel
    {
        public HomeViewModel()
        {
            Liste = new List<BookViewModel>();
        }

        public List<BookViewModel> Liste { get; set; }
    }
}