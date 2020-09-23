using AutoMapper;
using FluentAssertions;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using Newtonsoft.Json;
using PaymentApi.Api;
using PaymentApi.Resources.Constants;
using PaymentApi.Api.Controllers;
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

//using System.Transactions;
using Xunit;

namespace PaymentApi.XUnitTests.Unit
{
	public class AccountCreatorServiceTests
	{
		private readonly ApplicationDbContext _context;
		private readonly AccountRepositoryAsync _accountRepo;
		private readonly TransactionRepositoryAsync _transRepo;
		private readonly Mock<ILogger<AccountController>> _mockLogger;

		public AccountCreatorServiceTests()
		{
			var options = new DbContextOptionsBuilder<ApplicationDbContext>().UseInMemoryDatabase(Guid.NewGuid().ToString()).Options;
			_context = new ApplicationDbContext(options);
			_accountRepo = new AccountRepositoryAsync(_context);
			_transRepo = new TransactionRepositoryAsync(_context);
			_mockLogger = new Mock<ILogger<AccountController>>();
		}

		[Fact]
		public async Task Unit_CreateNewAccount_ExpectAccountInResultAndRepository()
		{
			var newAccountName = "Johnny";
			AccountCreatorService creator = new AccountCreatorService(_mockLogger.Object, newAccountName, _accountRepo);
			ServiceResult result = await creator.CreateAccount();
			result.Should().NotBeNull();
			AccountInsertResultDto newAccount = JsonConvert.DeserializeObject<AccountInsertResultDto>(result.ContentResult);
			newAccount.Should().NotBeNull();
			newAccount.Name.Should().Be(newAccountName);
			newAccount.AccountId.Should().BeGreaterThan(0);
			Account newAccountFromDb = await _accountRepo.GetAsync(newAccount.AccountId);
			newAccountFromDb.Should().NotBeNull();
			newAccountFromDb.Name.Should().Be(newAccount.Name);
			newAccountFromDb.Id.Should().Be(newAccount.AccountId);
		}
	}
}