using ReactiveUI;

namespace CodeFestApp.DI
{
    public interface IViewModelFactory
    {
        TViewModel Create<TViewModel, TModel>(TModel param) where TViewModel : IRoutableViewModel;
    }
}
