using JacksonVeroneze.Shopping.Domain.Interface.Repositories;
using JacksonVeroneze.Shopping.Domain.Interface.Services;
using JacksonVeroneze.Shopping.Domain.Results;
using JacksonVeroneze.Shopping.MvvmHelpers;
using JacksonVeroneze.Shopping.Services.Interfaces;
using JacksonVeroneze.Shopping.Views;
using Prism.Commands;
using Prism.Navigation;
using Prism.Services;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace JacksonVeroneze.Shopping.ViewModels
{
    //
    // Summary:
    //     Class responsible for viewModel.
    //
    public class MainPageViewModel : ViewModelBase
    {
        private readonly IPageDialogService _pageDialogService;
        //
        private readonly ICrashlyticsService _crashlyticsService;
        //
        private readonly ICategoryService _categoryService;
        private readonly IFavoriteService _favoriteService;
        private readonly IProductService _productService;
        private readonly IPromotionService _promotionService;
        //
        private readonly IFavoriteRepository _favoriteRepository;

        private DelegateCommand<ProductModelData> _addRemoveFavoriteCommand;
        private DelegateCommand<ProductModelData> _decrementQuantityCommand;
        private DelegateCommand<ProductModelData> _incrementQuantityCommand;

        private DelegateCommand _filterByCategoryCommand;
        private DelegateCommand _refreshCommand;
        private DelegateCommand _buyCommand;

        public DelegateCommand<ProductModelData> AddRemoveFavoriteCommand =>
            _addRemoveFavoriteCommand ?? (
                _addRemoveFavoriteCommand = new DelegateCommand<ProductModelData>(AddRemoveFavoriteAsync));

        public DelegateCommand<ProductModelData> DecrementQuantityCommand =>
            _decrementQuantityCommand ?? (
                _decrementQuantityCommand = new DelegateCommand<ProductModelData>(DecrementQuantity));

        public DelegateCommand<ProductModelData> IncrementQuantityCommand =>
            _incrementQuantityCommand ?? (
                _incrementQuantityCommand = new DelegateCommand<ProductModelData>(IncrementQuantity));

        public DelegateCommand FilterByCategoryCommand =>
            _filterByCategoryCommand ?? (
                _filterByCategoryCommand = new DelegateCommand(FilterByCategoryAsync));

        public DelegateCommand RefreshCommand =>
            _refreshCommand ?? (
                _refreshCommand = new DelegateCommand(RefreshDataAsync,
                () => ViewModelState.IsRefresh is false)
            )
            .ObservesProperty(() => ViewModelState.IsRefresh);

        public DelegateCommand BuyCommand =>
            _buyCommand ?? (
                _buyCommand = new DelegateCommand(BuyAsync,
                () => ViewModelState.IsBusyNavigating is false)
            )
            .ObservesProperty(() => ViewModelState.IsBusyNavigating);

        private ObservableRangeCollection<ProductModelData> _listData = new ObservableRangeCollection<ProductModelData>();
        public ObservableRangeCollection<ProductModelData> ListData
        {
            get => _listData;
            set => SetProperty(ref _listData, value);
        }

        private string _textButtonBuy = "Comprar";
        public string TextButtonBuy
        {
            get => _textButtonBuy;
            set => SetProperty(ref _textButtonBuy, value);
        }

        private IList<CategoryResult> _categories = new List<CategoryResult>();
        private IList<ProductResult> _products = new List<ProductResult>();
        private IList<PromotionResult> _promotions = new List<PromotionResult>();

        private IList<ProductModelData> _cart = new List<ProductModelData>();

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

            ListData.CollectionChanged += (s, e) => UpdateViewModeStateData(s as IEnumerable<object>);
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
        //     Method responsible for performing the command action.
        // 
        // Parameters:
        //   productModelData:
        //     The productModelData param.
        //
        public void DecrementQuantity(ProductModelData productModelData)
        {
            if (productModelData.Quantity - 1 < 0)
                return;

            if (productModelData.Quantity - 1 >= 0 && _cart.Any(x => x.Id == productModelData.Id))
            {
                _cart.Remove(productModelData);

                _crashlyticsService.TrackEvent("Removed product from cart", new Dictionary<string, string> { { "Product Name", productModelData.Name } });
            }

            productModelData.Quantity--;

            UpdateDataProduct(productModelData);

            UpdateTextButtonBuy();
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

            if (_cart.Any(x => x.Id == productModelData.Id) is false)
            {
                _cart.Add(productModelData);

                _crashlyticsService.TrackEvent("Added product in cart", new Dictionary<string, string> { { "Product Name", productModelData.Name } });
            }

            UpdateDataProduct(productModelData);

            UpdateTextButtonBuy();
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
        public async void BuyAsync()
        {
            if (_cart.Any() is false)
            {
                await _pageDialogService.DisplayAlertAsync("Aviso", "Antes de proseguir adicione um produto.", "Ok");

                return;
            }

            _crashlyticsService.TrackEvent("Clicked buy", new Dictionary<string, string> { { "Total Products", _cart.Count().ToString() } });

            ViewModelState.IsBusyNavigating = true;

            await _navigationService.NavigateAsync(nameof(CartPage), new NavigationParameters {
                { "cart", _cart },
            });

            ViewModelState.IsBusyNavigating = false;
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

                ListData.ReplaceRange(await FactoryProductModelDataAsync(_products));
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

            foreach (ProductResult product in productResults)
            {
                //Favorite favorite = await _favoriteRepository.FindByProductId(x.Id);

                ProductModelData productModelData = _cart.FirstOrDefault(x => x.Id == product.Id);

                if (productModelData != null)
                {
                    productModelDatas.Add(productModelData);
                }
                else
                {
                    productModelDatas.Add(new ProductModelData()
                    {
                        Id = product.Id,
                        Name = product.Name,
                        Description = product.Description,
                        Photo = product.Photo,
                        OriginalPrice = product.Price,
                        PriceWithDiscount = product.Price,
                        CategoryId = product.CategoryId,
                        IsFavorite = false
                    });
                }
            }

            return productModelDatas;
        }

        //
        // Summary:
        //     Method responsible for update data in ProductModelData.
        // 
        // Parameters:
        //   productModelData:
        //     The productModelData param.
        //
        private void UpdateDataProduct(ProductModelData productModelData)
        {
            if (productModelData.CategoryId is null)
            {
                productModelData.PriceWithDiscount = productModelData.OriginalPrice * productModelData.Quantity;
                return;
            }

            if (productModelData.Quantity == 0)
            {
                productModelData.PriceWithDiscount = productModelData.OriginalPrice;
                productModelData.PercentageDiscount = 0;

                return;
            }

            PromotionPoliceResult promotionPoliceResult = FindPromotionPoliceResultByProductCategory(productModelData);

            if (promotionPoliceResult is null)
            {
                productModelData.PriceWithDiscount = productModelData.OriginalPrice * productModelData.Quantity;
                productModelData.PercentageDiscount = 0;

                return;
            }

            productModelData.PriceWithDiscount = (productModelData.OriginalPrice - (productModelData.OriginalPrice * (promotionPoliceResult.Discount / 100))) * productModelData.Quantity;
            productModelData.PercentageDiscount = promotionPoliceResult.Discount;
        }

        //
        // Summary:
        //     Method responsible for find PromotionPolice.
        // 
        // Parameters:
        //   productModelData:
        //     The productModelData param.
        //
        private PromotionPoliceResult FindPromotionPoliceResultByProductCategory(ProductModelData productModelData)
        {
            PromotionPoliceResult promotionPoliceResult = _promotions
                    .Where(x => x.CategoryId == productModelData.CategoryId)
                    ?.Select(x => x.Policies.OrderByDescending(c => c.Min)
                            .FirstOrDefault(y => y.Min <= productModelData.Quantity))
                            .FirstOrDefault();

            return promotionPoliceResult;
        }

        //
        // Summary:
        //     Method responsible for update text button buy.
        // 
        private void UpdateTextButtonBuy()
        {
            double total = _cart.Where(x => x.Quantity > 0).Sum(x => x.PriceWithDiscount);

            string totalPtBR = total.ToString("C", CultureInfo.CreateSpecificCulture("pt-BR"));

            TextButtonBuy = total > 0 ? $"Comprar ► {totalPtBR}" : "Comprar";
        }
    }
}