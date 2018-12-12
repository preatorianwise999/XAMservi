using System;
using System.Collections.Generic;

namespace ServipagMobile {
	public class ListadoComuna {
		public List<RegionComuna> listaComunas { get; set; }

		private static ListadoComuna instance;

		private ListadoComuna() { }

		private ListadoComuna(List<RegionComuna> list) {
			this.listaComunas = list;
		}


		public static ListadoComuna GetInstance() {
			if (instance == null) {
				instance = new ListadoComuna();
			}
			return instance;
		}
		//Metodo estático "sobrecargado" que devuelve una única instancia de "Singleton" ...
		public static ListadoComuna GetInstance(List<RegionComuna> list) {
			if (instance == null) {
				instance = new ListadoComuna(list);
			}
			return instance;
		}
	}
}
