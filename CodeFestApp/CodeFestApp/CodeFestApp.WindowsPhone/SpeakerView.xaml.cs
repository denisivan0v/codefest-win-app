using System;

using CodeFestApp.ViewModels;

using ReactiveUI;

namespace CodeFestApp
{
    public sealed partial class SpeakerView : IViewFor<SpeakerViewModel>
    {
        public SpeakerView()
        {
            InitializeComponent();

            this.WhenAnyValue(x => x.ViewModel)
                .Subscribe(x => DataContext = x);
        } 

        object IViewFor.ViewModel
        {
            get { return ViewModel; }
            set { ViewModel = (SpeakerViewModel)value; }
        }

        public SpeakerViewModel ViewModel { get; set; }
    }
}