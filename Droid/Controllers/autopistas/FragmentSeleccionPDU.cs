using Android.OS;
using Android.Support.Design.Widget;
using Android.Support.V4.App;
using Android.Support.V4.View;
using Android.Views;

namespace ServipagMobile.Droid {
	public class FragmentSeleccionPDU : Fragment {
		private TabLayout tabLayout;
		private ViewPager viewPager;

		private FragmentIngresaDatosPDU ingresaDatosPDU;
		private FragmentIngresaDatosPDT ingresaDatosPDT;
		private FragmentIngresaDatosUPDU ingresaDatosUPDU;
		private FragmentIngresaDatosUPDT ingresaDatosUPDT;
		private FragmentManager SupportFragmentManager;
		private string idBiller;
		private string idServicio;
		private bool isUPDU;
		private bool isLogin;
		private PaseDiario uPDU;
		private PaseTardio uPDT;

		public FragmentSeleccionPDU(FragmentManager sfm, string idBiller, string idServicio, bool isUPDU, bool isLogin, PaseDiario uPDU) {
			SupportFragmentManager = sfm;
			this.idBiller = idBiller;
			this.idServicio = idServicio;
			this.isUPDU = isUPDU;
			this.uPDU = uPDU;
			this.isLogin = isLogin;
		}

		public FragmentSeleccionPDU(FragmentManager sfm, string idBiller, string idServicio, bool isUPDU, bool isLogin, PaseTardio uPDT) {
			SupportFragmentManager = sfm;
			this.idBiller = idBiller;
			this.idServicio = idServicio;
			this.isUPDU = isUPDU;
			this.uPDT = uPDT;
			this.isLogin = isLogin;
		}

		public override void OnCreate(Bundle savedInstanceState) {
			base.OnCreate(savedInstanceState);
		}

		public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState) {
			return inflater.Inflate(Resource.Layout.FragmentSeleccionPDU, container, false);
		}

		public override void OnViewCreated(View view, Bundle savedInstanceState) {
			base.OnViewCreated(view, savedInstanceState);

			viewPager = view.FindViewById<ViewPager>(Resource.Id.viewPagerPDU);
			setupViewPager(viewPager);
			tabLayout = view.FindViewById<TabLayout>(Resource.Id.tabsPDU);
			tabLayout.SetupWithViewPager(viewPager);
		}

		private void initFragment() {
			if (isUPDU) {
				if (uPDU != null) {
					ingresaDatosUPDU = new FragmentIngresaDatosUPDU((PDUActivity)Activity, SupportFragmentManager, uPDU, isLogin, "886", idServicio);
					ingresaDatosPDT = new FragmentIngresaDatosPDT((PDUActivity)Activity, SupportFragmentManager, isLogin, "964", idServicio);
				} else {
					ingresaDatosPDU = new FragmentIngresaDatosPDU((PDUActivity)Activity, SupportFragmentManager, isLogin, "886", idServicio);
					ingresaDatosUPDT = new FragmentIngresaDatosUPDT((PDUActivity)Activity, SupportFragmentManager, uPDT, isLogin, "964", idServicio);
				}
			} else {
				ingresaDatosPDU = new FragmentIngresaDatosPDU((PDUActivity)Activity, SupportFragmentManager, isLogin, "886", idServicio);
				ingresaDatosPDT = new FragmentIngresaDatosPDT((PDUActivity)Activity, SupportFragmentManager, isLogin, "964", idServicio);
			}
		}

		public void setupViewPager(ViewPager viewPager) {
			initFragment();
			GenericFragmentPagerAdapter adapter = new GenericFragmentPagerAdapter(SupportFragmentManager);

			string ttlPDU= Resources.GetString(Resource.String.pdu_tab_pdu);
			string ttlPDT = Resources.GetString(Resource.String.pdu_tab_pdt);
			if (isUPDU) {
				if (uPDU != null) {
					adapter.addFragment(ingresaDatosUPDU, ttlPDU);
					adapter.addFragment(ingresaDatosPDT, ttlPDT);
				} else {
					adapter.addFragment(ingresaDatosPDU, ttlPDU);
					adapter.addFragment(ingresaDatosUPDT, ttlPDT);
				}
			} else {
				adapter.addFragment(ingresaDatosPDU, ttlPDU);
				adapter.addFragment(ingresaDatosPDT, ttlPDT);
			}


			viewPager.Adapter = adapter;

			if (idBiller == "886") {
				viewPager.SetCurrentItem(0, true);
			} else if (idBiller == "964") {
				viewPager.SetCurrentItem(1, true);
			}

		}
	}
}
