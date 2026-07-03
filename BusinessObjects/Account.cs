using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObjects
{
    public class Account
    {
        public int AccountId { get; set; }
        public string Username { get; set; } = null!;
        public string Password { get; set; } = null!;
        public string? Role { get; set; } // 'Admin', 'Member'
        public int? MemberId { get; set; }
        public bool? IsActive { get; set; }

        // Liên kết 1-1 với Member
        public virtual Member? Member { get; set; }
    }
}