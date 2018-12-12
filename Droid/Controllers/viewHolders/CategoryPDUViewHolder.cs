using System;
using Android.Support.V7.Widget;
using Android.Views;
using Android.Widget;

namespace ServipagMobile.Droid {
	public class CategoryPDUViewHolder : RecyclerView.ViewHolder{
		public ImageView imgCategory { get; private set; }
		public TextView hintCategory { get; private set; }

		public CategoryPDUViewHolder(View itemView) : base(itemView) {
			imgCategory = itemView.FindViewById<ImageView>(Resource.Id.imgCategory);
			hintCategory = itemView.FindViewById<TextView>(Resource.Id.hintCategory);
		}
	}
}
