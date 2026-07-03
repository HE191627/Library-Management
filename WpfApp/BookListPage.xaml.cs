using System.Windows.Controls;
using BusinessObjects;
using Services;

namespace WpfApp
{
    public partial class BookListPage : Page
    {
        private readonly IBookService _bookService = new BookService();
        private List<Book> _allBooks = new();

        public BookListPage()
        {
            InitializeComponent();
            LoadData();
            LoadCategoryFilter();
        }

        private void LoadData()
        {
            _allBooks = _bookService.GetBooks()
                .Where(b => b.Status == "Available" && b.Quantity > 0)
                .ToList();
            dgBooks.ItemsSource = _allBooks;
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
            if (_allBooks == null) return;

            var filtered = _allBooks.AsEnumerable();

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

            dgBooks.ItemsSource = filtered.ToList();
        }
    }
}
