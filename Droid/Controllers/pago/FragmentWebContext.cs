using Android.OS;
using Android.Support.V4.App;
using Android.Views;
using Android.Webkit;

using Android.Content;
using Android.Util;
using System;
using System.Net;
using System.Text;
using Java.Net;
using System.Threading;
using System.Linq;

namespace ServipagMobile.Droid {
	public class FragmentWebContext : Fragment, View.IOnTouchListener {
		private WebView webContext;
		private MediosPago mPago;
		private string idPago;
		private bool isLogin;
		public PagoActivity pa;
		public RecargasActivity ra;
		public PDUActivity PDUAct;
		private string userType;
		private string MP_URL = "https://200.68.48.249:443";
		private string WEB_COOKIE = "servipag|";
		private string NAME_COOKIE = "ServipagSsnsPrtl";
		private string DOMAIN_COOKIE = "200.68.48.249";
		private TimerCallback timerDelegate;
		public int counter = 0;
		public bool isClosing = false;
		private string tipoParent;
		public Timer timer;
		private BuscaDeudas deudaPDU;


		public FragmentWebContext() {}

		public FragmentWebContext(MediosPago mPago, string idPago, bool isLogin, string tipoParent) {
			this.mPago = mPago;
			this.idPago = idPago;
			this.isLogin = isLogin;
			this.tipoParent = tipoParent;
		}

		public FragmentWebContext(MediosPago mPago, string idPago, bool isLogin, BuscaDeudas deudaPDU, string tipoParent) {
			this.mPago = mPago;
			this.idPago = idPago;
			this.isLogin = isLogin;
			this.deudaPDU = deudaPDU;
			this.tipoParent = tipoParent;
		}

		public override void OnCreate(Bundle savedInstanceState) {
			base.OnCreate(savedInstanceState);

			if (tipoParent.Equals("recarga")) {
				this.ra = (RecargasActivity)Activity;
			} else if (tipoParent.Equals("pdu")) {
				this.PDUAct = (PDUActivity)Activity;
			} else {
				this.pa = (PagoActivity)Activity;
			}

		}

		public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState) {
			return inflater.Inflate(Resource.Layout.FragmentWebContext, container, false);
		}

		public override void OnViewCreated(View view, Bundle savedInstanceState) {
			base.OnViewCreated(view, savedInstanceState);
			webContext = view.FindViewById<WebView>(Resource.Id.webView);

			MP_URL = MP_URL + mPago.url_banco;
			string rutSinDV = UserData.GetInstance().rut.Substring(0, UserData.GetInstance().rut.Length - 1);

			if (isLogin) {
				userType = "CLIENTE";
				WEB_COOKIE = WEB_COOKIE +
					UserData.GetInstance().cookie + "|" +
					rutSinDV.Substring(rutSinDV.Length - 4) + "|" +
		            DateTime.Now.ToString("yyyy.MM.dd HH:mm:ss");
			} else {
				userType = "EXPRESS";
				WEB_COOKIE = WEB_COOKIE +
					UserData.GetInstance().cookie + "|" +
					"0000|" +
		            DateTime.Now.ToString("yyyy.MM.dd HH:mm:ss");
			}

			postData(userType);
		}

		private async void postData(string userType) {
			string parameters = "usuario=" + URLEncoder.Encode(userType, "UTF-8") + "&" +
					"banco=" + URLEncoder.Encode(mPago.id_banco.ToString(), "UTF-8") + "&" +
					"tipo=1&" +
					"cuenta=0&" +
					"mail=" + URLEncoder.Encode(UserData.GetInstance().correo, "UTF-8") + "&" +
					"rut2=" + URLEncoder.Encode(UserData.GetInstance().rut.Substring(
							UserData.GetInstance().rut.Length - (UserData.GetInstance().rut.Length - 1)), "UTF-8");

			WebSettings settings = webContext.Settings;
			settings.JavaScriptEnabled = true;
			settings.SetAppCacheEnabled(true);
			settings.SetPluginState(WebSettings.PluginState.On);

			CookieContainer cookieContainer = new CookieContainer();
			Cookie c = new Cookie();
			c.Name = NAME_COOKIE;
			c.Domain = DOMAIN_COOKIE;
			c.Value = WEB_COOKIE;
			cookieContainer.Add(c);

			var cookieManager = Android.Webkit.CookieManager.Instance;
			cookieManager.SetAcceptCookie(true);
			cookieManager.RemoveAllCookie();
			var cookies = cookieContainer.GetCookies(new Uri(MP_URL));
			for (var i = 0; i < cookies.Count; i++) {
				string cookieValue = cookies[i].Value;
				string cookieDomain = cookies[i].Domain;
				string cookieName = cookies[i].Name;
				cookieManager.SetCookie(cookieDomain, cookieName + "=" + cookieValue);
			}
			webContext.SetWebViewClient(new WvClient(showVoucher));
			webContext.PostUrl(MP_URL, Encoding.ASCII.GetBytes(parameters));
			webContext.SetOnTouchListener(this);
			webContext.Settings.LoadWithOverviewMode = true;
			webContext.Settings.UseWideViewPort = true;
			webContext.Settings.BuiltInZoomControls = true;
			webContext.Settings.DisplayZoomControls = false;

			Log.Debug("Parametros", parameters);
			Log.Debug("URL", MP_URL);
			Log.Debug("Cookie", WEB_COOKIE);

			timerDelegate = new TimerCallback(CheckStatus);
			timer = new Timer(timerDelegate, this, 1000, 1000);
		}

		private static void CheckStatus(Object state) {
			FragmentWebContext s = (FragmentWebContext)state;
			s.counter++;

			if (s.isClosing) {
				s.timer.Dispose();
				s.timer = null;
			} else if (s.counter == 300) {
				s.timer.Dispose();
				s.timer = null;

				if (s.tipoParent.Equals("recarga")) {
					s.ra.RunOnUiThread(() => {
						CustomAlertDialog alert = new CustomAlertDialog(s.ra, "¿Estas ahí?",
																	"¿Necesitas más tiempo para terminar tu pago?",
																	"Si",
																	"No", s.reRunTimer, s.closePayment);
						alert.showDialog();
					});
				} else if (s.tipoParent.Equals("pdu")) {
					s.PDUAct.RunOnUiThread(() => {
						CustomAlertDialog alert = new CustomAlertDialog(s.PDUAct, "¿Estas ahí?",
																	"¿Necesitas más tiempo para terminar tu pago?",
																	"Si",
																	"No", s.reRunTimer, s.closePayment);
						alert.showDialog();
					});
				} else {
					s.pa.RunOnUiThread(() => {
						CustomAlertDialog alert = new CustomAlertDialog(s.pa, "¿Estas ahí?",
																	"¿Necesitas más tiempo para terminar tu pago?",
																	"Si",
																	"No", s.reRunTimer, s.closePayment);
						alert.showDialog();
					});
				}
			}
		}

		private void showVoucher() {
			Intent i;

			if (tipoParent.Equals("recarga")) {
				saveLastReloadRecarga();
				i = new Intent(ra, typeof(ComprobanteActivity));
			} else if (tipoParent.Equals("pdu")) {
				saveLastReloadPD();
				i = new Intent(PDUAct, typeof(ComprobanteActivity));
			} else {
				i = new Intent(pa, typeof(ComprobanteActivity));
			}


			i.PutExtra("isPagoExpress", false);
			i.PutExtra("status", "pagoCompletado");
			i.PutExtra("idPago", idPago);

			if (tipoParent.Equals("recarga")) {
				isClosing = true;
				ra.StartActivityForResult(i , 3);
			} else if (tipoParent.Equals("pdu")) {
				isClosing = true;
				PDUAct.StartActivityForResult(i, 3);
			} else {
				pa.StartActivity(i);
				closePayment();
			}
		}

		public  void reRunTimer() {
			counter = 0;
			timer = new Timer(timerDelegate, this, 1000, 1000);
		}

		public void closePayment() {
			Intent intent = new Intent();
			isClosing = true;
			intent.PutExtra("pago", "noAction");

			if (tipoParent.Equals("recarga")) {
				ra.SetResult(Android.App.Result.Ok, intent);
				ra.Finish();
			} else if (tipoParent.Equals("pdu")) {
				PDUAct.SetResult(Android.App.Result.Ok, intent);
				PDUAct.Finish();
			} else {
				pa.SetResult(Android.App.Result.Ok, intent);
				pa.Finish();
			}
		}

		public bool OnTouch(View v, MotionEvent e) {
			counter = 0;
			Log.Debug("Resetea timer", "El timer se ha reseteado debido a una acción del usuario");
			return false;
		}

		private void saveLastReloadRecarga() {
			var existNumber = RealmDB.GetInstance().realm.All<SolicitaRecargaPE>().Where(a => a.identificador == ra.solicitaRecarga.identificador).ToList();

			if (RealmDB.GetInstance().realm.All<SolicitaRecargaPE>().Count() == 10) {
				RealmDB.GetInstance().realm.Write(() => {
					var first = RealmDB.GetInstance().realm.All<SolicitaRecargaPE>().First();
					RealmDB.GetInstance().realm.Remove(first);

					var solicitaRecargaPE = new SolicitaRecargaPE {
						id_periodo_solicitado = ra.solicitaRecarga.id_periodo_solicitado,
						id_pago_solicitado = ra.solicitaRecarga.id_pago_solicitado,
						nombreBiller = ra.solicitaRecarga.nombreBiller,
						acepta_abono = ra.solicitaRecarga.acepta_abono,
						acepta_pago_min = ra.solicitaRecarga.acepta_pago_min,
						boleta = ra.solicitaRecarga.boleta,
						direccion_factura = ra.solicitaRecarga.direccion_factura,
						fecha_vencimiento = ra.solicitaRecarga.fecha_vencimiento,
						id_biller = ra.solicitaRecarga.id_biller,
						id_servicio = ra.solicitaRecarga.id_servicio,
						identificador = ra.solicitaRecarga.identificador,
						monto_minimo = ra.solicitaRecarga.monto_minimo,
						monto_total = ra.solicitaRecarga.monto_total,
						texto_facturador = ra.solicitaRecarga.texto_facturador,
						rut = ra.solicitaRecarga.rut,
						email = ra.solicitaRecarga.email,
						logoEmpresa = ra.servicioRecarga.logo
					};

					RealmDB.GetInstance().realm.Add(solicitaRecargaPE);
				});
			} else if (existNumber.Count > 0) {
				RealmDB.GetInstance().realm.Write(() => {
					RealmDB.GetInstance().realm.Remove(existNumber[0]);

					var solicitaRecargaPE = new SolicitaRecargaPE {
						id_periodo_solicitado = ra.solicitaRecarga.id_periodo_solicitado,
						id_pago_solicitado = ra.solicitaRecarga.id_pago_solicitado,
						nombreBiller = ra.solicitaRecarga.nombreBiller,
						acepta_abono = ra.solicitaRecarga.acepta_abono,
						acepta_pago_min = ra.solicitaRecarga.acepta_pago_min,
						boleta = ra.solicitaRecarga.boleta,
						direccion_factura = ra.solicitaRecarga.direccion_factura,
						fecha_vencimiento = ra.solicitaRecarga.fecha_vencimiento,
						id_biller = ra.solicitaRecarga.id_biller,
						id_servicio = ra.solicitaRecarga.id_servicio,
						identificador = ra.solicitaRecarga.identificador,
						monto_minimo = ra.solicitaRecarga.monto_minimo,
						monto_total = ra.solicitaRecarga.monto_total,
						texto_facturador = ra.solicitaRecarga.texto_facturador,
						rut = ra.solicitaRecarga.rut,
						email = ra.solicitaRecarga.email,
						logoEmpresa = ra.servicioRecarga.logo
					};

					RealmDB.GetInstance().realm.Add(solicitaRecargaPE);
				});
			} else {
				RealmDB.GetInstance().realm.Write(() => {
					var solicitaRecargaPE = new SolicitaRecargaPE {
						id_periodo_solicitado = ra.solicitaRecarga.id_periodo_solicitado,
						id_pago_solicitado = ra.solicitaRecarga.id_pago_solicitado,
						nombreBiller = ra.solicitaRecarga.nombreBiller,
						acepta_abono = ra.solicitaRecarga.acepta_abono,
						acepta_pago_min = ra.solicitaRecarga.acepta_pago_min,
						boleta = ra.solicitaRecarga.boleta,
						direccion_factura = ra.solicitaRecarga.direccion_factura,
						fecha_vencimiento = ra.solicitaRecarga.fecha_vencimiento,
						id_biller = ra.solicitaRecarga.id_biller,
						id_servicio = ra.solicitaRecarga.id_servicio,
						identificador = ra.solicitaRecarga.identificador,
						monto_minimo = ra.solicitaRecarga.monto_minimo,
						monto_total = ra.solicitaRecarga.monto_total,
						texto_facturador = ra.solicitaRecarga.texto_facturador,
						rut = ra.solicitaRecarga.rut,
						email = ra.solicitaRecarga.email,
						logoEmpresa = ra.servicioRecarga.logo
					};

					RealmDB.GetInstance().realm.Add(solicitaRecargaPE);
				});
			}
		}

		private void saveLastReloadPD() {
			if (PDUAct.pd.idBiller.Equals("886")) {
				var existPatent = RealmDB.GetInstance().realm.All<PaseDiario>().Where(a => a.patente == PDUAct.pd.patente).ToList();
				if (RealmDB.GetInstance().realm.All<PaseDiario>().Count() == 15) {
					RealmDB.GetInstance().realm.Write(() => {
						var first = RealmDB.GetInstance().realm.All<PaseDiario>().First();
						RealmDB.GetInstance().realm.Remove(first);

						var pDiario = new PaseDiario {
							nombre_fantasia = deudaPDU.nombre_fantasia,
							fecha_vencimiento = deudaPDU.fecha_vencimiento,
							identificador = UserData.GetInstance().rut,
							monto_total = deudaPDU.monto_total,
							isPDU = true,
							tipoPDU = PDUAct.pd.tipoPDU,
							patente = PDUAct.pd.patente,
							categoria = PDUAct.pd.categoria,
							fecha_circulacion = deudaPDU.fecha_vencimiento,
							idBiller = PDUAct.pd.idBiller,
							idServicio = PDUAct.pd.idServicio,
							isSelected = false

						};
						RealmDB.GetInstance().realm.Add(pDiario);
					});
				} else if (existPatent.Count > 0) {
					RealmDB.GetInstance().realm.Write(() => {
						RealmDB.GetInstance().realm.Remove(existPatent[0]);

						var pDiario = new PaseDiario {
							nombre_fantasia = deudaPDU.nombre_fantasia,
							fecha_vencimiento = deudaPDU.fecha_vencimiento,
							identificador = UserData.GetInstance().rut,
							monto_total = deudaPDU.monto_total,
							isPDU = true,
							tipoPDU = PDUAct.pd.tipoPDU,
							patente = PDUAct.pd.patente,
							categoria = PDUAct.pd.categoria,
							fecha_circulacion = deudaPDU.fecha_vencimiento,
							idBiller = PDUAct.pd.idBiller,
							idServicio = PDUAct.pd.idServicio,
							isSelected = false

						};
						RealmDB.GetInstance().realm.Add(pDiario);
					});
				} else {
					RealmDB.GetInstance().realm.Write(() => {
						var pDiario = new PaseDiario {
							nombre_fantasia = deudaPDU.nombre_fantasia,
							fecha_vencimiento = deudaPDU.fecha_vencimiento,
							identificador = UserData.GetInstance().rut,
							monto_total = deudaPDU.monto_total,
							isPDU = true,
							tipoPDU = PDUAct.pd.tipoPDU,
							patente = PDUAct.pd.patente,
							categoria = PDUAct.pd.categoria,
							fecha_circulacion = deudaPDU.fecha_vencimiento,
							idBiller = PDUAct.pd.idBiller,
							idServicio = PDUAct.pd.idServicio,
							isSelected = false
						};

						RealmDB.GetInstance().realm.Add(pDiario);
					});
				}
			} else if (PDUAct.pd.idBiller.Equals("964")) {
				var existPatent = RealmDB.GetInstance().realm.All<PaseTardio>().Where(a => a.patente == PDUAct.pd.patente).ToList();
				if (RealmDB.GetInstance().realm.All<PaseTardio>().Count() == 15) {
					RealmDB.GetInstance().realm.Write(() => {
						var first = RealmDB.GetInstance().realm.All<PaseTardio>().First();
						RealmDB.GetInstance().realm.Remove(first);

						var pTardio = new PaseTardio {
							nombre_fantasia = deudaPDU.nombre_fantasia,
							fecha_vencimiento = deudaPDU.fecha_vencimiento,
							identificador = UserData.GetInstance().rut,
							monto_total = deudaPDU.monto_total,
							isPDU = false,
							tipoPDU = PDUAct.pd.tipoPDU,
							patente = PDUAct.pd.patente,
							categoria = PDUAct.pd.categoria,
							fecha_circulacion = deudaPDU.fecha_vencimiento,
							idBiller = PDUAct.pd.idBiller,
							idServicio = PDUAct.pd.idServicio,
							isSelected = false

						};
						RealmDB.GetInstance().realm.Add(pTardio);
					});
				} else if (existPatent.Count > 0) {
					RealmDB.GetInstance().realm.Write(() => {
						RealmDB.GetInstance().realm.Remove(existPatent[0]);

						var pTardio = new PaseTardio {
							nombre_fantasia = deudaPDU.nombre_fantasia,
							fecha_vencimiento = deudaPDU.fecha_vencimiento,
							identificador = UserData.GetInstance().rut,
							monto_total = deudaPDU.monto_total,
							isPDU = false,
							tipoPDU = PDUAct.pd.tipoPDU,
							patente = PDUAct.pd.patente,
							categoria = PDUAct.pd.categoria,
							fecha_circulacion = deudaPDU.fecha_vencimiento,
							idBiller = PDUAct.pd.idBiller,
							idServicio = PDUAct.pd.idServicio,
							isSelected = false

						};
						RealmDB.GetInstance().realm.Add(pTardio);
					});
				} else {
					RealmDB.GetInstance().realm.Write(() => {
						var pTardio = new PaseTardio {
							nombre_fantasia = deudaPDU.nombre_fantasia,
							fecha_vencimiento = deudaPDU.fecha_vencimiento,
							identificador = UserData.GetInstance().rut,
							monto_total = deudaPDU.monto_total,
							isPDU = false,
							tipoPDU = PDUAct.pd.tipoPDU,
							patente = PDUAct.pd.patente,
							categoria = PDUAct.pd.categoria,
							fecha_circulacion = deudaPDU.fecha_vencimiento,
							idBiller = PDUAct.pd.idBiller,
							idServicio = PDUAct.pd.idServicio,
							isSelected = false
						};

						RealmDB.GetInstance().realm.Add(pTardio);
					});
				}
			}

		}
	}
}
