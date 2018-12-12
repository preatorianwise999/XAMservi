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
using Newtonsoft.Json.Linq;
using ServipagMobile.Classes;
using Newtonsoft.Json;

namespace ServipagMobile.Droid {
	public class FragmentListaDeudas : Fragment {
		private MisDeudasAdapter adapter;
		private RecyclerView recyclerView;
		private RecyclerView.LayoutManager layoutManager;
		private Drawable divider;
		private RecyclerView.ItemDecoration dividerDecoration;
		private List<BuscaDeudas> misDeudas = new List<BuscaDeudas>();
        private List<Automata> AutomatasDatas = new List<Automata>();
		public int deudaTotal = 0;
		private CultureInfo culture { get; set; }

		private ImageView selectAccount;
		private TextView nombreBiller;
		private TextView montoTotalCuenta;
		private TextView identificador;
		private TextView numDocumento;
		private TextView fechaVencimiento;
		private TextView hintServicio;
		private TextView nombreCuenta;
		private RadioButton radioSActual;
		private RadioButton radioSAnterior;
		private TextView valueActual;
		private TextView valueAnterior;
		private RelativeLayout containerRadioGroup;
		public TextView montoTotal;
		private RelativeLayout bttnPagar;
		private RelativeLayout containerEmptyList;
		private Validations val;

		private bool isLogin;
		private PagoActivity pa;

		public FragmentListaDeudas() { }

		public FragmentListaDeudas(bool isLogin, List<BuscaDeudas> misDeudas) {
			this.isLogin = isLogin;
			this.misDeudas = misDeudas;
			this.culture = new CultureInfo("es-CL");
			this.val = new Validations();
		}

		public override void OnCreate(Bundle savedInstanceState) {
			base.OnCreate(savedInstanceState);

			pa = (PagoActivity)Activity;
		}

		public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState) {
			return inflater.Inflate(Resource.Layout.FragmentListaDeudas, container, false);
		}

		public override void OnViewCreated(View view, Bundle savedInstanceState) {
			base.OnViewCreated(view, savedInstanceState);

			selectAccount = view.FindViewById<ImageView>(Resource.Id.selectAccount);
			nombreBiller = view.FindViewById<TextView>(Resource.Id.nombreBiller);
			montoTotalCuenta = view.FindViewById<TextView>(Resource.Id.montoTotalCuenta);
			identificador = view.FindViewById<TextView>(Resource.Id.identificador);
			numDocumento = view.FindViewById<TextView>(Resource.Id.numDocumento);
			fechaVencimiento = view.FindViewById<TextView>(Resource.Id.fechaVencimiento);
			hintServicio = view.FindViewById<TextView>(Resource.Id.hintServicio);
			nombreCuenta = view.FindViewById<TextView>(Resource.Id.nombreCuenta);
			radioSActual = view.FindViewById<RadioButton>(Resource.Id.radioSActual);
			radioSAnterior = view.FindViewById<RadioButton>(Resource.Id.radioSAnterior);
			valueActual = view.FindViewById<TextView>(Resource.Id.valueActual);
			valueAnterior = view.FindViewById<TextView>(Resource.Id.valueAnterior);
			containerRadioGroup = view.FindViewById<RelativeLayout>(Resource.Id.containerRadioGroup);
			montoTotal = view.FindViewById<TextView>(Resource.Id.montoTotal);
			recyclerView = view.FindViewById<RecyclerView>(Resource.Id.listaDeudas);
			bttnPagar = view.FindViewById<RelativeLayout>(Resource.Id.bttnPagar);
			containerEmptyList = view.FindViewById<RelativeLayout>(Resource.Id.containerEmptyList);

			if (misDeudas.Count == 0) {
				containerEmptyList.Visibility = ViewStates.Visible;
				recyclerView.Visibility = ViewStates.Gone;
				bttnPagar.Visibility = ViewStates.Gone;
			} else {
				containerEmptyList.Visibility = ViewStates.Gone;
				recyclerView.Visibility = ViewStates.Visible;
				bttnPagar.Visibility = ViewStates.Visible;

				foreach (BuscaDeudas item in misDeudas) {
					if (item.id_estado_pago_solt == 3) {
						deudaTotal = deudaTotal + item.monto_total;
					}
				}

				if (deudaTotal == 0) {
					containerEmptyList.Visibility = ViewStates.Visible;
					recyclerView.Visibility = ViewStates.Gone;
					bttnPagar.Visibility = ViewStates.Gone;
				} else {
					containerEmptyList.Visibility = ViewStates.Gone;
					recyclerView.Visibility = ViewStates.Visible;
					bttnPagar.Visibility = ViewStates.Visible;

					montoTotal.Text = "Total: " + deudaTotal.ToString("C", culture);
				}
			}

			adapter = new MisDeudasAdapter(misDeudas, this, isLogin);
			divider = ContextCompat.GetDrawable(Context, Resource.Drawable.divider);
			dividerDecoration = new CustomItemDecoration(divider);

			recyclerView.SetAdapter(adapter);
			recyclerView.AddItemDecoration(dividerDecoration);

			layoutManager = new LinearLayoutManager((PagoActivity)Activity);
			recyclerView.SetLayoutManager(layoutManager);



			bttnPagar.Click += (sender, e) => {
				if (misDeudas.Count == 0) {
					CustomAlertDialog alert = new CustomAlertDialog((PagoActivity)Activity, "¡Ojo!", "Usted no tiene deudas para pagar.", "Aceptar", "", null, null);
					alert.showDialog();
				} else if (deudaTotal == 0) {
					CustomAlertDialog alert = new CustomAlertDialog((PagoActivity)Activity, "¡Ojo!", "El monto a pagar debe ser mayor a 0.", "Aceptar", "", null, null);
					alert.showDialog();
				} else if (!val.validateCFTPayment(misDeudas)) {
					CustomAlertDialog alert = new CustomAlertDialog((PagoActivity)Activity, "¡Ojo!", "Tienes que pagar primero tus cuotas más antiguas o pagarlas todas juntas.", "Aceptar", "", null, null);
					alert.showDialog();
				} else if (!val.validateIPPayment(misDeudas)) {
					CustomAlertDialog alert = new CustomAlertDialog((PagoActivity)Activity, "¡Ojo!", "Tienes que pagar primero tus cuotas más antiguas o pagarlas todas juntas.", "Aceptar", "", null, null);
					alert.showDialog();
				} else {
					JObject param = new JObject();
					AndHUD.Shared.Show(pa, null, -1, MaskType.Black);
					param.Add("sesion", RealmDB.GetInstance().realm.All<PersistentData>().First().idTransaccion);

					getMediosPago(param);
				}
			};
		}

		public async void getMediosPago(JObject parametros) {
			//var response = await MyClass.WorklightClient.UnprotectedInvokeAsync("mediosPago", "medios_pago", "POST", parametros);
            var response = await MyClass.WorklightClient.UnprotectedInvokeAsync("nuevoMP", "medios_pago", "POST", parametros);

            var responseAutomata = await MyClass.WorklightClient.UnprotectedInvokeAsync("automatasMP", "medios_pago", "POST", parametros);

			if (response.Success) {
				if ((int)response.State["Error"] == 0) {
					pa.mediosPago = setMediosPago(response.Response);
                    pa.AutomataDatos = setAutomatas(responseAutomata.Response);

                    //AutomataList.ListaAutomatas = setAutomatas(responseAutomata.Response);

					if (isLogin) {
						pa.changeMainFragment(new FragmentMediosPago(pa.SupportFragmentManager,
						                                             pa.mediosPago, misDeudas, isLogin),
						                      Resources.GetString(Resource.String.medios_pago_id_fragment));
                        pa.changeMainFragment(new FragmentVistaMP(pa.SupportFragmentManager,pa.AutomataDatos,pa.mediosPago,pa.mediosPago,misDeudas),
                                              Resources.GetString(Resource.String.medios_pago_id_fragment));
					} else {
						pa.changeMainFragment(new FragmentRutEmail(pa,pa.mediosPago, misDeudas),
											  Resources.GetString(Resource.String.rut_email_title));
					}

				} else {
					CustomAlertDialog alert = new CustomAlertDialog((PagoActivity)Activity, "¡Oops!", response.State["Mensaje"].ToString(), "Aceptar", "", null, null);
					alert.showDialog();
				}
			} else {
				CustomAlertDialog alert = new CustomAlertDialog((PagoActivity)Activity, "¡Oops!", response.Message, "Aceptar", "", null, null);
				alert.showDialog();
			}
			AndHUD.Shared.Dismiss((PagoActivity)Activity);
		}

		private List<MediosPago> setMediosPago(JObject response) {
			List<MediosPago> list = new List<MediosPago>();

			var listMP = response["MediosPago"];
            int counter = listMP.Count();
			for (var i = 0; i < listMP.Count(); i++) {
				list.Add(new MediosPago(
					listMP[i]["descripcion"].ToString(),
					Convert.ToInt32(listMP[i]["forma_pago"].ToString()),
					Convert.ToInt32(listMP[i]["id_banco"].ToString()),
					listMP[i]["logo_banco"].ToString(),
					listMP[i]["orden"].ToString(),
					listMP[i]["url_banco"].ToString(),
					listMP[i]["valor_parametro_banco"].ToString(),
					listMP[i]["valor_popup"].ToString(),
                    listMP[i]["principalColor"].ToString(),
                    listMP[i]["navigationBarTextTint"].ToString(),
                    listMP[i]["darkerPrincipalColor"].ToString(),
                    listMP[i]["secondaryColor"].ToString(),
                    listMP[i]["mainButtonStyle"].ToString(),
                    listMP[i]["hideWebAddressInformationInForm"].ToString(),
                    listMP[i]["useBarCenteredLogoInForm"].ToString(),
                    listMP[i]["font"].ToString(),
                    listMP[i]["Switch"].ToString()));
			}

			return list;
		}

        private List<Automata> setAutomatas(JObject response) {
            List<Automata> list = new List<Automata>();

            var listAuto = response["Automatas"];


            for (var i = 0; i < listAuto.Count(); i++) {
                list.Add(new Automata(
                    Convert.ToInt32(listAuto[i]["id_banco"].ToString()),
                    listAuto[i]["tipovalidacionrut"].ToString(),
                    listAuto[i]["tipo"].ToString(),
                    listAuto[i]["tx"].ToString(),
                    listAuto[i]["Nombreparametrorut"].ToString(),
                    listAuto[i]["cuenta"].ToString(),
                    listAuto[i]["usuario"].ToString(),
                    listAuto[i]["email"].ToString(),
                    listAuto[i]["actionName"].ToString()));
            }

            return list;

            /*for (var i = 0; i < listAuto.Count(); i++) {
            list.Add(new Automata(1, "bch", "<EXPRESS>", "", "1", "Yes", "NO", "Y1", "0000"));
            list.Add(new Automata(9, "mpe", "<CLIENTE>", "", "1", "Yes", "NO", "Y1", "0000"));
            list.Add(new Automata(12, "mpe", "<EXPRESS>", "", "1", "Yes", "NO", "Y1", "0000"));
            list.Add(new Automata(14, "stbk", "<EXPRESS>", "", "1", "Yes", "NO", "Y1", "0000"));
            }
            return list;
            */
        }
	}
}
