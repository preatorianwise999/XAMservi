using System.Collections.Generic;
using Android.OS;
using Android.Support.V4.App;
using Android.Util;
using Android.Views;
using Android.Widget;

namespace ServipagMobile.Droid {
	public class FragmentYourName : Fragment {
		private Button nextName;
		private EditText nameField;
		private EditText lpName;
		private EditText lmName;
		private Validations val;

		public override void OnCreate(Bundle savedInstanceState) {
			base.OnCreate(savedInstanceState);
			this.val = new Validations();
		}

		public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState) {
			return inflater.Inflate(Resource.Layout.FragmentYourName, container, false);
		}

		public override void OnViewCreated(View view, Bundle savedInstanceState) {
			base.OnViewCreated(view, savedInstanceState);

			nextName = view.FindViewById<Button>(Resource.Id.nextName);
			nameField = view.FindViewById<EditText>(Resource.Id.nameField);
			lpName = view.FindViewById<EditText>(Resource.Id.plnameField);
			lmName = view.FindViewById<EditText>(Resource.Id.mlnameField);

			if (RegisterData.GetInstance().name != null) {
				nameField.Text = RegisterData.GetInstance().name;
			}
			if (RegisterData.GetInstance().lastNP != "") {
				lpName.Text = RegisterData.GetInstance().lastNP;
			}
			if (RegisterData.GetInstance().lastNM != "") {
				lmName.Text = RegisterData.GetInstance().lastNM;
			}

			nextName.Click += delegate {
				onNextClick();
			};
		}

		private void onNextClick() {
			Dictionary<string, string> fields = new Dictionary<string, string>();
			fields.Add(Resources.GetString(Resource.String.registro_your_name_hint_name), nameField.Text);
			fields.Add(Resources.GetString(Resource.String.registro_your_name_hint_plname), lpName.Text);
			fields.Add(Resources.GetString(Resource.String.registro_your_name_hint_mlname), lmName.Text);

			if ((bool)val.isEmpty(fields)["code"]) {
				CustomAlertDialog alert = new CustomAlertDialog((RegistroActivity)Activity, "¡Ojo!", (string)val.isEmpty(fields)["data"], "Aceptar", "", null, null);
				alert.showDialog();
			} else {
				RegisterData.GetInstance().name = nameField.Text;
				RegisterData.GetInstance().lastNP = lpName.Text;
				RegisterData.GetInstance().lastNM = lmName.Text;
				var activity = (RegistroActivity)Activity;
				activity.changeMainFragment(new FragmentRut(), Resources.GetString(Resource.String.registro_id_rut));
			}
		}


	}
}
