using System;
using System.Collections.Generic;
using System.Linq;
using Android.OS;
using Android.Support.V4.App;
using Android.Views;
using Android.Widget;
using AndroidHUD;
using Newtonsoft.Json.Linq;

namespace ServipagMobile.Droid {
	public class FragmentRut : Fragment {
		private TextView subTitleRut;
		private EditText rutField;
		private Button nextRut;
		private Validations val;
		private Utils utils;

		public override void OnCreate(Bundle savedInstanceState) {
			base.OnCreate(savedInstanceState);

			this.val = new Validations();
			this.utils = new Utils();
		}

		public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState) {
			return inflater.Inflate(Resource.Layout.FragmentRut, container, false);
		}
		public override void OnViewCreated(View view, Bundle savedInstanceState) {
			base.OnViewCreated(view, savedInstanceState);
			nextRut = view.FindViewById<Button>(Resource.Id.nextRut);
			rutField = view.FindViewById<EditText>(Resource.Id.rutField);

			if (RegisterData.GetInstance().rut != "") {
				rutField.Text = RegisterData.GetInstance().rut;
				if (val.expresionRut(rutField.Text) && (bool)val.validateRut(rutField.Text)["code"]) {
					rutField.SetTextColor(Resources.GetColor(Resource.Color.servipag_green));
				} else {
					rutField.SetTextColor(Resources.GetColor(Resource.Color.servipag_red));
				}
			}

			nextRut.Click += delegate {
				onNextClick();
			};

			var edited = true;
			rutField.TextChanged += (sender, e) => {
				if (edited) {
					edited = false;
					string editado = utils.formatearRut(rutField.Text);
					rutField.Text = editado;
					if (val.expresionRut(rutField.Text) && (bool)val.validateRut(rutField.Text)["code"]) {
						rutField.SetTextColor(Resources.GetColor(Resource.Color.servipag_green));
					} else {
						rutField.SetTextColor(Resources.GetColor(Resource.Color.servipag_red));
					}
					rutField.SetSelection(rutField.Text.Length);
					edited = true;
				}
			};
		}

		private void onNextClick() {
			Dictionary<string, string> fields = new Dictionary<string, string>();
			fields.Add("RUT", rutField.Text);

			if ((bool)val.isEmpty(fields)["code"]) {
				CustomAlertDialog alert = new CustomAlertDialog((RegistroActivity)Activity, "¡Ojo!", "Debes ingresar el siguiente dato para continuar:\n\nRut", "Aceptar", "", null, null);
				alert.showDialog();
			} else if (val.expresionRut(rutField.Text) && (bool)val.validateRut(rutField.Text)["code"]) {
				RegisterData.GetInstance().rut = rutField.Text;
				var activity = (RegistroActivity)Activity;
				activity.changeMainFragment(new FragmentRegionComuna(), Resources.GetString(Resource.String.registro_id_regCom));
			} else {
				CustomAlertDialog alert = new CustomAlertDialog((RegistroActivity)Activity, "¡Ojo!", (string)val.validateRut(rutField.Text)["data"], "Aceptar", "", null, null);
				alert.showDialog();
			}
		}
	}
}
