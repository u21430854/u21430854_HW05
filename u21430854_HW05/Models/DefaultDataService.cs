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
            connection = new SqlConnection(connectionString);

            try
            {
                string bookQuery = "select b.bookId as bookId, b.name as bookName, b.pagecount as pageCount, " +
                                    "b.point as points, a.authorId as authorId, a.surname as authorSurname, " +
                                    "t.typeId as typeId, t.name as typeName from books b " +
                                    "INNER JOIN authors a ON b.authorId = a.authorId " +
                                    "INNER JOIN types t ON b.typeId = t.typeId";
                SqlCommand selectBooks = new SqlCommand(bookQuery, connection);

                connection.Open();

                SqlDataReader readBooks = selectBooks.ExecuteReader();
                while(readBooks.Read())
                {
                    Books book = new Books();
                    book.id = Convert.ToInt32(readBooks["bookId"]);
                    book.name = readBooks["bookName"].ToString();
                    book.pageCount = Convert.ToInt32(readBooks["pageCount"]);
                    book.point = Convert.ToInt32(readBooks["points"]);
                    book.author = new Authors()
                    {
                        id = Convert.ToInt32(readBooks["authorId"]),
                        surname = readBooks["authorSurname"].ToString()
                    };
                    book.genre = new Types()
                    {
                        id = Convert.ToInt32(readBooks["typeId"]),
                        name = readBooks["typeName"].ToString()
                    };

                    //get book status
                    string status;
                    int studentId;
                    SetBookStatus(book.id, out status, out studentId);
                    book.status = status;
                    book.lastBorrower = studentId;

                    allBooks.Add(book);
                }
            }
            catch (Exception)
            {
                throw;
            }
            finally
            { connection.Close(); }

            return allBooks;
        }

        public void SetBookStatus(int id, out string status, out int studentId)
        {
            connection = new SqlConnection(connectionString);

            try
            {
                //book status is determined using a stored procedure that accepts book Id and
                //returns 'available' or 'out' and the student id of the last person to borrow the book
                //create command object for stored procedure
                SqlCommand storedProc = new SqlCommand("sp_BookStatus", connection);
                storedProc.CommandType = System.Data.CommandType.StoredProcedure;

                //add input (book id) parameter to stored procedure and execute
                storedProc.Parameters.AddWithValue("@BookId", id);

                //add output parameters to stored procedure
                storedProc.Parameters.Add("@BookStatus", System.Data.SqlDbType.VarChar, 50);
                storedProc.Parameters["@BookStatus"].Direction = System.Data.ParameterDirection.Output;
                storedProc.Parameters.Add("@LastBorrower", System.Data.SqlDbType.Int);
                storedProc.Parameters["@LastBorrower"].Direction = System.Data.ParameterDirection.Output;
                connection.Open();
                                
                //execute
                storedProc.ExecuteNonQuery();

                //store output parameters
                status = storedProc.Parameters["@BookStatus"].Value.ToString();
                studentId = Convert.ToInt32(storedProc.Parameters["@LastBorrower"].Value);
            }
            catch (Exception)
            {
                throw;
            }
            finally
            { connection.Close(); }
        }

        public List<Books> SearchBooks(string bookname, string typeid, string authorid)
        {
            List<Books> filteredBooks = new List<Books>();
            connection = new SqlConnection(connectionString);

            try
            {
                string bookQuery = "select b.bookId as bookId, b.name as bookName, b.pagecount as pageCount, " +
                                    "b.point as points, a.authorId as authorId, a.surname as authorSurname, " +
                                    "t.typeId as typeId, t.name as typeName from books b " +
                                    "INNER JOIN authors a ON b.authorId = a.authorId " +
                                    "INNER JOIN types t ON b.typeId = t.typeId " +
                                    "WHERE b.name LIKE '%" + bookname + "%'";

                if (!String.IsNullOrEmpty(typeid))
                {
                    bookQuery += " AND t.typeId = " + typeid;
                }
                if (!String.IsNullOrEmpty(authorid))
                {
                    bookQuery += " AND a.authorId = " + authorid;
                }

                SqlCommand selectBooks = new SqlCommand(bookQuery, connection);
                connection.Open();

                SqlDataReader readBooks = selectBooks.ExecuteReader();
                while (readBooks.Read())
                {
                    Books book = new Books();
                    book.id = Convert.ToInt32(readBooks["bookId"]);
                    book.name = readBooks["bookName"].ToString();
                    book.pageCount = Convert.ToInt32(readBooks["pageCount"]);
                    book.point = Convert.ToInt32(readBooks["points"]);
                    book.author = new Authors()
                    {
                        id = Convert.ToInt32(readBooks["authorId"]),
                        surname = readBooks["authorSurname"].ToString()
                    };
                    book.genre = new Types()
                    {
                        id = Convert.ToInt32(readBooks["typeId"]),
                        name = readBooks["typeName"].ToString()
                    };

                    //get book status
                    string status;
                    int studentId;
                    SetBookStatus(book.id, out status, out studentId);
                    book.status = status;
                    book.lastBorrower = studentId;

                    filteredBooks.Add(book);
                }
            }
            catch (Exception)
            {
                throw;
            }
            finally
            { connection.Close(); }

            return filteredBooks;
        }

        public List<Borrows> GetBorrows(int bookId)
        {
            List<Borrows> allBorrows = new List<Borrows>();
            connection = new SqlConnection(connectionString);

            try
            {
                string borrowsQuery = "select borrowId, takenDate, broughtDate, students.name as studentName, surname, " +
                                       "books.name as bookName " +
                                       "from borrows INNER JOIN students " +
                                       "ON borrows.studentId = students.studentId " +
                                       "INNER JOIN books ON borrows.bookId = books.bookId " +
                                       "WHERE borrows.bookId = " + bookId +
                                       " ORDER BY borrowId desc";

                SqlCommand selectBorrows = new SqlCommand(borrowsQuery, connection);
                connection.Open();

                SqlDataReader readBorrows = selectBorrows.ExecuteReader();
                int i = 0;
                while (readBorrows.Read())
                {
                    i++;
                    Borrows borrowed = new Borrows();
                    borrowed.id = Convert.ToInt32(readBorrows["borrowId"]);
                    borrowed.student.name = readBorrows["studentName"].ToString();
                    borrowed.student.surname = readBorrows["surname"].ToString();
                    borrowed.book.id = bookId;
                    borrowed.book.name = readBorrows["bookName"].ToString();

                    //I don't want to have to set book status more than 1ce because it has to read from the database again,
                    //call a stored procedure, etc. It's a whole process
                    if (i == 1)
                    {
                        //get book status
                        string status;
                        int studentId;
                        SetBookStatus(bookId, out status, out studentId);
                        borrowed.book.status = status;
                        borrowed.book.lastBorrower = studentId;
                    }

                    borrowed.takenDate = Convert.ToDateTime(readBorrows["takenDate"]);
                    borrowed.broughtDate = Convert.ToDateTime(readBorrows["broughtDate"]);
                    allBorrows.Add(borrowed);
                }
            }
            catch (Exception)
            {
                throw;
            }
            finally
            { connection.Close(); }

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