using System;
using Realms;

namespace ServipagMobile.Droid {
	public class MisCuentas : RealmObject{
		public string aliasCuenta { get; set; }
		public string billerCuenta { get; set; }
		public int idBiller { get; set; }
		public int idServicio { get; set; }
		public string idCuenta { get; set; }
		public string imagenBiller { get; set; }
		public string imagenServicio { get; set; }
		public bool modificable { get; set; }
		public string nombreServicio { get; set; }
		public bool isSelected { get; set; }

		public MisCuentas() { }
		public MisCuentas(string aliasCuenta,
		                 string billerCuenta,
		                 int idBiller,
		                 int idServicio,
		                 string idCuenta,
		                 string imagenBiller,
		                 string imagenServicio,
		                 bool modificable,
	                	 string nombreServicio) {

			this.aliasCuenta = aliasCuenta;
			this.billerCuenta = billerCuenta;
			this.idBiller = idBiller;
			this.idServicio = idServicio;
			this.idCuenta = idCuenta;
			this.imagenBiller = imagenBiller;
			this.imagenServicio = imagenServicio;
			this.modificable = modificable;
			this.nombreServicio = nombreServicio;
			this.isSelected = true;
		}
	}
}
