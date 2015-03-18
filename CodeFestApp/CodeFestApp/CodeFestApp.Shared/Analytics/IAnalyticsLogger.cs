using System;

using CodeFestApp.DataModel;

using ReactiveUI;

namespace CodeFestApp.Analytics
{
    public interface IAnalyticsLogger
    {
        void StartSession();

        void LogLectureLikeEvent(Lecture lecture);
        void LogLectureDislikeEvent(Lecture lecture);
        
        IDisposable LogViewModelRouted(IRoutableViewModel viewModel);
        
        void LogEvent(string eventName);
        void LogException(Exception exception);
    }
}