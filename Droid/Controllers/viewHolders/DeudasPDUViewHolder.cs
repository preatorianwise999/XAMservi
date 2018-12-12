using Android.Support.V7.Widget;
using Android.Views;
using Android.Widget;

namespace ServipagMobile.Droid {
	public class DeudasPDUViewHolder : RecyclerView.ViewHolder {
		public ImageButton selectAccount;
		public TextView nombreBiller, montoTotalPDU, patente, numDocumento, fechaVencimiento, hintServicio;

		public DeudasPDUViewHolder(View itemView) : base(itemView) {
			selectAccount = itemView.FindViewById<ImageButton>(Resource.Id.selectAccount);
			nombreBiller = itemView.FindViewById<TextView>(Resource.Id.nombreBiller);
			montoTotalPDU = itemView.FindViewById<TextView>(Resource.Id.montoTotalPDU);
			patente = itemView.FindViewById<TextView>(Resource.Id.patente);
			numDocumento = itemView.FindViewById<TextView>(Resource.Id.numDocumento);
			fechaVencimiento = itemView.FindViewById<TextView>(Resource.Id.fechaVencimiento);
			hintServicio = itemView.FindViewById<TextView>(Resource.Id.hintServicio);
		}
	}
}