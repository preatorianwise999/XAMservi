using System;
using Realms;

namespace ServipagMobile {
	public class PaseDiario : RealmObject {
		public string nombre_fantasia { get; set; }
		public string fecha_vencimiento { get; set; }
		public string identificador { get; set; }
		public int monto_total { get; set; }
		public bool isPDU { get; set; }
		public string tipoPDU { get; set; }
		public string patente { get; set; }
		public int categoria { get; set; }
		public string fecha_circulacion { get; set; }
		public bool isSelected { get; set; }
		public string idBiller { get; set; }
		public string idServicio { get; set; }
	}
}
