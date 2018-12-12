using System.Collections.Generic;
using System.Globalization;
using Android.Support.V7.Widget;
using Android.Views;
using Android.Widget;

namespace ServipagMobile.Droid {
	public class MisDeudasViewHolder : RecyclerView.ViewHolder {
		public ImageView selectAccount { get; private set; }
		public TextView nombreBiller { get; private set; }
		public TextView montoTotalCuenta { get; private set; }
		public TextView identificador { get; private set; }
		public TextView numDocumento { get; private set; }
		public TextView fechaVencimiento { get; private set; }
		public TextView hintServicio { get; private set; }
		public TextView nombreCuenta { get; private set; }
		public RadioButton radioSActual { get; private set; }
		public RadioButton radioSAnterior{ get; private set; }
		public TextView valueActual { get; private set; }
		public TextView valueAnterior { get; private set; }
		public RelativeLayout containerRadioGroup { get; private set; }
		public TextView montoTotal { get; private set; }
		private CultureInfo culture { get; set; }

		public MisDeudasViewHolder(View itemView, List<BuscaDeudas> misDeudas , FragmentListaDeudas fld, bool isLogin) : base (itemView) {
			selectAccount = itemView.FindViewById<ImageView>(Resource.Id.selectAccount);
			nombreBiller = itemView.FindViewById<TextView>(Resource.Id.nombreBiller);
			montoTotalCuenta = itemView.FindViewById<TextView>(Resource.Id.montoTotalCuenta);
			identificador = itemView.FindViewById<TextView>(Resource.Id.identificador);
			numDocumento = itemView.FindViewById<TextView>(Resource.Id.numDocumento);
			fechaVencimiento = itemView.FindViewById<TextView>(Resource.Id.fechaVencimiento);
			hintServicio = itemView.FindViewById<TextView>(Resource.Id.hintServicio);
			nombreCuenta = itemView.FindViewById<TextView>(Resource.Id.nombreCuenta);
			radioSActual = itemView.FindViewById<RadioButton>(Resource.Id.radioSActual);
			radioSAnterior = itemView.FindViewById<RadioButton>(Resource.Id.radioSAnterior);
			valueActual = itemView.FindViewById<TextView>(Resource.Id.valueActual);
			valueAnterior = itemView.FindViewById<TextView>(Resource.Id.valueAnterior);
			containerRadioGroup = itemView.FindViewById<RelativeLayout>(Resource.Id.containerRadioGroup);
			culture = new CultureInfo("es-CL");

			radioSActual.Click += (sender, e) => {
				fld.deudaTotal = fld.deudaTotal - misDeudas[AdapterPosition].monto_minimo +
					misDeudas[AdapterPosition].monto_total;
				
				montoTotalCuenta.Text = misDeudas[AdapterPosition].monto_total.ToString("C", culture);
				fld.montoTotal.Text = "Total: " +  fld.deudaTotal.ToString("C", culture);
			};

			radioSAnterior.Click += (sender, e) => {
				fld.deudaTotal = fld.deudaTotal - misDeudas[AdapterPosition].monto_total +
					misDeudas[AdapterPosition].monto_minimo;
				montoTotalCuenta.Text = misDeudas[AdapterPosition].monto_minimo.ToString("C", culture);
				fld.montoTotal.Text = "Total: " +  fld.deudaTotal.ToString("C", culture);
			};
		}
	}
}
