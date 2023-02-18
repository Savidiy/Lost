using MvvmModule;
using SettingsModule;
using UnityEngine;

namespace WireGameModule.View
{
    public sealed class WireView : View<WireGameWireHierarchy, IWireViewModel>
    {
        private readonly GameSettings _gameSettings;

        // _view = _viewFactory.CreateView<WireView, WireHierarchy>(PREFAB_NAME, root);
        public WireView(GameObject gameObject, IViewFactory viewFactory, GameSettings gameSettings) : base(gameObject, viewFactory)
        {
            _gameSettings = gameSettings;
        }

        protected override void UpdateViewModel(IWireViewModel viewModel)
        {
            Vector3 startPoint = viewModel.StartPoint;
            Vector3 endPoint = viewModel.EndPoint;

            Vector3 delta = endPoint - startPoint;
            float magnitude = delta.magnitude;
            var localScale = new Vector3(magnitude, _gameSettings.WireY, _gameSettings.WireZ);

            float angle = Vector3.Angle(startPoint, endPoint);
            Quaternion rotation = Quaternion.Euler(0, 0, angle);

            Hierarchy.RectTransform.position = startPoint;
            Hierarchy.RectTransform.localScale = localScale;
            Hierarchy.RectTransform.rotation = rotation;
        }
    }
}