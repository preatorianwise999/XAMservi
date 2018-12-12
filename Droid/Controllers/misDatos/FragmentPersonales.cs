using Android.OS;
using Android.Support.V4.App;
using Android.Util;
using Android.Views;
using Android.Widget;
using Com.Bumptech.Glide;

namespace ServipagMobile.Droid {
	public class FragmentPersonales : Fragment {
		private ImageView userAvatar;
		private TextView userName;
		private TextView userId;
		private TextView region;
		private TextView comuna;
		private TextView email;
		private TextView fechaNacimiento;
		private MainActivity ma;


		public override void OnCreate(Bundle savedInstanceState) {
			base.OnCreate(savedInstanceState);
			ma = (MainActivity)Activity;
		}

		public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState) {
			return inflater.Inflate(Resource.Layout.FragmentPersonales, container, false);
		}

		public override void OnViewCreated(View view, Bundle savedInstanceState) {
			base.OnViewCreated(view, savedInstanceState);

			userAvatar = view.FindViewById<ImageView>(Resource.Id.userAvatar);
			userName = view.FindViewById<TextView>(Resource.Id.userName);
			userId = view.FindViewById<TextView>(Resource.Id.userId);
			region = view.FindViewById<TextView>(Resource.Id.region);
			comuna = view.FindViewById<TextView>(Resource.Id.comuna);
			email = view.FindViewById<TextView>(Resource.Id.email);
			fechaNacimiento = view.FindViewById<TextView>(Resource.Id.fechaNacimiento);

			Glide.With(this)
			 .Load("https://www.smashingmagazine.com/wp-content/uploads/2015/06/10-dithering-opt.jpg")
			 .Transform(new CircleTransform(ma))
			 .Into(userAvatar);

			userName.Text = UserData.GetInstance().nombre;
			userId.Text = UserData.GetInstance().rutShow;
			region.Text = ListadoRegion.GetInstance().listaRegiones.Find(
				x => x.id == UserData.GetInstance().region.ToString()).nombre;
			comuna.Text = ListadoComuna.GetInstance().listaComunas.Find(
				x => x.id == UserData.GetInstance().comuna.ToString()).nombre;
			email.Text = UserData.GetInstance().correo;
			fechaNacimiento.Text = UserData.GetInstance().cumpleanos;
		}
	}
}
