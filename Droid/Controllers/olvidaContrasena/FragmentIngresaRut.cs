using System.Collections.Generic;
using Android.OS;
using Android.Support.V4.App;
using Android.Views;
using Android.Widget;

namespace ServipagMobile.Droid {
	public class FragmentIngresaRut : Fragment {
		private EditText rut;
		private Button bttnNext;
		private OlvidaContrasenaActivity oca;
		private Validations val;
		private Utils utils;

		public override void OnCreate(Bundle savedInstanceState) {
			base.OnCreate(savedInstanceState);

			oca = (OlvidaContrasenaActivity)Activity;
			val = new Validations();
			utils = new Utils();
		}

		public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState) {
			return inflater.Inflate(Resource.Layout.FragmentIngresaRut, container, false);
		}

		public override void OnViewCreated(View view, Bundle savedInstanceState) {
			base.OnViewCreated(view, savedInstanceState);

			rut = view.FindViewById<EditText>(Resource.Id.rutOC);
			bttnNext = view.FindViewById<Button>(Resource.Id.buttonIRutNext);


			var edited = true;
			rut.TextChanged += (sender, e) => {
				if (edited) {
					edited = false;
					string editado = utils.formatearRut(rut.Text);
					rut.Text = editado;
					if (val.expresionRut(rut.Text) && (bool)val.validateRut(rut.Text)["code"]) {
						rut.SetTextColor(Resources.GetColor(Resource.Color.servipag_green));
					} else {
						rut.SetTextColor(Resources.GetColor(Resource.Color.servipag_red));
					}
					rut.SetSelection(rut.Text.Length);
					edited = true;
				}
			};

			bttnNext.Click += (sender, e) => {
				onNextClick();
			};
		}

		private void onNextClick() {
			Dictionary<string, string> fields = new Dictionary<string, string>();
			fields.Add("RUT", rut.Text);

			if ((bool)val.isEmpty(fields)["code"]) {
				CustomAlertDialog alert = new CustomAlertDialog(oca, "¡Ojo!", (string)val.isEmpty(fields)["data"], "Aceptar", "", null, null);
				alert.showDialog();
			} else if (!val.expresionRut(rut.Text)) {
				CustomAlertDialog alert = new CustomAlertDialog(oca, "¡Ojo!", "El rut ingresado no es válido.", "Aceptar", "", null, null);
				alert.showDialog();
			} else if (!(bool)val.validateRut(rut.Text)["code"]) {
				CustomAlertDialog alert = new CustomAlertDialog(oca, "¡Ojo!", (string)val.validateRut(rut.Text)["data"], "Aceptar", "", null, null);
				alert.showDialog();
			} else {
				oca.changeMainFragment(new FragmentCaptcha(rut.Text, "recuperar"), Resources.GetString(Resource.String.ocontrasena_id_fragment_captcha));
			}
		}
	}
}
