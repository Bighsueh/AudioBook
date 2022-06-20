using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AudioBook.Models
{
    public class table_files
    {
        public int file_id { get; set; }
        public int content_id { get; set; }
        public string local_path { get; set; }
        public string file_title { get; set; }
        public string file_type { get; set; }
        public Guid file_code { get; set; }

    }
}