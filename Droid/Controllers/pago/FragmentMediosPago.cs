using System;
using System.Collections.Generic;
using System.Linq;
using Android.OS;
using Android.Support.Design.Widget;
using Android.Support.V4.App;
using Android.Support.V4.View;
using Android.Views;
using Newtonsoft.Json.Linq;

namespace ServipagMobile.Droid {
	public class FragmentMediosPago : Fragment {
		private TabLayout tabLayout;
		private ViewPager viewPager;
		private FragmentManager SupportFragmentManager;

		private List<TiposMediosPago> tabs;
		private List<MediosPago> mediosPago;
		private List<FragmentVistaMP> listFragments = new List<FragmentVistaMP>();
		private List<BuscaDeudas> misDeudas = new List<BuscaDeudas>();
		private List<string> listTtlTabs = new List<string>();
		private SolicitaRecarga solicitaRecarga;
		private bool isLogin;
		private bool isUR;

		private BuscaDeudas deudaPDU;
		private bool isUPDU;

		public FragmentMediosPago() { }

		public FragmentMediosPago(FragmentManager sfm, List<MediosPago> mediosPago, List<BuscaDeudas> misDeudas, bool isLogin) {
			SupportFragmentManager = sfm;
			this.mediosPago = mediosPago;
			this.tabs = Properties.GetInstance().tiposMediosPago;
			this.misDeudas = misDeudas;
			this.isLogin = isLogin;
		}

		public FragmentMediosPago(FragmentManager sfm, List<MediosPago> mediosPago, SolicitaRecarga solicitaRecarga, bool isUR, bool isLogin) {
			SupportFragmentManager = sfm;
			this.mediosPago = mediosPago;
			this.solicitaRecarga = solicitaRecarga;
			this.tabs = Properties.GetInstance().tiposMediosPago;
			this.isLogin = isLogin;
			this.isUR = isUR;
		}

		public FragmentMediosPago(FragmentManager sfm, List<MediosPago> mediosPago, BuscaDeudas deudaPDU, bool isUPDU, bool isLogin) {
			SupportFragmentManager = sfm;
			this.mediosPago = mediosPago;
			this.deudaPDU = deudaPDU;
			this.tabs = Properties.GetInstance().tiposMediosPago;
			this.isLogin = isLogin;
			this.isUPDU = isUPDU;
		}

		public override void OnCreate(Bundle savedInstanceState) {
			base.OnCreate(savedInstanceState);
		}

		public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState) {
			return inflater.Inflate(Resource.Layout.FragmentMediosPago, container, false);
		}

		public override void OnViewCreated(View view, Bundle savedInstanceState) {
			base.OnViewCreated(view, savedInstanceState);

			viewPager = view.FindViewById<ViewPager>(Resource.Id.viewPager);
			setupViewPager(viewPager);
			tabLayout = view.FindViewById<TabLayout>(Resource.Id.tabs);
			tabLayout.SetupWithViewPager(viewPager);
		}

		private void initFragment() {
			if (solicitaRecarga != null) {
				for (int i = 0; i < tabs.Count; i++) {
					listFragments.Add(new FragmentVistaMP(
						tabs[i].tipoVista,
						Properties.GetInstance().url,
						setDifMediosPago(tabs[i].id), solicitaRecarga, isLogin, isUR, mediosPago));
					listTtlTabs.Add(tabs[i].nombreTab);
				}
			} else if (deudaPDU != null) {
				for (int i = 0; i < tabs.Count; i++) {
					listFragments.Add(new FragmentVistaMP(
						tabs[i].tipoVista,
						Properties.GetInstance().url,
						setDifMediosPago(tabs[i].id), deudaPDU, isLogin, isUPDU, mediosPago));
					listTtlTabs.Add(tabs[i].nombreTab);
				}
			} else {
				for (int i = 0; i < tabs.Count; i++) {
					listFragments.Add(new FragmentVistaMP(
						tabs[i].tipoVista,
						Properties.GetInstance().url,
						setDifMediosPago(tabs[i].id), misDeudas, isLogin, mediosPago));
					listTtlTabs.Add(tabs[i].nombreTab);
				}
			}
		}

		private List<MediosPago> setDifMediosPago(string id) {
			return mediosPago.Where(x => x.forma_pago.Equals(Convert.ToInt32(id))).ToList();
		}

		public void setupViewPager(ViewPager viewPager) {
			initFragment();
			GenericFragmentPagerAdapter adapter = new GenericFragmentPagerAdapter(SupportFragmentManager);

			for (int i = 0; i < listFragments.Count; i++) {
				adapter.addFragment(listFragments[i], listTtlTabs[i]);
			}

			viewPager.Adapter = adapter;
		}
	}
}
