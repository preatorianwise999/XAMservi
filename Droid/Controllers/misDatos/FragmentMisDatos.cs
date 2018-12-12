using Android.OS;
using Android.Support.Design.Widget;
using Android.Support.V4.App;
using Android.Support.V4.View;
using Android.Views;

namespace ServipagMobile.Droid {
	public class FragmentMisDatos : Fragment {
		private TabLayout tabLayout;
		private ViewPager viewPager;

		private FragmentPersonales datosPersonales;
		private FragmentCambiaClave cambiarClave;
		private FragmentManager SupportFragmentManager;

		public FragmentMisDatos(FragmentManager sfm) {
			SupportFragmentManager = sfm;
		}

		public override void OnCreate(Bundle savedInstanceState) {
			base.OnCreate(savedInstanceState);
		}

		public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState) {
			return inflater.Inflate(Resource.Layout.FragmentMisDatos, container, false);
		}

		public override void OnViewCreated(View view, Bundle savedInstanceState) {
			base.OnViewCreated(view, savedInstanceState);

			viewPager = view.FindViewById<ViewPager>(Resource.Id.viewPagerMD);
			setupViewPager(viewPager);
			tabLayout = view.FindViewById<TabLayout>(Resource.Id.tabsMD);
			tabLayout.SetupWithViewPager(viewPager);
		}

		private void initFragment() {
			datosPersonales = new FragmentPersonales();
			cambiarClave = new FragmentCambiaClave(viewPager);
		}

		public void setupViewPager(ViewPager viewPager) {
			initFragment();
			GenericFragmentPagerAdapter adapter = new GenericFragmentPagerAdapter(SupportFragmentManager);

			string ttlPagoExpress = Resources.GetString(Resource.String.mdatos_tab_personales);
			string ttlInicioSesion = Resources.GetString(Resource.String.mdatos_tab_cambia_clave);

			adapter.addFragment(datosPersonales, ttlPagoExpress);
			adapter.addFragment(cambiarClave, ttlInicioSesion);

			viewPager.Adapter = adapter;
		}
	}
}
