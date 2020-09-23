using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace PaymentApi.DataAccess.Repository.Interfaces
{
	public interface IRepositoryAsync<T> where T : class
	{
		Task<T> GetAsync(int id);

		Task<IEnumerable<T>> GetAllAsync(
			Expression<Func<T, bool>> filter = null,
			Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
			string includeProperties = null
			);

		Task<T> GetFirstOrDefaultAsync(
			Expression<Func<T, bool>> filter = null,
			string includeProperties = null
			);

		Task<bool> AddAsync(T entity);

		Task<bool> RemoveAsync(int id);

		Task<bool> RemoveAsync(T entity);

		Task<bool> UpdateAsync(T entity);

		Task<bool> RemoveRangeAsync(IEnumerable<T> entity);

		bool Save();
	}
}