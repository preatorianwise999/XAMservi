using System;
using System.Collections.Generic;
using System.Globalization;
using Android.Support.V7.Widget;
using Android.Views;

namespace ServipagMobile.Droid {
	public class UltimasRecargasAdapter : RecyclerView.Adapter {
		private List<Recargas> listUR;
		private List<SolicitaRecarga> listURPE;
		private int count = 0;
		private int positionSelected = 0;
		private CultureInfo culture { get; set; }

		public UltimasRecargasAdapter() { }

		public UltimasRecargasAdapter(List<Recargas> listUR) {
			this.listUR = listUR;
			this.count = listUR.Count;
			this.culture = new CultureInfo("es-CL");
		}

		public UltimasRecargasAdapter(List<SolicitaRecarga> listURPE) {
			this.listURPE = listURPE;
			this.count = listURPE.Count;
			this.culture = new CultureInfo("es-CL");
		}

		public override int ItemCount {
			get {
				return count;
			}
		}

		public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType) {
			View itemView = LayoutInflater.From(parent.Context).Inflate(Resource.Layout.RowUltimasRecargas, parent, false);
			UltimasRecargasViewHolder vh = new UltimasRecargasViewHolder(itemView);

			return vh;
		}

		public override void OnBindViewHolder(RecyclerView.ViewHolder holder, int position) {
			UltimasRecargasViewHolder vh = holder as UltimasRecargasViewHolder;

			if (listURPE == null) {
				if (listUR[position].isSelected) {
					vh.selectAccount.SetImageResource(Resource.Drawable.seleccion_home);
				} else {
					vh.selectAccount.SetImageResource(Resource.Drawable.sin_seleccion_home);
				}
				vh.nombreBiller.Text = listUR[position].nombre_biller;
				vh.montoTotalRecarga.Text = listUR[position].monto_pago.ToString("C", culture);
				vh.tipo.Text = listUR[position].alias;
				vh.idURecarga.Text = listUR[position].codigo_identificacion.Substring(1, listUR[position].codigo_identificacion.Length-1);
			} else {
				if (listURPE[position].isSelected) {
					vh.selectAccount.SetImageResource(Resource.Drawable.seleccion_home);
				} else {
					vh.selectAccount.SetImageResource(Resource.Drawable.sin_seleccion_home);
				}
				vh.nombreBiller.Text = listURPE[position].nombreBiller;
				vh.montoTotalRecarga.Text = listURPE[position].monto_total.ToString("C", culture);
				vh.tipo.Text = listURPE[position].fecha_vencimiento;
				vh.idURecarga.Text = listURPE[position].identificador.Substring(1, listURPE[position].identificador.Length - 1);
			}


			vh.selectAccount.Click += (sender, e) => {
				if (listURPE == null) {
					if (!listUR[position].isSelected) {
						listUR[positionSelected].isSelected = false;
						listUR[position].isSelected = true;
						positionSelected = position;
						NotifyDataSetChanged();
					}
					
				} else {
					if (!listURPE[position].isSelected) {
						listURPE[positionSelected].isSelected = false;
						listURPE[position].isSelected = true;
						positionSelected = position;
						NotifyDataSetChanged();
					}
				}
			};
		}

		public void reloadListUR(List<Recargas> listUR) {
			this.listUR = listUR;
			NotifyDataSetChanged();
		}

		public void reloadListURPE(List<SolicitaRecarga> listURPE) {
			this.listURPE = listURPE;
			NotifyDataSetChanged();
		}
	}
}
