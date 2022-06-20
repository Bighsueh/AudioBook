using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AudioBook.Models
{
    public class table_pages
    {
        public int page_id { get; set; }
        public int page_num { get; set; }
        public int book_id { get; set; }
        public string image_path { get; set; }
        public string page_title { get; set; }
        public string page_content { get; set; }
    }
}