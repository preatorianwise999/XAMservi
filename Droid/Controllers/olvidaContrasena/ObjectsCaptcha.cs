using System;
using System.Collections.Generic;
using Android.Content.Res;

namespace ServipagMobile.Droid {
	public class ObjectsCaptcha {
		public List<RandomCaptcha> listObjectCaptcha;

		public ObjectsCaptcha() {
			listObjectCaptcha = new List<RandomCaptcha>();

			listObjectCaptcha.Add(new RandomCaptcha(Resource.Drawable.agua, Resource.String.captcha_agua, false, false, 1));
			listObjectCaptcha.Add(new RandomCaptcha(Resource.Drawable.autopistas, Resource.String.captcha_autopista, false, false, 2));
			listObjectCaptcha.Add(new RandomCaptcha(Resource.Drawable.carrier, Resource.String.captcha_telefono, false, false, 3));
			listObjectCaptcha.Add(new RandomCaptcha(Resource.Drawable.casas_comerciales, Resource.String.captcha_bolsa, false, false, 4));
			listObjectCaptcha.Add(new RandomCaptcha(Resource.Drawable.cementerios, Resource.String.captcha_arboles, false, false, 5));
			listObjectCaptcha.Add(new RandomCaptcha(Resource.Drawable.clubes, Resource.String.captcha_casa, false, false, 6));
			listObjectCaptcha.Add(new RandomCaptcha(Resource.Drawable.creditos, Resource.String.captcha_tarjeta, false, false, 7));
			listObjectCaptcha.Add(new RandomCaptcha(Resource.Drawable.gas, Resource.String.captcha_gas, false, false, 8));
			listObjectCaptcha.Add(new RandomCaptcha(Resource.Drawable.internet, Resource.String.captcha_wifi, false, false, 9));
			listObjectCaptcha.Add(new RandomCaptcha(Resource.Drawable.luz, Resource.String.captcha_ampolleta, false, false, 10));
			listObjectCaptcha.Add(new RandomCaptcha(Resource.Drawable.promotoras, Resource.String.captcha_hoja, false, false, 11));
			listObjectCaptcha.Add(new RandomCaptcha(Resource.Drawable.publicidad, Resource.String.captcha_altavoz, false, false, 12));
			listObjectCaptcha.Add(new RandomCaptcha(Resource.Drawable.recargas_tv, Resource.String.captcha_tv, false, false, 13));
			listObjectCaptcha.Add(new RandomCaptcha(Resource.Drawable.seguridad_alarmas, Resource.String.captcha_candado, false, false, 14));
			listObjectCaptcha.Add(new RandomCaptcha(Resource.Drawable.seguros, Resource.String.captcha_escudo, false, false, 15));
			listObjectCaptcha.Add(new RandomCaptcha(Resource.Drawable.tv_satelital, Resource.String.captcha_antena, false, false, 16));
		}

		public List<RandomCaptcha> randomImages() {
			List<RandomCaptcha> lrc = new List<RandomCaptcha>();
			int random;

			Random r = new Random();

			for (var i = 0; i < 9; i++) {
				random = r.Next(0, listObjectCaptcha.Count - 1);
				lrc.Add(listObjectCaptcha[random]);
			}

			random = r.Next(0, lrc.Count - 1);
			lrc[random].selected = true;

			for (var j = 0; j < lrc.Count; j++) {
				if (lrc[random].id == lrc[j].id) {
					lrc[j].selected = true;
				}
			}

			return lrc;
		}
	}
}
