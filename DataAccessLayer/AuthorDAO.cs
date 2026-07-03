using BusinessObjects;

namespace DataAccessLayer
{
    public class AuthorDAO
    {
        public static List<Author> GetAuthors()
        {
            using var db = new LibraryEliteDBContext();
            return db.Authors.ToList();
        }
    }
}
