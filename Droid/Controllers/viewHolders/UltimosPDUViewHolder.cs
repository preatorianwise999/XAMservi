using System;
using Android.Support.V7.Widget;
using Android.Views;
using Android.Widget;

namespace ServipagMobile.Droid {
	public class UltimosPDUViewHolder : RecyclerView.ViewHolder {
		public ImageButton selectAccount;
		public TextView nombreBiller, montoTotalRecarga, identificador, fechaPago;

		public UltimosPDUViewHolder(View itemView) : base (itemView) {
			selectAccount = itemView.FindViewById<ImageButton>(Resource.Id.selectAccount);
			nombreBiller = itemView.FindViewById<TextView>(Resource.Id.nombreBiller);
			montoTotalRecarga = itemView.FindViewById<TextView>(Resource.Id.montoTotalRecarga);
			identificador = itemView.FindViewById<TextView>(Resource.Id.identificador);
			fechaPago = itemView.FindViewById<TextView>(Resource.Id.fechaPago);
		}
	}
}
