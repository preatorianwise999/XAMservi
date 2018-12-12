using System.Globalization;
using Android.Content;
using Android.Graphics.Drawables;
using Android.OS;
using Android.Support.V4.App;
using Android.Support.V4.Content;
using Android.Support.V7.Widget;
using Android.Views;
using Android.Widget;
using Java.Lang;

namespace ServipagMobile.Droid {
	public class FragmentVoucher : Fragment {
		private TextView clientName, paymentDay, paymentHour, paymentNumber, paymentType, hintTrx;
		private RecyclerView listVoucher;
		private Button bttnDone;
		private VoucherData vData;

		private VoucherAdapter adapter;
		private RecyclerView.LayoutManager layoutManager;
		private Drawable divider;
		private RecyclerView.ItemDecoration dividerDecoration;
		private CultureInfo culture { get; set; }

		public FragmentVoucher(VoucherData vData) {
			this.vData = vData;
			this.culture = new CultureInfo("es-CL");
		}

		public override void OnCreate(Bundle savedInstanceState) {
			base.OnCreate(savedInstanceState);

			// Create your fragment here
		}

		public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState) {
			return inflater.Inflate(Resource.Layout.FragmentVoucher, container, false);
		}

		public override void OnViewCreated(View view, Bundle savedInstanceState) {
			base.OnViewCreated(view, savedInstanceState);

			clientName = view.FindViewById<TextView>(Resource.Id.clientName);
			paymentDay = view.FindViewById<TextView>(Resource.Id.paymentDay);
			paymentHour = view.FindViewById<TextView>(Resource.Id.paymentHour);
			paymentNumber = view.FindViewById<TextView>(Resource.Id.paymentNumber);
			paymentType = view.FindViewById<TextView>(Resource.Id.paymentType);
			hintTrx = view.FindViewById<TextView>(Resource.Id.hintTrx);
			listVoucher = view.FindViewById<RecyclerView>(Resource.Id.listVoucher);
			bttnDone = view.FindViewById<Button>(Resource.Id.bttnDone);

			clientName.Text = vData.clientName;
			paymentDay.Text = vData.date;
			paymentHour.Text = vData.hour;
			paymentNumber.Text = vData.nRequest;
			paymentType.Text = vData.paymentType;
			hintTrx.Text = "Con fecha " + vData.date + " hemos procedido a realizar una transacción en " +
				vData.paymentType + " que alcanza un total de " + Integer.ParseInt(vData.amount).ToString("C", culture) +
				" por concepto de la(s) siguiente(s) cuenta(s):";

			adapter = new VoucherAdapter(vData.voucherDetail);
			divider = ContextCompat.GetDrawable(Context, Resource.Drawable.divider);
			dividerDecoration = new CustomItemDecoration(divider);
			listVoucher.SetAdapter(adapter);
			listVoucher.AddItemDecoration(dividerDecoration);

			layoutManager = new LinearLayoutManager((ComprobanteActivity)Activity);
			listVoucher.SetLayoutManager(layoutManager);

			bttnDone.Click += (sender, e) => {
				Intent intent = new Intent();
				intent.PutExtra("recargas", "closeRecargas");
				((ComprobanteActivity)Activity).SetResult(Android.App.Result.Ok, intent);
				((ComprobanteActivity)Activity).Finish();
			};
		}
	}
}
