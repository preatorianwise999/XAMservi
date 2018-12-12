using System;
using Android.Support.V7.Widget;
using Android.Views;
using Android.Widget;

namespace ServipagMobile.Droid {
	public class RecargasViewHolder : RecyclerView.ViewHolder {
		public ImageButton selectAccount;
		public TextView nombreBiller, montoTotalRecarga, idRecarga, tipo;

		public RecargasViewHolder(View itemView) : base (itemView) {
			selectAccount = itemView.FindViewById<ImageButton>(Resource.Id.selectAccount);
			nombreBiller = itemView.FindViewById<TextView>(Resource.Id.nombreBiller);
			montoTotalRecarga = itemView.FindViewById<TextView>(Resource.Id.montoTotalRecarga);
			idRecarga = itemView.FindViewById<TextView>(Resource.Id.idRecarga);
			tipo = itemView.FindViewById<TextView>(Resource.Id.tipo);
		}
	}
}
