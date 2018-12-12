using System.Collections.Generic;
using Android.Support.V7.Widget;
using Android.Views;

namespace ServipagMobile.Droid {
	public class ServiciosAdapter : RecyclerView.Adapter {
		public List<Servicios> listaServicios;
		private AgregarActivity aa;
		private string nameFragment;
		private bool isPagoExpress;
		private bool isAutopista;

		public ServiciosAdapter(List<Servicios> l, AgregarActivity aa, string nf, bool isPagoExpress, bool isAutopista) {
			this.listaServicios = l;
			this.aa = aa;
			this.nameFragment = nf;
			this.isPagoExpress = isPagoExpress;
			this.isAutopista = isAutopista;
		}

		public override int ItemCount {
			get {
				return listaServicios.Count;
			}
		}

		public override void OnBindViewHolder(RecyclerView.ViewHolder holder, int position) {
			ServiciosViewHolder vh = holder as ServiciosViewHolder;
			vh.nombre.Text = listaServicios[position].nombre;
		}

		public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType) {
			ServiciosViewHolder vh;
			View itemView = LayoutInflater.From(parent.Context).Inflate(Resource.Layout.RowServicios, parent, false);
			vh = new ServiciosViewHolder(itemView, aa, this, isPagoExpress, isAutopista);

			return vh;
		}

		public void filterList(List<Servicios> l) {
			listaServicios = new List<Servicios>();
			listaServicios.AddRange(l);
			NotifyDataSetChanged();
		}
	}
}
