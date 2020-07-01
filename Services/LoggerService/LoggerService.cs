using NLog;
using NLog.Fluent;
using System;
using System.Collections.Generic;
using System.Text;

namespace Services.LoggerService
{
    public class LoggerService : ILoggerService
    {
        private readonly ILogger logger = LogManager.GetCurrentClassLogger();
        public void Debug(string message)
        {
            logger.Debug(message);
        }

        public void Error(string message)
        {
            logger.Error(message);
        }

        public void Info(string message)
        {
            logger.Info(message);
        }

        public void Warn(string message)
        {
            logger.Warn(message);
        }
    }
}
