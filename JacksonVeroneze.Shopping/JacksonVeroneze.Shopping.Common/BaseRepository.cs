using SQLite;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace JacksonVeroneze.Shopping.Common
{
    public abstract class BaseRepository<T> : IBaseRepository<T> where T : BaseEntity, new()
    {
        protected readonly SQLiteAsyncConnection _connection;

        public BaseRepository(ISQLiteConnectionProvider connectionProvider)
            => _connection = connectionProvider.GetConnection();

        public async Task AddAsync(T entity)
            => await _connection.InsertAsync(entity);

        public Task<T> FindAsync(int id)
            => _connection.FindAsync<T>(id);

        public Task<List<T>> FindAllAsync()
            => _connection.Table<T>().ToListAsync();

        public async Task RemoveAsync(T entity)
            => await _connection.DeleteAsync(entity);

        public async Task UpdateAsync(T entity)
        {
            entity.UpdatedAt = DateTime.Now;
            entity.IncrementVersion();

            await _connection.UpdateAsync(entity);
        }

        public async Task UpdateExceptUpdateAtAsync(T entity)
        {
            entity.IncrementVersion();

            await _connection.UpdateAsync(entity);
        }
    }
}