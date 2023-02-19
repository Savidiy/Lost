using MvvmModule;
using UniRx;
using UnityEngine;

namespace WireGameModule.View
{
    public interface IWireViewModel : IViewModel
    {
        IReadOnlyReactiveProperty<Vector3> StartPoint { get; }
        IReadOnlyReactiveProperty<Vector3> EndPoint { get; }
    }
}