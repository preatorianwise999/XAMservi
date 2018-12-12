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
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace ServipagMobile.Droid {
	public class FragmentUltimasRecargas : Fragment {
		private LinearLayout containerUR, containerWUR;
		private TextView ttlLastPDU;
		private UltimasRecargasAdapter urAdater;
		private RecyclerView.LayoutManager layoutManager;
		private Drawable divider;
		private RecyclerView.ItemDecoration dividerDecoration;
		private RecyclerView listURecargas;
		private Button nuevaRecarga, pagar;

		private List<Recargas> listUltimasRecargas;
		private List<SolicitaRecarga> listURPE;
		private List<ServiciosRecarga> listSRMovil;
		private List<ServiciosRecarga> listSRFijo;
		private bool isLogin;
		private string badgeCount;
		private MainActivity ma;


		public FragmentUltimasRecargas() { }
		public FragmentUltimasRecargas(List<Recargas> listUltimasRecargas, bool isLogin, string badgeCount, MainActivity ma) {
			this.listUltimasRecargas = listUltimasRecargas;
			this.isLogin = isLogin;
			this.badgeCount = badgeCount;
			this.ma = ma;
		}

		public FragmentUltimasRecargas(List<SolicitaRecarga> listURPE, bool isLogin, string badgeCount, MainActivity ma) {
			this.listURPE = listURPE;
			this.isLogin = isLogin;
			this.badgeCount = badgeCount;
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
			ttlLastPDU = view.FindViewById<TextView>(Resource.Id.ttlLastPDU);
			containerUR = view.FindViewById<LinearLayout>(Resource.Id.containerUR);
			containerWUR = view.FindViewById<LinearLayout>(Resource.Id.containerWUR);
			listURecargas = view.FindViewById<RecyclerView>(Resource.Id.listURecargas);
			nuevaRecarga = view.FindViewById<Button>(Resource.Id.nuevaRecarga);
			pagar = view.FindViewById<Button>(Resource.Id.pagar);

			ma.carroCompraLayout.Visibility = ViewStates.Gone;
			if (isLogin) {
				if (listUltimasRecargas.Count > 0) {
					ttlLastPDU.Text = Resources.GetString(Resource.String.recargas_title_recargas_f);
					pagar.Visibility = ViewStates.Visible;
					containerUR.Visibility = ViewStates.Visible;
					containerWUR.Visibility = ViewStates.Gone;

					nuevaRecarga.SetWidth(metrics.WidthPixels / 2);
					pagar.SetWidth(metrics.WidthPixels / 2);

					urAdater = new UltimasRecargasAdapter(listUltimasRecargas);
					divider = ContextCompat.GetDrawable(Context, Resource.Drawable.divider);
					dividerDecoration = new CustomItemDecoration(divider);

					listURecargas.SetAdapter(urAdater);
					listURecargas.AddItemDecoration(dividerDecoration);
					layoutManager = new LinearLayoutManager((MainActivity)Activity);
					listURecargas.SetLayoutManager(layoutManager);
				} else {
					pagar.Visibility = ViewStates.Gone;
					containerUR.Visibility = ViewStates.Gone;
					containerWUR.Visibility = ViewStates.Visible;
					nuevaRecarga.SetWidth(metrics.WidthPixels);
				}
			} else {
				if (listURPE.Count > 0) {
					ttlLastPDU.Text = Resources.GetString(Resource.String.recargas_title_u_recargas);
					pagar.Visibility = ViewStates.Visible;
					containerUR.Visibility = ViewStates.Visible;
					containerWUR.Visibility = ViewStates.Gone;

					nuevaRecarga.SetWidth(metrics.WidthPixels / 2);
					pagar.SetWidth(metrics.WidthPixels / 2);

					urAdater = new UltimasRecargasAdapter(listURPE);
					divider = ContextCompat.GetDrawable(Context, Resource.Drawable.divider);
					dividerDecoration = new CustomItemDecoration(divider);

					listURecargas.SetAdapter(urAdater);
					listURecargas.AddItemDecoration(dividerDecoration);
					layoutManager = new LinearLayoutManager((MainActivity)Activity);
					listURecargas.SetLayoutManager(layoutManager);
				} else {
					pagar.Visibility = ViewStates.Gone;
					containerUR.Visibility = ViewStates.Gone;
					containerWUR.Visibility = ViewStates.Visible;
					nuevaRecarga.SetWidth(metrics.WidthPixels);
				}
			}

			nuevaRecarga.Click += (sender, e) => {
				JObject parametros = new JObject();
				AndHUD.Shared.Show(ma, null, -1, MaskType.Black);

				parametros.Add("sesion", RealmDB.GetInstance().realm.All<PersistentData>().First().idTransaccion);
				parametros.Add("idServicio", "18");

				getServiciosRecarga(parametros, false);
			};

			pagar.Click += (sender, e) => {
				JObject parametros = new JObject();
				AndHUD.Shared.Show(ma, null, -1, MaskType.Black);

				parametros.Add("sesion", RealmDB.GetInstance().realm.All<PersistentData>().First().idTransaccion);
				if (listUltimasRecargas != null) {
					parametros.Add("idServicio", listUltimasRecargas.Single(s => s.isSelected == true).id_servicio);
					parametros.Add("idBiller", listUltimasRecargas.Single(s => s.isSelected == true).id_biller);
				} else if (listURPE != null) {
					parametros.Add("idServicio", listURPE.Single(s => s.isSelected == true).id_servicio);
					parametros.Add("idBiller", listURPE.Single(s => s.isSelected == true).id_biller);
				}

				getMontoRecarga(parametros);
			};
		}

		public override void OnActivityResult(int requestCode, int resultCode, Intent data) {
			base.OnActivityResult(requestCode, resultCode, data);
			switch (requestCode) {
				case 2:
				switch (data.GetStringExtra("recargas")) {
					case "reloadUR":
						if (isLogin) {
							JObject parametros = new JObject();
							AndHUD.Shared.Show(ma, null, -1, MaskType.Black);

							parametros.Add("canal", DeviceInformation.GetInstance().channel);
							parametros.Add("sesion", RealmDB.GetInstance().realm.All<PersistentData>().First().cookie);
							parametros.Add("firma", "");
							parametros.Add("idUsuario", UserData.GetInstance().rut);

							getUltimasRecargas(parametros);
						} else {
							listURPE.Clear();
							foreach (SolicitaRecargaPE srpe in RealmDB.GetInstance().realm.All<SolicitaRecargaPE>()) {
								var sr = new SolicitaRecarga();
								sr.id_periodo_solicitado = srpe.id_periodo_solicitado;
								sr.id_pago_solicitado = srpe.id_pago_solicitado;
								sr.nombreBiller = srpe.nombreBiller;
								sr.acepta_abono = srpe.acepta_abono;
								sr.acepta_pago_min = srpe.acepta_pago_min;
								sr.boleta = srpe.boleta;
								sr.direccion_factura = srpe.direccion_factura;
								sr.fecha_vencimiento = srpe.fecha_vencimiento;
								sr.id_biller = srpe.id_biller;
								sr.id_servicio = srpe.id_servicio;
								sr.identificador = srpe.identificador;
								sr.monto_minimo = srpe.monto_minimo;
								sr.monto_total = srpe.monto_total;
								sr.texto_facturador = srpe.texto_facturador;
								sr.rut = srpe.rut;
								sr.email = srpe.email;
								sr.isSelected = false;
								sr.logoEmpresa = srpe.logoEmpresa;
								listURPE.Add(sr);
							}
							listURPE[0].isSelected = true;
							ma.listURPE = listURPE;
							
							var metrics = Resources.DisplayMetrics;
							pagar.Visibility = ViewStates.Visible;
							containerUR.Visibility = ViewStates.Visible;
							containerWUR.Visibility = ViewStates.Gone;

							nuevaRecarga.SetWidth(metrics.WidthPixels / 2);
							pagar.SetWidth(metrics.WidthPixels / 2);

							urAdater = new UltimasRecargasAdapter(listURPE);
							divider = ContextCompat.GetDrawable(Context, Resource.Drawable.divider);
							dividerDecoration = new CustomItemDecoration(divider);

							listURecargas.SetAdapter(urAdater);
							listURecargas.AddItemDecoration(dividerDecoration);
							layoutManager = new LinearLayoutManager((MainActivity)Activity);
							listURecargas.SetLayoutManager(layoutManager);
						}
					break;
					case "abreCarroCompra":
						ma.carroCompraLayout.Visibility = ViewStates.Gone;
						ma.sortMyAccounts.Visibility = ViewStates.Visible;
						ma.addAccount.Visibility = ViewStates.Visible;

						ma.isCarroCompra = true;
						ma.isLogin = false;
						ma.childFragment = new FragmentListaCuentas(isLogin, ma.listAdd);
						ma.changeMainFragment(ma.childFragment, "mCarroCompra");
					break;
				}
				break;
			}
		}

		public async void getUltimasRecargas(JObject parametros) {
			var respuesta = await MyClass.WorklightClient.UnprotectedInvokeAsync("ultimasRecargas", "ultimas_recargas", "POST", parametros);

			if (respuesta.Success) {
				if ((int)respuesta.State["Error"] == 0) {
					listUltimasRecargas = setListUltimasRecargas(respuesta.Response);
					urAdater.reloadListUR(listUltimasRecargas);
				} else {
					CustomAlertDialog alert = new CustomAlertDialog(ma, "¡Oops!", respuesta.State["Mensaje"].ToString(), "Aceptar", "", null, null);
					alert.showDialog();
				}
			} else {
				CustomAlertDialog alert = new CustomAlertDialog(ma, "¡Oops!", respuesta.Message, "Aceptar", "", null, null);
				alert.showDialog();
			}
			AndHUD.Shared.Dismiss(ma);
		}

		public async void getMontoRecarga(JObject parametros) {
			var respuesta = await MyClass.WorklightClient.UnprotectedInvokeAsync("montoRecarga", "monto_recarga", "POST", parametros);

			if (respuesta.Success) {
				if ((int)respuesta.State["Error"] == 0) {
					Intent i = new Intent(ma, typeof(RecargasActivity));

					i.PutExtra("isLogin", isLogin);
					i.PutExtra("badgeCount", badgeCount);

					if (listUltimasRecargas != null) {
						i.PutExtra("isUR", true);
						i.PutExtra("datosRecarga", JsonConvert.SerializeObject(listUltimasRecargas.Single(s => s.isSelected == true)));
						i.PutExtra("listMontoRecarga", JsonConvert.SerializeObject(setListMontoRecarga(respuesta.Response)));
					} else if (listURPE != null) {
						i.PutExtra("isUR", true);
						i.PutExtra("datosRecarga", JsonConvert.SerializeObject(listURPE.Single(s => s.isSelected == true)));
						i.PutExtra("listMontoRecarga", JsonConvert.SerializeObject(setListMontoRecarga(respuesta.Response)));
					}

					i.PutExtra("listSRMovil", JsonConvert.SerializeObject(listSRMovil));
					i.PutExtra("listSRFijo", JsonConvert.SerializeObject(listSRFijo));

					StartActivityForResult(i, 2);

				} else {
					CustomAlertDialog alert = new CustomAlertDialog(ma, "¡Oops!", respuesta.State["Mensaje"].ToString(), "Aceptar", "", null, null);
					alert.showDialog();
				}
			} else {
				CustomAlertDialog alert = new CustomAlertDialog(ma, "¡Oops!", respuesta.Message, "Aceptar", "", null, null);
				alert.showDialog();
			}
			AndHUD.Shared.Dismiss(ma);
		}

		public async void getServiciosRecarga(JObject parametros, bool isLast) {
			var respuesta = await MyClass.WorklightClient.UnprotectedInvokeAsync("serviciosRecarga", "servicios_recarga", "POST", parametros);

			if (respuesta.Success) {
				if ((int)respuesta.State["Error"] == 0) {
					if (isLast) {
						listSRFijo = setListServiciosRecarga(respuesta.Response);

						Intent i = new Intent(ma, typeof(RecargasActivity));

						i.PutExtra("isLogin", isLogin);
						i.PutExtra("badgeCount", badgeCount);
						i.PutExtra("listSRMovil", JsonConvert.SerializeObject(listSRMovil));
						i.PutExtra("listSRFijo", JsonConvert.SerializeObject(listSRFijo));
						i.PutExtra("isUR", false);

						StartActivityForResult(i, 2);
						AndHUD.Shared.Dismiss(ma);
					} else {
						listSRMovil = setListServiciosRecarga(respuesta.Response);

						JObject param = new JObject();
						param.Add("sesion", RealmDB.GetInstance().realm.All<PersistentData>().First().idTransaccion);
						param.Add("idServicio", "19");

						getServiciosRecarga(param, true);
					}

				} else {
					CustomAlertDialog alert = new CustomAlertDialog(ma, "¡Oops!", respuesta.State["Mensaje"].ToString(), "Aceptar", "", null, null);
					alert.showDialog();
					AndHUD.Shared.Dismiss(ma);
				}
			} else {
				CustomAlertDialog alert = new CustomAlertDialog(ma, "¡Oops!", respuesta.Message, "Aceptar", "", null, null);
				alert.showDialog();
				AndHUD.Shared.Dismiss(ma);
			}
		}

		private List<Recargas> setListUltimasRecargas(JObject response) {
			var listaRecargas = new List<Recargas>();
			var listR = response["ObtieneUltimasRecargas"];

			for (var i = 0; i < listR.Count(); i++) {
				Recargas r = new Recargas();
				r.user_id = listR[i]["user_id"].ToString();
				r.id_pago = (int)listR[i]["id_pago"];
				r.id_servicio = (int)listR[i]["id_servicio"];
				r.descripcion_servicio = listR[i]["descripción_servicio"].ToString();
				r.codigo_identificacion = listR[i]["codigo_identificacion"].ToString();
				r.id_biller = (int)listR[i]["id_biller"];
				r.nombre_biller = listR[i]["nombre_biller"].ToString();
				r.monto_pago = (int)listR[i]["monto_pago"];
				r.alias = listR[i]["alias"].ToString();

				listaRecargas.Add(r);
			}
			listaRecargas[0].isSelected = true;

			return listaRecargas;
		}

		private List<MontoRecarga> setListMontoRecarga(JObject response) {
			var listMontoRecarga = new List<MontoRecarga>();
			var listMR = response["ObtieneMontoRecarga"];

			for (var i = 0; i < listMR.Count(); i++) {
				var mRecarga = new MontoRecarga();

				mRecarga.tipo_tramo = listMR[i]["tipo_tramo"].ToString();
				mRecarga.valor1 = (int)listMR[i]["valor1"];
				mRecarga.valor2 = (int)listMR[i]["valor2"];

				listMontoRecarga.Add(mRecarga);
			}

			return listMontoRecarga;
		}

		private List<ServiciosRecarga> setListServiciosRecarga(JObject response) {
			var listServiciosRecargas = new List<ServiciosRecarga>();
			var listSR = response["ObtieneServicioRecarga"];

			for (var i = 0; i < listSR.Count(); i++) {
				var sRecarga = new ServiciosRecarga();

				sRecarga.categoria = listSR[i]["categoria"].ToString();
				sRecarga.id_biller = (int)listSR[i]["id_biller"];
				sRecarga.id_servicio = (int)listSR[i]["id_servicio"];
				sRecarga.logo = listSR[i]["logo"].ToString();
				sRecarga.nombre = listSR[i]["nombre"].ToString();

				listServiciosRecargas.Add(sRecarga);
			}

			return listServiciosRecargas;
		}
	}
}
