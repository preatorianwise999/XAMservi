using System.Collections.Generic;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Support.V7.App;
using Android.Widget;
using Newtonsoft.Json;

namespace ServipagMobile.Droid {
	[Activity(Label = "CustomNumberPicker", Theme="@style/ServipagTransparentTheme")]
	public class CustomNumberPicker : AppCompatActivity {
		private NumberPicker amountPicker;
		private List<MontoRecarga> listMR;
		private List<string> montos = new List<string>();
		private string selectedAmount;


		protected override void OnCreate(Bundle savedInstanceState) {
			base.OnCreate(savedInstanceState);
			SetContentView(Resource.Layout.CustomNumberPicker);

			listMR = JsonConvert.DeserializeObject<List<MontoRecarga>>(Intent.GetStringExtra("listMR"));
			amountPicker = FindViewById<NumberPicker>(Resource.Id.amountPicker);

			amountPicker.MinValue = 0;
			amountPicker.MaxValue = listMR.Count - 1;
			int constant = listMR.Count - 1;
			foreach (MontoRecarga amount in listMR) {
				montos.Add(amount.valor2.ToString());
			}

			selectedAmount = montos[0];
			amountPicker.SetDisplayedValues(montos.ToArray());
			amountPicker.WrapSelectorWheel = false;

			amountPicker.Click += (sender, e) => {
				Intent i = new Intent();
				i.PutExtra("montoSeleccionado", montos[amountPicker.Value]);
				SetResult(Result.Ok, i);
				Finish();
			};
		}

		public override void OnBackPressed() { }
	}
}
