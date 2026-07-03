using BusinessObjects;

namespace Services
{
    public interface IBookService
    {
        List<Book> GetBooks();
        Book? GetBookById(int id);
        void AddBook(Book book);
        void UpdateBook(Book book);
        void UpdateBookStatus(int bookId, string newStatus);
        void DeleteBook(int bookId);
        List<Category> GetCategories();
        List<Author> GetAuthors();
    }
}