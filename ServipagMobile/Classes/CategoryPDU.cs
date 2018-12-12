using System;
namespace ServipagMobile {
	public class CategoryPDU {
		public string category;
		public int idImgCategory;

		public CategoryPDU(string category, int idImgCategory) {
			this.category = category;
			this.idImgCategory = idImgCategory;
		}
	}
}
