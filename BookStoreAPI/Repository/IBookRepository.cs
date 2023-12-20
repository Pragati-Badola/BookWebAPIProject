using BookStoreAPI.DataTransferObjects;

namespace BookStoreAPI.Repository
{
    public interface IBookRepository
    {
        List<BookDto> GetAllBooks();

        BookDto GetBookById(int id);

        bool AddBook(Models.Book book);

        bool EditBook(int id, Models.Book book);

        bool DeleteBook(int id);
    }
}
