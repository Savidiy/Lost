using UnityEngine;
using WireGameModule.View;

namespace WireGameModule.ViewModels
{
    public sealed class WireViewModel : IWireViewModel
    {
        // private readonly IViewModelFactory _viewModelFactory;
        // var viewModel = _viewModelFactory.CreateViewModel<WireViewModel>();

        public Vector3 StartPoint { get; }
        public Vector3 EndPoint { get; }

        public void Dispose()
        {
        }
    }
}