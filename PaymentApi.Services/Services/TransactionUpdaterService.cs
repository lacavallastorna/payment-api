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
	public class TransactionUpdaterService : ITransactionUpdaterService
	{
		private readonly IMapper _mapper;
		private readonly int _accountId;
		private readonly int _transactionId;
		private readonly IAccountRepositoryAsync _accountRepo;
		private readonly ITransactionRepositoryAsync _transRepo;
		private readonly TransactionStatusEnum _status;
		private readonly string _server500Error;
		private readonly string _closedComment;
		private readonly ILogger _logger;

		public TransactionUpdaterService(ILogger logger, IMapper mapper, int accountId, int transactionId, IAccountRepositoryAsync accountRepo, ITransactionRepositoryAsync transRepo, TransactionStatusEnum status, string server500Error, string closedComment = null)
		{
			this._logger = logger;
			this._mapper = mapper;
			this._accountId = accountId;
			this._transactionId = transactionId;
			this._accountRepo = accountRepo;
			this._transRepo = transRepo;
			this._status = status;
			this._server500Error = server500Error;
			this._closedComment = closedComment;
		}

		public async Task<ServiceResult> UpdateTransaction()
		{
			try
			{
				Account account = await _accountRepo.GetAsync(_accountId);
				if (account == null)
				{
					return new ServiceResult { StatusCode = StatusCodes.Status404NotFound, ContentResult = JsonConvert.SerializeObject(new ErrorResponseDto { Message = Messages.Account_AccountNotFound }) };
				}

				Transaction paymentFromDb = await _transRepo.GetFirstOrDefaultAsync(t => t.Id == _transactionId && t.TransactionType == TransactionTypeEnum.Withdrawal && t.AccountId == _accountId);
				if (paymentFromDb == null)
				{
					return new ServiceResult { StatusCode = StatusCodes.Status404NotFound, ContentResult = JsonConvert.SerializeObject(new ErrorResponseDto { Message = Messages.Payment_NotFound }) };
				}

				if (paymentFromDb.TransactionStatus == TransactionStatusEnum.Processed)
				{
					return new ServiceResult { StatusCode = StatusCodes.Status400BadRequest, ContentResult = JsonConvert.SerializeObject(new ErrorResponseDto { Message = Messages.Payment_StatusIsProcessed }) };
				}

				if (paymentFromDb.TransactionStatus == TransactionStatusEnum.Closed)
				{
					return new ServiceResult { StatusCode = StatusCodes.Status400BadRequest, ContentResult = JsonConvert.SerializeObject(new ErrorResponseDto { Message = Messages.Payment_StatusIsClosed }) };
				}

				paymentFromDb.TransactionStatus = _status;
				paymentFromDb.LastUpdateDate = DateTime.Now; ;
				if (_closedComment != null)
				{
					paymentFromDb.ClosedReason = _closedComment;
				}

				bool result = await _transRepo.UpdateAsync(paymentFromDb);
				if (!result)
				{
					return new ServiceResult { StatusCode = StatusCodes.Status500InternalServerError, ContentResult = JsonConvert.SerializeObject(new ErrorResponseDto { Message = _server500Error }) };
				}
				return new ServiceResult { StatusCode = StatusCodes.Status200OK, ContentResult = JsonConvert.SerializeObject(_mapper.Map<TransactionResultDto>(paymentFromDb), new DecimalFormatConverter()) };
			}
			catch (Exception ex)
			{
				_logger.LogError(ex.Message, $"An error has occurred executing UpdateTransaction for Account {_accountId}, TransactionId {_transactionId}");
				return new ServiceResult { StatusCode = StatusCodes.Status500InternalServerError, ContentResult = JsonConvert.SerializeObject(new ErrorResponseDto { Message = ex.Message }) };
			}
		}
	}
}