using FluentAssertions;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Newtonsoft.Json;
using PaymentApi.Api;
using PaymentApi.Resources.Constants;
using PaymentApi.DataAccess.Data;
using PaymentApi.Models.Models;
using PaymentApi.Models.Models.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Serilog;

//using System.Transactions;
using Xunit;

[assembly: CollectionBehavior(DisableTestParallelization = true)]

namespace PaymentApi.XUnitTests.Integration
{
	public class AccountControllerTests : IDisposable
	{
		private readonly ApplicationDbContext _context;
		private readonly HttpClient _client;

		public AccountControllerTests()
		{
			var builder = new WebHostBuilder().UseEnvironment("Testing")
				.UseSerilog()
				.UseStartup<Startup>();

			var server = new TestServer(builder);
			_context = server.Host.Services.GetService(typeof(ApplicationDbContext)) as ApplicationDbContext;
			_client = server.CreateClient();
		}

		public void Dispose()
		{
			_context.Transactions.RemoveRange(_context.Transactions);
			_context.Accounts.RemoveRange(_context.Accounts);
			_context.SaveChanges();
		}

		#region api/account/create

		[Fact]
		public async Task Integration_CreateNewAccount_ExpectAccountInResultAndRepository()
		{
			var newAccount = new AccountInsertDto { Name = "Johnny" };
			var content = JsonConvert.SerializeObject(newAccount);
			var stringContent = new StringContent(content, Encoding.UTF8, "application/json");
			var response = await _client.PostAsync("/api/account/create", stringContent);
			response.StatusCode.Should().Be(System.Net.HttpStatusCode.Created);
			var responseString = await response.Content.ReadAsStringAsync();
			AccountInsertResultDto account = JsonConvert.DeserializeObject<AccountInsertResultDto>(responseString);
			account.Should().NotBeNull();
			account.Name.Should().Be("Johnny");
			Account accountFromDb = _context.Accounts.Find(account.AccountId);
			accountFromDb.Should().NotBeNull();
			accountFromDb.Name.Should().Be(account.Name);
			accountFromDb.Id.Should().Be(account.AccountId);
		}

		#region 400 Errors handled by [apicontroller]

		public static IEnumerable<object[]> GetAccountInsertDtos
		{
			get
			{
				return new[]
				{
				new object[] { new AccountInsertDto() },
				new object[] { new AccountInsertDto { Name = string.Empty } },
				new object[] { new AccountInsertDto { Name = null } }
				};
			}
		}

		[Theory]
		[MemberData(nameof(GetAccountInsertDtos))]
		public async Task Integration_CreateNewAccount_EmptyDto_ExpectBadRequest(AccountInsertDto objDto)
		{
			var content = JsonConvert.SerializeObject(objDto);
			var stringContent = new StringContent(content, Encoding.UTF8, "application/json");
			var response = await _client.PostAsync("/api/account/create", stringContent);
			response.StatusCode.Should().Be(System.Net.HttpStatusCode.BadRequest);
			var responseString = await response.Content.ReadAsStringAsync();
			responseString.Should().Contain("The Name field is required");
		}

		[Fact]
		public async Task Integration_CreateNewAccount_EmptyBody_ExpectBadRequest()
		{
			var content = JsonConvert.SerializeObject(null);
			var stringContent = new StringContent(content, Encoding.UTF8, "application/json");
			var response = await _client.PostAsync("/api/account/create", stringContent);
			response.StatusCode.Should().Be(System.Net.HttpStatusCode.BadRequest);
			var responseString = await response.Content.ReadAsStringAsync();
			responseString.Should().Contain("A non-empty request body is required.");
		}

		#endregion 400 Errors handled by [apicontroller]

		#endregion api/account/create

		#region api/account/balance/{id}

		[Fact]
		public async Task Integration_GetAccountBalance_Expect70000()
		{
			Account newAccount = new Account { Name = "Test Account" };
			_context.Accounts.Add(newAccount);
			_context.SaveChanges();
			// 10 Deposits of 1000
			for (int i = 0; i < 10; i++)
			{
				_context.Transactions.Add(new Transaction
				{
					AccountId = newAccount.Id,
					Amount = 1000,
					TransactionStatus = TransactionStatusEnum.Processed,
					TransactionType = TransactionTypeEnum.Deposit,
					Date = new DateTime(2020, 1, 1),
					CreationDate = new DateTime(2020, 1, 1),
					LastUpdateDate = new DateTime(2020, 1, 1)
				});
			}
			// 2 Pending Payments
			for (int i = 0; i < 2; i++)
			{
				_context.Transactions.Add(new Transaction
				{
					AccountId = newAccount.Id,
					Amount = 1000,
					TransactionStatus = TransactionStatusEnum.Pending,
					TransactionType = TransactionTypeEnum.Withdrawal,
					Date = new DateTime(2020, 1, 1),
					CreationDate = new DateTime(2020, 1, 1),
					LastUpdateDate = new DateTime(2020, 1, 1)
				});
			}
			// 1 Procesed Payment
			_context.Transactions.Add(new Transaction
			{
				AccountId = newAccount.Id,
				Amount = 1000,
				TransactionStatus = TransactionStatusEnum.Processed,
				TransactionType = TransactionTypeEnum.Withdrawal,
				Date = new DateTime(2020, 1, 1),
				CreationDate = new DateTime(2020, 1, 1),
				LastUpdateDate = new DateTime(2020, 1, 1)
			});
			// 2 Closed Payments
			for (int i = 0; i < 3; i++)
			{
				_context.Transactions.Add(new Transaction
				{
					AccountId = newAccount.Id,
					Amount = 100000,
					TransactionStatus = TransactionStatusEnum.Closed,
					TransactionType = TransactionTypeEnum.Withdrawal,
					Date = new DateTime(2020, 1, 1),
					CreationDate = new DateTime(2020, 1, 1),
					LastUpdateDate = new DateTime(2020, 1, 1),
					ClosedReason = Messages.Payment_NotEnoughFundsReason
				});
			}
			_context.SaveChanges();

			var response = await _client.GetAsync($"/api/account/balance/{newAccount.Id}");
			response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
			var responseString = await response.Content.ReadAsStringAsync();
			AccountBalanceResultDto accountBalanceResult = JsonConvert.DeserializeObject<AccountBalanceResultDto>(responseString);
			accountBalanceResult.Should().NotBeNull();
			accountBalanceResult.OpeningBalance.Should().Be(10000);
			accountBalanceResult.ProcessedPaymentsBalance.Should().Be(1000);
			accountBalanceResult.PendingdPaymentsBalance.Should().Be(2000);
			accountBalanceResult.ClosingBalance.Should().Be(7000);
			accountBalanceResult.Deposits.Should().HaveCount(10);
			accountBalanceResult.Payments.Should().HaveCount(6);
			accountBalanceResult.Payments.Where(p => p.TransactionStatus == TransactionStatusEnum.Pending.ToString()).Should().HaveCount(2);
			accountBalanceResult.Payments.Where(p => p.TransactionStatus == TransactionStatusEnum.Closed.ToString()).Should().HaveCount(3);
			accountBalanceResult.Payments.Where(p => p.TransactionStatus == TransactionStatusEnum.Processed.ToString()).Should().HaveCount(1);
		}

		[Theory]
		[InlineData(-1)]
		[InlineData(0)]
		public async Task Integration_GetAccountBalance_NegativeId_ExpectBadRequest(int accountId)
		{
			var response = await _client.GetAsync($"/api/account/balance/{accountId}");
			response.StatusCode.Should().Be(System.Net.HttpStatusCode.BadRequest);
			var responseString = await response.Content.ReadAsStringAsync();
			responseString.Should().Contain(Messages.Account_InvalidAccountId);
		}

		[Theory]
		[InlineData(1)]
		[InlineData(10)]
		[InlineData(100)]
		public async Task Integration_GetAccountBalance_AccountDoesNotExist_ExpectNotFound(int accountId)
		{
			var response = await _client.GetAsync($"/api/account/balance/{accountId}");
			response.StatusCode.Should().Be(System.Net.HttpStatusCode.NotFound);
			var responseString = await response.Content.ReadAsStringAsync();
			responseString.Should().Contain(Messages.Account_AccountNotFound);
		}

		#endregion api/account/balance/{id}
	}
}