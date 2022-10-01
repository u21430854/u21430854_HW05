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
        public ActionResult Index(string bookName, string author, string type)
        {
            return View();
        }

        public ActionResult BookDetails()
        {
            return View();
        }

        public ActionResult Students()
        {
            return View();
        }
    }
}