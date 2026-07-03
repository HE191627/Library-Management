using System.Collections.Generic;

namespace BusinessObjects
{
    public class Category
    {
        public int CategoryId { get; set; }
        public string CategoryName { get; set; } = null!;
        public string? Description { get; set; }

        // Một thể loại có nhiều sách
        public virtual ICollection<Book> Books { get; set; } = new List<Book>();
    }
}