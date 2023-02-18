using MvvmModule;
using UnityEngine;

namespace WireGameModule.View
{
    public interface IWireViewModel : IViewModel
    {
        Vector3 StartPoint { get; }
        Vector3 EndPoint { get; }
    }
}