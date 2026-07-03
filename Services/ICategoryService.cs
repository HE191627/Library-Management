using BusinessObjects;

namespace Services
{
    public interface ICategoryService
    {
        List<Category> GetCategories();
        void AddCategory(Category category);
        void UpdateCategory(Category category);
        void DeleteCategory(int id);
    }
}
