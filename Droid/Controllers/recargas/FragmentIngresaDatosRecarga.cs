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
	public class FragmentIngresaDatosRecarga : Fragment {
		private TextView tipoRecarga, hintMonto, hintTyC;
		private ImageView imgEmpresaRecarga;
		private EditText fieldMonto, fieldNumero, fieldRNumero, fieldAlias;
		private LinearLayout buttonMonto;
		private RadioButton radioTyC;
		private Button bttnPagarRecarga;

		private RecargasActivity ra;
		private List<MontoRecarga> listMR;
		private ServiciosRecarga servicioRecarga;
		private string urlImage;
		private CultureInfo culture { get; set; }
		private Validations val;
		private SolicitaRecarga solicitaRecarga;
		private int montoSeleccionado;
		private bool isLogin;
		private Utils utils;

		public FragmentIngresaDatosRecarga() { }

		public FragmentIngresaDatosRecarga(ServiciosRecarga servicioRecarga, List<MontoRecarga>listMR, RecargasActivity ra, bool isLogin) {
			this.servicioRecarga = servicioRecarga;
			this.listMR = listMR;
			this.ra = ra;
			this.isLogin = isLogin;
			this.urlImage = "https://www.servipag.com/PortalWS/Content/images";
			this.culture = new CultureInfo("es-CL");
			this.val = new Validations();
			this.utils = new Utils();
			this.solicitaRecarga = new SolicitaRecarga();
		}

		public override void OnCreate(Bundle savedInstanceState) {
			base.OnCreate(savedInstanceState);
		}

		public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState) {
			return inflater.Inflate(Resource.Layout.FragmentIngresaDatosRecarga, container, false);
		}

		public override void OnViewCreated(View view, Bundle savedInstanceState) {
			base.OnViewCreated(view, savedInstanceState);

			tipoRecarga = view.FindViewById<TextView>(Resource.Id.tipoRecarga);
			imgEmpresaRecarga = view.FindViewById<ImageView>(Resource.Id.imgEmpresaRecarga);
			fieldMonto = view.FindViewById<EditText>(Resource.Id.fieldMonto);
			buttonMonto = view.FindViewById<LinearLayout>(Resource.Id.buttonMonto);
			hintMonto = view.FindViewById<TextView>(Resource.Id.hintMonto);
			fieldNumero = view.FindViewById<EditText>(Resource.Id.fieldNumero);
			fieldRNumero = view.FindViewById<EditText>(Resource.Id.fieldRNumero);
			fieldAlias = view.FindViewById<EditText>(Resource.Id.fieldAlias);
			hintTyC = view.FindViewById<TextView>(Resource.Id.hintTyC);
			radioTyC = view.FindViewById<RadioButton>(Resource.Id.radioTyC);
			bttnPagarRecarga = view.FindViewById<Button>(Resource.Id.bttnPagarRecarga);

			Glide.With(ra)
			     .Load(urlImage + "/" + servicioRecarga.logo)
			     .Into(imgEmpresaRecarga);

			if (isLogin) {
				fieldAlias.Visibility = ViewStates.Visible;
				fieldRNumero.ImeOptions = Android.Views.InputMethods.ImeAction.Next;
			} else {
				fieldAlias.Visibility = ViewStates.Gone;
				fieldRNumero.ImeOptions = Android.Views.InputMethods.ImeAction.Done;
			}
			if (ra.solicitaRecarga != null) {
				if (listMR.Count > 1) {
					hintMonto.Text = ra.solicitaRecarga.monto_total.ToString("C", culture);
				} else if (listMR.Count == 1) {
					fieldMonto.Text = ra.solicitaRecarga.monto_total.ToString("C", culture);
				}
				fieldNumero.Text = ra.solicitaRecarga.identificador;
				fieldRNumero.Text = ra.solicitaRecarga.identificador;
				fieldAlias.Text = ra.solicitaRecarga.nombreBiller;

				fieldNumero.Enabled = true;
				fieldRNumero.Enabled = true;
				fieldAlias.Enabled = true;
				radioTyC.Enabled = true;

				hintMonto.SetTextColor(Resources.GetColor(Resource.Color.servipag_black));
				fieldNumero.SetBackgroundColor(Resources.GetColor(Resource.Color.servipag_white));
				fieldRNumero.SetBackgroundColor(Resources.GetColor(Resource.Color.servipag_white));
				fieldAlias.SetBackgroundColor(Resources.GetColor(Resource.Color.servipag_white));
				fieldNumero.SetTextColor(Resources.GetColor(Resource.Color.servipag_green));
				fieldRNumero.SetTextColor(Resources.GetColor(Resource.Color.servipag_green));
				hintTyC.SetTextColor(Resources.GetColor(Resource.Color.servipag_black));
			}
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

			fieldNumero.TextChanged += (sender, e) => {
				if (val.validPhoneNumber(fieldNumero.Text)) {
					fieldNumero.SetTextColor(Resources.GetColor(Resource.Color.servipag_green));
				} else {
					fieldNumero.SetTextColor(Resources.GetColor(Resource.Color.servipag_red));
				}
			};

			fieldRNumero.TextChanged += (sender, e) => {
				if (val.validPhoneNumber(fieldRNumero.Text)) {
					fieldRNumero.SetTextColor(Resources.GetColor(Resource.Color.servipag_green));
				} else {
					fieldRNumero.SetTextColor(Resources.GetColor(Resource.Color.servipag_red));
				}
			};

			radioTyC.Click += (sender, e) => {
				ra.changeMainFragment(new FragmentTerminosCondiciones(),
				                      Resources.GetString(Resource.String.recargas_id_hint_term_cond));
			};

			bttnPagarRecarga.Click += (sender, e) => {
				evButtonPagar();
			};

			hasAmountPopUp();
			setTipoRecarga();
			isAmountSet();

		}

		private void evButtonPagar() {
			Dictionary<string, string> fields = new Dictionary<string, string>();
			if (listMR.Count == 1) {
				fields.Add(Resources.GetString(Resource.String.recargas_id_hint_monto), fieldMonto.Text);
			} else {
				fields.Add(Resources.GetString(Resource.String.recargas_id_hint_monto), hintMonto.Text);
			}

			fields.Add(Resources.GetString(Resource.String.recargas_cel_id_hint_n_celular), fieldNumero.Text);
			fields.Add(Resources.GetString(Resource.String.recargas_cel_id_hint_r_celular), fieldRNumero.Text);
			if (isLogin) {
				fields.Add(Resources.GetString(Resource.String.recargas_id_hint_alias), fieldAlias.Text);
			}


			if ((bool)val.isEmpty(fields)["code"]) {
				CustomAlertDialog alert =
					new CustomAlertDialog(ra,
										  "¡Ojo!",
										  (string)val.isEmpty(fields)["data"],
										  "Aceptar",
										  "",
										  null,
										  null);
				alert.showDialog();
			} else if (listMR.Count == 1 && !val.validAmountOnRange(listMR[0].valor1, listMR[0].valor2, Int32.Parse(fieldMonto.Text))) {
				CustomAlertDialog alert =
					new CustomAlertDialog(ra,
										  "¡Ojo!",
										  Resources.GetString(Resource.String.recargas_id_monto_out_range),
										  "Aceptar",
										  "",
										  null,
										  null);
				alert.showDialog();
			} else if (listMR.Count > 1 && hintMonto.Text.Equals(Resources.GetString(Resource.String.recargas_id_hint_monto))) {
				CustomAlertDialog alert =
					new CustomAlertDialog(ra,
										  "¡Ojo!",
										  Resources.GetString(Resource.String.recargas_id_monto_dont_add),
										  "Aceptar",
										  "",
										  null,
										  null);
				alert.showDialog();
			} else if (!val.validPhoneNumber(fieldNumero.Text) || !val.validPhoneNumber(fieldRNumero.Text)) {
				CustomAlertDialog alert =
					new CustomAlertDialog(ra,
										  "¡Ojo!",
										  Resources.GetString(Resource.String.recargas_id_number_out_digits),
										  "Aceptar",
										  "",
										  null,
										  null);
				alert.showDialog();
			} else if (!(bool)val.areEquals(fieldNumero.Text, fieldRNumero.Text, "números telefónicos")["code"]) {
				CustomAlertDialog alert =
					new CustomAlertDialog(ra,
										  "¡Ojo!",
										  (string)val.areEquals(fieldNumero.Text, fieldRNumero.Text, "números telefónicos")["data"],
										  "Aceptar",
										  "",
										  null,
										  null);
				alert.showDialog();
			} else {
				JObject parametros = new JObject();
				AndHUD.Shared.Show(ra, null, -1, MaskType.Black);

				if (isLogin) {
					parametros.Add("sesion", RealmDB.GetInstance().realm.All<PersistentData>().First().cookie);
				} else {
					parametros.Add("sesion", RealmDB.GetInstance().realm.All<PersistentData>().First().idTransaccion);
				}
				parametros.Add("canal", DeviceInformation.GetInstance().channel);
				parametros.Add("identif", fieldNumero.Text);
				parametros.Add("id_biller", servicioRecarga.id_biller);
				parametros.Add("id_servicio", servicioRecarga.id_servicio);
				if (listMR.Count == 1) {
					parametros.Add("monto", fieldMonto.Text);
				} else if (montoSeleccionado > 0) {
					parametros.Add("monto", montoSeleccionado);
				} else {
					parametros.Add("monto", ra.solicitaRecarga.monto_total);
				}

				parametros.Add("firma", "");

				consultaRecarga(parametros);
			}
		}

		public override void OnActivityResult(int requestCode, int resultCode, Intent data) {
			base.OnActivityResult(requestCode, resultCode, data);
			if (requestCode == 10) {
				montoSeleccionado = Int32.Parse(data.GetStringExtra("montoSeleccionado"));
				hintMonto.Text =  montoSeleccionado.ToString("C", culture);
				hintMonto.SetTextColor(Resources.GetColor(Resource.Color.servipag_black));
				fieldNumero.Enabled = true;
				fieldRNumero.Enabled = true;
				fieldAlias.Enabled = true;
				radioTyC.Enabled = true;

				fieldNumero.SetBackgroundColor(Resources.GetColor(Resource.Color.servipag_white));
				fieldRNumero.SetBackgroundColor(Resources.GetColor(Resource.Color.servipag_white));
				fieldAlias.SetBackgroundColor(Resources.GetColor(Resource.Color.servipag_white));
				hintTyC.SetTextColor(Resources.GetColor(Resource.Color.servipag_black));
			}
		}

		private void hasAmountPopUp() {
			if (listMR.Count > 1) {
				fieldMonto.Visibility = ViewStates.Gone;
				buttonMonto.Visibility = ViewStates.Visible;
			} else {
				fieldMonto.Hint = Resources.GetString(Resource.String.recargas_id_hint_monto) + " " +
						listMR[0].valor1.ToString("C", culture) + "-" + listMR[0].valor2.ToString("C", culture);
				fieldMonto.Visibility = ViewStates.Visible;
				buttonMonto.Visibility = ViewStates.Gone;
			}
		}

		private void setTipoRecarga() {
			if (servicioRecarga.id_servicio == 18) {
				tipoRecarga.Text = Resources.GetString(Resource.String.recargas_nr_title_celular);
			} else if (servicioRecarga.id_servicio == 19) {
				tipoRecarga.Text = Resources.GetString(Resource.String.recargas_nr_title_fijo);
			}
		}

		private void isAmountSet() {
			if (listMR.Count == 1) {
				fieldMonto.TextChanged += (sender, e) => {
					if (fieldMonto.Text.Equals("")) {
						fieldNumero.Enabled = false;
						fieldRNumero.Enabled = false;
						fieldAlias.Enabled = false;
						radioTyC.Enabled = false;

						fieldNumero.SetBackgroundColor(Resources.GetColor(Resource.Color.servipag_almost_white));
						fieldRNumero.SetBackgroundColor(Resources.GetColor(Resource.Color.servipag_almost_white));
						fieldAlias.SetBackgroundColor(Resources.GetColor(Resource.Color.servipag_almost_white));
						hintTyC.SetTextColor(Resources.GetColor(Resource.Color.servipag_grey));
					} else {
						fieldNumero.Enabled = true;
						fieldRNumero.Enabled = true;
						fieldAlias.Enabled = true;
						radioTyC.Enabled = true;

						fieldNumero.SetBackgroundColor(Resources.GetColor(Resource.Color.servipag_white));
						fieldRNumero.SetBackgroundColor(Resources.GetColor(Resource.Color.servipag_white));
						fieldAlias.SetBackgroundColor(Resources.GetColor(Resource.Color.servipag_white));
						hintTyC.SetTextColor(Resources.GetColor(Resource.Color.servipag_black));
					}
				};
			}
		}

		public async void consultaRecarga(JObject parametros) {
			var respuesta = await MyClass.WorklightClient.UnprotectedInvokeAsync("consultaRecarga", "consulta_recarga", "POST", parametros);

			if (respuesta.Success) {
				if ((int)respuesta.State["Error"] == 0) {
					solicitaRecarga.id_periodo_solicitado = respuesta.Response["ObtieneConsultaRecarga"]["id_periodo"].ToString();
					solicitaRecarga.id_pago_solicitado = respuesta.Response["ObtieneConsultaRecarga"]["id_pago"].ToString();

					JObject param = new JObject();
					if (isLogin) {
						param.Add("sesion", RealmDB.GetInstance().realm.All<PersistentData>().First().cookie);
					} else {
						param.Add("sesion", RealmDB.GetInstance().realm.All<PersistentData>().First().idTransaccion);
					}

					param.Add("canal", DeviceInformation.GetInstance().channel);
					param.Add("id_periodo_solicitado", solicitaRecarga.id_periodo_solicitado);
					param.Add("id_pago_solicitado", solicitaRecarga.id_pago_solicitado);
					param.Add("tipo_consulta", "NA");
					param.Add("filtro", "NA");
					param.Add("firma", "");
					solicitarRecarga(param);
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

		public async void solicitarRecarga(JObject parametros) {
			var respuesta = await MyClass.WorklightClient.UnprotectedInvokeAsync("solicitaRecarga", "solicita_recarga", "POST", parametros);

			if (respuesta.Success) {
				if ((int)respuesta.State["Error"] == 0) {
					solicitaRecarga.acepta_abono = respuesta.Response["ObtienePagoSolicitado"]["acepta_abono"].ToString();
					solicitaRecarga.acepta_pago_min = respuesta.Response["ObtienePagoSolicitado"]["acepta_pago_min"].ToString();
					solicitaRecarga.boleta = respuesta.Response["ObtienePagoSolicitado"]["boleta"].ToString();
					solicitaRecarga.direccion_factura = respuesta.Response["ObtienePagoSolicitado"]["direccion_factura"].ToString();
					solicitaRecarga.fecha_vencimiento = respuesta.Response["ObtienePagoSolicitado"]["fecha_vencimiento"].ToString();
					solicitaRecarga.id_biller = respuesta.Response["ObtienePagoSolicitado"]["id_biller"].ToString();
					solicitaRecarga.id_servicio = respuesta.Response["ObtienePagoSolicitado"]["id_servicio"].ToString();
					solicitaRecarga.identificador = respuesta.Response["ObtienePagoSolicitado"]["identificador"].ToString();
					solicitaRecarga.monto_minimo = (int)respuesta.Response["ObtienePagoSolicitado"]["monto_minimo"];
					solicitaRecarga.monto_total = (int)respuesta.Response["ObtienePagoSolicitado"]["monto_total"];
					solicitaRecarga.texto_facturador = respuesta.Response["ObtienePagoSolicitado"]["texto_facturador"].ToString();
					solicitaRecarga.nombreBiller = servicioRecarga.nombre;
					               
					ra.solicitaRecarga = solicitaRecarga;

					ra.changeMainFragment(new FragmentListaRecargas(solicitaRecarga, ra, isLogin),
					                      Resources.GetString(Resource.String.recargas_deudas_title));
				} else {
					CustomAlertDialog alert = new CustomAlertDialog(ra, "¡Oops!", respuesta.State["Mensaje"].ToString(), "Aceptar", "", null, null);
					alert.showDialog();
				}
			} else {
				CustomAlertDialog alert = new CustomAlertDialog(ra, "¡Oops!", respuesta.Message, "Aceptar", "", null, null);
				alert.showDialog();
			}

			AndHUD.Shared.Dismiss(ra);
		}
	}
}
