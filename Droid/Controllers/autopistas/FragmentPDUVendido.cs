using Android.Content;
using Android.OS;
using Android.Support.V4.App;
using Android.Views;
using Android.Widget;
using AndroidHUD;
using Newtonsoft.Json.Linq;

namespace ServipagMobile.Droid {
	public class FragmentPDUVendido : Fragment {
		private TextView pasesVendidos, idCuenta, fechaVenc;
		private Button bttnPagarPDU;

		private int cantPasesVendidos;
		private bool isPagoExpress;
		private PDUActivity PDUAct;

		public FragmentPDUVendido() { }

		public FragmentPDUVendido(int cantPasesVendidos, bool isPagoExpress, PDUActivity PDUAct) {
			this.cantPasesVendidos = cantPasesVendidos;
			this.isPagoExpress = isPagoExpress;
			this.PDUAct = PDUAct;
		}

		public override void OnCreate(Bundle savedInstanceState) {
			base.OnCreate(savedInstanceState);
		}

		public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState) {
			return inflater.Inflate(Resource.Layout.FragmentPDUVendido, container, false);
		}

		public override void OnViewCreated(View view, Bundle savedInstanceState) {
			base.OnViewCreated(view, savedInstanceState);

			pasesVendidos = view.FindViewById<TextView>(Resource.Id.pasesVendidos);
			idCuenta = view.FindViewById<TextView>(Resource.Id.idCuenta);
			fechaVenc = view.FindViewById<TextView>(Resource.Id.fechaVenc);
			bttnPagarPDU = view.FindViewById<Button>(Resource.Id.bttnPagarPDU);

			PDUAct.carroCompraLayout.Visibility = ViewStates.Gone;
			pasesVendidos.Text = cantPasesVendidos + " de 15";
			idCuenta.Text = PDUAct.pd.patente.ToUpper();
			fechaVenc.Text = PDUAct.pd.fecha_circulacion;

			bttnPagarPDU.Click += (sender, e) => {
				PDUAct.changeMainFragment(new FragmentListaDeudasPDU(PDUAct.pd, PDUAct, !isPagoExpress),
										   Resources.GetString(Resource.String.autopista_id_list_deudas_pdu));
			};
		}
	}
}
