using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Riddle.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace Riddle.Controllers
{
    public class RiddleController : Controller
    {
        private void UserConnect(dynamic v)
        {
            bool? logged = Convert.ToBoolean(HttpContext.Session.GetString("logged"));
            if (logged == true)
            {
                v.Logged = logged;
                v.Name = HttpContext.Session.GetString("Name");
                v.FirstName = HttpContext.Session.GetString("FirstName");
                v.UserName = HttpContext.Session.GetString("UserName");
            }
        }

        public IActionResult Index()
        {
            UserConnect(ViewBag);
            return View();
        }

        public IActionResult APropos()
        {
            UserConnect(ViewBag);
            return View();
        }

        public IActionResult RiddleDeposit()
        {
            UserConnect(ViewBag);
            Riddle1 r = new Riddle1();
            ViewBag.CategoryList = Category.LoadCategory();
            return View();
        }

        public IActionResult RiddleList(int? idCategory)
        {
            UserConnect(ViewBag);
            ViewBag.Category = Category.LoadCategory();
            List<Riddle1> liste = Riddle1.RiddleLoad(idCategory);
            return View(liste);
        }

        [Route("[Controller]/AddRiddle")]
        public IActionResult AddRiddle()
        {
            UserConnect(ViewBag);
            bool connected = Convert.ToBoolean(HttpContext.Session.GetString("logged"));
            if (connected)
            {
                Riddle1 r = new Riddle1();
                ViewBag.CategoryList = Category.LoadCategory();
                return View("RiddleDeposit", r);
            }
            else
            {
                return RedirectToRoute(new { controller = "User", action = "Login" });
            }
        }

        [Route("[Controller]/AddRiddlePost")]
        [HttpPost]
        public async Task<IActionResult> AddRiddlePost(string titre, string enonce, string indice, string solution, int? category, List<IFormFile> image)
        {
            Riddle1 r = new Riddle1() { Titre = titre, Enonce = enonce, Indice = indice, Solution = solution, IdCategory = (int)category };
            r.IdUser = Convert.ToInt32(HttpContext.Session.GetString("idUser"));
            List<string> errors = new List<string>();
            if (titre == null)
            {
                errors.Add("Merci d'indiquer un titre");
            }
            if (enonce == null)
            {
                errors.Add("Merci d'indiquer un énoncé");
            }
            if (indice == null)
            {
                errors.Add("Merci d'indiquer un indice");
            }
            if (solution == null)
            {
                errors.Add("Merci d'indiquer la solution");
            }
            if (category == 0)
            {
                errors.Add("Merci d'indiquer la catégorie de l'énigme");
            }
            ViewBag.errors = errors;
            if (errors.Count > 0)
            {
                //ViewBag.error = true;
                //ViewBag.errors = errors;
                //return RedirectToAction("RiddleDeposit", r);
                //return View("RiddleDeposit", r);
            }
            else
            {
                foreach (IFormFile f in image)
                {
                    if (f.FileName.Contains(".png") || f.FileName.Contains(".jpg"))
                    {

                        var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Images/Enigme/", r.IdUser.ToString() + "-" + f.FileName);
                        var stream = new FileStream(path, FileMode.Create);
                        await f.CopyToAsync(stream);
                        r.Image.Add(new RiddleImage { Url = "Images/Enigme/" + r.IdUser.ToString() + "-" + f.FileName });
                    }

                }
                r.AddRiddle();

            }
            return RedirectToAction("RiddleList", r);
            //return View("RiddleList", r);
        }

        public IActionResult RiddleDetail(int Id)
        {

            UserConnect(ViewBag);
            bool connected = Convert.ToBoolean(HttpContext.Session.GetString("logged"));
            if (connected)
            {
                Riddle1 r = new Riddle1();
                List<Riddle1> liste = Riddle1.GetRiddle(Id);
                return View("RiddleDetail", liste);
            }
            else
            {
                return RedirectToRoute(new { controller = "User", action = "Login" });
            }
        }

        public IActionResult DeleteRiddle(int id)
        {
            UserConnect(ViewBag);
            Riddle1 r = new Riddle1 { Id = id };
            r.DeleteRiddle();
            return RedirectToRoute(new { controller = "Riddle", action = "RiddleList" });
        }
    }
}