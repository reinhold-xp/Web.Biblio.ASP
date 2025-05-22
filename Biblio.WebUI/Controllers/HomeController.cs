using Biblio.Business.Services;
using Biblio.Data;
using Biblio.Data.Entities;
using Biblio.WebUI.Models;
using System;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web.Mvc;

namespace Biblio.WebUI.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            var vm = new HomeViewModel();
            using (var livresRepository = new GenericRepository<Livre>())
            {
                // Sélection aléatoire (3)
                var livres = livresRepository.GetAll().OrderBy(x => Guid.NewGuid()).Take(3).ToList();

                foreach (var livre in livres)
                {
                    var obj = new BookViewModel();
                    obj.Livre = livre;

                    using (var avisRepository = new GenericRepository<Avis>())
                    {
                        obj.Avis = avisRepository.GetAll().Where(x => x.LivreId == obj.Livre.Id).ToList();
                    }

                    if (obj.Avis.Count == 0)
                        obj.Note = 0;
                    else
                        obj.Note = Math.Round(obj.Avis.Average(a => a.Note), 2);

                    vm.Liste.Add(obj);
                }
            }
            return View(vm);
        }

        public ActionResult About()
        {
            return View();
        }

        public ActionResult Contact()
        {
            return View();
        }

        [HttpPost]
        public ActionResult SendMail(string email, string message)
        {
            if (string.IsNullOrWhiteSpace(message) || string.IsNullOrWhiteSpace(email))
            {
                ModelState.AddModelError("", "Adresse mail et message requis");
                return View("Contact"); 
            }

            if (!Regex.IsMatch(email, @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
            {
                ModelState.AddModelError("", "Adresse email invalide");
                return View("Contact");
            }

            try
            {
                MailService.SendMail(email, message);
            }
            catch (Exception)
            {
                ViewBag.Error = "Echec de l'envoi";
                return View("Contact");
            }

            ViewBag.Success = "Votre message a bien été envoyé";
            return View("Contact");
        }
    }
}