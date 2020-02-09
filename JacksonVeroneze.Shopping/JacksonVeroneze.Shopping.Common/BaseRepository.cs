using System;
using LiteDB;

namespace JacksonVeroneze.Shopping.Common
{
    public abstract class BaseRepository<T> : IBaseRepository<T> where T : BaseEntity, new()
    {
        protected LiteDatabase _connection;

        protected ILiteCollection<T> _context;

        public BaseRepository(IDbConnectionProvider connectionProvider)
            => _context = connectionProvider.GetConnection().GetCollection<T>();

        public bool Add(T entity)
            => _context.Insert(entity);

        public bool Remove(Guid id)
             => _context.Delete(id);
    }
}