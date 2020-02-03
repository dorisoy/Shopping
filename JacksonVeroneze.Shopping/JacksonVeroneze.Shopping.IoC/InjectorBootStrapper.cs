using JacksonVeroneze.Shopping.Domain.Interface.Services;
using JacksonVeroneze.Shopping.Infra.CrossCutting.Network;
using JacksonVeroneze.Shopping.Services;
using JacksonVeroneze.Shopping.Services.Interfaces;
using Prism.Ioc;

namespace JacksonVeroneze.Shopping.IoC
{
    public class InjectorBootStrapper
    {
        public static void RegisterTypes(IContainerRegistry containerRegistry)
        {
            // Services
            containerRegistry.Register<ICategoryService, CategoryService>();
            containerRegistry.Register<IProductService, ProductService>();
            containerRegistry.Register<IPromotionService, PromotionService>();
            containerRegistry.Register<ICrashlyticsService, CrashlyticsService>();

            // Repositories

            // Network
            containerRegistry.Register<MakeRequest, MakeRequest>();
        }
    }
}