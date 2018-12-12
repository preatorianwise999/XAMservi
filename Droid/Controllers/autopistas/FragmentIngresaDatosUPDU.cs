using System;
using System.Linq;
using Android.Content;
using Android.OS;
using Android.Support.Design.Widget;
using Android.Support.V4.App;
using Android.Support.V4.View;
using Android.Views;
using Android.Widget;
using AndroidHUD;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace ServipagMobile.Droid {
	public class FragmentIngresaDatosUPDU : Fragment {
		private PDUActivity PDUAct;
		private FragmentManager sfm;
		private PaseDiario uPDU;
		private bool isLogin;
		private string typeSelect = "nacional";

		private ImageView imgNacionalUPDU, imgMotosUPDU, imgExtranjerasUPDU, infoFechaUPDU;
		private TextView ttlTipoUPDU, patente, hintFechaUPDU;
		private ViewPager selectCategoryUPDU;
		private TabLayout paggingCategoryUPDU;
		private LinearLayout bttnFechaUPDU;
		private Button bttnContinuar;

		private FragmentCategory autos;
		private FragmentCategory motos;
		private FragmentCategory camiones;
		private FragmentCategory remolques;

		public FragmentIngresaDatosUPDU() { }

		public FragmentIngresaDatosUPDU(PDUActivity PDUAct, FragmentManager sfm, PaseDiario uPDU, bool isLogin, string idBiller, string idServicio) {
			this.PDUAct = PDUAct;
			this.sfm = sfm;
			this.uPDU = uPDU;
			this.isLogin = isLogin;
		}

		public override void OnCreate(Bundle savedInstanceState) {
			base.OnCreate(savedInstanceState);
		}

		public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState) {
			return inflater.Inflate(Resource.Layout.FragmentIngresaDatosUPDU, container, false);
		}

		public override void OnViewCreated(View view, Bundle savedInstanceState) {
			base.OnViewCreated(view, savedInstanceState);

			imgNacionalUPDU = view.FindViewById<ImageView>(Resource.Id.imgNacionalUPDU);
			imgMotosUPDU = view.FindViewById<ImageView>(Resource.Id.imgMotosUPDU);
			imgExtranjerasUPDU = view.FindViewById<ImageView>(Resource.Id.imgExtranjerasUPDU);
			infoFechaUPDU = view.FindViewById<ImageView>(Resource.Id.infoFechaUPDU);

			ttlTipoUPDU = view.FindViewById<TextView>(Resource.Id.ttlTipoUPDU);
			patente = view.FindViewById<TextView>(Resource.Id.patente);
			hintFechaUPDU = view.FindViewById<TextView>(Resource.Id.hintFechaUPDU);

			selectCategoryUPDU = view.FindViewById<ViewPager>(Resource.Id.selectCategoryUPDU);
			paggingCategoryUPDU = view.FindViewById<TabLayout>(Resource.Id.paggingCategoryUPDU);
			bttnFechaUPDU = view.FindViewById<LinearLayout>(Resource.Id.bttnFechaUPDU);
			bttnContinuar = view.FindViewById<Button>(Resource.Id.bttnContinuar);

			imgNacionalUPDU.Click += (sender, e) => {
				if (!typeSelect.Equals("nacional")) {
					typeSelect = "nacional";
					imgNacionalUPDU.SetImageDrawable(Resources.GetDrawable(Resource.Drawable.pdu_naciona_on));
					imgMotosUPDU.SetImageDrawable(Resources.GetDrawable(Resource.Drawable.pdu_motos_off));
					imgExtranjerasUPDU.SetImageDrawable(Resources.GetDrawable(Resource.Drawable.pdu_extranjera_off));

					ttlTipoUPDU.Text = Resources.GetString(Resource.String.pdu_title_nacional);

					setupViewPager(selectCategoryUPDU);
					paggingCategoryUPDU.SetupWithViewPager(selectCategoryUPDU);
				}
			};
			imgMotosUPDU.Click += (sender, e) => {
				if (!typeSelect.Equals("motos")) {
					typeSelect = "motos";
					imgNacionalUPDU.SetImageDrawable(Resources.GetDrawable(Resource.Drawable.pdu_nacional_off));
					imgMotosUPDU.SetImageDrawable(Resources.GetDrawable(Resource.Drawable.pdu_motos_on));
					imgExtranjerasUPDU.SetImageDrawable(Resources.GetDrawable(Resource.Drawable.pdu_extranjera_off));

					ttlTipoUPDU.Text = Resources.GetString(Resource.String.pdu_title_motos);

					setupViewPager(selectCategoryUPDU);
					paggingCategoryUPDU.SetupWithViewPager(selectCategoryUPDU);
				}
			};
			imgExtranjerasUPDU.Click += (sender, e) => {
				if (!typeSelect.Equals("extranjeras")) {
					typeSelect = "extranjeras";
					imgNacionalUPDU.SetImageDrawable(Resources.GetDrawable(Resource.Drawable.pdu_nacional_off));
					imgMotosUPDU.SetImageDrawable(Resources.GetDrawable(Resource.Drawable.pdu_motos_off));
					imgExtranjerasUPDU.SetImageDrawable(Resources.GetDrawable(Resource.Drawable.pdu_extranjera_on));

					ttlTipoUPDU.Text = Resources.GetString(Resource.String.pdu_title_extranjero);

					setupViewPager(selectCategoryUPDU);
					paggingCategoryUPDU.SetupWithViewPager(selectCategoryUPDU);
				}
			};
			bttnFechaUPDU.Click += (sender, e) => {
				FragmentDatePicker frag = FragmentDatePicker.NewInstance(delegate (DateTime time) {
					string month, day;

					if (time.Month < 10) {
						month = "0" + time.Month;
					} else {
						month = time.Month.ToString();
					}

					if (time.Day < 10) {
						day = "0" + time.Day;
					} else {
						day = time.Day.ToString();
					}
					hintFechaUPDU.Text = day + "/" + month + "/" + time.Year;
				});

				frag.parent = "PDU";
				frag.Show(FragmentManager, FragmentDatePicker.TAG);
			};
			infoFechaUPDU.Click += (sender, e) => {
				PDUAct.changeMainFragment(new FragmentTCAutopista(PDUAct),
				                          Resources.GetString(Resource.String.autopista_id_info_date_pdu));
			};

			bttnContinuar.Click += (sender, e) => {
				if (hintFechaUPDU.Text.Equals(Resources.GetString(Resource.String.pdu_hint_fld_fecha))) {
					CustomAlertDialog alert = new CustomAlertDialog(PDUAct, "¡Ojo!", Resources.GetString(Resource.String.pdu_miss_f_circulacion), "Aceptar", "", null, null);
					alert.showDialog();
				} else {
					JObject parametros = new JObject();
					AndHUD.Shared.Show(PDUAct, null, -1, MaskType.Black);

					parametros.Add("canal", DeviceInformation.GetInstance().channel);
					if (isLogin) {
						parametros.Add("sesion", RealmDB.GetInstance().realm.All<PersistentData>().First().cookie);
					} else {
						parametros.Add("sesion", RealmDB.GetInstance().realm.All<PersistentData>().First().idTransaccion);
					}

					parametros.Add("patente", patente.Text);
					parametros.Add("categoria", getCategoria());
					parametros.Add("firma", "");

					validaPatente(parametros);
				}
			};

			setValuesUPDU();
		}

		public async void validaPatente(JObject parametros) {
			var respuesta = await MyClass.WorklightClient.UnprotectedInvokeAsync("validaPatente", "valida_patente", "POST", parametros);

			if (respuesta.Success) {
				if ((int)respuesta.State["Error"] == 0) {
					PDUAct.pd = new PaseDiario();
					PDUAct.pd.patente = uPDU.patente;
					PDUAct.pd.fecha_circulacion = hintFechaUPDU.Text;
					PDUAct.pd.categoria = Int32.Parse(getCategoria());
					PDUAct.pd.isPDU = uPDU.isPDU;
					PDUAct.pd.idBiller = uPDU.idBiller;
					PDUAct.pd.idServicio = uPDU.idServicio;
					PDUAct.pd.tipoPDU = uPDU.tipoPDU;

					PDUAct.changeMainFragment(new FragmentPDUVendido(RealmDB.GetInstance().realm.All<PaseDiario>().Count() + 1, !isLogin, PDUAct), "pduvendido");
				} else {
					CustomAlertDialog alert = new CustomAlertDialog(PDUAct, "¡Oops!", respuesta.State["Mensaje"].ToString(), "Aceptar", "", null, null);
					alert.showDialog();
				}
			} else {
				CustomAlertDialog alert = new CustomAlertDialog(PDUAct, "¡Oops!", respuesta.Message, "Aceptar", "", null, null);
				alert.showDialog();
			}
			AndHUD.Shared.Dismiss(PDUAct);
		}

		public override void OnActivityResult(int requestCode, int resultCode, Intent data) {
			base.OnActivityResult(requestCode, resultCode, data);

			switch (requestCode) {
				case 14:
					if (data.GetStringExtra("actionPDU") == "openListDeudaPDU") {
						PDUAct.changeMainFragment(new FragmentListaDeudasPDU(PDUAct.pd, PDUAct, isLogin),
					                              Resources.GetString(Resource.String.autopista_id_list_deudas_pdu));
					}
				break;
			}
		}

		private void setValuesUPDU() {
			typeSelect = uPDU.tipoPDU;
			setTypeSelected();
			patente.Text = uPDU.patente;
		}

		private void setTypeSelected() {
			switch (typeSelect) {
				case "nacional":
					imgNacionalUPDU.SetImageDrawable(Resources.GetDrawable(Resource.Drawable.pdu_naciona_on));
					imgMotosUPDU.SetImageDrawable(Resources.GetDrawable(Resource.Drawable.pdu_motos_off));
					imgExtranjerasUPDU.SetImageDrawable(Resources.GetDrawable(Resource.Drawable.pdu_extranjera_off));

					ttlTipoUPDU.Text = Resources.GetString(Resource.String.pdu_title_nacional);
					selectCategoryUPDU.SetCurrentItem(uPDU.categoria - 1, true);
				break;
				case "motos":
					imgNacionalUPDU.SetImageDrawable(Resources.GetDrawable(Resource.Drawable.pdu_nacional_off));
					imgMotosUPDU.SetImageDrawable(Resources.GetDrawable(Resource.Drawable.pdu_motos_on));
					imgExtranjerasUPDU.SetImageDrawable(Resources.GetDrawable(Resource.Drawable.pdu_extranjera_off));

					ttlTipoUPDU.Text = Resources.GetString(Resource.String.pdu_title_motos);
					selectCategoryUPDU.SetCurrentItem(0, true);
				break;
				case "extranjera":
					imgNacionalUPDU.SetImageDrawable(Resources.GetDrawable(Resource.Drawable.pdu_nacional_off));
					imgMotosUPDU.SetImageDrawable(Resources.GetDrawable(Resource.Drawable.pdu_motos_off));
					imgExtranjerasUPDU.SetImageDrawable(Resources.GetDrawable(Resource.Drawable.pdu_extranjera_on));

					ttlTipoUPDU.Text = Resources.GetString(Resource.String.pdu_title_extranjero);
					selectCategoryUPDU.SetCurrentItem(uPDU.categoria - 1, true);
				break;
			}

			setupViewPager(selectCategoryUPDU);
			paggingCategoryUPDU.SetupWithViewPager(selectCategoryUPDU);
		}

		private void initFragment() {
			autos = new FragmentCategory(Resources.GetString(Resource.String.pdu_category_autos),
										 Resource.Drawable.pdu_auto);
			motos = new FragmentCategory(Resources.GetString(Resource.String.pdu_category_motos),
										 Resource.Drawable.pdu_moto);
			camiones = new FragmentCategory(Resources.GetString(Resource.String.pdu_category_camiones),
											Resource.Drawable.pdu_bus);
			remolques = new FragmentCategory(Resources.GetString(Resource.String.pdu_category_remolques),
											 Resource.Drawable.pdu_camion);
		}

		public void setupViewPager(ViewPager viewPager) {
			initFragment();
			CategoryPDUAdapter adapter = new CategoryPDUAdapter(ChildFragmentManager);

			switch (typeSelect) {
				case "nacional":
					adapter.clearFragments();
					adapter.addFragment(autos);
					adapter.addFragment(camiones);
					adapter.addFragment(remolques);
				break;
				case "motos":
					adapter.clearFragments();
					adapter.addFragment(motos);
				break;
				case "extranjeras":
					adapter.clearFragments();
					adapter.addFragment(autos);
					adapter.addFragment(camiones);
					adapter.addFragment(remolques);
					adapter.addFragment(motos);
				break;
			}

			viewPager.Adapter = adapter;
		}

		private string getCategoria() {
			switch (typeSelect) {
			case "nacional":
			return (selectCategoryUPDU.CurrentItem + 1).ToString();
			case "motos":
			return "4";
			case "extranjeras":
			return (selectCategoryUPDU.CurrentItem + 1).ToString();
			default:
			return "";
			}
		}
	}
}
