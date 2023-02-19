using System.Collections.Generic;
using MvvmModule;
using UniRx;
using UnityEngine;
using WireGameModule.Infrastructure;

namespace WireGameModule.View.WireGameWindow
{
    public interface IWireGameWindowViewModel : IViewModel
    {
        public Sprite BackSprite { get; }
        public IReadOnlyList<IConnectPointViewModel> PointsA { get; }
        public IReadOnlyList<IConnectPointViewModel> PointsB { get; }
        public IReadOnlyList<PointPair> Connections { get; }
        public int TargetSum { get; }
        public IReadOnlyReactiveProperty<int> CurrentSum { get; }
    }
}