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

        //新增群組
        [HttpPost]
        public string CreateGroup(string group_name, string group_content)
        {
            string sql_exc = dbUserManager.CreateGroup(group_name, group_content);
            return sql_exc;
        }

        //取得欲修改資料
        [HttpPost]
        public ActionResult GetGroupInfo(int group_id)
        {
            List<list_user_group> user_group = dbUserManager.GetGroupInfo(group_id);

            return Json(new
            {
                group_id = user_group[0].group_id,
                group_name = user_group[0].group_name,
                group_content = user_group[0].group_content,
            });
        }

        //儲存欲修改資料
        [HttpPost]
        public string StoreGroupInfo(int group_id, string group_name, string group_content)
        {
            string sql_exc = dbUserManager.UpdateGroupInfo(group_id, group_name, group_content);
            return sql_exc;
        }

        //刪除群組資料School
        [HttpPost]
        public string DeleteGroup(int group_id)
        {
            string sql_exc = dbUserManager.DeleteRow(group_id);
            return sql_exc;
        }
    }
}
