using System;
using System.Collections.Generic;

namespace ServipagMobile.Droid {
	public class ListadoRegion {
		public List<RegionComuna> listaRegiones { get; set; }

		private static ListadoRegion instance;

		private ListadoRegion() { }

		private ListadoRegion(List<RegionComuna> list) {
			this.listaRegiones = list;
		}


		public static ListadoRegion GetInstance() {
			if (instance == null) {
				instance = new ListadoRegion();
			}
			return instance;
		}
		//Metodo estático "sobrecargado" que devuelve una única instancia de "Singleton" ...
		public static ListadoRegion GetInstance(List<RegionComuna> list) {
			if (instance == null) {
				instance = new ListadoRegion(list);
			}
			return instance;
		}
	}
}
