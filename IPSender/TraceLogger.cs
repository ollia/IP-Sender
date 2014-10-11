namespace IPSender
{
    using System;
    using System.Configuration;
    using System.Diagnostics;

    public static class TraceLogger
    {   
        public static void Write(string message)
        {
            bool loggingEnabled;

            if (!bool.TryParse(ConfigurationManager.AppSettings.Get("LoggingEnabled"), out loggingEnabled))
            {
                Trace.Write(DateTime.Now.ToString("s") + " : " + "LoggingEnabled parameter not found in application configuration file");
                Trace.Write(DateTime.Now.ToString("s") + " : " + message);
                Trace.Write(Environment.NewLine);
            }
            
            if(loggingEnabled)
            {
                Trace.Write(DateTime.Now.ToString("s") + " : " + message);
                Trace.Write(Environment.NewLine);
            }
            
        }
    }
}
