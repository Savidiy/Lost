using System.Collections.Generic;
using MvvmModule;
using UniRx;
using UnityEngine;

namespace WireGameModule.View.WireGameWindow
{
    public interface IWireGameWindowViewModel : IViewModel
    {
        Sprite BackSprite { get; }
        IReadOnlyList<IConnectPointViewModel> PointsA { get; }
        IReadOnlyList<IConnectPointViewModel> PointsB { get; }
        IReadOnlyList<IWireViewModel> WireViewModels { get; }
        int TargetSum { get; }
        IReadOnlyReactiveProperty<int> CurrentSum { get; }
    }
}