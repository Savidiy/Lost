using MvvmModule;
using UnityEngine;

namespace WireGameModule.View.WireGameLevel
{
    public sealed class WireGameLevelView : View<WireGameLevelHierarchy, IWireGameLevelViewModel>
    {
        // _view = _viewFactory.CreateView<WireConnectPointView, WireConnectPointHierarchy>(PREFAB_NAME, root);
        public WireGameLevelView(GameObject gameObject, IViewFactory viewFactory) : base(gameObject, viewFactory)
        {
        }

        protected override void UpdateViewModel(IWireGameLevelViewModel viewModel)
        {
        }
    }
}