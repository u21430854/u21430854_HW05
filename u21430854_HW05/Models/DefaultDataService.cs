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
        SqlConnection connection;

        public List<Types> GetTypes()
        {
            List<Types> allTypes = new List<Types>();
            connection = new SqlConnection(connectionString);

            try
            {
                SqlCommand selectTypes = new SqlCommand("select * from types", connection);
                connection.Open();

                SqlDataReader readTypes = selectTypes.ExecuteReader();
                while(readTypes.Read())
                {
                    allTypes.Add(new Types()
                    {
                        id = Convert.ToInt32(readTypes["typeId"]),
                        name = readTypes["name"].ToString()
                    });
                }
            }
            catch (Exception)
            {
                throw;
            }
            finally
            { connection.Close(); }

            return allTypes;
        }

        public List<Authors> GetAuthors()
        {
            List<Authors> allAuthors = new List<Authors>();
            connection = new SqlConnection(connectionString);

            try
            {
                SqlCommand selectAuthors = new SqlCommand("select authorId, surname from authors", connection);
                connection.Open();

                SqlDataReader readAuthors = selectAuthors.ExecuteReader();
                while (readAuthors.Read())
                {
                    allAuthors.Add(new Authors()
                    {
                        id = Convert.ToInt32(readAuthors["authorId"]),
                        surname = readAuthors["surname"].ToString()
                    });
                }
            }
            catch (Exception)
            {
                throw;
            }
            finally
            { connection.Close(); }

            return allAuthors;
        }

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