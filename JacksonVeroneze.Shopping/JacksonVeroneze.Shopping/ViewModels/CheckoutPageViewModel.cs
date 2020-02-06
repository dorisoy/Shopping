using Acr.UserDialogs;
using JacksonVeroneze.Shopping.Services.Interfaces;
using JacksonVeroneze.Shopping.Util;
using JacksonVeroneze.Shopping.Views;
using Prism.Commands;
using Prism.Navigation;
using Prism.Services;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

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
            if (string.IsNullOrEmpty(CardNumber) || string.IsNullOrEmpty(Expiration) || string.IsNullOrEmpty(Cvv))
            {
                await _pageDialogService.DisplayAlertAsync("Erro", "Antes de prosseguir, informe os dados do cartão de crédito.", "Ok");

                return;
            }

            if (IsCreditCardInfoValid(CardNumber, Expiration, Cvv) is false)
            {
                await _pageDialogService.DisplayAlertAsync("Erro", "Os dados do cartão são inválidos.", "Ok");
                return;
            }

            UserDialogs.Instance.ShowLoading("Aguarde, efetuando pagamento.", MaskType.Black);
            await Task.Delay(3000);
            UserDialogs.Instance.HideLoading();

            _crashlyticsService.TrackEvent(ApplicationEvents.CHECKOUT, new Dictionary<string, string> { { "Value", "Value" } });

            await _pageDialogService.DisplayAlertAsync("Aviso", "Pagamento efetuado com sucesso.", "Ok");
        }

        //
        // Summary:
        //     Method responsible for executing when the screen receives navigation.
        // 
        // Parameters:
        //   parameters:
        //     The parameters param.
        //
        public override void Initialize(INavigationParameters parameters)
        {
            ViewModelState.IsLoading = true;

            _crashlyticsService.TrackEvent(ApplicationEvents.OPEN_SCREAM,
                    new Dictionary<string, string>() { { "Page", nameof(CheckoutPage) } });

            ViewModelState.IsLoading = false;
        }

        public static bool IsCreditCardInfoValid(string cardNo, string expiryDate, string cvv)
        {
            var cardCheck = new Regex(@"^(1298|1267|4512|4567|8901|8933)([\-\s]?[0-9]{4}){3}$");
            var monthCheck = new Regex(@"^(0[1-9]|1[0-2])$");
            var yearCheck = new Regex(@"^20[0-9]{2}$");
            var cvvCheck = new Regex(@"^\d{3}$");

            if (!cardCheck.IsMatch(cardNo))
                return false;
            if (!cvvCheck.IsMatch(cvv))
                return false;

            var dateParts = expiryDate.Split('/');    
            if (!monthCheck.IsMatch(dateParts[0]) || !yearCheck.IsMatch(dateParts[1])) // <3 - 6>
                return false;

            var year = int.Parse(dateParts[1]);
            var month = int.Parse(dateParts[0]);
            var lastDateOfExpiryMonth = DateTime.DaysInMonth(year, month);
            var cardExpiry = new DateTime(year, month, lastDateOfExpiryMonth, 23, 59, 59);

            return (cardExpiry > DateTime.Now && cardExpiry < DateTime.Now.AddYears(6));
        }
    }
}