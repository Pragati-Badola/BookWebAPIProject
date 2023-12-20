using AutoMapper;
using BookStoreAPI.DataTransferObjects;
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

        private readonly IMapper _mapper;
        
        public BookRepository(IConfiguration configuration, IMapper mapper)
        {
            _configuration = configuration;
            _mapper = mapper;
        }

        public List<BookDto> GetAllBooks()
        {
            List<BookDto> books = new List<BookDto>();

            using (SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
            {
                using(SqlCommand command = new SqlCommand())
                {
                    command.CommandText = "[dbo].[GetAllBooks]";
                    command.CommandType = CommandType.StoredProcedure;
                    command.Connection = connection;
                    connection.Open();                    
                    using(SqlDataReader reader = command.ExecuteReader())
                    {
                        if(reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                BookDto book = _mapper.Map<BookDto>(reader);
                                books.Add(book);
                            }
                        }
                    }
                }

            }

            return books;
        }

        public BookDto GetBookById(int id)
        {
            BookDto book = new BookDto();

            using (SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
            {
                using (SqlCommand command = new SqlCommand())
                {
                    command.CommandText = "[dbo].[GetBookById]";
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.Add("@ID", SqlDbType.Int).Value = id;
                    command.Connection = connection;
                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();
                    if(reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            book = _mapper.Map<BookDto>(reader);
                        }                  
                    }
                    connection.Close();
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

        public bool EditBook(int id, Book book)
        {
            using (SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
            {
                using (SqlCommand command = new SqlCommand())
                {
                    command.CommandText = "[dbo].[EditBook]";
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.Add("@Id", SqlDbType.Int, 50).Value = id;
                    command.Parameters.Add("@Title", SqlDbType.NVarChar, 50).Value = book.Title;
                    command.Parameters.Add("@Description", SqlDbType.NVarChar, int.MaxValue).Value = book.Description;
                    command.Parameters.Add("@IntStatus", SqlDbType.Int).Direction = ParameterDirection.Output;
                    command.Parameters.Add("@OutStatus", SqlDbType.NVarChar, 50).Direction = ParameterDirection.Output;
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
        public bool DeleteBook(int id)
        {
            using (SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
            {
                using (SqlCommand command = new SqlCommand())
                {
                    command.CommandText = "[dbo].[DeleteBook]";
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.Add("@Id", SqlDbType.Int, 50).Value = id;
                    command.Parameters.Add("@IntStatus", SqlDbType.Int).Direction = ParameterDirection.Output;
                    command.Parameters.Add("@OutStatus", SqlDbType.NVarChar, 50).Direction = ParameterDirection.Output;
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
