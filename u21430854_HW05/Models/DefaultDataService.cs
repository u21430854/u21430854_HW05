using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;

namespace u21430854_HW05.Models
{
    public class DefaultDataService
    {
        public static string connectionString = @"Data Source=.\SQLEXPRESS;Initial Catalog=Library;Integrated Security=True";
        SqlConnection connection = new SqlConnection(connectionString);

        public List<Books> GetBooks() 
        {
            List<Books> allBooks = new List<Books>();

            return allBooks;
        }

        public List<Books> SearchBooks(string book, string type, string author)
        {
            List<Books> filteredBooks = new List<Books>();

            return filteredBooks;
        }

        public List<Borrows> GetBorrows(int bookId)
        {
            List<Borrows> allBorrows = new List<Borrows>();

            return allBorrows;
        }

        public List<Students> GetStudents(int bookId)
        {
            List<Students> students = new List<Students>();

            return students;
        }

        public void BorrowBook(int bookId)
        {

        }

        public void ReturnBook(int bookId)
        {

        }
    }
}