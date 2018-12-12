using System.Collections.Generic;
using System.Linq;
using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using Android.Support.V7.App;
using Android.Views;
using Android.Widget;
using Newtonsoft.Json;

namespace ServipagMobile.Droid {
	[Activity(Label = "PDUActivity", ScreenOrientation = ScreenOrientation.Portrait)]
	public class PDUActivity : AppCompatActivity {
		private Android.Support.V7.Widget.Toolbar toolbar;
		public RelativeLayout carroCompraLayout;
		private ImageButton carroCompra;
		private TextView badgeText;
		private string idFragment;
		private bool isLogin;
		private bool isUPDU;
		private PaseDiario uPDU;
		private PaseTardio uPDT;
		public string badgeCount;
		private string idBiller;
		private string idServicio;
		public List<MediosPago> listMediosPago;
		public bool isPDU;
		public PaseDiario pd;

		protected override void OnCreate(Bundle savedInstanceState) {
			base.OnCreate(savedInstanceState);
			SetContentView(Resource.Layout.PDU);

			isLogin = Intent.GetBooleanExtra("isLogin", false);
			badgeCount = Intent.GetStringExtra("badgeCount");
			idBiller = Intent.GetStringExtra("idBiller");
			idServicio = Intent.GetStringExtra("idServicio");
			isUPDU = Intent.GetBooleanExtra("isUPDU", false);
			if (isUPDU) {
				if (idBiller.Equals("886")) {
					uPDU = JsonConvert.DeserializeObject<PaseDiario>(Intent.GetStringExtra("uPDU"));
				} else if (idBiller.Equals("964")) {
					uPDT = JsonConvert.DeserializeObject<PaseTardio>(Intent.GetStringExtra("uPDT"));
				}
			}


			setValuesRecargas();
		}

		public override void OnBackPressed() { }

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
					intent.PutExtra("pdu", "abreCarroCompra");
					SetResult(Result.Ok, intent);
					Finish();
				};
			}

			if (idBiller.Equals("886")) {
				changeMainFragment(new FragmentSeleccionPDU(SupportFragmentManager, idBiller, idServicio, isUPDU, isLogin, uPDU),
							   Resources.GetString(Resource.String.autopista_id_select_pdu));
			} else if (idBiller.Equals("964")) {
				changeMainFragment(new FragmentSeleccionPDU(SupportFragmentManager, idBiller, idServicio, isUPDU, isLogin, uPDT),
							   Resources.GetString(Resource.String.autopista_id_select_pdu));
			}
		}

		public void changeMainFragment(Android.Support.V4.App.Fragment fragment, string idFragment) {
			this.idFragment = idFragment;
			var ft = SupportFragmentManager.BeginTransaction();
			ft.Replace(Resource.Id.frameSelectPDU, fragment);
			ft.Commit();
		}

		public override bool OnOptionsItemSelected(IMenuItem item) {
			if (item.ItemId == Android.Resource.Id.Home) {
				switch (idFragment) {
					case "selectPDU":
						Intent intent = new Intent();
						intent.PutExtra("pdu", "noAction");
						SetResult(Result.Ok, intent);
						Finish();
					break;
					case "infoDatePDU":
						if (idBiller.Equals("886")) {
							changeMainFragment(new FragmentSeleccionPDU(SupportFragmentManager, idBiller, idServicio, isUPDU, isLogin, uPDU),
										   Resources.GetString(Resource.String.autopista_id_select_pdu));
						} else if (idBiller.Equals("964")) {
							changeMainFragment(new FragmentSeleccionPDU(SupportFragmentManager, idBiller, idServicio, isUPDU, isLogin, uPDT),
										   Resources.GetString(Resource.String.autopista_id_select_pdu));
						}
					break;
					case "pduvendido":
						carroCompraLayout.Visibility = ViewStates.Visible;
						if (idBiller.Equals("886")) {
							changeMainFragment(new FragmentSeleccionPDU(SupportFragmentManager, idBiller, idServicio, isUPDU, isLogin, uPDU),
										   Resources.GetString(Resource.String.autopista_id_select_pdu));
						} else if (idBiller.Equals("964")) {
							changeMainFragment(new FragmentSeleccionPDU(SupportFragmentManager, idBiller, idServicio, isUPDU, isLogin, uPDT),
										   Resources.GetString(Resource.String.autopista_id_select_pdu));
						}
					break;
					case "listaDeudasPDU":
						changeMainFragment(new FragmentPDUVendido(
						RealmDB.GetInstance().realm.All<PaseDiario>().Count() + 1, !isLogin, this), "pduvendido");
					break;
					case "mediosPagoPDU":
						changeMainFragment(new FragmentListaDeudasPDU(pd, this, isLogin),
										   Resources.GetString(Resource.String.autopista_id_list_deudas_pdu));
						
					break;
					case "rutEmailPDU":
						changeMainFragment(new FragmentListaDeudasPDU(pd, this, isLogin),
										   Resources.GetString(Resource.String.autopista_id_list_deudas_pdu));
					break;
					case "wcontextPDU":
						changeMainFragment(new FragmentListaDeudasPDU(pd, this, isLogin),
										   Resources.GetString(Resource.String.autopista_id_list_deudas_pdu));
					break;
					case "wcontextUPDU":
						changeMainFragment(new FragmentListaDeudasPDU(pd, this, isLogin),
										   Resources.GetString(Resource.String.autopista_id_list_deudas_pdu));
					break;
				}
			}
			return base.OnOptionsItemSelected(item);
		}

		protected override void OnActivityResult(int requestCode, Result resultCode, Intent data) {
			base.OnActivityResult(requestCode, resultCode, data);
			switch (requestCode) {
				case 3:
				if (data.GetStringExtra("recargas").Equals("closeRecargas")) {
					Intent intent = new Intent();
					intent.PutExtra("pdu", "reloadUPDU");
					SetResult(Result.Ok, intent);
					Finish();
				}
				break;
				case 14:
					if (data.GetStringExtra("actionPDU") == "noAction") {
						if (idBiller.Equals("886")) {
							changeMainFragment(new FragmentSeleccionPDU(SupportFragmentManager, idBiller, idServicio, isUPDU, isLogin, uPDU),
										   Resources.GetString(Resource.String.autopista_id_select_pdu));
						} else if (idBiller.Equals("964")) {
							changeMainFragment(new FragmentSeleccionPDU(SupportFragmentManager, idBiller, idServicio, isUPDU, isLogin, uPDT),
										   Resources.GetString(Resource.String.autopista_id_select_pdu));
						}
					}
				break;
			}
		}
	}
}
