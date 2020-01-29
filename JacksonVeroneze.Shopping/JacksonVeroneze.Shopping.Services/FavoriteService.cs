using JacksonVeroneze.Shopping.Common;
using JacksonVeroneze.Shopping.Domain.Entities;
using JacksonVeroneze.Shopping.Domain.Interface.Repositories;
using JacksonVeroneze.Shopping.Domain.Interface.Services;
using System.Threading.Tasks;

namespace JacksonVeroneze.Shopping.Services
{
    //
    // Summary:
    //     /// Class responsible for the service. ///
    //
    public class FavoriteService : BaseService, IFavoriteService
    {
        private readonly IFavoriteRepository _favoriteRepository;

        //
        // Summary:
        //     /// Method responsible for initializing the service. ///
        //
        // Parameters:
        //   favoriteRepository:
        //     The favoriteRepository param.
        //
        public FavoriteService(IFavoriteRepository favoriteRepository)
            => _favoriteRepository = favoriteRepository;

        //
        // Summary:
        //     /// Method responsible for creating a record. ///
        //
        // Parameters:
        //   entity:
        //     The entity param.
        //
        public Task AddAsync(Favorite entity)
            => _favoriteRepository.AddAsync(entity);

        //
        // Summary:
        //     /// Method responsible for remove a record. ///
        //
        // Parameters:
        //   entity:
        //     The entity param.
        //
        public Task RemoveAsync(Favorite entity)
            => _favoriteRepository.AddAsync(entity);
    }
}