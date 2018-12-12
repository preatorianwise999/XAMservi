using System;
using System.Collections.Generic;
using Android.Support.V7.Widget;
using Android.Util;
using Android.Views;
using Android.Widget;

namespace ServipagMobile.Droid {
	public class RegionComunaViewHolder : RecyclerView.ViewHolder {
		public TextView nameRC { get; private set; }

		public RegionComunaViewHolder(View itemView, string nameFragment, List<RegionComuna>list) : base(itemView) {
			nameRC = itemView.FindViewById<TextView>(Resource.Id.nameRC);
			itemView.Click += (sender, e) => {
				if (RegisterData.GetInstance().listType == "region") {
					RegisterData.GetInstance().region = Convert.ToInt32(list[this.AdapterPosition].id);
					RegisterData.GetInstance().comuna = -1;
					var a = (RegistroActivity)RegisterData.GetInstance().activity;
					a.changeMainFragment(new FragmentRegionComuna(), nameFragment);
				} else {
					RegisterData.GetInstance().comuna = Convert.ToInt32(list[this.AdapterPosition].id);
					var a = (RegistroActivity)RegisterData.GetInstance().activity;
					a.changeMainFragment(new FragmentRegionComuna(), nameFragment);
				}
			};
		}
	}
}
