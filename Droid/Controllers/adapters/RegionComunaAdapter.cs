using System.Collections.Generic;
using Android.Support.V7.Widget;
using Android.Views;

namespace ServipagMobile.Droid {
	public class RegionComunaAdapter : RecyclerView.Adapter {
		public List<RegionComuna> listaRC;
		private string nameFragment;

		public RegionComunaAdapter(List<RegionComuna> l, string nf) {
			this.listaRC = l;
			this.nameFragment = nf;
		}

		public override int ItemCount {
			get {
				return listaRC.Count;
			}
		}

		public override void OnBindViewHolder(RecyclerView.ViewHolder holder, int position) {
			RegionComunaViewHolder vh = holder as RegionComunaViewHolder;
			vh.nameRC.Text = listaRC[position].nombre;
		}

		public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType) {
			View itemView = LayoutInflater.From(parent.Context).Inflate(Resource.Layout.RowRegionComuna, parent, false);
			RegionComunaViewHolder vh = new RegionComunaViewHolder(itemView, nameFragment, listaRC);
			return vh;
		}
	}
}
