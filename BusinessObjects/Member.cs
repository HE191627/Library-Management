using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObjects
{
    public class Member
    {
        public int MemberId { get; set; }
        public string FullName { get; set; } = null!;
        public string? Email { get; set; }
        public string? Phone { get; set; }
        public string? Address { get; set; }
        public DateTime? JoinDate { get; set; }
        public string? Status { get; set; } // 'Active', 'Locked'

        // Một thành viên có nhiều lần mượn sách
        public virtual ICollection<Loan> Loans { get; set; } = new List<Loan>();
    }
}
