using System.Collections.Generic;
using System.IO;
using System.Linq;
using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using Android.Support.V7.App;
using Android.Views;
using AndroidHUD;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace ServipagMobile.Droid {
	[Activity(Label = "AgregarActivity", ScreenOrientation = ScreenOrientation.Portrait)]
	public class AgregarActivity : AppCompatActivity {
		public List<MisCuentas> listAdd = new List<MisCuentas>();
		public List<Servicios> listaServicios = new List<Servicios>();
		public List<Servicios> listaBillers = new List<Servicios>();
		public List<Servicios> listaServiciosBillers = new List<Servicios>();
		public Android.Widget.TextView badgeCarro;
		public string idFragment;
		public string filteredText;
		public bool isFiltered = false;
		private Android.Support.V7.Widget.Toolbar toolbar;
		private Android.Widget.RelativeLayout carroCompraLayout;
		private Android.Widget.ImageButton carroCompra;
		private FragmentListaServicios fls;
		private bool isPagoExpress;
		private bool isAutopista;

		protected override void OnCreate(Bundle savedInstanceState) {
			base.OnCreate(savedInstanceState);
			SetContentView(Resource.Layout.Agregar);

			isPagoExpress = Intent.GetBooleanExtra("isPagoExpress", true);
			isAutopista = Intent.GetBooleanExtra("autopista", true);
			listAdd = JsonConvert.DeserializeObject<List<MisCuentas>>(Intent.GetStringExtra("listAdd"));

			JObject parametros = new JObject();
			AndHUD.Shared.Show(this, null, -1, MaskType.Black);
			parametros.Add("sesion", RealmDB.GetInstance().realm.All<PersistentData>().First().idTransaccion);

			getServicios(parametros);

			setValuesAgregar();
		}

		public override void OnBackPressed() {
			//base.OnBackPressed();
		}

		public void setValuesAgregar() {
			toolbar = FindViewById<Android.Support.V7.Widget.Toolbar>(Resource.Id.toolbarBack);
			carroCompraLayout = FindViewById<Android.Widget.RelativeLayout>(Resource.Id.carroCompraLayout);
			carroCompra = FindViewById<Android.Widget.ImageButton>(Resource.Id.carroCompra);
			badgeCarro = FindViewById<Android.Widget.TextView>(Resource.Id.badgeText);

			badgeCarro.Text = listAdd.Count.ToString();

			SetSupportActionBar(toolbar);
			SupportActionBar.SetDisplayHomeAsUpEnabled(true);
			SupportActionBar.SetDisplayShowHomeEnabled(true);
			SupportActionBar.SetDisplayShowTitleEnabled(false);
		}

		public override bool OnOptionsItemSelected(IMenuItem item) {
			if (item.ItemId == Android.Resource.Id.Home) {
				switch (idFragment) {
					case "sYb":
						Intent intent = new Intent();
						if (isPagoExpress) {
							intent.PutExtra("actionAgregar", "noAction");
							intent.PutExtra("listAdd", JsonConvert.SerializeObject(listAdd));
						} else {
							intent.PutExtra("actionAgregar", "reloadList");
						}

						SetResult(Result.Ok, intent);
						Finish();
					break;
					case "biller":
						fls.adapter.filterList(listaServicios);
						fls.search.SetQuery("", false);
						idFragment = Resources.GetString(Resource.String.agregar_cta_serv_billers);
					break;
					case "add":
						if (isPagoExpress) {
							fls = new FragmentListaServicios(true, false);
							changeMainFragment(fls, Resources.GetString(Resource.String.agregar_cta_serv_billers));
						} else {
							fls = new FragmentListaServicios(false, false);
							changeMainFragment(fls, Resources.GetString(Resource.String.agregar_cta_serv_billers));
						}
					break;
					case "tcPDU":
						fls = new FragmentListaServicios(true, isAutopista);
						changeMainFragment(fls, Resources.GetString(Resource.String.agregar_cta_serv_billers));
					break;
				}
			}
			return base.OnOptionsItemSelected(item);
		}

		protected override void OnActivityResult(int requestCode, Result resultCode, Android.Content.Intent data) {
			base.OnActivityResult(requestCode, resultCode, data);

			if (requestCode == 3) {
				switch (data.GetStringExtra("actionComprobanteAgregar")) {
					case "cerrarAgregar":
						Intent intent = new Intent();
						if (isPagoExpress) {
							intent.PutExtra("actionAgregar", "mCarroCompra");
							intent.PutExtra("listAdd", JsonConvert.SerializeObject(listAdd));
						} else {
							intent.PutExtra("actionAgregar", "reloadList");
						}

						SetResult(Result.Ok, intent);
						Finish();
					break;
					case "mCarroCompra":
						Intent intent2 = new Intent();
						intent2.PutExtra("actionAgregar", "mCarroCompra");
						intent2.PutExtra("listAdd", JsonConvert.SerializeObject(listAdd));

						SetResult(Result.Ok, intent2);
						Finish();
					break;
					case "agregaNuevo":
						if (isPagoExpress) {
							fls = new FragmentListaServicios(true, false);
							changeMainFragment(fls, Resources.GetString(Resource.String.agregar_cta_serv_billers));
						} else {
							fls = new FragmentListaServicios(false, false);
							changeMainFragment(fls, Resources.GetString(Resource.String.agregar_cta_serv_billers));
						}
					break;
				}
			}
		}

		public void changeMainFragment(Android.Support.V4.App.Fragment fragment, string idFragment) {
			this.idFragment = idFragment;
			var ft = SupportFragmentManager.BeginTransaction();
			ft.Replace(Resource.Id.mainFragmentAddCta, fragment);
			ft.Commit();
		}

		private async void getServiciosBiller() {
			JObject parametros = new JObject();
			var response = await MyClass.WorklightClient.UnprotectedInvokeAsync("busquedaServicioBiller", "busquedaServicioBiller", "POST", parametros);

			if (response.Success) {
				setListaServiciosBillers(response.Response);

				if (isPagoExpress) {
					carroCompraLayout.Visibility = ViewStates.Visible;
					carroCompra.Touch += (sender, e) => {
						Intent intent = new Intent();
						intent.PutExtra("actionAgregar", "mCarroCompra");
						intent.PutExtra("listAdd", JsonConvert.SerializeObject(listAdd));

						SetResult(Result.Ok, intent);
						Finish();
					};

					fls = new FragmentListaServicios(true, isAutopista);
					changeMainFragment(fls, Resources.GetString(Resource.String.agregar_cta_serv_billers));
				} else {
					fls = new FragmentListaServicios(false, isAutopista);
					changeMainFragment(fls, Resources.GetString(Resource.String.agregar_cta_serv_billers));
				}
			} else {
				Intent intent = new Intent();
				intent.PutExtra("actionAgregar", "noServices");
				SetResult(Result.Ok, intent);
				Finish();
			}
			AndHUD.Shared.Dismiss(this);
		}

		public async void getServicios(JObject parametros) {
			var response = await MyClass.WorklightClient.UnprotectedInvokeAsync("servicios", "servicios", "POST", parametros);

			if (response.Success) {
				if ((int)response.State["Error"] == 0) {
					setListaServicios(response.Response);
					getServiciosBiller();
				} else {
					Intent intent = new Intent();
					intent.PutExtra("actionAgregar", "noServices");
					SetResult(Result.Ok, intent);
					Finish();
					AndHUD.Shared.Dismiss(this);
				}
			} else {
				Intent intent = new Intent();
				intent.PutExtra("actionAgregar", "noServices");
				SetResult(Result.Ok, intent);
				Finish();
				AndHUD.Shared.Dismiss(this);
			}
		}

		private void setListaServicios(JToken response) {
			var listServices = response["ListaServicios"];
			for (var i = 0; i < listServices.Count(); i++) {
				listaServicios.Add(new Servicios("servicio",
									   listServices[i]["id"].ToString(),
									   listServices[i]["nombre"].ToString(),
									   listServices[i]["imagen_logo"].ToString(),
									   "", "", "", "", "", "", ""));
			}
		}

		private void setListaServiciosBillers(JToken response) {
			var listServicesBiller = response["data"];
			for (var i = 0; i < listServicesBiller.Count(); i++) {
				if (listServicesBiller[i]["entidad"].ToString() == "servicio") {
					var serv = new Servicios();
					serv.entidad = listServicesBiller[i]["entidad"].ToString();
					serv.id = listServicesBiller[i]["id"].ToString();
					serv.nombre = listServicesBiller[i]["nombre"].ToString();
					serv.imagen_logo = listServicesBiller[i]["imagen_logo"].ToString();
					listaServiciosBillers.Add(serv);
				} else {
					var biller = new Servicios();
					biller.entidad = listServicesBiller[i]["entidad"].ToString();
					biller.id = listServicesBiller[i]["id"].ToString();
					biller.nombre = listServicesBiller[i]["nombre"].ToString();
					biller.imagen_logo = listServicesBiller[i]["imagen_logo"].ToString();
					biller.descripcion_primaria_identificador = listServicesBiller[i]["descripcion_primaria_identificador"].ToString();
					biller.descripcion_secundaria_identificador = listServicesBiller[i]["descripcion_secundaria_identificador"].ToString();
					biller.dias_vencimiento = listServicesBiller[i]["dias_vencimiento"].ToString();
					biller.ejemplo_identificador = listServicesBiller[i]["ejemplo_identificador"].ToString();
					biller.id_servicio = listServicesBiller[i]["id_servicio"].ToString();
					biller.imagen_boleta = listServicesBiller[i]["imagen_boleta"].ToString();
					biller.nombre_servicio = listServicesBiller[i]["nombre_servicio"].ToString();
					listaServiciosBillers.Add(biller);
				}
			}
		}
	}
}
