using CodeFestApp.ViewModels;

using ReactiveUI;

namespace CodeFestApp
{
    public sealed partial class AboutView : IViewFor<AboutViewModel>
    {
        public AboutView()
        {
            InitializeComponent();

            this.WhenAnyValue(x => x.ViewModel)
                .BindTo(this, x => x.DataContext);
        }

        object IViewFor.ViewModel
        {
            get { return ViewModel; }
            set { ViewModel = (AboutViewModel)value; }
        }

        public AboutViewModel ViewModel { get; set; }
    }
}
