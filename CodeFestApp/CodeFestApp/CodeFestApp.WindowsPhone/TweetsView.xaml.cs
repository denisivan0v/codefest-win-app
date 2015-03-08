using System;

using CodeFestApp.ViewModels;

using ReactiveUI;

namespace CodeFestApp
{
    public sealed partial class TweetsView : IViewFor<TweetsViewModel>
    {
        public TweetsView()
        {
            InitializeComponent();

            this.WhenAnyValue(x => x.ViewModel)
                .Subscribe(x => DataContext = x);
        }

        object IViewFor.ViewModel
        {
            get { return ViewModel; }
            set { ViewModel = (TweetsViewModel)value; }
        }

        public TweetsViewModel ViewModel { get; set; }
    }
}
