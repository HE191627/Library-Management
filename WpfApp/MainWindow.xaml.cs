using System.Windows;

namespace WpfApp
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            // Hiển thị tên admin
            if (App.CurrentAccount?.Member != null)
            {
                txtAdminName.Text = $"Xin chào, {App.CurrentAccount.Member.FullName}";
            }
            else
            {
                txtAdminName.Text = "Xin chào, Admin";
            }

            // Mặc định mở trang Kho Sách
            MainFrame.Navigate(new InventoryPage());
        }

        private void btnInventory_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new InventoryPage());
        }

        private void btnLoan_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new LoanPage());
        }

        private void btnCategory_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new CategoryPage());
        }

        private void btnMember_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new MemberPage());
        }

        private void btnLogout_Click(object sender, RoutedEventArgs e)
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