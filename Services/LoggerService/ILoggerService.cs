using System;
using System.Collections.Generic;
using System.Text;

namespace Services.LoggerService
{
    public interface ILoggerService
    {
        public void Debug(string message);
        public void Info(string message);
        public void Warn(string message);
        public void Error(string message);

    }
}
