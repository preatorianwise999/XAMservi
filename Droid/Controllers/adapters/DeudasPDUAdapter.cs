using System;
using System.Globalization;
using Android.Support.V7.Widget;
using Android.Views;

namespace ServipagMobile.Droid {
	public class DeudasPDUAdapter : RecyclerView.Adapter {
		private BuscaDeudas deudaPDU;
		private PDUActivity PDUAct;
		private FragmentListaDeudasPDU fldPDU;
		private bool isSelected = true;
		private CultureInfo culture { get; set; }

		public DeudasPDUAdapter() { }

		public DeudasPDUAdapter(BuscaDeudas deudaPDU, PDUActivity PDUAct, FragmentListaDeudasPDU fldPDU) {
			this.deudaPDU = deudaPDU;
			this.PDUAct = PDUAct;
			this.fldPDU = fldPDU;
			this.culture = new CultureInfo("es-CL");
		}


		public override int ItemCount {
			get {
				return 1;
			}
		}

		public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType) {
			View itemView = LayoutInflater.From(parent.Context).Inflate(Resource.Layout.RowDeudasPDU, parent, false);
			DeudasPDUViewHolder vh = new DeudasPDUViewHolder(itemView);

			return vh;
		}

		public override void OnBindViewHolder(RecyclerView.ViewHolder holder, int position) {
			DeudasPDUViewHolder vh = holder as DeudasPDUViewHolder;

			vh.nombreBiller.Text = deudaPDU.nombre_fantasia;
			vh.montoTotalPDU.Text = deudaPDU.monto_total.ToString("C", culture);
			vh.patente.Text = deudaPDU.identificador.Split('-')[0];
			vh.numDocumento.Text = deudaPDU.identificador;
			vh.fechaVencimiento.Text = deudaPDU.fecha_vencimiento;
			vh.hintServicio.Text = deudaPDU.mensaje_respuesta_usr;

			vh.selectAccount.Click += (sender, e) => {
				if (isSelected) {
					int m = 0;
					isSelected = false;
					vh.selectAccount.SetImageResource(Resource.Drawable.sin_seleccion_home);
					fldPDU.montoTotal.Text = "Total: " + m.ToString("C", culture);
				} else {
					isSelected = true;
					vh.selectAccount.SetImageResource(Resource.Drawable.seleccion_home);
					fldPDU.montoTotal.Text = "Total: " + deudaPDU.monto_total.ToString("C", culture);
				}
			};

			vh.hintServicio.Click += (sender, e) => {
				CustomAlertDialog alert = new CustomAlertDialog((PDUActivity)fldPDU.Activity, deudaPDU.nombre_fantasia, deudaPDU.mensaje_respuesta_usr, "", "", null, null);
				alert.showDialog();

			};
		}
	}
}