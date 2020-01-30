using JacksonVeroneze.Shopping.Domain.Entities;
using JacksonVeroneze.Shopping.Domain.Interface.Repositories;
using JacksonVeroneze.Shopping.Domain.Interface.Services;
using JacksonVeroneze.Shopping.Domain.Results;
using JacksonVeroneze.Shopping.MvvmHelpers;
using JacksonVeroneze.Shopping.Services.Interfaces;
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
        private readonly IFavoriteService _favoriteService;
        private readonly IProductService _productService;
        private readonly IPromotionService _promotionService;
        //
        private readonly IFavoriteRepository _favoriteRepository;

        private DelegateCommand _filterByCategoryCommand;
        private DelegateCommand _refreshCommand;

        private DelegateCommand<ProductModelData> _decrementQuantityCommand;
        private DelegateCommand<ProductModelData> _incrementQuantityCommand;
        private DelegateCommand<ProductModelData> _addRemoveFavoriteCommand;

        public DelegateCommand<ProductModelData> DecrementQuantityCommand =>
            _decrementQuantityCommand ?? (
                _decrementQuantityCommand = new DelegateCommand<ProductModelData>(DecrementQuantity));

        public DelegateCommand<ProductModelData> IncrementQuantityCommand =>
            _incrementQuantityCommand ?? (
                _incrementQuantityCommand = new DelegateCommand<ProductModelData>(IncrementQuantity));

        public DelegateCommand<ProductModelData> AddRemoveFavoriteCommand =>
            _addRemoveFavoriteCommand ?? (
                _addRemoveFavoriteCommand = new DelegateCommand<ProductModelData>(AddRemoveFavoriteAsync));

        public DelegateCommand FilterByCategoryCommand =>
            _filterByCategoryCommand ?? (
                _filterByCategoryCommand = new DelegateCommand(FilterByCategoryAsync));

        public DelegateCommand RefreshCommand =>
            _refreshCommand ?? (
                _refreshCommand = new DelegateCommand(RefreshDataAsync,
                () => ViewModelState.IsRefresh is false)
            )
            .ObservesProperty(() => ViewModelState.IsRefresh);

        private IList<CategoryResult> _categories = new List<CategoryResult>();
        private IList<ProductResult> _products = new List<ProductResult>();
        private IList<PromotionResult> _promotions = new List<PromotionResult>();

        private ObservableRangeCollection<ProductModelData> _listData = new ObservableRangeCollection<ProductModelData>();
        public ObservableRangeCollection<ProductModelData> ListData
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
        //   favoriteService:
        //     The favoriteService param.
        //
        //   productService:
        //     The productService param.
        //
        //   promotionService:
        //     The promotionService param.
        //
        //   favoriteRepository:
        //     The favoriteRepository param.
        //
        public MainPageViewModel(INavigationService navigationService,
            IPageDialogService pageDialogService,
            ICrashlyticsService crashlyticsService,
            ICategoryService categoryService,
            IFavoriteService favoriteService,
            IProductService productService,
            IPromotionService promotionService,
            IFavoriteRepository favoriteRepository) : base(navigationService)
        {
            _pageDialogService = pageDialogService;
            _crashlyticsService = crashlyticsService;
            _categoryService = categoryService;
            _favoriteService = favoriteService;
            _productService = productService;
            _promotionService = promotionService;
            _favoriteRepository = favoriteRepository;
        }

        //
        // Summary:
        //     Method responsible for performing the command action.
        // 
        private async void FilterByCategoryAsync()
        {
            IList<IActionSheetButton> buttons = new List<IActionSheetButton>();

            Action<CategoryResult> showProductsBycategoryAction = async (i)
                => ListData.ReplaceRange(await FactoryProductModelDataAsync(_products.Where(x => x.CategoryId == i.Id).ToList()));

            Action showAllProducts = async () => ListData.ReplaceRange(await FactoryProductModelDataAsync(_products));

            buttons.Add(ActionSheetButton.CreateButton("Todas as categorias", showAllProducts));

            foreach (CategoryResult category in _categories)
                buttons.Add(ActionSheetButton.CreateButton(category.Name, showProductsBycategoryAction, category));

            await _pageDialogService.DisplayActionSheetAsync("Escolha a categoria para filtrar.", buttons.ToArray());
        }

        //
        // Summary:
        //     Method responsible for performing the command action.
        // 
        public async void RefreshDataAsync()
        {
            ViewModelState.IsRefresh = true;

            await LoadDataAsync();

            ViewModelState.IsRefresh = false;
        }

        //
        // Summary:
        //     Method responsible for performing the command action.
        // 
        // Parameters:
        //   productModelData:
        //     The productModelData param.
        //
        public void DecrementQuantity(ProductModelData productModelData)
        {
            if (productModelData.Quantity - 1 >= 0)
                productModelData.Quantity--;
        }

        //
        // Summary:
        //     Method responsible for performing the command action.
        // 
        // Parameters:
        //   productModelData:
        //     The productModelData param.
        //
        public void IncrementQuantity(ProductModelData productModelData)
        {
            productModelData.Quantity++;
        }

        //
        // Summary:
        //     Method responsible for performing the command action.
        // 
        // Parameters:
        //   productModelData:
        //     The productModelData param.
        //
        public async void AddRemoveFavoriteAsync(ProductModelData productModelData)
        {
            //Favorite favorite = await _favoriteRepository.FindByProductId(productModelData.Id);

            //if (favorite != null && productModelData.IsFavorite is true)
            //    await _favoriteRepository.RemoveAsync(favorite);

            //favorite = new Favorite(productModelData.Id);

            //await _favoriteService.AddAsync(favorite);

            productModelData.IsFavorite = !productModelData.IsFavorite;
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

                ListData.AddRange(await FactoryProductModelDataAsync(_products));
            }
            catch (Exception e)
            {
                _crashlyticsService.TrackError(e);

                ViewModelState.HasError = true;
            }
        }

        //
        // Summary:
        //     Method responsible for create a ProductModelData list data.
        // 
        // Parameters:
        //   productResults:
        //     The productResults param.
        //
        private async Task<IList<ProductModelData>> FactoryProductModelDataAsync(IList<ProductResult> productResults)
        {
            IList<ProductModelData> productModelDatas = new List<ProductModelData>();

            foreach (ProductResult x in productResults)
            {
                //Favorite favorite = await _favoriteRepository.FindByProductId(x.Id);

                productModelDatas.Add(new ProductModelData()
                {
                    Id = x.Id,
                    Name = x.Name,
                    Description = x.Description,
                    Photo = x.Photo,
                    Price = x.Price,
                    IsFavorite = false
                });
            }

            return productModelDatas;
        }
    }
}