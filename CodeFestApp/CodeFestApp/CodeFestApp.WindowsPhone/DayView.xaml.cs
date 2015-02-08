using System;

using CodeFestApp.ViewModels;

using ReactiveUI;

namespace CodeFestApp
{
    public sealed partial class DayView : IViewFor<DayViewModel>
    {
        public DayView()
        {
            InitializeComponent();

            this.WhenAnyValue(x => x.ViewModel)
                .Subscribe(x => DataContext = x);
        } 

        object IViewFor.ViewModel
        {
            get { return ViewModel; }
            set { ViewModel = (DayViewModel)value; }
        }

        public DayViewModel ViewModel { get; set; }
    }
}