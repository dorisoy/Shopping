using LiteDB;
using System;

namespace JacksonVeroneze.Shopping.Common
{
    public interface IBaseRepository<T> where T : IBaseEntity
    {
        bool Add(T entity);

        bool Remove(Guid id);
    }
}