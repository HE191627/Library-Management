using BusinessObjects;
using DataAccessLayer;

namespace Repositories
{
    public class CategoryRepository : ICategoryRepository
    {
        public List<Category> GetCategories() => CategoryDAO.GetCategories();
        public Category? GetCategoryById(int id) => CategoryDAO.GetCategoryById(id);
        public void AddCategory(Category category) => CategoryDAO.AddCategory(category);
        public void UpdateCategory(Category category) => CategoryDAO.UpdateCategory(category);
        public void DeleteCategory(int id) => CategoryDAO.DeleteCategory(id);
    }
}
