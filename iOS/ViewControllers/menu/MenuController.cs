using System;
using System.Drawing;
using CoreAnimation;
using CoreGraphics;
using Foundation;
using UIKit;

namespace ServipagMobile.iOS {
	public partial class MenuController : BaseController {
		private NSUserDefaults deviceDim;

		private MenuCellsLayout[] menuItemsPagoExpress = new MenuCellsLayout[] {
			new MenuCellsLayout("Como Funciona", "", "", false),
			new MenuCellsLayout("Corporativo", "", "", false),
			new MenuCellsLayout("Sucursales", "", "", false),
			new MenuCellsLayout("Autopistas", "", "", false),
			new MenuCellsLayout("Recargas", "", "", false)
		};

		private MenuCellsLayout[] menuItemsInicioSesion = new MenuCellsLayout[] {
			new MenuCellsLayout("Recargas", "", "recargas", false),
			new MenuCellsLayout("Mis Cuentas", "", "mis_cuentas", false),
			new MenuCellsLayout("Sucursales", "", "sucursales", false),
			new MenuCellsLayout("Cerrar Sesion", "", "cierra_sesion", false)
		};

		public MenuController(IntPtr handle) : base(handle) {
			deviceDim = NSUserDefaults.StandardUserDefaults;
		}

		public override void ViewDidLoad() {
			base.ViewDidLoad();
			var contentController = (ContentController)Storyboard.InstantiateViewController("ContentController");

			setValuesPagoExpress();
			setValuesInicioSesion();

		}

		public void setValuesPagoExpress() {
			MenuPagoExpress.Frame = new RectangleF(0, 0, deviceDim.FloatForKey("width") , deviceDim.FloatForKey("height"));
			MenuPagoExpress.BackgroundColor = UIColor.FromRGB(0, 158, 184);
			MenuPagoExpress.TableHeaderView = new UIView(new RectangleF(0, 0, deviceDim.FloatForKey("width"), 30));
			MenuPagoExpress.TableFooterView = new UIView(new RectangleF(0, 0, deviceDim.FloatForKey("width"), 60));

			MenuPagoExpress.Source = new MenuViewSource(menuItemsPagoExpress, this, true);
			Add(MenuPagoExpress);
		}

		public void setValuesInicioSesion() {
			MenuIniciaSesion.Frame = new RectangleF(0, 0, deviceDim.FloatForKey("width"), deviceDim.FloatForKey("height"));
			MenuIniciaSesion.BackgroundColor = UIColor.FromRGB(0, 158, 184);
			var header = new UIView(new RectangleF(0, 0, deviceDim.FloatForKey("width"), 200));

			UIImageView userImage = new UIImageView();
			UILabel welcomeLabel = new UILabel() {
				TextColor = UIColor.White,
				BackgroundColor = UIColor.Clear,
				Text = "Bienvenido Daniel",
				TextAlignment = UITextAlignment.Center
			};

			userImage.Frame = new RectangleF(((float)(deviceDim.FloatForKey("width") / 1.5) - 120) / 2, 40, 120, 120);
			CALayer userImageCircle = userImage.Layer;
			userImageCircle.CornerRadius = 60;
			userImageCircle.MasksToBounds = true;
			welcomeLabel.Frame = new CGRect(0, userImage.Frame.Height + 45,
			                                (float)(deviceDim.FloatForKey("width") / 1.5), 25);
			var imageUrl = new NSUrl("https://www.smashingmagazine.com/wp-content/uploads/2015/06/10-dithering-opt.jpg");
			var imageData = NSData.FromUrl(imageUrl);
			userImage.Image = UIImage.LoadFromData(imageData);

			header.AddSubview(userImage);
			header.AddSubview(welcomeLabel);

			MenuIniciaSesion.TableHeaderView = header;
			MenuIniciaSesion.TableFooterView = new UIView(new RectangleF(0, 0, deviceDim.FloatForKey("width"), 60));

			MenuIniciaSesion.Source = new MenuViewSource(menuItemsInicioSesion, this, false);
			Add(MenuIniciaSesion);
		}

		public override void DidReceiveMemoryWarning() {
			base.DidReceiveMemoryWarning();

		}

		public void showMenuLogion() {
			this.View.BringSubviewToFront(MenuIniciaSesion);
		}

		public void showMenuPagoExpress() {
			this.View.BringSubviewToFront(MenuPagoExpress);
		}
	}
}

