using System;

using CodeFestApp.ViewModels;

using ReactiveUI;

namespace CodeFestApp
{
    public sealed partial class LectureView : IViewFor<LectureViewModel>
    {
        public LectureView()
        {
            InitializeComponent();

            this.WhenAnyValue(x => x.ViewModel)
                .Subscribe(x => DataContext = x);
        } 

        object IViewFor.ViewModel
        {
            get { return ViewModel; }
            set { ViewModel = (LectureViewModel)value; }
        }

        public LectureViewModel ViewModel { get; set; }
    }
}