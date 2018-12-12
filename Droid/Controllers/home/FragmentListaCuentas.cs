using Android.OS;
using Android.Views;
using Android.Support.V7.Widget;
using System.Collections.Generic;
using Android.Graphics.Drawables;
using Android.Support.V4.Content;
using Android.Support.V4.App;
using Android.Content;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using AndroidHUD;
using System.Linq;
using System;

namespace ServipagMobile.Droid {
	public class FragmentListaCuentas : Fragment {
		public MisCuentasAdapter adapter;
		private Android.Widget.RelativeLayout fragmentListaCuentas;
		public RecyclerView recyclerView;
		public Android.Widget.ImageView sinCuentas;
		public Android.Widget.TextView hintSinCuentas;
		public Android.Widget.Button bttnBuscarBoleta;
		private RecyclerView.LayoutManager layoutManager;
		private Drawable divider;
		private List<MisCuentas> misCuentas = new List<MisCuentas>();
		private List<MisDeudas> misDeudas = new List<MisDeudas>();
		private List<BuscaDeudas> buscaDeudas = new List<BuscaDeudas>();
		private RecyclerView.ItemDecoration dividerDecoration;
		private MainActivity ma;
		private Utils utils;
		private bool isLogin;


		public FragmentListaCuentas() {}

		public FragmentListaCuentas(bool isLogin, List<MisCuentas> misCuentas) {
			this.isLogin = isLogin;
			this.misCuentas = misCuentas;
		}

		public override void OnCreate(Bundle savedInstanceState) {
			base.OnCreate(savedInstanceState);

			ma = (MainActivity)Activity;
			utils = new Utils();
		}

		public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState) {
			return inflater.Inflate(Resource.Layout.FragmentListaCuentas, container, false);
		}

		public override void OnViewCreated(View view, Bundle savedInstanceState) {
			base.OnViewCreated(view, savedInstanceState);

			fragmentListaCuentas = view.FindViewById<Android.Widget.RelativeLayout>(Resource.Id.fragmentListaCuentas);
			recyclerView = view.FindViewById<RecyclerView>(Resource.Id.listaCuentas);
			sinCuentas = view.FindViewById<Android.Widget.ImageView>(Resource.Id.sinCuentas);
			hintSinCuentas = view.FindViewById<Android.Widget.TextView>(Resource.Id.hintSinCuentas);
			bttnBuscarBoleta = view.FindViewById<Android.Widget.Button>(Resource.Id.bttnBuscarBoleta);

			if (misCuentas.Count > 0) {
				if (isLogin) {
					persistentSort(ma.sortSelected);
				} else {
					persistentSort(ma.sortSelectedPE);
				}
			} else {
				bttnBuscarBoleta.SetText(Resource.String.home_button_agrega_cuentas);
				if (isLogin) {
					hintSinCuentas.SetText(Resource.String.home_sin_cuentas_registrado);
				} else {
					hintSinCuentas.SetText(Resource.String.home_sin_cuentas_express);
				}
				sinCuentas.Visibility = ViewStates.Visible;
				hintSinCuentas.Visibility = ViewStates.Visible;
				recyclerView.Visibility = ViewStates.Gone;
			}

			bttnBuscarBoleta.Click += (sender, e) => {
				if (misCuentas.Count > 0) {
					buscarBoleta();
				} else {
					openAddService(!isLogin);
				}
			};

			adapter = new MisCuentasAdapter(misCuentas, this, ma, isLogin);
			divider = ContextCompat.GetDrawable(Context, Resource.Drawable.divider);
			dividerDecoration = new CustomItemDecoration(divider);


			recyclerView.SetAdapter(adapter);
			recyclerView.AddItemDecoration(dividerDecoration);

			layoutManager = new LinearLayoutManager(ma);
			recyclerView.SetLayoutManager(layoutManager);


		}

		public void buscarBoleta() {
			var listSelected = misCuentas.FindAll(s => s.isSelected);
			var last = listSelected.Last();
			var cuentasSelected = "";

			if (!arePTTandAuto(listSelected)) {
				foreach (var item in listSelected) {
					if (isLogin) {
						if (item.Equals(last)) {
							cuentasSelected = cuentasSelected + item.idBiller + "," + item.idServicio + "," + item.idCuenta;
						} else {
							cuentasSelected = cuentasSelected + item.idBiller + "," + item.idServicio + "," + item.idCuenta + "|";
						}
					} else {
						if (item.Equals(last)) {
							cuentasSelected = cuentasSelected + item.idServicio + "," + item.idBiller + "," + item.idCuenta;
						} else {
							cuentasSelected = cuentasSelected + item.idServicio + "," + item.idBiller + "," + item.idCuenta + "|";
						}
					}
				}


				JObject parametros = new JObject();
				AndHUD.Shared.Show(ma, null, -1, MaskType.Black);

				parametros.Add("canal", DeviceInformation.GetInstance().channel);
				parametros.Add("cuentas", cuentasSelected);

				if (isLogin) {
					parametros.Add("sesion", RealmDB.GetInstance().realm.All<PersistentData>().First().cookie);
					parametros.Add("idUsuario", UserData.GetInstance().rut);
					buscarCuentasCR(parametros);
				} else {
					parametros.Add("sesion", RealmDB.GetInstance().realm.All<PersistentData>().First().idTransaccion);
					buscarCuentasCNR(parametros);
				}
			} else {
				CustomAlertDialog alert = 
					new CustomAlertDialog(ma,
					                      Resources.GetString(Resource.String.autopista_title_alert_more_than_one),
					                      Resources.GetString(Resource.String.autopista_body_alert_more_than_one),
					                      Resources.GetString(Resource.String.autopista_button_alert_more_than_one),
					                      "", null, null);
				alert.showDialog();
				AndHUD.Shared.Dismiss(ma);
			}
		}

		private bool arePTTandAuto(List<MisCuentas>listSelected) {
			var arePTT = listSelected.Where(x => x.idBiller == 130);
			var areAuto = listSelected.Where(x => x.idServicio == 11 && x.idBiller != 130);

			if ((arePTT.Count() > 0) && (areAuto.Count() > 0)) {
				return true;
			} else {
				return false;
			}
		}

		public override void OnActivityResult(int requestCode, int resultCode, Intent data) {
			base.OnActivityResult(requestCode, resultCode, data);

			if (requestCode == 2) {
				switch (data.GetStringExtra("actionEdit")) {
					case "changeData":
						int indexCuenta = data.GetIntExtra("indexCuenta", -1);
						string aliasCuenta = data.GetStringExtra("aliasCuenta") ?? "";

						misCuentas[indexCuenta].aliasCuenta = aliasCuenta;
						adapter.updateAdapter(indexCuenta, misCuentas[indexCuenta]);
					break;
				}
			}
		}

		private void persistentSort(int sort) {
			switch (sort) {
				case 0:
					utils.QuickSortCuentas(misCuentas, 0, misCuentas.Count - 1, false);
					break;
				case 1:
					utils.QuickSortCuentas(misCuentas, 0, misCuentas.Count - 1, true);
					break;
				case 2:
					utils.QuickSortAlias(misCuentas, 0, misCuentas.Count - 1, false);
					break;
				case 3:
					utils.QuickSortAlias(misCuentas, 0, misCuentas.Count - 1, true);
					break;
			}
		}

		public void sortNombreCuenta(bool esMenorMayor) {
			utils.QuickSortCuentas(misCuentas, 0, misCuentas.Count - 1, esMenorMayor);

			adapter.refreshAdapter();
		}

		public void sortAliasCuenta(bool esMenorMayor) {
			utils.QuickSortAlias(misCuentas, 0, misCuentas.Count - 1, esMenorMayor);

			adapter.refreshAdapter();
		}

		public void openAddService(bool isPagoExpress) {
			Intent intent = new Intent(ma, typeof(AgregarActivity));
			intent.PutExtra("isPagoExpress", isPagoExpress);
			intent.PutExtra("autopista", false);
			intent.PutExtra("listAdd", JsonConvert.SerializeObject(ma.listAdd));
			ma.StartActivityForResult(intent, 1);
		}

		public async void buscarCuentasCR(JObject parametros) {
			var response = await MyClass.WorklightClient.UnprotectedInvokeAsync("buscarCuentasCR", "buscar_cuentas_cr", "POST", parametros);

			if (response.Success) {
				if ((int)response.State["Error"] == 0) {
					var listBD = setListBDeudas(response.Response);
					Intent intent = new Intent(ma, typeof(PagoActivity));
					intent.PutExtra("isLogin", isLogin);
					intent.PutExtra("misDeudas", JsonConvert.SerializeObject(listBD));
					ma.StartActivityForResult(intent, 1);

					AndHUD.Shared.Dismiss(ma);
				} else {
					CustomAlertDialog alert = new CustomAlertDialog(ma, "¡Oops!", response.State["Mensaje"].ToString(), "Aceptar", "", null, null);
					alert.showDialog();
					AndHUD.Shared.Dismiss(ma);
				}
			} else {
				CustomAlertDialog alert = new CustomAlertDialog(ma, "¡Oops!", response.Message, "Aceptar", "", null, null);
				alert.showDialog();
				AndHUD.Shared.Dismiss(ma);
			}
		}

		public async void buscarCuentasCNR(JObject parametros) {
			var response = await MyClass.WorklightClient.UnprotectedInvokeAsync("buscarCuentasCNR", "buscar_cuentas_cnr", "POST", parametros);

			if (response.Success) {
				if ((int)response.State["Error"] == 0) {
					var listBD = setListBDeudas(response.Response);
					Intent intent = new Intent(ma, typeof(PagoActivity));
					intent.PutExtra("isLogin", isLogin);
					intent.PutExtra("misDeudas", JsonConvert.SerializeObject(listBD));
					ma.StartActivityForResult(intent, 1);

					AndHUD.Shared.Dismiss(ma);
				} else {
					CustomAlertDialog alert = new CustomAlertDialog(ma, "¡Oops!", response.State["Mensaje"].ToString(), "Ok", "", null, null);
					alert.showDialog();
					AndHUD.Shared.Dismiss(ma);
				}
			} else {
				CustomAlertDialog alert = new CustomAlertDialog(ma, "¡Oops!", response.Message, "Aceptar", "", null, null);
				alert.showDialog();
				AndHUD.Shared.Dismiss(ma);
			}
		}

		private List<BuscaDeudas> setListBDeudas(JObject response) {
			List<BuscaDeudas> list = new List<BuscaDeudas>();
			var listBDeudas = response["ObtieneBuscaDeuda"];

			foreach (var item in listBDeudas) {
				list.Add(new BuscaDeudas(item["S"].ToString(),
										item["acepta_abono"].ToString(),
										item["acepta_casa_comercial"].ToString(),
										item["acepta_pago_min"].ToString(),
										item["acepta_prog"].ToString(),
										item["alias"].ToString(),
										item["boleta"].ToString(),
			                         	Convert.ToInt32(item["cero"].ToString()),
										item["codigo_barra"].ToString(),
										item["codigo_tecno"].ToString(),
										item["codigo_tecno2"].ToString(),
										item["cuota"].ToString(),
										item["descrip_tipo_document"].ToString(),
										item["dias_vencimiento"].ToString(),
										item["direccion"].ToString(),
										item["direccion_factura"].ToString(),
										item["fecha_prog"].ToString(),
										item["fecha_venc"].ToString(),
										item["fecha_vencimiento"].ToString(),
										item["grafico"].ToString(),
			                         	Convert.ToInt32(item["id_biller"].ToString()),
				                        Convert.ToInt32(item["id_estado_pago_solt"].ToString()),
			                         	Convert.ToInt32(item["id_pago_solicitado"].ToString()),
										item["id_periodo_solicitado"].ToString(),
										item["id_secuencia_solicitado"].ToString(),
		                         		Convert.ToInt32(item["id_servicio"].ToString()),
										item["identificador"].ToString(),
										item["imagen_logo"].ToString(),
										item["interes"].ToString(),
										item["logo_servicio"].ToString(),
										item["mensaje"].ToString(),
										item["mensaje_respuesta_usr"].ToString(),
										item["moneda"].ToString(),
			                         	Convert.ToInt32(item["monto_minimo"].ToString()),
										item["monto_origen"].ToString(),
										item["monto_origen2"].ToString(),
										item["monto_original"].ToString(),
			                         	Convert.ToInt32(item["monto_total"].ToString()),
										item["mostrar_cod_barra"].ToString(),
										item["mostrar_fecha_venc"].ToString(),
										item["multa"].ToString(),
										item["nombre_fantasia"].ToString(),
										item["pnd_prog"].ToString(),
										item["rubro"].ToString(),
										item["rut_biller"].ToString(),
										item["telefono"].ToString(),
										item["texto_facturador"].ToString(),
										item["tipo_cliente"].ToString(),
										item["valor_cambio"].ToString(),
										item["valor_cambio2"].ToString(),
										item["valor_uf"].ToString(),
										item["webpay"].ToString()));
			}

			return list;
		}
	}
}
