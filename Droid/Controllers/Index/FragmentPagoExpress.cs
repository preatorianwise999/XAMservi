using Android.OS;
using Android.Support.V4.App;
using Android.Views;
using Android.Widget;
using Android.Content;
using Newtonsoft.Json;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using System.Linq;
using AndroidHUD;
using Android.Util;

namespace ServipagMobile.Droid {
	public class FragmentPagoExpress : Fragment {
		private MainActivity ma;
		private ImageView bttnPagoExpress;
		private Button bttnSignIn;
		private RelativeLayout containerDown;
		private RelativeLayout containerRecargas;
		private RelativeLayout containerAutopistas;
		private ImageView imageRecargas;
		private ImageView imageAutopistas;
		private UtilsAndroid utilsAndroid = new UtilsAndroid();

		public FragmentPagoExpress() {}

		public override void OnCreate(Bundle savedInstanceState) {
			base.OnCreate(savedInstanceState);

			this.ma = (MainActivity)Activity;
		}

		public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState) {
			return inflater.Inflate(Resource.Layout.FragmentPagoExpress, container, false);
		}

		public override void OnViewCreated(View view, Bundle savedInstanceState) {
			base.OnViewCreated(view, savedInstanceState);

			bttnPagoExpress = view.FindViewById<ImageView>(Resource.Id.bttnPagoExpress);
			bttnSignIn = view.FindViewById<Button>(Resource.Id.registrarte);
			imageRecargas = view.FindViewById<ImageView>(Resource.Id.imageRecargas);
			imageAutopistas = view.FindViewById<ImageView>(Resource.Id.imageAutopistas);

			bttnPagoExpress.Click += (sender, e) => {
				Intent intent = new Intent(ma, typeof(AgregarActivity));
				intent.PutExtra("isPagoExpress", true);
				intent.PutExtra("autopista", false);
				intent.PutExtra("listAdd", JsonConvert.SerializeObject(ma.listAdd));
				ma.StartActivityForResult(intent, 1);
			};

			bttnSignIn.Click += delegate {
				Intent intent = new Intent(ma, typeof(RegistroActivity));
				ma.StartActivityForResult(intent, 0);
			};

			imageRecargas.Click += (sender, e) => {
				ma.carroCompraLayout.Visibility = ViewStates.Visible;
				ma.sortMyAccounts.Visibility = ViewStates.Gone;
				ma.addAccount.Visibility = ViewStates.Gone;
				ma.idFragment = "recargas";

				if (RealmDB.GetInstance().realm.All<SolicitaRecargaPE>().Count() == 0) {
					ma.changeMainFragment(new FragmentUltimasRecargas(ma.listURPE, ma.isLogin, ma.badgeCount.Text, ma), ma.idFragment);
				} else {
					ma.listURPE.Clear();
					foreach (SolicitaRecargaPE srpe in RealmDB.GetInstance().realm.All<SolicitaRecargaPE>()) {
						var sr = new SolicitaRecarga();
						sr.id_periodo_solicitado = srpe.id_periodo_solicitado;
						sr.id_pago_solicitado = srpe.id_pago_solicitado;
						sr.nombreBiller = srpe.nombreBiller;
						sr.acepta_abono = srpe.acepta_abono;
						sr.acepta_pago_min = srpe.acepta_pago_min;
						sr.boleta = srpe.boleta;
						sr.direccion_factura = srpe.direccion_factura;
						sr.fecha_vencimiento = srpe.fecha_vencimiento;
						sr.id_biller = srpe.id_biller;
						sr.id_servicio = srpe.id_servicio;
						sr.identificador = srpe.identificador;
						sr.monto_minimo = srpe.monto_minimo;
						sr.monto_total = srpe.monto_total;
						sr.texto_facturador = srpe.texto_facturador;
						sr.rut = srpe.rut;
						sr.email = srpe.email;
						sr.isSelected = false;
						sr.logoEmpresa = srpe.logoEmpresa;
						ma.listURPE.Add(sr);
					}
					ma.listURPE[0].isSelected = true;
					ma.changeMainFragment(new FragmentUltimasRecargas(ma.listURPE, ma.isLogin, ma.badgeCount.Text, ma), ma.idFragment);
				}
			};

			imageAutopistas.Click += (sender, e) => {
				Intent intent = new Intent(ma, typeof(AgregarActivity));
				intent.PutExtra("isPagoExpress", true);
				intent.PutExtra("autopista", true);
				intent.PutExtra("listAdd", JsonConvert.SerializeObject(ma.listAdd));
				ma.StartActivityForResult(intent, 1);
			};
		}
	}
}
