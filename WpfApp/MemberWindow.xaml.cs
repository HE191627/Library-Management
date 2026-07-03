using System.Windows;
using System.Windows.Controls;
using BusinessObjects;
using Services;

namespace WpfApp
{
    public partial class MemberWindow : Window
    {
        private readonly IBookService _bookService = new BookService();
        private List<Book> _availableBooks = new();

        public MemberWindow()
        {
            InitializeComponent();

            if (App.CurrentAccount?.Member != null)
            {
                txtWelcome.Text = $"Xin chào, {App.CurrentAccount.Member.FullName}!";
            }
            else
            {
                txtWelcome.Text = "Xin chào, Thành viên!";
            }

            LoadData();
            LoadCategoryFilter();
        }

        private void LoadData()
        {
            var allBooks = _bookService.GetBooks();
            _availableBooks = allBooks.Where(b => b.Status == "Available" && b.Quantity > 0).ToList();
            dgAvailableBooks.ItemsSource = _availableBooks;
        }

        private void LoadCategoryFilter()
        {
            var categories = _bookService.GetCategories();
            var filterList = new List<Category> { new Category { CategoryId = 0, CategoryName = "-- Tất cả thể loại --" } };
            filterList.AddRange(categories);
            cbCategoryFilter.ItemsSource = filterList;
            cbCategoryFilter.SelectedIndex = 0;
        }

        private void txtSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            ApplyFilter();
        }

        private void cbCategoryFilter_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ApplyFilter();
        }

        private void ApplyFilter()
        {
            if (_availableBooks == null) return;

            var filtered = _availableBooks.AsEnumerable();

            // Tìm kiếm theo tên sách / tác giả
            if (!string.IsNullOrWhiteSpace(txtSearch.Text))
            {
                var keyword = txtSearch.Text.ToLower();
                filtered = filtered.Where(b =>
                    (b.Title?.ToLower().Contains(keyword) == true) ||
                    (b.Author?.AuthorName?.ToLower().Contains(keyword) == true));
            }

            // Lọc theo thể loại
            if (cbCategoryFilter.SelectedItem is Category selectedCat && selectedCat.CategoryId != 0)
            {
                filtered = filtered.Where(b => b.CategoryId == selectedCat.CategoryId);
            }

            dgAvailableBooks.ItemsSource = filtered.ToList();
        }

        private void Logout_Click(object sender, RoutedEventArgs e)
        {
            var result = MessageBox.Show("Bạn có chắc muốn đăng xuất?", "Xác nhận",
                MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result == MessageBoxResult.Yes)
            {
                App.CurrentAccount = null;
                new LoginWindow().Show();
                this.Close();
            }
        }
    }
}
