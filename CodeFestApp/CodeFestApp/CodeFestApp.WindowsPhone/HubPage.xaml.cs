﻿using System;
using System.Reactive.Linq;

using CodeFestApp.Data;
using CodeFestApp.ViewModels;

using ReactiveUI;

using Windows.Graphics.Display;
using Windows.UI.Xaml.Controls;

namespace CodeFestApp
{
    public sealed partial class HubPage : IViewFor<HubViewModel>
    {
        public HubPage()
        {
            InitializeComponent();

            // Hub is only supported in Portrait orientation
            DisplayInformation.AutoRotationPreferences = DisplayOrientations.Portrait;

            this.WhenAnyValue(x => x.ViewModel)
                .Subscribe(x => DataContext = x);

            this.WhenAnyObservable(x => x.ViewModel.NavigateToSectionCommand)
                .Cast<ItemClickEventArgs>()
                .Select(x => x.ClickedItem)
                .Cast<SampleDataGroup>()
                .BindTo(this, x => x.ViewModel.GroupToNavigate);

            this.WhenAnyObservable(x => x.ViewModel.NavigateToItemCommand)
                .Cast<ItemClickEventArgs>()
                .Select(x => x.ClickedItem)
                .Cast<SampleDataItem>()
                .BindTo(this, x => x.ViewModel.ItemToNavigate);
        }

        object IViewFor.ViewModel
        {
            get { return ViewModel; }
            set { ViewModel = (HubViewModel)value; }
        }

        public HubViewModel ViewModel { get; set; }
    }
}