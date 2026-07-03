using BusinessObjects;
using DataAccessLayer;

namespace Repositories
{
    public class LoanRepository : ILoanRepository
    {
        public List<Loan> GetLoans() => LoanDAO.GetAllLoans();
        public Loan? GetLoanById(int loanId) => LoanDAO.GetLoanById(loanId);
        public void CreateLoan(Loan loan) => LoanDAO.CreateLoan(loan);
        public bool HasOverdueLoans(int memberId) => LoanDAO.HasOverdueLoans(memberId);
        public void ReturnBook(int loanId) => LoanDAO.MarkAsReturned(loanId);
        public void DeleteLoan(int loanId) => LoanDAO.DeleteLoan(loanId);
    }
}
