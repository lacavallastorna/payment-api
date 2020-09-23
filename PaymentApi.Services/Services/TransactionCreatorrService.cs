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
using System.Threading.Tasks;

namespace PaymentApi.Services.Services
{
	public class TransactionCreatorService : ITransactionCreatorService
	{
		private readonly IMapper _mapper;
		private readonly int _accountId;
		private readonly decimal _amount;
		private readonly DateTime _date;
		private readonly IAccountRepositoryAsync _accountRepo;
		private readonly ITransactionRepositoryAsync _transRepo;
		private readonly TransactionStatusEnum _status;
		private readonly TransactionTypeEnum _type;
		private readonly string _server500Error;
		private readonly ILogger _logger;

		public TransactionCreatorService(ILogger logger, IMapper mapper, int accountId, decimal amount, DateTime date, IAccountRepositoryAsync accountRepo, ITransactionRepositoryAsync transRepo, TransactionStatusEnum status, TransactionTypeEnum type, string server500Error)
		{
			this._logger = logger;
			this._mapper = mapper;
			this._accountId = accountId;
			this._amount = amount;
			this._date = date;
			this._accountRepo = accountRepo;
			this._transRepo = transRepo;
			this._status = status;
			this._type = type;
			this._server500Error = server500Error;
		}

		public async Task<ServiceResult> CreateTransaction()
		{
			try
			{
				Account account = await _accountRepo.GetAsync(_accountId);
				if (account == null)
				{
					return new ServiceResult { StatusCode = StatusCodes.Status404NotFound, ContentResult = JsonConvert.SerializeObject(new ErrorResponseDto { Message = Messages.Account_AccountNotFound }) };
				}

				DateTime creationDate = DateTime.Now;
				bool insufficientBalance = false;

				Transaction newTransaction = new Transaction
				{
					TransactionStatus = _status,
					TransactionType = _type,
					Date = _date,
					CreationDate = creationDate,
					LastUpdateDate = creationDate,
					AccountId = _accountId,
					Amount = _amount
				};

				if (_type == TransactionTypeEnum.Withdrawal)
				{
					decimal accountAvailableBalance = await _accountRepo.GetAccountBalanceAsync(account.Id);

					insufficientBalance = accountAvailableBalance < _amount;
					if (insufficientBalance)
					{
						newTransaction.TransactionStatus = TransactionStatusEnum.Closed;
						newTransaction.ClosedReason = Messages.Payment_NotEnoughFundsReason;
					}
				}

				bool result = await _transRepo.AddAsync(newTransaction);
				if (!result)
				{
					return new ServiceResult { StatusCode = StatusCodes.Status500InternalServerError, ContentResult = JsonConvert.SerializeObject(new ErrorResponseDto { Message = _server500Error }) };
				}
				return new ServiceResult { StatusCode = StatusCodes.Status201Created, ContentResult = JsonConvert.SerializeObject(_mapper.Map<TransactionResultDto>(newTransaction), new DecimalFormatConverter()) };
			}
			catch (Exception ex)
			{
				_logger.LogError(ex.Message, $"An error has occurred executing CreateTransaction for Account {_accountId}, Amount {_amount}, Type {_type.ToString()}, Date {_date.ToShortDateString()}");
				return new ServiceResult { StatusCode = StatusCodes.Status500InternalServerError, ContentResult = JsonConvert.SerializeObject(new ErrorResponseDto { Message = ex.Message }) };
			}
		}
	}
}