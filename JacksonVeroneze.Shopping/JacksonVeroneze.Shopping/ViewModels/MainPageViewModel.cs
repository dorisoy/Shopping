using JacksonVeroneze.Shopping.Domain.Interface.Services;
using JacksonVeroneze.Shopping.Domain.Results;
using JacksonVeroneze.Shopping.MvvmHelpers;
using Prism.Commands;
using Prism.Navigation;
using Prism.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JacksonVeroneze.Shopping.ViewModels
{
    //
    // Summary:
    //     Class responsible for viewModel.
    //
    public class MainPageViewModel : ViewModelBase
    {
        private readonly IPageDialogService _pageDialogService;
        private readonly ICrashlyticsService _crashlyticsService;
        //
        private readonly ICategoryService _categoryService;
        private readonly IProductService _productService;
        private readonly IPromotionService _promotionService;

        private DelegateCommand _filterByCategoryCommand;

        public DelegateCommand FilterByCategoryCommand =>
            _filterByCategoryCommand ?? (
                _filterByCategoryCommand = new DelegateCommand(FilterByCategoryAsync));

        private IList<CategoryResult> _categories = new List<CategoryResult>();
        private IList<ProductResult> _products = new List<ProductResult>();
        private IList<PromotionResult> _promotions = new List<PromotionResult>();

        private ObservableRangeCollection<ProductResult> _listData = new ObservableRangeCollection<ProductResult>();
        public ObservableRangeCollection<ProductResult> ListData
        {
            get => _listData;
            set => SetProperty(ref _listData, value);
        }

        //
        // Summary:
        //     Method responsible for initializing the viewModel.
        //
        // Parameters:
        //   navigationService:
        //     The navigationService param.
        //
        //   pageDialogService:
        //     The pageDialogService param.
        //
        //   crashlyticsService:
        //     The crashlyticsService param.
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
        public MainPageViewModel(INavigationService navigationService,
            IPageDialogService pageDialogService,
            ICrashlyticsService crashlyticsService,
            ICategoryService categoryService,
            IProductService productService,
            IPromotionService promotionService) : base(navigationService)
        {
            _pageDialogService = pageDialogService;
            _crashlyticsService = crashlyticsService;
            _categoryService = categoryService;
            _productService = productService;
            _promotionService = promotionService;
        }

        //
        // Summary:
        //     Method responsible for performing the command action.
        // 
        private async void FilterByCategoryAsync()
        {
            IList<IActionSheetButton> buttons = new List<IActionSheetButton>();

            Action<CategoryResult> showProductsBycategoryAction = (i)
                => ListData.ReplaceRange(_products.Where(x => x.CategoryId == i.Id));

            Action showAllProducts = () => ListData.ReplaceRange(_products);

            buttons.Add(ActionSheetButton.CreateButton("Todas as categorias", showAllProducts));
            
            foreach (CategoryResult category in _categories)
                buttons.Add(ActionSheetButton.CreateButton(category.Name, showProductsBycategoryAction, category));

            await _pageDialogService.DisplayActionSheetAsync("Escolha a categoria para filtrar.", buttons.ToArray());
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
            ViewModelState.IsLoading = true;

            await LoadDataAsync();

            ViewModelState.IsLoading = false;

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
                _categories = await _categoryService.FindAllAsync();
                _products = await _productService.FindAllAsync();
                _promotions = await _promotionService.FindAllAsync();

                ListData.AddRange(_products);
            }
            catch (Exception e)
            {
                //_crashlyticsService.TrackErrorAsync(e);

                //ViewModelState.HasError = true;
            }
        }
    }
}
