using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace u21430854_HW05.Models
{
    public class Books
    {
        public int id { get; set; }
        public string name { get; set; }
        public int pageCount { get; set; }
        public int point { get; set; }
        //only need to set author surname
        public Authors author { get; set; }
        public Types genre { get; set; }
        public string status { get; set; }
    }
}