using BusinessObjects;

namespace Services
{
    public interface ILoanService
    {
        List<Loan> GetLoans();
        void CreateLoanTicket(int memberId, List<int> bookIds, int durationDays);
        void ReturnBook(int loanId);
        decimal CalculateFine(int loanId);
        void DeleteLoan(int loanId);
    }
}
