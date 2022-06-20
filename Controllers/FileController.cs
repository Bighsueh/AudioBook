using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;
using System.Web.Mvc;
using AudioBook.Models;

namespace AudioBook.Controllers
{
    public class FileController : Controller
    {
        private DBTablePageContent dbPageContentManager = new DBTablePageContent();
        DBTableFiles dbFilesManager = new DBTableFiles();

        // GET: File
        public ActionResult Index()
        {
            List<table_files> table_files = dbFilesManager.GetTable();
            List<table_page_content> table_page_contents = dbPageContentManager.GetTable();

            ViewBag.table_files = table_files;
            ViewBag.table_page_content = table_page_contents;

            return View();
        }

        //新增檔案
        [HttpPost]
        public string CreateImageFile(string file_title, HttpPostedFileBase upload_file = null)
        {
            string status = ""; //程式運行狀態
            string file_type = "image"; //檔案類型
            try
            {
                /*
                 運作流程：
                 1.處理檔案(upload_image)上傳
                 2.將書籍名稱(book_title),書籍說明(book_content)及檔案名稱(fileName)加入資料庫
                 */

                string fileName = ""; //檔案名稱
                string localPath = "~/UploadFile/Files"; //檔案路徑

                /* 1.處理檔案(upload_image)上傳 */
                //若檔案為空
                if (upload_file == null)
                {
                    status = "fileIsNull";
                }

                //如果檔案不為空則上傳檔案
                if (upload_file != null)
                {
                    var fileValid = false; //驗證檔案合法性

                    //限制檔案大小小於10MB
                    if (upload_file.ContentLength <= 0 || upload_file.ContentLength > 10485760)
                    {
                        return "fileInValid";
                    }

                    //測試檔案類型為image/png,image/jpeg
                    if (upload_file.ContentType == "image/png" || upload_file.ContentType == "image/jpeg" ||
                        upload_file.ContentType == "image/jpg")
                    {
                        file_type = "image";
                        fileValid = true;
                    }

                    //若檔案合法
                    if (fileValid == true)
                    {
                        //將檔案儲存起來
                        string extension = Path.GetExtension(upload_file.FileName);
                        fileName = $"{Guid.NewGuid()}{extension}";
                        string savePath = Path.Combine(Server.MapPath(localPath), fileName);
                        upload_file.SaveAs(savePath);

                        /* 2.將書籍名稱(book_title),書籍說明(book_content)及檔案名稱(fileName)加入資料庫 */

                        int content_id = dbPageContentManager.InsertTable(file_title, "image");
                        dbFilesManager.InsertTable(content_id, fileName, file_title, file_type);
                    }

                    //檔案不合法
                    if (fileValid == false)
                    {
                        status = "fileInValid";
                    }
                }
            }
            catch (Exception ex)
            {
                //執行錯誤
                status = ex.ToString();
                Console.WriteLine(ex);
            }

            return status;
        }

        //新增音樂檔案
        [HttpPost]
        public string CreateAudioFile(string file_title, string audio_speed, HttpPostedFileBase slow_audio = null,
            HttpPostedFileBase english_audio = null, HttpPostedFileBase bilingual_audio = null)
        {
            string status = ""; //程式運行狀態
            string file_type = ""; //檔案類型
            try
            {
                string fileName_audio;
                string fileName_english_audio;
                string fileName_bilingual_audio;

                string localPath = "~/UploadFile/Files"; //檔案路徑

                /* 1.處理檔案(upload_image)上傳 */
                //若檔案為空
                if (slow_audio == null)
                {
                    status = "fileIsNull";
                }

                if (english_audio == null)
                {
                    status = "fileIsNull";
                }

                if (bilingual_audio == null)
                {
                    status = "fileIsNull";
                }

                //如果檔案不為空則上傳檔案
                if (slow_audio != null && english_audio != null && bilingual_audio != null)
                {
                    var fileValid = false; //驗證檔案合法性

                    //測試檔案類型為audio/wav,audio/mpeg
                    if (slow_audio.ContentType == "audio/wav" || slow_audio.ContentType == "audio/mpeg")
                    {
                        fileValid = true;
                    }

                    if (english_audio.ContentType == "audio/wav" || english_audio.ContentType == "audio/mpeg")
                    {
                        fileValid = true;
                    }

                    if (bilingual_audio.ContentType == "audio/wav" || bilingual_audio.ContentType == "audio/mpeg")
                    {
                        fileValid = true;
                    }


                    //若檔案合法
                    if (fileValid == true)
                    {
                        //將檔案儲存起來
                        string extension_slow_audio = Path.GetExtension(slow_audio.FileName);
                        string extension_english_audio = Path.GetExtension(english_audio.FileName);
                        string extension_bilingual_audio = Path.GetExtension(bilingual_audio.FileName);

                        fileName_audio = $"{Guid.NewGuid()}{extension_slow_audio}";
                        fileName_english_audio = $"{Guid.NewGuid()}{extension_english_audio}";
                        fileName_bilingual_audio = $"{Guid.NewGuid()}{extension_bilingual_audio}";

                        string savePath_slow_audio = Path.Combine(Server.MapPath(localPath), fileName_audio);
                        string savePath_english_audio = Path.Combine(Server.MapPath(localPath), fileName_english_audio);
                        string savePath_bilingual_audio =
                            Path.Combine(Server.MapPath(localPath), fileName_bilingual_audio);

                        slow_audio.SaveAs(savePath_slow_audio);
                        english_audio.SaveAs(savePath_english_audio);
                        bilingual_audio.SaveAs(savePath_bilingual_audio);

                        int content_id = dbPageContentManager.InsertTable(file_title, "audio");
                        dbFilesManager.InsertTable(content_id, savePath_slow_audio, file_title, "slow");
                        dbFilesManager.InsertTable(content_id, savePath_english_audio, file_title, "english");
                        dbFilesManager.InsertTable(content_id, savePath_bilingual_audio, file_title, "bilingual");

                        status = "success";
                    }

                    //檔案不合法
                    if (fileValid == false)
                    {
                        status = "fileInValid";
                    }
                }
            }
            catch (Exception ex)
            {
                //執行錯誤
                status = ex.ToString();
                Console.WriteLine(ex);
            }

            return status;
        }

        //取得編輯書籍資料
        [HttpPost]
        public ActionResult GetEditFile(int content_id)
        {
            List<table_page_content> table_page_contents = dbPageContentManager.GetTable(content_id);
            List<table_files> table_book = dbFilesManager.GetTable(content_id);

            string folder_path = "../UploadFile/Files";

            return Json(new
            {
                content_id = table_page_contents[0].content_id,
                content_name = table_page_contents[0].content_name,
                content_type = table_page_contents[0].content_type,
                img_path = String.Format("{0}/{1}", folder_path, table_book[0].local_path)
            });
        }

        //儲存修改圖片
        [HttpPost]
        public string StoreImageFile(int content_id, string content_name, HttpPostedFileBase upload_file = null)
        {
            string status = ""; //程式運行狀態
            string file_type = "image"; //檔案類型
            try
            {
                string fileName = ""; //檔案名稱
                string localPath = "~/UploadFile/Files"; //檔案路徑

                /* 1.處理檔案(upload_image)上傳 */
                //若檔案為空
                if (upload_file == null)
                {
                    status = "fileIsNull";
                }

                //如果檔案不為空則上傳檔案
                if (upload_file != null)
                {
                    var fileValid = false; //驗證檔案合法性

                    //限制檔案大小小於10MB
                    if (upload_file.ContentLength <= 0 || upload_file.ContentLength > 10485760)
                    {
                        return "fileInValid";
                    }

                    //測試檔案類型為image/png,image/jpeg
                    if (upload_file.ContentType == "image/png" || upload_file.ContentType == "image/jpeg" ||
                        upload_file.ContentType == "image/jpg")
                    {
                        file_type = "image";
                        fileValid = true;
                    }

                    //若檔案合法
                    if (fileValid == true)
                    {
                        //將檔案儲存起來
                        string extension = Path.GetExtension(upload_file.FileName);

                        fileName = $"{Guid.NewGuid()}{extension}";
                        string savePath = Path.Combine(Server.MapPath(localPath), fileName);
                        upload_file.SaveAs(savePath);
                        status = "success";

                        /* 2.將書籍名稱(book_title),書籍說明(book_content)及檔案名稱(fileName)加入資料庫 */
                        dbPageContentManager.UpdateTable(content_id, content_name, "image");
                        dbFilesManager.UpdateTable(content_id, content_name, file_type, fileName);
                        status = "UpdateTable";
                    }

                    //檔案不合法
                    if (fileValid == false)
                    {
                        status = "fileInValid";
                    }
                }
            }
            catch (Exception ex)
            {
                //執行錯誤
                status = ex.ToString();
                Console.WriteLine(ex);
            }

            return status;
        }

        //儲存修改音檔
        [HttpPost]
        public string StoreAudioFile(int content_id, string content_name, HttpPostedFileBase slow_audio = null,
            HttpPostedFileBase english_audio = null, HttpPostedFileBase bilingual_audio = null)
        {
            string status = ""; //程式運行狀態
            string file_type = ""; //檔案類型
            try
            {
                string fileName_audio;
                string fileName_english_audio;
                string fileName_bilingual_audio;

                string localPath = "~/UploadFile/Files"; //檔案路徑

                /* 1.處理檔案(upload_image)上傳 */
                //若檔案為空
                if (slow_audio == null)
                {
                    status = "fileIsNull";
                }

                if (english_audio == null)
                {
                    status = "fileIsNull";
                }

                if (bilingual_audio == null)
                {
                    status = "fileIsNull";
                }

                //如果檔案不為空則上傳檔案
                if (slow_audio != null && english_audio != null && bilingual_audio != null)
                {
                    var fileValid = false; //驗證檔案合法性

                    //測試檔案類型為audio/wav,audio/mpeg
                    if (slow_audio.ContentType == "audio/wav" || slow_audio.ContentType == "audio/mpeg")
                    {
                        fileValid = true;
                    }

                    if (english_audio.ContentType == "audio/wav" || english_audio.ContentType == "audio/mpeg")
                    {
                        fileValid = true;
                    }

                    if (bilingual_audio.ContentType == "audio/wav" || bilingual_audio.ContentType == "audio/mpeg")
                    {
                        fileValid = true;
                    }


                    //若檔案合法
                    if (fileValid == true)
                    {
                        //將檔案儲存起來
                        string extension_slow_audio = Path.GetExtension(slow_audio.FileName);
                        string extension_english_audio = Path.GetExtension(english_audio.FileName);
                        string extension_bilingual_audio = Path.GetExtension(bilingual_audio.FileName);

                        fileName_audio = $"{Guid.NewGuid()}{extension_slow_audio}";
                        fileName_english_audio = $"{Guid.NewGuid()}{extension_english_audio}";
                        fileName_bilingual_audio = $"{Guid.NewGuid()}{extension_bilingual_audio}";

                        string savePath_slow_audio = Path.Combine(Server.MapPath(localPath), fileName_audio);
                        string savePath_english_audio = Path.Combine(Server.MapPath(localPath), fileName_english_audio);
                        string savePath_bilingual_audio =
                            Path.Combine(Server.MapPath(localPath), fileName_bilingual_audio);

                        slow_audio.SaveAs(savePath_slow_audio);
                        english_audio.SaveAs(savePath_english_audio);
                        bilingual_audio.SaveAs(savePath_bilingual_audio);

                        dbPageContentManager.UpdateTable(content_id, content_name, "audio");
                        dbFilesManager.UpdateTable(content_id, content_name, "slow", savePath_slow_audio);
                        dbFilesManager.UpdateTable(content_id, content_name, "english", savePath_english_audio);
                        dbFilesManager.UpdateTable(content_id, content_name, "bilingual", savePath_bilingual_audio);

                        status = "success";
                    }

                    //檔案不合法
                    if (fileValid == false)
                    {
                        status = "fileInValid";
                    }
                }
            }
            catch (Exception ex)
            {
                //執行錯誤
                status = ex.ToString();
                Console.WriteLine(ex);
            }

            return status;
        }
        
        //刪除檔案
        [HttpPost]
        public string DeleteFile(int content_id)
        {
            dbPageContentManager.DeleteRow(content_id);
            dbFilesManager.DeleteRow(content_id);
            return "success";
        }
    }
}
