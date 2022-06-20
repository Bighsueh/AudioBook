using System;
using System.Web.Mvc;
using AudioBook.Models;

namespace AudioBook.Controllers
{
    public class LoginController : Controller
    {
        DBTableUser dbUserManager = new DBTableUser();

        // GET
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult LoginCheck(string account = "", string password = "")
        {
            if ((account != "") && (password != ""))
            {
                //check_admin
                Boolean admin_check = dbUserManager.AdminCheck(account, password);
                if (admin_check)
                {
                    return Redirect("~/Book/Index");
                }
            }

            return RedirectToAction("Index");
        }

        public ActionResult LogOut()
        {
            return RedirectToAction("Index");
        }
    }
}
