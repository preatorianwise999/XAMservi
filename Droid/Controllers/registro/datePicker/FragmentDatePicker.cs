using System;
using Android.App;
using Android.Icu.Util;
using Android.OS;
using Android.Widget;

namespace ServipagMobile.Droid {
	public class FragmentDatePicker : Android.Support.V4.App.DialogFragment, DatePickerDialog.IOnDateSetListener {
		public static readonly string TAG = "datepicker";
		public string parent;

		Action<DateTime> dateSelectedHandler = delegate { };

		public static FragmentDatePicker NewInstance(Action<DateTime> onDateSelected) {
			FragmentDatePicker frag = new FragmentDatePicker();
			frag.dateSelectedHandler = onDateSelected;
			return frag;
		}

		public override Dialog OnCreateDialog(Bundle savedInstanceState) {
			DateTime currently = DateTime.Now;
			DatePickerDialog dialog;
			Calendar calendarMin;
			Calendar calendarMax;

			if (parent.Equals("PDU")) {
				dialog = new DatePickerDialog(Activity, Resource.Style.AlertDialogCustom,
											  this,
											  currently.Year,
											  currently.Month - 1,
											  currently.Day);
				calendarMin = Calendar.Instance;
				calendarMin.Set(currently.Year, currently.Month - 1, currently.Day - 2);
				calendarMax = Calendar.Instance;
				calendarMax.Set(currently.Year, currently.Month - 1, currently.Day + 30);

				long minDate = calendarMin.TimeInMillis;
				long maxDate = calendarMax.TimeInMillis;

				dialog.DatePicker.MinDate = minDate;
				dialog.DatePicker.MaxDate = maxDate;
			} else if (parent.Equals("PDT")) {
				dialog = new DatePickerDialog(Activity, Resource.Style.AlertDialogCustom,
											  this,
											  currently.Year,
											  currently.Month - 1,
											  currently.Day);
				calendarMin = Calendar.Instance;
				calendarMin.Set(currently.Year, currently.Month - 1, currently.Day - 20);
				calendarMax = Calendar.Instance;
				calendarMax.Set(currently.Year, currently.Month - 1, currently.Day - 3);

				long minDate = calendarMin.TimeInMillis;
				long maxDate = calendarMax.TimeInMillis;

				dialog.DatePicker.MinDate = minDate;
				dialog.DatePicker.MaxDate = maxDate;
			} else {
				dialog = new DatePickerDialog(Activity, Resource.Style.AlertDialogCustom,
											  this,
											  currently.Year - 31,
											  currently.Month,
											  currently.Day);
			}
			 
			return dialog;
		}

		public void OnDateSet(DatePicker view, int year, int monthOfYear, int dayOfMonth) {
			DateTime selectedDate = new DateTime(year, monthOfYear + 1, dayOfMonth);
			dateSelectedHandler(selectedDate);
		}
	}
}
