using System;
namespace ServipagMobile.Droid {
	public class MisDeudas {
		public string nombreBiller;
		public string identificador;
		public string numDocumento;
		public string fechaVencimiento;
		public string hintServicio;
		public string estadoCuenta;
		public string aliasCuenta;
		public int saldoSelect;
		public bool hasSaldoAnterior;
		public int saldoActual;
		public int saldoAnterior;
		public bool isSelected;
		public int montoMostrar;

		public MisDeudas(string nombreBiller,
		                 string identificador,
		                 string numDocumento,
		                 string fechaVencimiento,
		                 string hintServicio,
		                 string estadoCuenta,
		                 string aliasCuenta,
		                 int saldoSelect,
		                 bool hasSaldoAnterior,
	                	 int saldoActual,
		                 int saldoAnterior) {

			this.nombreBiller = nombreBiller;
			this.identificador = identificador;
			this.numDocumento = numDocumento;
			this.fechaVencimiento = fechaVencimiento;
			this.hintServicio = hintServicio;
			this.estadoCuenta = estadoCuenta;
			this.aliasCuenta = aliasCuenta;
			this.saldoSelect = saldoSelect;
			this.hasSaldoAnterior = hasSaldoAnterior;
			this.saldoActual = saldoActual;
			this.saldoAnterior = saldoAnterior;
			this.isSelected = true;
			this.montoMostrar = saldoActual;
		}
	}
}
