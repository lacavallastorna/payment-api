using PaymentApi.DataAccess.Data;
using PaymentApi.DataAccess.Repository.Interfaces;
using PaymentApi.Models.Models;

namespace PaymentApi.DataAccess.Repository.Instances
{
	public class TransactionRepositoryAsync : RepositoryAsync<Transaction>, ITransactionRepositoryAsync
	{
		private readonly ApplicationDbContext _db;

		public TransactionRepositoryAsync(ApplicationDbContext db) : base(db)
		{
			_db = db;
		}
	}
}