using System.Collections.Generic;
using Android.OS;
using Android.Support.Design.Widget;
using Android.Support.V4.App;
using Android.Support.V4.View;
using Android.Views;

namespace ServipagMobile.Droid {
	public class FragmentNuevaRecarga : Fragment {
		private TabLayout tabsNuevaRecarga;
		private ViewPager vpNuevaRecarga;

		private FragmentRecargasMovil frMovil;
		private FragmentRecargasFijo frFijo;
		private FragmentManager SupportFragmentManager;
		private List<ServiciosRecarga> listSRMovil;
		private List<ServiciosRecarga> listSRFijo;
		private bool isLogin;

		public FragmentNuevaRecarga() { }

		public FragmentNuevaRecarga(FragmentManager sfm, List<ServiciosRecarga> listSRMovil, List<ServiciosRecarga> listSRFijo, bool isLogin) {
			this.SupportFragmentManager = sfm;
			this.listSRMovil = listSRMovil;
			this.listSRFijo = listSRFijo;
			this.isLogin = isLogin;
		}

		public override void OnCreate(Bundle savedInstanceState) {
			base.OnCreate(savedInstanceState);
		}

		public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState) {
			return inflater.Inflate(Resource.Layout.FragmentNuevaRecarga, container, false);
		}

		public override void OnViewCreated(View view, Bundle savedInstanceState) {
			base.OnViewCreated(view, savedInstanceState);

			vpNuevaRecarga = view.FindViewById<ViewPager>(Resource.Id.vpNuevaRecarga);
			setupViewPager(vpNuevaRecarga);
			tabsNuevaRecarga = view.FindViewById<TabLayout>(Resource.Id.tabsNuevaRecarga);
			tabsNuevaRecarga.SetupWithViewPager(vpNuevaRecarga);
		}

		private void initFragment() {
			frMovil = new FragmentRecargasMovil(listSRMovil, isLogin);
			frFijo = new FragmentRecargasFijo(listSRFijo, isLogin);
		}

		public void setupViewPager(ViewPager viewPager) {
			initFragment();
			GenericFragmentPagerAdapter adapter = new GenericFragmentPagerAdapter(SupportFragmentManager);

			string ttlCM = Resources.GetString(Resource.String.recargas_nr_title_celular);
			string ttlFTV = Resources.GetString(Resource.String.recargas_nr_title_fijo);

			adapter.addFragment(frMovil, ttlCM);
			adapter.addFragment(frFijo, ttlFTV);

			viewPager.Adapter = adapter;
		}
	}
}
