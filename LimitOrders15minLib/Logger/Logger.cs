using NLog;
using NLog.Config;
using NLog.Targets;
using System;

namespace ViertelStdTool.Log
{
    public class NLogger : INLogger
    {
        // Log should be placed here.
        private readonly string fileName = AppDomain.CurrentDomain.BaseDirectory + "logfile.log";
        private readonly Logger logger;

        #region constructor
        public NLogger()
        {
            // Step 1. Create configuration object 
            var config = new LoggingConfiguration();

            var fileTarget = new FileTarget("target1")
            {
                FileName = this.fileName,
                Layout = "${longdate} ${level} ${message}  ${exception}"
            };
            config.AddTarget(fileTarget);

            // Step 3. Define rules           
            config.AddRuleForAllLevels(fileTarget); // all to file

            // Step 4. Activate the configuration
            LogManager.Configuration = config;

            logger = LogManager.GetLogger("Autoruns");
        }
        #endregion

        #region write line to log
        /// <summary>
        /// write line to log
        /// </summary>
        /// <param name="message"></param>
        public void WriteInfo(string message)
        {
            logger.Info(message);
        }
        #endregion

        #region write error line to log
        /// <summary>
        /// write line to log
        /// </summary>
        /// <param name="message"></param>
        public void WriteError(string message)
        {
            logger.Error(message);
        }
        #endregion
    }
}
