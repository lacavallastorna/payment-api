using AutoMapper;
using FluentAssertions;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using PaymentApi.Api;
using PaymentApi.Resources.Constants;
using PaymentApi.Services.Services;
using PaymentApi.DataAccess.Data;
using PaymentApi.DataAccess.Repository.Instances;
using PaymentApi.Models.Mapper;
using PaymentApi.Models.Models;
using PaymentApi.Models.Models.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Moq;

//using System.Transactions;
using Xunit;
using Microsoft.Extensions.Logging;
using PaymentApi.Api.Controllers;

namespace PaymentApi.XUnitTests.Unit
{
	public class AccountBalanceServiceTests
	{
		private readonly ApplicationDbContext _context;
		private readonly AccountRepositoryAsync _accountRepo;
		private readonly TransactionRepositoryAsync _transRepo;
		private readonly IMapper _mapper;
		private readonly Mock<ILogger<AccountController>> _mockLogger;

		public AccountBalanceServiceTests()
		{
			var options = new DbContextOptionsBuilder<ApplicationDbContext>().UseInMemoryDatabase(Guid.NewGuid().ToString()).Options;
			_context = new ApplicationDbContext(options);
			_accountRepo = new AccountRepositoryAsync(_context);
			_transRepo = new TransactionRepositoryAsync(_context);
			_mockLogger = new Mock<ILogger<AccountController>>();
			var config = new MapperConfiguration(opts =>
			{
				opts.AddProfile(new PaymentApiMapper());
			});
			_mapper = config.CreateMapper();
		}

		[Fact]
		public async Task Unit_GetAccountBalance_Expect70000()
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

			AccountBalanceService balanceService = new AccountBalanceService(_mockLogger.Object, newAccount.Id, _mapper, _accountRepo, _transRepo);
			ServiceResult result = await balanceService.GetAccountBalance();

			result.Should().NotBeNull();
			AccountBalanceResultDto balance = JsonConvert.DeserializeObject<AccountBalanceResultDto>(result.ContentResult);
			balance.Should().NotBeNull();
			balance.AccountId.Should().Be(newAccount.Id);
			balance.OpeningBalance.Should().Be(10000);
			balance.ProcessedPaymentsBalance.Should().Be(1000);
			balance.PendingdPaymentsBalance.Should().Be(2000);
			balance.ClosingBalance.Should().Be(7000);
			balance.Deposits.Count().Should().Be(10);
			balance.Payments.Count().Should().Be(6);
		}

		[Fact]
		public async Task Unit_GetAccountBalance_NegativeAccountId_ExpectBadRequest()
		{
			AccountBalanceService balanceService = new AccountBalanceService(_mockLogger.Object, -1, _mapper, _accountRepo, _transRepo);
			ServiceResult result = await balanceService.GetAccountBalance();
			result.Should().NotBeNull();
			result.StatusCode.Should().Be(StatusCodes.Status400BadRequest);
			result.ContentResult.Should().Contain(Messages.Account_InvalidAccountId);
		}

		[Fact]
		public async Task Unit_GetAccountBalance_AccountDoesNotExist_ExpectNotFound()
		{
			AccountBalanceService balanceService = new AccountBalanceService(_mockLogger.Object, 100, _mapper, _accountRepo, _transRepo);
			ServiceResult result = await balanceService.GetAccountBalance();
			result.Should().NotBeNull();
			result.StatusCode.Should().Be(StatusCodes.Status404NotFound);
			result.ContentResult.Should().Contain(Messages.Account_AccountNotFound);
		}
	}
}