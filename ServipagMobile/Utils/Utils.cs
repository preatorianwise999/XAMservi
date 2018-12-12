using System;
using System.Collections.Generic;
using ServipagMobile.Droid;

namespace ServipagMobile {
	public class Utils {
		public Utils() {
		}

		public string getUserId(string rut) {
			string userId = "";
			userId = rut.ToUpper();
			userId = userId.Replace("-", "");
			userId = userId.Replace(".", "");

			switch (userId.Length) {
				case 7:
					userId = "000" + userId;
					break;
				case 8:
					userId = "00" + userId;
					break;
				case 9:
					userId = "0" + userId;
					break;
			}
			return userId;
		}

		public void QuickSortCuentas(List<MisCuentas> cuentas, int left, int right, bool esMenorMayor) {
			MisCuentas pivote;
			int le, ri;
			le = left;
			ri = right;
			pivote = cuentas[(left + right) / 2];

			do {
				if (esMenorMayor) {
					while (string.Compare(cuentas[le].billerCuenta, pivote.billerCuenta, StringComparison.CurrentCulture) < 0)
						le++;
					while (string.Compare(cuentas[ri].billerCuenta, pivote.billerCuenta, StringComparison.CurrentCulture) > 0)
						ri--;
				} else {
					while (string.Compare(cuentas[le].billerCuenta, pivote.billerCuenta, StringComparison.CurrentCulture) > 0)
						le++;
					while (string.Compare(cuentas[ri].billerCuenta, pivote.billerCuenta, StringComparison.CurrentCulture) < 0)
						ri--;
				}
				
				if (le <= ri) {
					MisCuentas temp;
					temp = cuentas[le];
					cuentas[le] = cuentas[ri];
					cuentas[ri] = temp;
					le++;
					ri--;
				}
			} while (le <= ri);

			if (left < ri) {
				QuickSortCuentas(cuentas, left, ri, esMenorMayor);
			}
			if (le < right) {
				QuickSortCuentas(cuentas, le, right, esMenorMayor);
			}
		}

		public void QuickSortAlias(List<MisCuentas> cuentas, int left, int right, bool esMenorMayor) {
			MisCuentas pivote;
			int le, ri;
			le = left;
			ri = right;
			pivote = cuentas[(left + right) / 2];

			do {
				if (esMenorMayor) {
					while (string.Compare(cuentas[le].aliasCuenta, pivote.aliasCuenta, StringComparison.CurrentCulture) < 0)
						le++;
					while (string.Compare(cuentas[ri].aliasCuenta, pivote.aliasCuenta, StringComparison.CurrentCulture) > 0)
						ri--;
				} else {
					while (string.Compare(cuentas[le].aliasCuenta, pivote.aliasCuenta, StringComparison.CurrentCulture) > 0)
						le++;
					while (string.Compare(cuentas[ri].aliasCuenta, pivote.aliasCuenta, StringComparison.CurrentCulture) < 0)
						ri--;
				}

				if (le <= ri) {
					MisCuentas temp;
					temp = cuentas[le];
					cuentas[le] = cuentas[ri];
					cuentas[ri] = temp;
					le++;
					ri--;
				}
			} while (le <= ri);

			if (left < ri) {
				QuickSortAlias(cuentas, left, ri, esMenorMayor);
			}
			if (le < right) {
				QuickSortAlias(cuentas, le, right, esMenorMayor);
			}
		}

		public int ConvertPixelsToDp(float pixelValue, float screenDensity) {
			var dp = (int)((pixelValue) / screenDensity);
			return dp;
		}

		public string getFormaPago(int idFormaPago) {
			string fPago = "TB";
			switch(idFormaPago) {
				case 1:
					fPago = "TB";
				break;
				case 2:
					fPago = "CC";
				break;
				case 3:
					fPago = "TC";
				break;
				case 4:
					fPago = "TD";
				break;
			}

			return fPago;
		}

		public string formatearRut(string rut) {
			int cont = 0;
			string format;
			if (rut.Length == 0) {
				return "";
			} else {
				rut = rut.Replace(".", "");
				rut = rut.Replace("-", "");
				if (rut.Length == 0) {
					return "";
				}
				format = "-" + rut.Substring(rut.Length - 1);
				for (int i = rut.Length - 2; i >= 0; i--) {
					format = rut.Substring(i, 1) + format;
					cont++;
					if (cont == 3 && i != 0) {
						format = "." + format;
						cont = 0;
					}
				}
				return format;
			}
		}
	}
}
