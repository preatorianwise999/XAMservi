
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.OS;
using Android.Support.V4.App;
using Android.Util;
using Android.Views;
using Android.Widget;

namespace ServipagMobile.Droid {
	public class FragmentPersonalData : Fragment {
		private Button getBirthDate;
		private RadioButton rMan;
		private RadioButton rWoman;
		private Button nextPD;
		private Validations val;

		public override void OnCreate(Bundle savedInstanceState) {
			base.OnCreate(savedInstanceState);

			this.val = new Validations();
		}

		public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState) {
			return inflater.Inflate(Resource.Layout.FragmentPersonalData, container, false);
		}

		public override void OnViewCreated(View view, Bundle savedInstanceState) {
			base.OnViewCreated(view, savedInstanceState);

			getBirthDate = view.FindViewById<Button>(Resource.Id.bttnBirthDate);
			rMan = view.FindViewById<RadioButton>(Resource.Id.radioMan);
			rWoman = view.FindViewById<RadioButton>(Resource.Id.radioWoman);
			nextPD = view.FindViewById<Button>(Resource.Id.nextPersonalData);

			if (RegisterData.GetInstance().birthDate != "") {
				getBirthDate.Text = RegisterData.GetInstance().showBD;
			}

			if (RegisterData.GetInstance().radioGenderSelected == "M") {
				rMan.Checked = true;
				rWoman.Checked = false;
			} else if (RegisterData.GetInstance().radioGenderSelected == "F"){
				rMan.Checked = false;
				rWoman.Checked = true;
			}

			getBirthDate.Click += delegate {
				FragmentDatePicker frag = FragmentDatePicker.NewInstance(delegate (DateTime time) {
					getBirthDate.Text = time.ToString("MMMM dd, yyyy");
					RegisterData.GetInstance().birthDate = time.ToString("yyyy/MM/dd") + " " + time.ToString("T").Split(' ')[0];
					RegisterData.GetInstance().showBD = time.ToLongDateString();
				});
				frag.parent = "";
				frag.Show(FragmentManager, FragmentDatePicker.TAG);
			};
			nextPD.Click += delegate {
				onNextClick();
			};
		}

		public void onNextClick() {
			Dictionary<string, string> date = new Dictionary<string, string>();
			List<bool> fields = new List<bool>();

			date.Add(Resources.GetString(Resource.String.registro_pd_hint_date), getBirthDate.Text);
			fields.Add(rMan.Checked);
			fields.Add(rWoman.Checked);

			if (rMan.Checked) {
				RegisterData.GetInstance().radioGenderSelected = "M";
			} else if (rWoman.Checked) {
				RegisterData.GetInstance().radioGenderSelected = "F";
			}

			if (!(bool)val.areSelected(date)["code"]) {
				CustomAlertDialog alert = new CustomAlertDialog((RegistroActivity)Activity, "¡Ojo!", (string)val.areSelected(date)["data"], "Aceptar", "", null, null);
				alert.showDialog();
			} else if (!(bool)val.areChecked("su género", fields)["code"]) {
				CustomAlertDialog alert = new CustomAlertDialog((RegistroActivity)Activity, "¡Ojo!", (string)val.areChecked("su género", fields)["data"], "Aceptar", "", null, null);
				alert.showDialog();
			} else {
				var activity = (RegistroActivity)Activity;
				activity.changeMainFragment(new FragmentPassword(), Resources.GetString(Resource.String.registro_id_password));
			}
		}
	}
}
