using UnityEngine;

namespace WireGameModule.View
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