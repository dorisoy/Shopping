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
    public class CartPageViewModel : ViewModelBase
    {
        private readonly IPageDialogService _pageDialogService;
        //
        private readonly ICrashlyticsService _crashlyticsService;
        
        private DelegateCommand _buyCommand;

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

        private double _total = 0;
        public double Total
        {
            get => _total;
            set => SetProperty(ref _total, value);
        }

        private int _quantity = 0;
        public int Quantity
        {
            get => _quantity;
            set => SetProperty(ref _quantity, value);
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
        public CartPageViewModel(INavigationService navigationService,
            IPageDialogService pageDialogService,
            ICrashlyticsService crashlyticsService) : base(navigationService)
        {
            _pageDialogService = pageDialogService;
            _crashlyticsService = crashlyticsService;

            ListData.CollectionChanged += (s, e) => UpdateViewModeStateData(s as IEnumerable<object>);
        }

        //
        // Summary:
        //     Method responsible for performing the command action.
        // 
        public async void BuyAsync()
        {
            //_crashlyticsService.TrackEvent("Clicked buy", new Dictionary<string, string> { { "Total Products", _cart.Count().ToString() } });

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

            IList<ProductModelData> productModels = parameters.GetValue<IList<ProductModelData>>("cart");

            ListData.ReplaceRange(productModels);

            Quantity = ListData.Sum(x => x.Quantity);
            Total = ListData.Sum(x => x.FinalPrice);

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


                //ListData.ReplaceRange(await FactoryProductModelDataAsync(_products));
            }
            catch (Exception e)
            {
                _crashlyticsService.TrackError(e);

                ViewModelState.HasError = true;
            }
        }
    }
}