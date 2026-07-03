using BusinessObjects;
using Repositories;

namespace Services
{
    public class AccountService : IAccountService
    {
        private readonly IAccountRepository _accountRepo = new AccountRepository();
        private readonly IMemberRepository _memberRepo = new MemberRepository();

        public Account Authenticate(string user, string pass)
        {
            var account = _accountRepo.CheckLogin(user, pass);
            if (account == null)
            {
                throw new Exception("Tài khoản hoặc mật khẩu không đúng!");
            }
            return account;
        }

        public void Register(string username, string password, string fullName, string email, string phone, string address)
        {
            // Kiểm tra username đã tồn tại chưa
            var existing = _accountRepo.GetAccountByUsername(username);
            if (existing != null)
            {
                throw new Exception("Tên đăng nhập đã được sử dụng!");
            }

            // Kiểm tra email trùng
            var members = _memberRepo.GetMembers();
            if (members.Any(m => m.Email == email))
            {
                throw new Exception("Email này đã được sử dụng!");
            }

            // Tạo Member trước
            var newMember = new Member
            {
                FullName = fullName,
                Email = email,
                Phone = phone,
                Address = address,
                JoinDate = DateTime.Now,
                Status = "Active"
            };
            _memberRepo.AddMember(newMember);

            // Tạo Account liên kết với Member vừa tạo
            var newAccount = new Account
            {
                Username = username,
                Password = password,
                Role = "Member",
                MemberId = newMember.MemberId,
                IsActive = true
            };
            _accountRepo.CreateAccount(newAccount);
        }
    }
}
