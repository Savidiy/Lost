using UnityEngine;

namespace MvvmModule.Templates
{
    public sealed class A123Hierarchy : MonoBehaviour
    {
    }

    // _view = _viewFactory.CreateView<A123View, A123Hierarchy>(PREFAB_NAME, root);
    public sealed class A123View : View<A123Hierarchy, IA123ViewModel>
    {
        public A123View(GameObject gameObject, IViewFactory viewFactory) : base(gameObject, viewFactory)
        {
        }

        protected override void UpdateViewModel(IA123ViewModel viewModel)
        {
        }
    }

    public interface IA123ViewModel : IViewModel
    {
    }

    // private readonly IViewModelFactory _viewModelFactory;
    // var viewModel = _viewModelFactory.CreateViewModel<A123ViewModel>();
    public sealed class A123ViewModel : IA123ViewModel
    {
        public void Dispose()
        {
        }
    }
}