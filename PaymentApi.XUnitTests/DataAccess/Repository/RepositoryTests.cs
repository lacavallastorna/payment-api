using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using PaymentApi.DataAccess.Data;
using PaymentApi.DataAccess.Repository.Instances;
using PaymentApi.Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace PaymentApi.XUnitTests.DataAccess.Repository
{
	public class RepositoryTests : IDisposable
	{
		public readonly ApplicationDbContext _context;

		public RepositoryTests()
		{
			var options = new DbContextOptionsBuilder<ApplicationDbContext>()
				  .UseInMemoryDatabase(Guid.NewGuid().ToString())
				  .Options;
			_context = new ApplicationDbContext(options);
		}

		public void Dispose()
		{
			_context.Transactions.RemoveRange(_context.Transactions);
			_context.Accounts.RemoveRange(_context.Accounts);
			_context.SaveChanges();
		}

		[Fact]
		public async Task AddAsync_AddAccount_ExpectTrue()
		{
			AccountRepositoryAsync accountRepo = new AccountRepositoryAsync(_context);
			Account account = new Account { Name = "Johnny" };
			var result = await accountRepo.AddAsync(account);
			result.Should().BeTrue();
		}

		[Fact]
		public async Task GetFirstOrDefaultAsync_FindAccount_ExpectAccountInRepository()
		{
			Account account = new Account { Name = "Johnny" };
			_context.Accounts.Add(account).Should().NotBeNull();
			_context.SaveChanges().Should().BeGreaterThan(0);

			AccountRepositoryAsync accountRepo = new AccountRepositoryAsync(_context);
			Account accountFromDb = await accountRepo.GetFirstOrDefaultAsync();
			accountFromDb.Should().NotBeNull();
			accountFromDb.Name.Should().Be(account.Name);
		}

		[Fact]
		public async Task GetFirstOrDefaultAsyncFiltered_FindAccount_ExpectAccountInRepository()
		{
			Account account = new Account { Name = "Johnny" };
			_context.Accounts.Add(account).Should().NotBeNull();
			_context.SaveChanges().Should().BeGreaterThan(0);

			AccountRepositoryAsync accountRepo = new AccountRepositoryAsync(_context);
			Account accountFromDb = await accountRepo.GetFirstOrDefaultAsync(a => a.Name == "Johnny");
			accountFromDb.Should().NotBeNull();
			accountFromDb.Name.Should().Be(account.Name);
		}

		[Fact]
		public async Task GetFirstOrDefaultAsync_FindTransactionAndIncludeAccount_ExpectAccountInResult()
		{
			Account newAccount = new Account { Name = $"Johnny" };
			_context.Accounts.Add(newAccount).Should().NotBeNull();
			_context.SaveChanges().Should().BeGreaterThan(0);
			_context.Transactions.Add(new Transaction { AccountId = newAccount.Id, Date = new DateTime(2020, 1, 1), Amount = 1000 }).Should().NotBeNull();
			_context.SaveChanges().Should().BeGreaterThan(0);

			TransactionRepositoryAsync transRepo = new TransactionRepositoryAsync(_context);
			Transaction transactionFromDb = await transRepo.GetFirstOrDefaultAsync(t => t.AccountId == newAccount.Id, includeProperties: nameof(Account));
			transactionFromDb.Should().NotBeNull();
			Account account = transactionFromDb.Account;
			account.Should().NotBeNull();
			account.Name.Should().Be(newAccount.Name);
			account.Id.Should().Be(newAccount.Id);
		}

		[Fact]
		public async Task GetAsync_FindAccount_ExpectAccountInRepository()
		{
			Account account = new Account { Name = "Johnny" };
			_context.Accounts.Add(account).Should().NotBeNull();
			_context.SaveChanges().Should().BeGreaterThan(0);

			AccountRepositoryAsync accountRepo = new AccountRepositoryAsync(_context);
			Account accountFromDb = await accountRepo.GetAsync(account.Id);
			accountFromDb.Should().NotBeNull();
			accountFromDb.Name.Should().Be(account.Name);
		}

		[Fact]
		public async Task GetAllAsync_FindAccounts_ExpectAccountsInRepository()
		{
			for (int i = 0; i < 10; i++) { _context.Accounts.Add(new Account { Name = $"Account {i}" }).Should().NotBeNull(); }
			_context.SaveChanges().Should().BeGreaterThan(0);
			AccountRepositoryAsync accountRepo = new AccountRepositoryAsync(_context);
			IEnumerable<Account> accountsFromDb = await accountRepo.GetAllAsync();
			accountsFromDb.ToList().Count.Should().Be(10);
		}

		[Fact]
		public async Task GetAllAsync_FindAccountsFiltered_ExpectFiveAccountsInRepository()
		{
			for (int i = 0; i < 10; i++) { _context.Accounts.Add(new Account { Name = $"Account {i}" }).Should().NotBeNull(); }
			_context.SaveChanges().Should().BeGreaterThan(0);
			AccountRepositoryAsync accountRepo = new AccountRepositoryAsync(_context);
			IEnumerable<Account> accountsFromDb = await accountRepo.GetAllAsync(a => a.Id < 6);
			accountsFromDb.ToList().Count.Should().Be(5);
		}

		[Fact]
		public async Task GetAllAsync_FindAccountsFilteredDescending_ExpectFiveAccountsInRepository()
		{
			for (int i = 0; i < 10; i++) { _context.Accounts.Add(new Account { Name = $"Account {i}" }).Should().NotBeNull(); }
			_context.SaveChanges().Should().BeGreaterThan(0);
			AccountRepositoryAsync accountRepo = new AccountRepositoryAsync(_context);
			IEnumerable<Account> accountsFromDb = await accountRepo.GetAllAsync(a => a.Id < 6, orderBy: c => c.OrderByDescending(a => a.Id));
			accountsFromDb.ToList().Count.Should().Be(5);
			accountsFromDb.ToList()[0].Id.Should().Be(5);
		}

		[Fact]
		public async Task UpdateAsync_UpdateAccount_ExpectAccountInRepository()
		{
			Account newAccount = new Account { Name = $"Phil" };
			_context.Accounts.Add(newAccount).Should().NotBeNull();
			_context.SaveChanges().Should().BeGreaterThan(0);
			AccountRepositoryAsync accountRepo = new AccountRepositoryAsync(_context);
			newAccount.Name = "Sullivan";
			var result = await accountRepo.UpdateAsync(newAccount);
			result.Should().BeTrue();
			Account accountFromDb = await accountRepo.GetAsync(newAccount.Id);
			accountFromDb.Should().NotBeNull();
			accountFromDb.Name.Should().Be(newAccount.Name);
		}

		[Fact]
		public async Task RemoveAsyncById_RemoveAccount_ExpectAccountNotInRepository()
		{
			Account newAccount = new Account { Name = $"Phil" };
			_context.Accounts.Add(newAccount).Should().NotBeNull();
			_context.SaveChanges().Should().BeGreaterThan(0);
			AccountRepositoryAsync accountRepo = new AccountRepositoryAsync(_context);
			Account accountFromDb = await accountRepo.GetAsync(newAccount.Id);
			accountFromDb.Should().NotBeNull();
			accountFromDb.Name.Should().Be(newAccount.Name);
			var result = await accountRepo.RemoveAsync(accountFromDb.Id);
			result.Should().BeTrue();
			accountFromDb = await accountRepo.GetAsync(newAccount.Id);
			accountFromDb.Should().BeNull();
		}

		[Fact]
		public async Task RemoveAsyncByEntity_RemoveAccount_ExpectAccountNotInRepository()
		{
			Account newAccount = new Account { Name = $"Phil" };
			_context.Accounts.Add(newAccount).Should().NotBeNull();
			_context.SaveChanges().Should().BeGreaterThan(0);
			AccountRepositoryAsync accountRepo = new AccountRepositoryAsync(_context);
			Account accountFromDb = await accountRepo.GetAsync(newAccount.Id);
			accountFromDb.Should().NotBeNull();
			accountFromDb.Name.Should().Be(newAccount.Name);
			var result = await accountRepo.RemoveAsync(accountFromDb);
			result.Should().BeTrue();
			accountFromDb = await accountRepo.GetAsync(newAccount.Id);
			accountFromDb.Should().BeNull();
		}

		[Fact]
		public async Task RemoveRangeAsync_RemoveAccounts_ExpectAccountsNotInRepository()
		{
			IEnumerable<Account> accounts = new Account[]
			{
				new Account { Name = $"Phil1" },
				new Account { Name = $"Phil2" },
				new Account { Name = $"Phil3" }
			};
			_context.Accounts.AddRange(accounts);
			_context.SaveChanges().Should().BeGreaterThan(0);

			AccountRepositoryAsync accountRepo = new AccountRepositoryAsync(_context);
			var result = await accountRepo.RemoveRangeAsync(accounts);
			result.Should().BeTrue();
			_context.Accounts.Count().Should().Be(0);
		}

		[Fact]
		public async Task GetAllAsync_FindTransactionsIncludeAccount_ExpectAccountToBeIncluded()
		{
			Account newAccount = new Account { Name = $"Johnny" };
			_context.Accounts.Add(newAccount).Should().NotBeNull();
			_context.SaveChanges().Should().BeGreaterThan(0);
			_context.Transactions.Add(new Transaction { AccountId = newAccount.Id, Date = new DateTime(2020, 1, 1), Amount = 1000 }).Should().NotBeNull();
			_context.SaveChanges().Should().BeGreaterThan(0);
			TransactionRepositoryAsync transRepo = new TransactionRepositoryAsync(_context);
			IEnumerable<Transaction> transactionFromDb = await transRepo.GetAllAsync(t => t.AccountId == newAccount.Id, includeProperties: nameof(Account));
			transactionFromDb.Should().NotBeNull();
			transactionFromDb.Should().NotBeEmpty();
			Account account = transactionFromDb.ToList()[0].Account;
			account.Should().NotBeNull();
			account.Name.Should().Be(newAccount.Name);
			account.Id.Should().Be(newAccount.Id);
		}
	}
}