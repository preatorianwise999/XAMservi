using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Android.Graphics.Drawables;
using Android.OS;
using Android.Support.V4.App;
using Android.Support.V4.Content;
using Android.Support.V7.Widget;
using Android.Util;
using Android.Views;
using Android.Widget;
using AndroidHUD;
using Com.Browser2app.Khenshin;
using Com.Bumptech.Glide;
using Newtonsoft.Json.Linq;
//using ServipagMobile.Droid.Controllers.adapters;
using ServipagMobile.Classes;

namespace ServipagMobile.Droid {
	public class FragmentVistaMP : Fragment {
		private string tipoVista;
		private string urlImage;
		private List<MediosPago> mediosPago;
		private List<MediosPago> mediosPagoFull;
        private List<Automata> mediosAutomata;
        //private List<MediosPagoAutomata> mediosPagoAutos;
		private List<BuscaDeudas> misDeudas;
		private List<BuscaDeudas> paymentList;

		private RelativeLayout lastTimeContainer;
		private ImageView lastTimeImage;
		private TextView lastTimeName;
		private TextView lastTimeText;
		private LinearLayout separationLine;
		private RecyclerView mpRow;
		private MediosPagoAdapter adapter;
       // private MediosPagoAdapterAutomata adapterAuto;
		private RelativeLayout containerChangePM;
		private ImageView bttnChangePM;
		private RecyclerView.LayoutManager layoutManager;
		private SolicitaRecarga solicitaRecarga;
		private Utils utils;
		private bool isLogin;
		private bool isUR;
		private BuscaDeudas deudaPDU;
		private bool isUPDU;

		private string pPago = "";
		private int montoTotal = 0;
		private bool isBttnChangePressed = false;

        public FragmentVistaMP(FragmentManager sfm, List<Automata> mediosAutomata , List<MediosPago> mediosPago ,List<MediosPago> mediosPagoFull,List<BuscaDeudas> misDeudas){
            this.mediosAutomata = mediosAutomata;
            this.mediosPago = mediosPago;
            this.mediosPagoFull = mediosPagoFull;
            this.misDeudas = misDeudas;
        }


		public FragmentVistaMP(string tipoVista, string urlImage, List<MediosPago> mediosPago, List<BuscaDeudas> misDeudas, bool isLogin, List<MediosPago> mediosPagoFull) {
			this.tipoVista = tipoVista;
			this.urlImage = urlImage;
			this.mediosPago = mediosPago;
			this.misDeudas = misDeudas;
			this.isLogin = isLogin;
			this.mediosPagoFull = mediosPagoFull;
			this.utils = new Utils();
		}

		public FragmentVistaMP(string tipoVista, string urlImage, List<MediosPago> mediosPago, SolicitaRecarga solicitaRecarga, bool isLogin, bool isUR, List<MediosPago> mediosPagoFull) {
			this.tipoVista = tipoVista;
			this.urlImage = urlImage;
			this.mediosPago = mediosPago;
			this.solicitaRecarga = solicitaRecarga;
			this.isLogin = isLogin;
			this.isUR = isUR;
			this.mediosPagoFull = mediosPagoFull;
			this.utils = new Utils();
		}

		public FragmentVistaMP(string tipoVista, string urlImage, List<MediosPago> mediosPago, BuscaDeudas deudaPDU, bool isLogin, bool isUPDU, List<MediosPago> mediosPagoFull) {
			this.tipoVista = tipoVista;
			this.urlImage = urlImage;
			this.mediosPago = mediosPago;
			this.deudaPDU = deudaPDU;
			this.isLogin = isLogin;
			this.isUPDU = isUPDU;
			this.mediosPagoFull = mediosPagoFull;
			this.utils = new Utils();
		}

		public override void OnCreate(Bundle savedInstanceState) {
			base.OnCreate(savedInstanceState);
		}

		public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState) {
			return inflater.Inflate(Resource.Layout.FragmentVistaMP, container, false);
		}

		public override void OnViewCreated(View view, Bundle savedInstanceState) {
			base.OnViewCreated(view, savedInstanceState);

			lastTimeContainer = view.FindViewById<RelativeLayout>(Resource.Id.lastTimeContainer);
			lastTimeImage = view.FindViewById<ImageView>(Resource.Id.lastTimeImage);
			lastTimeName = view.FindViewById<TextView>(Resource.Id.lastTimeName);
			lastTimeText = view.FindViewById<TextView>(Resource.Id.lastTimeText);
			separationLine = view.FindViewById<LinearLayout>(Resource.Id.separationLine);
			containerChangePM = view.FindViewById<RelativeLayout>(Resource.Id.containerChangePM);
			bttnChangePM = view.FindViewById<ImageView>(Resource.Id.bttnChangePM);
			mpRow = view.FindViewById<RecyclerView>(Resource.Id.mpRow);

			var pData = RealmDB.GetInstance().realm.All<PersistentData>().First();
			var match = mediosPagoFull.Where(obj => obj.id_banco == pData.id_banco);

			if (match.Count() == 1) {
				lastTimeContainer.Visibility = ViewStates.Visible;
				lastTimeText.Visibility = ViewStates.Visible;
				lastTimeImage.Visibility = ViewStates.Visible;
				separationLine.Visibility = ViewStates.Visible;

                int w;
                int h;

				if (solicitaRecarga != null) {
                    var metrics = ((RecargasActivity)Activity).Resources.DisplayMetrics;
					w = Convert.ToInt32((metrics.WidthPixels / 3) - 10);
				}
				else if (deudaPDU != null) {
                    var metrics = ((PDUActivity)Activity).Resources.DisplayMetrics;
                    w = Convert.ToInt32((metrics.WidthPixels / 3) - 10);
				}
				else {
                    var metrics = ((PagoActivity)Activity).Resources.DisplayMetrics;
                    w = Convert.ToInt32((metrics.WidthPixels / 3) - 10);
				}
                h = Convert.ToInt32(w / 1.7f);

				lastTimeImage.LayoutParameters.Width = w;
				lastTimeImage.LayoutParameters.Height = h;
                lastTimeName.LayoutParameters.Width = w;
				lastTimeName.LayoutParameters.Height = h;

				if (isBttnChangePressed) {
					containerChangePM.Visibility = ViewStates.Gone;
					mpRow.Visibility = ViewStates.Visible;
				} else {
					containerChangePM.Visibility = ViewStates.Visible;
					mpRow.Visibility = ViewStates.Gone;

					bttnChangePM.Click += (sender, e) => {
						containerChangePM.Visibility = ViewStates.Gone;
						mpRow.Visibility = ViewStates.Visible;
						isBttnChangePressed = true;
					};
				}

				ImageInterface interfaceMP= new ImageInterface(match.First(), 0, lastTimeImage, lastTimeName, hideName);

				if (solicitaRecarga != null) {
					Glide.With((RecargasActivity)Activity)
					 .Load(/*urlImage + */"https://www.servipag.com/PortalWS/Content/images/" + match.First().logo_banco)
					 .Listener(interfaceMP)
					 .Into(lastTimeImage);
				} else if (deudaPDU != null) {
					Glide.With((PDUActivity)Activity)
					 .Load(/*urlImage + */"https://www.servipag.com/PortalWS/Content/images/" + match.First().logo_banco)
					 .Listener(interfaceMP)
					 .Into(lastTimeImage);
				} else {
					Glide.With((PagoActivity)Activity)
					 .Load(/*urlImage + */"https://www.servipag.com/PortalWS/Content/images/" + match.First().logo_banco)
					 .Listener(interfaceMP)
					 .Into(lastTimeImage);
				}


				lastTimeImage.Click += (sender, e) => {
                    pagarBoleta(match.First());
				};
			}

			if (solicitaRecarga != null) {
				adapter = new MediosPagoAdapter(mediosPago, solicitaRecarga, tipoVista, (RecargasActivity)Activity, isLogin, isUR);

			} else if (deudaPDU != null) {
				adapter = new MediosPagoAdapter(mediosPago, deudaPDU, tipoVista, (PDUActivity)Activity, isLogin, isUPDU);
			} else {
               // string Swither = "MP";

               // if (Swither == "MP"){

                adapter = new MediosPagoAdapter(mediosAutomata,mediosPago, misDeudas, tipoVista, (PagoActivity)Activity, isLogin);

              //  }else{

               //     adapterAuto.MediosPagoAdapterAuto(mediosPagoAutos, misDeudas, tipoVista, (PagoActivity)Activity, isLogin);

              //  }
				
			}

			mpRow.SetAdapter(adapter);

			layoutManager = new LinearLayoutManager(Context);
			mpRow.SetLayoutManager(layoutManager);
		}

        public void pagoKhipu(JToken response) {
            var paymentIntent = Servipag.Current.Khenshin.StartTaskIntent;
            paymentIntent.PutExtra(KhenshinConstants.ExtraAutomatonId, "SCOTIABANKDEBITO");
            string rutSinDV = UserData.GetInstance().rut.Substring(0, UserData.GetInstance().rut.Length - 1);

            var bundle = new Bundle();
            bundle.PutString("webpayId", "");
            bundle.PutString("subject", "Pago prueba");
            bundle.PutString("merchant", "Servipag");
            bundle.PutString("paymentId", response["id_pago"].ToString());
			bundle.PutString("ServipagSsnsPrtl", "servipag|" +
                             UserData.GetInstance().cookie + "|" +
					         rutSinDV.Substring(rutSinDV.Length - 4) + "|" +
					         DateTime.Now.ToString("yyyy.MM.dd HH:mm:ss"));
            bundle.PutString("actionName", "stbk");
            bundle.PutString("usuario", "CLIENTE");
            bundle.PutString("banco", "14");
            bundle.PutString("tipo", "1");
            bundle.PutString("cuenta", "0000");
            bundle.PutString("mail", "daniel.hernandezjara@gmail.com");
            bundle.PutString("rut2", "0168814089");

            paymentIntent.PutExtra(KhenshinConstants.ExtraAutomatonParameters, bundle);


            StartActivityForResult(paymentIntent, 101);
        }

		public async void pagarBoleta(MediosPago medioPago) {
			JObject parametros = new JObject();

			if (solicitaRecarga != null) {
				AndHUD.Shared.Show((RecargasActivity)Activity, null, -1, MaskType.Black);
				pPago = solicitaRecarga.id_biller + "," +
									   solicitaRecarga.id_servicio + "," +
									   solicitaRecarga.identificador + "," +
									   solicitaRecarga.monto_total + "," +
									   utils.getFormaPago(medioPago.forma_pago) + "," +
									   DateTime.Now.ToString("yyyyMMdd") + "," +
									   DateTime.ParseExact(solicitaRecarga.fecha_vencimiento, "dd/MM/yyyy", CultureInfo.InvariantCulture).ToString("yyyyMMdd") + "," +
									   solicitaRecarga.boleta + "," +
									   solicitaRecarga.monto_total + "," +
									   solicitaRecarga.monto_minimo + "," +
									   solicitaRecarga.acepta_pago_min + "," +
									   solicitaRecarga.acepta_abono + "," +
									   solicitaRecarga.texto_facturador + "," +
									   solicitaRecarga.direccion_factura;
				
			} else if (deudaPDU != null) {
				AndHUD.Shared.Show((PDUActivity)Activity, null, -1, MaskType.Black);
				pPago = deudaPDU.id_biller + "," +
									   deudaPDU.id_servicio + "," +
									   deudaPDU.identificador + "," +
									   deudaPDU.monto_total + "," +
									   utils.getFormaPago(medioPago.forma_pago) + "," +
									   DateTime.Now.ToString("yyyyMMdd") + "," +
									   DateTime.ParseExact(deudaPDU.fecha_vencimiento, "dd/MM/yyyy", CultureInfo.InvariantCulture).ToString("yyyyMMdd") + "," +
									   deudaPDU.boleta + "," +
									   deudaPDU.monto_total + "," +
									   deudaPDU.monto_minimo + "," +
									   deudaPDU.acepta_pago_min + "," +
									   deudaPDU.acepta_abono + "," +
									   deudaPDU.texto_facturador + "," +
									   deudaPDU.direccion_factura;
			} else {
				AndHUD.Shared.Show((PagoActivity)Activity, null, -1, MaskType.Black);
				getPaymentList(medioPago);
			}

			parametros.Add("canal", DeviceInformation.GetInstance().channel);
			if (isLogin) {
				parametros.Add("sesion", RealmDB.GetInstance().realm.All<PersistentData>().First().cookie);
			} else {
				parametros.Add("sesion", RealmDB.GetInstance().realm.All<PersistentData>().First().idTransaccion);
			}

			if (solicitaRecarga != null) {
				parametros.Add("id_periodo_solicitado", solicitaRecarga.id_periodo_solicitado);
				parametros.Add("id_pago_solicitado", solicitaRecarga.id_pago_solicitado);
			} else if (deudaPDU != null) {
				parametros.Add("id_periodo_solicitado", deudaPDU.id_periodo_solicitado);
				parametros.Add("id_pago_solicitado", deudaPDU.id_pago_solicitado);
			} else {
				parametros.Add("id_periodo_solicitado", misDeudas[0].id_periodo_solicitado);
				parametros.Add("id_pago_solicitado", misDeudas[0].id_pago_solicitado);
			}

			if (isLogin) {
				parametros.Add("tipo_pago", "CLIENTE");
			} else {
				parametros.Add("tipo_pago", "EXPRESS");
			}
			parametros.Add("total_pagos_webpay", 0);
			parametros.Add("banco", medioPago.id_banco);
			parametros.Add("idUsuario", UserData.GetInstance().rut);
			parametros.Add("popup", "NO");
			parametros.Add("url_banco", medioPago.url_banco);
			parametros.Add("opciones_win", "");
			parametros.Add("mail", "");
			parametros.Add("parametros_pago", pPago);
			if (solicitaRecarga != null) {
				parametros.Add("total_pagos_bancos", solicitaRecarga.monto_total);
			} else if (deudaPDU != null) {
				parametros.Add("total_pagos_bancos", deudaPDU.monto_total);
			} else {
				parametros.Add("total_pagos_bancos", montoTotal);
			}
			parametros.Add("firma", "");

			var pData = RealmDB.GetInstance().realm.All<PersistentData>().First();
			RealmDB.GetInstance().realm.Write(() => pData.id_banco = medioPago.id_banco);

			var response = await MyClass.WorklightClient.UnprotectedInvokeAsync("pagarBoleta", "pagar_boleta", "POST", parametros);

			if (response.Success) {
				if ((int)response.State["Error"] == 0) {
					if (response.Response["PagarBoleta"]["mensaje_retorno"].ToString().Split('|')[0] == "Operacion Exitosa") {
						UserData.GetInstance().cookie = response.Response["PagarBoleta"]["mensaje_retorno"].ToString().Split('|')[1];

                        pagoKhipu(response.Response["PagarBoleta"]);
						/*if (solicitaRecarga != null) {
							if (isUR) {
								((RecargasActivity)Activity).changeMainFragment(new FragmentWebContext(medioPago, response.Response["PagarBoleta"]["id_pago"].ToString(), isLogin, "recarga"), ((RecargasActivity)Activity).Resources.GetString(Resource.String.wcontext_ur_id_fragment));
							} else {
								((RecargasActivity)Activity).changeMainFragment(new FragmentWebContext(medioPago, response.Response["PagarBoleta"]["id_pago"].ToString(), isLogin, "recarga"), ((RecargasActivity)Activity).Resources.GetString(Resource.String.wcontext_id_fragment));
							}
						} else if (deudaPDU != null) {
							if (isUPDU) {
								((PDUActivity)Activity).changeMainFragment(new FragmentWebContext(medioPago, response.Response["PagarBoleta"]["id_pago"].ToString(), isLogin, deudaPDU, "pdu"), ((PDUActivity)Activity).Resources.GetString(Resource.String.autopista_id_wcontext_updu));
							} else {
								((PDUActivity)Activity).changeMainFragment(new FragmentWebContext(medioPago, response.Response["PagarBoleta"]["id_pago"].ToString(), isLogin, deudaPDU, "pdu"), ((PDUActivity)Activity).Resources.GetString(Resource.String.autopista_id_wcontext_pdu));
							}
						} else {
							((PagoActivity)Activity).changeMainFragment(new FragmentWebContext(medioPago, response.Response["PagarBoleta"]["id_pago"].ToString(), isLogin, "pago"), ((PagoActivity)Activity).Resources.GetString(Resource.String.wcontext_id_fragment));
						}*/
					} else {
						CustomAlertDialog alert;
						if (solicitaRecarga != null) {
							alert = new CustomAlertDialog(((RecargasActivity)Activity), "¡Oops!", response.Response["PagarBoleta"]["mensaje_retorno"].ToString(), "Aceptar", "", null, null);
						} else if (deudaPDU != null) {
							alert = new CustomAlertDialog(((PDUActivity)Activity), "¡Oops!", response.Response["PagarBoleta"]["mensaje_retorno"].ToString(), "Aceptar", "", null, null);
						} else {
							alert = new CustomAlertDialog(((PagoActivity)Activity), "¡Oops!", response.Response["PagarBoleta"]["mensaje_retorno"].ToString(), "Aceptar", "", null, null);
						}

						alert.showDialog();
					}

					if (solicitaRecarga != null) {
						AndHUD.Shared.Dismiss(((RecargasActivity)Activity));
					} else if (deudaPDU != null) {
						AndHUD.Shared.Dismiss(((PDUActivity)Activity));
					} else {
						AndHUD.Shared.Dismiss(((PagoActivity)Activity));
					}
				} else {
					CustomAlertDialog alert;
					if (solicitaRecarga != null) {
						alert = new CustomAlertDialog(((RecargasActivity)Activity), "¡Oops!", response.State["Mensaje"].ToString(), "Aceptar", "", null, null);
					} else if (deudaPDU != null) {
						alert = new CustomAlertDialog(((PDUActivity)Activity), "¡Oops!", response.State["Mensaje"].ToString(), "Aceptar", "", null, null);
					} else {
						alert = new CustomAlertDialog(((PagoActivity)Activity), "¡Oops!", response.State["Mensaje"].ToString(), "Aceptar", "", null, null);
					}
					alert.showDialog();

					if (solicitaRecarga != null) {
						AndHUD.Shared.Dismiss(((RecargasActivity)Activity));
					} else if (deudaPDU != null) {
						AndHUD.Shared.Dismiss(((PDUActivity)Activity));
					} else {
						AndHUD.Shared.Dismiss(((PagoActivity)Activity));
					}

				}
			} else {
				CustomAlertDialog alert;
				if (solicitaRecarga != null) {
					alert = new CustomAlertDialog(((RecargasActivity)Activity), "¡Oops!", response.Message, "Aceptar", "", null, null);
				} else if (deudaPDU != null) {
					alert = new CustomAlertDialog(((PDUActivity)Activity), "¡Oops!", response.Message, "Aceptar", "", null, null);
				} else {
					alert = new CustomAlertDialog(((PagoActivity)Activity), "¡Oops!", response.Message, "Aceptar", "", null, null);
				}

				alert.showDialog();

				if (solicitaRecarga != null) {
					AndHUD.Shared.Dismiss(((RecargasActivity)Activity));
				} else if (deudaPDU != null) {
					AndHUD.Shared.Dismiss(((PDUActivity)Activity));
				} else {
					AndHUD.Shared.Dismiss(((PagoActivity)Activity));
				}

			}
		}

		private void getPaymentList(MediosPago medioPago) {
			paymentList = new List<BuscaDeudas>();

			foreach (BuscaDeudas deuda in misDeudas) {
				if (deuda.isSelected) {
					setParametrosPago(deuda, medioPago);
					montoTotal = montoTotal + deuda.monto_total;
				}
			}
		}

		private void setParametrosPago(BuscaDeudas deuda, MediosPago medioPago) {
			if (pPago != "") {
				pPago = pPago + ",";
			}

			pPago = pPago + deuda.id_biller + "," +
				 deuda.id_servicio + "," +
				 deuda.identificador + "," +
				 deuda.monto_total + "," +
				 utils.getFormaPago(medioPago.forma_pago) + "," +
				 DateTime.Now.ToString("yyyyMMdd") + "," +
				 DateTime.ParseExact(deuda.fecha_vencimiento, "dd/MM/yyyy", CultureInfo.InvariantCulture).ToString("yyyyMMdd") + "," +
				 deuda.boleta + "," +
				 deuda.monto_total + "," +
				 deuda.monto_minimo + "," +
				 deuda.acepta_pago_min + "," +
				 deuda.acepta_abono + "," +
				 deuda.texto_facturador + "," +
				 deuda.direccion_factura;

			Log.Debug("ParametrosPago: ", pPago);
		}

		private void hideName(MediosPago medioPago, TextView name, ImageView image, int position) {
			image.Visibility = ViewStates.Gone;
			name.Visibility = ViewStates.Visible;

			name.Text = medioPago.descripcion;
			name.Click += (sender, e) => {
				pagarBoleta(mediosPago[position]);
			};
		}
	}
}
