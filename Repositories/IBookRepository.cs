using BusinessObjects;

namespace Repositories
{
    public interface IBookRepository
    {
        List<Book> GetBooks();
        Book? GetBookById(int id);
        void AddBook(Book book);
        void UpdateBook(Book book);
        void UpdateBookStatus(int bookId, string status);
        void DeleteBook(int id);
        List<Category> GetCategories();
        List<Author> GetAuthors();
    }
}