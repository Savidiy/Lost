using MvvmModule;
using UnityEngine;

namespace WireGameModule.View.WireGameWindow
{
    public sealed class WireGameWindowView : View<WireGameWindowHierarchy, IWireGameWindowViewModel>
    {
        public WireGameWindowView(GameObject gameObject, IViewFactory viewFactory) : base(gameObject, viewFactory)
        {
        }

        protected override void UpdateViewModel(IWireGameWindowViewModel viewModel)
        {
            Hierarchy.Label.text = viewModel.Text;
        }
    }
}