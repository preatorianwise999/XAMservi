using Android.OS;
using Android.Support.V4.App;
using Android.Views;
using Android.Widget;

namespace ServipagMobile.Droid {
	public class FragmentNoVoucher : Fragment {
		private bool isError;
		private string idPago;
		private TextView titleCP, subTitleCP;

		public FragmentNoVoucher() {}

		public FragmentNoVoucher(bool isError, string idPago) {
			this.isError = isError;
			this.idPago = idPago;
			
		}
		public override void OnCreate(Bundle savedInstanceState) {
			base.OnCreate(savedInstanceState);
		}

		public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState) {
			return inflater.Inflate(Resource.Layout.FragmentNoVoucher, container, false);
		}

		public override void OnViewCreated(View view, Bundle savedInstanceState) {
			base.OnViewCreated(view, savedInstanceState);
			titleCP = view.FindViewById<TextView>(Resource.Id.titleCP);
			subTitleCP = view.FindViewById<TextView>(Resource.Id.subTitleCP);
			((ComprobanteActivity)Activity).downloadVoucher.Visibility = ViewStates.Gone;

			titleCP.Text = Resources.GetString(Resource.String.pago_comprobante_title);
			if (!isError) {
				subTitleCP.Text = Resources.GetString(Resource.String.pago_comprobante_body_without_confirmation) + "\n\n" +
					Resources.GetString(Resource.String.pago_comprobante_n_consulta) + " " + idPago;
			} else {
				subTitleCP.Text = Resources.GetString(Resource.String.pago_comprobante_body_error);
			}
		}
	}
}
