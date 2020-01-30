using JacksonVeroneze.Shopping.Common;
using JacksonVeroneze.Shopping.Domain.Entities;
using JacksonVeroneze.Shopping.Domain.Interface.Repositories;
using System.Threading.Tasks;

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
        public FavoriteRepository(ISQLiteConnectionProvider connectionProvider) : base(connectionProvider)
            => _connection.CreateTableAsync<Favorite>();

        //
        // Summary:
        //     /// Method responsible for searching the records according to the parameters. ///
        //
        // Parameters:
        //   productId:
        //     The productId param.
        //
        public Task<Favorite> FindByProductId(int productId)
        {
            return _connection
                    .Table<Favorite>()
                    .Where(x => x.ProductId == productId && x.DeletedAt == null)
                    .FirstOrDefaultAsync();
        }
    }
}