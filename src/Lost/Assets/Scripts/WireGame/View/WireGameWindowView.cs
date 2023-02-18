using MvvmModule;
using UnityEngine;

namespace Lost.WireGame.View
{
    internal sealed class WireGameWindowView : View<WireGameWindowHierarchy, IWireGameWindowViewModel>
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