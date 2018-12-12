using System.Collections.Generic;
using Android.Support.V7.App;
using Android.Support.V4.App;

namespace ServipagMobile.Droid {
	public class RegisterData {
		public string name { get; set; }
		public string lastNP { get; set; }
		public string lastNM { get; set; }
		public string rut { get; set; }
		public int region { get; set; }
		public int comuna { get; set; }
		public string email { get; set; }
		public List<int> radioEmailSelected { get; set; }
		public string birthDate { get; set; }
		public string showBD { get; set; }
		public string radioGenderSelected { get; set; }
		public AppCompatActivity activity { get; set; }
		public string listType { get; set; }
		public string deviceType { get; set; }
		public string idFragment { get; set; }

		private static RegisterData instance;

		private RegisterData() { }

		public RegisterData(string name, string lnp, string lnm, string rut,
		                    int region, int comuna, string email, List<int> res,
		                    string bd, string sdb, string rgs, AppCompatActivity activity,
		                    string lt, string dt, string idFragment) {
			this.name = name;
			this.lastNP = lnp;
			this.lastNM = lnm;
			this.rut = rut;
			this.region = region;
			this.comuna = comuna;
			this.email = email;
			this.birthDate = bd;
			this.showBD = sdb;
			this.radioGenderSelected = rgs;
			this.activity = activity;
			this.listType = lt;
			this.deviceType = dt;
			this.idFragment = idFragment;

			if (radioEmailSelected != null) {
				this.radioEmailSelected = res;
			} else {
				this.radioEmailSelected = new List<int>();
				this.radioEmailSelected.Add(-1);
				this.radioEmailSelected.Add(-1);
				this.radioEmailSelected.Add(-1);
			}
		}

		public static RegisterData GetInstance() {
			if (instance == null) {
				instance = new RegisterData();
			}
			return instance;
		}
		//Metodo estático "sobrecargado" que devuelve una única instancia de "Singleton" ...
		public static RegisterData GetInstance(string name, string lnp, string lnm,
		                                       string rut, int region, int comuna, string email,
		                                       List<int> res, string bd, string sdb, string rgs,
		                                       AppCompatActivity activity, string lt, string dt,
		                                       string idFragment) {
			if (instance == null) {
				instance = new RegisterData(name, lnp, lnm, rut, region, comuna, email, res, bd, sdb, rgs, activity, lt, dt, idFragment);
			}
			return instance;
		}
	}
}
