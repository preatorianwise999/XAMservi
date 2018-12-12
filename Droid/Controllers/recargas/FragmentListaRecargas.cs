using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Android.Graphics.Drawables;
using Android.OS;
using Android.Support.V4.App;
using Android.Support.V4.Content;
using Android.Support.V7.Widget;
using Android.Views;
using Android.Widget;
using AndroidHUD;
using Newtonsoft.Json.Linq;

namespace ServipagMobile.Droid {
	public class FragmentListaRecargas : Fragment {
		private RecargasAdapter recargasAdapter;
		private RecyclerView.LayoutManager layoutManager;
		private Drawable divider;
		private RecyclerView.ItemDecoration dividerDecoration;
		private RecyclerView listaDeudasRecarga;
		private RelativeLayout bttnPagar;
		public TextView montoTotal;

		private SolicitaRecarga solicitaRecarga;
		private RecargasActivity ra;
		private CultureInfo culture { get; set; }
		private bool isLogin;

		public FragmentListaRecargas() { }

		public FragmentListaRecargas(SolicitaRecarga solicitaRecarga, RecargasActivity ra, bool isLogin) {
			this.solicitaRecarga = solicitaRecarga;
			this.ra = ra;
			this.isLogin = isLogin;
			this.culture = new CultureInfo("es-CL");
		}

		public override void OnCreate(Bundle savedInstanceState) {
			base.OnCreate(savedInstanceState);
		}

		public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState) {
			return inflater.Inflate(Resource.Layout.FragmentListaRecargas, container, false);
		}

		public override void OnViewCreated(View view, Bundle savedInstanceState) {
			base.OnViewCreated(view, savedInstanceState);

			listaDeudasRecarga = view.FindViewById<RecyclerView>(Resource.Id.listaDeudasRecarga);
			bttnPagar = view.FindViewById<RelativeLayout>(Resource.Id.bttnPagar);
			montoTotal = view.FindViewById<TextView>(Resource.Id.montoTotal);
			montoTotal.Text = "Total: " + solicitaRecarga.monto_total.ToString("C", culture);
			recargasAdapter = new RecargasAdapter(solicitaRecarga, ra, this);
			divider = ContextCompat.GetDrawable(Context, Resource.Drawable.divider);
			dividerDecoration = new CustomItemDecoration(divider);

			listaDeudasRecarga.SetAdapter(recargasAdapter);
			listaDeudasRecarga.AddItemDecoration(dividerDecoration);

			layoutManager = new LinearLayoutManager((RecargasActivity)Activity);
			listaDeudasRecarga.SetLayoutManager(layoutManager);

			bttnPagar.Click += (sender, e) => {
				JObject param = new JObject();
				AndHUD.Shared.Show(ra, null, -1, MaskType.Black);
				if (isLogin) {
					param.Add("sesion", RealmDB.GetInstance().realm.All<PersistentData>().First().cookie);
				} else {
					param.Add("sesion", RealmDB.GetInstance().realm.All<PersistentData>().First().idTransaccion);
				}


				getMediosPago(param);
			};
		}

		public async void getMediosPago(JObject parametros) {
			var response = await MyClass.WorklightClient.UnprotectedInvokeAsync("mediosPago", "medios_pago", "POST", parametros);

			if (response.Success) {
				if ((int)response.State["Error"] == 0) {
					if (isLogin) {
						ra.changeMainFragment(new FragmentMediosPago(ra.SupportFragmentManager,
																	 setMediosPago(response.Response),
																	 solicitaRecarga,
						                                             false,
						                                             isLogin), Resources.GetString(Resource.String.recargas_medios_pago_title));
					} else {
						ra.listMediosPago = setMediosPago(response.Response);
						ra.changeMainFragment(new FragmentRutEmail(setMediosPago(response.Response), solicitaRecarga, isLogin, ra),
											  Resources.GetString(Resource.String.recargas_rut_email_title));
					}

				} else {
					CustomAlertDialog alert = new CustomAlertDialog(ra, "¡Oops!", response.State["Mensaje"].ToString(), "Aceptar", "", null, null);
					alert.showDialog();
				}
			} else {
				CustomAlertDialog alert = new CustomAlertDialog(ra, "¡Oops!", response.Message, "Aceptar", "", null, null);
				alert.showDialog();
			}
			AndHUD.Shared.Dismiss(ra);
		}

		private List<MediosPago> setMediosPago(JObject response) {
			List<MediosPago> list = new List<MediosPago>();

			var listMP = response["MediosPago"];

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
	}
}
