using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AudioBook.Models
{
    public class table_books
    {
        public int book_id { get; set; }
        public string book_title { get; set; }
        public string book_content { get; set; }
        public string front_cover { get; set; }
    }
}