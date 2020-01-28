using JacksonVeroneze.Shopping.Domain.Interface.Services;
using JacksonVeroneze.Shopping.Domain.Results;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace JacksonVeroneze.Shopping.ViewModels
{
    public class MainPageViewModel : ViewModelBase
    {
        private readonly ICategoryService _categoryService;
        private readonly IProductService _productService;
        private readonly IPromotionService _promotionService;

        //
        // Summary:
        //     Method responsible for initializing the viewModel.
        //
        // Parameters:
        //   navigationService:
        //     The navigationService param.
        //
        //   categoryService:
        //     The categoryService param.
        //
        //   productService:
        //     The productService param.
        //
        //   promotionService:
        //     The promotionService param.
        //
        public MainPageViewModel(INavigationService navigationService, ICategoryService categoryService, IProductService productService, IPromotionService promotionService) : base(navigationService)
        {
            _categoryService = categoryService;
            _productService = productService;
            _promotionService = promotionService;
        }

        //
        // Summary:
        //     Method responsible for executing when the screen receives navigation.
        // 
        // Parameters:
        //   parameters:
        //     The parameters param.
        //
        public override async void Initialize(INavigationParameters parameters)
        {
            //ViewModelState.IsLoading = true;

            await LoadDataAsync();

            //ViewModelState.IsLoading = false;

            //_crashlyticsService.TrackEventAsync(ApplicationEvents.OPEN_SCREAM,
            //    new Dictionary<string, string>() { { "Page", nameof(AuthenticatePage) } });
        }

        //
        // Summary:
        //     Method responsible for executing when the screen receives navigation.
        // 
        public async Task LoadDataAsync()
        {
            try
            {
                IList<CategoryResult> result1 = await _categoryService.FindAllAsync();
                IList<ProductResult> result2 = await _productService.FindAllAsync();
                IList<PromotionResult> result3 = await _promotionService.FindAllAsync();
            }
            catch (Exception e)
            {
                //_crashlyticsService.TrackErrorAsync(e);

                //ViewModelState.HasError = true;
            }
        }
    }
}
