using BusinessObjects;
using Microsoft.EntityFrameworkCore;

namespace DataAccessLayer
{
    public class BookDAO
    {
        public static List<Book> GetBooks()
        {
            using var db = new LibraryEliteDBContext();
            return db.Books.Include(b => b.Author).Include(b => b.Category).ToList();
        }

        public static Book? GetBookById(int id)
        {
            using var db = new LibraryEliteDBContext();
            return db.Books.Include(b => b.Author).Include(b => b.Category).FirstOrDefault(b => b.BookId == id);
        }

        public static void AddBook(Book book)
        {
            using var db = new LibraryEliteDBContext();
            db.Books.Add(book);
            db.SaveChanges();
        }

        public static void UpdateBook(Book book)
        {
            using var db = new LibraryEliteDBContext();
            db.Entry(book).State = EntityState.Modified;
            db.SaveChanges();
        }

        public static void UpdateBookStatus(int bookId, string status)
        {
            using var db = new LibraryEliteDBContext();
            var book = db.Books.Find(bookId);
            if (book != null)
            {
                book.Status = status;
                db.SaveChanges();
            }
        }

        public static void DeleteBook(int id)
        {
            using var db = new LibraryEliteDBContext();
            var book = db.Books.Find(id);
            if (book != null)
            {
                db.Books.Remove(book);
                db.SaveChanges();
            }
        }
    }
}
