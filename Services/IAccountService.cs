using BusinessObjects;

namespace Services
{
    public interface IAccountService
    {
        Account Authenticate(string user, string pass);
        void Register(string username, string password, string fullName, string email, string phone, string address);
    }
}
