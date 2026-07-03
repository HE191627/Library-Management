using BusinessObjects;
using Microsoft.EntityFrameworkCore;

namespace DataAccessLayer
{
    public class LoanDAO
    {
        public static List<Loan> GetAllLoans()
        {
            using var db = new LibraryEliteDBContext();
            return db.Loans
                     .Include(l => l.Member)
                     .Include(l => l.LoanDetails)
                        .ThenInclude(ld => ld.Book)
                     .ToList();
        }

        public static void CreateLoan(Loan loan)
        {
            using var db = new LibraryEliteDBContext();
            db.Loans.Add(loan);
            db.SaveChanges();
        }

        public static bool IsBookBeingBorrowed(int bookId)
        {
            using var db = new LibraryEliteDBContext();
            return db.LoanDetails
                     .Include(ld => ld.Loan)
                     .Any(ld => ld.BookId == bookId && ld.Loan!.ReturnDate == null);
        }

        public static bool HasOverdueLoans(int memberId)
        {
            using var db = new LibraryEliteDBContext();
            return db.Loans.Any(l => l.MemberId == memberId && l.Status == "Overdue");
        }

        public static void MarkAsReturned(int loanId)
        {
            using var db = new LibraryEliteDBContext();
            var loan = db.Loans.Include(l => l.LoanDetails).FirstOrDefault(l => l.LoanId == loanId);
            if (loan != null)
            {
                loan.ReturnDate = DateTime.Now;
                loan.Status = "Returned";

                // Trả lại số lượng sách vào kho
                foreach (var detail in loan.LoanDetails)
                {
                    var book = db.Books.Find(detail.BookId);
                    if (book != null)
                    {
                        book.Quantity = (book.Quantity ?? 0) + (detail.Quantity ?? 1);
                    }
                }

                db.SaveChanges();
            }
        }

        public static Loan? GetLoanById(int loanId)
        {
            using var db = new LibraryEliteDBContext();
            return db.Loans
                     .Include(l => l.Member)
                     .Include(l => l.LoanDetails)
                        .ThenInclude(ld => ld.Book)
                     .FirstOrDefault(l => l.LoanId == loanId);
        }

        public static void DeleteLoan(int loanId)
        {
            using var db = new LibraryEliteDBContext();
            var loan = db.Loans.Include(l => l.LoanDetails).FirstOrDefault(l => l.LoanId == loanId);
            if (loan != null)
            {
                db.LoanDetails.RemoveRange(loan.LoanDetails);
                db.Loans.Remove(loan);
                db.SaveChanges();
            }
        }
    }
}
