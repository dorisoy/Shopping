using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Text;

namespace JacksonVeroneze.Shopping.ViewModels
{
    public class ViewModelState : BindableBase
    {
        private bool _hasNetworkAccess = false;
        public bool HasNetworkAccess
        {
            get => _hasNetworkAccess;
            set => SetProperty(ref _hasNetworkAccess, value);
        }

        private bool _isLoading = false;
        public bool IsLoading
        {
            get => _isLoading;
            set => SetProperty(ref _isLoading, value);
        }

        private bool _isBusy = false;
        public bool IsBusy
        {
            get => _isBusy;
            set => SetProperty(ref _isBusy, value);
        }

        private bool _isBusyNavigating = false;
        public bool IsBusyNavigating
        {
            get => _isBusyNavigating;
            set => SetProperty(ref _isBusyNavigating, value);
        }

        private bool _isRefresh = false;
        public bool IsRefresh
        {
            get => _isRefresh;
            set => SetProperty(ref _isRefresh, value);
        }

        private bool _isSearch = false;
        public bool IsSearch
        {
            get => _isSearch;
            set => SetProperty(ref _isSearch, value);
        }

        private bool _hasData = false;
        public bool HasData
        {
            get => _hasData;
            set => SetProperty(ref _hasData, value);
        }

        private bool _noData = false;
        public bool NoData
        {
            get => _noData;
            set => SetProperty(ref _noData, value);
        }

        private bool _hasError = false;
        public bool HasError
        {
            get => _hasError;
            set
            {
                SetProperty(ref _hasError, value);

                if (_hasError is true)
                {
                    IsLoading = false;
                    IsBusy = false;
                    IsRefresh = false;
                    IsSearch = false;
                    HasData = false;
                    NoData = false;
                }
            }
        }
    }
}
