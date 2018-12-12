using System.Collections.Generic;
using System.Linq;
using Android.Content;
using Android.OS;
using Android.Support.V4.App;
using Android.Util;
using Android.Views;
using Android.Widget;
using AndroidHUD;
using Newtonsoft.Json.Linq;

namespace ServipagMobile.Droid {
	public class FragmentCaptcha : Fragment {
		private TextView messageCaptcha;
		private List<ImageView> images;
		private List<ImageView> isSelected;
		private List<RandomCaptcha> randomList;
		private Button bttnNext;
		private ObjectsCaptcha oc = new ObjectsCaptcha();
		private Validations val;
		private OlvidaContrasenaActivity oca;
		private Utils utils = new Utils();
		private string parent;
		private string rut;
		private string password;

		public FragmentCaptcha (string data, string parent) {
			this.parent = parent;
			if (this.parent == "registro") {
				this.password = data;
			} else {
				this.rut = data;
			}
		}

		public override void OnCreate(Bundle savedInstanceState) {
			base.OnCreate(savedInstanceState);

			if (this.parent == "recuperar") {
				oca = (OlvidaContrasenaActivity)Activity;
			}

			val = new Validations();
		}

		public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState) {
			return inflater.Inflate(Resource.Layout.FragmentCaptcha, container, false);
		}

		public override void OnViewCreated(View view, Bundle savedInstanceState) {
			base.OnViewCreated(view, savedInstanceState);

			messageCaptcha = view.FindViewById<TextView>(Resource.Id.messageCaptcha);
			bttnNext = view.FindViewById<Button>(Resource.Id.buttonNextCaptcha);

			images = new List<ImageView>();
			isSelected = new List<ImageView>();
			randomList = new List<RandomCaptcha>();

			images.Add(view.FindViewById<ImageView>(Resource.Id.imageOne));
			images.Add(view.FindViewById<ImageView>(Resource.Id.imageTwo));
			images.Add(view.FindViewById<ImageView>(Resource.Id.imageThree));
			images.Add(view.FindViewById<ImageView>(Resource.Id.imageFour));
			images.Add(view.FindViewById<ImageView>(Resource.Id.imageFive));
			images.Add(view.FindViewById<ImageView>(Resource.Id.imageSix));
			images.Add(view.FindViewById<ImageView>(Resource.Id.imageSeven));
			images.Add(view.FindViewById<ImageView>(Resource.Id.imageEight));
			images.Add(view.FindViewById<ImageView>(Resource.Id.imageNine));

			isSelected.Add(view.FindViewById<ImageView>(Resource.Id.isCaptchaSelectedOne));
			isSelected.Add(view.FindViewById<ImageView>(Resource.Id.isCaptchaSelectedTwo));
			isSelected.Add(view.FindViewById<ImageView>(Resource.Id.isCaptchaSelectedThree));
			isSelected.Add(view.FindViewById<ImageView>(Resource.Id.isCaptchaSelectedFour));
			isSelected.Add(view.FindViewById<ImageView>(Resource.Id.isCaptchaSelectedFive));
			isSelected.Add(view.FindViewById<ImageView>(Resource.Id.isCaptchaSelectedSix));
			isSelected.Add(view.FindViewById<ImageView>(Resource.Id.isCaptchaSelectedSeven));
			isSelected.Add(view.FindViewById<ImageView>(Resource.Id.isCaptchaSelectedEight));
			isSelected.Add(view.FindViewById<ImageView>(Resource.Id.isCaptchaSelectedNine));

			getDataCaptcha();
			setSelected();

			bttnNext.Click += (sender, e) => {
				onNextClick();
			};


		}

		private void getDataCaptcha() {
			randomList = oc.randomImages();
			messageCaptcha.Text = Resources.GetString(randomList.Find(x => x.selected == true).message);

			images[0].SetImageResource(randomList[0].image);
			images[1].SetImageResource(randomList[1].image);
			images[2].SetImageResource(randomList[2].image);
			images[3].SetImageResource(randomList[3].image);
			images[4].SetImageResource(randomList[4].image);
			images[5].SetImageResource(randomList[5].image);
			images[6].SetImageResource(randomList[6].image);
			images[7].SetImageResource(randomList[7].image);
			images[8].SetImageResource(randomList[8].image);
		}

		private void setSelected() {
			images[0].Click += (sender, e) => {
				if (randomList[0].isSelected) {
					isSelected[0].Visibility = ViewStates.Gone;
					randomList[0].isSelected = false;
				} else {
					isSelected[0].Visibility = ViewStates.Visible;
					randomList[0].isSelected = true;
				}
			};
			images[1].Click += (sender, e) => {
				if (randomList[1].isSelected) {
					isSelected[1].Visibility = ViewStates.Gone;
					randomList[1].isSelected = false;
				} else {
					isSelected[1].Visibility = ViewStates.Visible;
					randomList[1].isSelected = true;
				}
			};
			images[2].Click += (sender, e) => {
				if (randomList[2].isSelected) {
					isSelected[2].Visibility = ViewStates.Gone;
					randomList[2].isSelected = false;
				} else {
					isSelected[2].Visibility = ViewStates.Visible;
					randomList[2].isSelected = true;
				}
			};
			images[3].Click += (sender, e) => {
				if (randomList[3].isSelected) {
					isSelected[3].Visibility = ViewStates.Gone;
					randomList[3].isSelected = false;
				} else {
					isSelected[3].Visibility = ViewStates.Visible;
					randomList[3].isSelected = true;
				}
			};
			images[4].Click += (sender, e) => {
				if (randomList[4].isSelected) {
					isSelected[4].Visibility = ViewStates.Gone;
					randomList[4].isSelected = false;
				} else {
					isSelected[4].Visibility = ViewStates.Visible;
					randomList[4].isSelected = true;
				}
			};
			images[5].Click += (sender, e) => {
				if (randomList[5].isSelected) {
					isSelected[5].Visibility = ViewStates.Gone;
					randomList[5].isSelected = false;
				} else {
					isSelected[5].Visibility = ViewStates.Visible;
					randomList[5].isSelected = true;
				}
			};
			images[6].Click += (sender, e) => {
				if (randomList[6].isSelected) {
					isSelected[6].Visibility = ViewStates.Gone;
					randomList[6].isSelected = false;
				} else {
					isSelected[6].Visibility = ViewStates.Visible;
					randomList[6].isSelected = true;
				}
			};
			images[7].Click += (sender, e) => {
				if (randomList[7].isSelected) {
					isSelected[7].Visibility = ViewStates.Gone;
					randomList[7].isSelected = false;
				} else {
					isSelected[7].Visibility = ViewStates.Visible;
					randomList[7].isSelected = true;
				}
			};
			images[8].Click += (sender, e) => {
				if (randomList[8].isSelected) {
					isSelected[8].Visibility = ViewStates.Gone;
					randomList[8].isSelected = false;
				} else {
					isSelected[8].Visibility = ViewStates.Visible;
					randomList[8].isSelected = true;
				}
			};
		}

		private void resetCaptcha() {
			isSelected[0].Visibility = ViewStates.Gone;
			isSelected[1].Visibility = ViewStates.Gone;
			isSelected[2].Visibility = ViewStates.Gone;
			isSelected[3].Visibility = ViewStates.Gone;
			isSelected[4].Visibility = ViewStates.Gone;
			isSelected[5].Visibility = ViewStates.Gone;
			isSelected[6].Visibility = ViewStates.Gone;
			isSelected[7].Visibility = ViewStates.Gone;
			isSelected[8].Visibility = ViewStates.Gone;

			randomList[0].isSelected = false;
			randomList[0].selected = false;
			randomList[1].isSelected = false;
			randomList[1].selected = false;
			randomList[2].isSelected = false;
			randomList[2].selected = false;
			randomList[3].isSelected = false;
			randomList[3].selected = false;
			randomList[4].isSelected = false;
			randomList[4].selected = false;
			randomList[5].isSelected = false;
			randomList[5].selected = false;
			randomList[6].isSelected = false;
			randomList[6].selected = false;
			randomList[7].isSelected = false;
			randomList[7].selected = false;
			randomList[8].isSelected = false;
			randomList[8].selected = false;

			getDataCaptcha();
		}

		private void onNextClick() {
			if ((bool)val.areRandomSelected(randomList)["code"]) {
				JObject parametros = new JObject();

				if (this.parent == "registro") {
					AndHUD.Shared.Show((RegistroActivity)Activity, null, -1, MaskType.Black);

					parametros.Add("canal", DeviceInformation.GetInstance().channel);
					parametros.Add("sesion", RealmDB.GetInstance().realm.All<PersistentData>().First().idTransaccion);
					parametros.Add("idUsuario", utils.getUserId(RegisterData.GetInstance().rut).Remove(utils.getUserId(RegisterData.GetInstance().rut).Length - 1));
					parametros.Add("dv", utils.getUserId(RegisterData.GetInstance().rut)[utils.getUserId(RegisterData.GetInstance().rut).Length - 1] + "");
					parametros.Add("clave", password);
					parametros.Add("nombres", RegisterData.GetInstance().name);
					parametros.Add("apellido_p", RegisterData.GetInstance().lastNP);
					parametros.Add("apellido_m", RegisterData.GetInstance().lastNM);
					parametros.Add("email", RegisterData.GetInstance().email);
					parametros.Add("fono", "");
					parametros.Add("fecha_nacimiento", RegisterData.GetInstance().birthDate);
					parametros.Add("direccion", "");
					parametros.Add("idregion", RegisterData.GetInstance().region);
					parametros.Add("idcomuna", RegisterData.GetInstance().comuna);
					parametros.Add("cuentasdisponibles", RegisterData.GetInstance().radioEmailSelected[0]);
					parametros.Add("recordatoriocuentas", 0);
					parametros.Add("vencimientocuentas", 0);
					parametros.Add("correonewsletter", RegisterData.GetInstance().radioEmailSelected[1]);
					parametros.Add("cartolahist", RegisterData.GetInstance().radioEmailSelected[2]);
					parametros.Add("sexo", RegisterData.GetInstance().radioGenderSelected);
					parametros.Add("firma", "");

					registroCliente(parametros);
				} else {
					AndHUD.Shared.Show(oca, null, -1, MaskType.Black);

					parametros.Add("sesion", RealmDB.GetInstance().realm.All<PersistentData>().First().idTransaccion);
					parametros.Add("canal", DeviceInformation.GetInstance().channel);
					parametros.Add("idUsuario", utils.getUserId(rut));
					parametros.Add("firma", "");

					obtenerClave(parametros);
				}
			} else {
				CustomAlertDialog alert;
				if (this.parent == "registro") {
					alert = new CustomAlertDialog(
					(RegistroActivity)Activity, "¡Oops!", (string)val.areRandomSelected(randomList)["data"], "Aceptar", "", null, null);
				} else {
					alert = new CustomAlertDialog(
					oca, "¡Oops!", (string)val.areRandomSelected(randomList)["data"], "Aceptar", "", null, null);
				}

				alert.showDialog();

				resetCaptcha();
			}
		}

		public async void registroCliente(JObject parametros) {
			var response = await MyClass.WorklightClient.UnprotectedInvokeAsync("registroCliente", "registro_clientes", "POST", parametros);

			if (response.Success) {
				if ((int)response.State["Error"] == 0) {
					RegisterData.GetInstance().name = "";
					RegisterData.GetInstance().lastNP = "";
					RegisterData.GetInstance().lastNM = "";
					RegisterData.GetInstance().rut = "";
					RegisterData.GetInstance().region = -1;
					RegisterData.GetInstance().comuna = -1;
					RegisterData.GetInstance().email = "";
					RegisterData.GetInstance().radioEmailSelected = null;
					RegisterData.GetInstance().birthDate = "";
					RegisterData.GetInstance().showBD = "";
					RegisterData.GetInstance().radioGenderSelected = "";
					RegisterData.GetInstance().activity = null;
					RegisterData.GetInstance().listType = "";
					RegisterData.GetInstance().deviceType = "";
					RegisterData.GetInstance().idFragment = "";
					Intent intent = new Intent();
					intent.PutExtra("registro", "complete");
					intent.PutExtra("user", RegisterData.GetInstance().rut);
					intent.PutExtra("pass", password);

					var activity = (RegistroActivity)Activity;
					activity.SetResult(Android.App.Result.Ok, intent);
					activity.Finish();

				} else {
					CustomAlertDialog alert = new CustomAlertDialog((RegistroActivity)Activity, "¡Oops!", response.State["Mensaje"].ToString(), "Aceptar", "", null, null);
					alert.showDialog();
				}
			} else {
				CustomAlertDialog alert = new CustomAlertDialog((RegistroActivity)Activity, "¡Oops!", response.Message, "Aceptar", "", null, null);
				alert.showDialog();
			}

			AndHUD.Shared.Dismiss((RegistroActivity)Activity);
		}

		public async void obtenerClave(JObject parametros) {
			var response = await MyClass.WorklightClient.UnprotectedInvokeAsync("obtenerClave", "obtener_clave", "POST", parametros);

			if (response.Success) {
				if ((int)response.State["Error"] == 0) {
					Intent intent = new Intent(oca, typeof(ComprobanteActivity));
					intent.PutExtra("status", "ocSuccess");
					intent.PutExtra("mensaje", response.Response["DatosUsuario"]["Email"].ToString());
					oca.StartActivityForResult(intent, 4);
				} else {
					Intent intent = new Intent(oca, typeof(ComprobanteActivity));
					intent.PutExtra("status", "ocFail");
					intent.PutExtra("mensaje", response.State["Mensaje"].ToString());

					oca.StartActivityForResult(intent, 4);
				}
			} else {
				Intent intent = new Intent(oca, typeof(ComprobanteActivity));
				intent.PutExtra("status", "ocFail");
				intent.PutExtra("mensaje", response.Message);

				oca.StartActivityForResult(intent, 4);
			}

			AndHUD.Shared.Dismiss(oca);
		}
	}
}
