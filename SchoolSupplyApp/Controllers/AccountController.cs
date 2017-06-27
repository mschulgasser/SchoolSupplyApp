using SchoolSupplyApp.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace SchoolSupplyApp.Controllers
{
    public class AccountController : Controller
    {
        // GET: Account
        public ActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Login(string email, string password)
        {
            var repo = new AccountRepository(Properties.Settings.Default.ConStr);
            School school = repo.Login(email, password);
            if (school != null)
            {
                FormsAuthentication.SetAuthCookie(email, true);
                return Redirect("/admin/index");
            }
            return Redirect("/account/login");
        }
        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            return Redirect("/");
        }
    }
}