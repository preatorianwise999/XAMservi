using System;
using System.Collections.Generic;
using System.Globalization;
using Android.Support.V7.Widget;
using Android.Views;

namespace ServipagMobile.Droid {
	public class RecargasAdapter : RecyclerView.Adapter {
		private SolicitaRecarga solicitaRecarga;
		private RecargasActivity ra;
		private FragmentListaRecargas flr;
		private bool isSelected = true;
		private CultureInfo culture { get; set; }

		public RecargasAdapter() { }

		public RecargasAdapter(SolicitaRecarga solicitaRecarga, RecargasActivity ra, FragmentListaRecargas flr) {
			this.solicitaRecarga = solicitaRecarga;
			this.ra = ra;
			this.flr = flr;
			this.culture = new CultureInfo("es-CL");
		}


		public override int ItemCount {
			get {
				return 1;
			}
		}

		public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType) {
			View itemView = LayoutInflater.From(parent.Context).Inflate(Resource.Layout.RowRecargas, parent, false);
			RecargasViewHolder vh = new RecargasViewHolder(itemView);

			return vh;
		}

		public override void OnBindViewHolder(RecyclerView.ViewHolder holder, int position) {
			RecargasViewHolder vh = holder as RecargasViewHolder;

			vh.nombreBiller.Text = solicitaRecarga.nombreBiller;
			vh.montoTotalRecarga.Text = solicitaRecarga.monto_total.ToString("C", culture);
			vh.idRecarga.Text = solicitaRecarga.identificador.Substring(1, solicitaRecarga.identificador.Length - 1);
			vh.tipo.Text = "Recarga";

			vh.selectAccount.Click += (sender, e) => {
				if (isSelected) {
					int m = 0;
					isSelected = false;
					vh.selectAccount.SetImageResource(Resource.Drawable.sin_seleccion_home);
					flr.montoTotal.Text = "Total: " + m.ToString("C", culture);
				} else {
					isSelected = true;
					vh.selectAccount.SetImageResource(Resource.Drawable.seleccion_home);
					flr.montoTotal.Text = "Total: " + solicitaRecarga.monto_total.ToString("C", culture);
				}
			};
		}
	}
}
