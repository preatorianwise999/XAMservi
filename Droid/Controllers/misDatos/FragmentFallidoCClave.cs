using Android.OS;
using Android.Support.V4.App;
using Android.Views;

namespace ServipagMobile.Droid {
	public class FragmentFallidoCClave : Fragment {
		private string mensaje;

		public FragmentFallidoCClave(string mensaje) {
			this.mensaje = mensaje;
		}

		public override void OnCreate(Bundle savedInstanceState) {
			base.OnCreate(savedInstanceState);
		}

		public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState) {
			return inflater.Inflate(Resource.Layout.FragmentFallidoCClave, container, false);
		}

		public override void OnViewCreated(View view, Bundle savedInstanceState) {
			base.OnViewCreated(view, savedInstanceState);
		}
	}
}
