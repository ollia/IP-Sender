namespace IPSender
{
    using System;
    using System.Configuration;
    using System.Linq;
    using System.Net;
    using System.Net.NetworkInformation;
    using System.ServiceProcess;
    using System.Text;

    public partial class IpSenderService : ServiceBase
    {       
        private static string observableNetworkInterfaceName;

        private static bool createRdcFile;

        public IpSenderService()
        {
            this.InitializeComponent();
        }

        private static Func<NetworkInterface, bool> ObservableNetworkInterface
        {
            get { return a => a.Name.Equals(observableNetworkInterfaceName, StringComparison.OrdinalIgnoreCase); }
        }

        protected override void OnStart(string[] args)
        {
            TraceLogger.Write("Starting IP-Sender");

            observableNetworkInterfaceName = ConfigurationManager.AppSettings.Get("ObservableNetworkInterface");
            bool.TryParse(ConfigurationManager.AppSettings.Get("CreateRdcFile"), out createRdcFile);

            NetworkChange.NetworkAddressChanged += AddressChangedCallback;
        }

        protected override void OnStop()
        {
            TraceLogger.Write("Stopping IP-Sender");
        }

        private static void AddressChangedCallback(object sender, EventArgs e)
        {
            try
            {
                var sb = new StringBuilder();
                var adapters = NetworkInterface.GetAllNetworkInterfaces();
                byte[] rdcFile = null;

                if (adapters.Any(ObservableNetworkInterface))
                {
                    var networkInterface = adapters.First(ObservableNetworkInterface);
                    var ip4Address = AppendNetworkInterfaceData(sb, networkInterface);

                    if (createRdcFile && ip4Address != null)
                    {
                        rdcFile = RdcGenerator.GenerateRdcConnectionFile(ip4Address);
                    }
                }
                else
                {
                    foreach (NetworkInterface n in adapters)
                    {
                        AppendNetworkInterfaceData(sb, n);
                    }
                }

                MailSender.SendMail(sb.ToString(), rdcFile);                
            }
            catch (Exception ex)
            {
                TraceLogger.Write("Address changed callback exception: " + ex);
            }
        }

        private static IPAddress AppendNetworkInterfaceData(StringBuilder sb, NetworkInterface n)
        {
            IPAddress ip4Address = null;
            sb.Append(n.Name + " : ");

            foreach (UnicastIPAddressInformation ip in n.GetIPProperties().UnicastAddresses)
            {
                if (ip.Address.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
                {
                    sb.Append(ip.Address);
                    sb.Append(Environment.NewLine);
                    ip4Address = ip.Address;
                }
            }

            return ip4Address;
        }
    }
}
