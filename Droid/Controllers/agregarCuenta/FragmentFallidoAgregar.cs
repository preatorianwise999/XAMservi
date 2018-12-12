using Android.OS;
using Android.Support.V4.App;
using Android.Views;

namespace ServipagMobile.Droid {
	public class FragmentFallidoAgregar : Fragment {
		private Android.Widget.TextView mensajeError1, mensajeError2;
		private string mensaje;
		private bool isAdd;

		public FragmentFallidoAgregar(string mensaje, bool isAdd) {
			this.mensaje = mensaje;
			this.isAdd = isAdd;
		}

		public override void OnCreate(Bundle savedInstanceState) {
			base.OnCreate(savedInstanceState);
		}

		public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState) {
			return inflater.Inflate(Resource.Layout.FragmentFallidoAgregar, container, false);
		}

		public override void OnViewCreated(View view, Bundle savedInstanceState) {
			base.OnViewCreated(view, savedInstanceState);

			mensajeError1 = view.FindViewById<Android.Widget.TextView>(Resource.Id.subTitleCA);
			mensajeError2 = view.FindViewById<Android.Widget.TextView>(Resource.Id.subTitle2CA);

			if (!isAdd) {
				mensajeError1.Visibility = ViewStates.Gone;
			}
			mensajeError2.Text = mensaje;
		}
	}
}
