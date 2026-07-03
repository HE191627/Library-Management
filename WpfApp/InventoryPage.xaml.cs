using System.Windows;
using System.Windows.Controls;
using BusinessObjects;
using Services;

namespace WpfApp
{
    public partial class InventoryPage : Page
    {
        private readonly IBookService _bookService = new BookService();
        private List<Book> _allBooks = new();

        public InventoryPage()
        {
            InitializeComponent();
            LoadData();
            LoadComboBoxes();
        }

        private void LoadData()
        {
            _allBooks = _bookService.GetBooks();
            dgBooks.ItemsSource = _allBooks;
        }

        private void LoadComboBoxes()
        {
            var categories = _bookService.GetCategories();
            var authors = _bookService.GetAuthors();

            // Filter ComboBox - thêm item "Tất cả"
            var filterList = new List<Category> { new Category { CategoryId = 0, CategoryName = "-- Tất cả --" } };
            filterList.AddRange(categories);
            cbCategoryFilter.ItemsSource = filterList;
            cbCategoryFilter.SelectedIndex = 0;

            // Form ComboBoxes
            cbCategory.ItemsSource = categories;
            cbAuthor.ItemsSource = authors;
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

            // Search by title/ISBN/author
            if (!string.IsNullOrWhiteSpace(txtSearch.Text))
            {
                var keyword = txtSearch.Text.ToLower();
                filtered = filtered.Where(b =>
                    (b.Title?.ToLower().Contains(keyword) == true) ||
                    (b.Isbn?.ToLower().Contains(keyword) == true) ||
                    (b.Author?.AuthorName?.ToLower().Contains(keyword) == true));
            }

            // Filter by category
            if (cbCategoryFilter.SelectedItem is Category selectedCat && selectedCat.CategoryId != 0)
            {
                filtered = filtered.Where(b => b.CategoryId == selectedCat.CategoryId);
            }

            dgBooks.ItemsSource = filtered.ToList();
        }

        private void dgBooks_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (dgBooks.SelectedItem is Book book)
            {
                txtTitle.Text = book.Title;
                txtISBN.Text = book.Isbn;
                txtYear.Text = book.PublishYear?.ToString();
                txtPrice.Text = book.Price?.ToString("N0");
                txtQuantity.Text = book.Quantity?.ToString();
                txtShelfLocation.Text = book.ShelfLocation;

                // Select author in ComboBox
                cbAuthor.SelectedValue = book.AuthorId;
                cbCategory.SelectedValue = book.CategoryId;

                // Select status
                foreach (ComboBoxItem item in cbStatus.Items)
                {
                    if (item.Content?.ToString() == book.Status)
                    {
                        cbStatus.SelectedItem = item;
                        break;
                    }
                }
            }
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(txtTitle.Text))
                {
                    MessageBox.Show("Vui lòng nhập tên sách!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                var book = new Book
                {
                    Title = txtTitle.Text.Trim(),
                    Isbn = txtISBN.Text.Trim(),
                    AuthorId = cbAuthor.SelectedValue as int?,
                    CategoryId = cbCategory.SelectedValue as int?,
                    PublishYear = int.TryParse(txtYear.Text, out var y) ? y : null,
                    Price = decimal.TryParse(txtPrice.Text.Replace(",", "").Replace(".", ""), out var p) ? p : null,
                    Quantity = int.TryParse(txtQuantity.Text, out var q) ? q : 0,
                    ShelfLocation = txtShelfLocation.Text.Trim(),
                    Status = (cbStatus.SelectedItem as ComboBoxItem)?.Content?.ToString() ?? "Available",
                    CreatedAt = DateTime.Now
                };

                _bookService.AddBook(book);
                MessageBox.Show("Thêm sách thành công!", "Thành công", MessageBoxButton.OK, MessageBoxImage.Information);
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
                if (dgBooks.SelectedItem is not Book selectedBook)
                {
                    MessageBox.Show("Vui lòng chọn sách cần cập nhật!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                selectedBook.Title = txtTitle.Text.Trim();
                selectedBook.Isbn = txtISBN.Text.Trim();
                selectedBook.AuthorId = cbAuthor.SelectedValue as int?;
                selectedBook.CategoryId = cbCategory.SelectedValue as int?;
                selectedBook.PublishYear = int.TryParse(txtYear.Text, out var y) ? y : null;
                selectedBook.Price = decimal.TryParse(txtPrice.Text.Replace(",", "").Replace(".", ""), out var p) ? p : null;
                selectedBook.Quantity = int.TryParse(txtQuantity.Text, out var q) ? q : 0;
                selectedBook.ShelfLocation = txtShelfLocation.Text.Trim();

                var newStatus = (cbStatus.SelectedItem as ComboBoxItem)?.Content?.ToString() ?? "Available";

                // Dùng service để update status (có business logic check sách quý)
                if (selectedBook.Status != newStatus)
                {
                    _bookService.UpdateBookStatus(selectedBook.BookId, newStatus);
                }

                selectedBook.Status = newStatus;
                // Clear navigation properties to avoid EF tracking issues
                selectedBook.Author = null;
                selectedBook.Category = null;
                selectedBook.LoanDetails = new List<LoanDetail>();
                _bookService.UpdateBook(selectedBook);

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
                if (dgBooks.SelectedItem is not Book selectedBook)
                {
                    MessageBox.Show("Vui lòng chọn sách cần xóa!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                var result = MessageBox.Show($"Bạn có chắc muốn xóa sách '{selectedBook.Title}'?", "Xác nhận xóa",
                    MessageBoxButton.YesNo, MessageBoxImage.Warning);

                if (result == MessageBoxResult.Yes)
                {
                    _bookService.DeleteBook(selectedBook.BookId);
                    MessageBox.Show("Đã xóa sách!", "Thành công", MessageBoxButton.OK, MessageBoxImage.Information);
                    LoadData();
                    ClearForm();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void btnRefresh_Click(object sender, RoutedEventArgs e)
        {
            LoadData();
            txtSearch.Text = "";
            cbCategoryFilter.SelectedIndex = 0;
        }

        private void btnClear_Click(object sender, RoutedEventArgs e)
        {
            ClearForm();
        }

        private void ClearForm()
        {
            txtTitle.Text = "";
            txtISBN.Text = "";
            txtYear.Text = "";
            txtPrice.Text = "";
            txtQuantity.Text = "";
            txtShelfLocation.Text = "";
            cbAuthor.SelectedIndex = -1;
            cbCategory.SelectedIndex = -1;
            cbStatus.SelectedIndex = 0;
            dgBooks.SelectedItem = null;
        }
    }
}
