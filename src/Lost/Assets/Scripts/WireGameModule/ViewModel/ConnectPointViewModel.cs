using System;
using UniRx;
using UnityEngine;
using WireGameModule.View;

namespace WireGameModule.ViewModel
{
    public sealed class ConnectPointViewModel : IConnectPointViewModel
    {
        private readonly ReactiveProperty<EPointCondition> _pointCondition = new();
        // private readonly IViewModelFactory _viewModelFactory;
        // var viewModel = _viewModelFactory.CreateViewModel<WireConnectPointViewModel>();

        public IReadOnlyReactiveProperty<EPointCondition> PointCondition => _pointCondition;
        public Vector3 Position { get; }
        public string Text { get; } = string.Empty;

        public ConnectPointViewModel()
        {
            EPointCondition pointCondition = EPointCondition.Selected;
            SetPointColor(pointCondition);
        }

        private void SetPointColor(EPointCondition pointCondition)
        {
            throw new System.NotImplementedException();
        }

        public void ClickFromView()
        {
            throw new System.NotImplementedException();
        }
    }
}