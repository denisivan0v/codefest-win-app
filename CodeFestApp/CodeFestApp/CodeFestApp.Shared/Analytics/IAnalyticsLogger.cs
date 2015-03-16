using System;

using ReactiveUI;

namespace CodeFestApp.Analytics
{
    public interface IAnalyticsLogger
    {
        void StartSession();
        void LogLectureLikeEvent();
        void LogLectureDislikeEvent();
        IDisposable LogViewModelRouted(IRoutableViewModel viewModel);
        void LogException(Exception exception);
    }
}