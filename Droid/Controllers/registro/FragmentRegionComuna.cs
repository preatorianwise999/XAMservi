using System;
using System.Collections.Generic;
using System.Linq;
using Android.OS;
using Android.Support.V4.App;
using Android.Util;
using Android.Views;
using Android.Widget;
using AndroidHUD;
using Newtonsoft.Json.Linq;

namespace ServipagMobile.Droid {
	public class FragmentRegionComuna : Fragment {
		private TextView subtitleRC;
		private Button showRegion;
		private Button showComuna;
		private Button nextRC;
		private Validations val;

		public override void OnCreate(Bundle savedInstanceState) {
			base.OnCreate(savedInstanceState);

			this.val = new Validations();
		}

		public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState) {
			return inflater.Inflate(Resource.Layout.FragmentRegionComuna, container, false);
		}

		public override void OnViewCreated(View view, Bundle savedInstanceState) {
			base.OnViewCreated(view, savedInstanceState);

			showRegion = view.FindViewById<Button>(Resource.Id.showListRegion);
			showComuna = view.FindViewById<Button>(Resource.Id.showListComuna);
			nextRC = view.FindViewById<Button>(Resource.Id.nextRC);

			if (RegisterData.GetInstance().region != -1) {
				showRegion.Text = ListadoRegion.GetInstance().listaRegiones.Find(
					x => Convert.ToInt32(x.id) == RegisterData.GetInstance().region).nombre;
			}
			if (RegisterData.GetInstance().comuna != -1) {
				showComuna.Text = ListadoComuna.GetInstance().listaComunas.Find(
					x => Convert.ToInt32(x.id) == RegisterData.GetInstance().comuna).nombre;
			}

			showRegion.Click += delegate {
				RegisterData.GetInstance().listType = "region";
				var activity = (RegistroActivity)Activity;
				activity.changeMainFragment(new FragmentListaRC(), Resources.GetString(Resource.String.registro_id_listarc));
			};

			showComuna.Click += delegate {
				Dictionary<string, string> fields = new Dictionary<string, string>();
				fields.Add(Resources.GetString(Resource.String.registro_rc_hint_region), showRegion.Text);

				if ((bool)val.areSelected(fields)["code"]) {
					JObject parametros = new JObject();
					AndHUD.Shared.Show((RegistroActivity)Activity, null, -1, MaskType.Black);

					parametros.Add("sesion", RealmDB.GetInstance().realm.All<PersistentData>().First().idTransaccion);
					parametros.Add("idRegion", RegisterData.GetInstance().region);
					listadoComunas(parametros);
				} else {
					CustomAlertDialog alert = new CustomAlertDialog((RegistroActivity)Activity, "¡Ojo!", (string)val.areSelected(fields)["data"], "Aceptar", "", null, null);
					alert.showDialog();
				}
			};

			nextRC.Click += delegate {
				onNextClick();
			};
		}

		private void onNextClick() {
			Dictionary<string, string> fields = new Dictionary<string, string>();
			fields.Add(Resources.GetString(Resource.String.registro_rc_hint_region), showRegion.Text);
			fields.Add(Resources.GetString(Resource.String.registro_rc_hint_comuna), showComuna.Text);

			if ((bool)val.areSelected(fields)["code"]) {
				var activity = (RegistroActivity)Activity;
				activity.changeMainFragment(new FragmentEmail(), Resources.GetString(Resource.String.registro_id_email));
			} else {
				CustomAlertDialog alert = new CustomAlertDialog((RegistroActivity)Activity, "¡Ojo!", (string)val.areSelected(fields)["data"], "Aceptar", "", null, null);
				alert.showDialog();
			}
		}

		public async void listadoComunas(JObject parametros) {
			var response = await MyClass.WorklightClient.UnprotectedInvokeAsync("obtieneComunas", "obtiene_comunas", "POST", parametros);

			if (response.Success) {
				if ((int)response.State["Error"] == 0) {
					ListadoComuna.GetInstance().listaComunas = setListComunas(response.Response);
					RegisterData.GetInstance().listType = "comuna";
					var activity = (RegistroActivity)Activity;
					activity.changeMainFragment(new FragmentListaRC(), Resources.GetString(Resource.String.registro_id_listarc));
				} else {
					CustomAlertDialog alert = new CustomAlertDialog((RegistroActivity)Activity, "¡Ojo!", response.State["Mensaje"].ToString(), "Aceptar", "", null, null);
					alert.showDialog();
				}
			} else {
				CustomAlertDialog alert = new CustomAlertDialog((RegistroActivity)Activity, "¡Ojo!", response.Message, "Aceptar", "", null, null);
				alert.showDialog();
			}
			AndHUD.Shared.Dismiss((RegistroActivity)Activity);
		}

		private List<RegionComuna> setListComunas(JObject response) {
			List<RegionComuna> list = new List<RegionComuna>();

			var listComunas = response["ListadoComunas"];

			for (var i = 0; i < listComunas.Count(); i++) {
				list.Add(new RegionComuna(
					listComunas[i]["id_comuna"].ToString(), listComunas[i]["nombre_comuna"].ToString()));
			}

			return list;
		}
	}
}
