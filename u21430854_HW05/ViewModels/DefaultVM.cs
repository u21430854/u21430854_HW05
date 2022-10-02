using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using u21430854_HW05.Models;
using u21430854_HW05.ViewModels;

namespace u21430854_HW05.ViewModels
{
    public class DefaultVM
    {
        public static List<Types> bookTypes { get; set; }

        public static List<Authors> bookAuthors { get; set; }

        //for displaying books on index view
        public List<Books> books { get; set; }        
    }
}