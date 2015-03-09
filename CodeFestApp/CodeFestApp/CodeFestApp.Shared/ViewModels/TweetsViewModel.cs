using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

using CodeFestApp.DataModel;

using LinqToTwitter;

using ReactiveUI;

namespace CodeFestApp.ViewModels
{
    public class TweetsViewModel : ReactiveObject, IRoutableViewModel
    {
        private readonly ObservableAsPropertyHelper<ReactiveList<Tweet>> _tweets;

        public TweetsViewModel(IScreen hostScreen)
        {
            HostScreen = hostScreen;

            SearchForTweetsCommand = ReactiveCommand.CreateAsyncTask(_ => SearchForTweets("codefestru", "#codefest"));
            _tweets = SearchForTweetsCommand.ToProperty(this, x => x.Tweets);
        }

        public string Title
        {
            get { return "твиттер-лента"; }
        }

        public ReactiveList<Tweet> Tweets
        {
            get { return _tweets.Value; }
        }

        public ReactiveCommand<ReactiveList<Tweet>> SearchForTweetsCommand { get; private set; }

        public string UrlPathSegment
        {
            get { return "usertweets"; }
        }

        public IScreen HostScreen { get; private set; }

        private static async Task<ReactiveList<Tweet>> SearchForTweets(string term1, string term2)
        {
            var auth = new SingleUserAuthorizer
                {
                    
                };

            await auth.AuthorizeAsync();

            var tweets = new Collection<Tweet>();
            PerformSearch(auth, term1, tweets);
            PerformSearch(auth, term2, tweets);

            return new ReactiveList<Tweet>(tweets.OrderByDescending(x => x.PublicationDate));
        }

        private static void PerformSearch(IAuthorizer authorizer, string term, ICollection<Tweet> tweets)
        {
            var result1 = (from search in new TwitterContext(authorizer).Search
                           where search.Type == SearchType.Search &&
                                 search.SearchLanguage == "ru" &&
                                 search.Query == term
                           select search).SingleOrDefault();

            if (result1 != null && result1.Statuses != null)
            {
                foreach (var tweet in result1.Statuses)
                {
                    tweets.Add(new Tweet
                        {
                            Text = tweet.Text,
                            ScreenName = tweet.User.Name,
                            UserName = !string.IsNullOrEmpty(tweet.ScreenName) ? "@" + tweet.ScreenName : null,
                            PublicationDate = GetElapsedPeriod(tweet.CreatedAt),
                            Image = tweet.User.ProfileImageUrl
                        });
                }
            }
        }

        // For reference http://www.dotnetperls.com/pretty-date"
        private static string GetElapsedPeriod(DateTime d)
        {
            var s = DateTime.Now.Subtract(d);
            var dayDiff = (int)s.TotalDays;
            var secDiff = (int)s.TotalSeconds;
            if (dayDiff == 0)
            {
                if (secDiff < 60)
                {
                    return "just now";
                }

                if (secDiff < 120)
                {
                    return "1 minute ago";
                }

                if (secDiff < 3600)
                {
                    return string.Format("{0} minutes ago", Math.Floor((double)secDiff / 60));
                }

                if (secDiff < 7200)
                {
                    return "1 hour ago";
                }

                if (secDiff < 86400)
                {
                    return string.Format("{0} hours ago", Math.Floor((double)secDiff / 3600));
                }
            }

            if (dayDiff == 1)
            {
                return "yesterday";
            }

            if (dayDiff < 7)
            {
                return string.Format("{0} days ago", dayDiff);
            }

            if (dayDiff < 31)
            {
                return string.Format("{0} weeks ago", Math.Ceiling((double)dayDiff / 7));
            }

            return d.ToString();
        } 
    }
}
