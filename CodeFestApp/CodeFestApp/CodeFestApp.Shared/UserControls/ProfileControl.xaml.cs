////*******************************************************
//// 
//// Copyright (c) Microsoft. All rights reserved. 
//// THIS CODE IS PROVIDED *AS IS* WITHOUT WARRANTY OF 
//// ANY KIND, EITHER EXPRESS OR IMPLIED, INCLUDING ANY 
//// IMPLIED WARRANTIES OF FITNESS FOR A PARTICULAR 
//// PURPOSE, MERCHANTABILITY, OR NON-INFRINGEMENT. 
//// 
////*******************************************************

using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236
namespace CodeFestApp.UserControls
{
    public sealed partial class ProfileControl : UserControl
    {
        public static readonly DependencyProperty ShowDescriptionProperty =
            DependencyProperty.Register("ShowDescription", typeof(bool), typeof(ProfileControl), null);

        public ProfileControl()
        {
            InitializeComponent();
        }

        public bool ShowDescription
        {
            get { return (bool)GetValue(ShowDescriptionProperty); }
            set { SetValue(ShowDescriptionProperty, value); }
        }
    }
}
