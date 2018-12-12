using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using Android.Support.V7.App;
using Android.Views;

namespace ServipagMobile.Droid {
	[Activity(Label = "OlvidaContrasenaActivity", ScreenOrientation = ScreenOrientation.Portrait)]
	public class OlvidaContrasenaActivity : AppCompatActivity {
		private Android.Support.V7.Widget.Toolbar toolbar;
		private string idFragment;

		protected override void OnCreate(Bundle savedInstanceState) {
			base.OnCreate(savedInstanceState);
			SetContentView(Resource.Layout.OlvidaContrasena);

			setValuesOContrasena();
		}

		public override void OnBackPressed() {
			//base.OnBackPressed();

		}

		public void setValuesOContrasena() {
			toolbar = FindViewById<Android.Support.V7.Widget.Toolbar>(Resource.Id.toolbarBack);
			SetSupportActionBar(toolbar);
			SupportActionBar.SetDisplayHomeAsUpEnabled(true);
			SupportActionBar.SetDisplayShowHomeEnabled(true);
			SupportActionBar.SetDisplayShowTitleEnabled(false);

			changeMainFragment(new FragmentIngresaRut(), Resources.GetString(Resource.String.ocontrasena_id_fragment_IR));
		}

		public override bool OnOptionsItemSelected(IMenuItem item) {
			if (item.ItemId == Android.Resource.Id.Home) {
				switch(idFragment) {
					case "IR":
						Intent intent = new Intent();
						intent.PutExtra("valueCompleted", "noAction");
						SetResult(Result.Ok, intent);
						Finish();
					break;
					case "captcha":
						changeMainFragment(new FragmentIngresaRut(), Resources.GetString(Resource.String.ocontrasena_id_fragment_IR));
					break;
				}

			}
			return base.OnOptionsItemSelected(item);
		}

		protected override void OnActivityResult(int requestCode, Result resultCode, Android.Content.Intent data) {
			base.OnActivityResult(requestCode, resultCode, data);

			if (requestCode == 4) {
				if (data.GetStringExtra("type") == "close") {
					Intent intent = new Intent();
					intent.PutExtra("valueCompleted", "success");
					SetResult(Result.Ok, intent);
					Finish();
				}
			}
		}

		public void changeMainFragment(Android.Support.V4.App.Fragment fragment, string idFragment) {
			this.idFragment = idFragment;
			var ft = SupportFragmentManager.BeginTransaction();
			ft.Replace(Resource.Id.mainFragmentOContrasena, fragment);
			ft.Commit();
		}
	}
}
