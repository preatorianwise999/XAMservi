using Realms;

namespace ServipagMobile {
	public class PersistentData: RealmObject {
		public int sortType { get; set; }
		public int sortTypePE { get; set; }
		public string idTransaccion { get; set; }
		public string cookie { get; set; }
		public string descripcion { get; set; }
		public int forma_pago { get; set; }
		public int id_banco { get; set; }
		public string logo_banco { get; set; }
		public string orden { get; set; }
		public string url_banco { get; set; }
		public string valor_parametro_banco { get; set; }
		public string valor_popup { get; set; }
		public bool acepta_tc_pdu { get; set; }
	}
}
