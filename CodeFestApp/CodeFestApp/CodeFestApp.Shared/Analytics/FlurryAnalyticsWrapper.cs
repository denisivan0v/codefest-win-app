using System;
using System.Collections.Generic;
using System.Reactive.Disposables;

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

        public void LogLectureLikeEvent()
        {
            Api.LogEvent(LectureLike);
        }

        public void LogLectureDislikeEvent()
        {
            Api.LogEvent(LectureDislike);
        }

        public IDisposable LogViewModelRouted(IRoutableViewModel viewModel)
        {
            Api.LogEvent(PageView, new List<Parameter> { new Parameter(viewModel.UrlPathSegment, string.Empty) });
            return Disposable.Empty;
        }

        public void LogException(Exception exception)
        {
            Api.LogError(exception.Message, exception);
        }
    }
}
