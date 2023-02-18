using MvvmModule;
using WireGameModule.View.WireGameWindow;

namespace WireGameModule.ViewModel
{
    public sealed class WireGameWindowViewModel : EmptyViewModel, IWireGameWindowViewModel
    {
        public string Text { get; } = "URA!";

        public WireGameWindowViewModel(IViewModelFactory viewModelFactory) : base(viewModelFactory)
        {
        }
    }
}