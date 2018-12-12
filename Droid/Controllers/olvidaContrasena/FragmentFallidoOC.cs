using Android.OS;
using Android.Support.V4.App;
using Android.Views;

namespace ServipagMobile.Droid {
	public class FragmentFallidoOC : Fragment {
		private Android.Widget.TextView mensajeError;
		private string mensaje;

		public FragmentFallidoOC(string mensaje) {
			this.mensaje = mensaje;
		}

		public override void OnCreate(Bundle savedInstanceState) {
			base.OnCreate(savedInstanceState);
		}

		public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState) {
			return inflater.Inflate(Resource.Layout.FragmentFallidoOC, container, false);
		}

		public override void OnViewCreated(View view, Bundle savedInstanceState) {
			base.OnViewCreated(view, savedInstanceState);

			mensajeError = view.FindViewById<Android.Widget.TextView>(Resource.Id.subTitleOC);
			mensajeError.Text = mensaje;
		}
	}
}
