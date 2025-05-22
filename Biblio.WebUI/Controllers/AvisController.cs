using Biblio.Data;
using Biblio.Data.Entities;
using Biblio.WebUI.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Biblio.WebUI.Controllers
{
    public class AvisController : Controller
    {
        public ActionResult Comment(int id)
        {
            var vm = new BookViewModel();
            using (var repository = new GenericRepository<Avis>())
            {
                vm.Avis = repository.GetAll().Where(x => x.LivreId == id).ToList();
            }
            using (var repository = new GenericRepository<Livre>())
            {
                vm.Livre = repository.Read(id);
            }
            return View(vm); 
        }

        [HttpPost]
        public ActionResult SaveComment(int id, string commentaire, string nom, string note)
        {
            if (string.IsNullOrWhiteSpace(nom) || string.IsNullOrWhiteSpace(commentaire))
            {
                ModelState.AddModelError("", "Le nom et le commentaire sont obligatoires.");

                var vm = new BookViewModel();
                using (var repository = new GenericRepository<Livre>())
                {
                    vm.Livre = repository.Read(id);
                }
                return View("Comment", vm);
            }

            Avis nouvelAvis = new Avis
            {
                Date = DateTime.Now,
                Commentaire = commentaire,
                User = nom,
                Note = (float)Convert.ToDouble(note),
                LivreId = id    
            };

            using (var repository = new GenericRepository<Avis>())
            {
                repository.Create(nouvelAvis);
            }
            return RedirectToAction("Details", "Book", new { id = id });
        }
    }
}