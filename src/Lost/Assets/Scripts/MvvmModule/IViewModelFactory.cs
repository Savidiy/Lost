namespace MvvmModule
{
    public interface IViewModelFactory
    {
        TViewModel CreateViewModel<TViewModel>()
            where TViewModel : IViewModel;
    }
}