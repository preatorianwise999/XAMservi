using System;
using Android.Support.V7.Widget;
using Android.Views;
using Android.Widget;

namespace ServipagMobile.Droid {
	public class ServiciosRecargaViewHolder : RecyclerView.ViewHolder {
		public ImageView imgMPOne { get; private set; }
		public ImageView imgMPTwo { get; private set; }
		public ImageView imgMPThree { get; private set; }

		public TextView nameMPOne { get; private set; }
		public TextView nameMPTwo { get; private set; }
		public TextView nameMPThree { get; private set; }

		public ServiciosRecargaViewHolder(View itemView, int width) : base(itemView) {
			imgMPOne = itemView.FindViewById<ImageView>(Resource.Id.imgMPOne);
			imgMPTwo = itemView.FindViewById<ImageView>(Resource.Id.imgMPTwo);
			imgMPThree = itemView.FindViewById<ImageView>(Resource.Id.imgMPThree);

			nameMPOne = itemView.FindViewById<TextView>(Resource.Id.nameMPOne);
			nameMPTwo = itemView.FindViewById<TextView>(Resource.Id.nameMPTwo);
			nameMPThree = itemView.FindViewById<TextView>(Resource.Id.nameMPThree);

			imgMPOne.LayoutParameters.Width = (int)(width * 0.25);
			imgMPOne.LayoutParameters.Height = 150;
			imgMPTwo.LayoutParameters.Width = (int)(width * 0.25);
			imgMPTwo.LayoutParameters.Height = 150;
			imgMPThree.LayoutParameters.Width = (int)(width * 0.25);
			imgMPThree.LayoutParameters.Height = 150;

			nameMPOne.LayoutParameters.Width = (int)(width * 0.25);
			nameMPOne.LayoutParameters.Height = 150;
			nameMPTwo.LayoutParameters.Width = (int)(width * 0.25);
			nameMPTwo.LayoutParameters.Height = 150;
			nameMPThree.LayoutParameters.Width = (int)(width * 0.25);
			nameMPThree.LayoutParameters.Height = 150;
		}
	}
}
