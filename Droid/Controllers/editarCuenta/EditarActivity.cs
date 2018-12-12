using Android.App;
using Android.Content;
using Android.OS;
using Android.Support.V7.App;
using Android.Views;
using Newtonsoft.Json;

namespace ServipagMobile.Droid {
	[Activity(Label = "EditarActivity")]
	public class EditarActivity : AppCompatActivity {
		private Android.Support.V7.Widget.Toolbar toolbar;
		private MisCuentas servicio;
		private int indexCuenta;

		protected override void OnCreate(Bundle savedInstanceState) {
			base.OnCreate(savedInstanceState);
			SetContentView(Resource.Layout.Editar);

			servicio = JsonConvert.DeserializeObject<MisCuentas>(Intent.GetStringExtra("servicio"));
			indexCuenta = Intent.GetIntExtra("indexCuenta", 0);

			setValuesEditarCta();
		}

		public override void OnBackPressed() {
			//base.OnBackPressed();
		}

		public void setValuesEditarCta() {
			toolbar = FindViewById<Android.Support.V7.Widget.Toolbar>(Resource.Id.toolbarBack);
			SetSupportActionBar(toolbar);
			SupportActionBar.SetDisplayHomeAsUpEnabled(true);
			SupportActionBar.SetDisplayShowHomeEnabled(true);
			SupportActionBar.SetDisplayShowTitleEnabled(false);

			changeMainFragment(new FragmentEditCuenta(servicio, indexCuenta));
		}

		public override bool OnOptionsItemSelected(IMenuItem item) {
			if (item.ItemId == Android.Resource.Id.Home) {
				Intent intent = new Intent();
				intent.PutExtra("actionEditar", "noAction");
				SetResult(Result.Ok, intent);
				Finish();
			}
			return base.OnOptionsItemSelected(item);
		}

		public void changeMainFragment(Android.Support.V4.App.Fragment fragment) {
			var ft = SupportFragmentManager.BeginTransaction();
			ft.Replace(Resource.Id.mainFragmentEditCta, fragment);
			ft.Commit();
		}

		protected override void OnActivityResult(int requestCode, Result resultCode, Intent data) {
			base.OnActivityResult(requestCode, resultCode, data);

			if (requestCode == 21) {
				if (data.GetStringExtra("actionComprobanteEditar").Equals("cerrarEditar")) {
					Intent intent = new Intent();
					intent.PutExtra("actionEditar", "reloadList");

					SetResult(Result.Ok, intent);
					Finish();
				}
			}
		}
	}
}
