using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using PaymentApi.DataAccess.Data;
using PaymentApi.DataAccess.Interfaces;
using PaymentApi.Models.Models;
using System;
using System.Linq;

namespace PaymentApi.DataAccess.Initialization
{
	public class DbInitializer : IDbInitializer
	{
		private readonly IServiceScopeFactory _scopeFactory;

		public DbInitializer(IServiceScopeFactory scopeFactory)
		{
			this._scopeFactory = scopeFactory;
		}

		public void Initialize()
		{
			using (var serviceScope = _scopeFactory.CreateScope())
			{
				using (var context = serviceScope.ServiceProvider.GetService<ApplicationDbContext>())
				{
					context.Database.Migrate();
				}
			}
		}

		public void SeedData()
		{
			using (var serviceScope = _scopeFactory.CreateScope())
			{
				using (var context = serviceScope.ServiceProvider.GetService<ApplicationDbContext>())
				{
					if (!context.Accounts.Any())
					{
						Account account = new Account
						{
							Name = "Test Account 001",
							CreationDate = new DateTime(2020, 7, 1, 8, 0, 0)
						};
						context.Accounts.Add(account);
						context.SaveChanges();

						Transaction deposit = new Transaction
						{
							AccountId = account.Id,
							Amount = 100000m,
							CreationDate = new DateTime(2020, 7, 1, 8, 0, 0),
							LastUpdateDate = new DateTime(2020, 7, 1, 8, 0, 0),
							Date = new DateTime(2020, 7, 1, 8, 0, 0),
							TransactionStatus = TransactionStatusEnum.Processed,
							TransactionType = TransactionTypeEnum.Deposit
						};
						context.Transactions.Add(deposit);
						context.SaveChanges();
					}
				}
			}
		}
	}
}