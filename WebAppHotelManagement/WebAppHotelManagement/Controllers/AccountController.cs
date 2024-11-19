using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using WebAppHotelManagement.Models;

namespace WebAppHotelManagement.Controllers
{
    public class AccountController : Controller
    {
        // GET: Account
        [Authorize(Roles = ("Admin"))]
        public ActionResult Index()
        {
            using (OurDbContext db = new OurDbContext())
            {
                return View(db.userAccount.ToList());
            }
        }
        [Authorize(Roles = ("Admin"))]
        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [Authorize(Roles = ("Admin"))]
        public ActionResult Register(UserAccount account)
        {
            if (ModelState.IsValid)
            {
                using (OurDbContext db = new OurDbContext())
                {
                    db.userAccount.Add(account);
                    db.SaveChanges();
                }
                ModelState.Clear();
                ViewBag.Message = account.FirstName + " " + account.LastName + " successfully registered.";
            }
            return View();

        }

        //login
        [AllowAnonymous]
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [AllowAnonymous]
        public ActionResult Login(UserAccount user)
        {
            using (OurDbContext db = new OurDbContext())
            {
                try
                {
                    Session.RemoveAll();
                    if (user.UserName.Equals("admin") && user.Password.Equals("123"))
                    {
                        Session["UserName"] = "admin";
                        FormsAuthentication.SetAuthCookie("admin", false);
                        return RedirectToAction("Index", "Home");
                    }
                    else
                    {
                        var usr = db.userAccount.Single(u => u.UserName == user.UserName && u.Password == user.Password);

                        Session["UserId"] = usr.UserId.ToString();
                        Session["UserName"] = usr.UserName.ToString();
                        FormsAuthentication.SetAuthCookie(usr.UserName, false);
                        return RedirectToAction("Index", "Home");
                    }
                   
                }
                catch (Exception e)
                {
                    ModelState.AddModelError("", "Username or Password is wrong. Pls try again!");

                }
            }
            return View();
        }

        public ActionResult LoggedIn()
        {
            if (Session["UserId"] != null)
            {
                return View();
            }
            else
            {
                return RedirectToAction("Login");
            }
        }



        [HttpGet]
        [Authorize(Roles = ("Admin"))]
        public ActionResult Delete()
        {
            var readId = Request.QueryString["idUser"];
            var id = Convert.ToInt64(readId);
            using (OurDbContext db = new OurDbContext())
            {
                var user = db.userAccount.Find(id);


                return View(user);

            }

        }

        [HttpPost]
        [Authorize(Roles = ("Admin"))]
        public ActionResult Delete(int idUser)
        {

            using (OurDbContext db = new OurDbContext())
            {
                

                db.userAccount.Remove(db.userAccount.Find(idUser));
                db.SaveChanges();
                return RedirectToAction("Index");

            }
        }

        public ActionResult LogOff()
        {
            FormsAuthentication.SignOut();
            Session.Remove("UserName");
            Session.Remove("UserId");
            return RedirectToAction("Index", "Home");
        }




    }
}