using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using u21430854_HW05.Models;

namespace u21430854_HW05.ViewModels
{
    public class BookDetailsVM
    {
        //for displaying book details on book details view        
        public List<Borrows> bookDetails { get; set; }
        //only need to set book id, name and status for 1st book
        public Books currentbook { get; set; }

        public BookDetailsVM()
        {
            currentbook = new Books();
        }
    }
}