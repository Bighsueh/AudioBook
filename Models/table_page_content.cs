using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AudioBook.Models
{
    public class table_page_content
    {
        public int content_id { get; set; }
        public string content_name { get; set; }
        public string content_type { get; set; }
        public Guid content_code { get; set; }
    }
}
