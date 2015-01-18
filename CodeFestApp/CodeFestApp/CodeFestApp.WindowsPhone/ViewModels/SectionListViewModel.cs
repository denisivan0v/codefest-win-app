using ReactiveUI;

namespace CodeFestApp.ViewModels
{
    public class SectionListViewModel : ReactiveObject, IRoutableViewModel
    {
        public SectionListViewModel(IScreen screen)
        {
            HostScreen = screen;
            NavigateToSectionCommand = ReactiveCommand.Create();
        }

        public ReactiveCommand<object> NavigateToSectionCommand { get; private set; }

        public string UrlPathSegment
        {
            get { return "section-list"; }
        }

        public IScreen HostScreen { get; private set; }
    }
}