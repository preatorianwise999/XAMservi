using System;
using System.Collections.Generic;

namespace ServipagMobile {
	public class RandomCaptcha {
		public int image;
		public int message;
		public bool selected;
		public bool isSelected;
		public int id;

		public RandomCaptcha(int image, int message, bool isSelected, bool selected, int id) {
			this.image = image;
			this.message = message;
			this.isSelected = isSelected;
			this.selected = selected;
			this.id = id;
		}
	}
}
