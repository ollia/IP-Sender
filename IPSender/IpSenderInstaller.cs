namespace IPSender
{
    using System.ComponentModel;

    [RunInstaller(true)]
    public partial class IpSenderInstaller : System.Configuration.Install.Installer
    {
        public IpSenderInstaller()
        {
            this.InitializeComponent();
        }
    }
}
