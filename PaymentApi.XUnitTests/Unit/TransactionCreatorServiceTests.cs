using AutoMapper;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using Newtonsoft.Json;
using PaymentApi.Resources.Constants;
using PaymentApi.Api.Controllers;
using PaymentApi.Services.Services;
using PaymentApi.DataAccess.Data;
using PaymentApi.DataAccess.Repository.Instances;
using PaymentApi.Models.Mapper;
using PaymentApi.Models.Models;
using PaymentApi.Models.Models.Dtos;
using System;
using System.Threading.Tasks;
using Xunit;

namespace PaymentApi.XUnitTests.Unit
{
	public class TransactionCreatorServiceTests
	{
		private readonly ApplicationDbContext _context;
		private readonly AccountRepositoryAsync _accountRepo;
		private readonly TransactionRepositoryAsync _transRepo;
		private readonly IMapper _mapper;
		private readonly Mock<ILogger<DepositController>> _mockDepositLogger;
		private readonly Mock<ILogger<PaymentController>> _mockPaymentLogger;

		public TransactionCreatorServiceTests()
		{
			var options = new DbContextOptionsBuilder<ApplicationDbContext>().UseInMemoryDatabase(Guid.NewGuid().ToString()).Options;
			_context = new ApplicationDbContext(options);
			_accountRepo = new AccountRepositoryAsync(_context);
			_transRepo = new TransactionRepositoryAsync(_context);
			_mockDepositLogger = new Mock<ILogger<DepositController>>();
			_mockPaymentLogger = new Mock<ILogger<PaymentController>>();

			var config = new MapperConfiguration(opts =>
			{
				opts.AddProfile(new PaymentApiMapper());
			});
			_mapper = config.CreateMapper();
		}

		#region Deposit

		[Fact]
		public async Task Unit_CreateNewDeposit_ExpectDepositInResult()
		{
			Account newAccount = new Account { Name = "Test Account" };
			_context.Accounts.Add(newAccount);
			_context.SaveChanges();

			TransactionCreatorService creator = new TransactionCreatorService(_mockDepositLogger.Object, _mapper, newAccount.Id, 10000, new DateTime(2020, 1, 1), _accountRepo, _transRepo, TransactionStatusEnum.Processed, TransactionTypeEnum.Deposit, Messages.Deposit_FailedToCreate);
			ServiceResult result = await creator.CreateTransaction();
			result.Should().NotBeNull();
			result.StatusCode.Should().Be(StatusCodes.Status201Created);
			TransactionResultDto transaction = JsonConvert.DeserializeObject<TransactionResultDto>(result.ContentResult);
			transaction.Should().NotBeNull();
			transaction.AccountId.Should().Be(newAccount.Id);
			transaction.Amount.Should().Be(10000);
			transaction.Date.Should().Be(new DateTime(2020, 1, 1));
			transaction.TransactionStatus.Should().Be(TransactionStatusEnum.Processed.ToString());
		}

		[Theory]
		[InlineData(-100)]
		[InlineData(100)]
		public async Task Unit_CreateNewDeposit_AccountNegative_ExpectNotFound(int accountId)
		{
			TransactionCreatorService creator = new TransactionCreatorService(_mockDepositLogger.Object, _mapper, accountId, 10000, new DateTime(2020, 1, 1), _accountRepo, _transRepo, TransactionStatusEnum.Processed, TransactionTypeEnum.Deposit, Messages.Deposit_FailedToCreate);
			ServiceResult result = await creator.CreateTransaction();
			result.Should().NotBeNull();
			result.StatusCode.Should().Be(StatusCodes.Status404NotFound);
			result.ContentResult.Should().Contain(Messages.Account_AccountNotFound);
		}

		#endregion Deposit

		#region Payment

		[Fact]
		public async Task Unit_CreateNewPayment_ExpectPaymentInResult()
		{
			Account newAccount = new Account { Name = "Test Account" };
			_context.Accounts.Add(newAccount);
			_context.SaveChanges();

			Transaction deposit = new Transaction { AccountId = newAccount.Id, Amount = 1000000, Date = new DateTime(2020, 1, 1), TransactionStatus = TransactionStatusEnum.Processed, TransactionType = TransactionTypeEnum.Deposit, CreationDate = new DateTime(2020, 1, 1), LastUpdateDate = new DateTime(2020, 1, 1) };
			_context.Transactions.Add(deposit);
			_context.SaveChanges();

			TransactionCreatorService creator = new TransactionCreatorService(_mockPaymentLogger.Object, _mapper, newAccount.Id, 10000, new DateTime(2020, 1, 1), _accountRepo, _transRepo, TransactionStatusEnum.Pending, TransactionTypeEnum.Withdrawal, Messages.Payment_FailedToCreate);
			ServiceResult result = await creator.CreateTransaction();
			result.Should().NotBeNull();
			result.StatusCode.Should().Be(StatusCodes.Status201Created);
			TransactionResultDto transaction = JsonConvert.DeserializeObject<TransactionResultDto>(result.ContentResult);
			transaction.Should().NotBeNull();
			transaction.AccountId.Should().Be(newAccount.Id);
			transaction.Amount.Should().Be(10000);
			transaction.Date.Should().Be(new DateTime(2020, 1, 1));
			transaction.TransactionStatus.Should().Be(TransactionStatusEnum.Pending.ToString());
		}

		[Fact]
		public async Task Unit_CreateNewPayment_ExpectPaymentInResultClosed()
		{
			Account newAccount = new Account { Name = "Test Account" };
			_context.Accounts.Add(newAccount);
			_context.SaveChanges();

			TransactionCreatorService creator = new TransactionCreatorService(_mockPaymentLogger.Object, _mapper, newAccount.Id, 10000, new DateTime(2020, 1, 1), _accountRepo, _transRepo, TransactionStatusEnum.Pending, TransactionTypeEnum.Withdrawal, Messages.Payment_FailedToCreate);
			ServiceResult result = await creator.CreateTransaction();
			result.Should().NotBeNull();
			result.StatusCode.Should().Be(StatusCodes.Status201Created);
			TransactionResultDto transaction = JsonConvert.DeserializeObject<TransactionResultDto>(result.ContentResult);
			transaction.Should().NotBeNull();
			transaction.AccountId.Should().Be(newAccount.Id);
			transaction.Amount.Should().Be(10000);
			transaction.Date.Should().Be(new DateTime(2020, 1, 1));
			transaction.TransactionStatus.Should().Be(TransactionStatusEnum.Closed.ToString());
		}

		[Theory]
		[InlineData(-100)]
		[InlineData(100)]
		public async Task Unit_CreateNewPayment_AccountNegative_ExpectNotFound(int accountId)
		{
			TransactionCreatorService creator = new TransactionCreatorService(_mockPaymentLogger.Object, _mapper, accountId, 10000, new DateTime(2020, 1, 1), _accountRepo, _transRepo, TransactionStatusEnum.Processed, TransactionTypeEnum.Deposit, Messages.Payment_FailedToCreate);
			ServiceResult result = await creator.CreateTransaction();
			result.Should().NotBeNull();
			result.StatusCode.Should().Be(StatusCodes.Status404NotFound);
			result.ContentResult.Should().Contain(Messages.Account_AccountNotFound);
		}

		#endregion Payment
	}
}