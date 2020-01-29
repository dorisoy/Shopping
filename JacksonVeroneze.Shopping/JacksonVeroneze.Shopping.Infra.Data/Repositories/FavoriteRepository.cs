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
        public FavoriteRepository(ISQLiteConnectionProvider connectionProvider) : base(connectionProvider)
            => _connection.CreateTableAsync<Favorite>();
    }
}