using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Android.OS;
using Android.Support.V7.Widget;
using Android.Util;
using Android.Views;
using Android.Widget;
using AndroidHUD;
using Com.Browser2app.Khenshin;
using Com.Bumptech.Glide;
using Newtonsoft.Json.Linq;
using ServipagMobile.Classes;

namespace ServipagMobile.Droid {
	public class MediosPagoAdapter : RecyclerView.Adapter {
		private List<MediosPago> mediosPago;
        private List<Automata> mediosAutomata;
        private List<Automata> AutomatasData;
		private List<BuscaDeudas> misDeudas;
        private int IDEBanco;
		private SolicitaRecarga solicitaRecarga;
		private List<BuscaDeudas> paymentList;
		private string urlImage;
		private int width;
		private string tipoVista;
		private PagoActivity pa;
		private RecargasActivity ra;
        private PDUActivity PDUAct;
		private bool isLogin;
		private bool isUR;

		private BuscaDeudas deudaPDU;
		private bool isUPDU;

		private Utils utils;
		private string pPago = "";
		private int itemCount;
		private int montoTotal = 0;

        public MediosPagoAdapter(List<Automata> mediosAutomata , List<MediosPago> mediosPago, List<BuscaDeudas> misDeudas, string tipoVista, PagoActivity pa, bool isLogin) {
            this.mediosAutomata = mediosAutomata;
            this.mediosPago = mediosPago;
			this.misDeudas = misDeudas;
			this.tipoVista = tipoVista;
			this.pa = pa;
			this.isLogin = isLogin;
			this.urlImage = Properties.GetInstance().url;

			Log.Debug("urlImage", urlImage);

			this.utils = new Utils();
			decimal d = (decimal)mediosPago.Count / 3;
			itemCount = (int)Math.Ceiling(d);
		}

		public MediosPagoAdapter(List<MediosPago> mediosPago, SolicitaRecarga solicitaRecarga, string tipoVista, RecargasActivity ra, bool isLogin, bool isUR) {
			this.mediosPago = mediosPago;
			this.solicitaRecarga = solicitaRecarga;
			this.tipoVista = tipoVista;
			this.ra = ra;
			this.isLogin = isLogin;
			this.isUR = isUR;
			this.urlImage = Properties.GetInstance().url;

			Log.Debug("urlImage", urlImage);

			this.utils = new Utils();
			decimal d = (decimal)mediosPago.Count / 3;
			itemCount = (int)Math.Ceiling(d);
		}

		public MediosPagoAdapter(List<MediosPago> mediosPago, BuscaDeudas deudaPDU, string tipoVista, PDUActivity PDUAct, bool isLogin, bool isUPDU) {
			this.mediosPago = mediosPago;
			this.deudaPDU = deudaPDU;
			this.tipoVista = tipoVista;
			this.PDUAct = PDUAct;
			this.isLogin = isLogin;
			this.isUPDU = isUPDU;
			this.urlImage = Properties.GetInstance().url;

			Log.Debug("urlImage", urlImage);

			this.utils = new Utils();
			decimal d = (decimal)mediosPago.Count / 3;
			itemCount = (int)Math.Ceiling(d);
		}

		public override int ItemCount {
			get {
				return itemCount;
			}
		}

		public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType) {
			View itemView = LayoutInflater.From(parent.Context).Inflate(Resource.Layout.RowVistaMP, parent, false);
			MPVistaViewHolder vh;

			if (ra != null) {
				vh = new MPVistaViewHolder(itemView, tipoVista, ra.Resources.DisplayMetrics.WidthPixels);
			} else if (PDUAct != null) {
				vh = new MPVistaViewHolder(itemView, tipoVista, PDUAct.Resources.DisplayMetrics.WidthPixels);
			} else {
				vh = new MPVistaViewHolder(itemView, tipoVista, pa.Resources.DisplayMetrics.WidthPixels);
			}
			return vh;
		}

		public override void OnBindViewHolder(RecyclerView.ViewHolder holder, int position) {
			MPVistaViewHolder vh = holder as MPVistaViewHolder;

		//	if (tipoVista == "1") {
				for (int i = position * 3; i < mediosPago.Count; i++) {
               
					if ((int)(i / 3) != position) {
						break;
					}

					int mod = i % 3;
					switch (mod) {
						case 0:
						ImageInterface interfaceMPOne = new ImageInterface(mediosPago[i], ((position * 3) + 0), vh.imgMPOne, vh.nameMPOne, hideName);

					if (ra != null) {
						Glide.With(ra)
							.Load("https://www.servipag.com/PortalWS/Content/images/" + mediosPago[i].logo_banco)
							.Listener(interfaceMPOne)
							.Into(vh.imgMPOne);
					} else if (PDUAct != null) {
						Glide.With(PDUAct)
							.Load("https://www.servipag.com/PortalWS/Content/images/" + mediosPago[i].logo_banco)
							.Listener(interfaceMPOne)
							.Into(vh.imgMPOne);
					} else {
						Glide.With(pa)
							.Load("https://www.servipag.com/PortalWS/Content/images/" + mediosPago[i].logo_banco)
							.Listener(interfaceMPOne)
							.Into(vh.imgMPOne);
					}


					vh.imgMPOne.Click += (sender, e) => {
                        pagarBoleta(mediosPago[(position * 3) + 0]);
					};
					break;
					case 1:
					ImageInterface interfaceMPTwo = new ImageInterface(mediosPago[i], ((position * 3) + 1), vh.imgMPTwo, vh.nameMPTwo, hideName);
					if (ra != null) {
						Glide.With(ra)
							.Load("https://www.servipag.com/PortalWS/Content/images/" + mediosPago[i].logo_banco)
					     	.Listener(interfaceMPTwo)
					     	.Into(vh.imgMPTwo);
					} else if (PDUAct != null) {
						Glide.With(PDUAct)
							.Load("https://www.servipag.com/PortalWS/Content/images/" + mediosPago[i].logo_banco)
							 .Listener(interfaceMPTwo)
							 .Into(vh.imgMPTwo);
					} else {
						Glide.With(pa)
							.Load("https://www.servipag.com/PortalWS/Content/images/" + mediosPago[i].logo_banco)
							.Listener(interfaceMPTwo)
					     	.Into(vh.imgMPTwo);
					}

					vh.imgMPTwo.Click += (sender, e) => {
                        pagarBoleta(mediosPago[(position * 3) + 1]);
					};
					break;
					case 2:
					ImageInterface interfaceMPThree = new ImageInterface(mediosPago[i],((position * 3) + 2),  vh.imgMPThree, vh.nameMPThree, hideName);
					if (ra != null) {
						Glide.With(ra)
							.Load("https://www.servipag.com/PortalWS/Content/images/" + mediosPago[i].logo_banco)
						 	.Listener(interfaceMPThree)
					     	.Into(vh.imgMPThree);
					} else if (PDUAct != null) {
						Glide.With(PDUAct)
							.Load("https://www.servipag.com/PortalWS/Content/images/" + mediosPago[i].logo_banco)
							 .Listener(interfaceMPThree)
							 .Into(vh.imgMPThree);
					} else {
						Glide.With(pa)
							.Load("https://www.servipag.com/PortalWS/Content/images/" + mediosPago[i].logo_banco)
							.Listener(interfaceMPThree)
				     		.Into(vh.imgMPThree);
					}

						vh.imgMPThree.Click += (sender, e) => {
							pagarBoleta(mediosPago[(position * 3) + 1]);
						};
						break;
					}
				}
		/*	} 
         else {
				ImageInterface interfaceMP = new ImageInterface(mediosPago[position], ((position * 3) + 1), vh.imgMPTwo, vh.nameMPTwo, hideName);
				
				if (ra != null) {
					Glide.With(ra)
					     .Load(urlImage + "/" + mediosPago[position].logo_banco)
						 .Listener(interfaceMP)
					     .Into(vh.imgMPTwo);
				} else if (PDUAct != null) {
					Glide.With(PDUAct)
						 .Load(urlImage + "/" + mediosPago[position].logo_banco)
						 .Listener(interfaceMP)
						 .Into(vh.imgMPTwo);
				} else {
					Glide.With(pa)
                         
						.Load(urlImage + "/" + mediosPago[position].logo_banco)
						.Listener(interfaceMP)
						 .Into(vh.imgMPTwo);
				}

				vh.imgMPTwo.Click += (sender, e) => {
					pagarBoleta(mediosPago[(position * 3) + 1]);
				};
			}*/
		}

		public async void pagarBoleta(MediosPago medioPago) {
			JObject parametros = new JObject();
			if (ra != null) {
				AndHUD.Shared.Show(ra, null, -1, MaskType.Black);
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
			} else if (PDUAct != null) {
				AndHUD.Shared.Show(PDUAct, null, -1, MaskType.Black);
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
				AndHUD.Shared.Show(pa, null, -1, MaskType.Black);
				getPaymentList(medioPago);
			}

			parametros.Add("canal", DeviceInformation.GetInstance().channel);
			if (isLogin) {
				parametros.Add("sesion", RealmDB.GetInstance().realm.All<PersistentData>().First().cookie);
			} else {
				parametros.Add("sesion", RealmDB.GetInstance().realm.All<PersistentData>().First().idTransaccion);
			}

			if (ra != null) {
				parametros.Add("id_periodo_solicitado", solicitaRecarga.id_periodo_solicitado);
				parametros.Add("id_pago_solicitado", solicitaRecarga.id_pago_solicitado);
			} else if (PDUAct != null) {
				parametros.Add("id_periodo_solicitado", deudaPDU.id_periodo_solicitado);
				parametros.Add("id_pago_solicitado", deudaPDU.id_pago_solicitado);
			} else{
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
			parametros.Add("mail", UserData.GetInstance().correo);
			parametros.Add("parametros_pago", pPago);
			if (ra != null) {
				parametros.Add("total_pagos_bancos", solicitaRecarga.monto_total);
			} else if (PDUAct != null) {
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
                        pa.idPago = response.Response["PagarBoleta"]["id_pago"].ToString();

                        if (medioPago.switcher == "MP")
                        {
                            if (ra != null) {
                                if (isUR) {
                                ra.changeMainFragment(new FragmentWebContext(medioPago, response.Response["PagarBoleta"]["id_pago"].ToString(), isLogin, "recarga"), ra.Resources.GetString(Resource.String.wcontext_ur_id_fragment));
                                } else {
                                ra.changeMainFragment(new FragmentWebContext(medioPago, response.Response["PagarBoleta"]["id_pago"].ToString(), isLogin, "recarga"), ra.Resources.GetString(Resource.String.wcontext_id_fragment));
                                }
                             } else if (PDUAct != null) {
                                    if (isUPDU) {
                                        PDUAct.changeMainFragment(new FragmentWebContext(medioPago, response.Response["PagarBoleta"]["id_pago"].ToString(), isLogin, "pdu"), PDUAct.Resources.GetString(Resource.String.autopista_id_wcontext_updu));
                                    } else {
                                        PDUAct.changeMainFragment(new FragmentWebContext(medioPago, response.Response["PagarBoleta"]["id_pago"].ToString(), isLogin, "pdu"), PDUAct.Resources.GetString(Resource.String.autopista_id_wcontext_pdu));
                                    }
                             } else {
                                pa.changeMainFragment(new FragmentWebContext(medioPago, response.Response["PagarBoleta"]["id_pago"].ToString(), isLogin, "pago"), pa.Resources.GetString(Resource.String.wcontext_id_fragment));
                            }
                        }else{

                            if(medioPago.switcher == "KH"){
                                
                                IDEBanco = 0;
                                IDEBanco = Convert.ToInt32(response.Response["PagarBoleta"]["banco"].ToString());

                                pagoKhipu(response.Response["PagarBoleta"]);
                            }
                        }


					} else {
						CustomAlertDialog alert;
						if (ra != null) {
							alert = new CustomAlertDialog(ra, "¡Oops!", response.Response["PagarBoleta"]["mensaje_retorno"].ToString(), "Aceptar", "", null, null);
						} else if (PDUAct != null) {
							alert = new CustomAlertDialog(PDUAct, "¡Oops!", response.Response["PagarBoleta"]["mensaje_retorno"].ToString(), "Aceptar", "", null, null);
						} else {
							alert = new CustomAlertDialog(pa, "¡Oops!", response.Response["PagarBoleta"]["mensaje_retorno"].ToString(), "Aceptar", "", null, null);
						}
						alert.showDialog();
					}
					if (ra != null) {
						AndHUD.Shared.Dismiss(ra);
					} else if (ra != null) {
						AndHUD.Shared.Dismiss(PDUAct);
					} else {
						AndHUD.Shared.Dismiss(pa);
					}

				} else {
					CustomAlertDialog alert;
					if (ra != null) {
						alert = new CustomAlertDialog(ra, "¡Oops!", response.State["Mensaje"].ToString(), "Aceptar", "", null, null);
					} else if (PDUAct != null) {
						alert = new CustomAlertDialog(PDUAct,"¡Oops!", response.State["Mensaje"].ToString(), "Aceptar", "", null, null);
					} else {
						alert = new CustomAlertDialog(pa, "¡Oops!", response.State["Mensaje"].ToString(), "Aceptar", "", null, null);
					}

					alert.showDialog();

					if (ra != null) {
						AndHUD.Shared.Dismiss(ra);
					} else if (PDUAct != null) {
						AndHUD.Shared.Dismiss(PDUAct);
					} else {
						AndHUD.Shared.Dismiss(pa);
					}
				}
			} else {
				CustomAlertDialog alert;
				if (ra != null) {
					alert = new CustomAlertDialog(ra, "¡Oops!", response.Message, "Aceptar", "", null, null);
				} else if (PDUAct != null) {
					alert = new CustomAlertDialog(PDUAct, "¡Oops!", response.Message, "Aceptar", "", null, null);
				} else {
					alert = new CustomAlertDialog(pa, "¡Oops!", response.Message, "Aceptar", "", null, null);
				}
				alert.showDialog();

				if (ra != null) {
					AndHUD.Shared.Dismiss(ra);
				} else if (PDUAct != null) {
					AndHUD.Shared.Dismiss(PDUAct);
				} else {
					AndHUD.Shared.Dismiss(pa);
				}
			}
		}

        public void pagoKhipu(JToken response) {
			var paymentIntent = Servipag.Current.Khenshin.StartTaskIntent;
            paymentIntent.PutExtra(KhenshinConstants.ExtraAutomatonId, "CHILEDEBITO");
			string rutSinDV = UserData.GetInstance().rut.Substring(0, UserData.GetInstance().rut.Length - 1);

            AutomatasData = mediosAutomata;

            if(AutomatasData.Count > 0)
            {
                var newList = AutomatasData.FindAll(s => s.id_banco == IDEBanco);

                foreach (var list in newList) {
                    var bundle = new Bundle();
                    bundle.PutString("webpayId", "");
                    bundle.PutString("subject", "Pago prueba");
                    bundle.PutString("merchant", "Servipag");
                    bundle.PutString("paymentId", response["id_pago"].ToString());
                    bundle.PutString("ServipagSsnsPrtl", "servipag|" +
                                     723953752 + "|" +
                                     /*rutSinDV.Substring(rutSinDV.Length - 4) +*/ "0000|2017.1"/* +
                             DateTime.Now.ToString("yyyy.MM.dd HH:mm:ss")*/);
                    //bundle.PutString("actionName", "bch");
                    bundle.PutString("actionName", list.actionName);
                    //bundle.PutString("usuario", "EXPRESS");
                    bundle.PutString("usuario", list.usuario);
                    //bundle.PutString("banco", "1");
                    bundle.PutString("banco", list.id_banco.ToString());
                    //bundle.PutString("tipo", "1");
                    bundle.PutString("tipo", list.tipo);
                    //bundle.PutString("cuenta", "0000");
                    bundle.PutString("cuenta", list.cuenta);
                   // bundle.PutString("mail", "emilio.davis@gmail.com");
                    bundle.PutString("mail", list.email);
                    // bundle.PutString("rut2", "0103402948");

                    ParamServiPag ObjParam = new ParamServiPag();

                    if (list.tipovalidacionrut == ObjParam.TipoValrutPunto){
                        
                        bundle.PutString("rut2", formatRutSinPunto(list.Nombreparametrorut));

                    }else{
                        if (list.tipovalidacionrut == ObjParam.TipoValrutSinPunto) {
                            
                            bundle.PutString("rut2", formatRutConPunto(list.Nombreparametrorut));
                        }
                    }
                   
                    bundle.PutString("amount", response["total_pagos_banco"].ToString());

                    Log.Debug("paymentId", response["id_pago"].ToString());
                    Log.Debug("ServipagSsnsPrtl", "ServipagSsnsPrtl", "servipag|" +
                                     723943177 + "|" +
                                     /*rutSinDV.Substring(rutSinDV.Length - 4) +*/ "0000|" +
                                     DateTime.Now.ToString("yyyy.MM.dd HH:mm:ss"));
                    Log.Debug("amount", response["total_pagos_banco"].ToString());

                    paymentIntent.PutExtra(KhenshinConstants.ExtraAutomatonParameters, bundle);
                }
            }
			
			if (ra != null) {
				ra.StartActivityForResult(paymentIntent, 101);
			} else if (PDUAct != null) {
                PDUAct.StartActivityForResult(paymentIntent, 101);
			} else {
				pa.StartActivityForResult(paymentIntent, 101);
			}
		}
        private string formatRutConPunto(string varrut) {
            string rutFormated = "";
            string rutfinal = "";

              //  rutFormated = varrut.Replace(".", "");
                int cantcaract = rutFormated.Length;
                if (cantcaract > 0) {
                    if (cantcaract == 10) {
                        rutfinal = rutFormated;
                    }
                    if (cantcaract == 9) {
                        rutfinal = "0" + rutFormated;
                    }
                    if (cantcaract == 8) {
                        rutfinal = "00" + rutFormated;

                    }
                    if (cantcaract == 7) {

                        rutfinal = "000" + rutFormated;

                    }
                    if (cantcaract == 6) {
                        rutfinal = "0000" + rutFormated;

                    }

                } else {

                    rutfinal = "0000000000";
                }

            return rutfinal;
        }

        private string formatRutSinPunto(string varrut)
        {
            string rutFormated = "";
            string rutfinal = "";


            if(varrut.Contains("."))
            {
                rutFormated = varrut.Replace(".", "");
                int cantcaract = rutFormated.Length;
                if (cantcaract > 0) {
                    if (cantcaract == 10) {
                        rutfinal = rutFormated;
                    }
                    if (cantcaract == 9) {
                        rutfinal = "0" + rutFormated;
                    }
                    if (cantcaract == 8) {
                        rutfinal = "00" + rutFormated;

                    }
                    if (cantcaract == 7) {

                        rutfinal = "000" + rutFormated;

                    }
                    if (cantcaract == 6) {
                        rutfinal = "0000" + rutFormated;

                    }

                } else {

                    rutfinal = "0000000000";
                }
            }
           /* else{

                int cantcaract = rutFormated.Length;
                if (cantcaract > 0) {
                    if (cantcaract == 10) {
                        rutfinal = rutFormated;
                    }
                    if (cantcaract == 9) {
                        rutfinal = "0" + rutFormated;
                    }
                    if (cantcaract == 8) {
                        rutfinal = "00" + rutFormated;

                    }
                    if (cantcaract == 7) {

                        rutfinal = "000" + rutFormated;

                    }
                    if (cantcaract == 6) {
                        rutfinal = "0000" + rutFormated;

                    }

                } else {

                    rutfinal = "0000000000";
                }
            }*/


            return rutfinal;

        }
        private string formatRutConPuntos(string varrut) {
            string rutFormated = "";
            int cantcaract = varrut.Length;


            return rutFormated;

        }
        public async void getAutomatas(JObject parametros) {
            try 
            {
                var responseAutomata = await MyClass.WorklightClient.UnprotectedInvokeAsync("automatasMP", "medios_pago", "POST", parametros);

                if (responseAutomata.Success) {
                    List<Automata> Data = new List<Automata>();
                    Data = setAutomatas(responseAutomata.Response);
                    AutomatasData = Data;
                }
            }catch(Exception ex){


            }
        }

        private List<Automata> setAutomatas(JObject response) {
            List<Automata> list = new List<Automata>();

            var listAuto = response["Automatas"];
           

            for (var i = 0; i < listAuto.Count(); i++) {
                list.Add(new Automata(
                    Convert.ToInt32(listAuto[i]["id_banco"].ToString()),
                    listAuto[i]["rut"].ToString(),
                    listAuto[i]["tipo"].ToString(),
                    listAuto[i]["tx"].ToString(),
                    listAuto[i]["rut2"].ToString(),
                    listAuto[i]["cuenta"].ToString(),
                    listAuto[i]["usuario"].ToString(),
                    listAuto[i]["email"].ToString(),
                    listAuto[i]["actionName"].ToString()));
            }

            return list;


        }

        private List<Automata> ListAutomatas(int idBanco)
        {
           // var response = await MyClass.WorklightClient.UnprotectedInvokeAsync("nuevoMP", "medios_pago", "POST", parametros);
            List<Automata> list = new List<Automata>();
            var newList = list.FindAll(s => s.id_banco == idBanco);
            return newList;
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
