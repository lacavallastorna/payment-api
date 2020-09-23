using FluentAssertions;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using PaymentApi.Api;
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
	public class DepositControllerTests : IDisposable
	{
		private readonly ApplicationDbContext _context;
		private readonly HttpClient _client;

		public DepositControllerTests()
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
		public async Task Integration_CreateNewDeposit_ExpectAccountInResultAndRepository()
		{
			Account newAccount = new Account { Name = "Test Account" };
			_context.Accounts.Add(newAccount);
			_context.SaveChanges();

			TransactionInsertDto deposit = new TransactionInsertDto
			{
				AccountId = newAccount.Id,
				Amount = 1000,
				Date = new DateTime(2020, 1, 1)
			};

			var content = JsonConvert.SerializeObject(deposit);
			var stringContent = new StringContent(content, Encoding.UTF8, "application/json");
			var response = await _client.PostAsync("/api/deposit/create", stringContent);
			response.StatusCode.Should().Be(System.Net.HttpStatusCode.Created);
			var responseString = await response.Content.ReadAsStringAsync();
			TransactionResultDto depositResult = JsonConvert.DeserializeObject<TransactionResultDto>(responseString);
			depositResult.Should().NotBeNull();
			depositResult.AccountId.Should().Be(newAccount.Id);
			Transaction depositFromDb = _context.Transactions.Find(depositResult.Id);
			depositFromDb.Should().NotBeNull();
			depositFromDb.Amount.Should().Be(deposit.Amount);
			depositFromDb.Date.Should().Be((DateTime)deposit.Date);
			depositFromDb.Id.Should().Be(depositResult.Id);
			depositFromDb.TransactionStatus.Should().Be(TransactionStatusEnum.Processed);
			depositFromDb.TransactionType.Should().Be(TransactionTypeEnum.Deposit);
		}

		#region 400 Errors handled by [apicontroller]

		public static IEnumerable<object[]> GetTransactionInsertDtos_EmptyOrNullFields
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
		[MemberData(nameof(GetTransactionInsertDtos_EmptyOrNullFields))]
		public async Task Integration_CreateNewDeposit_EmptyOrNullFields_ExpectBadRequest(TransactionInsertDto objDto)
		{
			var content = JsonConvert.SerializeObject(objDto);
			var stringContent = new StringContent(content, Encoding.UTF8, "application/json");
			var response = await _client.PostAsync("/api/deposit/create", stringContent);
			response.StatusCode.Should().Be(System.Net.HttpStatusCode.BadRequest);
			var responseString = await response.Content.ReadAsStringAsync();
			responseString.Should().Contain("The AccountId field is required");
			responseString.Should().Contain("The Amount field is required");
			responseString.Should().Contain("The Date field is required");
		}

		[Fact]
		public async Task Integration_CreateNewDeposit_EmptyBody_ExpectBadRequest()
		{
			var content = JsonConvert.SerializeObject(null);
			var stringContent = new StringContent(content, Encoding.UTF8, "application/json");
			var response = await _client.PostAsync("/api/deposit/create", stringContent);
			response.StatusCode.Should().Be(System.Net.HttpStatusCode.BadRequest);
			var responseString = await response.Content.ReadAsStringAsync();
			responseString.Should().Contain("A non-empty request body is required.");
		}

		[Theory]
		[InlineData(-1)]
		[InlineData(0)]
		[InlineData(0.001)]
		[InlineData(9999999999999999999999999999.0)]
		public async Task Integration_CreateNewDeposit_OutOfRangeAmount_ExpectBadRequest(decimal amount)
		{
			var content = JsonConvert.SerializeObject(new TransactionInsertDto { AccountId = 1, Amount = amount, Date = new DateTime(2020, 1, 1) });
			var stringContent = new StringContent(content, Encoding.UTF8, "application/json");
			var response = await _client.PostAsync("/api/deposit/create", stringContent);
			response.StatusCode.Should().Be(System.Net.HttpStatusCode.BadRequest);
			var responseString = await response.Content.ReadAsStringAsync();
			responseString.Should().Contain("Amount must be between 0.01 and 999999999999999.99");
		}

		#endregion 400 Errors handled by [apicontroller]
	}
}