using MvvmModule;
using UniRx;
using UnityEngine;
using WireGameModule.View;

namespace WireGameModule.ViewModels
{
    public sealed class WireViewModel : EmptyViewModel, IWireViewModel
    {
        private readonly ReactiveProperty<Vector3> _startPoint = new();
        private readonly ReactiveProperty<Vector3> _endPoint = new();

        public IReadOnlyReactiveProperty<Vector3> StartPoint => _startPoint;
        public IReadOnlyReactiveProperty<Vector3> EndPoint => _endPoint;

        public WireViewModel(IViewModelFactory viewModelFactory) : base(viewModelFactory)
        {
        }

        public void UpdatePoints(Vector3 start, Vector3 end)
        {
            _startPoint.Value = start;
            _endPoint.Value = end;
        }
    }
}