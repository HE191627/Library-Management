using BusinessObjects;

namespace DataAccessLayer
{
    public class CategoryDAO
    {
        public static List<Category> GetCategories()
        {
            using var db = new LibraryEliteDBContext();
            return db.Categories.ToList();
        }

        public static Category? GetCategoryById(int id)
        {
            using var db = new LibraryEliteDBContext();
            return db.Categories.Find(id);
        }

        public static void AddCategory(Category category)
        {
            using var db = new LibraryEliteDBContext();
            db.Categories.Add(category);
            db.SaveChanges();
        }

        public static void UpdateCategory(Category category)
        {
            using var db = new LibraryEliteDBContext();
            db.Entry(category).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            db.SaveChanges();
        }

        public static void DeleteCategory(int id)
        {
            using var db = new LibraryEliteDBContext();
            var cat = db.Categories.Find(id);
            if (cat != null)
            {
                db.Categories.Remove(cat);
                db.SaveChanges();
            }
        }
    }
}
