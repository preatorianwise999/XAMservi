using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text.RegularExpressions;
using Newtonsoft.Json.Linq;

namespace ServipagMobile.Droid {
	public class Validations {

		public Validations() { }

		public JObject isEmpty(Dictionary<string, string> values) {
			JObject response = new JObject();
			string data = "";
			string value;

			foreach (string key in values.Keys) {
				if (values.TryGetValue(key, out value)) {
					if (value == "" || value == "Monto") {
						data = data + key + ",";
					}
				}
			}

			if (data == "") {
				response.Add("code", false);
				response.Add("data", "");
			} else {
				response.Add("code", true);
				response.Add("data", "Debes ingresar los siguientes datos para continuar: \n\n" + data.Replace(",", "\n"));
			}

			return response;
		}

		public JObject areEquals(string val1, string val2, string title) {
			JObject response = new JObject();

			if (val1.Equals(val2)) {
				response.Add("code", true);
				response.Add("data", "");
			} else {
				response.Add("code", false);
				response.Add("data", "Los " + title + " ingresados no coinciden.");
			}

			return response;
		}

		public JObject areSelected(Dictionary<string, string> values) {
			JObject response = new JObject();
			string data = "";
			string value;

			foreach (string key in values.Keys) {
				if (values.TryGetValue(key, out value)) {
					if (value.Equals(key)) {
						data = data + key + ",";
					}
				}
			}

			if (data == "") {
				response.Add("code", true);
				response.Add("data", "");
			} else {
				response.Add("code", false);
				response.Add("data", "Debes seleccionar los siguientes datos para continuar: \n\n" + data.Replace(",", "\n"));
			}

			return response;
		}

		public JObject areRandomSelected(List<RandomCaptcha> list) {
			JObject response = new JObject();
			int countSelected = 0;
			int countUnSelected = 0;
			int countIsSelected = 0;

			foreach (RandomCaptcha obj in list) {
				if (obj.isSelected != false) {
					countIsSelected++;
				}
				if (obj.selected) {
					if (obj.selected != obj.isSelected) {
						countSelected++;
					}
				} else {
					if (obj.isSelected) {
						countUnSelected++;
					}
				}
			}

			if (countIsSelected == 0) {
				response.Add("code", false);
				response.Add("data", "Debes seleccionar imágenes");
			} else if (countSelected > 0) {
				response.Add("code", false);
				response.Add("data", "Las imágenes no coinciden, vuelve a intentarlo");
			} else if (countUnSelected > 0){
				response.Add("code", false);
				response.Add("data", "Las imágenes no coinciden, vuelve a intentarlo");
			} else {
				response.Add("code", true);
				response.Add("data", "");
			}

			return response;
		}

		public JObject areChecked(string title, List<bool> isChecked) {
			JObject response = new JObject();
			int cant = 0;

			for (int i = 0; i < isChecked.Count; i++) {
				if (!isChecked[i]) {
					cant++;
				}
			}

			if (cant == 1) {
				response.Add("code", true);
				response.Add("data", "");
			} else {
				response.Add("code", false);
				response.Add("data", "Debe seleccionar " + title + " para continuar.");
			}

			return response;
		}

		public bool expresionEmail(string email) {
			Regex expresion = new Regex("([0-9]{0,50}[a-zA-Z]{1,50})([._+]{0,1})([0-9]{0,50}[a-zA-Z]{1,50})([@]{1})([a-zA-Z]{3,15})([.]{1})([a-zA-Z]{2,4})");

			if (!expresion.IsMatch(email)) {
				return false;
			} else {
				return true;
			}
		}

		public bool expresionBorrar(string password) {
			Regex expresionDigits = new Regex("([a-zA-Z0-9,;.:_!¡?¿]{1,15})$");

			if (!expresionDigits.IsMatch(password)) {
				return true;
			} else {
				return false;
			}
		}

		public string expresionPassword(string password) {
			Regex expresionMedia = new Regex("(?!^[0-9]*$)(?!^[a-zA-Z]*$)^([a-zA-Z0-9]{6,12})$");
			Regex expresionFuerte = new Regex("(?=^.{6,12}$)(?=.*\\d)(?=.*[a-z])(?=.*[A-Z])(?=.*[,;.:_!¡?¿])(?!.*\\s).*$");

			if (expresionMedia.IsMatch(password)) {
				return "medium";
			} else if (expresionFuerte.IsMatch(password)) {
				return "strong";
			} else {
				return "weak";
			}
		}

		public bool expresionRut(string rut) {
			Regex expresion = new Regex("^(\\d{1,2}[.]\\d{3}[.]\\d{3}[-][0-9kK]{1})");

			if (!expresion.IsMatch(rut)) {
				return false;
			} else {
				return true;
			}
		}

		public string Digito(int rut) {
			int suma = 0;
			int multiplicador = 1;
			while (rut != 0) {
				multiplicador++;
				if (multiplicador == 8)
					multiplicador = 2;
				suma += (rut % 10) * multiplicador;
				rut = rut / 10;
			}
			suma = 11 - (suma % 11);
			if (suma == 11) {
				return "0";
			} else if (suma == 10) {
				return "K";
			} else {
				return suma.ToString();
			}
		}

		public JObject validateRut(string rut) {
			JObject response = new JObject();
			int m = 0, s = 1;
			int rutAux;
			char dv;

			try {
				rut = rut.ToUpper();
				rut = rut.Replace("-", "");
				rut = rut.Replace(".", "");
				rutAux = Convert.ToInt32(rut.Substring(0, rut.Length - 1));
				dv = Convert.ToChar(rut.Substring(rut.Length - 1, 1));

				for (; rutAux != 0; rutAux /= 10) {
					s = (s + rutAux % 10 * (9 - m++ % 6)) % 11;
				}
				if (dv == (char)(s != 0 ? s + 47 : 75)) {
					response.Add("code", true);
					response.Add("data", "");
				} else {
					response.Add("code", false);
					response.Add("data", "El rut ingresado es incorrecto.");
				}
			} catch (Exception) {
			}
			return response;
		}

		public bool validPhoneNumber(string number) {
			if (number.Length == 9) {
				return true;
			} else {
				return false;
			}
		}

		public bool validAmountOnRange(int min, int max, int val) {
			if ((val > min) && (val < max)) {
				return true;
			} else {
				return false;
			}
		}

		public bool validateCFTPayment(List<BuscaDeudas> misDeudas) {
			int countError = 0;
			var education = misDeudas.FindAll(x => x.id_biller == 829).ToList();
			var educationOrder = education.OrderBy(x => x.boleta).ToList();

			for (int i = 0; i < educationOrder.Count - 2; i++) {
				if (!educationOrder[i].isSelected) {
					if (educationOrder[i + 1].isSelected) {
						countError++;
					}
				}
			}

			if (countError == 0) {
				return true;
			} else {
				return false;
			}
		}

		public bool validateIPPayment(List<BuscaDeudas> misDeudas) {
			int countError = 0;
			var education = misDeudas.FindAll(x => x.id_biller == 828).ToList();
			var educationOrder = education.OrderBy(x => x.boleta).ToList();

			for (int i = 0; i < educationOrder.Count - 2; i++) {
				if (!educationOrder[i].isSelected) {
					if (educationOrder[i + 1].isSelected) {
						countError++;
					}
				}
			}

			if (countError == 0) {
				return true;
			} else {
				return false;
			}
		}
	}
}
