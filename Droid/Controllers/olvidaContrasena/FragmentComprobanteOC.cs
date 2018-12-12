using Android.OS;
using Android.Support.V4.App;
using Android.Views;
using Android.Widget;

namespace ServipagMobile.Droid {
	public class FragmentComprobanteOC : Fragment {
		private TextView mssg;
		private string mensaje;

		public FragmentComprobanteOC(string mensaje) {
			this.mensaje = mensaje;
		}

		public override void OnCreate(Bundle savedInstanceState) {
			base.OnCreate(savedInstanceState);
		}

		public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState) {
			return inflater.Inflate(Resource.Layout.FragmentComprobanteOC, container, false);
		}

		public override void OnViewCreated(View view, Bundle savedInstanceState) {
			base.OnViewCreated(view, savedInstanceState);

			mssg = view.FindViewById<TextView>(Resource.Id.msjUnoOC);
			mssg.Text = Resources.GetString(Resource.String.occontrasena_msj_success_one) + " " + mensaje +
				Resources.GetString(Resource.String.occontrasena_msj_success_two);
		}
	}
}
