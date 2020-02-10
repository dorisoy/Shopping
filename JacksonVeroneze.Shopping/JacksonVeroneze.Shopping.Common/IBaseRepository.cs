using System;
using LiteDB;

namespace JacksonVeroneze.Shopping.Common
{
    public interface IBaseRepository<T> where T : IBaseEntity
    {
        void Add(T entity);

        void Remove(ObjectId id);
    }
}