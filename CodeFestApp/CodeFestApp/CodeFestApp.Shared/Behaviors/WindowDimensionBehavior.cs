using Microsoft.Xaml.Interactivity;

using Windows.UI.Xaml;

namespace CodeFestApp.Behaviors
{
    [TypeConstraint(typeof(FrameworkElement))]
    internal class WindowDimensionBehavior : DependencyObject, IBehavior
    {
        public WindowDimensionBehavior()
        {
            WidthMultiple = 1;
            HeightMultiple = 1;
            HeightPercentage = double.NaN;
            WidthPercentage = double.NaN;
        }

        public DependencyObject AssociatedObject { get; set; }
        public int WidthMultiple { get; set; }
        public double WidthPercentage { get; set; }
        public int HeightMultiple { get; set; }
        public double HeightPercentage { get; set; }

        public void Attach(DependencyObject associatedObject)
        {
            AssociatedObject = associatedObject;

            var frameworkElement = AssociatedObject as FrameworkElement;
            if (frameworkElement != null)
            {
                frameworkElement.Loaded += TypedObject_Loaded;
            }

            Window.Current.SizeChanged += Current_SizeChanged;
        }

        public void Detach()
        {
            var frameworkElement = AssociatedObject as FrameworkElement;
            if (frameworkElement != null)
            {
                frameworkElement.Loaded -= TypedObject_Loaded;
            }

            Window.Current.SizeChanged -= Current_SizeChanged;
        }

        private void TypedObject_Loaded(object sender, RoutedEventArgs e)
        {
            Handle();
        }

        private void Current_SizeChanged(object sender, Windows.UI.Core.WindowSizeChangedEventArgs e)
        {
            Handle();
        }

        private void Handle()
        {
            var frameWorkElement = AssociatedObject as FrameworkElement;

            var shortestDimension = Window.Current.Bounds.Width < Window.Current.Bounds.Height
                                        ? Window.Current.Bounds.Width
                                        : Window.Current.Bounds.Height;

            if (frameWorkElement != null)
            {
                if (!double.IsNaN(WidthPercentage))
                {
                    frameWorkElement.Width = shortestDimension * WidthPercentage * WidthMultiple;
                }

                if (!double.IsNaN(HeightPercentage))
                {
                    frameWorkElement.Height = shortestDimension * HeightPercentage * HeightMultiple;
                }
            }
        }
    }
}
