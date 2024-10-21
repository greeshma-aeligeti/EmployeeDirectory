using EmployeeDirectory.UI.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace EmployeeDirectory.UI.Data
{
    public class AppDbContext:DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
        }
        public DbSet<UserAccount> UserAccounts { get; set; }

    }
}
