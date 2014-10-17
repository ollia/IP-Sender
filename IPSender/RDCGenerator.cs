namespace IPSender
{    
    using System.Configuration;
    using System.Net;
    using System.Text;

    public class RdcGenerator
    {
        private static bool useAllMonitors;

        private static string userName;

        private static bool useLocalDevicesPrinters;

        private static bool useLocalDevicesClipboard;

        private static bool useLocalDevicesDrives;
        
        public static byte[] GenerateRdcConnectionFile(IPAddress ipAddress)
        {
            InitializeSettings();

            var sb = new StringBuilder();
            sb.AppendLine(RdcBaseContent);

            sb.AppendLine("full address:s:" + ipAddress);
            sb.AppendLine(useAllMonitors ? "use multimon:i:1" : "use multimon:i:0");
            sb.AppendLine("username:s:" + userName);
            sb.AppendLine(useLocalDevicesPrinters ? "redirectprinters:i:1" : "redirectprinters:i:0");
            sb.AppendLine(useLocalDevicesClipboard ? "redirectclipboard:i:1" : "redirectclipboard:i:0");

            // including all drives or no drives for now
            sb.AppendLine(useLocalDevicesDrives ? "drivestoredirect:s:*" : "drivestoredirect:s:");

            return Encoding.ASCII.GetBytes(sb.ToString());
        }        

        private static void InitializeSettings()
        {
            bool.TryParse(ConfigurationManager.AppSettings.Get("UseAllMonitors"), out useAllMonitors);
            userName = ConfigurationManager.AppSettings.Get("RdcUserName") ?? string.Empty;
            bool.TryParse(ConfigurationManager.AppSettings.Get("UseLocalDevicesPrinters"), out useLocalDevicesPrinters);
            bool.TryParse(ConfigurationManager.AppSettings.Get("UseLocalDevicesClipboard"), out useLocalDevicesClipboard);
            bool.TryParse(ConfigurationManager.AppSettings.Get("UseLocalDevicesDrives"), out useLocalDevicesDrives);
        }

        private const string RdcBaseContent = @"screen mode id:i:2
desktopwidth:i:800
desktopheight:i:600
session bpp:i:32
winposstr:s:0,3,0,0,800,600
compression:i:1
keyboardhook:i:2
audiocapturemode:i:0
videoplaybackmode:i:1
connection type:i:7
networkautodetect:i:1
bandwidthautodetect:i:1
displayconnectionbar:i:1
enableworkspacereconnect:i:0
disable wallpaper:i:0
allow font smoothing:i:0
allow desktop composition:i:0
disable full window drag:i:1
disable menu anims:i:1
disable themes:i:0
disable cursor setting:i:0
bitmapcachepersistenable:i:1
audiomode:i:0
redirectcomports:i:0
redirectsmartcards:i:1
redirectposdevices:i:0
autoreconnection enabled:i:1
authentication level:i:2
prompt for credentials:i:0
negotiate security layer:i:1
remoteapplicationmode:i:0
alternate shell:s:
shell working directory:s:
gatewayhostname:s:
gatewayusagemethod:i:4
gatewaycredentialssource:i:4
gatewayprofileusagemethod:i:0
promptcredentialonce:i:0
gatewaybrokeringtype:i:0
use redirection server name:i:0
rdgiskdcproxy:i:0
kdcproxyname:s:";
    }
}
