using System;
using System.Reactive.Linq;

using CodeFestApp.ViewModels;

using ReactiveUI;

using Windows.UI.Xaml;

namespace CodeFestApp
{
    public sealed partial class TweetsView : IViewFor<TweetsViewModel>
    {
        public TweetsView()
        {
            InitializeComponent();

            this.WhenAnyValue(x => x.ViewModel)
                .BindTo(this, x => x.DataContext);

            this.WhenAnyValue(x => x.ViewModel.SearchForTweetsCommand)
                .ObserveOn(RxApp.TaskpoolScheduler)
                .Subscribe(x => x.ExecuteAsyncTask());

            this.WhenAnyValue(x => x.ViewModel.IsBusy)
                .Subscribe(x =>
                    {
                        ProgressBar.Visibility = x ? Visibility.Visible : Visibility.Collapsed;
                    });
        }

        object IViewFor.ViewModel
        {
            get { return ViewModel; }
            set { ViewModel = (TweetsViewModel)value; }
        }

        public TweetsViewModel ViewModel { get; set; }
    }
}
