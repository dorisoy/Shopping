using LiteDB;
using System;

namespace JacksonVeroneze.Shopping.Common
{
    public abstract class BaseRepository<T> : IBaseRepository<T> where T : BaseEntity, new()
    {
        protected LiteDatabase _connection;

        protected ILiteCollection<T> _context;

        public BaseRepository(IDbConnectionProvider connectionProvider)
            => _connection = connectionProvider.GetConnection();

        public bool Add(T entity)
            => _context.Upsert(entity);

        public bool Remove(Guid id)
             => _context.Delete(id);
    }
}