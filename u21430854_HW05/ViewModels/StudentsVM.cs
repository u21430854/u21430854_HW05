using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using u21430854_HW05.Models;

namespace u21430854_HW05.ViewModels
{
    public class StudentsVM
    {
        //only need to set id, name, points, status and lastBorrower
        public Books currentBook { get; set; }
        public List<Students> students { get; set; }
        public static List<string> classes { get; set; }

        public StudentsVM()
        {
            currentBook = new Books();
        }
    }
}