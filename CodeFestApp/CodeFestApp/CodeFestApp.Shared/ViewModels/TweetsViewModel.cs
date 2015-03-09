using System;
using System.Linq;
using System.Reactive.Disposables;
using System.Threading.Tasks;

using CodeFestApp.DataModel;

using LinqToTwitter;

using ReactiveUI;

namespace CodeFestApp.ViewModels
{
    public class TweetsViewModel : ReactiveObject, IRoutableViewModel
    {
        public TweetsViewModel(IScreen hostScreen)
        {
            HostScreen = hostScreen;
            this.WhenNavigatedTo(() =>
                {
                    var result = SearchForTweets("codefestru", 5);
                    result.Wait();

                    Tweets = result.Result;

                    return Disposable.Empty;
                });
        }

        public string Title
        {
            get { return "twitter лента"; }
        }

        public ReactiveList<Tweet> Tweets { get; private set; }

        public string UrlPathSegment
        {
            get { return "usertweets"; }
        }

        public IScreen HostScreen { get; private set; }

        private static async Task<ReactiveList<Tweet>> SearchForTweets(string term, int count)
        {
            var result = new ReactiveList<Tweet>();

            var auth = new SingleUserAuthorizer
            {
                
            };

            await auth.AuthorizeAsync();

            var twitterContext = new TwitterContext(auth);
            
            var searchResponse = await (from search in twitterContext.Search
                                        where search.Type == SearchType.Search &&
                                              search.Query == term &&
                                              search.Count == count
                                        select search).SingleOrDefaultAsync();
             
            if (searchResponse != null && searchResponse.Statuses != null)
            {
                foreach (var tweet in searchResponse.Statuses)
                {
                    result.Add(new Tweet
                        {
                            Text = tweet.Text,
                            ScreenName = tweet.User.Name,
                            UserName = "@" + tweet.ScreenName,
                            PublicationDate = GetElapsedPeriod(tweet.CreatedAt),
                            Image = tweet.User.ProfileImageUrl
                        });
                }
            }

            /*
            var tweets = (from status in twitterContext.Status
                          where status.ScreenName == term &&
                                status.Type == StatusType.User
                          select status)
                .Take(count)
                .ToArray();

            foreach (var tweet in tweets)
            {
                result.Add(new Tweet
                {
                    Text = tweet.Text,
                    ScreenName = tweet.User.Name,
                    UserName = "@" + tweet.ScreenName,
                    PublicationDate = GetElapsedPeriod(tweet.CreatedAt),
                    Image = tweet.User.ProfileImageUrl
                });
            }
             */

            return result;
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
