using System;
using System.Reactive;

using CodeFestApp.Data;

using ReactiveUI;

using Splat;

using Windows.UI.Xaml.Controls;

namespace CodeFestApp.ViewModels
{
    public class HubViewModel : ReactiveObject, IScreen
    {
        public HubViewModel(IMutableDependencyResolver dependencyResolver = null, RoutingState routingState = null)
        {
            RegisterParts(dependencyResolver ?? Locator.CurrentMutable);
            
            Router = routingState ?? new RoutingState();
            NavigateCommand = ReactiveCommand.Create();
            
            NavigateCommand.Subscribe(x =>
                {
                    var eventPattern = (EventPattern<HubSectionHeaderClickEventArgs>)x;
                    
                    var sampleDataGroup = (SampleDataGroup)eventPattern.EventArgs.Section.DataContext;
                    Router.Navigate.Execute(new SectionViewModel(this, sampleDataGroup));
                });

            this.WhenAny(vm => vm.Changed, x => true)
                .Subscribe(async _ =>
                    {
                        Section3Items = await SampleDataSource.GetGroupAsync("Group-4");
                    });
        }

        public RoutingState Router { get; private set; }

        public ReactiveCommand<object> NavigateCommand { get; set; }

        public SampleDataGroup Section3Items { get; private set; }

        private void RegisterParts(IMutableDependencyResolver dependencyResolver)
        {
            dependencyResolver.RegisterConstant(this, typeof(IScreen));
            dependencyResolver.Register(() => new SectionPage(), typeof(IViewFor<SectionViewModel>));
        }
    }
}