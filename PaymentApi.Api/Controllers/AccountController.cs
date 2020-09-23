using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using PaymentApi.Api.Controllers.Extensions;
using PaymentApi.DataAccess.Repository.Interfaces;
using PaymentApi.Models.Models.Dtos;
using PaymentApi.Services.Services;
using System.Threading.Tasks;

namespace PaymentApi.Api.Controllers
{
	[ApiController]
	[Route("api/account")]
	public class AccountController : ControllerBase
	{
		private readonly ILogger<AccountController> _logger;
		private readonly IAccountRepositoryAsync _accountRepo;
		private readonly ITransactionRepositoryAsync _transRepo;
		private readonly IMapper _mapper;

		public AccountController(ILogger<AccountController> logger, IAccountRepositoryAsync accountRepo, ITransactionRepositoryAsync transRepo, IMapper mapper)
		{
			_logger = logger;
			_accountRepo = accountRepo;
			_transRepo = transRepo;
			_mapper = mapper;
		}

		/// <summary>
		/// Creates a new Account
		/// </summary>
		/// <param name="objDto"></param>
		/// <returns>Json object with Account details</returns>
		[HttpPost("create")]
		[ProducesResponseType(StatusCodes.Status201Created, Type = typeof(AccountInsertResultDto))]
		[ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ErrorResponseDto))]
		[ProducesDefaultResponseType]
		public async Task<IActionResult> CreateNewAccount([FromBody] AccountInsertDto objDto)
		{
			AccountCreatorService creator = new AccountCreatorService(_logger, objDto.Name, _accountRepo);
			return this.GetActionResultFromServiceResult(await creator.CreateAccount());
		}

		/// <summary>
		///	 Gets the Account Balance and list of Payment Transactions
		/// </summary>
		/// <param name="accountId">The Id of the Account for which the request is performed.</param>
		/// <returns>Json object with details on Account Balances and list of Payment Transactions</returns>
		[HttpGet("balance/{accountId:int}")]
		[ProducesResponseType(StatusCodes.Status200OK, Type = typeof(AccountBalanceResultDto))]
		[ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ErrorResponseDto))]
		[ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ErrorResponseDto))]
		[ProducesDefaultResponseType]
		public async Task<IActionResult> GetAccountBalance(int accountId)
		{
			AccountBalanceService balance = new AccountBalanceService(_logger, accountId, _mapper, _accountRepo, _transRepo);
			return this.GetActionResultFromServiceResult(await balance.GetAccountBalance());
		}
	}
}