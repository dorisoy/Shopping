using Inovadora.GVIS.Infra.Data.Repositories;
using JacksonVeroneze.Shopping.Domain.Interface.Repositories;
using JacksonVeroneze.Shopping.Domain.Interface.Services;
using JacksonVeroneze.Shopping.Infra.CrossCutting.Network;
using JacksonVeroneze.Shopping.Services;
using Prism.Ioc;

namespace JacksonVeroneze.Shopping.IoC
{
    public class InjectorBootStrapper
    {
        public static void RegisterTypes(IContainerRegistry containerRegistry)
        {
            // Services
            containerRegistry.Register<ICategoryService, CategoryService>();
            containerRegistry.Register<IFavoriteService, FavoriteService>();
            containerRegistry.Register<IProductService, ProductService>();
            containerRegistry.Register<IPromotionService, PromotionService>();
            containerRegistry.Register<ICrashlyticsService, CrashlyticsService>();

            // Repositories
            containerRegistry.Register<IFavoriteRepository, FavoriteRepository>();

            // Network
            containerRegistry.Register<MakeRequest, MakeRequest>();
        }
    }
}