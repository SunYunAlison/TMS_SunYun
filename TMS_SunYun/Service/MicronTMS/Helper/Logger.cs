using log4net;
using log4net.Config;
using System.Configuration;
using System.Web;


namespace MicronTMS.Helper
{
    /// <summary>
    /// Handles functionalities related to logging
    /// </summary>
    public class Logger
    {
        #region private members

        private readonly ILog _log;
        private readonly string _logId = string.Empty;
        private readonly string _loggerName = "LOG";

        #endregion private members

        /// <summary>
        /// Initializes a new instance of the <see cref="Logger"/> class for custom logging.
        /// </summary>
        public Logger()
        {
            _log = LogManager.GetLogger(_loggerName);
        }

        /// <summary>
        /// Write the specified message to log file as a DEBUG level Log4Net message
        /// </summary>
        /// <param name="message">The message.</param>
        public void Debug(string message, string methodName = "", string eventId = "")
        {
            _log.Debug(string.Format("[{0}][{1}] {2}", eventId, methodName, message));
        }

        /// <summary>
        /// Write the specified message to log file as a INFO level Log4Net message
        /// </summary>
        /// <param name="message">The message.</param>
        public void Info(string message, string methodName = "", string eventId = "")
        {
            _log.Info(string.Format("[{0}][{1}] {2}", eventId, methodName, message));
        }

        /// <summary>
        /// Write the specified message to log file as a WARN level Log4Net message
        /// </summary>
        /// <param name="message">The message.</param>
        public void Warn(string message, string methodName = "", string eventId = "")
        {
            _log.Warn(string.Format("[{0}][{1}] {2}", eventId, methodName, message));
        }

        /// <summary>
        /// Write the specified message to log file as a ERROR level Log4Net message
        /// </summary>
        /// <param name="message">The message.</param>
        public void Error(string message, string methodName = "", string eventId = "")
        {
            _log.Error(string.Format("[{0}][{1}] {2}", eventId, methodName, message));
        }
    }
}