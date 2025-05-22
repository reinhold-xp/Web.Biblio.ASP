using Biblio.Data.Entities;
using Biblio.Data;
using Biblio.WebUI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Biblio.WebUI.Controllers
{
    public class BookController : Controller
    {
      
        public ActionResult Index()
        {
            var vm = new BookViewModel();
            using (var repository = new GenericRepository<Livre>())
            {
                vm.Collection = repository.GetAll().OrderBy(x => x.Titre).ToList();
            }
            return View(vm);
        }

        public ActionResult Details(int id)
        {
            var vm = new BookViewModel();
            using (var repository = new GenericRepository<Livre>())
            {
                vm.Livre = repository.Read(id);
            }
            return View(vm);
        }

        public ActionResult Test(int id)
        {
            var vm = new BookViewModel();
            using (var repository = new GenericRepository<Livre>())
            {
                vm.Livre = repository.GetStoredProcedure(id);
            }
            return View(vm);
        }
    }
 }