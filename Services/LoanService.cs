using BusinessObjects;
using Repositories;

namespace Services
{
    public class LoanService : ILoanService
    {
        private readonly ILoanRepository _loanRepo = new LoanRepository();
        private readonly IBookRepository _bookRepo = new BookRepository();
        private readonly IMemberRepository _memberRepo = new MemberRepository();

        public List<Loan> GetLoans() => _loanRepo.GetLoans();

        public void CreateLoanTicket(int memberId, List<int> bookIds, int durationDays)
        {
            // 1. Kiểm tra member có bị khóa không
            var member = _memberRepo.GetMemberById(memberId);
            if (member == null)
                throw new Exception("Không tìm thấy thành viên!");
            if (member.Status == "Locked")
                throw new Exception("Thành viên đang bị khóa, không thể mượn sách!");

            // 2. Kiểm tra có nợ quá hạn không
            if (_loanRepo.HasOverdueLoans(memberId))
                throw new Exception("Thành viên đang có phiếu mượn quá hạn, vui lòng trả trước!");

            // 3. Không mượn quá 5 quyển
            if (bookIds.Count > 5)
                throw new Exception("Tối đa mượn 5 quyển!");

            if (bookIds.Count == 0)
                throw new Exception("Vui lòng chọn ít nhất 1 cuốn sách!");

            // 4. Tạo đối tượng Loan
            var newLoan = new Loan
            {
                MemberId = memberId,
                BorrowDate = DateTime.Now,
                DueDate = DateTime.Now.AddDays(durationDays),
                Status = "Borrowed",
                LoanDetails = new List<LoanDetail>()
            };

            // 5. Thêm chi tiết và trừ số lượng sách trong kho
            foreach (var bId in bookIds)
            {
                var book = _bookRepo.GetBookById(bId);
                if (book == null)
                    throw new Exception($"Không tìm thấy sách với mã {bId}!");
                if (book.Quantity <= 0)
                    throw new Exception($"Sách '{book.Title}' đã hết!");

                newLoan.LoanDetails.Add(new LoanDetail { BookId = bId, Quantity = 1 });

                // Giảm số lượng sách
                book.Quantity -= 1;
                _bookRepo.UpdateBook(book);
            }

            _loanRepo.CreateLoan(newLoan);
        }

        public void ReturnBook(int loanId)
        {
            _loanRepo.ReturnBook(loanId);
        }

        public decimal CalculateFine(int loanId)
        {
            // Cứ trễ 1 ngày phạt 5.000 VNĐ
            var loan = _loanRepo.GetLoanById(loanId);
            if (loan == null) return 0;
            if (loan.ReturnDate == null && DateTime.Now > loan.DueDate)
            {
                var daysLate = (DateTime.Now - loan.DueDate).Days;
                return daysLate * 5000;
            }
            if (loan.ReturnDate != null && loan.ReturnDate > loan.DueDate)
            {
                var daysLate = (loan.ReturnDate.Value - loan.DueDate).Days;
                return daysLate * 5000;
            }
            return 0;
        }

        public void DeleteLoan(int loanId)
        {
            _loanRepo.DeleteLoan(loanId);
        }
    }
}
