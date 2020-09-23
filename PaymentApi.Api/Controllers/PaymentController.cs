using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using PaymentApi.Resources.Constants;
using PaymentApi.Api.Controllers.Extensions;
using PaymentApi.Services.Services;
using PaymentApi.DataAccess.Repository.Interfaces;
using PaymentApi.Models.Models;
using PaymentApi.Models.Models.Dtos;
using System;
using System.Threading.Tasks;

namespace PaymentApi.Api.Controllers
{
	[ApiController]
	[Route("api/payment")]
	public class PaymentController : ControllerBase
	{
		private readonly IAccountRepositoryAsync _accountRepo;
		private readonly ITransactionRepositoryAsync _transRepo;
		private readonly IMapper _mapper;
		private readonly ILogger<PaymentController> _logger;

		public PaymentController(ILogger<PaymentController> logger, IAccountRepositoryAsync accountRepo, ITransactionRepositoryAsync tranRepo, IMapper mapper)
		{
			_logger = logger;
			_accountRepo = accountRepo;
			_transRepo = tranRepo;
			_mapper = mapper;
		}

		/// <summary>
		/// Creates a new Payment Request. If successful the transaction is created with a Status of Pending
		/// </summary>
		/// <param name="objDto">Json object with transaction details, such as AccountID, Amount and Date </param>
		/// <returns>Json object with details of the withdraw transaction created.</returns>
		[HttpPost("create")]
		[ProducesResponseType(StatusCodes.Status201Created, Type = typeof(TransactionResultDto))]
		[ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ErrorResponseDto))]
		[ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ErrorResponseDto))]
		[ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ErrorResponseDto))]
		[ProducesDefaultResponseType]
		public async Task<IActionResult> CreateNewPayment([FromBody] TransactionInsertDto objDto)
		{
			TransactionCreatorService creator = new TransactionCreatorService(_logger, _mapper, (int)objDto.AccountId, (decimal)objDto.Amount, (DateTime)objDto.Date, _accountRepo, _transRepo, TransactionStatusEnum.Pending, TransactionTypeEnum.Withdrawal, Messages.Payment_FailedToCreate);
			return this.GetActionResultFromServiceResult(await creator.CreateTransaction());
		}

		/// <summary>
		/// Attempts to change the status of a Payment Transaction to Processed which if successful will decrease accordingly the Account Balance.
		/// </summary>
		/// <param name="objDto">Json object with transaction details, such as AccountID, and Transaction Id</param>
		/// <returns>Json object with details of the payment transaction being processed.</returns>
		[HttpPut("process")]
		[ProducesResponseType(StatusCodes.Status200OK, Type = typeof(TransactionResultDto))]
		[ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ErrorResponseDto))]
		[ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ErrorResponseDto))]
		[ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ErrorResponseDto))]
		[ProducesDefaultResponseType]
		public async Task<IActionResult> ProcessPayment([FromBody] TransactionProcessDto objDto)
		{
			TransactionUpdaterService updater = new TransactionUpdaterService(_logger, _mapper, (int)objDto.AccountId, (int)objDto.TransactionId, _accountRepo, _transRepo, TransactionStatusEnum.Processed, Messages.Payment_FailedToProcess, null);
			return this.GetActionResultFromServiceResult(await updater.UpdateTransaction());
		}

		/// <summary>
		/// Attempts to Cancel a Payment with a Status of Pending. If successful the Status of the Payment Transaction is changed to Closed and the ClosedComment field updated with specified Reason text.
		/// </summary>
		/// <param name="objDto">Json object with transaction details, such as AccountID, and Transaction Id and Reason for Cancellation</param>
		/// <returns>Json object with details of the payment transaction being cancelled.</returns>
		[HttpPut("cancel")]
		[ProducesResponseType(StatusCodes.Status200OK, Type = typeof(TransactionResultDto))]
		[ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ErrorResponseDto))]
		[ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ErrorResponseDto))]
		[ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ErrorResponseDto))]
		[ProducesDefaultResponseType]
		public async Task<IActionResult> CancelPayment([FromBody] TransactionCancelDto objDto)
		{
			TransactionUpdaterService updater = new TransactionUpdaterService(_logger, _mapper, (int)objDto.AccountId, (int)objDto.TransactionId, _accountRepo, _transRepo, TransactionStatusEnum.Closed, Messages.Payment_FailedToCancel, objDto.Reason);
			return this.GetActionResultFromServiceResult(await updater.UpdateTransaction());
		}
	}
}