using System;
using Realms;

namespace ServipagMobile {
	public class SolicitaRecargaPE : RealmObject {
		public string acepta_abono { get; set; }
		public string acepta_pago_min { get; set; }
		public string boleta { get; set; }
		public string direccion_factura { get; set; }
		public string fecha_vencimiento { get; set; }
		public string id_biller { get; set; }
		public string id_servicio { get; set; }
		public string identificador { get; set; }
		public string id_periodo_solicitado { get; set; }
		public string id_pago_solicitado { get; set; }
		public int monto_minimo { get; set; } 
		public int monto_total { get; set; }
		public string nombreBiller { get; set; }
		public string texto_facturador { get; set; }
		public string rut { get; set; }
		public string email { get; set; }
		public bool isSelected { get; set; }
		public string logoEmpresa { get; set; }
	}
}
