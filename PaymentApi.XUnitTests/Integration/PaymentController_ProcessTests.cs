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
	public class PaymentController_ProcessTests : IDisposable
	{
		private ApplicationDbContext _context;
		private readonly HttpClient _client;
		private readonly TestServer _server;

		public PaymentController_ProcessTests()
		{
			var builder = new WebHostBuilder().UseEnvironment("Testing")
				.UseSerilog()
				.UseStartup<Startup>();
			_server = new TestServer(builder);
			_context = _server.Host.Services.GetService(typeof(ApplicationDbContext)) as ApplicationDbContext;
			_client = _server.CreateClient();
		}

		public void Dispose()
		{
			_context.Transactions.RemoveRange(_context.Transactions);
			_context.Accounts.RemoveRange(_context.Accounts);
			_context.SaveChanges();
		}

		[Fact]
		public async Task Integration_ProcessPayment_ExpectPaymentInResultAndRepository()
		{
			Account newAccount = new Account { Name = "Test Account" };
			_context.Accounts.Add(newAccount);
			_context.SaveChanges();
			// 1 Deposits of 10000
			Transaction deposit = new Transaction
			{
				AccountId = newAccount.Id,
				Amount = 1000,
				TransactionStatus = TransactionStatusEnum.Processed,
				TransactionType = TransactionTypeEnum.Deposit,
				Date = new DateTime(2020, 1, 1),
				CreationDate = new DateTime(2020, 1, 1),
				LastUpdateDate = new DateTime(2020, 1, 1)
			};
			_context.Transactions.Add(deposit);

			// 1 Payment of 1000
			Transaction newPayment = new Transaction
			{
				AccountId = newAccount.Id,
				Amount = 1000,
				TransactionStatus = TransactionStatusEnum.Pending,
				TransactionType = TransactionTypeEnum.Withdrawal,
				Date = new DateTime(2020, 1, 1),
				CreationDate = new DateTime(2020, 1, 1),
				LastUpdateDate = new DateTime(2020, 1, 1)
			};
			_context.Transactions.Add(newPayment);
			_context.SaveChanges();

			TransactionProcessDto payment = new TransactionProcessDto
			{
				AccountId = newPayment.AccountId,
				TransactionId = newPayment.Id
			};

			var content = JsonConvert.SerializeObject(payment);
			var stringContent = new StringContent(content, Encoding.UTF8, "application/json");
			var response = await _client.PutAsync("/api/payment/process", stringContent);
			response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
			var responseString = await response.Content.ReadAsStringAsync();
			TransactionResultDto PaymentResult = JsonConvert.DeserializeObject<TransactionResultDto>(responseString);
			PaymentResult.Should().NotBeNull();
			PaymentResult.AccountId.Should().Be(newAccount.Id);
			PaymentResult.Amount.Should().Be(newPayment.Amount);
			PaymentResult.Date.Should().Be((DateTime)newPayment.Date);
			PaymentResult.ClosedReason.Should().BeNull();
			PaymentResult.TransactionStatus.Should().Be(TransactionStatusEnum.Processed.ToString());

			Transaction PaymentFromDb = _context.Transactions.Find(PaymentResult.Id);

			_context.Entry(PaymentFromDb).Reload();

			PaymentFromDb.Should().NotBeNull();
			PaymentFromDb.Amount.Should().Be(PaymentResult.Amount);
			PaymentFromDb.Date.Should().Be((DateTime)PaymentResult.Date);
			PaymentFromDb.Id.Should().Be(PaymentResult.Id);
			PaymentFromDb.TransactionStatus.Should().Be(TransactionStatusEnum.Processed);
			PaymentFromDb.TransactionType.Should().Be(TransactionTypeEnum.Withdrawal);
			PaymentFromDb.ClosedReason.Should().BeNull();
		}

		[Fact]
		public async Task Integration_ProcessPayment_AccountDoesNotExists_ExpectNotFound()
		{
			var content = JsonConvert.SerializeObject(new TransactionProcessDto { AccountId = -100, TransactionId = 1 });
			var stringContent = new StringContent(content, Encoding.UTF8, "application/json");
			var response = await _client.PutAsync("/api/payment/process", stringContent);
			response.StatusCode.Should().Be(System.Net.HttpStatusCode.NotFound);
			var responseString = await response.Content.ReadAsStringAsync();
			responseString.Should().Contain(Messages.Account_AccountNotFound);
		}

		[Fact]
		public async Task Integration_ProcessPayment_PaymentDoesNotExists_ExpectNotFound()
		{
			Account newAccount = new Account { Name = "Test Account" };
			_context.Accounts.Add(newAccount);
			_context.SaveChanges();

			var content = JsonConvert.SerializeObject(new TransactionProcessDto { AccountId = newAccount.Id, TransactionId = 1 });
			var stringContent = new StringContent(content, Encoding.UTF8, "application/json");
			var response = await _client.PutAsync("/api/payment/process", stringContent);
			response.StatusCode.Should().Be(System.Net.HttpStatusCode.NotFound);
			var responseString = await response.Content.ReadAsStringAsync();
			responseString.Should().Contain(Messages.Payment_NotFound);
		}

		[Fact]
		public async Task Integration_ProcessPayment_PaymentIsProcessed_ExpectBadRequest()
		{
			Account newAccount = new Account { Name = "Test Account" };
			_context.Accounts.Add(newAccount);
			_context.SaveChanges();
			// 1 Payment of 1000
			Transaction newPayment = new Transaction
			{
				AccountId = newAccount.Id,
				Amount = 1000,
				TransactionStatus = TransactionStatusEnum.Processed,
				TransactionType = TransactionTypeEnum.Withdrawal,
				Date = new DateTime(2020, 1, 1),
				CreationDate = new DateTime(2020, 1, 1),
				LastUpdateDate = new DateTime(2020, 1, 1)
			};
			_context.Transactions.Add(newPayment);
			_context.SaveChanges();

			var content = JsonConvert.SerializeObject(new TransactionProcessDto { AccountId = newAccount.Id, TransactionId = newPayment.Id });
			var stringContent = new StringContent(content, Encoding.UTF8, "application/json");
			var response = await _client.PutAsync("/api/payment/process", stringContent);
			response.StatusCode.Should().Be(System.Net.HttpStatusCode.BadRequest);
			var responseString = await response.Content.ReadAsStringAsync();
			responseString.Should().Contain(Messages.Payment_StatusIsProcessed);
		}

		[Fact]
		public async Task Integration_ProcessPayment_PaymentIsClosed_ExpectBadRequest()
		{
			Account newAccount = new Account { Name = "Test Account" };
			_context.Accounts.Add(newAccount);
			_context.SaveChanges();
			// 1 Payment of 1000
			Transaction newPayment = new Transaction
			{
				AccountId = newAccount.Id,
				Amount = 1000,
				TransactionStatus = TransactionStatusEnum.Closed,
				TransactionType = TransactionTypeEnum.Withdrawal,
				Date = new DateTime(2020, 1, 1),
				CreationDate = new DateTime(2020, 1, 1),
				LastUpdateDate = new DateTime(2020, 1, 1)
			};
			_context.Transactions.Add(newPayment);
			_context.SaveChanges();

			var content = JsonConvert.SerializeObject(new TransactionProcessDto { AccountId = newAccount.Id, TransactionId = newPayment.Id });
			var stringContent = new StringContent(content, Encoding.UTF8, "application/json");
			var response = await _client.PutAsync("/api/payment/process", stringContent);
			response.StatusCode.Should().Be(System.Net.HttpStatusCode.BadRequest);
			var responseString = await response.Content.ReadAsStringAsync();
			responseString.Should().Contain(Messages.Payment_StatusIsClosed);
		}

		#region 400 Errors handled by [apicontroller]

		public static IEnumerable<object[]> GetProcessData_EmptyOrNullFields
		{
			get
			{
				return new[]
				{
				new object[] { new TransactionProcessDto() },
				new object[] { new TransactionProcessDto { AccountId = null, TransactionId = null } },
				};
			}
		}

		[Theory]
		[MemberData(nameof(GetProcessData_EmptyOrNullFields))]
		public async Task Integration_ProcessPayment_EmptyOrNullFields_ExpectBadRequest(TransactionProcessDto objDto)
		{
			var content = JsonConvert.SerializeObject(objDto);
			var stringContent = new StringContent(content, Encoding.UTF8, "application/json");
			var response = await _client.PutAsync("/api/payment/process", stringContent);
			response.StatusCode.Should().Be(System.Net.HttpStatusCode.BadRequest);
			var responseString = await response.Content.ReadAsStringAsync();
			responseString.Should().Contain("The AccountId field is required");
			responseString.Should().Contain("The TransactionId field is required");
		}

		[Fact]
		public async Task Integration_ProcessPayment_EmptyBody_ExpectBadRequest()
		{
			var content = JsonConvert.SerializeObject(null);
			var stringContent = new StringContent(content, Encoding.UTF8, "application/json");
			var response = await _client.PutAsync("/api/payment/process", stringContent);
			response.StatusCode.Should().Be(System.Net.HttpStatusCode.BadRequest);
			var responseString = await response.Content.ReadAsStringAsync();
			responseString.Should().Contain("A non-empty request body is required.");
		}

		#endregion 400 Errors handled by [apicontroller]
	}
}