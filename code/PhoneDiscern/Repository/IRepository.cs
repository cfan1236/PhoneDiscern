using PhoneDiscern.Domain;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace PhoneDiscern.Repository
{
	public interface IRepository<T> where T : IEntity
	{
		Task<string> Insert(T t);
		void BatchInsert(List<T> t);
		Task<bool> Delete(string id);
		Task<List<T>> GetAllList(int skip = 0, int count = 0);
		Task<List<T>> GetList(Expression<Func<T, bool>> conditions = null);
		Task<T> GetById(string id);
		Task<bool> Update(T t);
	}
}
