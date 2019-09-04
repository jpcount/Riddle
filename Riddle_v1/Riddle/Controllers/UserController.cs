using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Riddle.Models;
using System.IO;

namespace Riddle.Controllers
{
    public class UserController : Controller
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

        [Route("User")]
        public IActionResult UserListGet()
        {
            UserConnect(ViewBag);
            List<User> liste = new List<User>();
            //List<User> liste = User.GetUserList();
            return View("UserList", liste);
        }

        [Route("[Controller]/Login")]
        public IActionResult Login()
        {
            UserConnect(ViewBag);
            User u = new User();
            return View(u);
        }

        [HttpPost]
        public IActionResult LoginPost(string email, string password)
        {
            User u = new User { Email = email, Password = password };
            List<string> errors = new List<string>();
            if (email == null)
            {
                errors.Add("Merci de saisir un pseudo");
            }
            if (password == null)
            {
                errors.Add("Merci de saisir un mot de passe");
            }
            if (!u.UserLogin())
            {
                errors.Add("Il n'existe aucun utilisateur avec cet email et ce mot de passe");
            }
            if (errors.Count > 0)
            {
                ViewBag.errors = errors;
                return View("Login", u);
            }
            else
            {
                HttpContext.Session.SetString("logged", "true");
                HttpContext.Session.SetString("Name", u.Name);
                HttpContext.Session.SetString("FirstName", u.FirstName);
                HttpContext.Session.SetString("UserName", u.UserName);
                return RedirectToRoute(new { controller = "Riddle", action = "APropos" });
            }
        }

        [HttpPost]
        public IActionResult Register(string name, string firstName, string userName, string email, string phone, string password, string cPassword, string Admin)
        {
            List<string> errors = new List<string>();
            User u = new User { Name = name, FirstName = firstName, UserName = userName, Email = email, Phone = phone, Password = password, Admin = Admin };
            if (name == null)
            {
                errors.Add("Merci de saisir un nom");
            }
            if (firstName == null)
            {
                errors.Add("Merci de saisir un prénom");
            }
            if (userName == null)
            {
                errors.Add("Merci de saisir un pseudo");
            }
            if (email == null)
            {
                errors.Add("Merci de saisir un email");
            }
            if (phone == null)
            {
                errors.Add("Merci de saisir un numéro de téléphone");
            }
            if (password == null)
            {
                errors.Add("Merci de saisir un mot de passe");
            }
            if (password != cPassword)
            {
                errors.Add("Merci de saisir le même mot de passe");
            }
            if (u.UserExist())
            {
                errors.Add("Cette adresse email est déjà utilisée");
            }
            ViewBag.errors = errors;
            if (errors.Count > 0)
            {

            }
            else
            {
                u.AddUser();
                ViewBag.Inscription = true;
                ViewBag.Message = "Merci pour votre inscription, vous pouvez à présent vous connecter !";
            }
            return View("Login", u);
        }



        public IActionResult LogOut()
        {
            HttpContext.Session.Clear();
            return RedirectToRoute(new { controller = "Riddle", action = "APropos" });
        }


    }
}