using FluentAssertions;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Newtonsoft.Json;
using PaymentApi.Api;
using PaymentApi.Resources.Constants;
using PaymentApi.DataAccess.Data;
using PaymentApi.Models.Models;
using PaymentApi.Models.Models.Dtos;
using Serilog;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace PaymentApi.XUnitTests.Integration
{
	public class PaymentController_CreateTests : IDisposable
	{
		private readonly ApplicationDbContext _context;
		private readonly HttpClient _client;

		public PaymentController_CreateTests()
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

		[Fact]
		public async Task Integration_CreateNewPayment_ExpectPaymentInResultAndRepository()
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
			_context.SaveChanges();

			TransactionInsertDto payment = new TransactionInsertDto
			{
				AccountId = newAccount.Id,
				Amount = 1000,
				Date = new DateTime(2020, 1, 1)
			};

			var content = JsonConvert.SerializeObject(payment);
			var stringContent = new StringContent(content, Encoding.UTF8, "application/json");
			var response = await _client.PostAsync("/api/payment/create", stringContent);
			response.StatusCode.Should().Be(System.Net.HttpStatusCode.Created);
			var responseString = await response.Content.ReadAsStringAsync();
			TransactionResultDto PaymentResult = JsonConvert.DeserializeObject<TransactionResultDto>(responseString);
			PaymentResult.Should().NotBeNull();
			PaymentResult.AccountId.Should().Be(newAccount.Id);
			PaymentResult.Amount.Should().Be(payment.Amount);
			PaymentResult.Date.Should().Be((DateTime)payment.Date);
			PaymentResult.ClosedReason.Should().BeNull();
			PaymentResult.TransactionStatus.Should().Be(TransactionStatusEnum.Pending.ToString());

			Transaction PaymentFromDb = _context.Transactions.Find(PaymentResult.Id);
			PaymentFromDb.Should().NotBeNull();
			PaymentFromDb.Amount.Should().Be(payment.Amount);
			PaymentFromDb.Date.Should().Be((DateTime)payment.Date);
			PaymentFromDb.Id.Should().Be(PaymentResult.Id);
			PaymentFromDb.TransactionStatus.Should().Be(TransactionStatusEnum.Pending);
			PaymentFromDb.TransactionType.Should().Be(TransactionTypeEnum.Withdrawal);
		}

		[Fact]
		public async Task Integration_CreateNewPayment_AccountDoesNotExists_ExpectNotFound()
		{
			var content = JsonConvert.SerializeObject(new TransactionInsertDto { AccountId = -100, Date = new DateTime(2020, 1, 1), Amount = 1000 });
			var stringContent = new StringContent(content, Encoding.UTF8, "application/json");
			var response = await _client.PostAsync("/api/payment/create", stringContent);
			response.StatusCode.Should().Be(System.Net.HttpStatusCode.NotFound);
			var responseString = await response.Content.ReadAsStringAsync();
			responseString.Should().Contain(Messages.Account_AccountNotFound);
		}

		[Fact]
		public async Task Integration_CreateNewPayment_NoBalance_ExpectCreated_ButTransactionCreatedAndClosed()
		{
			Account newAccount = new Account { Name = "Test Account" };
			_context.Accounts.Add(newAccount);
			_context.SaveChanges();
			TransactionInsertDto payment = new TransactionInsertDto { AccountId = newAccount.Id, Date = new DateTime(2020, 1, 1), Amount = 1000 };
			var content = JsonConvert.SerializeObject(payment);
			var stringContent = new StringContent(content, Encoding.UTF8, "application/json");
			var response = await _client.PostAsync("/api/payment/create", stringContent);
			response.StatusCode.Should().Be(System.Net.HttpStatusCode.Created);
			var responseString = await response.Content.ReadAsStringAsync();
			TransactionResultDto PaymentResult = JsonConvert.DeserializeObject<TransactionResultDto>(responseString);

			PaymentResult.Should().NotBeNull();
			PaymentResult.AccountId.Should().Be(newAccount.Id);
			PaymentResult.Amount.Should().Be(payment.Amount);
			PaymentResult.Date.Should().Be((DateTime)payment.Date);
			PaymentResult.ClosedReason.Should().Be(Messages.Payment_NotEnoughFundsReason);
			PaymentResult.TransactionStatus.Should().Be(TransactionStatusEnum.Closed.ToString());

			Transaction PaymentFromDb = _context.Transactions.Find(PaymentResult.Id);
			PaymentFromDb.Should().NotBeNull();
			PaymentFromDb.Amount.Should().Be(payment.Amount);
			PaymentFromDb.Date.Should().Be((DateTime)payment.Date);
			PaymentFromDb.Id.Should().Be(PaymentResult.Id);
			PaymentFromDb.TransactionStatus.Should().Be(TransactionStatusEnum.Closed);
			PaymentFromDb.TransactionType.Should().Be(TransactionTypeEnum.Withdrawal);
			PaymentFromDb.ClosedReason.Should().Be(Messages.Payment_NotEnoughFundsReason);
		}

		#region 400 Errors handled by [apicontroller]

		public static IEnumerable<object[]> GetCreateData_EmptyOrNullFields
		{
			get
			{
				return new[]
				{
				new object[] { new TransactionInsertDto() },
				new object[] { new TransactionInsertDto { AccountId = null, Amount = null, Date = null } },
				};
			}
		}

		[Theory]
		[MemberData(nameof(GetCreateData_EmptyOrNullFields))]
		public async Task Integration_CreateNewPayment_EmptyOrNullFields_ExpectBadRequest(TransactionInsertDto objDto)
		{
			var content = JsonConvert.SerializeObject(objDto);
			var stringContent = new StringContent(content, Encoding.UTF8, "application/json");
			var response = await _client.PostAsync("/api/payment/create", stringContent);
			response.StatusCode.Should().Be(System.Net.HttpStatusCode.BadRequest);
			var responseString = await response.Content.ReadAsStringAsync();
			responseString.Should().Contain("The AccountId field is required");
			responseString.Should().Contain("The Amount field is required");
			responseString.Should().Contain("The Date field is required");
		}

		[Fact]
		public async Task Integration_CreateNewPayment_EmptyBody_ExpectBadRequest()
		{
			var content = JsonConvert.SerializeObject(null);
			var stringContent = new StringContent(content, Encoding.UTF8, "application/json");
			var response = await _client.PostAsync("/api/payment/create", stringContent);
			response.StatusCode.Should().Be(System.Net.HttpStatusCode.BadRequest);
			var responseString = await response.Content.ReadAsStringAsync();
			responseString.Should().Contain("A non-empty request body is required.");
		}

		[Theory]
		[InlineData(-1)]
		[InlineData(0)]
		[InlineData(0.001)]
		[InlineData(9999999999999999999999999999.0)]
		public async Task Integration_CreateNewPayment_OutOfRangeAmount_ExpectBadRequest(decimal amount)
		{
			var content = JsonConvert.SerializeObject(new TransactionInsertDto { AccountId = 1, Amount = amount, Date = new DateTime(2020, 1, 1) });
			var stringContent = new StringContent(content, Encoding.UTF8, "application/json");
			var response = await _client.PostAsync("/api/payment/create", stringContent);
			response.StatusCode.Should().Be(System.Net.HttpStatusCode.BadRequest);
			var responseString = await response.Content.ReadAsStringAsync();
			responseString.Should().Contain("Amount must be between 0.01 and 999999999999999.99");
		}

		#endregion 400 Errors handled by [apicontroller]
	}
}