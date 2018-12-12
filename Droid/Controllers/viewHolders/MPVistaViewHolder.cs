using System;
using System.Collections.Generic;
using Android.Support.V7.Widget;
using Android.Views;
using Android.Widget;

namespace ServipagMobile.Droid {
	public class MPVistaViewHolder : RecyclerView.ViewHolder {
		public ImageView imgMPOne { get; private set; }
		public ImageView imgMPTwo { get; private set; }
		public ImageView imgMPThree { get; private set; }

		public TextView nameMPOne { get; private set; }
		public TextView nameMPTwo { get; private set; }
		public TextView nameMPThree { get; private set; }

		public MPVistaViewHolder(View itemView, string tipoVista, int width) : base (itemView) {
			imgMPOne = itemView.FindViewById<ImageView>(Resource.Id.imgMPOne);
			imgMPTwo = itemView.FindViewById<ImageView>(Resource.Id.imgMPTwo);
			imgMPThree = itemView.FindViewById<ImageView>(Resource.Id.imgMPThree);

			nameMPOne = itemView.FindViewById<TextView>(Resource.Id.nameMPOne);
			nameMPTwo = itemView.FindViewById<TextView>(Resource.Id.nameMPTwo);
			nameMPThree = itemView.FindViewById<TextView>(Resource.Id.nameMPThree);

            var w = Convert.ToInt32((width / 3) - 10);
            var h = Convert.ToInt32(w / 1.7f);
            imgMPOne.LayoutParameters.Width = w;
			imgMPOne.LayoutParameters.Height = h;
            imgMPTwo.LayoutParameters.Width = w;
            imgMPTwo.LayoutParameters.Height = h;
            imgMPThree.LayoutParameters.Width = w;
            imgMPThree.LayoutParameters.Height = h;

            nameMPOne.LayoutParameters.Width = w;
			nameMPOne.LayoutParameters.Height = h;
			nameMPTwo.LayoutParameters.Width = w;
			nameMPTwo.LayoutParameters.Height = h;
			nameMPThree.LayoutParameters.Width = w;
			nameMPThree.LayoutParameters.Height = h;
		}

	}
}
