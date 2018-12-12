using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using Android.Support.V7.App;
using Android.Views;

namespace ServipagMobile.Droid {
	[Activity(Label = "RegistroActivity", ScreenOrientation = ScreenOrientation.Portrait)]
	public class RegistroActivity : AppCompatActivity {
		private Android.Support.V7.Widget.Toolbar toolbar;
		private string idFragment;

		protected override void OnCreate(Bundle savedInstanceState) {
			base.OnCreate(savedInstanceState);
			SetContentView(Resource.Layout.Registro);

			setValuesRegistro();
		}

		public override void OnBackPressed() {
			//base.OnBackPressed();

		}

		public void setValuesRegistro() {
			toolbar = FindViewById<Android.Support.V7.Widget.Toolbar>(Resource.Id.toolbarBack);
			SetSupportActionBar(toolbar);
			SupportActionBar.SetDisplayHomeAsUpEnabled(true);
			SupportActionBar.SetDisplayShowHomeEnabled(true);
			SupportActionBar.SetDisplayShowTitleEnabled(false);

			RegisterData.GetInstance("", "", "", "", -1, -1, "", null, "", "", "", this, "", "", "");

			changeMainFragment(new FragmentYourName(), Resources.GetString(Resource.String.registro_id_name));
		}

		public override bool OnOptionsItemSelected(IMenuItem item) {
			if (item.ItemId == Android.Resource.Id.Home) {
				switch (idFragment) {
					case "name":
						RegisterData.GetInstance().name = "";
						RegisterData.GetInstance().lastNP = "";
						RegisterData.GetInstance().lastNM = "";
						RegisterData.GetInstance().rut = "";
						RegisterData.GetInstance().region = -1;
						RegisterData.GetInstance().comuna = -1;
						RegisterData.GetInstance().email = "";
						RegisterData.GetInstance().radioEmailSelected = null;
						RegisterData.GetInstance().birthDate = "";
						RegisterData.GetInstance().showBD = "";
						RegisterData.GetInstance().radioGenderSelected = "";
						RegisterData.GetInstance().activity = null;
						RegisterData.GetInstance().listType = "";
						RegisterData.GetInstance().deviceType = "";
						RegisterData.GetInstance().idFragment = "";
						Intent intent = new Intent();
						intent.PutExtra("registro", "finish");
						SetResult(Result.Ok, intent);
						Finish();
					break;
					case "rut":
						changeMainFragment(new FragmentYourName(), Resources.GetString(Resource.String.registro_id_name));
					break;
					case "regCom":
						changeMainFragment(new FragmentRut(), Resources.GetString(Resource.String.registro_id_rut));
					break;
					case "listarc":
						changeMainFragment(new FragmentRegionComuna(), Resources.GetString(Resource.String.registro_id_regCom));
					break;
					case "email":
						changeMainFragment(new FragmentRegionComuna(), Resources.GetString(Resource.String.registro_id_regCom));
					break;
					case "pData":
						changeMainFragment(new FragmentEmail(), Resources.GetString(Resource.String.registro_id_email));
					break;
					case "dPicker":
						changeMainFragment(new FragmentPersonalData(), Resources.GetString(Resource.String.registro_id_pData));
					break;
					case "password":
						changeMainFragment(new FragmentPersonalData(), Resources.GetString(Resource.String.registro_id_pData));
					break;
					case "captcha":
						changeMainFragment(new FragmentPassword(), Resources.GetString(Resource.String.registro_id_password));
					break;
				}
			}
			return base.OnOptionsItemSelected(item);
		}

		public void changeMainFragment(Android.Support.V4.App.Fragment fragment, string idFragment) {
			this.idFragment = idFragment;
			var ft = SupportFragmentManager.BeginTransaction();
			ft.Replace(Resource.Id.mainFragmentRegistro, fragment);
			ft.Commit();
		}
	}
}
