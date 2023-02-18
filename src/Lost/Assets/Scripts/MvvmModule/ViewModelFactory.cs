using Zenject;

namespace MvvmModule
{
    internal sealed class ViewModelFactory : IViewModelFactory
    {
        private readonly DiContainer _diContainer;

        public ViewModelFactory(DiContainer diContainer)
        {
            _diContainer = diContainer;
        }

        public TViewModel CreateViewModel<TViewModel>()
            where TViewModel : IViewModel
        {
            _diContainer.Bind<TViewModel>().AsSingle();

            var viewModel = _diContainer.Resolve<TViewModel>();

            _diContainer.Unbind<TViewModel>();

            return viewModel;
        }
    }
}