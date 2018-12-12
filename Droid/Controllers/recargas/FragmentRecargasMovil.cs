﻿using System.Collections.Generic;
using Android.OS;
using Android.Support.V4.App;
using Android.Support.V7.Widget;
using Android.Views;

namespace ServipagMobile.Droid {
	public class FragmentRecargasMovil : Fragment {
		private List<ServiciosRecarga> listSRMovil;
		private RecyclerView listEmpresasRecarga;
		private ServiciosRecargaAdapter adapter;
		private RecyclerView.LayoutManager layoutManager;
		private bool isLogin;

		public FragmentRecargasMovil() { }

		public FragmentRecargasMovil(List<ServiciosRecarga> listSRMovil, bool isLogin) {
			this.listSRMovil = listSRMovil;
			this.isLogin = isLogin;
		}

		public override void OnCreate(Bundle savedInstanceState) {
			base.OnCreate(savedInstanceState);
		}

		public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState) {
			return inflater.Inflate(Resource.Layout.FragmentRecargasMovil, container, false);
		}

		public override void OnViewCreated(View view, Bundle savedInstanceState) {
			base.OnViewCreated(view, savedInstanceState);

			listEmpresasRecarga = view.FindViewById<RecyclerView>(Resource.Id.listEmpresasRecarga);
			adapter = new ServiciosRecargaAdapter(listSRMovil, (RecargasActivity)Activity, isLogin);
			listEmpresasRecarga.SetAdapter(adapter);

			layoutManager = new LinearLayoutManager(Context);
			listEmpresasRecarga.SetLayoutManager(layoutManager);
		}
	}
}