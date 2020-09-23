using Microsoft.EntityFrameworkCore;
using PaymentApi.DataAccess.Data;
using PaymentApi.DataAccess.Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace PaymentApi.DataAccess.Repository.Instances
{
	public class RepositoryAsync<T> : IRepositoryAsync<T> where T : class
	{
		private readonly ApplicationDbContext _db;
		internal DbSet<T> dbSet;

		public RepositoryAsync(ApplicationDbContext db)
		{
			_db = db;
			this.dbSet = _db.Set<T>();
		}

		public async Task<bool> AddAsync(T entity)
		{
			await dbSet.AddAsync(entity);
			return Save();
		}

		public async Task<T> GetAsync(int id)
		{
			return await dbSet.FindAsync(id);
		}

		public async Task<IEnumerable<T>> GetAllAsync(Expression<Func<T, bool>> filter = null, Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null, string includeProperties = null)
		{
			IQueryable<T> query = dbSet;

			if (filter != null)
			{
				query = query.Where(filter);
			}

			if (includeProperties != null)
			{
				foreach (var includeProp in includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
				{
					query = query.Include(includeProp);
				}
			}

			if (orderBy != null)
			{
				return await orderBy(query).ToListAsync();
			}
			return await query.ToListAsync();
		}

		public async Task<T> GetFirstOrDefaultAsync(Expression<Func<T, bool>> filter = null, string includeProperties = null)
		{
			IQueryable<T> query = dbSet;

			if (filter != null)
			{
				query = query.Where(filter);
			}

			if (includeProperties != null)
			{
				foreach (var includeProp in includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
				{
					query = query.Include(includeProp);
				}
			}

			return await query.FirstOrDefaultAsync();
		}

		public async Task<bool> RemoveAsync(int id)
		{
			T entity = await dbSet.FindAsync(id);
			return await RemoveAsync(entity);
		}

		public async Task<bool> RemoveAsync(T entity)
		{
			await Task.Run(() => { dbSet.Remove(entity); });
			return Save();
		}

		public async Task<bool> UpdateAsync(T entity)
		{
			await Task.Run(() => { dbSet.Update(entity); });
			return Save();
		}

		public async Task<bool> RemoveRangeAsync(IEnumerable<T> entity)
		{
			await Task.Run(() => { dbSet.RemoveRange(entity); });
			return Save();
		}

		public bool Save()
		{
			return _db.SaveChanges() >= 0 ? true : false;
		}
	}
}