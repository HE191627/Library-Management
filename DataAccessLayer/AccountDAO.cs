using BusinessObjects;
using Microsoft.EntityFrameworkCore;

namespace DataAccessLayer
{
    public class AccountDAO
    {
        public static Account? GetAccount(string user, string pass)
        {
            using var db = new LibraryEliteDBContext();
            return db.Accounts
                     .Include(a => a.Member)
                     .FirstOrDefault(a => a.Username == user && a.Password == pass && a.IsActive == true);
        }

        public static Account? GetAccountByUsername(string username)
        {
            using var db = new LibraryEliteDBContext();
            return db.Accounts.FirstOrDefault(a => a.Username == username);
        }

        public static void CreateAccount(Account account)
        {
            using var db = new LibraryEliteDBContext();
            db.Accounts.Add(account);
            db.SaveChanges();
        }
    }
}