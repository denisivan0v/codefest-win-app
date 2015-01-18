using CodeFestApp.Data;

using ReactiveUI;

namespace CodeFestApp.ViewModels
{
    public class ItemViewModel : ReactiveObject, IRoutableViewModel
    {
        private readonly SampleDataItem _sampleDataItem;

        public ItemViewModel(IScreen hostScreen, SampleDataItem sampleDataItem)
        {
            HostScreen = hostScreen;
            _sampleDataItem = sampleDataItem;
            
            GoBackCommand = ReactiveCommand.Create();

            this.WhenNavigatedTo(() =>
                {
                    SetItem();
                    return null;
                });
        }

        public string UrlPathSegment
        {
            get { return "item"; }
        }

        public IScreen HostScreen { get; private set; }

        public SampleDataItem Item { get; private set; }

        public ReactiveCommand<object> GoBackCommand { get; private set; }

        private async void SetItem()
        {
            Item = await SampleDataSource.GetItemAsync(_sampleDataItem.UniqueId);
        }
    }
} 