using System.Collections.Generic;
using Android.Content;
using Android.OS;
using Android.Support.V4.App;
using Android.Views;
using Android.Widget;
using Newtonsoft.Json.Linq;
using AndroidHUD;
using Android.Text;
using System.Linq;
using Android.Util;
using Org.Json;
using Worklight.Android;
using System;
using Worklight.Push;
using Android.App;

namespace ServipagMobile.Droid {
	public class FragmentInicioSesion : Android.Support.V4.App.Fragment, IWLLoginResponseListener {
		private MainActivity ma;
		private UtilsAndroid utilsAndroid = new UtilsAndroid();
		private EditText userField, passField;
		private Button bttnForgotPass, bttnLogin, bttnSignIn;
		private ImageView userImage;
		private TextView welcomeText;
		private ImageView securePassword;
		private LinearLayout containerLogin;
		private Validations val;
		private Utils utils;

		private static string securityCheckName = "UserLogin";
		private JSONObject credentials;
		private bool hidePassword = true;

		public override void OnCreate(Bundle savedInstanceState) {
			base.OnCreate(savedInstanceState);
			ma = (MainActivity)Activity;
			val = new Validations();
			utils = new Utils();
		}

		public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState) {
			return inflater.Inflate(Resource.Layout.FragmentInicioSesion, container, false);
		}

		public override void OnViewCreated(View view, Bundle savedInstanceState) {
			base.OnViewCreated(view, savedInstanceState);

			userField = view.FindViewById<EditText>(Resource.Id.user);
			passField = view.FindViewById<EditText>(Resource.Id.password);
			securePassword = view.FindViewById<ImageView>(Resource.Id.securePassword);

			bttnForgotPass = view.FindViewById<Button>(Resource.Id.fgtPassword);
			bttnLogin = view.FindViewById<Button>(Resource.Id.ingresar);
			bttnSignIn = view.FindViewById<Button>(Resource.Id.registrar);

			securePassword.Click += delegate {
				if (hidePassword) {
					hidePassword = false;
					securePassword.SetImageResource(Resource.Drawable.ver_blanco);
					passField.InputType = InputTypes.ClassText|InputTypes.TextVariationVisiblePassword;
				} else {
					hidePassword = true;
					securePassword.SetImageResource(Resource.Drawable.nover_blanco);
					passField.InputType = InputTypes.ClassText|InputTypes.TextVariationPassword;
				}
			};
			bttnLogin.Click += delegate {
				eventButtonLogin();
			};

			bttnSignIn.Click += delegate {
				Intent intent = new Intent(ma, typeof(RegistroActivity));
				ma.StartActivityForResult(intent, 0);
			};

			bttnForgotPass.Click += (sender, e) => {
				Intent intent = new Intent(ma, typeof(OlvidaContrasenaActivity));
				StartActivityForResult(intent, 20);
			};

			var edited = true;
			userField.TextChanged += (sender, e) => {
				if (edited) {
					edited = false;
					string editado = utils.formatearRut(userField.Text);
					userField.Text = editado;
					if (val.expresionRut(userField.Text) && (bool)val.validateRut(userField.Text)["code"]) {
						userField.SetTextColor(Resources.GetColor(Resource.Color.servipag_green));
					} else {
						userField.SetTextColor(Resources.GetColor(Resource.Color.servipag_red));
					}
					userField.SetSelection(userField.Text.Length);
					edited = true;
				}
			};

			passField.TextChanged += (sender, e) => {
				passField.SetSelection(passField.Text.Length);
			};
		}

		public override void OnActivityResult(int requestCode, int resultCode, Intent data) {
			base.OnActivityResult(requestCode, resultCode, data);
			if (requestCode == 20) {
				if (data.GetStringExtra("valueCompleted").Equals("success")) {
					userField.Text = "";
					passField.Text = "";
				}
			}
		}

		public void eventButtonLogin() {
			Dictionary<string, string> fields = new Dictionary<string, string>();
			fields.Add("RUT", userField.Text);
			fields.Add("Contraseña", passField.Text);

			if ((bool)val.isEmpty(fields)["code"]) {
				CustomAlertDialog alert = new CustomAlertDialog(ma, "¡Ojo!", (string)val.isEmpty(fields)["data"], "Aceptar", "", null, null);
				alert.showDialog();
			} else if (!val.expresionRut(userField.Text)) {
				CustomAlertDialog alert = new CustomAlertDialog(ma, "¡Ojo!", "Rut de usuario incorrecto. Vuelve a intentarlo.", "Aceptar", "", null, null);
				alert.showDialog();
			} else if (!(bool)val.validateRut(userField.Text)["code"]) {
				CustomAlertDialog alert = new CustomAlertDialog(ma, "¡Ojo!", (string)val.validateRut(userField.Text)["data"], "Aceptar", "", null, null);
				alert.showDialog();
			} else if (passField.Text.Length < 4) {
				CustomAlertDialog alert = new CustomAlertDialog(ma, "¡Ojo!", Resources.GetString(Resource.String.login_menos_caracteres), "Aceptar", "", null, null);
				alert.showDialog();
			} else {
				JObject parametros = new JObject();
				AndHUD.Shared.Show(ma, null, -1, MaskType.Black);
				
				parametros.Add("canal", DeviceInformation.GetInstance().channel);
				parametros.Add("sesion", RealmDB.GetInstance().realm.All<PersistentData>().First().idTransaccion);
				parametros.Add("firma", "");
				parametros.Add("idUsuario", utils.getUserId(userField.Text));
				parametros.Add("password", passField.Text);

				loginUser(parametros);
			}
		}

		public async void loginUser(JObject parametros) {
			var response = await MyClass.WorklightClient.UnprotectedInvokeAsync("login", "login", "POST", parametros);

			if (response.Success) {
				if ((int)response.State["Error"] == 0) {
					if ((int)response.Response["DatosUsuarioLogin"]["Retorno"] == 0) {
						registerPush();

						UserData.GetInstance().nombre = response.Response["DatosUsuarioLogin"]["Nombre"].ToString();
						UserData.GetInstance().rut = utils.getUserId(userField.Text);
						UserData.GetInstance().rutShow = userField.Text;
						UserData.GetInstance().correo = response.Response["DatosUsuarioLogin"]["Email"].ToString();

						var pData = RealmDB.GetInstance().realm.All<PersistentData>().First();
						RealmDB.GetInstance().realm.Write(() => pData.cookie = response.Response["DatosUsuarioLogin"]["Cookie"].ToString());

						parametros["sesion"] = response.Response["DatosUsuarioLogin"]["Cookie"].ToString();

						ma.isLogin = true;
						cuentasInscritas(parametros);
					} else {
						CustomAlertDialog alert = new CustomAlertDialog(ma, "¡Oops!", Resources.GetString(Resource.String.login_connection_error), "Aceptar", "", null, null);
						alert.showDialog();

						AndHUD.Shared.Dismiss(ma);
					}
				} else {
					CustomAlertDialog alert = new CustomAlertDialog(ma, "¡Oops!", Resources.GetString(Resource.String.login_connection_error), "Aceptar", "", null, null);
					alert.showDialog();

					AndHUD.Shared.Dismiss(ma);
				}
			} else {
				CustomAlertDialog alert = new CustomAlertDialog(ma, "¡Oops!", Resources.GetString(Resource.String.login_connection_error), "Aceptar", "", null, null);
				alert.showDialog();

				AndHUD.Shared.Dismiss(ma);
			}
		}

		public void registerPush() {
			credentials = new JSONObject("{username:" + utils.getUserId(userField.Text) +
												 ", password:" + passField.Text +
												 ", success: true}");

			WLAuthorizationManager.Instance.Login(securityCheckName, credentials, this);
		}

		public async void cuentasInscritas(JObject parametros) {
			var response = await MyClass.WorklightClient.UnprotectedInvokeAsync("cuentasInscritas", "cuentas_inscritas", "POST", parametros);

			if (response.Success) {
				if ((int)response.State["Error"] == 0) {
					setFragmentHome(setListCuentasInscritas(response.Response));

					if (UserData.GetInstance().nombre.Split(' ').Count() > 1) {
						changeMenu(UserData.GetInstance().nombre.Split(' ')[1]);
					} else {
						changeMenu(UserData.GetInstance().nombre.Split(' ')[0]);
					}

				} else {
					CustomAlertDialog alert = new CustomAlertDialog(ma, "¡Oops!", response.State["Mensaje"].ToString(), "Aceptar", "", null, null);
					alert.showDialog();
				}
			} else {
				CustomAlertDialog alert = new CustomAlertDialog(ma, "¡Oops!", response.Message, "Aceptar", "", null, null);
				alert.showDialog();
			}

			AndHUD.Shared.Dismiss(ma);
		}

		private List<MisCuentas> setListCuentasInscritas(JObject response) {
			ma.listaCuentasInscritas.Clear();
			var listCI = response["CuentasInscritas"];
			for (var i = 0; i < listCI.Count(); i++) {
				ma.listaCuentasInscritas.Add(new MisCuentas(listCI[i]["alias"].ToString(),
				                       listCI[i]["biller"].ToString(),
				                       (int)listCI[i]["id_biller"],
				                       (int)listCI[i]["id_servicio"],
				                       listCI[i]["identificador"].ToString(),
				                       listCI[i]["imagen_biller"].ToString(),
				                       listCI[i]["imagen_servicio"].ToString(),
			                           (bool)listCI[i]["modificable"],
				                        listCI[i]["servicio"].ToString()));
			}

			return ma.listaCuentasInscritas;
		}

		private void setFragmentHome(List<MisCuentas> list) {
			ma.isCarroCompra = false;
			ma.idFragment = "home";
			ma.childFragment = new FragmentListaCuentas(true, list);
			ma.changeMainFragment(ma.childFragment, "Home");
		}

		private void changeMenu(string name) {
			var carroCompraLayout = ma.FindViewById<RelativeLayout>(Resource.Id.carroCompraLayout);
			var sortAccounts = ma.FindViewById<ImageButton>(Resource.Id.sortMyAccounts);
			var addAccount = ma.FindViewById<ImageButton>(Resource.Id.addAccount);

			carroCompraLayout.Visibility = ViewStates.Gone;
			sortAccounts.Visibility = ViewStates.Visible;
			addAccount.Visibility = ViewStates.Visible;

			ma.changeMenu(name);
		}

		public void OnFailure(WLFailResponse wlFailResponse) {
			Log.Debug(securityCheckName, "Ha ocurrido un problema al ingresar las credenciales. " + wlFailResponse);
		}

		public void OnSuccess() {
			Log.Debug(securityCheckName, "Credenciales ingresadas correctamente");
			registraDispositivo();
		}

		public async void registraDispositivo() {
			var response = await MyClass.WorklightClient.RegisterAsync();
			Log.Debug(securityCheckName, "Dispositivo registrado para push");
			registrarPush();
		}

		public async void registrarPush() {
			var response = await MyClass.WorklightClient.SubscribeAsync();
			Log.Debug(securityCheckName, "Se ha registrado a tag predefinido");

			MyClass.WorklightClient.push.NotificationReceived += handleNotification;
		}

		public void handleNotification(object sender, EventArgs e) {
			PushEventArgs eventArgs = (PushEventArgs)e;
			Console.WriteLine("Notification received. Payload is " + eventArgs.Payload +
							  ". URL is " + eventArgs.Url + "\n\nAlert: " + eventArgs.Alert);

			SendNotification(eventArgs.Alert);
		}

		public void SendNotification(string message) {
			var intent = new Intent(ma, typeof(MainActivity));
			intent.PutExtra("pushRecive", true);
			intent.AddFlags(ActivityFlags.ClearTop);
			var pendingIntent = PendingIntent.GetActivity(ma, 0, intent, PendingIntentFlags.OneShot);

			var notificationBuilder = new Notification.Builder(ma)
													  .SetSmallIcon(Resource.Mipmap.Icon)
				.SetContentTitle("Servipag")
				.SetContentText(message)
			  	.SetAutoCancel(true);

			var notificationManager = (NotificationManager)ma.GetSystemService(Context.NotificationService);
			notificationManager.Notify(0, notificationBuilder.Build());
		}
	}
}
