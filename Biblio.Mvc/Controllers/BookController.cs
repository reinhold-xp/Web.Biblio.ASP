using Biblio.Data.Entities;
using Biblio.Data;
using Biblio.Mvc.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Biblio.Mvc.Controllers
{
    public class BookController : Controller
    {
      
        public ActionResult Index()
        {
            var vm = new BookViewModel();
            using (var repository = new GenericRepository<Livre>())
            {
                vm.Liste = repository.GetAll().OrderBy(x => Guid.NewGuid()).ToList();
            }
            return View(vm);
        }

        public ActionResult Details(int id)
        {
            var vm = new BookViewModel();
            using (var repository = new GenericRepository<Livre>())
            {
                vm.Livre = repository.GetAll().FirstOrDefault(x => x.Id == id);
            }
            return View(vm);
        }
      }
    }