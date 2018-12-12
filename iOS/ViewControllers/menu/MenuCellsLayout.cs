using System;
namespace ServipagMobile.iOS {
	public class MenuCellsLayout {
		public string nameRow;
		public string subtitleRow;
		public string imageRow;
		public bool isHeader;

		public MenuCellsLayout(string nameRow, string subtitleRow, string imageRow, bool isHeader) {
			this.nameRow = nameRow;
			this.subtitleRow = subtitleRow;
			this.imageRow = imageRow;
			this.isHeader = isHeader;
		}
	}
}
