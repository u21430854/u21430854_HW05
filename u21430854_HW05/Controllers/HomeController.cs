using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using u21430854_HW05.Models;
using u21430854_HW05.ViewModels;

namespace u21430854_HW05.Controllers
{
    public class HomeController : Controller
    {
        private DefaultDataService dataService = new DefaultDataService();

        public ActionResult Index()
        {
            DefaultVM vm = new DefaultVM();
            DefaultVM.bookAuthors = dataService.GetAuthors();
            DefaultVM.bookTypes = dataService.GetTypes();
            vm.books = dataService.GetBooks();

            return View(vm);
        }

        [HttpPost]
        public ActionResult SearchBooks(string bookName, string authorid, string typeid)
        {
            DefaultVM vm = new DefaultVM();
            vm.books = dataService.SearchBooks(bookName, typeid, authorid);

            if (!String.IsNullOrEmpty(bookName))
                ViewBag.SearchText = bookName;

            return View("Index", vm);
        }

        public ActionResult BookDetails(int bookId, bool borrowing = false, bool returning = false)
        {
            BookDetailsVM vm = new BookDetailsVM();
            vm.bookDetails = dataService.GetBorrows(bookId);
            vm.currentbook = dataService.GetCurrentBook(bookId);

            if (borrowing)
            {
                //borrow book
            }
            else if (returning)
            {
                //return book
            }

            return View(vm);
        }

        public ActionResult Students(int bookId)
        {
            StudentsVM vm = new StudentsVM();
            StudentsVM.classes = dataService.GetStudentClasses();
            vm.currentBook = dataService.GetCurrentBook(bookId);
            vm.students = dataService.GetStudents();

            return View(vm);
        }

        public ActionResult SearchStudents(string bookId, string name, string studentClass)
        {
            //get filtered list and current book
            StudentsVM vm = new StudentsVM();
            vm.currentBook = dataService.GetCurrentBook(Convert.ToInt32(bookId));
            vm.students = dataService.SearchStudents(name, studentClass);

            if (!String.IsNullOrEmpty(name))
                ViewBag.SearchText = name;

            return View("Students", vm);
        }
    }
}