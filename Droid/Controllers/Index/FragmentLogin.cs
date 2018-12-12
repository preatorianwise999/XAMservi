using System;
using Android.OS;
using Android.Support.Design.Widget;
using Android.Support.V4.App;
using Android.Support.V4.View;
using Android.Views;

namespace ServipagMobile.Droid {
	public class FragmentLogin : Fragment {
		private TabLayout tabLayout;
		private ViewPager viewPager;

		private FragmentPagoExpress pagoExpress;
		private FragmentInicioSesion inicioSesion;
		private FragmentManager SupportFragmentManager;
		private MainActivity ma;

		public FragmentLogin() { }
		public FragmentLogin(FragmentManager sfm, MainActivity ma) {
			SupportFragmentManager = sfm;
			this.ma = ma;
		}

		public override void OnCreate(Bundle savedInstanceState) {
			base.OnCreate(savedInstanceState);
		}

		public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState) {
			return inflater.Inflate(Resource.Layout.FragmentLogin, container, false);
		}

		public override void OnViewCreated(View view, Bundle savedInstanceState) {
			base.OnViewCreated(view, savedInstanceState);

			viewPager = view.FindViewById<ViewPager>(Resource.Id.viewPager);
			setupViewPager(viewPager);
			if (ma.tabSelected == "is") {
				viewPager.SetCurrentItem(1, true);
			} else if (ma.tabSelected == "pe") {
				viewPager.SetCurrentItem(0, true);
			}
			tabLayout = view.FindViewById<TabLayout>(Resource.Id.tabs);
			tabLayout.SetupWithViewPager(viewPager);

			viewPager.PageSelected += (sender, e) => {
				if (ma.tabSelected == "is") {
					ma.tabSelected = "pe";
				} else if (ma.tabSelected == "pe") {
					ma.tabSelected = "is";
				}
			};
		}


		private void initFragment() {
			pagoExpress = new FragmentPagoExpress();
			inicioSesion = new FragmentInicioSesion();
		}

		public void setupViewPager(ViewPager viewPager) {
			initFragment();
			GenericFragmentPagerAdapter adapter = new GenericFragmentPagerAdapter(SupportFragmentManager);

			string ttlPagoExpress = Resources.GetString(Resource.String.title_tab_express);
			string ttlInicioSesion = Resources.GetString(Resource.String.title_tab_sesion);

			adapter.addFragment(pagoExpress, ttlPagoExpress);
			adapter.addFragment(inicioSesion, ttlInicioSesion);

			viewPager.Adapter = adapter;
		}
	}
}
