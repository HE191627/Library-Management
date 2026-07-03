using System.Windows;
using System.Windows.Controls;
using BusinessObjects;
using Services;

namespace WpfApp
{
    public partial class CategoryPage : Page
    {
        private readonly ICategoryService _categoryService = new CategoryService();

        public CategoryPage()
        {
            InitializeComponent();
            LoadData();
        }

        private void LoadData()
        {
            dgCategories.ItemsSource = _categoryService.GetCategories();
        }

        private void dgCategories_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (dgCategories.SelectedItem is Category cat)
            {
                txtCategoryName.Text = cat.CategoryName;
                txtDescription.Text = cat.Description;
            }
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(txtCategoryName.Text))
                {
                    MessageBox.Show("Vui lòng nhập tên thể loại!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                var category = new Category
                {
                    CategoryName = txtCategoryName.Text.Trim(),
                    Description = txtDescription.Text.Trim()
                };

                _categoryService.AddCategory(category);
                MessageBox.Show("Thêm thể loại thành công!", "Thành công", MessageBoxButton.OK, MessageBoxImage.Information);
                LoadData();
                ClearForm();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void btnUpdate_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (dgCategories.SelectedItem is not Category selected)
                {
                    MessageBox.Show("Vui lòng chọn thể loại cần cập nhật!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                selected.CategoryName = txtCategoryName.Text.Trim();
                selected.Description = txtDescription.Text.Trim();
                selected.Books = new List<Book>();

                _categoryService.UpdateCategory(selected);
                MessageBox.Show("Cập nhật thành công!", "Thành công", MessageBoxButton.OK, MessageBoxImage.Information);
                LoadData();
                ClearForm();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (dgCategories.SelectedItem is not Category selected)
                {
                    MessageBox.Show("Vui lòng chọn thể loại cần xóa!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                var result = MessageBox.Show($"Bạn có chắc muốn xóa thể loại '{selected.CategoryName}'?\nCác sách thuộc thể loại này có thể bị ảnh hưởng!", 
                    "Xác nhận xóa", MessageBoxButton.YesNo, MessageBoxImage.Warning);

                if (result == MessageBoxResult.Yes)
                {
                    _categoryService.DeleteCategory(selected.CategoryId);
                    MessageBox.Show("Đã xóa thể loại!", "Thành công", MessageBoxButton.OK, MessageBoxImage.Information);
                    LoadData();
                    ClearForm();
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Không thể xóa thể loại này vì đang có sách liên kết!", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void btnClear_Click(object sender, RoutedEventArgs e)
        {
            ClearForm();
        }

        private void ClearForm()
        {
            txtCategoryName.Text = "";
            txtDescription.Text = "";
            dgCategories.SelectedItem = null;
        }
    }
}
