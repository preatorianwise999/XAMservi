using System;
using Android.Support.V7.Widget;
using Android.Views;
using Android.Widget;

namespace ServipagMobile.Droid {
	public class UltimasRecargasViewHolder : RecyclerView.ViewHolder {
		public ImageButton selectAccount;
		public TextView nombreBiller, montoTotalRecarga, idURecarga, tipo;

		public UltimasRecargasViewHolder(View itemView) : base (itemView) {
			selectAccount = itemView.FindViewById<ImageButton>(Resource.Id.selectAccount);
			nombreBiller = itemView.FindViewById<TextView>(Resource.Id.nombreBiller);
			montoTotalRecarga = itemView.FindViewById<TextView>(Resource.Id.montoTotalRecarga);
			idURecarga = itemView.FindViewById<TextView>(Resource.Id.idURecarga);
			tipo = itemView.FindViewById<TextView>(Resource.Id.tipo);
		}
	}
}
