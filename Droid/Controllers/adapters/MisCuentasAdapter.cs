using System;
using System.Collections.Generic;
using System.Linq;
using Android.Content;
using Android.Support.V7.Widget;
using Android.Views;
using Android.Widget;
using AndroidHUD;
using AndroidSwipeLayout;
using AndroidSwipeLayout.Adapters;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace ServipagMobile.Droid {
	public class MisCuentasAdapter : RecyclerSwipeAdapter {
		public List<MisCuentas> misCuentas;
		private FragmentListaCuentas flc;
		private MainActivity ma;
		private bool isLogin;


		public MisCuentasAdapter(List<MisCuentas> mc, FragmentListaCuentas flc, MainActivity ma, bool isLogin) {
			this.misCuentas = mc;
			this.flc = flc;
			this.ma = ma;
			this.isLogin = isLogin;
		}

		public override int ItemCount {
			get {
				return misCuentas.Count;
			}
		}

		public override void OnBindViewHolder(RecyclerView.ViewHolder viewHolder, int position) {
			MisCuentasViewHolder vh = viewHolder as MisCuentasViewHolder;

			vh.nombreCuenta.Text = misCuentas[position].billerCuenta;
			vh.idCuenta.Text = misCuentas[position].idCuenta;
			vh.aliasCuenta.Text = misCuentas[position].aliasCuenta;

			if (misCuentas[vh.AdapterPosition].isSelected) {
				vh.selectCuenta.SetImageResource(Resource.Drawable.seleccion_home);
			} else {
				vh.selectCuenta.SetImageResource(Resource.Drawable.sin_seleccion_home);
			}

			if (!isLogin) {
				vh.aliasCuenta.Visibility = ViewStates.Gone;
				vh.bttnEdit.Visibility = ViewStates.Gone;
			}

			MItemManager.BindView(vh.ItemView, position);
		}

		public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int position) {
			View itemView = LayoutInflater.From(parent.Context).Inflate(Resource.Layout.RowMiCuenta, parent, false);
			MisCuentasViewHolder vh = new MisCuentasViewHolder(itemView, isLogin);
			vh.SwipeLayout.SetShowMode(SwipeLayout.ShowMode.LayDown);

			vh.selectCuenta.Click += (sender, e) => {
				if (misCuentas[vh.AdapterPosition].isSelected) {
					misCuentas[vh.AdapterPosition].isSelected = false;
					vh.selectCuenta.SetImageResource(Resource.Drawable.sin_seleccion_home);
				} else {
					misCuentas[vh.AdapterPosition].isSelected = true;
					vh.selectCuenta.SetImageResource(Resource.Drawable.seleccion_home);
				}
			};

			vh.bttnEdit.Click += (sender, e) => {
				Intent intent = new Intent(ma, typeof(EditarActivity));
				intent.PutExtra("servicio", JsonConvert.SerializeObject(misCuentas[vh.AdapterPosition]));
				intent.PutExtra("indexCuenta", vh.AdapterPosition);

				ma.StartActivityForResult(intent, 22);
				vh.SwipeLayout.Close();
			};

			vh.bttnDelete.Click += (sender, e) => {
				onDeleteClick(vh.AdapterPosition);
				vh.SwipeLayout.Close();
			};

			return vh;
		}

		private void onDeleteClick(int index) {
			if (isLogin) {
				JObject parametros = new JObject();

				parametros.Add("sesion", RealmDB.GetInstance().realm.All<PersistentData>().First().cookie);
				parametros.Add("canal", DeviceInformation.GetInstance().channel);
				parametros.Add("idUsuario", UserData.GetInstance().rut);
				parametros.Add("idServicio", misCuentas[index].idServicio);
				parametros.Add("idBiller", misCuentas[index].idBiller);
				parametros.Add("identificador", misCuentas[index].idCuenta);
				parametros.Add("firma", "");

				CustomAlertDialog alert =
				new CustomAlertDialog(ma, "Eliminar Cuenta", "¿Desea eliminar esta cuenta?", "Cancelar", "Eliminar", null, () => eliminarCuenta(parametros, index));
				alert.showDialog();
			} else {
				CustomAlertDialog alert =
				new CustomAlertDialog(ma, "Eliminar Cuenta", "¿Desea eliminar esta cuenta?", "Cancelar", "Eliminar", null, () => deleteAccount(index));
				alert.showDialog();
			}

		}

		public async void eliminarCuenta(JObject parametros, int index) {
			AndHUD.Shared.Show(ma, null, -1, MaskType.Black);

			var response = await MyClass.WorklightClient.UnprotectedInvokeAsync("eliminarCuentasInscritas", "eliminar_cuentas_inscritas", "POST", parametros);

			if (response.Success) {
				if ((int)response.State["Error"] == 0) {
					deleteAccount(index);
				} else {
					CustomAlertDialog alert = new CustomAlertDialog(ma, "¡Oops!", response.State["Mensaje"].ToString(), "Aceptar", "", null, null);
					alert.showDialog();
				}
			} else {
				CustomAlertDialog alert = new CustomAlertDialog(ma, "¡Oops!", response.Message, "Aceptar", "", null, null);
				alert.showDialog();
			}

			AndHUD.Shared.Dismiss(ma);
		}

		public bool deleteAccount(int index) {
			string nameDeleted = misCuentas[index].billerCuenta;
			var account = RealmDB.GetInstance().realm.All<MisCuentas>().ToList()[index];

			using (var trans = RealmDB.GetInstance().realm.BeginWrite()) {
				RealmDB.GetInstance().realm.Remove(account);
				trans.Commit();
			}

			misCuentas.RemoveAt(index);
			NotifyItemRemoved(index);
			NotifyItemRangeChanged(index, misCuentas.Count);
			MItemManager.CloseAllItems();

			if (misCuentas.Count == 0) {
				flc.bttnBuscarBoleta.SetText(Resource.String.home_button_agrega_cuentas);
				if (isLogin) {
					flc.hintSinCuentas.SetText(Resource.String.home_sin_cuentas_registrado);
				} else {
					flc.hintSinCuentas.SetText(Resource.String.home_sin_cuentas_express);
				}
				flc.sinCuentas.Visibility = ViewStates.Visible;
				flc.hintSinCuentas.Visibility = ViewStates.Visible;
				flc.recyclerView.Visibility = ViewStates.Gone;
			}

			Toast.MakeText(ma, "Se ha eliminado la cuenta " + nameDeleted + " de forma exitosa.", ToastLength.Long).Show();
			return true;
		}

		public override int GetSwipeLayoutResourceId(int p0) {
			return Resource.Id.swipe;
		}

		public void reloadList(List<MisCuentas> l) {
			misCuentas = new List<MisCuentas>();
			misCuentas.AddRange(l);
			flc.recyclerView.SetAdapter(this);
			if (misCuentas.Count > 0) {
				flc.bttnBuscarBoleta.SetText(Resource.String.home_button_buscar_boleta);
				flc.sinCuentas.Visibility = ViewStates.Gone;
				flc.hintSinCuentas.Visibility = ViewStates.Gone;
				flc.recyclerView.Visibility = ViewStates.Visible;
			}

			NotifyDataSetChanged();
		}

		public void updateAdapter(int index, MisCuentas item) {
			misCuentas.Insert(index + 1, item);
			NotifyItemInserted(index + 1);
			misCuentas.RemoveAt(index);
			NotifyItemRemoved(index);
			NotifyItemRangeChanged(index, misCuentas.Count);
			MItemManager.CloseAllItems();
			Toast.MakeText(ma, "Cuenta editada con exito ", ToastLength.Short).Show();
		}

		public void refreshAdapter() {
			NotifyDataSetChanged();
		}
	}
}
