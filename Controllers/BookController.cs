using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.IO;
using AudioBook.Models;
using System.Data.SqlClient;

namespace AudioBook.Controllers
{
    public class BookController : Controller
    {
        DBTableBooks dbBookManager = new DBTableBooks();
        DBTablePages dbPagesManager = new DBTablePages();

        // 書籍列表GET: Index
        public ActionResult Index()
        {
            List<table_books> table_books = dbBookManager.GetTable();
            ViewBag.books = table_books;

            return View();
        }

        //新增書籍
        [HttpPost]
        public string CreateBook(string book_title, string book_content, HttpPostedFileBase upload_image = null)
        {
            string status = ""; //程式運行狀態
            try
            {
                /*
                 運作流程：
                 1.處理檔案(upload_image)上傳
                 2.將書籍名稱(book_title),書籍說明(book_content)及檔案名稱(fileName)加入資料庫
                 */

                string fileName = "";   //檔案名稱
                string localPath = "~/UploadFile/FrontCover";  //檔案路徑


                /* 1.處理檔案(upload_image)上傳 */
                //若檔案為空
                if (upload_image == null)
                {
                    status = "fileIsNull";
                }

                //如果檔案不為空則上傳檔案
                if (upload_image != null)
                {
                    var fileValid = true;   //驗證檔案合法性

                    //限制檔案大小大於5MB
                    if (upload_image.ContentLength <= 0 || upload_image.ContentLength > 5242880)
                    {
                        fileValid = false;
                    }
                    //限制檔案類型為image/png,image/jpeg
                    if (upload_image.ContentType != "image/png" && upload_image.ContentType != "image/jpeg")
                    {
                        fileValid = false;
                    }

                    //若檔案合法
                    if (fileValid == true)
                    {
                        //將檔案儲存起來
                        string extension = Path.GetExtension(upload_image.FileName);
                        fileName = $"{Guid.NewGuid()}{extension}";
                        string savePath = Path.Combine(Server.MapPath(localPath), fileName);
                        upload_image.SaveAs(savePath);
                        status = "success";
                    }

                    //檔案不合法
                    if (fileValid == false)
                    {
                        status = "fileInValid";
                    }
                }


                /* 2.將書籍名稱(book_title),書籍說明(book_content)及檔案名稱(fileName)加入資料庫 */
                string sql_return = dbBookManager.InsertTable(book_title, book_content, fileName);
                return sql_return;

            }
            catch (Exception ex)
            {
                //執行錯誤
                status = "error";
                Console.WriteLine(ex);

            }
            return status;
        }

        //取得編輯書籍資料
        [HttpPost]
        public ActionResult GetEditBook(int book_id)
        {
            List<table_books> table_book = dbBookManager.GetTable(book_id);

            string folder_path = "../UploadFile/FrontCover";

            return Json(new
            {
                book_id = table_book[0].book_id,
                book_title = table_book[0].book_title,
                book_content = table_book[0].book_content,
                front_cover = String.Format("{0}/{1}", folder_path, table_book[0].front_cover)
            });
        }

        //儲存編輯書籍資料
        [HttpPost]
        public string StoreEditBook(int book_id, string book_title, string book_content, HttpPostedFileBase upload_image = null)
        {
            string status = ""; //程式運行狀態
            try
            {
                /*
                 運作流程：
                 1.處理檔案(upload_image)上傳
                 2.將書籍名稱(book_title),書籍說明(book_content)及檔案名稱(fileName)加入資料庫
                 */

                string fileName = "none";   //檔案名稱
                string localPath = "~/UploadFile/FrontCover";  //檔案路徑


                /* 1.處理檔案(upload_image)上傳 */
                //若檔案為空
                if (upload_image == null)
                {
                    status = "fileIsNull";
                }

                //如果檔案不為空則上傳檔案
                if (upload_image != null)
                {
                    var fileValid = true;   //驗證檔案合法性

                    //限制檔案大小大於5MB
                    if (upload_image.ContentLength <= 0 || upload_image.ContentLength > 5242880)
                    {
                        fileValid = false;
                    }
                    //限制檔案類型為image/png,image/jpeg
                    if (upload_image.ContentType != "image/png" && upload_image.ContentType != "image/jpeg")
                    {
                        fileValid = false;
                    }

                    //若檔案合法
                    if (fileValid == true)
                    {
                        //將檔案儲存起來
                        string extension = Path.GetExtension(upload_image.FileName);
                        fileName = $"{Guid.NewGuid()}{extension}";
                        string savePath = Path.Combine(Server.MapPath(localPath), fileName);
                        upload_image.SaveAs(savePath);
                        status = "success";
                    }

                    //檔案不合法
                    if (fileValid == false)
                    {
                        status = "fileInValid";
                    }
                }


                /* 2.將書籍名稱(book_title),書籍說明(book_content)及檔案名稱(fileName)加入資料庫 */
                string sql_return = dbBookManager.UpdateTable(book_id, book_title, book_content, fileName);
                return sql_return;

            }
            catch (Exception ex)
            {
                //執行錯誤
                status = "error";
                Console.WriteLine(ex);

            }
            return status;
        }


        //刪除書籍
        [HttpPost]
        public string DeleteBook(int book_id)
        {
            string sql_exc = dbBookManager.DeleteRow(book_id);
            return sql_exc;
        }

        public ActionResult Page(int page_id = 0)
        {
            return RedirectToAction("Index");
        }

        //書籍頁面列表 GET: Book
        public ActionResult PageList(int book_id = 0)
        {
            if (book_id == 0)
            {
                return RedirectToAction("Index");
            }
            ViewBag.bookId = book_id;

            List<table_pages> table_pages = dbPagesManager.GetTable(book_id);
            ViewBag.pages = table_pages;

            List<table_books> table_books = dbBookManager.GetTable(book_id);
            ViewBag.books = table_books;


            return View();
        }

        public string CreatePage(int book_id)
        {
            string default_image_path = "../Content/image/empty_page.jpg";
            string sql_exc = dbPagesManager.InsertTable(book_id,default_image_path);

            return sql_exc;
        }
        
        //開啟詳細頁面設定頁面
        public ActionResult PageDetail(int page_id)
        {
            
            return View();
        }
    }
}