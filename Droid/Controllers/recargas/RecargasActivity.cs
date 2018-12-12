using System.Collections.Generic;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Support.V7.App;
using Android.Views;
using Android.Widget;
using Newtonsoft.Json;

namespace ServipagMobile.Droid {
	[Activity(Label = "RecargasActivity")]
	public class RecargasActivity : AppCompatActivity {
		private Android.Support.V7.Widget.Toolbar toolbar;
		private RelativeLayout carroCompraLayout;
		private ImageButton carroCompra;
		private TextView badgeText;
		private string idFragment;
		private List<ServiciosRecarga> listSRMovil;
		private List<ServiciosRecarga> listSRFijo;
		private List<MontoRecarga> listMR;
		private bool isLogin;
		private string badgeCount;

		private Recargas datosRecarga;
		private SolicitaRecarga datosRecargaPE;
		private bool isUR;

		public ServiciosRecarga servicioRecarga;
		public List<MontoRecarga> listMontoRecarga;
		public SolicitaRecarga solicitaRecarga;
		public List<MediosPago> listMediosPago;

		protected override void OnCreate(Bundle savedInstanceState) {
			base.OnCreate(savedInstanceState);
			SetContentView(Resource.Layout.Recargas);
			isLogin = Intent.GetBooleanExtra("isLogin", false);
			badgeCount = Intent.GetStringExtra("badgeCount");
			isUR = Intent.GetBooleanExtra("isUR", false);

			if (isUR) {
				if (isLogin) {
					datosRecarga = JsonConvert.DeserializeObject<Recargas>(Intent.GetStringExtra("datosRecarga"));
				} else {
					datosRecargaPE = JsonConvert.DeserializeObject<SolicitaRecarga>(Intent.GetStringExtra("datosRecarga"));
				}

				listMR = JsonConvert.DeserializeObject<List<MontoRecarga>>(Intent.GetStringExtra("listMontoRecarga"));
			}

			listSRMovil = JsonConvert.DeserializeObject<List<ServiciosRecarga>>(Intent.GetStringExtra("listSRMovil"));
			listSRFijo = JsonConvert.DeserializeObject<List<ServiciosRecarga>>(Intent.GetStringExtra("listSRFijo"));

			setValuesRecargas();
		}

		public override void OnBackPressed() {}

		protected override void OnActivityResult(int requestCode, Result resultCode, Intent data) {
			base.OnActivityResult(requestCode, resultCode, data);

			switch (requestCode) {
				case 3:
					if (data.GetStringExtra("recargas").Equals("closeRecargas")) {
						Intent intent = new Intent();
						intent.PutExtra("recargas", "reloadUR");
						SetResult(Result.Ok, intent);
						Finish();
					}
				break;
			}
		}

		public override bool OnOptionsItemSelected(IMenuItem item) {
			if (item.ItemId == Android.Resource.Id.Home) {
				switch (idFragment) {
					case "nuevaRecarga":
						Intent intent = new Intent();
						intent.PutExtra("recargas", "noAction");
						SetResult(Result.Ok, intent);
						Finish();
					break;
					case "recargasUR":
						Intent i = new Intent();
						i.PutExtra("recargas", "noAction");
						SetResult(Result.Ok, i);
						Finish();
					break;
					case "idRecargas":
						changeMainFragment(new FragmentNuevaRecarga(SupportFragmentManager, listSRMovil, listSRFijo, isLogin),
							   Resources.GetString(Resource.String.recargas_title_new_recargas));
					break;
					case "Términos y Condiciones Legales":
						changeMainFragment(new FragmentIngresaDatosRecarga(
							servicioRecarga, listMontoRecarga, this, isLogin),
											  Resources.GetString(Resource.String.recargas_title_id_recargas));
					break;
					case "deudasRecarga":
						changeMainFragment(new FragmentIngresaDatosRecarga(
							servicioRecarga, listMontoRecarga, this, isLogin),
											  Resources.GetString(Resource.String.recargas_title_id_recargas));
					break;
					case "rutEmail":
						changeMainFragment(new FragmentListaRecargas(solicitaRecarga, this, isLogin),
										  Resources.GetString(Resource.String.recargas_deudas_title));
					break;
					case "rutEmailUR":
						if (isLogin) {
							changeMainFragment(new FragmentRecargaUR(datosRecarga, listMR, this),
										  Resources.GetString(Resource.String.recargas_title_recargas_ur));
						} else {
							changeMainFragment(new FragmentRecargaUR(datosRecargaPE, listMR, this),
										  Resources.GetString(Resource.String.recargas_title_recargas_ur));
						}
					break;
					case "mediosPago":
						if (isLogin) {
							changeMainFragment(new FragmentListaRecargas(solicitaRecarga, this, isLogin),
										  Resources.GetString(Resource.String.recargas_deudas_title));
						} else {
							changeMainFragment(new FragmentRutEmail(listMediosPago, solicitaRecarga, false, this),
											  Resources.GetString(Resource.String.recargas_rut_email_title));
						}
					break;
					case "mediosPagoUR":
						if (isLogin) {
							changeMainFragment(new FragmentRecargaUR(datosRecarga, listMR, this),
										  Resources.GetString(Resource.String.recargas_title_recargas_ur));
						} else {
							changeMainFragment(new FragmentRutEmail(listMediosPago, solicitaRecarga, true, this),
											  Resources.GetString(Resource.String.recargas_rut_email_ur_title));
						}
						break;
					case "wcontext":
						changeMainFragment(new FragmentIngresaDatosRecarga(
							servicioRecarga, listMontoRecarga, this, isLogin),
											  Resources.GetString(Resource.String.recargas_title_id_recargas));
					break;
					case "wcontextUR":
						if (isLogin) {
							changeMainFragment(new FragmentRecargaUR(datosRecarga, listMR, this),
										  Resources.GetString(Resource.String.recargas_title_recargas_ur));
						} else {
							changeMainFragment(new FragmentRecargaUR(datosRecargaPE, listMR, this),
										  Resources.GetString(Resource.String.recargas_title_recargas_ur));
						}
					break;
				}
			}
			return base.OnOptionsItemSelected(item);
		}

		public void setValuesRecargas() {
			toolbar = FindViewById<Android.Support.V7.Widget.Toolbar>(Resource.Id.toolbarBack);
			SetSupportActionBar(toolbar);
			SupportActionBar.SetDisplayHomeAsUpEnabled(true);
			SupportActionBar.SetDisplayShowHomeEnabled(true);
			SupportActionBar.SetDisplayShowTitleEnabled(false);

			if (!isLogin) {
				carroCompraLayout = FindViewById<RelativeLayout>(Resource.Id.carroCompraLayout);
				carroCompra = FindViewById<ImageButton>(Resource.Id.carroCompra);
				badgeText = FindViewById<TextView>(Resource.Id.badgeText);

				carroCompraLayout.Visibility = ViewStates.Visible;
				badgeText.Text = badgeCount;

				carroCompra.Click += (sender, e) => {
					Intent intent = new Intent();
					intent.PutExtra("recargas", "abreCarroCompra");
					SetResult(Result.Ok, intent);
					Finish();
				};
			}

			if (isLogin) {
				if (datosRecarga != null) {
					changeMainFragment(new FragmentRecargaUR(datosRecarga, listMR, this), Resources.GetString(Resource.String.recargas_title_recargas_ur));
				} else {
					changeMainFragment(new FragmentNuevaRecarga(SupportFragmentManager, listSRMovil, listSRFijo, isLogin),
							   Resources.GetString(Resource.String.recargas_title_new_recargas));
				}
			} else {
				if (datosRecargaPE != null) {
					changeMainFragment(new FragmentRecargaUR(datosRecargaPE, listMR, this), Resources.GetString(Resource.String.recargas_title_recargas_ur));
				} else {
					changeMainFragment(new FragmentNuevaRecarga(SupportFragmentManager, listSRMovil, listSRFijo, isLogin),
							   Resources.GetString(Resource.String.recargas_title_new_recargas));
				}
			}
		}

		public void changeMainFragment(Android.Support.V4.App.Fragment fragment, string idFragment) {
			this.idFragment = idFragment;
			var ft = SupportFragmentManager.BeginTransaction();
			ft.Replace(Resource.Id.frameTiposRecarga, fragment);
			ft.Commit();
		}
	}
}
