using MvvmModule;
using UniRx;
using UnityEngine;

namespace WireGameModule.View
{
    public interface IConnectPointViewModel : IViewModel
    {
        IReadOnlyReactiveProperty<EPointCondition> PointCondition { get; }
        Vector3 Position { get; }
        string Text { get; }
        void ClickFromView();
    }
}