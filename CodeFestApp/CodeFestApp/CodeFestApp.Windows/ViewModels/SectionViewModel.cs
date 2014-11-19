using CodeFestApp.Data;

using ReactiveUI;

namespace CodeFestApp.ViewModels
{
    public class SectionViewModel : ReactiveObject, IRoutableViewModel
    {
        private readonly SampleDataGroup _sampleDataGroup;

        public SectionViewModel(IScreen hostScreen, SampleDataGroup sampleDataGroup)
        {
            _sampleDataGroup = sampleDataGroup;
            HostScreen = hostScreen;
        }

        public string UrlPathSegment
        {
            get { return "section"; }
        }

        public IScreen HostScreen { get; private set; }
    }
}