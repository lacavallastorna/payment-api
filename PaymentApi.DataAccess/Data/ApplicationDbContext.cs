using Microsoft.EntityFrameworkCore;
using PaymentApi.Models.Models;

namespace PaymentApi.DataAccess.Data
{
	public class ApplicationDbContext : DbContext
	{
		public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
		 : base(options)
		{
		}

		public DbSet<Account> Accounts { get; set; }
		public DbSet<Transaction> Transactions { get; set; }
	}
}