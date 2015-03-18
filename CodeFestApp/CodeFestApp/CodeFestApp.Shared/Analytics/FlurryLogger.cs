using System;

using ReactiveUI;

using Splat;

namespace CodeFestApp.Analytics
{
    public class FlurryLogger : IFullLogger
    {
        private readonly IFullLogger _inner;
        private readonly IAnalyticsLogger _analyticsLogger;

        public FlurryLogger(IFullLogger inner, IAnalyticsLogger analyticsLogger)
        {
            _inner = inner ?? new NullLogger();
            _analyticsLogger = analyticsLogger;
        }

        public LogLevel Level { get; set; }

        public void Write(string message, LogLevel logLevel)
        {
            if (logLevel == LogLevel.Info)
            {
                _analyticsLogger.LogEvent(message);
            }

            _inner.Write(message, logLevel);
        }

        public void Debug<T>(T value)
        {
            _inner.Debug(value);
        }

        public void Debug<T>(IFormatProvider formatProvider, T value)
        {
            _inner.Debug(formatProvider, value);
        }

        public void DebugException(string message, Exception exception)
        {
            _inner.DebugException(message, exception);
        }

        public void Debug(IFormatProvider formatProvider, string message, params object[] args)
        {
            _inner.Debug(formatProvider, message, args);
        }

        public void Debug(string message)
        {
            _inner.Debug(message);
        }

        public void Debug(string message, params object[] args)
        {
            _inner.Debug(message, args);
        }

        public void Debug<TArgument>(IFormatProvider formatProvider, string message, TArgument argument)
        {
            _inner.Debug(formatProvider, message, argument);
        }

        public void Debug<TArgument>(string message, TArgument argument)
        {
            _inner.Debug(message, argument);
        }

        public void Debug<TArgument1, TArgument2>(IFormatProvider formatProvider, string message, TArgument1 argument1, TArgument2 argument2)
        {
            _inner.Debug(formatProvider, message, argument1, argument2);
        }

        public void Debug<TArgument1, TArgument2>(string message, TArgument1 argument1, TArgument2 argument2)
        {
            _inner.Debug(message, argument1, argument2);
        }

        public void Debug<TArgument1, TArgument2, TArgument3>(IFormatProvider formatProvider, string message, TArgument1 argument1, TArgument2 argument2, TArgument3 argument3)
        {
            _inner.Debug(formatProvider, message, argument1, argument2, argument3);
        }

        public void Debug<TArgument1, TArgument2, TArgument3>(string message, TArgument1 argument1, TArgument2 argument2, TArgument3 argument3)
        {
            _inner.Debug(message, argument1, argument2, argument3);
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

            _inner.Info(value);
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

            _inner.Info(formatProvider, value);
        }

        public void InfoException(string message, Exception exception)
        {
            _analyticsLogger.LogException(exception);
            _inner.InfoException(message, exception);
        }

        public void Info(IFormatProvider formatProvider, string message, params object[] args)
        {
            _analyticsLogger.LogEvent(message);
            _inner.Info(formatProvider, message, args);
        }

        public void Info(string message)
        {
            _analyticsLogger.LogEvent(message);
            _inner.Info(message);
        }

        public void Info(string message, params object[] args)
        {
            _analyticsLogger.LogEvent(message);
            _inner.Info(message, args);
        }

        public void Info<TArgument>(IFormatProvider formatProvider, string message, TArgument argument)
        {
            _analyticsLogger.LogEvent(message);
            _inner.Info(formatProvider, message, argument);
        }

        public void Info<TArgument>(string message, TArgument argument)
        {
            _analyticsLogger.LogEvent(message);
            _inner.Info(message, argument);
        }

        public void Info<TArgument1, TArgument2>(IFormatProvider formatProvider, string message, TArgument1 argument1, TArgument2 argument2)
        {
            _analyticsLogger.LogEvent(message);
            _inner.Info(formatProvider, message, argument1, argument2);
        }

        public void Info<TArgument1, TArgument2>(string message, TArgument1 argument1, TArgument2 argument2)
        {
            _analyticsLogger.LogEvent(message);
            _inner.Info(message, argument1, argument2);
        }

        public void Info<TArgument1, TArgument2, TArgument3>(IFormatProvider formatProvider, string message, TArgument1 argument1, TArgument2 argument2, TArgument3 argument3)
        {
            _analyticsLogger.LogEvent(message);
            _inner.Info(formatProvider, message, argument1, argument2, argument3);
        }

        public void Info<TArgument1, TArgument2, TArgument3>(string message, TArgument1 argument1, TArgument2 argument2, TArgument3 argument3)
        {
            _analyticsLogger.LogEvent(message);
            _inner.Info(message, argument1, argument2, argument3);
        }

        public void Warn<T>(T value)
        {
            _inner.Warn(value);
        }

        public void Warn<T>(IFormatProvider formatProvider, T value)
        {
            _inner.Warn(formatProvider, value);
        }

        public void WarnException(string message, Exception exception)
        {
            _inner.WarnException(message, exception);
        }

        public void Warn(IFormatProvider formatProvider, string message, params object[] args)
        {
            _inner.Warn(formatProvider, message, args);
        }

        public void Warn(string message)
        {
            _inner.Warn(message);
        }

        public void Warn(string message, params object[] args)
        {
            _inner.Warn(message, args);
        }

        public void Warn<TArgument>(IFormatProvider formatProvider, string message, TArgument argument)
        {
            _inner.Warn(formatProvider, message, argument);
        }

        public void Warn<TArgument>(string message, TArgument argument)
        {
            _inner.Warn(message, argument);
        }

        public void Warn<TArgument1, TArgument2>(IFormatProvider formatProvider, string message, TArgument1 argument1, TArgument2 argument2)
        {
            _inner.Warn(formatProvider, message, argument1, argument2);
        }

        public void Warn<TArgument1, TArgument2>(string message, TArgument1 argument1, TArgument2 argument2)
        {
            _inner.Warn(message, argument1, argument2);
        }

        public void Warn<TArgument1, TArgument2, TArgument3>(IFormatProvider formatProvider, string message, TArgument1 argument1, TArgument2 argument2, TArgument3 argument3)
        {
            _inner.Warn(formatProvider, message, argument1, argument2, argument3);
        }

        public void Warn<TArgument1, TArgument2, TArgument3>(string message, TArgument1 argument1, TArgument2 argument2, TArgument3 argument3)
        {
            _inner.Warn(message, argument1, argument2, argument3);
        }

        public void Error<T>(T value)
        {
            _inner.Error(value);
        }

        public void Error<T>(IFormatProvider formatProvider, T value)
        {
            _inner.Error(formatProvider, value);
        }

        public void ErrorException(string message, Exception exception)
        {
            _inner.ErrorException(message, exception);
        }

        public void Error(IFormatProvider formatProvider, string message, params object[] args)
        {
            _inner.Error(formatProvider, message, args);
        }

        public void Error(string message)
        {
            _inner.Error(message);
        }

        public void Error(string message, params object[] args)
        {
            _inner.Error(message, args);
        }

        public void Error<TArgument>(IFormatProvider formatProvider, string message, TArgument argument)
        {
            _inner.Error(formatProvider, message, argument);
        }

        public void Error<TArgument>(string message, TArgument argument)
        {
            _inner.Error(message, argument);
        }

        public void Error<TArgument1, TArgument2>(IFormatProvider formatProvider, string message, TArgument1 argument1, TArgument2 argument2)
        {
            _inner.Error(formatProvider, message, argument1, argument2);
        }

        public void Error<TArgument1, TArgument2>(string message, TArgument1 argument1, TArgument2 argument2)
        {
            _inner.Error(message, argument1, argument2);
        }

        public void Error<TArgument1, TArgument2, TArgument3>(IFormatProvider formatProvider, string message, TArgument1 argument1, TArgument2 argument2, TArgument3 argument3)
        {
            _inner.Error(formatProvider, message, argument1, argument2, argument3);
        }

        public void Error<TArgument1, TArgument2, TArgument3>(string message, TArgument1 argument1, TArgument2 argument2, TArgument3 argument3)
        {
            _inner.Error(message, argument1, argument2, argument3);
        }

        public void Fatal<T>(T value)
        {
            _inner.Fatal(value);
        }

        public void Fatal<T>(IFormatProvider formatProvider, T value)
        {
            _inner.Fatal(formatProvider, value);
        }

        public void FatalException(string message, Exception exception)
        {
            _inner.FatalException(message, exception);
        }

        public void Fatal(IFormatProvider formatProvider, string message, params object[] args)
        {
            _inner.Fatal(formatProvider, message, args);
        }

        public void Fatal(string message)
        {
            _inner.Fatal(message);
        }

        public void Fatal(string message, params object[] args)
        {
            _inner.Fatal(message, args);
        }

        public void Fatal<TArgument>(IFormatProvider formatProvider, string message, TArgument argument)
        {
            _inner.Fatal(formatProvider, message, argument);
        }

        public void Fatal<TArgument>(string message, TArgument argument)
        {
            _inner.Fatal(message, argument);
        }

        public void Fatal<TArgument1, TArgument2>(IFormatProvider formatProvider, string message, TArgument1 argument1, TArgument2 argument2)
        {
            _inner.Fatal(formatProvider, message, argument1, argument2);
        }

        public void Fatal<TArgument1, TArgument2>(string message, TArgument1 argument1, TArgument2 argument2)
        {
            _inner.Fatal(message, argument1, argument2);
        }

        public void Fatal<TArgument1, TArgument2, TArgument3>(IFormatProvider formatProvider, string message, TArgument1 argument1, TArgument2 argument2, TArgument3 argument3)
        {
            _inner.Fatal(formatProvider, message, argument1, argument2, argument3);
        }

        public void Fatal<TArgument1, TArgument2, TArgument3>(string message, TArgument1 argument1, TArgument2 argument2, TArgument3 argument3)
        {
            _inner.Fatal(message, argument1, argument2, argument3);
        }

        private class NullLogger : IFullLogger
        {
            public LogLevel Level { get; set; }

            public void Write(string message, LogLevel logLevel)
            {
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
            }

            public void Info<T>(IFormatProvider formatProvider, T value)
            {
            }

            public void InfoException(string message, Exception exception)
            {
            }

            public void Info(IFormatProvider formatProvider, string message, params object[] args)
            {
            }

            public void Info(string message)
            {
            }

            public void Info(string message, params object[] args)
            {
            }

            public void Info<TArgument>(IFormatProvider formatProvider, string message, TArgument argument)
            {
            }

            public void Info<TArgument>(string message, TArgument argument)
            {
            }

            public void Info<TArgument1, TArgument2>(IFormatProvider formatProvider, string message, TArgument1 argument1, TArgument2 argument2)
            {
            }

            public void Info<TArgument1, TArgument2>(string message, TArgument1 argument1, TArgument2 argument2)
            {
            }

            public void Info<TArgument1, TArgument2, TArgument3>(IFormatProvider formatProvider, string message, TArgument1 argument1, TArgument2 argument2, TArgument3 argument3)
            {
            }

            public void Info<TArgument1, TArgument2, TArgument3>(string message, TArgument1 argument1, TArgument2 argument2, TArgument3 argument3)
            {
            }

            public void Warn<T>(T value)
            {
            }

            public void Warn<T>(IFormatProvider formatProvider, T value)
            {
            }

            public void WarnException(string message, Exception exception)
            {
            }

            public void Warn(IFormatProvider formatProvider, string message, params object[] args)
            {
            }

            public void Warn(string message)
            {
            }

            public void Warn(string message, params object[] args)
            {
            }

            public void Warn<TArgument>(IFormatProvider formatProvider, string message, TArgument argument)
            {
            }

            public void Warn<TArgument>(string message, TArgument argument)
            {
            }

            public void Warn<TArgument1, TArgument2>(IFormatProvider formatProvider, string message, TArgument1 argument1, TArgument2 argument2)
            {
            }

            public void Warn<TArgument1, TArgument2>(string message, TArgument1 argument1, TArgument2 argument2)
            {
            }

            public void Warn<TArgument1, TArgument2, TArgument3>(IFormatProvider formatProvider, string message, TArgument1 argument1, TArgument2 argument2, TArgument3 argument3)
            {
            }

            public void Warn<TArgument1, TArgument2, TArgument3>(string message, TArgument1 argument1, TArgument2 argument2, TArgument3 argument3)
            {
            }

            public void Error<T>(T value)
            {
            }

            public void Error<T>(IFormatProvider formatProvider, T value)
            {
            }

            public void ErrorException(string message, Exception exception)
            {
            }

            public void Error(IFormatProvider formatProvider, string message, params object[] args)
            {
            }

            public void Error(string message)
            {
            }

            public void Error(string message, params object[] args)
            {
            }

            public void Error<TArgument>(IFormatProvider formatProvider, string message, TArgument argument)
            {
            }

            public void Error<TArgument>(string message, TArgument argument)
            {
            }

            public void Error<TArgument1, TArgument2>(IFormatProvider formatProvider, string message, TArgument1 argument1, TArgument2 argument2)
            {
            }

            public void Error<TArgument1, TArgument2>(string message, TArgument1 argument1, TArgument2 argument2)
            {
            }

            public void Error<TArgument1, TArgument2, TArgument3>(IFormatProvider formatProvider, string message, TArgument1 argument1, TArgument2 argument2, TArgument3 argument3)
            {
            }

            public void Error<TArgument1, TArgument2, TArgument3>(string message, TArgument1 argument1, TArgument2 argument2, TArgument3 argument3)
            {
            }

            public void Fatal<T>(T value)
            {
            }

            public void Fatal<T>(IFormatProvider formatProvider, T value)
            {
            }

            public void FatalException(string message, Exception exception)
            {
            }

            public void Fatal(IFormatProvider formatProvider, string message, params object[] args)
            {
            }

            public void Fatal(string message)
            {
            }

            public void Fatal(string message, params object[] args)
            {
            }

            public void Fatal<TArgument>(IFormatProvider formatProvider, string message, TArgument argument)
            {
            }

            public void Fatal<TArgument>(string message, TArgument argument)
            {
            }

            public void Fatal<TArgument1, TArgument2>(IFormatProvider formatProvider, string message, TArgument1 argument1, TArgument2 argument2)
            {
            }

            public void Fatal<TArgument1, TArgument2>(string message, TArgument1 argument1, TArgument2 argument2)
            {
            }

            public void Fatal<TArgument1, TArgument2, TArgument3>(IFormatProvider formatProvider, string message, TArgument1 argument1, TArgument2 argument2, TArgument3 argument3)
            {
            }

            public void Fatal<TArgument1, TArgument2, TArgument3>(string message, TArgument1 argument1, TArgument2 argument2, TArgument3 argument3)
            {
            }
        }
    }
}
