using System;
using System.Collections.Generic;
using Android.Support.V7.Widget;
using Android.Views;

namespace ServipagMobile.Droid {
	public class VoucherAdapter : RecyclerView.Adapter {
		private List<VoucherInfo> listVoucher;

		public VoucherAdapter(List<VoucherInfo> listVoucher) {
			this.listVoucher = listVoucher;
		}

		public override int ItemCount {
			get {
				return listVoucher.Count;
			}
		}

		public override void OnBindViewHolder(RecyclerView.ViewHolder holder, int position) {
			VoucherViewHolder vh = holder as VoucherViewHolder;

			vh.company.Text = listVoucher[position].company;
			vh.accountName.Text = listVoucher[position].alias;
			vh.idAccount.Text = listVoucher[position].idAccount;
			vh.amount.Text = listVoucher[position].amount;
			vh.authCode.Text = listVoucher[position].authCode;
		}

		public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType) {
			View itemView = LayoutInflater.From(parent.Context).Inflate(Resource.Layout.RowVoucher, parent, false);
			VoucherViewHolder vh = new VoucherViewHolder(itemView);

			return vh;
		}
	}
}
