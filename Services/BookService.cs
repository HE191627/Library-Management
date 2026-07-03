using BusinessObjects;
using Repositories;

namespace Services
{
    public class BookService : IBookService
    {
        private readonly IBookRepository _repository = new BookRepository();

        public List<Book> GetBooks() => _repository.GetBooks();

        public Book? GetBookById(int id) => _repository.GetBookById(id);

        public void AddBook(Book book) => _repository.AddBook(book);

        public void UpdateBook(Book book) => _repository.UpdateBook(book);

        public void UpdateBookStatus(int bookId, string newStatus)
        {
            // LOGIC: Sách quý (giá > 1tr) thì không được báo "Mất" bừa bãi
            var book = _repository.GetBookById(bookId);
            if (book != null && book.Price > 1000000 && newStatus == "Lost")
            {
                throw new Exception("Sách giá trị cao cần biên bản kiểm kê, không thể cập nhật trực tiếp!");
            }
            _repository.UpdateBookStatus(bookId, newStatus);
        }

        public void DeleteBook(int bookId) => _repository.DeleteBook(bookId);

        public List<Category> GetCategories() => _repository.GetCategories();

        public List<Author> GetAuthors() => _repository.GetAuthors();
    }
}