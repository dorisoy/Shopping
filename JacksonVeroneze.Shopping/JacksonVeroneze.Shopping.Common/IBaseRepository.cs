using System.Collections.Generic;
using System.Threading.Tasks;

namespace JacksonVeroneze.Shopping.Common
{
    public interface IBaseRepository<T> where T : IBaseEntity
    {
        Task AddAsync(T entity);

        Task<T> FindAsync(int id);

        Task<List<T>> FindAllAsync();

        Task RemoveAsync(T entity);

        Task UpdateAsync(T entity);

        Task UpdateExceptUpdateAtAsync(T entity);
    }
}