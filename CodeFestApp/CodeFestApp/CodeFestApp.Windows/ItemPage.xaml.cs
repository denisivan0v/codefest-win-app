using System;

using CodeFestApp.ViewModels;

using ReactiveUI;

namespace CodeFestApp
{
    /// <summary>
    /// A page that displays details for a single item within a group.
    /// </summary>
    public sealed partial class ItemPage : IViewFor<ItemViewModel>
    {
        public ItemPage()
        {
            InitializeComponent();

            this.Bind(ViewModel, x => x.Item, x => x.ItemView.DataContext);
            this.BindCommand(ViewModel, x => x.GoBackCommand, x => x.BackButton);

            this.WhenAnyObservable(x => x.ViewModel.GoBackCommand)
                .Subscribe(x => ViewModel.HostScreen.Router.NavigateBack.Execute(null));
        }
        
        object IViewFor.ViewModel
        {
            get { return ViewModel; }
            set { ViewModel = (ItemViewModel)value; }
        }

        public ItemViewModel ViewModel { get; set; }
    }
}