using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObjects
{
    public class Loan
    {
        public int LoanId { get; set; }
        public int? MemberId { get; set; }
        public DateTime? BorrowDate { get; set; }
        public DateTime DueDate { get; set; }
        public DateTime? ReturnDate { get; set; }
        public string? Status { get; set; } // 'Borrowed', 'Overdue', 'Returned'
        public string? Notes { get; set; }

        // Chủ nhân của phiếu mượn này
        public virtual Member? Member { get; set; }

        // Danh sách các cuốn sách trong phiếu mượn này
        public virtual ICollection<LoanDetail> LoanDetails { get; set; } = new List<LoanDetail>();
    }
}
