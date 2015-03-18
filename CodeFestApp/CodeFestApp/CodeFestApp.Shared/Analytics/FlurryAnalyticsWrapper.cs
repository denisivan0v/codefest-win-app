using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Disposables;

using CodeFestApp.DataModel;

using FlurryWin8SDK;
using FlurryWin8SDK.Models;

using ReactiveUI;

namespace CodeFestApp.Analytics
{
    public class FlurryAnalyticsWrapper : IAnalyticsLogger
    {
        private const string LectureLike = "LectureLike";
        private const string LectureDislike = "LectureDislike";
        private const string PageView = "PageView";

        private readonly FlurryKey _key;

        public FlurryAnalyticsWrapper(FlurryKey key)
        {
            _key = key;
        }

        public void StartSession()
        {
            Api.StartSession(_key.ApiKey);
        }

        public void LogLectureLikeEvent(Lecture lecture)
        {
            LogLectureEvent(LectureLike, lecture);
        }

        public void LogLectureDislikeEvent(Lecture lecture)
        {
            LogLectureEvent(LectureDislike, lecture);
        }

        public IDisposable LogViewModelRouted(IRoutableViewModel viewModel)
        {
            Api.LogEvent(PageView, new List<Parameter> { new Parameter(viewModel.UrlPathSegment, string.Empty) });
            return Disposable.Empty;
        }

        public void LogEvent(string eventName)
        {
            Api.LogEvent(eventName);
        }

        public void LogException(Exception exception)
        {
            Api.LogError(exception.Message, exception);
        }

        private static void LogLectureEvent(string @event, Lecture lecture)
        {
            Api.LogEvent(@event,
                         new List<Parameter>
                             {
                                 new Parameter("id", lecture.Id.ToString()),
                                 new Parameter("title", lecture.Title),
                                 new Parameter("speaker", lecture.Speakers.First().Title)
                             });
        }
    }
}
