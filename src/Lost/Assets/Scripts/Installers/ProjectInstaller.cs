using Lost;
using MvvmModule;
using SettingsModule;
using UiModule;
using WireGameModule.ViewModel;
using Zenject;

namespace Installers
{
    public class ProjectInstaller : MonoInstaller
    {
        public GameSettings GameSettings;
        
        public override void InstallBindings()
        {
            Container.BindInterfacesTo<Bootstrapper>().AsSingle();

            Container.BindInterfacesTo<PrefabFactory>().AsSingle();
            Container.BindInterfacesTo<ViewFactory>().AsSingle();
            Container.BindInterfacesTo<ViewModelFactory>().AsSingle();

            Container.Bind<WireGamePresenter>().AsSingle();
            Container.Bind<WindowsRootProvider>().AsSingle();
            
            Container.Bind<GameSettings>().FromInstance(GameSettings);
        }
    }
}