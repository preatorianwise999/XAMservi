using System;
using Realms;

namespace ServipagMobile {
	public class RealmDB {
		public Realm realm { get; set; }
		private static RealmDB instance;

		private RealmDB() { }

		private RealmDB(Realm realm) {
			this.realm = realm;
		}

		public static RealmDB GetInstance() {
			if (instance == null) {
				instance = new RealmDB();
			}
			return instance;
		}
		//Metodo estático "sobrecargado" que devuelve una única instancia de "Singleton" ...
		public static RealmDB GetInstance(Realm realm) {
			if (instance == null) {
				instance = new RealmDB(realm);
			}
			return instance;
		}
	}
}
