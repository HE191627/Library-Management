using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObjects
{
    public class Book
    {
        public int BookId { get; set; }
        public string Title { get; set; } = null!;
        public string? Isbn { get; set; }
        public int? AuthorId { get; set; }
        public int? CategoryId { get; set; }
        public int? PublishYear { get; set; }
        public decimal? Price { get; set; }
        public int? Quantity { get; set; }
        public string? Summary { get; set; }
        public string? Status { get; set; } // 'Available', 'Damaged', 'Lost'
        public DateTime? CreatedAt { get; set; }
        public string? ShelfLocation { get; set; } // Vị trí kệ sách

        // Điều hướng đến bảng Cha
        public virtual Author? Author { get; set; }
        public virtual Category? Category { get; set; }

        // Liên kết đến bảng Chi tiết mượn
        public virtual ICollection<LoanDetail> LoanDetails { get; set; } = new List<LoanDetail>();
    }
}