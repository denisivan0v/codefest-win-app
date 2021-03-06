﻿using System;
using System.Reactive.Linq;

using CodeFestApp.ViewModels;

using ReactiveUI;

using Windows.Graphics.Display;
using Windows.UI.Xaml.Controls;

namespace CodeFestApp
{
    public sealed partial class HubView : IViewFor<HubViewModel>
    {
        public HubView()
        {
            InitializeComponent();

            // Hub is only supported in Portrait orientation
            DisplayInformation.AutoRotationPreferences = DisplayOrientations.Portrait;

            this.WhenAnyValue(x => x.ViewModel)
                .BindTo(this, x => x.DataContext);

            this.WhenAnyValue(x => x.ViewModel.LoadDaysCommand)
                               .ObserveOn(RxApp.TaskpoolScheduler)
                               .Subscribe(x => x.ExecuteAsyncTask());

            this.WhenAnyValue(x => x.ViewModel.LoadTracksCommand)
                .ObserveOn(RxApp.TaskpoolScheduler)
                .Subscribe(x => x.ExecuteAsyncTask());

            this.WhenAnyValue(x => x.ViewModel.LoadSpeakersCommand)
                .ObserveOn(RxApp.TaskpoolScheduler)
                .Subscribe(x => x.ExecuteAsyncTask());

            this.WhenAnyValue(x => x.ViewModel.LoadFavorites)
                .SubscribeOn(RxApp.TaskpoolScheduler)
                .Subscribe(x => x.ExecuteAsyncTask());

            this.WhenAnyValue(x => x.ViewModel.LoadCurrentLectures)
                .SubscribeOn(RxApp.TaskpoolScheduler)
                .Subscribe(x => x.ExecuteAsyncTask());
        }

        object IViewFor.ViewModel
        {
            get { return ViewModel; }
            set { ViewModel = (HubViewModel)value; }
        }

        public HubViewModel ViewModel { get; set; }

        private void DaysGridView_OnItemClick(object sender, ItemClickEventArgs e)
        {
            ViewModel.NavigateToDayCommand.ExecuteAsyncTask(e.ClickedItem);
        }

        private void TracksListView_OnItemClick(object sender, ItemClickEventArgs e)
        {
            ViewModel.NavigateToTrackCommand.ExecuteAsyncTask(e.ClickedItem);
        }
        
        private void SpeakersGridView_OnItemClick(object sender, ItemClickEventArgs e)
        {
            ViewModel.NavigateToSpeakerCommand.ExecuteAsyncTask(e.ClickedItem);
        }

        private void Hub_OnSectionsInViewChanged(object sender, SectionsInViewChangedEventArgs e)
        {
            var hub = (Hub)sender;
            var activeSection = hub.SectionsInView[0];
            ViewModel.ActiveSection = hub.Sections.IndexOf(activeSection);
        }

        private void FavoriteLecturesListView_OnItemClick(object sender, ItemClickEventArgs e)
        {
            ViewModel.HostScreen.Router.Navigate.ExecuteAsyncTask(e.ClickedItem);
        }

        private void CurrentLecturesListView_OnItemClick(object sender, ItemClickEventArgs e)
        {
            ViewModel.HostScreen.Router.Navigate.ExecuteAsyncTask(e.ClickedItem);
        }
    }
}