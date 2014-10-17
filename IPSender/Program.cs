namespace IPSender
{
    using System.ServiceProcess;

    public static class Program
    {
        public static void Main()
        {
            var servicesToRun = new ServiceBase[] 
            { 
                new IpSenderService() 
            };

            ServiceBase.Run(servicesToRun);
        }
    }
}
