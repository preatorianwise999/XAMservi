using System.Net;
using Android.Content;
using Android.Graphics;
using Android.Telephony;
using Android.Util;
using com.refractored.monodroidtoolkit;
using Java.IO;
using Java.Net;

namespace ServipagMobile.Droid {
	public class UtilsAndroid {
		public UtilsAndroid() {
		}  

		public string getDeviceType(Android.Support.V7.App.AppCompatActivity context) {
			var manager = context.GetSystemService(Context.TelephonyService) as TelephonyManager;

			if (manager == null) {
				return "unknow";
			} else if (manager.PhoneType == PhoneType.None) {
				return "Tablet";
			} else {
				return "Mobile";
			}
		}

		public string getChannel(string deviceType, string devicePlatform) {
			switch (deviceType+devicePlatform) {
				case "AndroidMobile":
					return "99";
				break;
				case "AndroidTablet":
					return "99";
				break;
				case "iOSMobile":
					return "99";
				break;
				case "iOSTablet":
					return "99";
				break;
				default:
					return "99";
				break;
			}
		}

		public string getIpAddress() {
			IPAddress[] addresses = Dns.GetHostAddresses(Dns.GetHostName());

			if (addresses != null && addresses[0] != null) {
				return addresses[0].ToString();
			} else {
				return null;
			}
		}
	}
}
