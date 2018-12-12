using System.Collections.Generic;
using System.Linq;
using Android.Content;
using Android.OS;
using Android.Support.V4.App;
using Android.Text;
using Android.Util;
using Android.Views;
using Android.Widget;
using AndroidHUD;
using Plugin.DeviceInfo;
using Newtonsoft.Json.Linq;
using Realms;

namespace ServipagMobile.Droid {
	public class FragmentPassword : Fragment {
		private TextView subtitlePass;
		private RelativeLayout containerStrength;
		private RelativeLayout containerFirstField;
		private Button terminarPassword;
		private EditText fPassword;
		private EditText fRePassword;
		private ImageView securePassword;
		private LinearLayout strView;
		private TextView strText;
		private Validations val;
		private Utils utils;
		private bool hidePassword = true;

		public override void OnCreate(Bundle savedInstanceState) {
			base.OnCreate(savedInstanceState);

			this.val = new Validations();
			utils = new Utils();
		}

		public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState) {
			return inflater.Inflate(Resource.Layout.FragmentPassword, container, false);
		}

		public override void OnViewCreated(View view, Bundle savedInstanceState) {
			base.OnViewCreated(view, savedInstanceState);

			fPassword = view.FindViewById<EditText>(Resource.Id.fieldPassword);
			fRePassword = view.FindViewById<EditText>(Resource.Id.fieldRePassword);
			securePassword = view.FindViewById<ImageView>(Resource.Id.showPasswords);
			strView = view.FindViewById<LinearLayout>(Resource.Id.strengthView);
			strText = view.FindViewById<TextView>(Resource.Id.strengthText);
			terminarPassword = view.FindViewById<Button>(Resource.Id.nextPassword);

			securePassword.Click += delegate {
				if (hidePassword) {
					hidePassword = false;
					securePassword.SetImageResource(Resource.Drawable.ver_blanco);
					fPassword.InputType = InputTypes.TextVariationVisiblePassword;
					fRePassword.InputType = InputTypes.TextVariationVisiblePassword;
				} else {
					hidePassword = true;
					securePassword.SetImageResource(Resource.Drawable.nover_blanco);
					fPassword.InputType = InputTypes.ClassText | InputTypes.TextVariationPassword;
					fRePassword.InputType = InputTypes.ClassText | InputTypes.TextVariationPassword;
				}
			};

			fPassword.TextChanged += delegate {
				strView.Visibility = ViewStates.Visible;
				strText.Visibility = ViewStates.Visible;

				if (val.expresionBorrar(fPassword.Text) && fPassword.Text != "") {
					fPassword.Text = fPassword.Text.Substring(0, fPassword.Text.Length - 1);
				}
				fPassword.SetSelection(fPassword.Text.Length);

				switch (val.expresionPassword(fPassword.Text)) {
					case "weak":
						strView.SetBackgroundColor(Resources.GetColor(Resource.Color.servipag_red));
						strText.Text = Resources.GetString(Resource.String.registro_password_strength_weak);
					break;
					case "medium":
						strView.SetBackgroundColor(Resources.GetColor(Resource.Color.servipag_yellow));
						strText.Text = Resources.GetString(Resource.String.registro_password_strength_medium);
					break;
					case "strong":
						strView.SetBackgroundColor(Resources.GetColor(Resource.Color.servipag_green));
						strText.Text = Resources.GetString(Resource.String.registro_password_strength_strong);
					break;
				}

			};

			fRePassword.TextChanged += delegate {
				if (val.expresionBorrar(fRePassword.Text) && fRePassword.Text != "") {
					fRePassword.Text = fRePassword.Text.Substring(0, fRePassword.Text.Length - 1);
				}
				fRePassword.SetSelection(fRePassword.Text.Length);
			};

			terminarPassword.Click += delegate {
				OnFinishClick();
			};
		}

		public void OnFinishClick() {
			Dictionary<string, string> fields = new Dictionary<string, string>();
			fields.Add(Resources.GetString(Resource.String.registro_password_hint_pass), fPassword.Text);
			fields.Add(Resources.GetString(Resource.String.registro_password_hint_repass), fRePassword.Text);

			if ((bool)val.isEmpty(fields)["code"]) {
				CustomAlertDialog alert = new CustomAlertDialog((RegistroActivity)Activity, "¡Ojo!", (string)val.isEmpty(fields)["data"], "Aceptar", "", null, null);
				alert.showDialog();
			} else if (!(bool)val.areEquals(fPassword.Text, fRePassword.Text, "Passwords")["code"]) {
				CustomAlertDialog alert = new CustomAlertDialog((RegistroActivity)Activity, "¡Ojo!", "Verifica que las contraseñas sean iguales.", "Aceptar", "", null, null);
				alert.showDialog();
			} else if (fPassword.Text.Length < 6) {
				CustomAlertDialog alert = new CustomAlertDialog((RegistroActivity)Activity, "¡Ojo!", Resources.GetString(Resource.String.login_menos_caracteres), "Aceptar", "", null, null);
				alert.showDialog();
			} else {
				var activity = (RegistroActivity)Activity;
				activity.changeMainFragment(new FragmentCaptcha(fPassword.Text, "registro"), Resources.GetString(Resource.String.ocontrasena_id_fragment_captcha));
			}
		}
	}
}
