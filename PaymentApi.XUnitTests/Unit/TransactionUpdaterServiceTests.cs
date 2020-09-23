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
	public class TransactionUpdaterServiceTests
	{
		private readonly ApplicationDbContext _context;
		private readonly AccountRepositoryAsync _accountRepo;
		private readonly TransactionRepositoryAsync _transRepo;
		private readonly IMapper _mapper;
		private readonly Mock<ILogger<PaymentController>> _mockPaymentLogger = new Mock<ILogger<PaymentController>>();

		public TransactionUpdaterServiceTests()
		{
			var options = new DbContextOptionsBuilder<ApplicationDbContext>().UseInMemoryDatabase(Guid.NewGuid().ToString()).Options;
			_context = new ApplicationDbContext(options);
			_accountRepo = new AccountRepositoryAsync(_context);
			_transRepo = new TransactionRepositoryAsync(_context);

			var config = new MapperConfiguration(opts =>
			{
				opts.AddProfile(new PaymentApiMapper());
			});
			_mapper = config.CreateMapper();
		}

		#region Process Payment

		[Fact]
		public async Task Unit_ProcessPendingPayment_ExpectPaymentProcessedInResult()
		{
			Account newAccount = new Account { Name = "Test Account" };
			_context.Accounts.Add(newAccount);
			_context.SaveChanges();

			Transaction deposit = new Transaction { AccountId = newAccount.Id, Amount = 1000000, Date = new DateTime(2020, 1, 1), TransactionStatus = TransactionStatusEnum.Processed, TransactionType = TransactionTypeEnum.Deposit, CreationDate = new DateTime(2020, 1, 1), LastUpdateDate = new DateTime(2020, 1, 1) };
			_context.Transactions.Add(deposit);
			_context.SaveChanges();

			Transaction payment = new Transaction { AccountId = newAccount.Id, Amount = 1000, Date = new DateTime(2020, 1, 1), TransactionStatus = TransactionStatusEnum.Pending, TransactionType = TransactionTypeEnum.Withdrawal, CreationDate = new DateTime(2020, 1, 1), LastUpdateDate = new DateTime(2020, 1, 1) };
			_context.Transactions.Add(payment);
			_context.SaveChanges();

			TransactionUpdaterService updater = new TransactionUpdaterService(_mockPaymentLogger.Object, _mapper, newAccount.Id, payment.Id, _accountRepo, _transRepo, TransactionStatusEnum.Processed, Messages.Payment_FailedToProcess);
			ServiceResult result = await updater.UpdateTransaction();
			result.Should().NotBeNull();
			result.StatusCode.Should().Be(StatusCodes.Status200OK);
			TransactionResultDto transaction = JsonConvert.DeserializeObject<TransactionResultDto>(result.ContentResult);
			transaction.Should().NotBeNull();
			transaction.AccountId.Should().Be(newAccount.Id);
			transaction.Amount.Should().Be(1000);
			transaction.Date.Should().Be(new DateTime(2020, 1, 1));
			transaction.TransactionStatus.Should().Be(TransactionStatusEnum.Processed.ToString());
		}

		[Fact]
		public async Task Unit_ProcessClosedPayment_ExpectBadRequest()
		{
			Account newAccount = new Account { Name = "Test Account" };
			_context.Accounts.Add(newAccount);
			_context.SaveChanges();

			Transaction deposit = new Transaction { AccountId = newAccount.Id, Amount = 1000000, Date = new DateTime(2020, 1, 1), TransactionStatus = TransactionStatusEnum.Processed, TransactionType = TransactionTypeEnum.Deposit, CreationDate = new DateTime(2020, 1, 1), LastUpdateDate = new DateTime(2020, 1, 1) };
			_context.Transactions.Add(deposit);
			_context.SaveChanges();

			Transaction payment = new Transaction { AccountId = newAccount.Id, Amount = 1000, Date = new DateTime(2020, 1, 1), TransactionStatus = TransactionStatusEnum.Closed, TransactionType = TransactionTypeEnum.Withdrawal, CreationDate = new DateTime(2020, 1, 1), LastUpdateDate = new DateTime(2020, 1, 1) };
			_context.Transactions.Add(payment);
			_context.SaveChanges();

			TransactionUpdaterService updater = new TransactionUpdaterService(_mockPaymentLogger.Object, _mapper, newAccount.Id, payment.Id, _accountRepo, _transRepo, TransactionStatusEnum.Processed, Messages.Payment_FailedToProcess);
			ServiceResult result = await updater.UpdateTransaction();
			result.Should().NotBeNull();
			result.StatusCode.Should().Be(StatusCodes.Status400BadRequest);
			result.ContentResult.Should().Contain(Messages.Payment_StatusIsClosed);
		}

		[Fact]
		public async Task Unit_ProcessProcessedPayment_ExpectBadRequest()
		{
			Account newAccount = new Account { Name = "Test Account" };
			_context.Accounts.Add(newAccount);
			_context.SaveChanges();

			Transaction deposit = new Transaction { AccountId = newAccount.Id, Amount = 1000000, Date = new DateTime(2020, 1, 1), TransactionStatus = TransactionStatusEnum.Processed, TransactionType = TransactionTypeEnum.Deposit, CreationDate = new DateTime(2020, 1, 1), LastUpdateDate = new DateTime(2020, 1, 1) };
			_context.Transactions.Add(deposit);
			_context.SaveChanges();

			Transaction payment = new Transaction { AccountId = newAccount.Id, Amount = 1000, Date = new DateTime(2020, 1, 1), TransactionStatus = TransactionStatusEnum.Processed, TransactionType = TransactionTypeEnum.Withdrawal, CreationDate = new DateTime(2020, 1, 1), LastUpdateDate = new DateTime(2020, 1, 1) };
			_context.Transactions.Add(payment);
			_context.SaveChanges();

			TransactionUpdaterService updater = new TransactionUpdaterService(_mockPaymentLogger.Object, _mapper, newAccount.Id, payment.Id, _accountRepo, _transRepo, TransactionStatusEnum.Processed, Messages.Payment_FailedToProcess);
			ServiceResult result = await updater.UpdateTransaction();
			result.Should().NotBeNull();
			result.StatusCode.Should().Be(StatusCodes.Status400BadRequest);
			result.ContentResult.Should().Contain(Messages.Payment_StatusIsProcessed);
		}

		[Fact]
		public async Task Unit_ProcessPendingPayment_TransactionDoesNotExist_ExpectNotFound()
		{
			Account newAccount = new Account { Name = "Test Account" };
			_context.Accounts.Add(newAccount);
			_context.SaveChanges();

			Transaction deposit = new Transaction { AccountId = newAccount.Id, Amount = 1000000, Date = new DateTime(2020, 1, 1), TransactionStatus = TransactionStatusEnum.Processed, TransactionType = TransactionTypeEnum.Deposit, CreationDate = new DateTime(2020, 1, 1), LastUpdateDate = new DateTime(2020, 1, 1) };
			_context.Transactions.Add(deposit);
			_context.SaveChanges();

			TransactionUpdaterService updater = new TransactionUpdaterService(_mockPaymentLogger.Object, _mapper, newAccount.Id, 10000, _accountRepo, _transRepo, TransactionStatusEnum.Processed, Messages.Payment_FailedToProcess);
			ServiceResult result = await updater.UpdateTransaction();
			result.Should().NotBeNull();
			result.StatusCode.Should().Be(StatusCodes.Status404NotFound);
			result.ContentResult.Should().Contain(Messages.Payment_NotFound);
		}

		[Fact]
		public async Task Unit_ProcessPendingPayment_AccountDoesNotExist_ExpectNotFound()
		{
			TransactionUpdaterService updater = new TransactionUpdaterService(_mockPaymentLogger.Object, _mapper, 1000, 10000, _accountRepo, _transRepo, TransactionStatusEnum.Processed, Messages.Payment_FailedToProcess);
			ServiceResult result = await updater.UpdateTransaction();
			result.Should().NotBeNull();
			result.StatusCode.Should().Be(StatusCodes.Status404NotFound);
			result.ContentResult.Should().Contain(Messages.Account_AccountNotFound);
		}

		#endregion Process Payment
	}
}