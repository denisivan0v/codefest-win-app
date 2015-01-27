﻿using System;

using CodeFestApp.ViewModels;

using ReactiveUI;

namespace CodeFestApp
{
    public sealed partial class ItemPage : IViewFor<ItemViewModel>
    {
        public ItemPage()
        {
            InitializeComponent();

            this.WhenAnyValue(x => x.ViewModel)
                .Subscribe(x => DataContext = x);
        } 

        object IViewFor.ViewModel
        {
            get { return ViewModel; }
            set { ViewModel = (ItemViewModel)value; }
        }

        public ItemViewModel ViewModel { get; set; }
    }
}