using System;
using System.Collections.Generic;
using System.Linq;
using Android.Content;
using Android.OS;
using Android.Support.Design.Widget;
using Android.Support.V4.App;
using Android.Support.V4.View;
using Android.Support.V7.Widget;
using Android.Text;
using Android.Util;
using Android.Views;
using Android.Widget;
using AndroidHUD;
using Java.Lang;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace ServipagMobile.Droid {
	public class FragmentIngresaDatosPDT : Fragment {
		private ImageView imgNacional, imgMotos, imgExtranjeras, infoFecha;
		private TextView ttlTipoPDU, hintTxtPDU, hintFechaPDU, guionDV, guionReDV;
		private EditText fieldPatente, fieldDVPatente, fieldRePatente, fieldDVRePatente;
		private ViewPager selectCategoryPDU;
		private TabLayout paggingCategoryPDU;
		private LinearLayout bttnFechaPDU;
		private Button bttnContinuar;

		private PDUActivity PDUAct;
		private bool isLogin;
		private string idBiller;
		private string idServicio;
		private string typeSelect = "nacional";

		private FragmentCategory autos;
		private FragmentCategory motos;
		private FragmentCategory camiones;
		private FragmentCategory remolques;
		private Validations val;
		private Utils utils;

		public FragmentIngresaDatosPDT() { }

		public FragmentIngresaDatosPDT(PDUActivity PDUAct, FragmentManager sfm, bool isLogin, string idBiller, string idServicio) {
			this.PDUAct = PDUAct;
			this.isLogin = isLogin;
			this.idBiller = idBiller;
			this.idServicio = idServicio;
			this.val = new Validations();
			this.utils = new Utils();
		}

		public override void OnCreate(Bundle savedInstanceState) {
			base.OnCreate(savedInstanceState);
		}

		public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState) {
			return inflater.Inflate(Resource.Layout.FragmentIngresaDatosPDU, container, false);
		}

		public override void OnViewCreated(View view, Bundle savedInstanceState) {
			base.OnViewCreated(view, savedInstanceState);

			imgNacional = view.FindViewById<ImageView>(Resource.Id.imgNacional);
			imgMotos = view.FindViewById<ImageView>(Resource.Id.imgMotos);
			imgExtranjeras = view.FindViewById<ImageView>(Resource.Id.imgExtranjeras);
			infoFecha = view.FindViewById<ImageView>(Resource.Id.infoFecha);

			ttlTipoPDU = view.FindViewById<TextView>(Resource.Id.ttlTipoPDU);
			hintTxtPDU = view.FindViewById<TextView>(Resource.Id.hintTxtPDU);
			hintFechaPDU = view.FindViewById<TextView>(Resource.Id.hintFechaPDU);
			guionDV = view.FindViewById<TextView>(Resource.Id.guionDV);
			guionReDV = view.FindViewById<TextView>(Resource.Id.guionReDV);

			fieldPatente = view.FindViewById<EditText>(Resource.Id.fieldPatente);
			fieldDVPatente = view.FindViewById<EditText>(Resource.Id.fieldDVPatente);
			fieldRePatente = view.FindViewById<EditText>(Resource.Id.fieldRePatente);
			fieldDVRePatente = view.FindViewById<EditText>(Resource.Id.fieldDVRePatente);

			selectCategoryPDU = view.FindViewById<ViewPager>(Resource.Id.selectCategoryPDU);
			paggingCategoryPDU = view.FindViewById<TabLayout>(Resource.Id.paggingCategoryPDU);
			bttnFechaPDU = view.FindViewById<LinearLayout>(Resource.Id.bttnFechaPDU);
			bttnContinuar = view.FindViewById<Button>(Resource.Id.bttnContinuar);

			if (PDUAct.pd != null) {
				typeSelect = PDUAct.pd.tipoPDU;
				fieldPatente.Text = PDUAct.pd.patente.Substring(0, PDUAct.pd.patente.Length - 1);
				fieldDVPatente.Text = PDUAct.pd.patente.Substring(PDUAct.pd.patente.Length - 1, 1);
				hintFechaPDU.Text = PDUAct.pd.fecha_circulacion;
			}

			setupViewPager(selectCategoryPDU);
			paggingCategoryPDU.SetupWithViewPager(selectCategoryPDU);

			var editedP = true;
			fieldPatente.TextChanged += (sender, e) => {
				if (editedP) {
					editedP = false;
					string datP = fieldPatente.Text.ToUpper();
					fieldPatente.Text = datP;
					fieldPatente.SetSelection(fieldPatente.Text.Length);
					editedP = true;
				}
			};
			var editedDV = true;
			fieldDVPatente.TextChanged += (sender, e) => {
				if (editedDV) {
					editedDV = false;
					string editado = fieldDVPatente.Text.ToUpper();
					fieldDVPatente.Text = editado;
					fieldDVPatente.SetSelection(fieldDVPatente.Text.Length);
					editedDV = true;
				}
			};
			var editedReP = true;
			fieldRePatente.TextChanged += (sender, e) => {
				if (editedReP) {
					editedReP = false;
					string datP = fieldRePatente.Text.ToUpper();
					fieldRePatente.Text = datP;
					fieldRePatente.SetSelection(fieldRePatente.Text.Length);
					editedReP = true;
				}
			};
			var editedReDV = true;
			fieldDVRePatente.TextChanged += (sender, e) => {
				if (editedReDV) {
					editedReDV = false;
					string editado = fieldDVRePatente.Text.ToUpper();
					fieldDVRePatente.Text = editado;
					fieldDVRePatente.SetSelection(fieldDVRePatente.Text.Length);
					editedReDV = true;
				}
			};

			imgNacional.Click += (sender, e) => {
				if (!typeSelect.Equals("nacional")) {
					typeSelect = "nacional";
					imgNacional.SetImageDrawable(Resources.GetDrawable(Resource.Drawable.pdu_naciona_on));
					imgMotos.SetImageDrawable(Resources.GetDrawable(Resource.Drawable.pdu_motos_off));
					imgExtranjeras.SetImageDrawable(Resources.GetDrawable(Resource.Drawable.pdu_extranjera_off));

					ttlTipoPDU.Text = Resources.GetString(Resource.String.pdu_title_nacional);
					hintTxtPDU.Text = Resources.GetString(Resource.String.pdu_hint_txt_patente);
					fieldDVPatente.Visibility = ViewStates.Visible;
					fieldDVRePatente.Visibility = ViewStates.Visible;
					guionDV.Visibility = ViewStates.Visible;
					guionReDV.Visibility = ViewStates.Visible;

					setupViewPager(selectCategoryPDU);
					paggingCategoryPDU.SetupWithViewPager(selectCategoryPDU);
				}
			};
			imgMotos.Click += (sender, e) => {
				if (!typeSelect.Equals("motos")) {
					typeSelect = "motos";
					imgNacional.SetImageDrawable(Resources.GetDrawable(Resource.Drawable.pdu_nacional_off));
					imgMotos.SetImageDrawable(Resources.GetDrawable(Resource.Drawable.pdu_motos_on));
					imgExtranjeras.SetImageDrawable(Resources.GetDrawable(Resource.Drawable.pdu_extranjera_off));

					ttlTipoPDU.Text = Resources.GetString(Resource.String.pdu_title_motos);
					hintTxtPDU.Text = Resources.GetString(Resource.String.pdu_hint_txt_patente_m);
					fieldDVPatente.Visibility = ViewStates.Gone;
					fieldDVRePatente.Visibility = ViewStates.Gone;
					guionDV.Visibility = ViewStates.Gone;
					guionReDV.Visibility = ViewStates.Gone;

					setupViewPager(selectCategoryPDU);
					paggingCategoryPDU.SetupWithViewPager(selectCategoryPDU);
				}
			};
			imgExtranjeras.Click += (sender, e) => {
				if (!typeSelect.Equals("extranjeras")) {
					typeSelect = "extranjeras";
					imgNacional.SetImageDrawable(Resources.GetDrawable(Resource.Drawable.pdu_nacional_off));
					imgMotos.SetImageDrawable(Resources.GetDrawable(Resource.Drawable.pdu_motos_off));
					imgExtranjeras.SetImageDrawable(Resources.GetDrawable(Resource.Drawable.pdu_extranjera_on));

					ttlTipoPDU.Text = Resources.GetString(Resource.String.pdu_title_extranjero);
					hintTxtPDU.Text = Resources.GetString(Resource.String.pdu_hint_txt_patente_m);
					fieldDVPatente.Visibility = ViewStates.Gone;
					fieldDVRePatente.Visibility = ViewStates.Gone;
					guionDV.Visibility = ViewStates.Gone;
					guionReDV.Visibility = ViewStates.Gone;

					setupViewPager(selectCategoryPDU);
					paggingCategoryPDU.SetupWithViewPager(selectCategoryPDU);
				}
			};
			bttnFechaPDU.Click += (sender, e) => {
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
					hintFechaPDU.Text = day + "/" + month + "/" + time.Year;
				});

				frag.parent = "PDT";
				frag.Show(FragmentManager, FragmentDatePicker.TAG);
			};
			infoFecha.Click += (sender, e) => {
				PDUAct.changeMainFragment(new FragmentTCAutopista(PDUAct),
										  Resources.GetString(Resource.String.autopista_id_info_date_pdu));
			};

			bttnContinuar.Click += (sender, e) => {
				Dictionary<string, string> fields = new Dictionary<string, string>();
				string patent;
				string repatent;
				fields.Add("Patente", fieldPatente.Text);
				fields.Add("Reingreso Patente", fieldRePatente.Text);
				if (typeSelect.Equals("nacional")) {
					fields.Add("DV Patente", fieldDVPatente.Text);
					fields.Add("Reingreso DV Patente", fieldDVRePatente.Text);
					patent = fieldPatente.Text + fieldDVPatente.Text;
					repatent = fieldRePatente.Text + fieldDVRePatente.Text;
				} else {
					patent = fieldPatente.Text;
					repatent = fieldRePatente.Text;
				}

				if ((bool)val.isEmpty(fields)["code"]) {
					CustomAlertDialog alert = new CustomAlertDialog(PDUAct, "¡Ojo!", (string)val.isEmpty(fields)["data"], "Aceptar", "", null, null);
					alert.showDialog();
				} else if (hintFechaPDU.Text.Equals(Resources.GetString(Resource.String.pdu_hint_fld_fecha))) {
					CustomAlertDialog alert = new CustomAlertDialog(PDUAct, "¡Ojo!", Resources.GetString(Resource.String.pdu_miss_f_circulacion), "Aceptar", "", null, null);
					alert.showDialog();
				} else if (!patent.Equals(repatent)) {
					CustomAlertDialog alert = new CustomAlertDialog(PDUAct, "¡Ojo!", Resources.GetString(Resource.String.autopista_pd_patente_dif), "Aceptar", "", null, null);
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

					parametros.Add("patente", fieldPatente.Text + fieldDVPatente.Text);
					parametros.Add("categoria", getCategoria());
					parametros.Add("firma", "");

					validaPatente(parametros);
				}
			};
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
			if (PDUAct.pd != null) {
				selectCategoryPDU.SetCurrentItem(PDUAct.pd.categoria - 1, true);
			}
		}

		public async void validaPatente(JObject parametros) {
			var respuesta = await MyClass.WorklightClient.UnprotectedInvokeAsync("validaPatente", "valida_patente", "POST", parametros);

			if (respuesta.Success) {
				if ((int)respuesta.State["Error"] == 0) {
					if ((int)respuesta.Response["retorno"] == 0) {
						PDUAct.pd = new PaseDiario();
						PDUAct.pd.patente = fieldPatente.Text + fieldDVPatente.Text;
						PDUAct.pd.fecha_circulacion = hintFechaPDU.Text;
						PDUAct.pd.categoria = Int32.Parse(getCategoria());
						PDUAct.pd.isPDU = true;
						PDUAct.pd.idBiller = idBiller;
						PDUAct.pd.idServicio = idServicio;
						PDUAct.pd.tipoPDU = typeSelect;

						PDUAct.changeMainFragment(new FragmentPDUVendido(RealmDB.GetInstance().realm.All<PaseDiario>().Count() + 1, !isLogin, PDUAct), "pduvendido");
					} else {
						CustomAlertDialog alert = new CustomAlertDialog(PDUAct, "¡Oops!", respuesta.Response["ValidaPatente"].ToString(), "Aceptar", "", null, null);
						alert.showDialog();
					}
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

		private string getCategoria() {
			switch (typeSelect) {
			case "nacional":
			return (selectCategoryPDU.CurrentItem + 1).ToString();
			case "motos":
			return "4";
			case "extranjeras":
			return (selectCategoryPDU.CurrentItem + 1).ToString();
			default:
			return "";
			}
		}
	}
}
