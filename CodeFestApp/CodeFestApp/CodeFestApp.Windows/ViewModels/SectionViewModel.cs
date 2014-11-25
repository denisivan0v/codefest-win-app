using System.Collections.Generic;

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

            NavigateToItemCommand = ReactiveCommand.Create();
        }

        public string UrlPathSegment
        {
            get { return "section"; }
        }

        public IScreen HostScreen { get; private set; }

        public SampleDataGroup Group
        {
            get { return _sampleDataGroup; }
        }

        public IEnumerable<SampleDataItem> Items
        {
            get { return _sampleDataGroup.Items; }
        }

        public ReactiveCommand<object> NavigateToItemCommand { get; private set; }
    }
}