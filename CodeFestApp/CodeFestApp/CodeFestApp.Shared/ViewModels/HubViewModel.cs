using System.Linq;

using CodeFestApp.Data;

using ReactiveUI;

namespace CodeFestApp.ViewModels
{
    public class HubViewModel : ReactiveObject, IRoutableViewModel
    {
        public HubViewModel(IScreen screen)
        {
            HostScreen = screen;
            NavigateToSectionCommand = ReactiveCommand.Create();
            NavigateToItemCommand = ReactiveCommand.Create();

            this.WhenNavigatedTo(() =>
                {
                    SetGroups();
                    return null;
                });
        }

        public ReactiveCommand<object> NavigateToSectionCommand { get; private set; }
        public ReactiveCommand<object> NavigateToItemCommand { get; private set; }

        public SampleDataGroup[] Groups { get; private set; }
        
        public IScreen HostScreen { get; private set; }

        public string UrlPathSegment
        {
            get { return "hubpage"; }
        }

        private async void SetGroups()
        {
            Groups = await SampleDataSource.GetGroupsAsync();
        }
    }
}