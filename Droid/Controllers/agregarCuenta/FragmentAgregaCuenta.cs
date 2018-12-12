using System;
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
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace ServipagMobile.Droid {
	public class FragmentAgregaCuenta : Fragment {
		private AgregarActivity aa;
		private ImageView iconType;
		private TextView titleType;
		private TextView textBiller;
		private TextView hintTipoID;
		private RadioGroup radioGroup;
		private RadioButton radioRut;
		private RadioButton radioNumCta;
		private EditText fieldIdService;
		private TextView textHintAliasTwo;
		private EditText fieldAliasCta;
		private ImageView imagenBoleta;
		private Button buttonSave;
		private Servicios servicio;
		private bool isPagoExpress;
		private Validations val;
		private Utils utils;

		public FragmentAgregaCuenta (Servicios s, bool isPagoExpress) {
			servicio = s;
			this.isPagoExpress = isPagoExpress;
		}
		public override void OnCreate(Bundle savedInstanceState) {
			base.OnCreate(savedInstanceState);

			aa = (AgregarActivity)Activity;
			val = new Validations();
			utils = new Utils();
		}

		public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState) {
			return inflater.Inflate(Resource.Layout.FragmentAgregaCuenta, container, false);
		}

		public override void OnViewCreated(View view, Bundle savedInstanceState) {
			base.OnViewCreated(view, savedInstanceState);

			iconType = view.FindViewById<ImageView>(Resource.Id.iconType);
			titleType = view.FindViewById<TextView>(Resource.Id.titleType);
			hintTipoID = view.FindViewById<TextView>(Resource.Id.hintTipoID);
			radioGroup = view.FindViewById<RadioGroup>(Resource.Id.radioGroup);
			radioRut = view.FindViewById<RadioButton>(Resource.Id.radioRut);
			radioNumCta = view.FindViewById<RadioButton>(Resource.Id.radioNumCta);
			textBiller = view.FindViewById<TextView>(Resource.Id.textBiller);
			fieldIdService = view.FindViewById<EditText>(Resource.Id.fieldIdService);
			textHintAliasTwo = view.FindViewById<TextView>(Resource.Id.textHintAliasTwo);
			fieldAliasCta = view.FindViewById<EditText>(Resource.Id.fieldAliasCta);
			imagenBoleta = view.FindViewById<ImageView>(Resource.Id.imagenBoleta);
			buttonSave = view.FindViewById<Button>(Resource.Id.buttonSave);
			buttonSave.SetText(Resource.String.agregar_cta_next);

			if (!isPagoExpress) {
				fieldAliasCta.Visibility = ViewStates.Visible;
			}

			if (servicio.id == "866") {
				hintTipoID.Visibility = ViewStates.Visible;
				radioGroup.Visibility = ViewStates.Visible;

				textBiller.Visibility = ViewStates.Gone;
				radioRut.Checked = true;
				fieldIdService.Hint = "RUT Cliente";
				textHintAliasTwo.Text = "(RUT sin puntos ni guión)";

				radioRut.Click += (sender, e) => {
					fieldIdService.Hint = "RUT Cliente";
					textHintAliasTwo.Text = "(RUT sin puntos ni guión)";
				};

				radioNumCta.Click += (sender, e) => {
					fieldIdService.Hint = servicio.descripcion_primaria_identificador;
					textHintAliasTwo.Text = servicio.descripcion_secundaria_identificador;
				};
			} else {
				hintTipoID.Visibility = ViewStates.Gone;
				radioGroup.Visibility = ViewStates.Gone;

				textBiller.Visibility = ViewStates.Visible;
				textBiller.Text = servicio.nombre;
				fieldIdService.Hint = servicio.descripcion_primaria_identificador;
				textHintAliasTwo.Text = servicio.descripcion_secundaria_identificador;
			}
			setIconType(servicio.id_servicio, iconType);
			titleType.Text = servicio.nombre_servicio;

			Glide.With(this)
			 .Load("https://www.servipag.com/PortalWS/Content/images/" + servicio.imagen_boleta)
			 .Placeholder(Resources.GetDrawable(Resource.Drawable.cuenta_sin_imagen))
		     .DontAnimate()
			 .Into(imagenBoleta);

			var attacher = new PhotoViewAttacher(imagenBoleta);

			buttonSave.Click += (sender, e) => {
				var accountExist = RealmDB.GetInstance().realm.All<MisCuentas>().Where(a => a.idBiller == Convert.ToInt32(servicio.id)).
				                          Where(c => c.idServicio == Convert.ToInt32(servicio.id_servicio)).
				                          Where(b => b.idCuenta == fieldIdService.Text).Count();
				if (accountExist == 0 || !isPagoExpress) {
					onSaveClick();
				} else {
					CustomAlertDialog alert = new CustomAlertDialog(aa, "¡Ojo!", Resources.GetString(Resource.String.agregar_cta_account_exist), "Aceptar", "", null, null);
					alert.showDialog();
				}

			};
		}

		private void onSaveClick() {
			Dictionary<string, string> fields = new Dictionary<string, string>();
			fields.Add(Resources.GetString(Resource.String.agregar_cta_hint_num_cliente), fieldIdService.Text);

			if (!isPagoExpress) {
				fields.Add("Alias cuenta", fieldAliasCta.Text);
			}

			if ((bool)val.isEmpty(fields)["code"]) {
				CustomAlertDialog alert = new CustomAlertDialog(aa, "¡Ojo!", (string)val.isEmpty(fields)["data"], "Aceptar", "", null, null);
				alert.showDialog();
			} else {
				if (isPagoExpress) {
					if (radioRut.Checked) {
						aa.listAdd.Add(new MisCuentas("",
												  servicio.nombre,
												  Convert.ToInt32(servicio.id),
												  Convert.ToInt32(servicio.id_servicio),
												  "RUT:" + fieldIdService.Text,
												  servicio.imagen_logo,
												  "",
												  true,
												  servicio.nombre_servicio));
						aa.badgeCarro.Text = aa.listAdd.Count.ToString();
					} else {
						aa.listAdd.Add(new MisCuentas("",
												  servicio.nombre,
												  Convert.ToInt32(servicio.id),
												  Convert.ToInt32(servicio.id_servicio),
												  fieldIdService.Text,
												  servicio.imagen_logo,
												  "",
												  true,
												  servicio.nombre_servicio));
						aa.badgeCarro.Text = aa.listAdd.Count.ToString();
					}

					persistAccountPE();

					Intent intent = new Intent(aa, typeof(ComprobanteActivity));
					intent.PutExtra("isPagoExpress", true);
					intent.PutExtra("badgeCount", aa.listAdd.Count.ToString());
					intent.PutExtra("status", "agregarSuccess");
					aa.StartActivityForResult(intent, 3);
				} else {
					onSaveAccountClick();
				}
			}
		}

		private void persistAccountPE() {
			var account = aa.listAdd[aa.listAdd.Count - 1];

			RealmDB.GetInstance().realm.Write(() => {
				var accountPE = new MisCuentas {
					aliasCuenta = account.aliasCuenta,
					billerCuenta = account.billerCuenta,
					idBiller = account.idBiller,
					idServicio = account.idServicio,
					idCuenta = account.idCuenta,
					imagenBiller = account.imagenBiller,
					imagenServicio = account.imagenServicio,
					modificable = account.modificable,
					nombreServicio = account.nombreServicio,
					isSelected = true
				};
				RealmDB.GetInstance().realm.Add(accountPE);
			});
		}

		private void onSaveAccountClick() {
			JObject parametros = new JObject();
			AndHUD.Shared.Show(aa, null, -1, MaskType.Black);

			parametros.Add("sesion", RealmDB.GetInstance().realm.All<PersistentData>().First().cookie);
			parametros.Add("canal", DeviceInformation.GetInstance().channel);
			parametros.Add("idUsuario", UserData.GetInstance().rut);
			parametros.Add("operacion", "I");
			parametros.Add("idServicio", Convert.ToInt32(servicio.id_servicio));
			parametros.Add("idBiller", Convert.ToInt32(servicio.id));
			if (radioRut.Checked) {
				parametros.Add("identificador", "RUT:" + fieldIdService.Text);
			} else {
				parametros.Add("identificador", fieldIdService.Text);
			}

			parametros.Add("alias", fieldAliasCta.Text);
			parametros.Add("firma", "");

			inscribirCuentas(parametros);
		}

		public async void inscribirCuentas(JObject parametros) {
			var response = await MyClass.WorklightClient.UnprotectedInvokeAsync("inscribirCuentas", "inscribir_cuentas", "POST", parametros);

			if (response.Success) {
				if ((int)response.State["Error"] == 0) {
					Intent intent = new Intent(aa, typeof(ComprobanteActivity));
					intent.PutExtra("isPagoExpress", false);
					intent.PutExtra("status", "agregarSuccess");
					aa.StartActivityForResult(intent, 3);
				} else {
					Intent intent = new Intent(aa, typeof(ComprobanteActivity));
					intent.PutExtra("isPagoExpress", false);
					intent.PutExtra("status", "agregarFail");
					intent.PutExtra("mensaje", response.State["Mensaje"].ToString());
					aa.StartActivityForResult(intent, 3);
				}
			} else {
				Intent intent = new Intent(aa, typeof(ComprobanteActivity));
				intent.PutExtra("isPagoExpress", false);
				intent.PutExtra("status", "agregarFail");
				intent.PutExtra("mensaje", response.Message);
				aa.StartActivityForResult(intent, 3);
			}

			AndHUD.Shared.Dismiss(aa);
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
