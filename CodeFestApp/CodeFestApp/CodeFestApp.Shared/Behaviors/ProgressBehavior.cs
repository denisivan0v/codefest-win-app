using System;

using Microsoft.Xaml.Interactivity;

using Windows.UI.ViewManagement;
using Windows.UI.Xaml;

namespace CodeFestApp.Behaviors
{
    public class ProgressBehavior : DependencyObject, IBehavior
    {
        public static readonly DependencyProperty TextProperty =
            DependencyProperty.Register("Text",
                                        typeof(string),
                                        typeof(ProgressBehavior),
                                        new PropertyMetadata(null, OnTextChanged));

        public static readonly DependencyProperty IsVisibleProperty =
            DependencyProperty.Register("IsVisible",
                                        typeof(bool),
                                        typeof(ProgressBehavior),
                                        new PropertyMetadata(false, OnIsVisibleChanged));

        public static readonly DependencyProperty ValueProperty =
            DependencyProperty.Register("Value",
                                        typeof(double),
                                        typeof(ProgressBehavior),
                                        new PropertyMetadata(null, OnValueChanged));

        public static readonly DependencyProperty IsIndeterminateProperty =
            DependencyProperty.Register("IsIndeterminate",
                                        typeof(bool),
                                        typeof(ProgressBehavior),
                                        new PropertyMetadata(false, OnIsIndeterminateChanged));
 

        public bool IsVisible
        {
            get { return (bool)GetValue(IsVisibleProperty); }
            set { SetValue(IsVisibleProperty, value); }
        }

        public string Text
        {
            get { return (string)GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }

        public double Value
        {
            get { return (double)GetValue(ValueProperty); }
            set { SetValue(ValueProperty, value); }
        }

        public bool IsIndeterminate
        {
            get { return (bool)GetValue(IsIndeterminateProperty); }
            set { SetValue(IsIndeterminateProperty, value); }
        }

        public DependencyObject AssociatedObject { get; private set; }

        public void Attach(DependencyObject associatedObject)
        {
        }

        public void Detach()
        {
        }

        private static async void OnIsVisibleChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var progressIndicator = StatusBar.GetForCurrentView().ProgressIndicator;

            var isvisible = (bool)e.NewValue;
            if (isvisible)
            {
                await progressIndicator.ShowAsync();
            }
            else
            {
                await progressIndicator.HideAsync();
            }
        }

        private static void OnTextChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            string text = null;
            if (e.NewValue != null)
            {
                text = e.NewValue.ToString();
            }

            StatusBar.GetForCurrentView().ProgressIndicator.Text = text;
        }

        private static void OnValueChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            StatusBar.GetForCurrentView().ProgressIndicator.ProgressValue = (double)e.NewValue;
        }

        private static void OnIsIndeterminateChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var progressIndicator = StatusBar.GetForCurrentView().ProgressIndicator;
            if ((bool)e.NewValue)
            {
                progressIndicator.ProgressValue = null;
            }
            else
            {
                progressIndicator.ProgressValue = 0;
            }
        }
    }
}