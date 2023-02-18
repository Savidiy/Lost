using Lost.WireGame.View;

namespace Lost.WireGame.ViewModel
{
    public sealed class WireGameWindowViewModel : IWireGameWindowViewModel
    {
        public string Text { get; } = "URA!";

        public void Dispose()
        {
        }
    }
}