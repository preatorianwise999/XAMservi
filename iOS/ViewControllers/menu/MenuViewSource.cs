using System;
using Foundation;
using UIKit;

namespace ServipagMobile.iOS {
	public partial class MenuViewSource : UITableViewSource {
		private MenuCellsLayout[] items;
		private string CellIdentifier = "TableCell";
		private MenuController menuContext;
		private bool isPagoExpress;
		public MenuViewSource(MenuCellsLayout[] items, MenuController ctxt, bool isPE) {
			this.items = items;
			this.menuContext = ctxt;
			this.isPagoExpress = isPE;
		}

		public override nint RowsInSection(UITableView tableview, nint section) {
			return items.Length;
		}

		public override UITableViewCell GetCell(UITableView tableView, NSIndexPath indexPath) {
			
			MenuCellsLayout item = items[indexPath.Row];
			UITableViewCell cell = tableView.DequeueReusableCell(CellIdentifier);

			if (cell == null) {
				cell = new UITableViewCell(UITableViewCellStyle.Default, CellIdentifier);
				if (!isPagoExpress) {
					cell.ImageView.Image = UIImage.FromBundle(item.imageRow);
				}
				cell.BackgroundColor = UIColor.FromRGB(0, 158, 184);
				cell.TextLabel.TextColor = UIColor.FromRGB(255, 255, 255);
				cell.TextLabel.Text = item.nameRow;
			}

			return cell;
		}

		public override void RowSelected(UITableView tableView, NSIndexPath indexPath) {
			if (items[indexPath.Row].nameRow == "Cerrar Sesion") {
				menuContext.showMenuPagoExpress();
			} else {
				UIAlertController okAlertController =
				UIAlertController.Create("Row Selected", items[indexPath.Row].nameRow, UIAlertControllerStyle.Alert);
				okAlertController.AddAction(UIAlertAction.Create("OK", UIAlertActionStyle.Default, null));

				menuContext.PresentViewController(okAlertController, true, null);
			}

    		tableView.DeselectRow(indexPath, true);
		}

		public override string TitleForHeader(UITableView tableView, nint section) {
			return "";
		}
		public override string TitleForFooter(UITableView tableView, nint section) {
			return "";
		}
	}
}

