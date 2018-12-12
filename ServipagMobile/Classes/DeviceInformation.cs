using System;
namespace ServipagMobile {
	public class DeviceInformation {

		public string channel { get; set;}
		public string deviceType { get; set; }
		public string version { get; set; }
		public string client { get; set; }

		private static DeviceInformation instance;

		private DeviceInformation() { }

		private DeviceInformation(string channel, string deviceType, string client, string version) {
			this.channel = channel;
			this.deviceType = deviceType;
			this.client = client;
			this.version = version;
		}

		public static DeviceInformation GetInstance() {
			if (instance == null) {
				instance = new DeviceInformation();
			}
			return instance;
		}
		//Metodo estático "sobrecargado" que devuelve una única instancia de "Singleton" ...
		public static DeviceInformation GetInstance(string channel, string deviceType, string client, string version) {
			if (instance == null) {
				instance = new DeviceInformation(channel, deviceType, client, version);
			}
			return instance;
		}
	}
}
