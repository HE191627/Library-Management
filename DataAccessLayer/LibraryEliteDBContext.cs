using BusinessObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace DataAccessLayer
{
    public class LibraryEliteDBContext : DbContext
    {
        public LibraryEliteDBContext() { }

        public DbSet<Account> Accounts { get; set; } = null!;
        public DbSet<Author> Authors { get; set; } = null!;
        public DbSet<Book> Books { get; set; } = null!;
        public DbSet<Category> Categories { get; set; } = null!;
        public DbSet<Loan> Loans { get; set; } = null!;
        public DbSet<LoanDetail> LoanDetails { get; set; } = null!;
        public DbSet<Member> Members { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                var builder = new ConfigurationBuilder()
                    .SetBasePath(Directory.GetCurrentDirectory())
                    .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                    .Build();
                optionsBuilder.UseSqlServer(builder.GetConnectionString("DB"));
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Table mappings
            modelBuilder.Entity<Account>().ToTable("Accounts");
            modelBuilder.Entity<Author>().ToTable("Authors");
            modelBuilder.Entity<Book>().ToTable("Books");
            modelBuilder.Entity<Category>().ToTable("Categories");
            modelBuilder.Entity<Loan>().ToTable("Loans");
            modelBuilder.Entity<LoanDetail>().ToTable("LoanDetails");
            modelBuilder.Entity<Member>().ToTable("Members");

            // Book -> Author (many-to-one)
            modelBuilder.Entity<Book>()
                .HasOne(b => b.Author)
                .WithMany(a => a.Books)
                .HasForeignKey(b => b.AuthorId);

            // Book -> Category (many-to-one)
            modelBuilder.Entity<Book>()
                .HasOne(b => b.Category)
                .WithMany(c => c.Books)
                .HasForeignKey(b => b.CategoryId);

            // Loan -> Member (many-to-one)
            modelBuilder.Entity<Loan>()
                .HasOne(l => l.Member)
                .WithMany(m => m.Loans)
                .HasForeignKey(l => l.MemberId);

            // LoanDetail -> Loan (many-to-one)
            modelBuilder.Entity<LoanDetail>()
                .HasOne(ld => ld.Loan)
                .WithMany(l => l.LoanDetails)
                .HasForeignKey(ld => ld.LoanId);

            // LoanDetail -> Book (many-to-one)
            modelBuilder.Entity<LoanDetail>()
                .HasOne(ld => ld.Book)
                .WithMany(b => b.LoanDetails)
                .HasForeignKey(ld => ld.BookId);

            // Account -> Member (many-to-one with cascade delete)
            modelBuilder.Entity<Account>()
                .HasOne(a => a.Member)
                .WithMany()
                .HasForeignKey(a => a.MemberId)
                .OnDelete(DeleteBehavior.Cascade);

            // ISBN column mapping
            modelBuilder.Entity<Book>()
                .Property(b => b.Isbn)
                .HasColumnName("ISBN");
        }
    }
}
