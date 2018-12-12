using System.Collections.Generic;
using System.Linq;
using Android.Content;
using Android.OS;
using Android.Support.V4.App;
using Android.Support.V4.View;
using Android.Text;
using Android.Views;
using Android.Widget;
using AndroidHUD;
using Newtonsoft.Json.Linq;

namespace ServipagMobile.Droid {
	public class FragmentCambiaClave : Fragment {
		private ViewPager viewPager;
		private EditText fieldCActual;
		private EditText fieldCNueva;
		private EditText fieldCRNueva;
		private ImageView showPasswords;
		private Button saveButton;
		private Validations val;
		private bool hidePassword = true;
		private MainActivity ma;

		public FragmentCambiaClave(ViewPager viewPager) {
			this.viewPager = viewPager;
		}

		public override void OnCreate(Bundle savedInstanceState) {
			base.OnCreate(savedInstanceState);

			ma = (MainActivity)Activity;
			val = new Validations();
		}

		public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState) {
			return inflater.Inflate(Resource.Layout.FragmentCambiaClave, container, false);
		}

		public override void OnViewCreated(View view, Bundle savedInstanceState) {
			base.OnViewCreated(view, savedInstanceState);

			fieldCActual = view.FindViewById<EditText>(Resource.Id.fieldClaveActual);
			fieldCNueva = view.FindViewById<EditText>(Resource.Id.fieldClaveNueva);
			fieldCRNueva = view.FindViewById<EditText>(Resource.Id.fieldClaveRepitaNueva);
			showPasswords = view.FindViewById<ImageView>(Resource.Id.showPasswords);
			saveButton = view.FindViewById<Button>(Resource.Id.saveNewPassword);

			showPasswords.Click += delegate {
				if (hidePassword) {
					hidePassword = false;
					showPasswords.SetImageResource(Resource.Drawable.ver_gris);
					fieldCActual.InputType = InputTypes.ClassText | InputTypes.TextVariationVisiblePassword;
					fieldCNueva.InputType = InputTypes.ClassText | InputTypes.TextVariationVisiblePassword;
					fieldCRNueva.InputType = InputTypes.ClassText | InputTypes.TextVariationVisiblePassword;
				} else {
					hidePassword = true;
					showPasswords.SetImageResource(Resource.Drawable.nover_gris);
					fieldCActual.InputType = InputTypes.ClassText | InputTypes.TextVariationPassword;
					fieldCNueva.InputType = InputTypes.ClassText | InputTypes.TextVariationPassword;
					fieldCRNueva.InputType = InputTypes.ClassText | InputTypes.TextVariationPassword;
				}
			};

			fieldCActual.TextChanged += (sender, e) => {
				fieldCActual.SetSelection(fieldCActual.Text.Length);
			};

			fieldCNueva.TextChanged += (sender, e) => {
				fieldCNueva.SetSelection(fieldCNueva.Text.Length);
			};

			fieldCRNueva.TextChanged += (sender, e) => {
				fieldCRNueva.SetSelection(fieldCRNueva.Text.Length);
			};

			saveButton.Click += (sender, e) => {
				onClickSaveButton();
			};
		}

		private void onClickSaveButton() {
			Dictionary<string, string> fields = new Dictionary<string, string>();

			fields.Add(Resources.GetString(Resource.String.mdatos_hint_cactual), fieldCActual.Text);
			fields.Add(Resources.GetString(Resource.String.mdatos_hint_cnueva_edit), fieldCNueva.Text);
			fields.Add(Resources.GetString(Resource.String.mdatos_hint_crepita), fieldCRNueva.Text);

			if ((bool)val.isEmpty(fields)["code"]) {
				CustomAlertDialog alert = new CustomAlertDialog(ma, "¡Ojo!", (string)val.isEmpty(fields)["data"], "Aceptar", "", null, null);
				alert.showDialog();
			} else if (!(bool)val.areEquals(fieldCNueva.Text, fieldCRNueva.Text, "contraseñas")["code"]) {
				CustomAlertDialog alert = new CustomAlertDialog(ma, "¡Ojo!", "Las contraseñas ingresadas no coinciden", "Aceptar", "", null, null);
				alert.showDialog();
			} else if ((bool)val.areEquals(fieldCActual.Text, fieldCNueva.Text, "contraseñas")["code"]) {
				CustomAlertDialog alert = new CustomAlertDialog(ma, "¡Ojo!", "La nueva contraseña debe ser distinta a la actual", "Aceptar", "", null, null);
				alert.showDialog();
			} else if (fieldCNueva.Text.Length < 6 || fieldCRNueva.Text.Length < 6){
				CustomAlertDialog alert = new CustomAlertDialog(ma, "¡Ojo!", Resources.GetString(Resource.String.login_menos_caracteres), "Aceptar", "", null, null);
				alert.showDialog();
			} else {
				JObject parametros = new JObject();
				AndHUD.Shared.Show(ma, null, -1, MaskType.Black);

				parametros.Add("sesion", RealmDB.GetInstance().realm.All<PersistentData>().First().cookie);
				parametros.Add("canal", DeviceInformation.GetInstance().channel);
				parametros.Add("idUsuario", UserData.GetInstance().rut);
				parametros.Add("claveActual", fieldCActual.Text);
				parametros.Add("nuevaClave", fieldCNueva.Text);
				parametros.Add("firma", "");

				modificarClave(parametros);
			}
		}

		public async void modificarClave(JObject parametros) {
			var response = await MyClass.WorklightClient.UnprotectedInvokeAsync("modificarClave", "modificar_clave", "POST", parametros);

			if (response.Success) {
				if ((int)response.State["Error"] == 0) {
					fieldCNueva.Text = "";
					fieldCActual.Text = "";
					fieldCRNueva.Text = "";

					Intent intent = new Intent(ma, typeof(ComprobanteActivity));
					intent.PutExtra("status", "cClaveSuccess");
					StartActivity(intent);
					viewPager.SetCurrentItem(0, true);
				} else {
					Intent intent = new Intent(ma, typeof(ComprobanteActivity));
					intent.PutExtra("status", "cClaveFail");
					intent.PutExtra("mensaje", response.State["Mensaje"].ToString());
					StartActivity(intent);
				}
			} else {
				Intent intent = new Intent(ma, typeof(ComprobanteActivity));
				intent.PutExtra("status", "cClaveFail");
				intent.PutExtra("mensaje", response.Message);
				StartActivity(intent);
			}

			AndHUD.Shared.Dismiss(ma);
		}
	}
}
