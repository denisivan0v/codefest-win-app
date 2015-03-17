using ReactiveUI;

namespace CodeFestApp.DI
{
    public interface IViewModelFactory
    {
        TViewModel Create<TViewModel>() where TViewModel : IRoutableViewModel;
        TViewModel Create<TViewModel, TModel>(TModel param) where TViewModel : IRoutableViewModel;
    }
}
