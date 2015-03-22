using System;

using ReactiveUI;

namespace CodeFestApp.ViewModels
{
    public class AboutViewModel : ReactiveObject, IRoutableViewModel
    {
        public AboutViewModel(IScreen hostScreen)
        {
            HostScreen = hostScreen;
        }

        public string ConferenceTitle
        {
            get { return "codefest 2015"; }
        }

        public string Title
        {
            get { return "о приложении"; }
        }

        public Uri GitHubLink
        {
            get { return new Uri("https://github.com/denisivan0v/codefest-win-app"); }
        }

        public string GitHubLinkContent
        {
            get { return "CodeFest App on GitHub"; }
        }

        public Uri FeedbackEmail
        {
            get { return new Uri("mailto:me@ivanovdenis.ru"); }
        }

        public string FeedbackEmailContent
        {
            get { return "me@ivanovdenis.ru"; }
        }

        public string UrlPathSegment
        {
            get { return "about"; }
        }

        public IScreen HostScreen { get; private set; }
    }
}
