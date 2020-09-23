using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using PaymentApi.DataAccess.Repository.Interfaces;
using PaymentApi.Models.Models;
using PaymentApi.Models.Models.Dtos;
using PaymentApi.Resources.Constants;
using PaymentApi.Services.Interfaces;
using PaymentApi.Services.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PaymentApi.Services.Services
{
	public class AccountBalanceService : IAccountBalanceService
	{
		private readonly ILogger _logger;
		private readonly int _accountId;
		private readonly IMapper _mapper;
		private readonly IAccountRepositoryAsync _accountRepo;
		private readonly ITransactionRepositoryAsync _transRepo;

		public AccountBalanceService(ILogger logger, int accountId, IMapper mapper, IAccountRepositoryAsync accountRepo, ITransactionRepositoryAsync transRepo)
		{
			this._logger = logger;
			this._accountId = accountId;
			this._mapper = mapper;
			this._accountRepo = accountRepo;
			this._transRepo = transRepo;
		}

		public async Task<ServiceResult> GetAccountBalance()
		{
			try
			{
				if (_accountId <= 0)
				{
					return new ServiceResult { StatusCode = StatusCodes.Status400BadRequest, ContentResult = JsonConvert.SerializeObject(new ErrorResponseDto { Message = Messages.Account_InvalidAccountId }) };
				}

				Account account = await _accountRepo.GetAsync(_accountId);
				if (account == null)
				{
					return new ServiceResult { StatusCode = StatusCodes.Status404NotFound, ContentResult = JsonConvert.SerializeObject(new ErrorResponseDto { Message = Messages.Account_AccountNotFound }) };
				}

				IEnumerable<Transaction> payments = await _transRepo.GetAllAsync(t => t.AccountId == _accountId && t.TransactionType == TransactionTypeEnum.Withdrawal, orderBy: q => q.OrderByDescending(t => t.Date));
				IEnumerable<Transaction> deposits = await _transRepo.GetAllAsync(t => t.AccountId == _accountId && t.TransactionType == TransactionTypeEnum.Deposit, orderBy: q => q.OrderByDescending(t => t.Date));

				AccountBalanceResultDto result = new AccountBalanceResultDto
				{
					AccountId = _accountId,
					OpeningBalance = await _accountRepo.GetAccountOpeningBalanceAsync(_accountId),
					ProcessedPaymentsBalance = await _accountRepo.GetAccountProcessedBalanceAsync(_accountId),
					ClosingBalance = await _accountRepo.GetAccountBalanceAsync(_accountId),
					PendingdPaymentsBalance = await _accountRepo.GetAccountPendingBalanceAsync(_accountId),
					Payments = _mapper.Map<List<PaymentTransactionResultDto>>(payments),
					Deposits = _mapper.Map<List<DepositTransactionResultDto>>(deposits),
				};
				return new ServiceResult { StatusCode = StatusCodes.Status200OK, ContentResult = JsonConvert.SerializeObject(result, new DecimalFormatConverter()) };
			}
			catch (Exception ex)
			{
				_logger.LogError(ex.Message, $"An error has occurred executing GetAccountBalance for Account {_accountId}");
				return new ServiceResult { StatusCode = StatusCodes.Status500InternalServerError, ContentResult = JsonConvert.SerializeObject(new ErrorResponseDto { Message = ex.Message }) };
			}
		}
	}
}