using BusinessObjects;

namespace Repositories
{
    public interface IAccountRepository
    {
        Account? CheckLogin(string user, string pass);
        Account? GetAccountByUsername(string username);
        void CreateAccount(Account account);
    }
}
