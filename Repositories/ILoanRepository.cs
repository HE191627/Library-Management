using BusinessObjects;

namespace Repositories
{
    public interface ILoanRepository
    {
        List<Loan> GetLoans();
        Loan? GetLoanById(int loanId);
        void CreateLoan(Loan loan);
        bool HasOverdueLoans(int memberId);
        void ReturnBook(int loanId);
        void DeleteLoan(int loanId);
    }
}
