using Android.Content;
using Android.OS;
using Android.Support.V4.App;
using Android.Views;
using Android.Widget;
using Newtonsoft.Json;

namespace ServipagMobile.Droid {
	public class FragmentComprobanteAgregar : Fragment {
		private Button addAnother;
		private Button pay;
		private TextView subTitleCA;
		private ComprobanteActivity ca;
		private bool isPagoExpress;
		private bool isAdd;

		public FragmentComprobanteAgregar(bool isPagoExpress, bool isAdd) {
			this.isPagoExpress = isPagoExpress;
			this.isAdd = isAdd;
		}

		public override void OnCreate(Bundle savedInstanceState) {
			base.OnCreate(savedInstanceState);
			this.ca = (ComprobanteActivity)Activity;
		}

		public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState) {
			return inflater.Inflate(Resource.Layout.FragmentComprobanteAgregar, container, false);
		}

		public override void OnViewCreated(View view, Bundle savedInstanceState) {
			base.OnViewCreated(view, savedInstanceState);

			subTitleCA = view.FindViewById<TextView>(Resource.Id.subTitleCA);
			addAnother = view.FindViewById<Button>(Resource.Id.addAnotherCA);
			pay = view.FindViewById<Button>(Resource.Id.payCA);

			var metrics = Resources.DisplayMetrics;

			if (isPagoExpress) {
				addAnother.SetWidth(metrics.WidthPixels / 2);
				pay.SetWidth(metrics.WidthPixels / 2);

				pay.Click += (sender, e) => {
					Intent intent = new Intent();
					intent.PutExtra("actionComprobanteAgregar", "mCarroCompra");
					ca.SetResult(Android.App.Result.Ok, intent);
					ca.Finish();
				};
			} else {
				addAnother.SetWidth(metrics.WidthPixels);
				addAnother.SetBackgroundColor(Resources.GetColor(Resource.Color.servipag_yellow));
				addAnother.SetTextColor(Resources.GetColor(Resource.Color.servipag_blue));

				pay.Visibility = ViewStates.Gone;
			}

			if (!isAdd) {
				addAnother.Visibility = ViewStates.Gone;
				pay.Visibility = ViewStates.Gone;
				subTitleCA.Text = Resources.GetString(Resource.String.editar_cta_success_desc);
			}


			addAnother.Click += (sender, e) => {
				Intent intent = new Intent();
				intent.PutExtra("actionComprobanteAgregar", "agregaNuevo");
				ca.SetResult(Android.App.Result.Ok, intent);
				ca.Finish();
			};
		}
	}
}
