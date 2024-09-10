using HouseInventory.Services.Interfaces;
using NLog;
using System.Diagnostics.CodeAnalysis;
using ILogger = NLog.ILogger;

namespace HouseInventory.Services
{
    [ExcludeFromCodeCoverage]
    public class LoggerManager : ILoggerManager
    {
        private static ILogger _logger = LogManager.GetCurrentClassLogger();

        public LoggerManager() { }

        /// <summary>
        /// Writes the diagnostic message at the Debug level.
        /// </summary>
        /// <param name="message">The message we want to log out.</param>
        public void LogDebug(string message)
        {
            _logger.Debug(message);
        }

        /// <summary>
        /// Writes the diagnostic message at the Error level.
        /// </summary>
        /// <param name="message">The message we want to log out.</param>
        public void LogError(string message)
        {
            _logger.Error(message);
        }

        /// <summary>
        /// Writes the diagnostic message at the Info level.
        /// </summary>
        /// <param name="message">The message we want to log out.</param>
        public void LogInfo(string message)
        {
            _logger.Info(message);
        }

        /// <summary>
        /// Writes the diagnostic message at the Warn level.
        /// </summary>
        /// <param name="message">The message we want to log out.</param>
        public void LogWarn(string message)
        {
            _logger.Warn(message);
        }
    }
}
