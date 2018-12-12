using Android.Support.V7.Widget;
using Android.Views;
using Android.Widget;
using AndroidSwipeLayout;

namespace ServipagMobile.Droid {
	public class MisCuentasViewHolder : RecyclerView.ViewHolder {
		public SwipeLayout SwipeLayout;
		public ImageView selectCuenta { get; private set; }
		public TextView nombreCuenta { get; private set; }
		public TextView idCuenta { get; private set; }
		public TextView aliasCuenta { get; private set; }

		public RelativeLayout bttnEdit { get; private set; }
		public RelativeLayout bttnDelete { get; private set; }


		public MisCuentasViewHolder(View itemView, bool isLogin) : base (itemView) {
			SwipeLayout = itemView.FindViewById<SwipeLayout>(Resource.Id.swipe);
			selectCuenta = itemView.FindViewById<ImageView>(Resource.Id.selectAccount);
			nombreCuenta = itemView.FindViewById<TextView>(Resource.Id.nombreCuenta);
			idCuenta = itemView.FindViewById<TextView>(Resource.Id.idCuenta);
			aliasCuenta = itemView.FindViewById<TextView>(Resource.Id.aliasCuenta);

			bttnEdit = itemView.FindViewById<RelativeLayout>(Resource.Id.bttnEdit);
			bttnDelete = itemView.FindViewById<RelativeLayout>(Resource.Id.bttnDelete);
		}
	}
}
