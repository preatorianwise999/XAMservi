using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Android.Content;
using Android.OS;
using Android.Support.V4.App;
using Android.Util;
using Android.Views;
using Android.Widget;
using AndroidHUD;
using Com.Bumptech.Glide;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Realms;

namespace ServipagMobile.Droid {
	public class FragmentRecargaUR : Fragment {
		private Recargas datosRecarga;
		private SolicitaRecarga datosRecargaPE;
		private SolicitaRecarga solicitaRecargaCR;
		private List<MontoRecarga> listMR;
		private RecargasActivity ra;
		private string urlImage;
		private Validations val;
		private CultureInfo culture { get; set; }

		private TextView tipoRecarga, hintMonto, identificador, hintSubMonto;
		private ImageView imgEmpresaRecarga;
		private EditText fieldMonto;
		private LinearLayout buttonMonto;
		private Button bttnPagarRecarga;
		private int montoSeleccionado;

		public FragmentRecargaUR() { }

		public FragmentRecargaUR(Recargas datosRecarga, List<MontoRecarga> listMR, RecargasActivity ra) {
			this.datosRecarga = datosRecarga;
			this.listMR = listMR;
			this.ra = ra;
			this.val = new Validations();
			this.urlImage = "https://www.servipag.com/PortalWS/Content/images";
			this.culture = new CultureInfo("es-CL");
		}

		public FragmentRecargaUR(SolicitaRecarga datosRecargaPE, List<MontoRecarga> listMR, RecargasActivity ra) {
			this.datosRecargaPE = datosRecargaPE;
			this.listMR = listMR;
			this.ra = ra;
			this.val = new Validations();
			this.urlImage = "https://www.servipag.com/PortalWS/Content/images";
			this.culture = new CultureInfo("es-CL");
		}

		public override void OnCreate(Bundle savedInstanceState) {
			base.OnCreate(savedInstanceState);
		}

		public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState) {
			return inflater.Inflate(Resource.Layout.FragmentRecargaUR, container, false);
		}

		public override void OnViewCreated(View view, Bundle savedInstanceState) {
			base.OnViewCreated(view, savedInstanceState);
			tipoRecarga = view.FindViewById<TextView>(Resource.Id.tipoRecarga);
			imgEmpresaRecarga = view.FindViewById<ImageView>(Resource.Id.imgEmpresaRecarga);
			fieldMonto = view.FindViewById<EditText>(Resource.Id.fieldMonto);
			buttonMonto = view.FindViewById<LinearLayout>(Resource.Id.buttonMonto);
			hintMonto = view.FindViewById<TextView>(Resource.Id.hintMonto);
			hintSubMonto = view.FindViewById<TextView>(Resource.Id.hintSubMonto);
			identificador = view.FindViewById<TextView>(Resource.Id.identificador);
			bttnPagarRecarga = view.FindViewById<Button>(Resource.Id.bttnPagarRecarga);
			solicitaRecargaCR = new SolicitaRecarga();

			if (datosRecarga != null) {
				setTipoRecarga();
				identificador.Text = datosRecarga.codigo_identificacion.Substring(1, datosRecarga.codigo_identificacion.Length-1);
			} else {
				setTipoRecargaPE();
				Glide.With(ra)
				     .Load(urlImage + "/" + datosRecargaPE.logoEmpresa)
				     .Into(imgEmpresaRecarga);

				identificador.Text = datosRecargaPE.identificador.Substring(1, datosRecargaPE.identificador.Length - 1);
			}

			setTipoMonto();

			buttonMonto.Click += (sender, e) => {
				Intent i = new Intent(ra, typeof(CustomNumberPicker));
				i.PutExtra("listMR", JsonConvert.SerializeObject(listMR));
				StartActivityForResult(i, 10);
			};

			fieldMonto.TextChanged += (sender, e) => {
				if (fieldMonto.Text != "") {
					if (val.validAmountOnRange(listMR[0].valor1, listMR[0].valor2, Int32.Parse(fieldMonto.Text))) {
						fieldMonto.SetTextColor(Resources.GetColor(Resource.Color.servipag_green));
					} else {
						fieldMonto.SetTextColor(Resources.GetColor(Resource.Color.servipag_red));
					}
				}
			};

			bttnPagarRecarga.Click += (sender, e) => {
				if (listMR.Count == 1) {
					if (fieldMonto.Text != "") {
						evButtonPagar();
					} else {
						CustomAlertDialog alert = new CustomAlertDialog(ra, Resources.GetString(Resource.String.recargas_id_title), Resources.GetString(Resource.String.recargas_id_monto_dont_type), "Aceptar", "", null, null);
						alert.showDialog();
					}
				} else if (listMR.Count > 1) {
					if (!hintMonto.Text.Equals(Resources.GetString(Resource.String.recargas_id_hint_monto))) {
						evButtonPagar();
					} else {
						CustomAlertDialog alert = new CustomAlertDialog(ra, Resources.GetString(Resource.String.recargas_id_title), Resources.GetString(Resource.String.recargas_id_monto_dont_add), "Aceptar", "", null, null);
						alert.showDialog();
					}
				}
			};
		}

		private void setTipoRecarga() {
			if (datosRecarga.id_servicio == 18) {
				tipoRecarga.Text = Resources.GetString(Resource.String.recargas_nr_title_celular);
			} else if (datosRecarga.id_servicio == 19) {
				tipoRecarga.Text = Resources.GetString(Resource.String.recargas_nr_title_fijo);
			}
		}

		private void setTipoRecargaPE() {
			if (datosRecargaPE.id_servicio == "18") {
				tipoRecarga.Text = Resources.GetString(Resource.String.recargas_nr_title_celular);
			} else if (datosRecargaPE.id_servicio == "19") {
				tipoRecarga.Text = Resources.GetString(Resource.String.recargas_nr_title_fijo);
			}
		}

		private void setTipoMonto() {
			if (listMR.Count == 1) {
				fieldMonto.Visibility = ViewStates.Visible;
				buttonMonto.Visibility = ViewStates.Gone;
				hintSubMonto.Visibility = ViewStates.Visible;
				hintSubMonto.Text = Resources.GetString(Resource.String.recargas_id_hint_monto) + " " +
					listMR[0].valor1.ToString("C", culture) + "-" +
					listMR[0].valor2.ToString("C", culture);
			} else if (listMR.Count > 1){
				fieldMonto.Visibility = ViewStates.Gone;
				buttonMonto.Visibility = ViewStates.Visible;
				hintSubMonto.Visibility = ViewStates.Gone;
			}
		}

		private void evButtonPagar() {
			JObject parametros = new JObject();
			AndHUD.Shared.Show(ra, null, -1, MaskType.Black);

			parametros.Add("sesion", RealmDB.GetInstance().realm.All<PersistentData>().First().idTransaccion);
			parametros.Add("canal", DeviceInformation.GetInstance().channel);
			parametros.Add("firma", "");

			if (listMR.Count == 1) {
				parametros.Add("monto", fieldMonto.Text);
			} else if (montoSeleccionado > 0) {
				parametros.Add("monto", montoSeleccionado);
			} else {
				parametros.Add("monto", ra.solicitaRecarga.monto_total);
			}

			if (datosRecarga != null) {
				parametros.Add("identif", datosRecarga.codigo_identificacion);
				parametros.Add("id_biller", datosRecarga.id_biller);
				parametros.Add("id_servicio", datosRecarga.id_servicio);
				consultaRecarga(parametros, true);
			} else {
				parametros.Add("identif", datosRecargaPE.identificador);
				parametros.Add("id_biller", datosRecargaPE.id_biller);
				parametros.Add("id_servicio", datosRecargaPE.id_servicio);
				consultaRecarga(parametros, false);
			}
		}

		public async void consultaRecarga(JObject parametros, bool isLogin) {
			var respuesta = await MyClass.WorklightClient.UnprotectedInvokeAsync("consultaRecarga", "consulta_recarga", "POST", parametros);

			if (respuesta.Success) {
				if ((int)respuesta.State["Error"] == 0) {
					JObject param = new JObject();
					param.Add("sesion", RealmDB.GetInstance().realm.All<PersistentData>().First().idTransaccion);
					param.Add("canal", DeviceInformation.GetInstance().channel);

					if (isLogin) {
						solicitaRecargaCR.id_periodo_solicitado = respuesta.Response["ObtieneConsultaRecarga"]["id_periodo"].ToString();
						solicitaRecargaCR.id_pago_solicitado = respuesta.Response["ObtieneConsultaRecarga"]["id_pago"].ToString();
						solicitaRecargaCR.nombreBiller = datosRecarga.nombre_biller;

						param.Add("id_periodo_solicitado", solicitaRecargaCR.id_periodo_solicitado);
						param.Add("id_pago_solicitado", solicitaRecargaCR.id_pago_solicitado);
					} else {
						datosRecargaPE.id_periodo_solicitado = respuesta.Response["ObtieneConsultaRecarga"]["id_periodo"].ToString();
						datosRecargaPE.id_pago_solicitado = respuesta.Response["ObtieneConsultaRecarga"]["id_pago"].ToString();

						param.Add("id_periodo_solicitado", datosRecargaPE.id_periodo_solicitado);
						param.Add("id_pago_solicitado", datosRecargaPE.id_pago_solicitado);
					}

					param.Add("tipo_consulta", "NA");
					param.Add("filtro", "NA");
					param.Add("firma", "");

					solicitarRecarga(param, isLogin);
				} else {
					CustomAlertDialog alert = new CustomAlertDialog(ra, "¡Oops!", respuesta.State["Mensaje"].ToString(), "Aceptar", "", null, null);
					alert.showDialog();
					AndHUD.Shared.Dismiss(ra);
				}
			} else {
				CustomAlertDialog alert = new CustomAlertDialog(ra, "¡Oops!", respuesta.Message, "Aceptar", "", null, null);
				alert.showDialog();
				AndHUD.Shared.Dismiss(ra);
			}
		}

		public async void solicitarRecarga(JObject parametros, bool isLogin) {
			var respuesta = await MyClass.WorklightClient.UnprotectedInvokeAsync("solicitaRecarga", "solicita_recarga", "POST", parametros);

			if (respuesta.Success) {
				if ((int)respuesta.State["Error"] == 0) {
					if (isLogin) {
						solicitaRecargaCR.acepta_abono = respuesta.Response["ObtienePagoSolicitado"]["acepta_abono"].ToString();
						solicitaRecargaCR.acepta_pago_min = respuesta.Response["ObtienePagoSolicitado"]["acepta_pago_min"].ToString();
						solicitaRecargaCR.boleta = respuesta.Response["ObtienePagoSolicitado"]["boleta"].ToString();
						solicitaRecargaCR.direccion_factura = respuesta.Response["ObtienePagoSolicitado"]["direccion_factura"].ToString();
						solicitaRecargaCR.fecha_vencimiento = respuesta.Response["ObtienePagoSolicitado"]["fecha_vencimiento"].ToString();
						solicitaRecargaCR.id_biller = respuesta.Response["ObtienePagoSolicitado"]["id_biller"].ToString();
						solicitaRecargaCR.id_servicio = respuesta.Response["ObtienePagoSolicitado"]["id_servicio"].ToString();
						solicitaRecargaCR.identificador = respuesta.Response["ObtienePagoSolicitado"]["identificador"].ToString();
						solicitaRecargaCR.monto_minimo = (int)respuesta.Response["ObtienePagoSolicitado"]["monto_minimo"];
						solicitaRecargaCR.monto_total = (int)respuesta.Response["ObtienePagoSolicitado"]["monto_total"];
						solicitaRecargaCR.texto_facturador = respuesta.Response["ObtienePagoSolicitado"]["texto_facturador"].ToString();

						ra.solicitaRecarga = solicitaRecargaCR;
					} else {
						datosRecargaPE.acepta_abono = respuesta.Response["ObtienePagoSolicitado"]["acepta_abono"].ToString();
						datosRecargaPE.acepta_pago_min = respuesta.Response["ObtienePagoSolicitado"]["acepta_pago_min"].ToString();
						datosRecargaPE.boleta = respuesta.Response["ObtienePagoSolicitado"]["boleta"].ToString();
						datosRecargaPE.direccion_factura = respuesta.Response["ObtienePagoSolicitado"]["direccion_factura"].ToString();
						datosRecargaPE.fecha_vencimiento = respuesta.Response["ObtienePagoSolicitado"]["fecha_vencimiento"].ToString();
						datosRecargaPE.id_biller = respuesta.Response["ObtienePagoSolicitado"]["id_biller"].ToString();
						datosRecargaPE.id_servicio = respuesta.Response["ObtienePagoSolicitado"]["id_servicio"].ToString();
						datosRecargaPE.identificador = respuesta.Response["ObtienePagoSolicitado"]["identificador"].ToString();
						datosRecargaPE.monto_minimo = (int)respuesta.Response["ObtienePagoSolicitado"]["monto_minimo"];
						datosRecargaPE.monto_total = (int)respuesta.Response["ObtienePagoSolicitado"]["monto_total"];
						datosRecargaPE.texto_facturador = respuesta.Response["ObtienePagoSolicitado"]["texto_facturador"].ToString();

						ra.solicitaRecarga = datosRecargaPE;
						saveLastReload();
					}

					JObject p = new JObject();
					p.Add("sesion", RealmDB.GetInstance().realm.All<PersistentData>().First().idTransaccion);
					getMediosPago(p, isLogin);

				} else {
					CustomAlertDialog alert = new CustomAlertDialog(ra, "¡Oops!", respuesta.State["Mensaje"].ToString(), "Aceptar", "", null, null);
					alert.showDialog();
					AndHUD.Shared.Dismiss(ra);
				}
			} else {
				CustomAlertDialog alert = new CustomAlertDialog(ra, "¡Oops!", respuesta.Message, "Aceptar", "", null, null);
				alert.showDialog();
				AndHUD.Shared.Dismiss(ra);
			}
		}

		public async void getMediosPago(JObject parametros, bool isLogin) {
			var response = await MyClass.WorklightClient.UnprotectedInvokeAsync("mediosPago", "medios_pago", "POST", parametros);

			if (response.Success) {
				if ((int)response.State["Error"] == 0) {
					if (isLogin) {
						ra.listMediosPago = setMediosPago(response.Response);
						ra.changeMainFragment(new FragmentMediosPago(ra.SupportFragmentManager,
																	 ra.listMediosPago,
																	 solicitaRecargaCR,
						                                             true,
																	 isLogin), Resources.GetString(Resource.String.recargas_medios_pago_ur_title));
					} else {
						ra.listMediosPago = setMediosPago(response.Response);
						ra.changeMainFragment(new FragmentRutEmail(ra.listMediosPago, datosRecargaPE, true, ra),
											  Resources.GetString(Resource.String.recargas_rut_email_ur_title));
					}

				} else {
					CustomAlertDialog alert = new CustomAlertDialog(ra, "¡Oops!", response.State["Mensaje"].ToString(), "Aceptar", "", null, null);
					alert.showDialog();
				}
			} else {
				CustomAlertDialog alert = new CustomAlertDialog(ra,"¡Oops!", response.Message, "Aceptar", "", null, null);
				alert.showDialog();
			}
			AndHUD.Shared.Dismiss(ra);
		}

		private void saveLastReload() {
			if (RealmDB.GetInstance().realm.All<SolicitaRecargaPE>().Count() == 10) {
				RealmDB.GetInstance().realm.Write(() => {
					var first = RealmDB.GetInstance().realm.All<SolicitaRecargaPE>().First();
					RealmDB.GetInstance().realm.Remove(first);

					var solicitaRecargaPE = new SolicitaRecargaPE {
						id_periodo_solicitado = datosRecargaPE.id_periodo_solicitado,
						id_pago_solicitado = datosRecargaPE.id_pago_solicitado,
						nombreBiller = datosRecargaPE.nombreBiller,
						acepta_abono = datosRecargaPE.acepta_abono,
						acepta_pago_min = datosRecargaPE.acepta_pago_min,
						boleta = datosRecargaPE.boleta,
						direccion_factura = datosRecargaPE.direccion_factura,
						fecha_vencimiento = datosRecargaPE.fecha_vencimiento,
						id_biller = datosRecargaPE.id_biller,
						id_servicio = datosRecargaPE.id_servicio,
						identificador = datosRecargaPE.identificador,
						monto_minimo = datosRecargaPE.monto_minimo,
						monto_total = datosRecargaPE.monto_total,
						texto_facturador = datosRecargaPE.texto_facturador,
						rut = datosRecargaPE.rut,
						email = datosRecargaPE.email,
						logoEmpresa = datosRecargaPE.logoEmpresa
					};
					RealmDB.GetInstance().realm.Add(solicitaRecargaPE);
				});
			} else {
				RealmDB.GetInstance().realm.Write(() => {
					var solicitaRecargaPE = new SolicitaRecargaPE {
						id_periodo_solicitado = datosRecargaPE.id_periodo_solicitado,
						id_pago_solicitado = datosRecargaPE.id_pago_solicitado,
						nombreBiller = datosRecargaPE.nombreBiller,
						acepta_abono = datosRecargaPE.acepta_abono,
						acepta_pago_min = datosRecargaPE.acepta_pago_min,
						boleta = datosRecargaPE.boleta,
						direccion_factura = datosRecargaPE.direccion_factura,
						fecha_vencimiento = datosRecargaPE.fecha_vencimiento,
						id_biller = datosRecargaPE.id_biller,
						id_servicio = datosRecargaPE.id_servicio,
						identificador = datosRecargaPE.identificador,
						monto_minimo = datosRecargaPE.monto_minimo,
						monto_total = datosRecargaPE.monto_total,
						texto_facturador = datosRecargaPE.texto_facturador,
						rut = datosRecargaPE.rut,
						email = datosRecargaPE.email,
						logoEmpresa = datosRecargaPE.logoEmpresa
					};

					RealmDB.GetInstance().realm.Add(solicitaRecargaPE);
				});
			}
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
                    listMP[i]["Switch"].ToString()));
			}

			return list;
		}

		public override void OnActivityResult(int requestCode, int resultCode, Intent data) {
			base.OnActivityResult(requestCode, resultCode, data);
			if (requestCode == 10) {
				montoSeleccionado = Int32.Parse(data.GetStringExtra("montoSeleccionado"));
				if (datosRecarga != null) {
					datosRecarga.monto_pago = montoSeleccionado;
				} else {
					datosRecargaPE.monto_total = montoSeleccionado;
					datosRecargaPE.monto_minimo = montoSeleccionado;
				}
				hintMonto.Text = montoSeleccionado.ToString("C", culture);
				hintMonto.SetTextColor(Resources.GetColor(Resource.Color.servipag_black));
			}
		}
	}
}
