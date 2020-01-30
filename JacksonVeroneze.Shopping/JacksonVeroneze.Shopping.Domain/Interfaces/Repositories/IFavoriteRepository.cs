using JacksonVeroneze.Shopping.Common;
using JacksonVeroneze.Shopping.Domain.Entities;
using System.Threading.Tasks;

namespace JacksonVeroneze.Shopping.Domain.Interface.Repositories
{
    public interface IFavoriteRepository : IBaseRepository<Favorite>
    {
        Task<Favorite> FindByProductId(int productId);
    }
}