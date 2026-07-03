using BusinessObjects;

namespace Repositories
{
    public interface ICategoryRepository
    {
        List<Category> GetCategories();
        Category? GetCategoryById(int id);
        void AddCategory(Category category);
        void UpdateCategory(Category category);
        void DeleteCategory(int id);
    }
}
