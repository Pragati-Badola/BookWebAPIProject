namespace BookStoreAPI.Repository
{
    public interface IBookRepository
    {
        List<Models.Book> GetAllBooks();

        Models.Book GetBookById(int id);

        bool AddBook(Models.Book book);
    }
}
