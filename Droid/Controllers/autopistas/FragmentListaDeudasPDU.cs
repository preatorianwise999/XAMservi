using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Android.Content;
using Android.Graphics.Drawables;
using Android.OS;
using Android.Support.V4.App;
using Android.Support.V4.Content;
using Android.Support.V7.Widget;
using Android.Views;
using Android.Widget;
using AndroidHUD;
using Newtonsoft.Json.Linq;

namespace ServipagMobile.Droid {
	public class FragmentListaDeudasPDU : Fragment {
		private DeudasPDUAdapter deudasPDUAdapter;
		private RecyclerView.LayoutManager layoutManager;
		private Drawable divider;
		private RecyclerView.ItemDecoration dividerDecoration;
		private RecyclerView listaDeudasPDU;
		private RelativeLayout bttnPagar;
		public TextView montoTotal;

		private PaseDiario paseDiario;
		private PDUActivity PDUAct;
		private CultureInfo culture { get; set; }
		private bool isLogin;
		private BuscaDeudas deudaPDU;

		public FragmentListaDeudasPDU() { }

		public FragmentListaDeudasPDU(PaseDiario paseDiario, PDUActivity PDUAct, bool isLogin) {
			this.paseDiario = paseDiario;
			this.PDUAct = PDUAct;
			this.isLogin = isLogin;
			this.culture = new CultureInfo("es-CL");
		}

		public override void OnCreate(Bundle savedInstanceState) {
			base.OnCreate(savedInstanceState);
		}

		public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState) {
			return inflater.Inflate(Resource.Layout.FragmentListaRecargas, container, false);
		}

		public override void OnViewCreated(View view, Bundle savedInstanceState) {
			base.OnViewCreated(view, savedInstanceState);

			listaDeudasPDU = view.FindViewById<RecyclerView>(Resource.Id.listaDeudasRecarga);
			bttnPagar = view.FindViewById<RelativeLayout>(Resource.Id.bttnPagar);
			montoTotal = view.FindViewById<TextView>(Resource.Id.montoTotal);

			string cuentasSelected;

			if (isLogin) {
				if (paseDiario.tipoPDU.Equals("extranjeras")) {
					cuentasSelected = paseDiario.idBiller + "," + paseDiario.idServicio + "," + paseDiario.patente + "-" + paseDiario.categoria + "-" + paseDiario.fecha_circulacion + "E";
				} else {
					cuentasSelected = paseDiario.idBiller + "," + paseDiario.idServicio + "," + paseDiario.patente + "-" + paseDiario.categoria + "-" + paseDiario.fecha_circulacion;
				}
			} else {
				PDUAct.carroCompraLayout.Visibility = ViewStates.Gone;
				if (paseDiario.tipoPDU.Equals("extranjeras")) {
					cuentasSelected = paseDiario.idServicio + "," + paseDiario.idBiller + "," + paseDiario.patente + "-" + paseDiario.categoria + "-" + paseDiario.fecha_circulacion + "E";
				} else {
					cuentasSelected = paseDiario.idServicio + "," + paseDiario.idBiller + "," + paseDiario.patente + "-" + paseDiario.categoria + "-" + paseDiario.fecha_circulacion;
				}
			}

			JObject parametros = new JObject();
			AndHUD.Shared.Show(PDUAct, null, -1, MaskType.Black);
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
		}

		public async void buscarCuentasCR(JObject parametros) {
			var response = await MyClass.WorklightClient.UnprotectedInvokeAsync("buscarCuentasCR", "buscar_cuentas_cr", "POST", parametros);

			if (response.Success) {
				if ((int)response.State["Error"] == 0) {
					deudaPDU = setBDeudas(response.Response);

					setValuesListaDeudasPDU();
				} else {
					CustomAlertDialog alert = new CustomAlertDialog(PDUAct, "¡Oops!", response.State["Mensaje"].ToString(), "Aceptar", "", null, null);
					alert.showDialog();
				}
			} else {
				CustomAlertDialog alert = new CustomAlertDialog(PDUAct, "¡Oops!", response.Message, "Aceptar", "", null, null);
				alert.showDialog();
			}
			AndHUD.Shared.Dismiss(PDUAct);
		}

		public async void buscarCuentasCNR(JObject parametros) {
			var response = await MyClass.WorklightClient.UnprotectedInvokeAsync("buscarCuentasCNR", "buscar_cuentas_cnr", "POST", parametros);

			if (response.Success) {
				if ((int)response.State["Error"] == 0) {
					deudaPDU = setBDeudas(response.Response);

					setValuesListaDeudasPDU();
				} else {
					CustomAlertDialog alert = new CustomAlertDialog(PDUAct, "¡Oops!", response.State["Mensaje"].ToString(), "Aceptar", "", null, null);
					alert.showDialog();
				}
			} else {
				CustomAlertDialog alert = new CustomAlertDialog(PDUAct, "¡Oops!", response.Message, "Aceptar", "", null, null);
				alert.showDialog();
			}
			AndHUD.Shared.Dismiss(PDUAct);
		}

		private BuscaDeudas setBDeudas(JObject response) {
			BuscaDeudas bDeuda = new BuscaDeudas();
			var listBDeudas = response["ObtieneBuscaDeuda"];

			bDeuda.S = listBDeudas[0]["S"].ToString();
			bDeuda.acepta_abono = listBDeudas[0]["acepta_abono"].ToString();
			bDeuda.acepta_casa_comercial = listBDeudas[0]["acepta_casa_comercial"].ToString();
			bDeuda.acepta_pago_min = listBDeudas[0]["acepta_pago_min"].ToString();
			bDeuda.acepta_prog = listBDeudas[0]["acepta_prog"].ToString();
			bDeuda.alias = listBDeudas[0]["alias"].ToString();
			bDeuda.boleta = listBDeudas[0]["boleta"].ToString();
			bDeuda.cero = Convert.ToInt32(listBDeudas[0]["cero"].ToString());
			bDeuda.codigo_barra = listBDeudas[0]["codigo_barra"].ToString();
			bDeuda.codigo_tecno = listBDeudas[0]["codigo_tecno"].ToString();
			bDeuda.codigo_tecno2 = listBDeudas[0]["codigo_tecno2"].ToString();
			bDeuda.cuota = listBDeudas[0]["cuota"].ToString();
			bDeuda.descrip_tipo_document = listBDeudas[0]["descrip_tipo_document"].ToString();
			bDeuda.dias_vencimiento = listBDeudas[0]["dias_vencimiento"].ToString();
			bDeuda.direccion = listBDeudas[0]["direccion"].ToString();
			bDeuda.direccion_factura = listBDeudas[0]["direccion_factura"].ToString();
			bDeuda.fecha_prog = listBDeudas[0]["fecha_prog"].ToString();
			bDeuda.fecha_venc = listBDeudas[0]["fecha_venc"].ToString();
			bDeuda.fecha_vencimiento = listBDeudas[0]["fecha_vencimiento"].ToString();
			bDeuda.grafico = listBDeudas[0]["grafico"].ToString();
			bDeuda.id_biller = Convert.ToInt32(listBDeudas[0]["id_biller"].ToString());
			bDeuda.id_estado_pago_solt = Convert.ToInt32(listBDeudas[0]["id_estado_pago_solt"].ToString());
			bDeuda.id_pago_solicitado = Convert.ToInt32(listBDeudas[0]["id_pago_solicitado"].ToString());
			bDeuda.id_periodo_solicitado = listBDeudas[0]["id_periodo_solicitado"].ToString();
			bDeuda.id_secuencia_solicitado = listBDeudas[0]["id_secuencia_solicitado"].ToString();
			bDeuda.id_servicio = Convert.ToInt32(listBDeudas[0]["id_servicio"].ToString());
			bDeuda.identificador = listBDeudas[0]["identificador"].ToString();
			bDeuda.imagen_logo = listBDeudas[0]["imagen_logo"].ToString();
			bDeuda.interes = listBDeudas[0]["interes"].ToString();
			bDeuda.logo_servicio = listBDeudas[0]["logo_servicio"].ToString();
			bDeuda.mensaje = listBDeudas[0]["mensaje"].ToString();
			bDeuda.mensaje_respuesta_usr = listBDeudas[0]["mensaje_respuesta_usr"].ToString();
			bDeuda.moneda = listBDeudas[0]["moneda"].ToString();
			bDeuda.monto_minimo = Convert.ToInt32(listBDeudas[0]["monto_minimo"].ToString());
			bDeuda.monto_origen = listBDeudas[0]["monto_origen"].ToString();
			bDeuda.monto_origen2 = listBDeudas[0]["monto_origen2"].ToString();
			bDeuda.monto_original = listBDeudas[0]["monto_original"].ToString();
			bDeuda.monto_total = Convert.ToInt32(listBDeudas[0]["monto_total"].ToString());
			bDeuda.mostrar_cod_barra = listBDeudas[0]["mostrar_cod_barra"].ToString();
			bDeuda.mostrar_fecha_venc = listBDeudas[0]["mostrar_fecha_venc"].ToString();
			bDeuda.multa = listBDeudas[0]["multa"].ToString();
			bDeuda.nombre_fantasia = listBDeudas[0]["nombre_fantasia"].ToString();
			bDeuda.pnd_prog = listBDeudas[0]["pnd_prog"].ToString();
			bDeuda.rubro = listBDeudas[0]["rubro"].ToString();
			bDeuda.rut_biller = listBDeudas[0]["rut_biller"].ToString();
			bDeuda.telefono = listBDeudas[0]["telefono"].ToString();
			bDeuda.texto_facturador = listBDeudas[0]["texto_facturador"].ToString();
			bDeuda.tipo_cliente = listBDeudas[0]["tipo_cliente"].ToString();
			bDeuda.valor_cambio = listBDeudas[0]["valor_cambio"].ToString();
			bDeuda.valor_cambio2 = listBDeudas[0]["valor_cambio2"].ToString();
			bDeuda.valor_uf = listBDeudas[0]["valor_uf"].ToString();
			bDeuda.webpay = listBDeudas[0]["webpay"].ToString();

			return bDeuda;
		}

		private void setValuesListaDeudasPDU() {
			montoTotal.Text = "Total: " + deudaPDU.monto_total.ToString("C", culture);

			deudasPDUAdapter = new DeudasPDUAdapter(deudaPDU, PDUAct, this);
			divider = ContextCompat.GetDrawable(Context, Resource.Drawable.divider);
			dividerDecoration = new CustomItemDecoration(divider);

			listaDeudasPDU.SetAdapter(deudasPDUAdapter);
			listaDeudasPDU.AddItemDecoration(dividerDecoration);

			layoutManager = new LinearLayoutManager(PDUAct);
			listaDeudasPDU.SetLayoutManager(layoutManager);

			bttnPagar.Click += (sender, e) => {
				JObject param = new JObject();
				AndHUD.Shared.Show(PDUAct, null, -1, MaskType.Black);
				param.Add("sesion", RealmDB.GetInstance().realm.All<PersistentData>().First().idTransaccion);

				PDUAct.pd = paseDiario;
				getMediosPago(param);
			};
		}

		public async void getMediosPago(JObject parametros) {
            var response = await MyClass.WorklightClient.UnprotectedInvokeAsync("nuevoMP", "medios_pago", "POST", parametros);

			if (response.Success) {
				if ((int)response.State["Error"] == 0) {
					if (isLogin) {
						PDUAct.changeMainFragment(new FragmentMediosPago(PDUAct.SupportFragmentManager,
																	 setMediosPago(response.Response),
					                                                 deudaPDU,
																	 false,
																	 false), Resources.GetString(Resource.String.autopista_id_medios_pago_pdu));
					} else {
						PDUAct.listMediosPago = setMediosPago(response.Response);
						PDUAct.changeMainFragment(new FragmentRutEmail(setMediosPago(response.Response), deudaPDU, false, PDUAct),
											  Resources.GetString(Resource.String.autopista_id_rut_email_pdu));
					}

				} else {
					CustomAlertDialog alert = new CustomAlertDialog(PDUAct, "¡Oops!", response.State["Mensaje"].ToString(), "Aceptar", "", null, null);
					alert.showDialog();
				}
			} else {
				CustomAlertDialog alert = new CustomAlertDialog(PDUAct, "¡Oops!", response.Message, "Aceptar", "", null, null);
				alert.showDialog();
			}
			AndHUD.Shared.Dismiss(PDUAct);
		}

		private List<MediosPago> setMediosPago(JObject response) {
			List<MediosPago> list = new List<MediosPago>();

			var listMP = response["MediosPago"];

			for (var i = 0; i < listMP.Count(); i++) {
				list.Add(new MediosPago(
					listMP[i]["descripcion"].ToString(),
					Convert.ToInt32(listMP[i]["forma_pago"].ToString()),
					Convert.ToInt32(listMP[i]["id_banco"].ToString()),
					listMP[i]["logo_banco"].ToString(),
					listMP[i]["orden"].ToString(),
					listMP[i]["url_banco"].ToString(),
					listMP[i]["valor_parametro_banco"].ToString(),
					listMP[i]["valor_popup"].ToString(),
					listMP[i]["principalColor"].ToString(),
					listMP[i]["navigationBarTextTint"].ToString(),
					listMP[i]["darkerPrincipalColor"].ToString(),
					listMP[i]["secondaryColor"].ToString(),
					listMP[i]["mainButtonStyle"].ToString(),
					listMP[i]["hideWebAddressInformationInForm"].ToString(),
					listMP[i]["useBarCenteredLogoInForm"].ToString(),
                    listMP[i]["font"].ToString(),
                     "KH"));
			}

			return list;
		}
	}
}
