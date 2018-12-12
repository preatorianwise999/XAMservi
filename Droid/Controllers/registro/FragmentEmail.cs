using System;
using System.Collections.Generic;
using Android.OS;
using Android.Support.V4.App;
using Android.Util;
using Android.Views;
using Android.Widget;

namespace ServipagMobile.Droid {
	public class FragmentEmail : Fragment {
		private Button nextEmail;
		private EditText fEmail;
		private EditText fReEmail;
		private RadioButton rCuentas;
		private RadioButton rNovedades;
		private RadioButton rCartolas;
		private Validations val;

		private bool cuenta = true;
		private bool novedades = true;
		private bool cartola = true;

		public override void OnCreate(Bundle savedInstanceState) {
			base.OnCreate(savedInstanceState);

			this.val = new Validations();
		}

		public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState) {
			return inflater.Inflate(Resource.Layout.FragmentEmail, container, false);
		}

		public override void OnViewCreated(View view, Bundle savedInstanceState) {
			base.OnViewCreated(view, savedInstanceState);

			nextEmail = view.FindViewById<Button>(Resource.Id.nextEmail);
			fEmail = view.FindViewById<EditText>(Resource.Id.emailFieldEmail);
			fReEmail = view.FindViewById<EditText>(Resource.Id.reEmailFieldEmail);
			rCuentas = view.FindViewById<RadioButton>(Resource.Id.radioCtas);
			rNovedades = view.FindViewById<RadioButton>(Resource.Id.radioNovedades);
			rCartolas = view.FindViewById<RadioButton>(Resource.Id.radioCartola);

			if (RegisterData.GetInstance().email != "") {
				fEmail.Text = RegisterData.GetInstance().email;
				if (val.expresionEmail(fEmail.Text)) {
					fEmail.SetTextColor(Resources.GetColor(Resource.Color.servipag_green));
				} else {
					fEmail.SetTextColor(Resources.GetColor(Resource.Color.servipag_red));
				}
			}

			if (RegisterData.GetInstance().radioEmailSelected[0] == -1) {
				rCuentas.Checked = true;
				rNovedades.Checked = true;
				rCartolas.Checked = true;
			} else {
				rCuentas.Checked = Convert.ToBoolean(RegisterData.GetInstance().radioEmailSelected[0]);
				rNovedades.Checked = Convert.ToBoolean(RegisterData.GetInstance().radioEmailSelected[1]);
				rCartolas.Checked = Convert.ToBoolean(RegisterData.GetInstance().radioEmailSelected[2]);

				cuenta = Convert.ToBoolean(RegisterData.GetInstance().radioEmailSelected[0]);
				novedades = Convert.ToBoolean(RegisterData.GetInstance().radioEmailSelected[1]);
				cartola = Convert.ToBoolean(RegisterData.GetInstance().radioEmailSelected[2]);
			}

			nextEmail.Click += delegate {
				onNextClick();
			};

			fEmail.TextChanged += delegate {
				if (val.expresionEmail(fEmail.Text)) {
					fEmail.SetTextColor(Resources.GetColor(Resource.Color.servipag_green));
				} else {
					fEmail.SetTextColor(Resources.GetColor(Resource.Color.servipag_red));
				}
			};

			fReEmail.TextChanged += delegate {
				if (val.expresionEmail(fReEmail.Text)) {
					fReEmail.SetTextColor(Resources.GetColor(Resource.Color.servipag_green));
				} else {
					fReEmail.SetTextColor(Resources.GetColor(Resource.Color.servipag_red));
				}
			};

			rCuentas.Click += (sender, e) => {
				if (cuenta) {
					cuenta = false;
					rCuentas.Checked = cuenta;
				} else {
					cuenta = true;
					rCuentas.Checked = cuenta;
				}
			};
			rNovedades.Click += (sender, e) => {
				if (novedades) {
					novedades = false;
					rNovedades.Checked = novedades;
				} else {
					novedades = true;
					rNovedades.Checked = novedades;
				}
			};
			rCartolas.Click += (sender, e) => {
				if (cartola) {
					cartola = false;
					rCartolas.Checked = cartola;
				} else {
					cartola = true;
					rCartolas.Checked = cartola;
				}
			};
		}

		private void onNextClick() {
			Dictionary<string, string> fields = new Dictionary<string, string>();
			fields.Add("Email", fEmail.Text);
			fields.Add("Repetir Email", fReEmail.Text);

			if ((bool)val.isEmpty(fields)["code"]) {
				CustomAlertDialog alert = new CustomAlertDialog((RegistroActivity)Activity, "¡Ojo!", (string)val.isEmpty(fields)["data"], "Aceptar", "", null, null);
				alert.showDialog();
			} else if (!(bool)val.areEquals(fEmail.Text, fReEmail.Text, "e-mails")["code"]){
				CustomAlertDialog alert = new CustomAlertDialog((RegistroActivity)Activity, "¡Ojo!", (string)val.areEquals(fEmail.Text, fReEmail.Text, "Emails")["data"], "Aceptar", "", null, null);
				alert.showDialog();
			} else if (!val.expresionEmail(fEmail.Text) || !val.expresionEmail(fReEmail.Text)){
				CustomAlertDialog alert = new CustomAlertDialog((RegistroActivity)Activity, "¡Ojo!", "El e-mail ingresado es incorrecto.", "Aceptar", "", null, null);
				alert.showDialog();
			} else {
				RegisterData.GetInstance().email = fEmail.Text;
				RegisterData.GetInstance().radioEmailSelected[0] = Convert.ToInt32(rCuentas.Checked);
				RegisterData.GetInstance().radioEmailSelected[1] = Convert.ToInt32(rNovedades.Checked);
				RegisterData.GetInstance().radioEmailSelected[2] = Convert.ToInt32(rCartolas.Checked);

				var activity = (RegistroActivity)Activity;
				activity.changeMainFragment(new FragmentPersonalData(), Resources.GetString(Resource.String.registro_id_pData));

			}
		}
	}
}
