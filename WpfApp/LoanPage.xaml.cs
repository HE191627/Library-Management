using System.Windows;
using System.Windows.Controls;
using BusinessObjects;
using Services;

namespace WpfApp
{
    public partial class LoanPage : Page
    {
        private readonly ILoanService _loanService = new LoanService();
        private readonly IBookService _bookService = new BookService();
        private readonly IMemberService _memberService = new MemberService();

        public LoanPage()
        {
            InitializeComponent();
            LoadData();
        }

        private void LoadData()
        {
            // Load danh sách phiếu mượn
            dgLoans.ItemsSource = _loanService.GetLoans();

            // Load danh sách thành viên active
            cbMember.ItemsSource = _memberService.GetActiveMembers();

            // Load sách còn có thể mượn (Available & quantity > 0)
            var availableBooks = _bookService.GetBooks()
                .Where(b => b.Status == "Available" && b.Quantity > 0)
                .ToList();
            lbBooks.ItemsSource = availableBooks;
        }

        private void dgLoans_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (dgLoans.SelectedItem is Loan loan)
            {
                dgLoanDetails.ItemsSource = loan.LoanDetails;
            }
            else
            {
                dgLoanDetails.ItemsSource = null;
            }
        }

        private void btnCreateLoan_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (cbMember.SelectedValue == null)
                {
                    MessageBox.Show("Vui lòng chọn độc giả!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                var selectedBooks = lbBooks.SelectedItems.Cast<Book>().Select(b => b.BookId).ToList();
                if (selectedBooks.Count == 0)
                {
                    MessageBox.Show("Vui lòng chọn ít nhất 1 cuốn sách!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                if (!int.TryParse(txtDays.Text, out var days) || days <= 0)
                {
                    MessageBox.Show("Số ngày mượn không hợp lệ!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                int memberId = (int)cbMember.SelectedValue;
                _loanService.CreateLoanTicket(memberId, selectedBooks, days);

                MessageBox.Show("Tạo phiếu mượn thành công!", "Thành công", MessageBoxButton.OK, MessageBoxImage.Information);
                LoadData();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void btnReturn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (dgLoans.SelectedItem is not Loan loan)
                {
                    MessageBox.Show("Vui lòng chọn phiếu mượn cần trả!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                if (loan.Status == "Returned")
                {
                    MessageBox.Show("Phiếu mượn này đã được trả rồi!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
                    return;
                }

                // Tính tiền phạt
                var fine = _loanService.CalculateFine(loan.LoanId);
                string fineMsg = fine > 0 ? $"\n⚠️ Tiền phạt trễ hạn: {fine:N0} VNĐ" : "";

                var result = MessageBox.Show($"Xác nhận trả sách cho phiếu mượn #{loan.LoanId}?{fineMsg}", 
                    "Xác nhận trả sách", MessageBoxButton.YesNo, MessageBoxImage.Question);

                if (result == MessageBoxResult.Yes)
                {
                    _loanService.ReturnBook(loan.LoanId);
                    MessageBox.Show("Trả sách thành công!", "Thành công", MessageBoxButton.OK, MessageBoxImage.Information);
                    LoadData();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        private void btnDeleteLoan_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (dgLoans.SelectedItem is not Loan loan)
                {
                    MessageBox.Show("Vui lòng chọn phiếu mượn cần xóa!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                var result = MessageBox.Show($"Bạn có chắc muốn xóa phiếu mượn #{loan.LoanId}?\nToàn bộ chi tiết mượn sẽ bị xóa!", 
                    "Xác nhận xóa", MessageBoxButton.YesNo, MessageBoxImage.Warning);

                if (result == MessageBoxResult.Yes)
                {
                    _loanService.DeleteLoan(loan.LoanId);
                    MessageBox.Show("Đã xóa phiếu mượn!", "Thành công", MessageBoxButton.OK, MessageBoxImage.Information);
                    dgLoanDetails.ItemsSource = null;
                    LoadData();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
