using Library.Models;
using Microsoft.EntityFrameworkCore;

namespace Library.DbContexts
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<Logging> Loggings { get; set; }
        public DbSet<Account> Accounts { get; set; }
        public DbSet<Book> Books { get; set; }
        public DbSet<AccountBook> AccountBooks { get; set; }
		public Book GetBookById(int id)
		{
			return Books.Find(id); // استرجاع الكتاب بناءً على ID
		}

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AccountBook>()
                .HasKey(ab => new { ab.AccountId, ab.BookId });

            modelBuilder.Entity<AccountBook>()
                .HasOne(ab => ab.Account)
                .WithMany(a => a.AccountBooks)
                .HasForeignKey(ab => ab.AccountId);

            modelBuilder.Entity<AccountBook>()
                .HasOne(ab => ab.Book)
                .WithMany(b => b.AccountBooks)
                .HasForeignKey(ab => ab.BookId);
        }

		public ApplicationDbContext(DbContextOptions options) : base(options)
        {

        }

       
    }
}
