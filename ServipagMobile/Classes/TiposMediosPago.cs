using System;
namespace ServipagMobile {
	public class TiposMediosPago {
		public string descripcion;
		public string tipoVista;
		public string id;
		public string nombreTab;

		public TiposMediosPago(string descripcion, string tipoVista, string id, string nombreTab) {
			this.descripcion = descripcion;
			this.tipoVista = tipoVista;
			this.id = id;
			this.nombreTab = nombreTab;
		}
	}
}
