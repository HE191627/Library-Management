using System.Windows;
using Services;

namespace WpfApp
{
    public partial class LoginWindow : Window
    {
        private readonly IAccountService _accountService = new AccountService();

        public LoginWindow()
        {
            InitializeComponent();
        }

        private void btnLogin_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(txtUser.Text) || string.IsNullOrWhiteSpace(txtPass.Password))
                {
                    MessageBox.Show("Vui lòng nhập đầy đủ tài khoản và mật khẩu!", "Thông báo",
                        MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                var account = _accountService.Authenticate(txtUser.Text, txtPass.Password);
                App.CurrentAccount = account;

                if (account.Role == "Admin")
                {
                    new MainWindow().Show();
                }
                else
                {
                    new MemberWindow().Show();
                }
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Lỗi đăng nhập", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void btnRegister_Click(object sender, RoutedEventArgs e)
        {
            var registerWindow = new RegisterWindow();
            registerWindow.ShowDialog();
        }
    }
}
