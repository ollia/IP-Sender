namespace IPSender
{
    using System.Diagnostics;
    using System.Net.NetworkInformation;
    using System;
    using System.ServiceProcess;
    using System.Text;

    public partial class IpSenderService : ServiceBase
    {
        public IpSenderService()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            Trace.Write(DateTime.Now.ToString("s") + ":" + "Starting IP-Sender");
            Trace.Write(Environment.NewLine);
            NetworkChange.NetworkAddressChanged += AddressChangedCallback;
        }

        protected override void OnStop()
        {
            this.Dispose();
        }

        static void AddressChangedCallback(object sender, EventArgs e)
        {
            try
            {
                var sb = new StringBuilder();

                NetworkInterface[] adapters = NetworkInterface.GetAllNetworkInterfaces();
                foreach (NetworkInterface n in adapters)
                {
                    sb.Append(n.Name + " : ");
                    sb.Append(n.OperationalStatus + " : ");   

                    foreach (UnicastIPAddressInformation ip in n.GetIPProperties().UnicastAddresses)
                    {
                        if (ip.Address.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
                        {
                            sb.Append(ip.Address);
                            sb.Append(Environment.NewLine);
                        }
                    }
                }

                var mailSender = new MailSender();
                mailSender.SendMail(sb.ToString());
            }
            catch (Exception ex)
            {
                Trace.Write(DateTime.Now.ToString("s") + ":" + "Address changed callback exception: " + ex);
                Trace.Write(Environment.NewLine);
            }
        }
    }
}
