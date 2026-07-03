using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObjects
{
    public class LoanDetail
    {
        [Key] // Thêm dòng này ngay trên DetailId
        public int DetailId { get; set; }
        public int? LoanId { get; set; }
        public int? BookId { get; set; }
        public int? Quantity { get; set; }

        // Liên kết ngược lại
        public virtual Loan? Loan { get; set; }
        public virtual Book? Book { get; set; }
    }
}
