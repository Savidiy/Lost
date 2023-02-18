using UniRx;
using UnityEngine;
using WireGameModule.View;

namespace WireGameModule.Setup
{
    internal class SetupConnectPointViewModel : IConnectPointViewModel
    {
        public IReadOnlyReactiveProperty<EPointCondition> PointCondition { get; } =
            new ReactiveProperty<EPointCondition>(EPointCondition.Unused);

        public Vector3 Position { get; }
        public string Text { get; }

        public SetupConnectPointViewModel(Vector3 position, string text)
        {
            Position = position;
            Text = text;
        }

        public void ClickFromView()
        {
            Debug.Log("Clicked");
        }
    }
}