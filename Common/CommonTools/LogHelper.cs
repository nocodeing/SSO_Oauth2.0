using System;
using log4net;

namespace CommonTools
{
    public sealed class LogHelper
    {
        private static readonly ILog Loginfo = LogManager.GetLogger("loggerinfo");
        private static readonly ILog Logerror = LogManager.GetLogger("loggererror");
        private static readonly ILog Logmonitor = LogManager.GetLogger("loggermonitor");

        private static LogHelper _instance = new LogHelper();

        private LogHelper()
        {
            log4net.Config.XmlConfigurator.ConfigureAndWatch(
                new System.IO.FileInfo(AppDomain.CurrentDomain.BaseDirectory + "bin\\Config\\log4net.config"));
        }

        public static LogHelper Instance
        {
            get
            {
                if (null == _instance)
                {
                    _instance = new LogHelper();
                }
                return _instance;
            }
        }

        public void Error(string errorMsg, Exception ex = null)
        {
            if (ex != null)
            {
                Logerror.Error(errorMsg, ex);
            }
            else
            {
                Logerror.Error(errorMsg);
            }
        }

        public void Info(string msg)
        {
            if (Loginfo.IsInfoEnabled)
                Loginfo.Info(msg);
        }

        public void Monitor(string msg)
        {
            Logmonitor.Info(msg);
        }
    }
}