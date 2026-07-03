using Microsoft.Extensions.Logging;

namespace TK75Attractions.Monitoring
{
    public class Log
    {
        private readonly ILogger _logger;

        internal Log(ILogger logger)
        {
            _logger = logger;
        }

        public void Information(string message) => _logger.LogInformation(message);
        public void Error(string message) => _logger.LogError(message);
        public void Warning(string message) => _logger.LogWarning(message);
        public void Critical(string message) => _logger.LogCritical(message);
        public void Debug(string message)
        {
            _logger.LogDebug(message);
            UnityEngine.Debug.Log(message);
        }
    }
}