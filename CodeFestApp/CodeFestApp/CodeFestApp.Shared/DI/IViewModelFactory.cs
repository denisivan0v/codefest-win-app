namespace CodeFestApp.DI
{
    public interface IViewModelFactory<out TViewModel>
    {
        TViewModel Create<TParam>(TParam param);
    }
}
