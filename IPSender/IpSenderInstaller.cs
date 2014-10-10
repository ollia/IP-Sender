using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration.Install;
using System.Linq;
using System.Threading.Tasks;

namespace IPSender
{
    [RunInstaller(true)]
    public partial class IpSenderInstaller : System.Configuration.Install.Installer
    {
        public IpSenderInstaller()
        {
            InitializeComponent();
        }
    }
}
