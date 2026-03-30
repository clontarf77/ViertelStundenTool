namespace ViertelStdTool.Log
{
    public interface INLogger
    {
        #region write info line to log
        /// <summary>
        /// write line to log
        /// </summary>
        /// <param name="logEntry"></param>
        void WriteInfo(string logEntry);
        #endregion

        #region write error line to log
        /// <summary>
        /// write line to log
        /// </summary>
        /// <param name="message"></param>
        void WriteError(string message);
        #endregion
    }
}