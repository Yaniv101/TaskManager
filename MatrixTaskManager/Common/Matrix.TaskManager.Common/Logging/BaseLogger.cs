//using System;

//namespace Matrix.TaskManager.Common.Logging
//{
//    public abstract class BaseLogger : ILogger
//    {
//        #region ILogger Interface Methods (Abstract)

//        /// <summary>
//        /// Traces the specified message.
//        /// </summary>
//        /// <param name="message">The message.</param>
//        public abstract void Trace(string message);

//        /// <summary>
//        /// Traces the specified message.
//        /// </summary>
//        /// <param name="message">The message.</param>
//        /// <param name="exception">The exception.</param>
//        public abstract void Trace(string message, Exception exception);

//        public abstract void Info(string message);

//        public abstract void Debug(string message);

//        public abstract void Debug(string message, Exception exception);

//        public abstract void Warn(string message);

//        public abstract void Warn(string message, Exception exception);

//        public abstract void Error(string message);

//        public abstract void Error(string message, Exception exception);

//        public abstract void Critical(string message);

//        public abstract void Critical(string message, Exception exception);

//        #endregion

//        #region Public Virtual Functions

//        public virtual void Log(string message, LogLevel logLevel)
//        {
//            switch (logLevel)
//            {
//                case LogLevel.Trace:
//                case LogLevel.Debug:
//                    Debug(message);
//                    break;
//                case LogLevel.Info:
//                    Info(message);
//                    break;
//                case LogLevel.Warning:
//                    Warn(message);
//                    break;
//                case LogLevel.Error:
//                    Error(message);
//                    break;
//                case LogLevel.Critical:
//                case LogLevel.Fatal:
//                    Critical(message);
//                    break;
//            }
//        }

//        public virtual void Log(string message, Exception exception, LogLevel logLevel)
//        {
//            switch (logLevel)
//            {
//                case LogLevel.Trace:
//                case LogLevel.Debug:
//                    Debug(message, exception);
//                    break;
//                case LogLevel.Info:
//                    Info(message);
//                    break;
//                case LogLevel.Warning:
//                    Warn(message, exception);
//                    break;
//                case LogLevel.Error:
//                    Error(message, exception);
//                    break;
//                case LogLevel.Critical:
//                case LogLevel.Fatal:
//                    Critical(message, exception);
//                    break;
//                default:
//                    Debug(message, exception);
//                    break;
//            }
//        }

//        #endregion

//    }
//}