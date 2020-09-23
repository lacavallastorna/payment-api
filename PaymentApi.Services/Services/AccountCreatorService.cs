using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using PaymentApi.DataAccess.Repository.Interfaces;
using PaymentApi.Models.Models;
using PaymentApi.Models.Models.Dtos;
using PaymentApi.Resources.Constants;
using PaymentApi.Services.Interfaces;
using System;
using System.Threading.Tasks;

namespace PaymentApi.Services.Services
{
	public class AccountCreatorService : IAccountCreatorService
	{
		private readonly IAccountRepositoryAsync _accountRepo;
		private readonly string _name;
		private readonly ILogger _logger;

		public AccountCreatorService(ILogger logger, string name, IAccountRepositoryAsync accountRepo)
		{
			this._logger = logger;
			this._accountRepo = accountRepo;
			this._name = name;
		}

		public async Task<ServiceResult> CreateAccount()
		{
			try
			{
				Account newAccount = new Account() { Name = _name, CreationDate = DateTime.Now };
				bool result = await _accountRepo.AddAsync(newAccount);
				if (!result)
				{
					return new ServiceResult { StatusCode = StatusCodes.Status500InternalServerError, ContentResult = JsonConvert.SerializeObject(new ErrorResponseDto { Message = Messages.Account_FailedToCreate }) };
				}
				return new ServiceResult { StatusCode = StatusCodes.Status201Created, ContentResult = JsonConvert.SerializeObject(new AccountInsertResultDto { AccountId = newAccount.Id, Name = newAccount.Name }) };
			}
			catch (Exception ex)
			{
				_logger.LogError(ex.Message, $"An error has occurred executing CreateAccount for Name {_name}");
				return new ServiceResult { StatusCode = StatusCodes.Status500InternalServerError, ContentResult = JsonConvert.SerializeObject(new ErrorResponseDto { Message = ex.Message }) };
			}
		}
	}
}