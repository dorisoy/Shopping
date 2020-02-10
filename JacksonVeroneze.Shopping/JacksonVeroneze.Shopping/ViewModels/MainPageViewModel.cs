using JacksonVeroneze.Shopping.Common;
using JacksonVeroneze.Shopping.Domain.Entities;
using JacksonVeroneze.Shopping.Domain.Interface.Repositories;
using JacksonVeroneze.Shopping.Domain.Interface.Services;
using JacksonVeroneze.Shopping.Domain.Results;
using JacksonVeroneze.Shopping.MvvmHelpers;
using JacksonVeroneze.Shopping.Services.Interfaces;
using JacksonVeroneze.Shopping.Util;
using JacksonVeroneze.Shopping.ViewModelsData;
using JacksonVeroneze.Shopping.Views;
using LiteDB;
using Prism.Commands;
using Prism.Navigation;
using Prism.Services;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Xamarin.Essentials;

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
        private readonly IProductService _productService;
        private readonly IPromotionService _promotionService;
        //
        
        private readonly IDbConnectionProvider _dbConnectionProvider;
        private readonly IFavoriteRepository _favoriteRepository;
        //

        private DelegateCommand<ProductModelData> _addRemoveFavoriteCommand;
        private DelegateCommand<ProductModelData> _decrementQuantityCommand;
        private DelegateCommand<ProductModelData> _incrementQuantityCommand;
        private DelegateCommand<string> _searchCommand;

        private DelegateCommand _filterByCategoryCommand;
        private DelegateCommand _refreshCommand;
        private DelegateCommand _buyCommand;

        public DelegateCommand<ProductModelData> AddRemoveFavoriteCommand =>
            _addRemoveFavoriteCommand ?? (
                _addRemoveFavoriteCommand = new DelegateCommand<ProductModelData>(AddRemoveFavorite));

        public DelegateCommand<ProductModelData> DecrementQuantityCommand =>
            _decrementQuantityCommand ?? (
                _decrementQuantityCommand = new DelegateCommand<ProductModelData>(DecrementQuantity));

        public DelegateCommand<ProductModelData> IncrementQuantityCommand =>
            _incrementQuantityCommand ?? (
                _incrementQuantityCommand = new DelegateCommand<ProductModelData>(IncrementQuantity));

        public DelegateCommand<string> SearchCommand =>
            _searchCommand ?? (_searchCommand = new DelegateCommand<string>(
                async (x) => await SearchAsync(x).ConfigureAwait(false)
            ));

        public DelegateCommand FilterByCategoryCommand =>
            _filterByCategoryCommand ?? (
                _filterByCategoryCommand = new DelegateCommand(FilterByCategoryAsync));

        public DelegateCommand RefreshCommand =>
            _refreshCommand ?? (
                _refreshCommand = new DelegateCommand(RefreshData,
                () => ViewModelState.IsRefresh is false)
            )
            .ObservesProperty(() => ViewModelState.IsRefresh);

        public DelegateCommand BuyCommand =>
            _buyCommand ?? (
                _buyCommand = new DelegateCommand(BuyAsync,
                () => ViewModelState.IsBusyNavigating is false && HasItensToBuy)
            )
            .ObservesProperty(() => ViewModelState.IsBusyNavigating)
            .ObservesProperty(() => HasItensToBuy);

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

        private bool _hasItensToBuy = false;
        public bool HasItensToBuy
        {
            get => _hasItensToBuy;
            set => SetProperty(ref _hasItensToBuy, value);
        }

        private int? _currentFilterCategory = null;

        private readonly Action<MainPageViewModel> VerifyItensToBuy = (vm) => vm.HasItensToBuy = vm._cart.Any();

        private IList<CategoryResult> _categories = new List<CategoryResult>();
        private IList<PromotionResult> _promotions = new List<PromotionResult>();

        private IList<ProductModelData> _cart = new List<ProductModelData>();

        private CancellationTokenSource _cancellationTokenSource = new CancellationTokenSource();

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
        //   favoriteRepository:
        //     The favoriteRepository param.
        //
        public MainPageViewModel(INavigationService navigationService,
            IPageDialogService pageDialogService,
            ICrashlyticsService crashlyticsService,
            ICategoryService categoryService,
            IProductService productService,
            IPromotionService promotionService,
            IFavoriteRepository favoriteRepository) : base(navigationService)
        {
            _pageDialogService = pageDialogService;
            _crashlyticsService = crashlyticsService;
            _categoryService = categoryService;
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
        public async void AddRemoveFavorite(ProductModelData productModelData)
        {
            try
            {
                Favorite favorite = _favoriteRepository.FindByProductId(productModelData.Id);

                if (favorite != null && productModelData.IsFavorite is true)
                {
                    _favoriteRepository.Remove(favorite.Id);
                    productModelData.IsFavorite = false;
                    _crashlyticsService.TrackEvent(ApplicationEvents.REMOVED_PRODUCT_FROM_FAVORITES, new Dictionary<string, string> { { "Product Name", productModelData.Name } });

                    return;
                }

                _favoriteRepository.Add(new Favorite(productModelData.Id));

                productModelData.IsFavorite = true;

                _crashlyticsService.TrackEvent(ApplicationEvents.ADDED_PRODUCT_IN_FAVORITES, new Dictionary<string, string> { { "Product Name", productModelData.Name } });
            }
            catch (Exception e)
            {
                await _pageDialogService.DisplayAlertAsync("Erro", "Houve um erro em adicionar/remover o produto dos favoritos, tente novamente.", "Ok");

                _crashlyticsService.TrackError(e);
            }
        }

        //
        // Summary:
        //     Method responsible for performing the command action.
        // 
        // Parameters:
        //   productModelData:
        //     The productModelData param.
        //
        public async void DecrementQuantity(ProductModelData productModelData)
        {
            try
            {
                if (productModelData.Quantity - 1 < 0)
                    return;

                if (productModelData.Quantity - 1 == 0 && _cart.Any(x => x.Id == productModelData.Id))
                {
                    _cart.Remove(productModelData);

                    _crashlyticsService.TrackEvent(ApplicationEvents.REMOVED_PRODUCT_FROM_CART, new Dictionary<string, string> { { "Product Name", productModelData.Name } });
                }

                productModelData.Quantity--;

                UpdateDataProduct(productModelData);

                UpdateTextButtonBuy();

                VerifyItensToBuy(this);
            }
            catch (Exception e)
            {
                await _pageDialogService.DisplayAlertAsync("Erro", "Houve um erro em decrementar a quantidade, tente novamente.", "Ok");

                _crashlyticsService.TrackError(e);
            }
        }

        //
        // Summary:
        //     Method responsible for performing the command action.
        // 
        // Parameters:
        //   productModelData:
        //     The productModelData param.
        //
        public async void IncrementQuantity(ProductModelData productModelData)
        {
            try
            {
                productModelData.Quantity++;

                if (_cart.Any(x => x.Id == productModelData.Id) is false)
                {
                    _cart.Add(productModelData);

                    _crashlyticsService.TrackEvent(ApplicationEvents.ADDED_PRODUCT_IN_CART, new Dictionary<string, string> { { "Product Name", productModelData.Name } });
                }

                UpdateDataProduct(productModelData);

                UpdateTextButtonBuy();

                VerifyItensToBuy(this);
            }
            catch (Exception e)
            {
                await _pageDialogService.DisplayAlertAsync("Erro", "Houve um erro em incrementar a quantidade, tente novamente.", "Ok");

                _crashlyticsService.TrackError(e);
            }
        }

        //
        // Summary:
        //     Method responsible for performing the command action.
        // 
        // Parameters:
        //   value:
        //     The value param.
        //
        public async Task SearchAsync(string value)
        {
            if (ViewModelState.HasError is true || ViewModelState.IsBusy is true || ViewModelState.IsLoading is true || ViewModelState.IsSearch is true)
                return;

            ViewModelState.IsSearch = true;

            try
            {
                Interlocked.Exchange(ref _cancellationTokenSource, new CancellationTokenSource()).Cancel();

                await Task.Delay(TimeSpan.FromMilliseconds(500), _cancellationTokenSource.Token)
                      .ContinueWith(async task =>
                      {
                          _currentFilterCategory = null;

                          ListData.ReplaceRange(FactoryProductModelDataAsync(await _productService.SearchAsync(value)));
                      },
                    CancellationToken.None,
                    TaskContinuationOptions.OnlyOnRanToCompletion,
                    TaskScheduler.FromCurrentSynchronizationContext());
            }
            catch (TaskCanceledException) { }
            catch (Exception e)
            {
                await _pageDialogService.DisplayAlertAsync("Erro", "Houve um efetuar a busca, tente novamente.", "Ok");

                _crashlyticsService.TrackError(e);
            }
            finally
            {
                ViewModelState.IsSearch = false;
            }
        }

        //
        // Summary:
        //     Method responsible for performing the command action.
        // 
        private async void FilterByCategoryAsync()
        {
            try
            {
                if (ViewModelState.HasError || ViewModelState.IsLoading is true)
                    return;

                IList<IActionSheetButton> buttons = new List<IActionSheetButton>();

                Action<CategoryResult> showProductsBycategoryAction = async (i) =>
                {
                    _currentFilterCategory = i.Id;

                    ListData.ReplaceRange(FactoryProductModelDataAsync(await _productService.FindByCategotyIdAsync(i.Id)));

                    _crashlyticsService.TrackEvent(ApplicationEvents.FILTER_BY_CATEGORY, new Dictionary<string, string> { { "Category Name", i.Name } });
                };

                Action showAllProducts = async () =>
                {
                    _currentFilterCategory = null;

                    ListData.ReplaceRange(FactoryProductModelDataAsync(await _productService.FindAllAsync()));
                };

                buttons.Add(ActionSheetButton.CreateButton("Todas as categorias", showAllProducts));

                foreach (CategoryResult category in _categories)
                    buttons.Add(ActionSheetButton.CreateButton(category.Name, showProductsBycategoryAction, category));

                await _pageDialogService.DisplayActionSheetAsync("Escolha a categoria para filtrar", buttons.ToArray());
            }
            catch (Exception e)
            {
                await _pageDialogService.DisplayAlertAsync("Erro", "Houve um filtar por categoria, tente novamente.", "Ok");

                _crashlyticsService.TrackError(e);
            }
        }

        //
        // Summary:
        //     Method responsible for performing the command action.
        // 
        public async void RefreshData()
        {
            try
            {
                ViewModelState.IsRefresh = true;

                IList<ProductResult> productResults = new List<ProductResult>();

                if (_currentFilterCategory is null)
                {
                    productResults = await _productService.FindAllAsync();
                }
                else
                {
                    productResults = await _productService.FindByCategotyIdAsync((int)_currentFilterCategory);
                }

                ListData.ReplaceRange(FactoryProductModelDataAsync(productResults));

                ViewModelState.IsRefresh = false;
            }
            catch (Exception e)
            {
                ViewModelState.HasError = true;

                _crashlyticsService.TrackError(e);
            }
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

            _crashlyticsService.TrackEvent(ApplicationEvents.BUY,
                    new Dictionary<string, string>() {
                        { "Quantity", _cart.Where(x => x.Quantity > 0).ToString() },
                        { "Total", _cart.Where(x => x.Quantity > 0).Sum(x => x.Total).ToString() }
                    });

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

            _crashlyticsService.TrackEvent(ApplicationEvents.OPEN_SCREAM,
                    new Dictionary<string, string>() { { "Page", nameof(MainPage) } });

            ViewModelState.IsLoading = false;
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
                _promotions = await _promotionService.FindAllAsync();

                IList<ProductResult> products = await _productService.FindAllAsync();

                ListData.ReplaceRange(FactoryProductModelDataAsync(products));
            }
            catch (Exception e)
            {
                ViewModelState.HasError = true;

                _crashlyticsService.TrackError(e);
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
        private IList<ProductModelData> FactoryProductModelDataAsync(IList<ProductResult> productResults)
        {
            IList<ProductModelData> productModelDatas = new List<ProductModelData>();

            try
            {
                foreach (ProductResult product in productResults)
                {
                    ProductModelData productModelDataCart = _cart.FirstOrDefault(x => x.Id == product.Id);

                    if (productModelDataCart != null)
                    {
                        productModelDatas.Add(productModelDataCart);
                    }
                    else
                    {
                        productModelDatas.Add(FactoryProductModelData(product));
                    }
                }
            }
            catch (Exception e)
            {
                ViewModelState.HasError = true;

                _crashlyticsService.TrackError(e);
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
            PromotionPoliceResult promotionPoliceResult = FindPromotionPoliceResultByProductCategory(productModelData);

            if (promotionPoliceResult != null)
            {
                productModelData.FinalPrice = (productModelData.OriginalPrice - (productModelData.OriginalPrice * (promotionPoliceResult.Discount / 100)));
                productModelData.PercentageDiscount = promotionPoliceResult.Discount;
                productModelData.Total = productModelData.FinalPrice * productModelData.Quantity;

                return;
            }

            productModelData.FinalPrice = productModelData.OriginalPrice;
            productModelData.PercentageDiscount = 0;
            productModelData.Total = productModelData.OriginalPrice * productModelData.Quantity;
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
        //     Method responsible for find Promotion.
        // 
        // Parameters:
        //   categoryId:
        //     The categoryId param.
        //
        private PromotionResult FindPromotionByCategoryId(int categoryId)
        {
            PromotionResult promotionResult = _promotions
                    .Where(x => x.CategoryId == categoryId)
                    .FirstOrDefault();

            return promotionResult;
        }

        //
        // Summary:
        //     Method responsible for update text button buy.
        // 
        private void UpdateTextButtonBuy()
        {
            double total = _cart.Where(x => x.Quantity > 0).Sum(x => x.Total);

            string totalPtBR = total.ToString("C", CultureInfo.CreateSpecificCulture("pt-BR"));

            TextButtonBuy = total > 0 ? $"Comprar ► {totalPtBR}" : "Comprar";
        }

        //
        // Summary:
        //     Method responsible for build ProductModelData.
        // 
        // Parameters:
        //   product:
        //     The product param.
        //
        private ProductModelData FactoryProductModelData(ProductResult product)
        {
            string promotionName = string.Empty;

            if (product.CategoryId != null)
            {
                PromotionResult promotionResult = FindPromotionByCategoryId((int)product.CategoryId);
                promotionName = promotionResult?.Name;
            }

            Favorite favorite = _favoriteRepository.FindByProductId(product.Id);

            return new ProductModelData()
            {
                Id = product.Id,
                Name = product.Name,
                Description = product.Description,
                Photo = product.Photo,
                OriginalPrice = product.Price,
                FinalPrice = product.Price,
                CategoryId = product.CategoryId,
                IsFavorite = favorite != null,
                Promotion = promotionName
            };
        }
    }
}