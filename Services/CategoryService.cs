using BusinessObjects;
using Repositories;

namespace Services
{
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository _repo = new CategoryRepository();

        public List<Category> GetCategories() => _repo.GetCategories();

        public void AddCategory(Category category)
        {
            // Kiểm tra trùng tên
            var existing = _repo.GetCategories();
            if (existing.Any(c => c.CategoryName.ToLower() == category.CategoryName.ToLower()))
                throw new Exception("Thể loại này đã tồn tại!");

            _repo.AddCategory(category);
        }

        public void UpdateCategory(Category category)
        {
            _repo.UpdateCategory(category);
        }

        public void DeleteCategory(int id)
        {
            _repo.DeleteCategory(id);
        }
    }
}
