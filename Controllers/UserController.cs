using System.Collections.Generic;
using System.Web.Mvc;
using AudioBook.Models;

namespace AudioBook.Controllers
{
    public class UserController : Controller
    {
        DBTableUser dbUserManager = new DBTableUser();

        // GET
        public ActionResult GroupList()
        {
            List<list_user_group> list_user_group = dbUserManager.ListUserGroups();

            ViewBag.listgroup = list_user_group;
            return View();
        }

        public ActionResult GroupDetail(int group_id, string group_name)
        {
            string group_type = "admin";
            if (group_name != "admin")
            {
                group_type = "school";
            }

            List<list_users> list_users = dbUserManager.ListUsers(group_id, group_type);

            ViewBag.listUsers = list_users;
            ViewBag.groupName = group_name;
            return View();
        }
    }
}
