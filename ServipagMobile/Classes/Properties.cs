using System;
using System.Collections.Generic;

namespace ServipagMobile {
	public class Properties {
		public List<TiposMediosPago> tiposMediosPago { get; set; }
		public string appversion { get; set; }
		public string slackHookIOS_QA { get; set; }
		public string showErrors { get; set; }
		public string qaDroid { get; set; }
		public string qaIOS { get; set; }
		public string slackHookDroid_QA { get; set; }
		public string slackHookError { get; set; }
		public string timeout { get; set; }
		public string url { get; set; }
		public string shownotification { get; set; }

		private static Properties instance;

		private Properties() { }

		private Properties(List<TiposMediosPago> tmp, string appv, string shiQA, string se, string qd, string qi, string shdQA, string she, string t, string u, string sn) {
			this.tiposMediosPago = tmp;
			this.appversion = appv;
			this.slackHookIOS_QA = shiQA;
			this.showErrors = se;
			this.qaDroid = qd;
			this.qaIOS = qi;
			this.slackHookDroid_QA = shdQA;
			this.slackHookError = she;
			this.timeout = t;
			this.url = u;
			this.shownotification = sn;
		}

		public static Properties GetInstance() {
			if (instance == null) {
				instance = new Properties();
			}
			return instance;
		}
		//Metodo estático "sobrecargado" que devuelve una única instancia de "Singleton" ...
		public static Properties GetInstance(List<TiposMediosPago> tmp, string appv, string shiQA, string se, string qd, string qi, string shdQA, string she, string t, string u, string sn) {
			if (instance == null) {
				instance = new Properties(tmp, appv, shiQA, se, qd, qi, shdQA, she, t, u, sn);
			}
			return instance;
		}
	}
}
