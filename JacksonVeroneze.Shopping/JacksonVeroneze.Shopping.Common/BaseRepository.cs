using System;

namespace JacksonVeroneze.Shopping.Common
{
    public abstract class BaseRepository<T> : IBaseRepository<T> where T : BaseEntity, new()
    {
    }
}