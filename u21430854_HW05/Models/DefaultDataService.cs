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
                    SetBookStatus(book.id, out string status, out int studentId);
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
                    SetBookStatus(book.id, out string status, out int studentId);
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
                string borrowsQuery = "select borrowId, takenDate, broughtDate, students.name as studentName, surname " +
                                       "from borrows INNER JOIN students " +
                                       "ON borrows.studentId = students.studentId " +
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
                    borrowed.takenDate = Convert.ToDateTime(readBorrows["takenDate"]);
                    if (readBorrows["broughtDate"] == null)
                    { borrowed.broughtDate = ""; }
                    else
                    { borrowed.broughtDate = readBorrows["broughtDate"].ToString(); }                    
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

        public List<string> GetStudentClasses()
        {
            List<string> studentclasses = new List<string>();
            connection = new SqlConnection(connectionString);

            try
            {
                string classesQuery = "SELECT DISTINCT class FROM students " +
                                      "ORDER BY class";

                SqlCommand selectClasses = new SqlCommand(classesQuery, connection);
                connection.Open();

                SqlDataReader readClasses = selectClasses.ExecuteReader();
                while (readClasses.Read())
                {
                    studentclasses.Add(readClasses["class"].ToString());
                }
            }
            catch (Exception)
            {
                throw;
            }
            finally
            { connection.Close(); }

            return studentclasses;
        }

        public List<Students> GetStudents()
        {
            List<Students> students = new List<Students>();
            connection = new SqlConnection(connectionString);

            try
            {
                SqlCommand selectStudents = new SqlCommand("SELECT studentId, name, surname, class, point FROM students", connection);
                connection.Open();

                SqlDataReader readStudents = selectStudents.ExecuteReader();
                while (readStudents.Read())
                {
                    Students student = new Students();
                    student.id = Convert.ToInt32(readStudents["studentId"]);
                    student.name = readStudents["name"].ToString();
                    student.surname = readStudents["surname"].ToString();
                    student.studentClass = readStudents["class"].ToString();
                    student.point = Convert.ToInt32(readStudents["point"]);
                    students.Add(student);
                }
            }
            catch (Exception)
            {
                throw;
            }
            finally
            { connection.Close(); }

            return students;
        }

        public List<Students> SearchStudents(string name, string studentClass)
        {
            List<Students> filteredStudents = new List<Students>();
            connection = new SqlConnection(connectionString);

            try
            {
                string studentQuery = "SELECT studentId, name, surname, class, point FROM students " +
                    "WHERE name LIKE '%" + name + "%' AND CLASS LIKE '%" + studentClass + "%'";

                SqlCommand selectStud = new SqlCommand(studentQuery, connection);
                connection.Open();

                SqlDataReader readStud = selectStud.ExecuteReader();
                while (readStud.Read())
                {
                    Students student = new Students();
                    student.id = Convert.ToInt32(readStud["studentId"]);
                    student.name = readStud["name"].ToString();
                    student.surname = readStud["surname"].ToString();
                    student.studentClass = readStud["class"].ToString();
                    student.point = Convert.ToInt32(readStud["point"]);
                    filteredStudents.Add(student);
                }
            }
            catch (Exception)
            {
                throw;
            }
            finally
            { connection.Close(); }

            return filteredStudents;
        }

        public Books GetCurrentBook(int bookId)
        {
            Books book = new Books();
            connection = new SqlConnection(connectionString);

            try
            {
                SqlCommand selectBook = new SqlCommand("SELECT bookId, name FROM books WHERE bookId = " + bookId, connection);
                connection.Open();

                SqlDataReader readBook = selectBook.ExecuteReader();
                while (readBook.Read())
                {
                    book.id = Convert.ToInt32(readBook["bookId"]);
                    book.name = readBook["name"].ToString();
                    //get book status and last borrower
                    SetBookStatus(bookId, out string status, out int studentId);
                    book.status = status;
                    book.lastBorrower = studentId;
                }
            }
            catch (Exception)
            {
                throw;
            }
            finally
            { connection.Close(); }

            return book;
        }

        public void BorrowBook(int studentId, int bookId)
        {
            /*
             INSERT INTO borrows (studentId, bookId, takenDate) 
            VALUES (236, 91, CAST(N'2015-10-27T08:59:00.000' AS DateTime))
             */
            connection = new SqlConnection(connectionString);

            try
            {
                string insert = "INSERT INTO borrows (studentId, bookId, takenDate) " +
                                "VALUES(" + studentId + ", " + bookId + ", CAST(N'" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff") + "' AS DateTime))";

                SqlCommand newBorrow = new SqlCommand(insert, connection);
                connection.Open();

                newBorrow.ExecuteNonQuery();
            }
            catch (Exception)
            {
                throw;
            }
            finally
            { connection.Close(); }
        }

        public void ReturnBook(int bookId)
        {
            connection = new SqlConnection(connectionString);

            try
            {
                //sp_ReturnBook is a stored procedure that takes bookId as input, uses it to find which row in the borrows
                //table needs to be updated and updates the broughtDate value of that record
                //create command object for stored procedure
                SqlCommand storedProc = new SqlCommand("sp_ReturnBook", connection);
                storedProc.CommandType = System.Data.CommandType.StoredProcedure;

                //add input (book id) parameter to stored procedure and execute
                storedProc.Parameters.AddWithValue("@BookId", bookId);
                connection.Open();

                //execute
                storedProc.ExecuteNonQuery();
            }
            catch (Exception)
            {
                throw;
            }
            finally
            { connection.Close(); }

        }
    }
}