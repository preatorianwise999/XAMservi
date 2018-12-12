using System.Linq;
using Android.Content;
using Android.OS;
using Android.Support.V4.App;
using Android.Text;
using Android.Text.Style;
using Android.Util;
using Android.Views;
using Android.Widget;
using Realms;

namespace ServipagMobile.Droid {
	public class FragmentTCAutopista : Fragment {
		private Servicios dataPD;
		private bool isPagoExpress;
		private AgregarActivity aa;
		private PDUActivity PDUAct;
		private TextView bodyPDU, bodyPDUT, bodyNext;
		private RadioButton radioTC;
		private Button bttnContinue;
		private bool isChecked = false;
		private string idBiller;
		private string idServicio;

		public FragmentTCAutopista() { }

		public FragmentTCAutopista(Servicios dataPD, bool isPagoExpress, string idBiller, string idServicio, AgregarActivity aa) {
			this.dataPD = dataPD;
			this.isPagoExpress = isPagoExpress;
			this.idBiller = idBiller;
			this.idServicio = idServicio;
			this.aa = aa;
		}

		public FragmentTCAutopista(PDUActivity PDUAct) {
			this.PDUAct = PDUAct;
		}

		public override void OnCreate(Bundle savedInstanceState) {
			base.OnCreate(savedInstanceState);
		}

		public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState) {
			return inflater.Inflate(Resource.Layout.FragmentTCAutopista, container, false);
		}

		public override void OnViewCreated(View view, Bundle savedInstanceState) {
			base.OnViewCreated(view, savedInstanceState);

			bodyPDU = view.FindViewById<TextView>(Resource.Id.bodyPDU);
			bodyPDUT = view.FindViewById<TextView>(Resource.Id.bodyPDUT);
			bodyNext = view.FindViewById<TextView>(Resource.Id.bodyNext);
			radioTC = view.FindViewById<RadioButton>(Resource.Id.radioTC);
			bttnContinue = view.FindViewById<Button>(Resource.Id.bttnContinue);

			bodyPDU.SetText(coloringTextPDU(), TextView.BufferType.Spannable);
			bodyPDUT.SetText(coloringTextPDUT(), TextView.BufferType.Spannable);
			bodyNext.SetText(coloringTextNext(), TextView.BufferType.Spannable);

			if (PDUAct != null) {
				radioTC.Visibility = ViewStates.Gone;
				bttnContinue.Visibility = ViewStates.Gone;
			}

			radioTC.Click += (sender, e) => {
				if (isChecked) {
					isChecked = false;
					radioTC.Checked = isChecked;
					RadioGroup.LayoutParams lp = new RadioGroup.LayoutParams(RadioGroup.LayoutParams.MatchParent,
					                                                             RadioGroup.LayoutParams.WrapContent);
					lp.BottomMargin = 0;
					radioTC.LayoutParameters = lp;
					bttnContinue.Visibility = ViewStates.Gone;
				} else {
					isChecked = true;
					radioTC.Checked = isChecked;
					RadioGroup.LayoutParams lp = new RadioGroup.LayoutParams(RadioGroup.LayoutParams.MatchParent,
																				 RadioGroup.LayoutParams.WrapContent);
					lp.BottomMargin = (int)TypedValue.ApplyDimension(ComplexUnitType.Dip, 40, Resources.DisplayMetrics);
					radioTC.LayoutParameters = lp;
					bttnContinue.Visibility = ViewStates.Visible;
				}
			};

			bttnContinue.Click += (sender, e) => {
				if (!RealmDB.GetInstance().realm.All<PersistentData>().First().acepta_tc_pdu) {
					var pData = RealmDB.GetInstance().realm.All<PersistentData>().First();
					RealmDB.GetInstance().realm.Write(() => pData.acepta_tc_pdu = true);
				}
				Intent intent = new Intent();
				intent.PutExtra("actionAgregar", "openLastPDU");
				intent.PutExtra("idBiller", idBiller);
				intent.PutExtra("idServicio", idServicio);
				aa.SetResult(Android.App.Result.Ok, intent);
				aa.Finish();
			};
		}

		private SpannableStringBuilder coloringTextPDU() {
			SpannableStringBuilder builder = new SpannableStringBuilder();
			string tc1 = Resources.GetText(Resource.String.autopista_pase_diario_tc_body_three) + " ";
			string tc2 = Resources.GetString(Resource.String.autopista_pase_diario_tc_body_four) + " ";
			string tc3 = Resources.GetText(Resource.String.autopista_pase_diario_tc_body_five) + " ";
			string tc4 = Resources.GetString(Resource.String.autopista_pase_diario_tc_body_six);

			SpannableString ss1 = new SpannableString(tc1);
			ss1.SetSpan(new ForegroundColorSpan(Resources.GetColor(Resource.Color.servipag_black)), 0, ss1.Length(), 0);
			builder.Append(ss1);

			SpannableString ss2 = new SpannableString(tc2);
			ss2.SetSpan(new ForegroundColorSpan(Resources.GetColor(Resource.Color.servipag_calipso)), 0, ss2.Length(), 0);
			builder.Append(ss2);

			SpannableString ss3 = new SpannableString(tc3);
			ss3.SetSpan(new ForegroundColorSpan(Resources.GetColor(Resource.Color.servipag_black)), 0, ss3.Length(), 0);
			builder.Append(ss3);

			SpannableString ss4 = new SpannableString(tc4);
			ss4.SetSpan(new ForegroundColorSpan(Resources.GetColor(Resource.Color.servipag_calipso)), 0, ss4.Length(), 0);
			builder.Append(ss4);

			return builder;
		}

		private SpannableStringBuilder coloringTextPDUT() {
			SpannableStringBuilder builder = new SpannableStringBuilder();
			string tc1 = Resources.GetText(Resource.String.autopista_pase_diario_tc_body_eight) + " ";
			string tc2 = Resources.GetString(Resource.String.autopista_pase_diario_tc_body_nine) + " ";
			string tc3 = Resources.GetText(Resource.String.autopista_pase_diario_tc_body_ten) + " ";
			string tc4 = Resources.GetString(Resource.String.autopista_pase_diario_tc_body_eleven);

			SpannableString ss1 = new SpannableString(tc1);
			ss1.SetSpan(new ForegroundColorSpan(Resources.GetColor(Resource.Color.servipag_black)), 0, ss1.Length(), 0);
			builder.Append(ss1);

			SpannableString ss2 = new SpannableString(tc2);
			ss2.SetSpan(new ForegroundColorSpan(Resources.GetColor(Resource.Color.servipag_calipso)), 0, ss2.Length(), 0);
			builder.Append(ss2);

			SpannableString ss3 = new SpannableString(tc3);
			ss3.SetSpan(new ForegroundColorSpan(Resources.GetColor(Resource.Color.servipag_black)), 0, ss3.Length(), 0);
			builder.Append(ss3);

			SpannableString ss4 = new SpannableString(tc4);
			ss4.SetSpan(new ForegroundColorSpan(Resources.GetColor(Resource.Color.servipag_calipso)), 0, ss4.Length(), 0);
			builder.Append(ss4);

			return builder;
		}

		private SpannableStringBuilder coloringTextNext() {
			SpannableStringBuilder builder = new SpannableStringBuilder();
			string tc1 = Resources.GetText(Resource.String.autopista_pase_diario_tc_body_twelve) + " ";
			string tc2 = Resources.GetString(Resource.String.autopista_pase_diario_tc_body_thirteen) + " ";
			string tc3 = Resources.GetText(Resource.String.autopista_pase_diario_tc_body_fourteen) + " ";
			string tc4 = Resources.GetString(Resource.String.autopista_pase_diario_tc_body_fifthteen);

			SpannableString ss1 = new SpannableString(tc1);
			ss1.SetSpan(new ForegroundColorSpan(Resources.GetColor(Resource.Color.servipag_black)), 0, ss1.Length(), 0);
			builder.Append(ss1);

			SpannableString ss2 = new SpannableString(tc2);
			ss2.SetSpan(new ForegroundColorSpan(Resources.GetColor(Resource.Color.servipag_calipso)), 0, ss2.Length(), 0);
			builder.Append(ss2);

			SpannableString ss3 = new SpannableString(tc3);
			ss3.SetSpan(new ForegroundColorSpan(Resources.GetColor(Resource.Color.servipag_black)), 0, ss3.Length(), 0);
			builder.Append(ss3);

			SpannableString ss4 = new SpannableString(tc4);
			ss4.SetSpan(new ForegroundColorSpan(Resources.GetColor(Resource.Color.servipag_calipso)), 0, ss4.Length(), 0);
			builder.Append(ss4);

			return builder;
		}
	}
}
