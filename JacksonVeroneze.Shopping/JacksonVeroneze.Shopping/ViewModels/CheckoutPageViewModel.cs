using JacksonVeroneze.Shopping.Services.Interfaces;
using JacksonVeroneze.Shopping.Util;
using JacksonVeroneze.Shopping.Views;
using Prism.Commands;
using Prism.Navigation;
using Prism.Services;
using System.Collections.Generic;

namespace JacksonVeroneze.Shopping.ViewModels
{
    //
    // Summary:
    //     Class responsible for viewModel.
    //
    public class CheckoutPageViewModel : ViewModelBase
    {
        private readonly IPageDialogService _pageDialogService;
        //
        private readonly ICrashlyticsService _crashlyticsService;

        private DelegateCommand _buyCommand;

        public DelegateCommand BuyCommand =>
            _buyCommand ?? (
                _buyCommand = new DelegateCommand(BuyAsync));

        private string _cardNumber;
        public string CardNumber
        {
            get => _cardNumber;
            set => SetProperty(ref _cardNumber, value);
        }

        private string _expiration;
        public string Expiration
        {
            get => _expiration;
            set => SetProperty(ref _expiration, value);
        }

        private string _cvv;
        public string Cvv
        {
            get => _cvv;
            set => SetProperty(ref _cvv, value);
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
        public CheckoutPageViewModel(INavigationService navigationService,
            IPageDialogService pageDialogService,
            ICrashlyticsService crashlyticsService) : base(navigationService)
        {
            _pageDialogService = pageDialogService;
            _crashlyticsService = crashlyticsService;
        }

        //
        // Summary:
        //     Method responsible for performing the command action.
        // 
        public async void BuyAsync()
        {

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

            _crashlyticsService.TrackEvent(ApplicationEvents.OPEN_SCREAM,
                    new Dictionary<string, string>() { { "Page", nameof(CheckoutPage) } });

            ViewModelState.IsLoading = false;
        }
    }
}
