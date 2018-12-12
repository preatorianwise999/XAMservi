using System.Collections.Generic;
using System.Linq;
using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using Android.Support.V7.App;
using Android.Util;
using AndroidHUD;
using Plugin.DeviceInfo;
using Newtonsoft.Json.Linq;
using Realms;
using Worklight.Xamarin.Android;
using Worklight.Xamarin.Android.Push;

namespace ServipagMobile.Droid {
	[Activity(Theme = "@style/ServipagTheme.Splash", MainLauncher = true, NoHistory = true, ScreenOrientation = ScreenOrientation.Portrait)]
	public class SplashActivity : AppCompatActivity {
		private UtilsAndroid utilsAndroid = new UtilsAndroid();

		protected override void OnCreate(Bundle savedInstanceState) {
			base.OnCreate(savedInstanceState);

			MyClass.WorklightClient = new ServiceDelegate(WorklightClient.CreateInstance(this), WorklightPush.Instance);

			var config = new RealmConfiguration() { SchemaVersion = 12};
			RealmDB.GetInstance(Realm.GetInstance(config));

			getIdTransaccion();
		}

		private void getIdTransaccion() {
			JObject parametros = new JObject();

			DeviceInformation.GetInstance(utilsAndroid.getChannel(utilsAndroid.getDeviceType(this),
																 CrossDeviceInfo.Current.Platform.ToString()),
																 CrossDeviceInfo.Current.Platform.ToString(),
																 utilsAndroid.getIpAddress(),
																 CrossDeviceInfo.Current.Version);

			parametros.Add("canal", DeviceInformation.GetInstance().channel);
			parametros.Add("dispositivo", DeviceInformation.GetInstance().deviceType);
			parametros.Add("firma", "");
			parametros.Add("versionSO", DeviceInformation.GetInstance().version);
			parametros.Add("cliente", DeviceInformation.GetInstance().client);

			generaIdTransaccion(parametros);
		}

		public async void RegistraDispositivo() {
			await MyClass.WorklightClient.RegisterAsync();
		}

		public async void generaIdTransaccion(JObject parametros) {
			var respuesta = await MyClass.WorklightClient.UnprotectedInvokeAsync("generaIdTransaccion", "genera_id_transaccion", "POST", parametros);

			if (respuesta.Success) {
				if ((int)respuesta.State["Error"] == 0) {
					if (RealmDB.GetInstance().realm.All<PersistentData>().Count() == 0) {
						RealmDB.GetInstance().realm.Write(() => {
							var pData = new PersistentData {
								sortType = -1,
								sortTypePE = -1,
								acepta_tc_pdu = false,
								idTransaccion = respuesta.Response["idTransaccion"].ToString()
							};
							RealmDB.GetInstance().realm.Add(pData);
						});
					} else {
						var pData = RealmDB.GetInstance().realm.All<PersistentData>().First();
						RealmDB.GetInstance().realm.Write(() => pData.idTransaccion = respuesta.Response["idTransaccion"].ToString());
					}

					JObject param = new JObject();
					param.Add("sesion", RealmDB.GetInstance().realm.All<PersistentData>().First().idTransaccion);
					listadoRegiones(param);
				} else {
					CustomAlertDialog alert = new CustomAlertDialog(this, "¡Oops!",
					                                                Resources.GetString(Resource.String.mensaje_error_generico),
					                                                Resources.GetString(Resource.String.button_reconectar), "", getIdTransaccion, null);
					alert.showDialog();
				}
			} else {
				CustomAlertDialog alert = new CustomAlertDialog(this, "¡Oops!",
																	Resources.GetString(Resource.String.mensaje_error_generico),
																	Resources.GetString(Resource.String.button_reconectar), "", getIdTransaccion, null);
				alert.showDialog();
			}
		}

		public async void listadoRegiones(JObject parametros) {
			var response = await MyClass.WorklightClient.UnprotectedInvokeAsync("listadoRegiones", "listado_regiones", "POST", parametros);

			if (response.Success) {
				if ((int)response.State["Error"] == 0) {
					ListadoRegion.GetInstance(setListRegiones(response.Response));

					JObject param = new JObject();

					param.Add("plataforma", "android");
					getProperties(param);
				} else {
					CustomAlertDialog alert = new CustomAlertDialog(this, "¡Oops!",
																	Resources.GetString(Resource.String.mensaje_error_generico),
																	Resources.GetString(Resource.String.button_reconectar), "", getIdTransaccion, null);
					alert.showDialog();
				}
			} else {
				CustomAlertDialog alert = new CustomAlertDialog(this, "¡Oops!",
																	Resources.GetString(Resource.String.mensaje_error_generico),
																	Resources.GetString(Resource.String.button_reconectar), "", getIdTransaccion, null);
				alert.showDialog();
			}
			AndHUD.Shared.Dismiss(this);
		}

		private async void getProperties(JObject parametros) {
			var response = await MyClass.WorklightClient.UnprotectedInvokeAsync("servipagProperties", "properties", "POST", parametros);

			if (response.Success) {
				Log.Debug("Test", response.Response["tiposMediosPago"]["1"]["descripcion"].ToString());
				Properties.GetInstance(setTiposMediosPago(response.Response),
				                       response.Response["appversion"].ToString(),
				                       response.Response["develop"]["slackHookIOS_QA"].ToString(),
				                       response.Response["develop"]["showErrors"].ToString(),
				                       response.Response["develop"]["qaDroid"].ToString(),
				                       response.Response["develop"]["qaIOS"].ToString(),
				                       response.Response["develop"]["slackHookDroid_QA"].ToString(),
				                       response.Response["develop"]["slackHookError"].ToString(),
				                       response.Response["timeout"].ToString(),
				                       response.Response["url"].ToString(),
				                       response.Response["shownotification"].ToString());
				StartActivity(new Intent(Application.Context, typeof(MainActivity)));
			} else {
				CustomAlertDialog alert = new CustomAlertDialog(this, "¡Oops!",
																	Resources.GetString(Resource.String.mensaje_error_generico),
																	Resources.GetString(Resource.String.button_reconectar), "", getIdTransaccion, null);
				alert.showDialog();

				AndHUD.Shared.Dismiss(this);
			}
		}

		private List<TiposMediosPago> setTiposMediosPago(JObject response) {
			List<TiposMediosPago> lTMP = new List<TiposMediosPago>();


			for (int i = 1; i < response["tiposMediosPago"].Count() + 1; i++) {
				lTMP.Add(new TiposMediosPago(response["tiposMediosPago"][i.ToString()]["descripcion"].ToString(),
										 response["tiposMediosPago"][i.ToString()]["tipoVista"].ToString(),
										 response["tiposMediosPago"][i.ToString()]["id"].ToString(),
										 response["tiposMediosPago"][i.ToString()]["nombreTab"].ToString()));
			}

			return lTMP;
		}

		private List<RegionComuna> setListRegiones(JObject response) {
			List<RegionComuna> list = new List<RegionComuna>();
			var listRegiones = response["ValidaRegiones"];

			for (var i = 0; i < listRegiones.Count(); i++) {
				list.Add(new RegionComuna(
					listRegiones[i]["id_region"].ToString(), listRegiones[i]["nombre_region"].ToString()));
			}

			if (list[0].id == "15") {
				list.Reverse();
			}
			return list;
		}
	}
}
