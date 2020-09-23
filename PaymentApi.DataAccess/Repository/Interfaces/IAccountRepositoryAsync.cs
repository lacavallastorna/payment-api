using PaymentApi.Models.Models;
using System.Threading.Tasks;

namespace PaymentApi.DataAccess.Repository.Interfaces
{
	public interface IAccountRepositoryAsync : IRepositoryAsync<Account>
	{
		Task<decimal> GetAccountOpeningBalanceAsync(int accountId);

		Task<decimal> GetAccountBalanceAsync(int accountId);

		Task<decimal> GetAccountPendingBalanceAsync(int accountId);

		Task<decimal> GetAccountProcessedBalanceAsync(int accountId);
	}
}