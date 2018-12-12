using System.Collections.Generic;
using System.Globalization;
using Android.Graphics;
using Android.Support.V7.Widget;
using Android.Views;
using Android.Widget;

namespace ServipagMobile.Droid {
	public class MisDeudasAdapter : RecyclerView.Adapter {
		public List<BuscaDeudas> misDeudas;
		private bool isLogin;
		private FragmentListaDeudas fld;
		private CultureInfo culture { get; set; }


		public MisDeudasAdapter(List<BuscaDeudas> md, FragmentListaDeudas fld,  bool isLogin) {
			this.misDeudas = md;
			this.fld = fld;
			this.isLogin = isLogin;
			this.culture = new CultureInfo("es-CL");

			setIsSelected();
		}

		public override int ItemCount {
			get {
				return misDeudas.Count;
			}
		}

		public override void OnBindViewHolder(RecyclerView.ViewHolder holder, int position) {
			MisDeudasViewHolder vh = holder as MisDeudasViewHolder;
			int leftMargin = fld.Resources.GetDimensionPixelSize(Resource.Dimension.deudas_left_margin);
			LinearLayout.LayoutParams ll = new LinearLayout.LayoutParams(ViewGroup.LayoutParams.MatchParent, ViewGroup.LayoutParams.WrapContent);
			ll.BottomMargin = leftMargin;
			ll.LeftMargin = leftMargin;

			vh.nombreBiller.Text = misDeudas[position].nombre_fantasia;
			vh.montoTotalCuenta.Text = misDeudas[position].monto_total.ToString("C", culture);
			vh.identificador.Text = misDeudas[position].identificador;
			vh.numDocumento.Text = misDeudas[position].boleta;
			vh.fechaVencimiento.Text = misDeudas[position].fecha_vencimiento;
			vh.hintServicio.Text = misDeudas[position].mensaje_respuesta_usr;
			vh.nombreCuenta.Text = misDeudas[position].alias;
			vh.radioSActual.Checked = true;
			vh.valueActual.Text = misDeudas[position].monto_total.ToString("C", culture);
			vh.valueAnterior.Text = misDeudas[position].monto_minimo.ToString("C", culture);

			if (misDeudas[position].id_estado_pago_solt == 3) {
				if (misDeudas[position].isSelected) {
					vh.selectAccount.SetImageResource(Resource.Drawable.seleccion_home);
				} else {
					vh.selectAccount.SetImageResource(Resource.Drawable.sin_seleccion_home);
				}
				vh.montoTotalCuenta.Visibility = ViewStates.Visible;
				vh.numDocumento.Visibility = ViewStates.Visible;
				vh.fechaVencimiento.Visibility = ViewStates.Visible;
				vh.hintServicio.Visibility = ViewStates.Visible;

				if (misDeudas[position].acepta_pago_min.Equals("N")) {
					vh.containerRadioGroup.Visibility = ViewStates.Gone;
				} else {
					vh.containerRadioGroup.Visibility = ViewStates.Visible;
				}

				if (!isLogin) {
					vh.nombreCuenta.Visibility = ViewStates.Gone;
					vh.hintServicio.LayoutParameters = ll;
				} else {
					vh.nombreCuenta.Text = misDeudas[position].alias;
					vh.nombreCuenta.LayoutParameters = ll;
				}

				vh.nombreBiller.SetTextColor(fld.Resources.GetColor(Resource.Color.servipag_blue));
				vh.identificador.SetTextColor(fld.Resources.GetColor(Resource.Color.servipag_blue));
				vh.nombreCuenta.SetTextColor(fld.Resources.GetColor(Resource.Color.servipag_blue));
			} else {
				vh.selectAccount.SetImageResource(Resource.Drawable.sin_seleccion_home);
				vh.montoTotalCuenta.Visibility = ViewStates.Gone;
				vh.numDocumento.Visibility = ViewStates.Gone;
				vh.fechaVencimiento.Visibility = ViewStates.Gone;
				vh.hintServicio.Visibility = ViewStates.Gone;
				vh.containerRadioGroup.Visibility = ViewStates.Gone;
				vh.nombreCuenta.Text = misDeudas[position].alias;
				vh.nombreCuenta.LayoutParameters = ll;

				if (!isLogin) {
					vh.nombreCuenta.Visibility = ViewStates.Gone;
					vh.identificador.LayoutParameters = ll;
				} else {
					vh.nombreCuenta.Text = misDeudas[position].alias;
					vh.nombreCuenta.LayoutParameters = ll;
				}

				vh.nombreBiller.SetTextColor(fld.Resources.GetColor(Resource.Color.servipag_dark_grey));
				vh.identificador.SetTextColor(fld.Resources.GetColor(Resource.Color.servipag_dark_grey));
				vh.nombreCuenta.SetTextColor(fld.Resources.GetColor(Resource.Color.servipag_dark_grey));
			}
		}

		public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType) {
			View itemView = LayoutInflater.From(parent.Context).Inflate(Resource.Layout.RowDeudas, parent, false);
			MisDeudasViewHolder vh = new MisDeudasViewHolder(itemView, misDeudas, fld, isLogin);

			vh.selectAccount.Click += (sender, e) => {
				if (misDeudas[vh.AdapterPosition].isSelected) {
					misDeudas[vh.AdapterPosition].isSelected = false;
					vh.selectAccount.SetImageResource(Resource.Drawable.sin_seleccion_home);
					vh.radioSActual.Enabled = false;
					vh.radioSAnterior.Enabled = false;
					fld.deudaTotal = fld.deudaTotal - misDeudas[vh.AdapterPosition].monto_total;
					fld.montoTotal.Text = "Total: " + fld.deudaTotal.ToString("C", culture);
				} else {
					misDeudas[vh.AdapterPosition].isSelected = true;
					vh.selectAccount.SetImageResource(Resource.Drawable.seleccion_home);
					vh.radioSActual.Enabled = true;
					vh.radioSAnterior.Enabled = true;
					fld.deudaTotal = fld.deudaTotal + misDeudas[vh.AdapterPosition].monto_total;
					fld.montoTotal.Text = "Total: " + fld.deudaTotal.ToString("C", culture);
				}
			};

			vh.hintServicio.Click += (sender, e) => {
				CustomAlertDialog alert = new CustomAlertDialog((PagoActivity)fld.Activity, "", misDeudas[vh.AdapterPosition].mensaje_respuesta_usr, "Aceptar", "", null, null);
				alert.showDialog();
			};

			return vh;
		}

		private void setIsSelected() {
			for (int i = 0; i<misDeudas.Count; i++) {
				if (misDeudas[i].id_estado_pago_solt == 3) {
					misDeudas[i].isSelected = true;
				} else {
					misDeudas[i].isSelected = false;
				}
			}
		}
	}
}
