using BusinessObjects;
using DataAccessLayer;

namespace Repositories
{
    public class AccountRepository : IAccountRepository
    {
        public Account? CheckLogin(string user, string pass) => AccountDAO.GetAccount(user, pass);
        public Account? GetAccountByUsername(string username) => AccountDAO.GetAccountByUsername(username);
        public void CreateAccount(Account account) => AccountDAO.CreateAccount(account);
    }
}
