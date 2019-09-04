using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Riddle.Models;

namespace Riddle.Controllers
{
    public class CategoryController : Controller
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

        [Route("Category")]
        public IActionResult CategoryList()
        {
            UserConnect(ViewBag);
            List<Category> liste = Category.LoadCategory();
            return View("CategoryList", liste);
        }

        [Route("Category/Add")]
        public IActionResult CategoryAdd()
        {
            UserConnect(ViewBag);
            List<Category> liste = Category.LoadCategory();
            return View("CategoryAdd", liste);
        }

        [Route("Category/Register")]
        [HttpPost]
        public IActionResult CategoryRegister(string titre)
        {
            UserConnect(ViewBag);
            List<string> errors = new List<string>();
            Category c = new Category { Titre = titre };
            if (titre == null)
            {
                errors.Add("Merci de saisir un nom de catégorie");
            }
            ViewBag.errors = errors;
            if (c.ExistCategory())
            {
                errors.Add("Catégorie déjà existante");
            }
            ViewBag.errors = errors;
            if (errors.Count > 0)
            {
                List<Category> liste = Category.LoadCategory();
                return View("CategoryAdd", liste);

            }
            else
            {
                c.AddCategory();
                ViewBag.Category = true;
                ViewBag.Message = "Catégorie ajoutée";
                List<Category> liste = Category.LoadCategory();
                return View("CategoryAdd", liste);
            }
        }

        public IActionResult DeleteCategory(int id)
        {
            UserConnect(ViewBag);
            Category c = new Category { Id = id };
            c.DeleteCategory();
            ViewBag.Category = true;
            ViewBag.Message = "Categorie Supprimée";
            List<Category> liste = Category.LoadCategory();
            return View("CategoryAdd", liste);
        }

    }
}