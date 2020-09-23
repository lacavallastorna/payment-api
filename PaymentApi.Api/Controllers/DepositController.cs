using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using PaymentApi.Resources.Constants;
using PaymentApi.Api.Controllers.Extensions;
using PaymentApi.DataAccess.Repository.Interfaces;
using PaymentApi.Models.Models;
using PaymentApi.Models.Models.Dtos;
using System;
using System.Threading.Tasks;
using PaymentApi.Services.Services;

namespace PaymentApi.Api.Controllers
{
	[ApiController]
	[Route("api/deposit")]
	public class DepositController : ControllerBase
	{
		private readonly ILogger<DepositController> _logger;
		private readonly IAccountRepositoryAsync _accountRepo;
		private readonly ITransactionRepositoryAsync _transRepo;
		private readonly IMapper _mapper;

		public DepositController(ILogger<DepositController> logger, IAccountRepositoryAsync accountRepo, ITransactionRepositoryAsync tranRepo, IMapper mapper)
		{
			_logger = logger;
			_accountRepo = accountRepo;
			_transRepo = tranRepo;
			_mapper = mapper;
		}

		/// <summary>
		/// Creates a Deposit Transaction for the given Account
		/// </summary>
		/// <param name="objDto">Json object with transaction details, such as AccountID, Amount and Date </param>
		/// <returns>Json object with details of the deposit transaction created.</returns>
		[HttpPost("create")]
		[ProducesResponseType(StatusCodes.Status201Created, Type = typeof(TransactionResultDto))]
		[ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(TransactionResultDto))]
		[ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ErrorResponseDto))]
		[ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ErrorResponseDto))]
		[ProducesDefaultResponseType]
		public async Task<IActionResult> CreateNewDeposit([FromBody] TransactionInsertDto objDto)
		{
			TransactionCreatorService creator = new TransactionCreatorService(_logger, _mapper, (int)objDto.AccountId, (decimal)objDto.Amount, (DateTime)objDto.Date, _accountRepo, _transRepo, TransactionStatusEnum.Processed, TransactionTypeEnum.Deposit, Messages.Deposit_FailedToCreate);
			return this.GetActionResultFromServiceResult(await creator.CreateTransaction());
		}
	}
}