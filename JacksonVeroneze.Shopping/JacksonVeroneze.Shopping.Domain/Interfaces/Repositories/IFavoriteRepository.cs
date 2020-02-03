using JacksonVeroneze.Shopping.Common;
using JacksonVeroneze.Shopping.Domain.Entities;

namespace JacksonVeroneze.Shopping.Domain.Interface.Repositories
{
    public interface IFavoriteRepository : IBaseRepository<Favorite>
    {
        Favorite FindByProductId(int productId);
    }
}