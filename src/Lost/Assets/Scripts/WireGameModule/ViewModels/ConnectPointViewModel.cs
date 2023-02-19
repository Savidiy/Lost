using System;
using MvvmModule;
using UniRx;
using UnityEngine;
using WireGameModule.Infrastructure;
using WireGameModule.View;

namespace WireGameModule.ViewModels
{
    public sealed class ConnectPointViewModelArgs
    {
        public EPointGroup PointGroup { get; }
        public int Index { get; }
        public Vector3 Position { get; }
        public EPointCondition StartCondition { get; }

        public ConnectPointViewModelArgs(EPointGroup pointGroup, int index, Vector3 position, EPointCondition startCondition)
        {
            PointGroup = pointGroup;
            Index = index;
            Position = position;
            StartCondition = startCondition;
        }
    }

    public sealed class ConnectPointViewModel : ViewModel<ConnectPointViewModelArgs>, IConnectPointViewModel
    {
        private readonly ReactiveProperty<EPointCondition> _pointCondition = new();

        public IReadOnlyReactiveProperty<EPointCondition> PointCondition => _pointCondition;
        public Vector3 Position { get; }
        public string Text { get; } = string.Empty;

        public event Action<EPointGroup, int> Clicked; 

        public ConnectPointViewModel(ConnectPointViewModelArgs model, IViewModelFactory viewModelFactory) : base(model,
            viewModelFactory)
        {
            Position = model.Position;
            _pointCondition.Value = model.StartCondition;
        }

        public void SetPointCondition(EPointCondition pointCondition)
        {
            _pointCondition.Value = pointCondition;
        }

        public void ClickFromView()
        {
            Clicked?.Invoke(Model.PointGroup, Model.Index);
        }
    }
}