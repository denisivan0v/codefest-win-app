using System;

using ReactiveUI;

using Splat;

namespace CodeFestApp.Analytics
{
    public class FlurryLogger : IFullLogger
    {
        private readonly IAnalyticsLogger _analyticsLogger;

        public FlurryLogger(IAnalyticsLogger analyticsLogger)
        {
            _analyticsLogger = analyticsLogger;
            Level = LogLevel.Debug;
        }

        public LogLevel Level { get; set; }

        public void Write(string message, LogLevel logLevel)
        {
            if (logLevel == LogLevel.Info)
            {
                _analyticsLogger.LogEvent(message);
            }
        }

        public void Debug<T>(T value)
        {
        }

        public void Debug<T>(IFormatProvider formatProvider, T value)
        {
        }

        public void DebugException(string message, Exception exception)
        {
        }

        public void Debug(IFormatProvider formatProvider, string message, params object[] args)
        {
        }

        public void Debug(string message)
        {
        }

        public void Debug(string message, params object[] args)
        {
        }

        public void Debug<TArgument>(IFormatProvider formatProvider, string message, TArgument argument)
        {
        }

        public void Debug<TArgument>(string message, TArgument argument)
        {
        }

        public void Debug<TArgument1, TArgument2>(IFormatProvider formatProvider, string message, TArgument1 argument1, TArgument2 argument2)
        {
        }

        public void Debug<TArgument1, TArgument2>(string message, TArgument1 argument1, TArgument2 argument2)
        {
        }

        public void Debug<TArgument1, TArgument2, TArgument3>(IFormatProvider formatProvider, string message, TArgument1 argument1, TArgument2 argument2, TArgument3 argument3)
        {
        }

        public void Debug<TArgument1, TArgument2, TArgument3>(string message, TArgument1 argument1, TArgument2 argument2, TArgument3 argument3)
        {
        }

        public void Info<T>(T value)
        {
            var viewModel = value as IRoutableViewModel;
            if (viewModel != null)
            {
                _analyticsLogger.LogViewModelRouted(viewModel);
            }
            else if (!value.Equals(default(T)))
            {
                _analyticsLogger.LogEvent(value.ToString());
            }
        }

        public void Info<T>(IFormatProvider formatProvider, T value)
        {
            var viewModel = value as IRoutableViewModel;
            if (viewModel != null)
            {
                _analyticsLogger.LogViewModelRouted(viewModel);
            }
            else if (!value.Equals(default(T)))
            {
                _analyticsLogger.LogEvent(value.ToString());
            }
        }

        public void InfoException(string message, Exception exception)
        {
            _analyticsLogger.LogException(exception);
        }

        public void Info(IFormatProvider formatProvider, string message, params object[] args)
        {
            _analyticsLogger.LogEvent(message);
        }

        public void Info(string message)
        {
            _analyticsLogger.LogEvent(message);
        }

        public void Info(string message, params object[] args)
        {
            _analyticsLogger.LogEvent(message);
        }

        public void Info<TArgument>(IFormatProvider formatProvider, string message, TArgument argument)
        {
            _analyticsLogger.LogEvent(message);
        }

        public void Info<TArgument>(string message, TArgument argument)
        {
            _analyticsLogger.LogEvent(message);
        }

        public void Info<TArgument1, TArgument2>(IFormatProvider formatProvider, string message, TArgument1 argument1, TArgument2 argument2)
        {
            _analyticsLogger.LogEvent(message);
        }

        public void Info<TArgument1, TArgument2>(string message, TArgument1 argument1, TArgument2 argument2)
        {
            _analyticsLogger.LogEvent(message);
        }

        public void Info<TArgument1, TArgument2, TArgument3>(IFormatProvider formatProvider, string message, TArgument1 argument1, TArgument2 argument2, TArgument3 argument3)
        {
            _analyticsLogger.LogEvent(message);
        }

        public void Info<TArgument1, TArgument2, TArgument3>(string message, TArgument1 argument1, TArgument2 argument2, TArgument3 argument3)
        {
            _analyticsLogger.LogEvent(message);
        }

        public void Warn<T>(T value)
        {
            if (!value.Equals(default(T)))
            {
                _analyticsLogger.LogEvent(value.ToString());
            }

            _analyticsLogger.LogEvent(value.ToString());
        }

        public void Warn<T>(IFormatProvider formatProvider, T value)
        {
            if (!value.Equals(default(T)))
            {
                _analyticsLogger.LogEvent(value.ToString());
            }
        }

        public void WarnException(string message, Exception exception)
        {
            _analyticsLogger.LogException(exception);
        }

        public void Warn(IFormatProvider formatProvider, string message, params object[] args)
        {
            _analyticsLogger.LogEvent(message);
        }

        public void Warn(string message)
        {
            _analyticsLogger.LogEvent(message);
        }

        public void Warn(string message, params object[] args)
        {
            _analyticsLogger.LogEvent(message);
        }

        public void Warn<TArgument>(IFormatProvider formatProvider, string message, TArgument argument)
        {
            _analyticsLogger.LogEvent(message);
        }

        public void Warn<TArgument>(string message, TArgument argument)
        {
            _analyticsLogger.LogEvent(message);
        }

        public void Warn<TArgument1, TArgument2>(IFormatProvider formatProvider, string message, TArgument1 argument1, TArgument2 argument2)
        {
            _analyticsLogger.LogEvent(message);
        }

        public void Warn<TArgument1, TArgument2>(string message, TArgument1 argument1, TArgument2 argument2)
        {
            _analyticsLogger.LogEvent(message);
        }

        public void Warn<TArgument1, TArgument2, TArgument3>(IFormatProvider formatProvider, string message, TArgument1 argument1, TArgument2 argument2, TArgument3 argument3)
        {
            _analyticsLogger.LogEvent(message);
        }

        public void Warn<TArgument1, TArgument2, TArgument3>(string message, TArgument1 argument1, TArgument2 argument2, TArgument3 argument3)
        {
            _analyticsLogger.LogEvent(message);
        }

        public void Error<T>(T value)
        {
            if (!value.Equals(default(T)))
            {
                _analyticsLogger.LogEvent(value.ToString());
            }
        }

        public void Error<T>(IFormatProvider formatProvider, T value)
        {
            if (!value.Equals(default(T)))
            {
                _analyticsLogger.LogEvent(value.ToString());
            }
        }

        public void ErrorException(string message, Exception exception)
        {
            _analyticsLogger.LogException(exception);
        }

        public void Error(IFormatProvider formatProvider, string message, params object[] args)
        {
            _analyticsLogger.LogEvent(message);
        }

        public void Error(string message)
        {
            _analyticsLogger.LogEvent(message);
        }

        public void Error(string message, params object[] args)
        {
            _analyticsLogger.LogEvent(message);
        }

        public void Error<TArgument>(IFormatProvider formatProvider, string message, TArgument argument)
        {
            _analyticsLogger.LogEvent(message);
        }

        public void Error<TArgument>(string message, TArgument argument)
        {
            _analyticsLogger.LogEvent(message);
        }

        public void Error<TArgument1, TArgument2>(IFormatProvider formatProvider, string message, TArgument1 argument1, TArgument2 argument2)
        {
            _analyticsLogger.LogEvent(message);
        }

        public void Error<TArgument1, TArgument2>(string message, TArgument1 argument1, TArgument2 argument2)
        {
            _analyticsLogger.LogEvent(message);
        }

        public void Error<TArgument1, TArgument2, TArgument3>(IFormatProvider formatProvider, string message, TArgument1 argument1, TArgument2 argument2, TArgument3 argument3)
        {
            _analyticsLogger.LogEvent(message);
        }

        public void Error<TArgument1, TArgument2, TArgument3>(string message, TArgument1 argument1, TArgument2 argument2, TArgument3 argument3)
        {
            _analyticsLogger.LogEvent(message);
        }

        public void Fatal<T>(T value)
        {
            if (!value.Equals(default(T)))
            {
                _analyticsLogger.LogEvent(value.ToString());
            }
        }

        public void Fatal<T>(IFormatProvider formatProvider, T value)
        {
            if (!value.Equals(default(T)))
            {
                _analyticsLogger.LogEvent(value.ToString());
            }
        }

        public void FatalException(string message, Exception exception)
        {
            _analyticsLogger.LogException(exception);
        }

        public void Fatal(IFormatProvider formatProvider, string message, params object[] args)
        {
            _analyticsLogger.LogEvent(message);
        }

        public void Fatal(string message)
        {
            _analyticsLogger.LogEvent(message);
        }

        public void Fatal(string message, params object[] args)
        {
            _analyticsLogger.LogEvent(message);
        }

        public void Fatal<TArgument>(IFormatProvider formatProvider, string message, TArgument argument)
        {
            _analyticsLogger.LogEvent(message);
        }

        public void Fatal<TArgument>(string message, TArgument argument)
        {
            _analyticsLogger.LogEvent(message);
        }

        public void Fatal<TArgument1, TArgument2>(IFormatProvider formatProvider, string message, TArgument1 argument1, TArgument2 argument2)
        {
            _analyticsLogger.LogEvent(message);
        }

        public void Fatal<TArgument1, TArgument2>(string message, TArgument1 argument1, TArgument2 argument2)
        {
            _analyticsLogger.LogEvent(message);
        }

        public void Fatal<TArgument1, TArgument2, TArgument3>(IFormatProvider formatProvider, string message, TArgument1 argument1, TArgument2 argument2, TArgument3 argument3)
        {
            _analyticsLogger.LogEvent(message);
        }

        public void Fatal<TArgument1, TArgument2, TArgument3>(string message, TArgument1 argument1, TArgument2 argument2, TArgument3 argument3)
        {
            _analyticsLogger.LogEvent(message);
        }
    }
}
