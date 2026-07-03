using System.Windows;
using System.Windows.Controls;
using BusinessObjects;
using Services;

namespace WpfApp
{
    public partial class MemberPage : Page
    {
        private readonly IMemberService _memberService = new MemberService();
        private List<Member> _allMembers = new();

        public MemberPage()
        {
            InitializeComponent();
            LoadData();
        }

        private void LoadData()
        {
            _allMembers = _memberService.GetMembers();
            dgMembers.ItemsSource = _allMembers;
        }

        private void txtSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (_allMembers == null) return;

            var keyword = txtSearch.Text.ToLower();
            var filtered = _allMembers.Where(m =>
                (m.FullName?.ToLower().Contains(keyword) == true) ||
                (m.Email?.ToLower().Contains(keyword) == true) ||
                (m.Phone?.Contains(keyword) == true)
            ).ToList();

            dgMembers.ItemsSource = filtered;
        }

        private void dgMembers_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (dgMembers.SelectedItem is Member member)
            {
                txtFullName.Text = member.FullName;
                txtEmail.Text = member.Email;
                txtPhone.Text = member.Phone;
                txtAddress.Text = member.Address;
            }
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(txtFullName.Text))
                {
                    MessageBox.Show("Vui lòng nhập họ tên!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                var member = new Member
                {
                    FullName = txtFullName.Text.Trim(),
                    Email = txtEmail.Text.Trim(),
                    Phone = txtPhone.Text.Trim(),
                    Address = txtAddress.Text.Trim(),
                    JoinDate = DateTime.Now,
                    Status = "Active"
                };

                _memberService.RegisterMember(member);
                MessageBox.Show("Thêm độc giả thành công!", "Thành công", MessageBoxButton.OK, MessageBoxImage.Information);
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
                if (dgMembers.SelectedItem is not Member selected)
                {
                    MessageBox.Show("Vui lòng chọn độc giả cần cập nhật!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                selected.FullName = txtFullName.Text.Trim();
                selected.Email = txtEmail.Text.Trim();
                selected.Phone = txtPhone.Text.Trim();
                selected.Address = txtAddress.Text.Trim();

                // Clear navigation properties
                selected.Loans = new List<Loan>();
                _memberService.UpdateMember(selected);

                MessageBox.Show("Cập nhật thành công!", "Thành công", MessageBoxButton.OK, MessageBoxImage.Information);
                LoadData();
                ClearForm();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void btnToggleStatus_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (dgMembers.SelectedItem is not Member selected)
                {
                    MessageBox.Show("Vui lòng chọn độc giả!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                selected.Status = selected.Status == "Active" ? "Locked" : "Active";
                selected.Loans = new List<Loan>();
                _memberService.UpdateMember(selected);

                MessageBox.Show($"Đã chuyển trạng thái thành '{selected.Status}'!", "Thành công", 
                    MessageBoxButton.OK, MessageBoxImage.Information);
                LoadData();
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
                if (dgMembers.SelectedItem is not Member selected)
                {
                    MessageBox.Show("Vui lòng chọn độc giả cần xóa!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                var result = MessageBox.Show($"Bạn có chắc muốn xóa độc giả '{selected.FullName}'?", "Xác nhận xóa",
                    MessageBoxButton.YesNo, MessageBoxImage.Warning);

                if (result == MessageBoxResult.Yes)
                {
                    _memberService.DeleteMember(selected.MemberId);
                    MessageBox.Show("Đã xóa độc giả!", "Thành công", MessageBoxButton.OK, MessageBoxImage.Information);
                    LoadData();
                    ClearForm();
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Không thể xóa độc giả này vì đang có phiếu mượn hoặc tài khoản liên kết!", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void btnRefresh_Click(object sender, RoutedEventArgs e)
        {
            LoadData();
            txtSearch.Text = "";
        }

        private void btnClear_Click(object sender, RoutedEventArgs e)
        {
            ClearForm();
        }

        private void ClearForm()
        {
            txtFullName.Text = "";
            txtEmail.Text = "";
            txtPhone.Text = "";
            txtAddress.Text = "";
            dgMembers.SelectedItem = null;
        }
    }
}
