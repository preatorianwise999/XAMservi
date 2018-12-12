using System.Collections.Generic;
using System.Linq;
using Android.OS;
using Android.Support.V4.App;
using Android.Views;
using Android.Widget;

namespace ServipagMobile.Droid {
	public class FragmentRutEmail : Fragment {
		private PagoActivity pa;
		private List<MediosPago> mediosPago;
		private List<BuscaDeudas> misDeudas;

		private SolicitaRecarga solicitaRecarga;
		private RecargasActivity ra;
		private bool isUR;

		private EditText rutField, mailField;
		private Button bttnNext;
		private Validations val;
		private Utils utils;

		private PDUActivity PDUAct;
		private BuscaDeudas deudaPDU;
		private bool isUPDU;

		public FragmentRutEmail() { }

		public FragmentRutEmail(PagoActivity pa, List<MediosPago> mediosPago, List<BuscaDeudas> misDeudas) {
			this.pa = pa;
			this.mediosPago = mediosPago;
			this.misDeudas = misDeudas;
		}

		public FragmentRutEmail(List<MediosPago> mediosPago, SolicitaRecarga solicitaRecarga, bool isUR, RecargasActivity ra) {
			this.ra = ra;
			this.isUR = isUR;
			this.solicitaRecarga = solicitaRecarga;
			this.mediosPago = mediosPago;
		}

		public FragmentRutEmail(List<MediosPago> mediosPago, BuscaDeudas deudaPDU, bool isUPDU, PDUActivity PDUAct) {
			this.PDUAct = PDUAct;
			this.deudaPDU = deudaPDU;
			this.isUPDU = isUPDU;
			this.mediosPago = mediosPago;
		}

		public override void OnCreate(Bundle savedInstanceState) {
			base.OnCreate(savedInstanceState);
			this.val = new Validations();
			this.utils = new Utils();
		}

		public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState) {
			return inflater.Inflate(Resource.Layout.FragmentRutEmail, container, false);
		}

		public override void OnViewCreated(View view, Bundle savedInstanceState) {
			base.OnViewCreated(view, savedInstanceState);

			rutField = view.FindViewById<EditText>(Resource.Id.rutField);
			mailField = view.FindViewById<EditText>(Resource.Id.mailField);
			bttnNext = view.FindViewById<Button>(Resource.Id.bttnNext);

			var edited = true;
			if (RealmDB.GetInstance().realm.All<UserDataPE>().Count() == 1) {
				rutField.Text = RealmDB.GetInstance().realm.All<UserDataPE>().First().rut;
				rutField.SetTextColor(Resources.GetColor(Resource.Color.servipag_green));

				mailField.Text = RealmDB.GetInstance().realm.All<UserDataPE>().First().correo;
				mailField.SetTextColor(Resources.GetColor(Resource.Color.servipag_green));
			}
			rutField.TextChanged += (sender, e) => {
				if (edited) {
					edited = false;
					string editado = utils.formatearRut(rutField.Text);
					rutField.Text = editado;
					if (val.expresionRut(rutField.Text) && (bool)val.validateRut(rutField.Text)["code"]) {
						rutField.SetTextColor(Resources.GetColor(Resource.Color.servipag_green));
					} else {
						rutField.SetTextColor(Resources.GetColor(Resource.Color.servipag_red));
					}
					rutField.SetSelection(rutField.Text.Length);
					edited = true;
				}
			};

			mailField.TextChanged += delegate {
				if (val.expresionEmail(mailField.Text)) {
					mailField.SetTextColor(Resources.GetColor(Resource.Color.servipag_green));
				} else {
					mailField.SetTextColor(Resources.GetColor(Resource.Color.servipag_red));
				}
			};

			bttnNext.Click += (sender, e) => {
				bttnNextEvent();
			};
		}

		private void bttnNextEvent() {
			Dictionary<string, string> fields = new Dictionary<string, string>();
			fields.Add("RUT", rutField.Text);
			fields.Add("e-mail", mailField.Text);

			if (pa != null) {
				openMediosPago(fields);
			} else if (ra != null) {
				openMediosPagoRecarga(fields);
			} else if (PDUAct != null) {
				openMediosPagoPDU(fields);
			}
		}

		private void openMediosPago(Dictionary<string, string> fields) {
			if ((bool)val.isEmpty(fields)["code"]) {
				CustomAlertDialog alert = new CustomAlertDialog(pa, "¡Ojo!", (string)val.isEmpty(fields)["data"], "Aceptar", "", null, null);
				alert.showDialog();
			} else if (!val.expresionEmail(mailField.Text)) {
				CustomAlertDialog alert = new CustomAlertDialog(pa, "¡Ojo!", "El e-mail ingresado es incorrecto.", "Aceptar", "", null, null);
				alert.showDialog();
			} else if (val.expresionRut(rutField.Text) && (bool)val.validateRut(rutField.Text)["code"]) {
				UserData.GetInstance().rut = utils.getUserId(rutField.Text);
				UserData.GetInstance().correo = mailField.Text;

				if (RealmDB.GetInstance().realm.All<UserDataPE>().Count() == 1) {
					RealmDB.GetInstance().realm.Write(() => {
						var first = RealmDB.GetInstance().realm.All<UserDataPE>().First();
						RealmDB.GetInstance().realm.Remove(first);

						var userDataPE = new UserDataPE {
							rut = rutField.Text,
							correo = mailField.Text
						};

						RealmDB.GetInstance().realm.Add(userDataPE);
					});
				} else {
					RealmDB.GetInstance().realm.Write(() => {
						var userDataPE = new UserDataPE {
							rut = rutField.Text,
							correo = mailField.Text
						};

						RealmDB.GetInstance().realm.Add(userDataPE);
					});
				}
				pa.changeMainFragment(new FragmentMediosPago(pa.SupportFragmentManager,
															 mediosPago,
															 misDeudas,
															 false), Resources.GetString(Resource.String.medios_pago_id_fragment));
			} else {
				CustomAlertDialog alert = new CustomAlertDialog(pa, "¡Ojo!", (string)val.validateRut(rutField.Text)["data"], "Aceptar", "", null, null);
				alert.showDialog();
			}
		}

		private void openMediosPagoRecarga(Dictionary<string, string> fields) {
			if ((bool)val.isEmpty(fields)["code"]) {
				CustomAlertDialog alert = new CustomAlertDialog(ra, "¡Ojo!", (string)val.isEmpty(fields)["data"], "Aceptar", "", null, null);
				alert.showDialog();
			} else if (!val.expresionEmail(mailField.Text)) {
				CustomAlertDialog alert = new CustomAlertDialog(ra, "¡Ojo!", "El Email ingresado es incorrecto.", "Aceptar", "", null, null);
				alert.showDialog();
			} else if (val.expresionRut(rutField.Text) && (bool)val.validateRut(rutField.Text)["code"]) {
				UserData.GetInstance().rut = utils.getUserId(rutField.Text);
				UserData.GetInstance().correo = mailField.Text;

				if (RealmDB.GetInstance().realm.All<UserDataPE>().Count() == 1) {
					RealmDB.GetInstance().realm.Write(() => {
						var first = RealmDB.GetInstance().realm.All<UserDataPE>().First();
						RealmDB.GetInstance().realm.Remove(first);

						var userDataPE = new UserDataPE {
							rut = rutField.Text,
							correo = mailField.Text
						};

						RealmDB.GetInstance().realm.Add(userDataPE);
					});
				} else {
					RealmDB.GetInstance().realm.Write(() => {
						var userDataPE = new UserDataPE {
							rut = rutField.Text,
							correo = mailField.Text
						};

						RealmDB.GetInstance().realm.Add(userDataPE);
					});
				}
				ra.changeMainFragment(new FragmentMediosPago(ra.SupportFragmentManager,
															 mediosPago,
															 solicitaRecarga,
															 isUR,
															 false), Resources.GetString(Resource.String.recargas_medios_pago_ur_title));
			} else {
				CustomAlertDialog alert = new CustomAlertDialog(ra, "¡Ojo!", (string)val.validateRut(rutField.Text)["data"], "Aceptar", "", null, null);
				alert.showDialog();
			}
		}

		private void openMediosPagoPDU(Dictionary<string, string> fields) {
			if ((bool)val.isEmpty(fields)["code"]) {
				CustomAlertDialog alert = new CustomAlertDialog(PDUAct, "¡Ojo!", (string)val.isEmpty(fields)["data"], "Aceptar", "", null, null);
				alert.showDialog();
			} else if (!val.expresionEmail(mailField.Text)) {
				CustomAlertDialog alert = new CustomAlertDialog(PDUAct, "¡Ojo!", "El Email ingresado es incorrecto.", "Aceptar", "", null, null);
				alert.showDialog();
			} else if (val.expresionRut(rutField.Text) && (bool)val.validateRut(rutField.Text)["code"]) {
				UserData.GetInstance().rut = utils.getUserId(rutField.Text);
				UserData.GetInstance().correo = mailField.Text;

				if (RealmDB.GetInstance().realm.All<UserDataPE>().Count() == 1) {
					RealmDB.GetInstance().realm.Write(() => {
						var first = RealmDB.GetInstance().realm.All<UserDataPE>().First();
						RealmDB.GetInstance().realm.Remove(first);

						var userDataPE = new UserDataPE {
							rut = rutField.Text,
							correo = mailField.Text
						};

						RealmDB.GetInstance().realm.Add(userDataPE);
					});
				} else {
					RealmDB.GetInstance().realm.Write(() => {
						var userDataPE = new UserDataPE {
							rut = rutField.Text,
							correo = mailField.Text
						};

						RealmDB.GetInstance().realm.Add(userDataPE);
					});
				}
				PDUAct.changeMainFragment(new FragmentMediosPago(PDUAct.SupportFragmentManager,
																 mediosPago,
																 deudaPDU,
																 false,
																 false), Resources.GetString(Resource.String.autopista_id_medios_pago_pdu));
			} else {
				CustomAlertDialog alert = new CustomAlertDialog(PDUAct, "¡Ojo!", (string)val.validateRut(rutField.Text)["data"], "Aceptar", "", null, null);
				alert.showDialog();
			}
		}
	}
}
