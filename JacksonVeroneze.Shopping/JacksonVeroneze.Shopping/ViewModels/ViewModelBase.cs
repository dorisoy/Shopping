using Prism.AppModel;
using Prism.Mvvm;
using Prism.Navigation;
using System.Threading.Tasks;

namespace JacksonVeroneze.Shopping.ViewModels
{
    public class ViewModelBase : BindableBase, INavigationAware, IDestructible, IApplicationLifecycleAware, IInitialize
    {
        protected INavigationService _navigationService { get; private set; }

        private static bool _hasAttachedEventConnectivityChanged = false;

        public ViewModelBase(INavigationService navigationService)
            => _navigationService = navigationService;

        public virtual void OnNavigatedFrom(INavigationParameters parameters) { }

        public virtual void OnNavigatedTo(INavigationParameters parameters) { }

        public virtual void Initialize(INavigationParameters parameters) { }

        public virtual void Destroy() { }

        public virtual void OnResume() { }

        public virtual void OnSleep() { }

        public virtual void OnDisappearing() { }

        internal virtual void OnBackButtonPressed() { }

        internal virtual Task OnBackButtonPressedAsync() => Task.CompletedTask;
    }
}