using PaymentApi.Services.Services;
using System.Threading.Tasks;

namespace PaymentApi.Services.Interfaces
{
	public interface ITransactionCreatorService
	{
		Task<ServiceResult> CreateTransaction();
	}
}