using System;
using System.Collections.Generic;
using Android.Graphics.Drawables;
using Android.OS;
using Android.Support.V4.App;
using Android.Support.V4.Content;
using Android.Support.V7.Widget;
using Android.Views;
using Android.Views.InputMethods;

namespace ServipagMobile.Droid {
	public class FragmentListaServicios : Fragment {
		public ServiciosAdapter adapter;
		public SearchView search;
		private Android.Widget.LinearLayout containerListaServicios;
		private RecyclerView recyclerView;
		private Android.Widget.ImageView buttonLupa;
		private RecyclerView.LayoutManager layoutManager;
		private Drawable divider;
		private RecyclerView.ItemDecoration dividerDecoration;
		private AgregarActivity aa;
		private bool isPagoExpress;
		private bool isAutopista;

		public FragmentListaServicios(bool isPagoExpress, bool isAutopista) {
			this.isPagoExpress = isPagoExpress;
			this.isAutopista = isAutopista;
		}
		public override void OnCreate(Bundle savedInstanceState) {
			base.OnCreate(savedInstanceState);

			this.aa = (AgregarActivity)Activity;
		}

		public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState) {
			return inflater.Inflate(Resource.Layout.FragmentListaServicios, container, false);
		}

		public override void OnViewCreated(View view, Bundle savedInstanceState) {
			base.OnViewCreated(view, savedInstanceState);

			search = view.FindViewById<SearchView>(Resource.Id.searchView);
			containerListaServicios = view.FindViewById<Android.Widget.LinearLayout>(Resource.Id.containerListaServicios);
			if (aa.isFiltered) {
				var filteredList = filter(aa.listaServiciosBillers, aa.filteredText);
				search.SetQuery(aa.filteredText, false);
				adapter = new ServiciosAdapter(filteredList, aa, Resources.GetString(Resource.String.agregar_cta_serv_billers), isPagoExpress, isAutopista);
			} else {
				if (aa.listaBillers.Count == 0) {
					adapter = new ServiciosAdapter(aa.listaServicios, aa, Resources.GetString(Resource.String.agregar_cta_serv_billers), isPagoExpress, isAutopista);
				} else {
					adapter = new ServiciosAdapter(aa.listaBillers, aa, Resources.GetString(Resource.String.agregar_cta_serv_billers), isPagoExpress, isAutopista);
				}
			}

			divider = ContextCompat.GetDrawable(Context, Resource.Drawable.divider);
			dividerDecoration = new CustomItemDecoration(divider);

			buttonLupa = view.FindViewById<Android.Widget.ImageView>(Resource.Id.buttonLupa);
			recyclerView = view.FindViewById<RecyclerView>(Resource.Id.listaServicios);
			recyclerView.SetAdapter(adapter);
			recyclerView.AddItemDecoration(dividerDecoration);

			layoutManager = new LinearLayoutManager(aa);
			recyclerView.SetLayoutManager(layoutManager);

			(search.FindViewById<Android.Widget.EditText>(Resource.Id.search_src_text)).SetTextColor(Resources.GetColor(Resource.Color.servipag_blue));
			search.QueryTextChange += (sender, e) => {
				if (e.NewText == "") {
					adapter.filterList(aa.listaServicios);
					aa.isFiltered = false;
					aa.filteredText = e.NewText;
					aa.idFragment = Resources.GetString(Resource.String.agregar_cta_serv_billers);
				} else {
					var filteredList = filter(aa.listaServiciosBillers, e.NewText);
					adapter.filterList(filteredList);
					aa.isFiltered = true;
					aa.filteredText = e.NewText;
					aa.idFragment = "biller";
				}
			};
			buttonLupa.Click += (sender, e) => {
				InputMethodManager inputManager = (InputMethodManager)Activity.GetSystemService(Android.Content.Context.InputMethodService);
				var currentFocus = Activity.CurrentFocus;
				if (currentFocus != null) {
					inputManager.HideSoftInputFromWindow(search.WindowToken, HideSoftInputFlags.None);
				}
			};
			containerListaServicios.Click += (sender, e) => {
				InputMethodManager inputManager = (InputMethodManager)Activity.GetSystemService(Android.Content.Context.InputMethodService);
				var currentFocus = Activity.CurrentFocus;
				if (currentFocus != null) {
					inputManager.HideSoftInputFromWindow(search.WindowToken, HideSoftInputFlags.None);
				}
			};
			recyclerView.Click += (sender, e) => {
				InputMethodManager inputManager = (InputMethodManager)Activity.GetSystemService(Android.Content.Context.InputMethodService);
				var currentFocus = Activity.CurrentFocus;
				if (currentFocus != null) {
					inputManager.HideSoftInputFromWindow(search.WindowToken, HideSoftInputFlags.None);
				}
			};

			if (isAutopista) {
				var filteredList = filterForAutopista(aa.listaServiciosBillers, "11");
				adapter.filterList(filteredList);
				aa.idFragment = "biller";
			}
		}

		public List<Servicios> filter(List<Servicios> list, String text) {
			List<Servicios> filteredModelList = new List<Servicios>();
			foreach (Servicios data in list) {
				if (data.nombre.ToLower().Contains(text.ToLower())) {
					filteredModelList.Add(data);
				}
			}
			return filteredModelList;
		}

		public List<Servicios> filterForAutopista(List<Servicios> list, String text) {
			List<Servicios> filteredModelList = new List<Servicios>();
			foreach (Servicios data in list) {
				if (data.entidad.Equals("servicio")) {
					if (data.id.Contains(text)) {
						filteredModelList.Add(data);
					}
				} else {
					if (data.id_servicio.Contains(text)) {
						filteredModelList.Add(data);
					}
				}

			}
			return filteredModelList;
		}
	}
}
