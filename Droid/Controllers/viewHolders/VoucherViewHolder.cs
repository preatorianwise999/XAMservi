using System;
using Android.Support.V7.Widget;
using Android.Views;
using Android.Widget;

namespace ServipagMobile.Droid {
	public class VoucherViewHolder : RecyclerView.ViewHolder {
		public TextView company, accountName, idAccount, amount, authCode;

		public VoucherViewHolder(View itemView) : base(itemView) {
			company = itemView.FindViewById<TextView>(Resource.Id.company);
			accountName = itemView.FindViewById<TextView>(Resource.Id.accountName);
			idAccount = itemView.FindViewById<TextView>(Resource.Id.idAccount);
			amount = itemView.FindViewById<TextView>(Resource.Id.amount);
			authCode = itemView.FindViewById<TextView>(Resource.Id.authCode);
		}
	}
}
