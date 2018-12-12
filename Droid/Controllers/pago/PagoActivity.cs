using System.Collections.Generic;
using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using Android.Support.V7.App;
using Android.Views;
using Android.Widget;
using Com.Browser2app.Khenshin;
using Newtonsoft.Json;
using ServipagMobile.Classes;

namespace ServipagMobile.Droid {
	[Activity(Label = "PagoActivity", ScreenOrientation = ScreenOrientation.Portrait)]
	public class PagoActivity : AppCompatActivity {
		private Android.Support.V7.Widget.Toolbar toolbar;
		private string idFragment;
		private bool isLogin;
		private List<BuscaDeudas> misDeudas = new List<BuscaDeudas>();
		public List<MediosPago> mediosPago = new List<MediosPago>();
        public List<Automata> AutomataDatos = new List<Automata>();
        public string idPago;

		private Android.Support.V4.App.Fragment childFragment;

		protected override void OnCreate(Bundle savedInstanceState) {
			base.OnCreate(savedInstanceState);
			SetContentView(Resource.Layout.Pago);
			isLogin = Intent.GetBooleanExtra("isLogin", true);
			misDeudas = JsonConvert.DeserializeObject<List<BuscaDeudas>>(Intent.GetStringExtra("misDeudas"));

			setValuesPago();
		}

		public override void OnBackPressed() {
			//base.OnBackPressed();

		}

		public void setValuesPago() {
			toolbar = FindViewById<Android.Support.V7.Widget.Toolbar>(Resource.Id.toolbarBack);
			SetSupportActionBar(toolbar);
			SupportActionBar.SetDisplayHomeAsUpEnabled(true);
			SupportActionBar.SetDisplayShowHomeEnabled(true);
			SupportActionBar.SetDisplayShowTitleEnabled(false);

			changeMainFragment(new FragmentListaDeudas(isLogin, misDeudas), Resources.GetString(Resource.String.deudas_id_fragment));
		}

		public override bool OnOptionsItemSelected(IMenuItem item) {
			if (item.ItemId == Android.Resource.Id.Home) {
				switch (idFragment) {
					case "deudas":
						Intent intent = new Intent();
						intent.PutExtra("pago", "noAction");
						SetResult(Result.Ok, intent);
						Finish();
						break;
					case "rutEmail":
						changeMainFragment(new FragmentListaDeudas(isLogin, misDeudas), Resources.GetString(Resource.String.deudas_id_fragment));
						break;
					//break;
					case "medios pago":
						if (isLogin) {
							changeMainFragment(new FragmentListaDeudas(isLogin, misDeudas), Resources.GetString(Resource.String.deudas_id_fragment));
						} else {
							changeMainFragment(new FragmentRutEmail(this, mediosPago, misDeudas), Resources.GetString(Resource.String.rut_email_title));
						}
						break;
					case "wcontext":
						((FragmentWebContext)childFragment).isClosing = true;
						Intent i = new Intent();
						i.PutExtra("pago", "noAction");
						SetResult(Result.Ok, i);
						Finish();
					break;
				}
			}
			return base.OnOptionsItemSelected(item);
		}

        protected override void OnActivityResult(int requestCode, Result resultCode, Intent data) {
            base.OnActivityResult(requestCode, resultCode, data);

			if (requestCode == 101) {
				Intent i = new Intent(this, typeof(ComprobanteActivity));
				i.PutExtra("isPagoExpress", !isLogin);
				i.PutExtra("status", "pagoCompletado");
				i.PutExtra("idPago", idPago);

				StartActivity(i);
				closePayment();
			}
        }

        private void closePayment() {
			Intent intent = new Intent();
			intent.PutExtra("pago", "noAction");
			SetResult(Android.App.Result.Ok, intent);
			Finish();
        }

		public void changeMainFragment(Android.Support.V4.App.Fragment fragment, string idFragment) {
			this.idFragment = idFragment;
			this.childFragment = fragment;
			var ft = SupportFragmentManager.BeginTransaction();
			ft.Replace(Resource.Id.mainFragmentPago, fragment);
			ft.Commit();
		}
	}
}
