using BookStoreAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Input;
using static System.Reflection.Metadata.BlobBuilder;

namespace BookStoreAPI.Repository
{
    public class BookRepository : IBookRepository
    {
        private readonly IConfiguration _configuration;
        
        public BookRepository(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public List<Book> GetAllBooks()
        {
            List<Book> books = new List<Book>();

            using (SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
            {
                using(SqlCommand command = new SqlCommand())
                {
                    command.CommandText = "[dbo].[GetAllBooks]";
                    command.CommandType = CommandType.StoredProcedure;
                    command.Connection = connection;
                    connection.Open();
                    object data;
                    
                    using(SqlDataReader reader = command.ExecuteReader())
                    {
                        if(reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                var book = new Book();
                                data = reader.GetValue(reader.GetOrdinal(nameof(Book.Id)));
                                if (data != null)
                                {
                                    book.Id =  (int)data;
                                }
                                data = reader.GetValue(reader.GetOrdinal(nameof(Book.Title)));
                                if( data != null)
                                {
                                    book.Title = data.ToString();
                                }
                                data = reader.GetValue(reader.GetOrdinal(nameof(Book.Description)));
                                if (data != null)
                                {
                                    book.Description = data.ToString();
                                }
                                books.Add(book);
                            }
                        }
                    }
                }

            }

            return books;
        }

        public Book GetBookById(int id)
        {
            Book book = new Book();

            using (SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
            {
                using (SqlCommand command = new SqlCommand())
                {
                    command.CommandText = "[dbo].[GetBookById]";
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.Add("@ID", SqlDbType.Int).Value = id;
                    command.Connection = connection;
                    connection.Open();
                    object data;
                    SqlDataReader reader = command.ExecuteReader();
                    if(reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            data = reader.GetValue(reader.GetOrdinal(nameof(Book.Id)));
                            if (data != null)
                            {
                                book.Id = (int)data;
                            }
                            data = reader.GetValue(reader.GetOrdinal(nameof(Book.Title)));
                            if (data != null)
                            {
                                book.Title = data.ToString();
                            }

                            data = reader.GetValue(reader.GetOrdinal(nameof(Book.Description)));
                            if (data != null)
                            {
                                book.Description = data.ToString();
                            }
                        }                  
                    }
                }

            }

            return book;
        }

        public bool AddBook(Book book)
        {
            using (SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
            {
                using (SqlCommand command = new SqlCommand())
                {
                    command.CommandText = "[dbo].[AddNewBook]";
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.Add("@Title", SqlDbType.NVarChar,50).Value = book.Title;
                    command.Parameters.Add("@Description", SqlDbType.NVarChar,int.MaxValue).Value= book.Description;
                    command.Parameters.Add("@IntStatus", SqlDbType.Int).Direction = ParameterDirection.Output;
                    command.Parameters.Add("@OutStatus", SqlDbType.NVarChar,50).Direction = ParameterDirection.Output;
                    command.Connection = connection;
                    connection.Open();
                    command.ExecuteNonQuery();
                    string outStatus = command.Parameters["@OutStatus"].Value as string;
                    int intStatus = (int)command.Parameters["@IntStatus"].Value;
                    
                    connection.Close();

                    return intStatus > 0;
                }

            }

        }
    }
}
