using JacksonVeroneze.Shopping.Common;
using JacksonVeroneze.Shopping.Domain.Entities;
using JacksonVeroneze.Shopping.Domain.Interface.Repositories;

namespace Inovadora.GVIS.Infra.Data.Repositories
{
    //
    // Summary:
    //     /// Class responsible for the repository. ///
    //
    public class FavoriteRepository : BaseRepository<Favorite>, IFavoriteRepository
    {
        //
        // Summary:
        //     /// Method responsible for initializing the repository. ///
        //
        // Parameters:
        //   connectionProvider:
        //     The connectionProvider param.
        //
        public FavoriteRepository(IDbConnectionProvider connectionProvider) : base(connectionProvider)
             => _context = _connection.GetCollection<Favorite>(nameof(Favorite));

        //
        // Summary:
        //     /// Method responsible for searching the records according to the parameters. ///
        //
        // Parameters:
        //   productId:
        //     The productId param.
        //
        public Favorite FindByProductId(int productId)
        {
            return _context
                    .Query()
                    .Where(x => x.ProductId == productId && x.DeletedAt == null)
                    .FirstOrDefault();
        }
    }
}