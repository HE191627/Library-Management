using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObjects
{
    public class Author
    {
        public int AuthorId { get; set; }
        public string AuthorName { get; set; } = null!;
        public string? Biography { get; set; }

        // Một tác giả có nhiều sách
        public virtual ICollection<Book> Books { get; set; } = new List<Book>();
    }
}
