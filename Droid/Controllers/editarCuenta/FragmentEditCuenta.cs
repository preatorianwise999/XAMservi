using System.Collections.Generic;
using System.Linq;
using Android.Content;
using Android.OS;
using Android.Support.V4.App;
using Android.Util;
using Android.Views;
using Android.Widget;
using AndroidHUD;
using Com.Bumptech.Glide;
using UK.CO.Senab.Photoview;
using Newtonsoft.Json.Linq;

namespace ServipagMobile.Droid {
	public class FragmentEditCuenta : Fragment {
		private ImageView iconType;
		private TextView titleType;
		private TextView textBiller;
		private TextView textIdService;
		private TextView textHintAliasOne;
		private TextView textHintAliasTwo;
		private EditText aliasCta;
		private ImageView imagenBoleta;
		private Button bttnSave;

		private EditarActivity ea;
		private Validations val;
		private int indexCuenta;
		private MisCuentas servicio;


		public FragmentEditCuenta(MisCuentas s, int index) {
			this.servicio = s;
			this.indexCuenta = index;
		}

		public override void OnCreate(Bundle savedInstanceState) {
			base.OnCreate(savedInstanceState);

			this.ea = (EditarActivity)Activity;
			this.val = new Validations();
		}

		public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState) {
			return inflater.Inflate(Resource.Layout.FragmentEditCuenta, container, false);
		}

		public override void OnViewCreated(View view, Bundle savedInstanceState) {
			base.OnViewCreated(view, savedInstanceState);

			iconType = view.FindViewById<ImageView>(Resource.Id.iconType);
			titleType = view.FindViewById<TextView>(Resource.Id.titleType);
			textBiller = view.FindViewById<TextView>(Resource.Id.textBiller);
			textIdService = view.FindViewById<TextView>(Resource.Id.textIdService);
			textHintAliasOne = view.FindViewById<TextView>(Resource.Id.textHintAliasOne);
			textHintAliasTwo = view.FindViewById<TextView>(Resource.Id.textHintAliasTwo);
			aliasCta = view.FindViewById<EditText>(Resource.Id.fieldAliasCta);
			imagenBoleta = view.FindViewById<ImageView>(Resource.Id.imagenBoleta);
			bttnSave = view.FindViewById<Button>(Resource.Id.buttonSave);

			JObject parametros = new JObject();
			AndHUD.Shared.Show(ea, null, -1, MaskType.Black);
			parametros.Add("sesion", RealmDB.GetInstance().realm.All<PersistentData>().First().idTransaccion);
			parametros.Add("idServicio", servicio.idServicio);
			parametros.Add("inscribible", "true");
			getBillers(parametros);


			bttnSave.Click += delegate {
				onSaveClick();
			};
		}

		public async void getBillers(JObject parametros) {
			var response = await MyClass.WorklightClient.UnprotectedInvokeAsync("empresas", "empresas", "POST", parametros);

			if (response.Success) {
				if ((int)response.State["Error"] == 0) {
					setValuesEdit(setListaEmpresas(response.Response));
				}
			}
			AndHUD.Shared.Dismiss(ea);
		}
		public void setValuesEdit(List<Servicios> listaBillers) {
			setIconType(servicio.idServicio.ToString(), iconType);
			titleType.Text = servicio.nombreServicio;
			textBiller.Text = servicio.billerCuenta;
			textIdService.Text = servicio.idCuenta;
			aliasCta.Text = servicio.aliasCuenta;
			aliasCta.SetSelection(aliasCta.Text.Length);

			var boleta = listaBillers.Find(a => a.id_servicio == servicio.idServicio.ToString() && a.id == servicio.idBiller.ToString());

			Glide.With(this)
		     .Load("https://www.servipag.com/PortalWS/Content/images/" + boleta.imagen_boleta)
			 .Placeholder(Resources.GetDrawable(Resource.Drawable.cuenta_sin_imagen))
			 .DontAnimate()
			 .Into(imagenBoleta);

			var attacher = new PhotoViewAttacher(imagenBoleta);
		}

		public void onSaveClick() {
			Dictionary<string, string> fields = new Dictionary<string, string>();
			fields.Add("Nombre cuenta", aliasCta.Text);

			if ((bool) val.isEmpty(fields)["code"]) {
				CustomAlertDialog alert = new CustomAlertDialog(ea, "¡Ojo!", (string)val.isEmpty(fields)["data"], "Aceptar", "", null, null);
				alert.showDialog();
			} else {
				JObject parametros = new JObject();
				AndHUD.Shared.Show(ea, null, -1, MaskType.Black);

				parametros.Add("sesion", RealmDB.GetInstance().realm.All<PersistentData>().First().cookie);
				parametros.Add("canal", DeviceInformation.GetInstance().channel);
				parametros.Add("idUsuario", UserData.GetInstance().rut);
				parametros.Add("idServicio", servicio.idServicio);
				parametros.Add("idBiller", servicio.idBiller);
				parametros.Add("identificador", servicio.idCuenta);
				parametros.Add("nuevoIdentificador", servicio.idCuenta);
				parametros.Add("alias", aliasCta.Text);
				parametros.Add("flag", 0);
				parametros.Add("firma", "");

				modificarCuentas(parametros);
			}
		}

		public async void modificarCuentas(JObject parametros) {
			var response = await MyClass.WorklightClient.UnprotectedInvokeAsync("modificaCuentasInscritas", "modifica_cuentas_inscritas", "POST", parametros);

			if (response.Success) {
				if ((int)response.State["Error"] == 0) {
					Intent intent = new Intent(ea, typeof(ComprobanteActivity));
					intent.PutExtra("isPagoExpress", false);
					intent.PutExtra("status", "editarSuccess");
					ea.StartActivityForResult(intent, 21);

					/*Intent intent = new Intent();
					 intent.PutExtra("actionEdit", "changeData");
					intent.PutExtra("indexCuenta", indexCuenta);
					intent.PutExtra("aliasCuenta", aliasCta.Text);

					ea.SetResult(Android.App.Result.Ok, intent);
					ea.Finish();*/
				} else {
					Intent intent = new Intent(ea, typeof(ComprobanteActivity));
					intent.PutExtra("isPagoExpress", false);
					intent.PutExtra("status", "editarFail");
					intent.PutExtra("mensaje", Resources.GetString(Resource.String.editar_cta_error_desc));
					ea.StartActivityForResult(intent, 21);
				}
			} else {
				Intent intent = new Intent(ea, typeof(ComprobanteActivity));
				intent.PutExtra("isPagoExpress", false);
				intent.PutExtra("status", "editarFail");
				intent.PutExtra("mensaje", Resources.GetString(Resource.String.editar_cta_error_desc));
				ea.StartActivityForResult(intent, 21);
			}

			AndHUD.Shared.Dismiss(ea);
		}

		private List<Servicios> setListaEmpresas(JToken response) {
			var listServices = response["ListaEmpresas"];
			var listaBillers = new List<Servicios>();
			for (var i = 0; i < listServices.Count(); i++) {
				listaBillers.Add(new Servicios("biller",
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

			return listaBillers;
		}

		private void setIconType(string idServicio, ImageView iv) {
			switch (idServicio) {
				case "22":
					iv.SetImageDrawable(Resources.GetDrawable(Resource.Drawable.seguros));
					break;
				case "1":
					iv.SetImageDrawable(Resources.GetDrawable(Resource.Drawable.agua));
					break;
				case "101":
					iv.SetImageDrawable(Resources.GetDrawable(Resource.Drawable.casas_comerciales));
					break;
				case "47":
					iv.SetImageDrawable(Resources.GetDrawable(Resource.Drawable.arriendos));
					break;
				case "11":
					iv.SetImageDrawable(Resources.GetDrawable(Resource.Drawable.autopistas));
					break;
				case "70":
					iv.SetImageDrawable(Resources.GetDrawable(Resource.Drawable.informes_comerciales));
					break;
				case "12":
					iv.SetImageDrawable(Resources.GetDrawable(Resource.Drawable.carrier));
					break;
				case "9":
					iv.SetImageDrawable(Resources.GetDrawable(Resource.Drawable.casas_comerciales));
					break;
				case "10":
					iv.SetImageDrawable(Resources.GetDrawable(Resource.Drawable.cementerios));
					break;
				case "15":
					iv.SetImageDrawable(Resources.GetDrawable(Resource.Drawable.clubes));
					break;
				case "37":
					iv.SetImageDrawable(Resources.GetDrawable(Resource.Drawable.cobranza));
					break;
				case "28":
					iv.SetImageDrawable(Resources.GetDrawable(Resource.Drawable.internet));
					break;
				case "71":
					iv.SetImageDrawable(Resources.GetDrawable(Resource.Drawable.contribuiciones_raices));
					break;
				case "21":
					iv.SetImageDrawable(Resources.GetDrawable(Resource.Drawable.contribuiciones_raices));
					break;
				case "34":
					iv.SetImageDrawable(Resources.GetDrawable(Resource.Drawable.creditos));
					break;
				case "36":
					iv.SetImageDrawable(Resources.GetDrawable(Resource.Drawable.distribucion));
					break;
				case "32":
					iv.SetImageDrawable(Resources.GetDrawable(Resource.Drawable.donaciones));
					break;
				case "16":
					iv.SetImageDrawable(Resources.GetDrawable(Resource.Drawable.educacion));
					break;
				case "45":
					iv.SetImageDrawable(Resources.GetDrawable(Resource.Drawable.autopistas));
					break;
				case "4":
					iv.SetImageDrawable(Resources.GetDrawable(Resource.Drawable.gas));
					break;
				case "8":
					iv.SetImageDrawable(Resources.GetDrawable(Resource.Drawable.hipotecarios));
					break;
				case "27":
					iv.SetImageDrawable(Resources.GetDrawable(Resource.Drawable.informes_comerciales));
					break;
				case "7":
					iv.SetImageDrawable(Resources.GetDrawable(Resource.Drawable.internet));
					break;
				case "14":
					iv.SetImageDrawable(Resources.GetDrawable(Resource.Drawable.luz));
					break;
				case "49":
					iv.SetImageDrawable(Resources.GetDrawable(Resource.Drawable.publicidad));
					break;
				case "48":
					iv.SetImageDrawable(Resources.GetDrawable(Resource.Drawable.publicidad));
					break;
				case "54":
					iv.SetImageDrawable(Resources.GetDrawable(Resource.Drawable.promotoras));
					break;
				case "30":
					iv.SetImageDrawable(Resources.GetDrawable(Resource.Drawable.publicidad));
					break;
				case "18":
					iv.SetImageDrawable(Resources.GetDrawable(Resource.Drawable.recargas_celulares));
					break;
				case "6":
					iv.SetImageDrawable(Resources.GetDrawable(Resource.Drawable.salud));
					break;
				case "55":
					iv.SetImageDrawable(Resources.GetDrawable(Resource.Drawable.seguridad_alarmas));
					break;
				case "20":
					iv.SetImageDrawable(Resources.GetDrawable(Resource.Drawable.seguros));
					break;
				case "61":
					iv.SetImageDrawable(Resources.GetDrawable(Resource.Drawable.publicidad));
					break;
				case "5":
					iv.SetImageDrawable(Resources.GetDrawable(Resource.Drawable.tv_cable));
					break;
				case "29":
					iv.SetImageDrawable(Resources.GetDrawable(Resource.Drawable.tv_satelital));
					break;
				case "60":
					iv.SetImageDrawable(Resources.GetDrawable(Resource.Drawable.tarjeta_credito));
					break;
				case "2":
					iv.SetImageDrawable(Resources.GetDrawable(Resource.Drawable.telefonia_celular));
					break;
				case "3":
					iv.SetImageDrawable(Resources.GetDrawable(Resource.Drawable.telefonia_fija));
					break;
			}
		}
	}
}
