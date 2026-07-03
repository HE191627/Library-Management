using BusinessObjects;
using DataAccessLayer;

namespace Repositories
{
    public class BookRepository : IBookRepository
    {
        public List<Book> GetBooks() => BookDAO.GetBooks();
        public Book? GetBookById(int id) => BookDAO.GetBookById(id);
        public void AddBook(Book book) => BookDAO.AddBook(book);
        public void UpdateBook(Book book) => BookDAO.UpdateBook(book);
        public void UpdateBookStatus(int bookId, string status) => BookDAO.UpdateBookStatus(bookId, status);
        public void DeleteBook(int id) => BookDAO.DeleteBook(id);
        public List<Category> GetCategories() => CategoryDAO.GetCategories();
        public List<Author> GetAuthors() => AuthorDAO.GetAuthors();
    }
}