namespace WebApi.Factory
{
    public static class LoggerFactory
    {
        public static NLog.Logger GetLoggerInstance(string directory = null)
        {
            return NLog.LogManager.GetCurrentClassLogger();
        }
    }
}