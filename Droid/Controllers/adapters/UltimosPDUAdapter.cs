using System;
using System.Collections.Generic;
using System.Globalization;
using Android.Support.V7.Widget;
using Android.Util;
using Android.Views;

namespace ServipagMobile.Droid {
	public class UltimosPDUAdapter : RecyclerView.Adapter {
		private List<PaseDiario> listUltimosPDU;
		private List<PaseTardio> listUltimosPDT;
		private int count = 0;
		private int positionSelected = 0;
		private CultureInfo culture { get; set; }

		public UltimosPDUAdapter() { }

		public UltimosPDUAdapter(List<PaseDiario> listUltimosPDU) {
			this.listUltimosPDU = listUltimosPDU;
			this.count = listUltimosPDU.Count;
			this.culture = new CultureInfo("es-CL");
		}

		public UltimosPDUAdapter(List<PaseTardio> listUltimosPDT) {
			this.listUltimosPDT = listUltimosPDT;
			this.count = listUltimosPDT.Count;
			this.culture = new CultureInfo("es-CL");
		}

		public override int ItemCount {
			get {
				return count;
			}
		}

		public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType) {
			View itemView = LayoutInflater.From(parent.Context).Inflate(Resource.Layout.RowUltimosPDU, parent, false);
			UltimosPDUViewHolder vh = new UltimosPDUViewHolder(itemView);

			return vh;
		}

		public override void OnBindViewHolder(RecyclerView.ViewHolder holder, int position) {
			UltimosPDUViewHolder vh = holder as UltimosPDUViewHolder;

			if (listUltimosPDU != null) {
				if (listUltimosPDU[position].isSelected) {
					vh.selectAccount.SetImageResource(Resource.Drawable.seleccion_home);
				} else {
					vh.selectAccount.SetImageResource(Resource.Drawable.sin_seleccion_home);
				}
				vh.nombreBiller.Text = listUltimosPDU[position].nombre_fantasia;
				vh.montoTotalRecarga.Text = listUltimosPDU[position].monto_total.ToString("C", culture);
				vh.identificador.Text = listUltimosPDU[position].patente;
				vh.fechaPago.Text = listUltimosPDU[position].fecha_vencimiento;

				vh.selectAccount.Click += (sender, e) => {
					if (listUltimosPDU == null) {
						if (!listUltimosPDU[position].isSelected) {
							listUltimosPDU[positionSelected].isSelected = false;
							listUltimosPDU[position].isSelected = true;
							positionSelected = position;
							NotifyDataSetChanged();
						}

					} else {
						if (!listUltimosPDU[position].isSelected) {
							listUltimosPDU[positionSelected].isSelected = false;
							listUltimosPDU[position].isSelected = true;
							positionSelected = position;
							NotifyDataSetChanged();
						}
					}
				};
			} else {
				if (listUltimosPDT[position].isSelected) {
					vh.selectAccount.SetImageResource(Resource.Drawable.seleccion_home);
				} else {
					vh.selectAccount.SetImageResource(Resource.Drawable.sin_seleccion_home);
				}
				vh.nombreBiller.Text = listUltimosPDT[position].nombre_fantasia;
				vh.montoTotalRecarga.Text = listUltimosPDT[position].monto_total.ToString("C", culture);
				vh.identificador.Text = listUltimosPDT[position].patente;
				vh.fechaPago.Text = listUltimosPDT[position].fecha_vencimiento;

				vh.selectAccount.Click += (sender, e) => {
					if (listUltimosPDT == null) {
						if (!listUltimosPDT[position].isSelected) {
							listUltimosPDT[positionSelected].isSelected = false;
							listUltimosPDT[position].isSelected = true;
							positionSelected = position;
							NotifyDataSetChanged();
						}

					} else {
						if (!listUltimosPDT[position].isSelected) {
							listUltimosPDT[positionSelected].isSelected = false;
							listUltimosPDT[position].isSelected = true;
							positionSelected = position;
							NotifyDataSetChanged();
						}
					}
				};
			}
		}

		public void reloadUPDU(List<PaseDiario> listUltimosPDU) {
			this.listUltimosPDU = listUltimosPDU;
			this.count = listUltimosPDU.Count;
			NotifyDataSetChanged();
		}

		public void reloadUPDT(List<PaseTardio> listUltimosPDT) {
			this.listUltimosPDT = listUltimosPDT;
			this.count = listUltimosPDT.Count;
			NotifyDataSetChanged();
		}
	}
}