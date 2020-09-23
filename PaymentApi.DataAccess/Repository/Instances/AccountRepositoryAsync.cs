using Microsoft.EntityFrameworkCore;
using PaymentApi.DataAccess.Data;
using PaymentApi.DataAccess.Repository.Interfaces;
using PaymentApi.Models.Models;
using System.Linq;
using System.Threading.Tasks;

namespace PaymentApi.DataAccess.Repository.Instances
{
	public class AccountRepositoryAsync : RepositoryAsync<Account>, IAccountRepositoryAsync
	{
		private readonly ApplicationDbContext _db;

		public AccountRepositoryAsync(ApplicationDbContext db) : base(db)
		{
			_db = db;
		}

		public async Task<decimal> GetAccountProcessedBalanceAsync(int accountId)
		{
			return await _db.Transactions.Where(t => t.AccountId == accountId
											&& t.TransactionType == TransactionTypeEnum.Withdrawal
											&& t.TransactionStatus == TransactionStatusEnum.Processed
											).SumAsync(t => t.Amount);
		}

		public async Task<decimal> GetAccountPendingBalanceAsync(int accountId)
		{
			return await _db.Transactions.Where(t => t.AccountId == accountId
											&& t.TransactionType == TransactionTypeEnum.Withdrawal
											&& t.TransactionStatus == TransactionStatusEnum.Pending
											).SumAsync(t => t.Amount);
		}

		public async Task<decimal> GetAccountOpeningBalanceAsync(int accountId)
		{
			return await _db.Transactions.Where(t => t.AccountId == accountId
											&& t.TransactionType == TransactionTypeEnum.Deposit
											&& t.TransactionStatus == TransactionStatusEnum.Processed
											).SumAsync(t => t.Amount);
		}

		public async Task<decimal> GetAccountBalanceAsync(int accountId)
		{
			decimal openingBalanceAmount = await GetAccountOpeningBalanceAsync(accountId);
			decimal processedAndPendingPaymentsAmount = await _db.Transactions
										.Where(
											t => t.AccountId == accountId &&
											(
												t.TransactionStatus == TransactionStatusEnum.Processed
												||
												t.TransactionStatus == TransactionStatusEnum.Pending
											) &&
											t.TransactionType == TransactionTypeEnum.Withdrawal
										).SumAsync(t => t.Amount);
			return openingBalanceAmount - processedAndPendingPaymentsAmount;
		}
	}
}