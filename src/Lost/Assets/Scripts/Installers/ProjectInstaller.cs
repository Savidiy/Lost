using Lost;
using Lost.WireGame.ViewModel;
using MvvmModule;
using Zenject;

namespace Installers
{
    public class ProjectInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.BindInterfacesTo<Bootstrapper>().AsSingle();

            Container.BindInterfacesTo<PrefabFactory>().AsSingle();
            Container.BindInterfacesTo<ViewFactory>().AsSingle();
            Container.BindInterfacesTo<ViewModelFactory>().AsSingle();

            Container.Bind<WireGamePresenter>().AsSingle();
            Container.Bind<WindowsRootProvider>().AsSingle();
        }
    }
}