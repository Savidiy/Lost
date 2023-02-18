using System;
using MvvmModule;
using UiModule;
using UnityEngine;
using WireGameModule.View.WireGameWindow;

namespace WireGameModule.ViewModel
{
    public class WireGamePresenter : IDisposable
    {
        private const string PREFAB_NAME = "WireGameWindow";
        private readonly WindowsRootProvider _windowsRootProvider;
        private readonly IViewFactory _viewFactory;
        private readonly IViewModelFactory _viewModelFactory;
        private WireGameWindowView _wireGameWindowView;

        public WireGamePresenter(WindowsRootProvider windowsRootProvider, IViewFactory viewFactory,
            IViewModelFactory viewModelFactory)
        {
            _viewFactory = viewFactory;
            _viewModelFactory = viewModelFactory;
            _windowsRootProvider = windowsRootProvider;
            CreateView();
        }

        public void ShowWindow()
        {
            var viewModel = _viewModelFactory.CreateViewModel<WireGameWindowViewModel>();
            _wireGameWindowView.Initialize(viewModel);
        }

        public void HideWindow()
        {
            _wireGameWindowView.ClearViewModel();
        }

        public void Dispose()
        {
            _wireGameWindowView.Dispose();
        }

        private void CreateView()
        {
            Transform root = _windowsRootProvider.GetWindowRoot();
            _wireGameWindowView = _viewFactory.CreateView<WireGameWindowView, WireGameWindowHierarchy>(PREFAB_NAME, root);
        }
    }
}