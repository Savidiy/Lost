using MvvmModule;
using SettingsModule;
using UnityEngine;

namespace WireGameModule.View
{
    public sealed class WireView : View<WireGameWireHierarchy, IWireViewModel>
    {
        private readonly GameSettings _gameSettings;

        public WireView(GameObject gameObject, IViewFactory viewFactory, GameSettings gameSettings) : base(gameObject, viewFactory)
        {
            _gameSettings = gameSettings;
        }

        protected override void UpdateViewModel(IWireViewModel viewModel)
        {
            Hierarchy.Image.color = viewModel.Color;
            BindSilently(viewModel.StartPoint, OnPointChange);
            Bind(viewModel.EndPoint, OnPointChange);
        }

        private void OnPointChange(Vector3 _)
        {
            Vector3 startPoint = ViewModel.StartPoint.Value;
            Vector3 endPoint = ViewModel.EndPoint.Value;

            Vector3 delta = endPoint - startPoint;
            float magnitude = delta.magnitude;
            var localScale = new Vector3(magnitude, _gameSettings.WireY, _gameSettings.WireZ);

            float angle = Vector3.Angle(delta, Vector3.right);
            if (delta.y < 0)
                angle *= -1f;
            Quaternion rotation = Quaternion.Euler(0, 0, angle);

            Hierarchy.RectTransform.position = startPoint;
            Hierarchy.RectTransform.localScale = localScale;
            Hierarchy.RectTransform.rotation = rotation;
        }

        public override void Dispose()
        {
            base.Dispose();
            if (Hierarchy != null)
                Object.Destroy(Hierarchy.gameObject);
        }
    }
}