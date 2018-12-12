using System.Collections.Generic;
using System.Linq;
using Android.App;
using Android.Content;
using Android.Support.V7.Widget;
using Android.Util;
using Android.Views;
using Android.Widget;
using AndroidHUD;
using Newtonsoft.Json.Linq;

namespace ServipagMobile.Droid {
	public class ServiciosViewHolder : RecyclerView.ViewHolder {
		public TextView nombre { get; private set; }
		private AgregarActivity aa;
		private ServiciosAdapter adapter;


		public ServiciosViewHolder(View itemView, AgregarActivity aa, ServiciosAdapter adapter, bool isPagoExpress, bool isAutopista) : base(itemView) {
			nombre = itemView.FindViewById<TextView>(Resource.Id.nombre);
			this.aa = aa;
			this.adapter = adapter;

			itemView.Click += (sender, e) => {
				if (adapter.listaServicios[AdapterPosition].entidad == "biller") {
					if (isAutopista) {
						if (adapter.listaServicios[AdapterPosition].id.Equals("964") ||
							adapter.listaServicios[AdapterPosition].id.Equals("886")) {
							if (RealmDB.GetInstance().realm.All<PersistentData>().First().acepta_tc_pdu) {
								Intent intent = new Intent();
								intent.PutExtra("idBiller", adapter.listaServicios[AdapterPosition].id);
								intent.PutExtra("idServicio", adapter.listaServicios[AdapterPosition].id_servicio);
								intent.PutExtra("actionAgregar", "openLastPDU");
								aa.SetResult(Result.Ok, intent);
								aa.Finish();

							} else {
								aa.changeMainFragment(new FragmentTCAutopista(adapter.listaServicios[AdapterPosition], isPagoExpress, adapter.listaServicios[AdapterPosition].id, 
								                                              adapter.listaServicios[AdapterPosition].id_servicio, aa), "tcPDU");
							}
						} else {
							aa.changeMainFragment(new FragmentAgregaCuenta(adapter.listaServicios[AdapterPosition], isPagoExpress), aa.Resources.GetString(Resource.String.agregar_cta_add));
						}
					} else {
						aa.changeMainFragment(new FragmentAgregaCuenta(adapter.listaServicios[AdapterPosition], isPagoExpress), aa.Resources.GetString(Resource.String.agregar_cta_add));
					}
				} else {
					JObject parametros = new JObject();
					AndHUD.Shared.Show(aa, null, -1, MaskType.Black);
					parametros.Add("sesion", RealmDB.GetInstance().realm.All<PersistentData>().First().idTransaccion);
					parametros.Add("idServicio", adapter.listaServicios[AdapterPosition].id);
					parametros.Add("inscribible", "true");

					getBillers(parametros);
				}
			};
		}

		public async void getBillers(JObject parametros) {
			var response = await MyClass.WorklightClient.UnprotectedInvokeAsync("empresas", "empresas", "POST", parametros);

			if (response.Success) {
				if ((int)response.State["Error"] == 0) {
					adapter.filterList(setListaEmpresas(response.Response));
					aa.idFragment = "biller";
				} else {
					CustomAlertDialog alert = new CustomAlertDialog(aa, "¡Oops!", response.State["Mensaje"].ToString(), "Aceptar", "", null, null);
					alert.showDialog();
				}
			} else {
				CustomAlertDialog alert = new CustomAlertDialog(aa, "¡Oops!", response.Message, "Aceptar", "", null, null);
				alert.showDialog();
			}

			AndHUD.Shared.Dismiss(aa);
		}

		private List<Servicios> setListaEmpresas(JToken response) {
			var listServices = response["ListaEmpresas"];
			aa.listaBillers.Clear();

			for (var i = 0; i < listServices.Count(); i++) {
				aa.listaBillers.Add(new Servicios("biller",
									   listServices[i]["id"].ToString(),
									   listServices[i]["nombre"].ToString(),
									   listServices[i]["imagen_logo"].ToString(),
									   listServices[i]["descripcion_primaria_identificador"].ToString(),
									   listServices[i]["descripcion_secundaria_identificador"].ToString(),
									   listServices[i]["dias_vencimiento"].ToString(),
									   listServices[i]["ejemplo_identificador"].ToString(),
									   listServices[i]["id_servicio"].ToString(),
									   listServices[i]["imagen_boleta"].ToString(),
									   listServices[i]["nombre_servicio"].ToString()));
			}

			return aa.listaBillers;
		}
	}
}
