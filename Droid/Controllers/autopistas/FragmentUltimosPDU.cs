using System;
using System.Collections.Generic;
using System.Linq;
using Android.Content;
using Android.Graphics.Drawables;
using Android.OS;
using Android.Support.V4.App;
using Android.Support.V4.Content;
using Android.Support.V7.Widget;
using Android.Util;
using Android.Views;
using Android.Widget;
using AndroidHUD;
using Com.Bumptech.Glide;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace ServipagMobile.Droid {
	public class FragmentUltimosPDU : Fragment {
		private LinearLayout containerUR, containerWUR;
		private TextView hintSinDatos, ttlLastPDU;
		private ImageView imgAutopista;
		private UltimosPDUAdapter urAdater;
		private RecyclerView.LayoutManager layoutManager;
		private Drawable divider;
		private RecyclerView.ItemDecoration dividerDecoration;
		private RecyclerView listURecargas;
		private Button nuevaRecarga, pagar;

		private bool isLogin;
		private string badgeCount;
		private string idBiller;
		private string idServicio;
		private MainActivity ma;

		public FragmentUltimosPDU() { }

		public FragmentUltimosPDU(bool isLogin, string badgeCount, string idBiller, string idServicio, MainActivity ma) {
			this.isLogin = isLogin;
			this.badgeCount = badgeCount;
			this.idBiller = idBiller;
			this.idServicio = idServicio;
			this.ma = ma;
		}

		public override void OnCreate(Bundle savedInstanceState) {
			base.OnCreate(savedInstanceState);
		}

		public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState) {
			return inflater.Inflate(Resource.Layout.FragmentUltimasRecargas, container, false);
		}

		public override void OnViewCreated(View view, Bundle savedInstanceState) {
			base.OnViewCreated(view, savedInstanceState);

			var metrics = Resources.DisplayMetrics;
			var countList = 0;

			ttlLastPDU = view.FindViewById<TextView>(Resource.Id.ttlLastPDU);
			imgAutopista = view.FindViewById<ImageView>(Resource.Id.imgAutopista);
			containerUR = view.FindViewById<LinearLayout>(Resource.Id.containerUR);
			containerWUR = view.FindViewById<LinearLayout>(Resource.Id.containerWUR);
			hintSinDatos = view.FindViewById<TextView>(Resource.Id.hintSinDatos);
			listURecargas = view.FindViewById<RecyclerView>(Resource.Id.listURecargas);
			nuevaRecarga = view.FindViewById<Button>(Resource.Id.nuevaRecarga);
			pagar = view.FindViewById<Button>(Resource.Id.pagar);
			nuevaRecarga.Text = Resources.GetString(Resource.String.autopista_updu_bttn_n_pdu);

			if (!isLogin) {
				ma.carroCompraLayout.Visibility = ViewStates.Visible;
			} else {
				ma.carroCompraLayout.Visibility = ViewStates.Gone;
			}

			ma.sortMyAccounts.Visibility = ViewStates.Gone;
			ma.addAccount.Visibility = ViewStates.Gone;

			if (idBiller.Equals("886")) {
				countList = ma.listaUltimosPDU.Count;
			} else if (idBiller.Equals("964")) {
				countList = ma.listaUltimosPDT.Count;
			}
			if (countList > 0) {
				ttlLastPDU.Text = Resources.GetString(Resource.String.autopista_updu_ttl_last_pdu);
				Glide.With(ma)
				     .Load(Resource.Drawable.autopistas)
				     .Into(imgAutopista);
				
				pagar.Visibility = ViewStates.Visible;
				containerUR.Visibility = ViewStates.Visible;
				containerWUR.Visibility = ViewStates.Gone;

				nuevaRecarga.SetWidth(metrics.WidthPixels / 2);
				pagar.SetWidth(metrics.WidthPixels / 2);

				if (idBiller.Equals("886")) {
					if (ma.listaUltimosPDU.Count > 10) {
						urAdater = new UltimosPDUAdapter(ma.listaUltimosPDU.GetRange((ma.listaUltimosPDU.Count - 1) - 10,
																				 10));
					} else {
						urAdater = new UltimosPDUAdapter(ma.listaUltimosPDU);
					}
				} else if (idBiller.Equals("964")) {
					if (ma.listaUltimosPDU.Count > 10) {
						urAdater = new UltimosPDUAdapter(ma.listaUltimosPDT.GetRange((ma.listaUltimosPDT.Count - 1) - 10,
																				 10));
					} else {
						urAdater = new UltimosPDUAdapter(ma.listaUltimosPDT);
					}
				}

				divider = ContextCompat.GetDrawable(Context, Resource.Drawable.divider);
				dividerDecoration = new CustomItemDecoration(divider);

				listURecargas.SetAdapter(urAdater);
				listURecargas.AddItemDecoration(dividerDecoration);
				layoutManager = new LinearLayoutManager((MainActivity)Activity);
				listURecargas.SetLayoutManager(layoutManager);
			} else {
				hintSinDatos.Text = Resources.GetString(Resource.String.autopista_updu_sin_datos);
				pagar.Visibility = ViewStates.Gone;
				containerUR.Visibility = ViewStates.Gone;
				containerWUR.Visibility = ViewStates.Visible;
				nuevaRecarga.SetWidth(metrics.WidthPixels);
			}

			nuevaRecarga.Click += (sender, e) => {
				Intent i = new Intent(ma, typeof(PDUActivity));

				i.PutExtra("isLogin", isLogin);
				i.PutExtra("badgeCount", badgeCount);
				i.PutExtra("idBiller", idBiller);
				i.PutExtra("idServicio", idServicio);
				i.PutExtra("isUPDU", false);

				StartActivityForResult(i, 11);
			};

			pagar.Click += (sender, e) => {
				Intent i = new Intent(ma, typeof(PDUActivity));

				i.PutExtra("isLogin", isLogin);
				i.PutExtra("badgeCount", badgeCount);
				i.PutExtra("idBiller", idBiller);
				i.PutExtra("idServicio", idServicio);
				i.PutExtra("isUPDU", true);

				if (idBiller.Equals("886")) {
					if (ma.listaUltimosPDU.Count > 10) {
						i.PutExtra("uPDU", JsonConvert.SerializeObject(ma.listaUltimosPDU.GetRange((ma.listaUltimosPDU.Count - 1) - 10,
																				 10).Single(s => s.isSelected == true)));
					} else {
						i.PutExtra("uPDU", JsonConvert.SerializeObject(ma.listaUltimosPDU.Single(s => s.isSelected == true)));
					}
				} else if (idBiller.Equals("964")) {
					if (ma.listaUltimosPDU.Count > 10) {
						i.PutExtra("uPDT", JsonConvert.SerializeObject(ma.listaUltimosPDT.GetRange((ma.listaUltimosPDT.Count - 1) - 10,
																				 10).Single(s => s.isSelected == true)));
					} else {
						i.PutExtra("uPDT", JsonConvert.SerializeObject(ma.listaUltimosPDT.Single(s => s.isSelected == true)));
					}
				}

				StartActivityForResult(i, 11);
			};
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

		public override void OnActivityResult(int requestCode, int resultCode, Intent data) {
			base.OnActivityResult(requestCode, resultCode, data);
			switch (requestCode) {
				case 11:
					if (data.GetStringExtra("pdu") == "abreCarroCompra") {
						ma.carroCompraLayout.Visibility = ViewStates.Gone;
						ma.sortMyAccounts.Visibility = ViewStates.Visible;
						ma.addAccount.Visibility = ViewStates.Visible;

						ma.isCarroCompra = true;
						ma.isLogin = false;
						ma.childFragment = new FragmentListaCuentas(isLogin, ma.listAdd);
						ma.changeMainFragment(ma.childFragment, "mCarroCompra");
					} else if (data.GetStringExtra("pdu") == "reloadUPDU") {
						ma.listaUltimosPDU = new List<PaseDiario>();
						ma.listaUltimosPDT = new List<PaseTardio>();

						foreach (PaseDiario pdp in RealmDB.GetInstance().realm.All<PaseDiario>()) {
							var pd = new PaseDiario();
							pd.nombre_fantasia = pdp.nombre_fantasia;
							pd.fecha_vencimiento = pdp.fecha_vencimiento;
							pd.identificador = pdp.identificador;
							pd.monto_total = pdp.monto_total;
							pd.isSelected = pdp.isSelected;

							pd.isPDU = pdp.isPDU;
							pd.tipoPDU = pdp.tipoPDU;
							pd.patente = pdp.patente;
							pd.categoria = pdp.categoria;
							pd.fecha_circulacion = pdp.fecha_circulacion;
							pd.idBiller = pdp.idBiller;
							pd.idServicio = pdp.idServicio;
							ma.listaUltimosPDU.Add(pd);
						}

						foreach (PaseTardio ptp in RealmDB.GetInstance().realm.All<PaseTardio>()) {
							var pt = new PaseTardio();
							pt.nombre_fantasia = ptp.nombre_fantasia;
							pt.fecha_vencimiento = ptp.fecha_vencimiento;
							pt.identificador = ptp.identificador;
							pt.monto_total = ptp.monto_total;
							pt.isSelected = ptp.isSelected;

							pt.isPDU = ptp.isPDU;
							pt.tipoPDU = ptp.tipoPDU;
							pt.patente = ptp.patente;
							pt.categoria = ptp.categoria;
							pt.fecha_circulacion = ptp.fecha_circulacion;
							pt.idBiller = ptp.idBiller;
							pt.idServicio = ptp.idServicio;
							ma.listaUltimosPDT.Add(pt);
						}

						if (ma.listaUltimosPDU.Count > 0) {
							ma.listaUltimosPDU[0].isSelected = true;
						}
						if (ma.listaUltimosPDT.Count > 0) {
							ma.listaUltimosPDT[0].isSelected = true;
						}

						Log.Debug("Amount PDU", ma.listaUltimosPDU.Count.ToString());
						Log.Debug("Amount PDT", ma.listaUltimosPDT.Count.ToString());
						Log.Debug("idBiller", idBiller);
						if (idBiller.Equals("886")) {
							urAdater.reloadUPDU(ma.listaUltimosPDU);
						} else if (idBiller.Equals("964")) {
							urAdater.reloadUPDT(ma.listaUltimosPDT);
						}
					}
				break;
			}
		}
	}
}
