using WireGameModule.ViewModel;
using Zenject;

namespace Lost
{
    public class Bootstrapper : IInitializable
    {
        private readonly WireGamePresenter _wireGamePresenter;

        public Bootstrapper(WireGamePresenter wireGamePresenter)
        {
            _wireGamePresenter = wireGamePresenter;
        }
        
        public void Initialize()
        {
            _wireGamePresenter.ShowWindow();
        }
    }
}