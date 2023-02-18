using WireGameModule.View.WireGameWindow;

namespace WireGameModule.ViewModel
{
    public sealed class WireGameWindowViewModel : IWireGameWindowViewModel
    {
        public string Text { get; } = "URA!";

        public void Dispose()
        {
        }
    }
}