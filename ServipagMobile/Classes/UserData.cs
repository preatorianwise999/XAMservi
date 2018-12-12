using System;
namespace ServipagMobile {
	public class UserData {
		public string nombre { get; set; }
		public string rut { get; set; }
		public string rutShow { get; set; } 
		public string region { get; set; } 
		public string comuna { get; set; }
		public string correo { get; set; }
		public string cumpleanos { get; set; }
		public string cookie { get; set; }

		private static UserData instance;

		private UserData() { }

		private UserData(string nombre, string rut, string rutShow, string region, string comuna, string correo, string cumpleanos, string cookie) {
			this.nombre = nombre;
			this.rut = rut;
			this.rutShow = rutShow;
			this.region = region;
			this.comuna = comuna;
			this.correo = correo;
			this.cumpleanos = cumpleanos;
			this.cookie = cookie;
		}

		public static UserData GetInstance() {
			if (instance == null) {
				instance = new UserData();
			}
			return instance;
		}
		//Metodo estático "sobrecargado" que devuelve una única instancia de "Singleton" ...
		public static UserData GetInstance(string nombre, string rut, string rutShow, string region, string comuna, string correo, string cumpleanos, string cookie) {
			if (instance == null) {
				instance = new UserData(nombre, rut, rutShow, region, comuna, correo, cumpleanos, cookie);
			}
			return instance;
		}
	}
}
