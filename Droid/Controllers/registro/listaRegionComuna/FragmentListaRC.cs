using System.Collections.Generic;
using Android.Graphics.Drawables;
using Android.OS;
using Android.Support.V4.App;
using Android.Support.V4.Content;
using Android.Support.V7.Widget;
using Android.Util;
using Android.Views;
using Android.Widget;

namespace ServipagMobile.Droid {
	public class FragmentListaRC : Fragment {
		private RecyclerView recyclerView;
		private RecyclerView.LayoutManager layoutManager;
		private RegionComunaAdapter adapter;
		private Drawable divider;
		private RecyclerView.ItemDecoration dividerDecoration;
		private List<RegionComuna> regionComuna = new List<RegionComuna>();

		public override void OnCreate(Bundle savedInstanceState) {
			base.OnCreate(savedInstanceState);
		}

		public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState) {
			return inflater.Inflate(Resource.Layout.FragmentListaRC, container, false);
		}

		public override void OnViewCreated(View view, Bundle savedInstanceState) {
			base.OnViewCreated(view, savedInstanceState);

			if (RegisterData.GetInstance().listType == "region") {
				regionComuna = ListadoRegion.GetInstance().listaRegiones;
			} else {
				regionComuna = ListadoComuna.GetInstance().listaComunas;
			}

			adapter = new RegionComunaAdapter(regionComuna, Resources.GetString(Resource.String.registro_id_regCom));

			divider = ContextCompat.GetDrawable(Context, Resource.Drawable.divider);
			dividerDecoration = new CustomItemDecoration(divider);

			recyclerView = view.FindViewById<RecyclerView>(Resource.Id.listaRC);
			recyclerView.SetAdapter(adapter);
			recyclerView.AddItemDecoration(dividerDecoration);
			layoutManager = new LinearLayoutManager((RegistroActivity)RegisterData.GetInstance().activity);

			recyclerView.SetLayoutManager(layoutManager);
		}
	}
}
