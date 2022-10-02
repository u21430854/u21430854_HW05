using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace u21430854_HW05.Models
{
    public class Borrows
    {
        public int id { get; set; }
        //only need to set student name + surname
        public Students student { get; set; }
        public DateTime takenDate { get; set; }
        public string broughtDate { get; set; }

        public Borrows()
        {
            student = new Students();
        }

    }
}