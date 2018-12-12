using System;
using System.Drawing;
using Foundation;
using UIKit;

namespace ServipagMobile.iOS {
	public partial class IntroController : BaseController {
		private NSUserDefaults deviceDim;
		public IntroController(IntPtr handle) : base(handle) {
			
		}

		public override void ViewDidLoad() {
			base.ViewDidLoad();
			deviceDim = NSUserDefaults.StandardUserDefaults;
			deviceDim.SetFloat((float)NavigationController.NavigationBar.Bounds.Height, "heightNB");

			setValues();
			this.View.BringSubviewToFront(FramePagoExpress);

			SegmentControl.ValueChanged += (sender, e) => {
				if (((UISegmentControl)sender).SelectedSegment == 0) {
					showPagoExpress();
				} else {
					showInicioSesion();
				}
			};

			ButtonLogin.TouchUpInside += delegate {
				AppDelegate asd = UIApplication.SharedApplication.Delegate as AppDelegate;
				asd.RootViewController.menuController.showMenuLogion();
			};
		}

		private void setValues() {
			SegmentControl.Frame = new RectangleF(
				0, deviceDim.FloatForKey("heightNB"), deviceDim.FloatForKey("width"), 70);
			SegmentControl.BackgroundColor = UIColor.FromRGB(48, 71, 91);
			SegmentControl.TintColor = UIColor.FromRGB(70, 100, 126);
			SegmentControl.SetTitleTextAttributes(new UITextAttributes {
				TextColor = UIColor.FromRGB(255, 255, 255),
				Font = UIFont.BoldSystemFontOfSize(16f)
			}, UIControlState.Selected);
			SegmentControl.SetTitleTextAttributes(new UITextAttributes {
				Font = UIFont.BoldSystemFontOfSize(16f)
			}, UIControlState.Normal);

			FramePagoExpress.Frame = new RectangleF(
				0, deviceDim.FloatForKey("heightNB") + 70, deviceDim.FloatForKey("width"),
				deviceDim.FloatForKey("height") - 70);
			FramePagoExpress.BackgroundColor = UIColor.FromRGB(35, 54, 69);

			FrameInicioSesion.Frame = new RectangleF(
				0, deviceDim.FloatForKey("heightNB")+ 70, deviceDim.FloatForKey("width"),
				deviceDim.FloatForKey("height") - 70);
			FrameInicioSesion.BackgroundColor = UIColor.FromRGB(35, 54, 69);

			//FramePagoExpress
			TitlePagoExpress.Frame = new RectangleF(
				0, 70, deviceDim.FloatForKey("width"), 50);
			ButtonPagoExpress.Frame = new RectangleF(
				40, (float)TitlePagoExpress.Bounds.Height + 100, deviceDim.FloatForKey("width") - 80, 50);
			ButtonPagoExpress.BackgroundColor = UIColor.FromRGB(253,214,73);
			ButtonPagoExpress.TintColor = UIColor.FromRGB(35, 54, 69);

			//FrameInicioSesion
			FieldUser.Frame = new RectangleF(
				40, 70, deviceDim.FloatForKey("width") - 80, 50);
			FieldUser.LeftViewMode = UITextFieldViewMode.Always;
			FieldUser.LeftView = new UIImageView(UIImage.FromBundle("user"));

			FieldPassword.Frame = new RectangleF(
				40, (float)FieldUser.Bounds.Height + 80, deviceDim.FloatForKey("width") - 80, 50);
			FieldPassword.LeftViewMode = UITextFieldViewMode.Always;
			FieldPassword.LeftView = new UIImageView(UIImage.FromBundle("password"));

			ButtonForgotPass.Frame = new RectangleF(
				40, (float)FieldUser.Bounds.Height + (float)FieldPassword.Bounds.Height + 110,
				deviceDim.FloatForKey("width") - 80, 30);
			ButtonLogin.Frame = new RectangleF(
				40, (float) FieldUser.Bounds.Height + (float)FieldPassword.Bounds.Height +
				(float)ButtonForgotPass.Bounds.Height + 120, deviceDim.FloatForKey("width") - 80, 50);
			ButtonLogin.BackgroundColor = UIColor.FromRGB(253, 214, 73);
			ButtonLogin.TintColor = UIColor.FromRGB(35, 54, 69);

			ButtonSignUp.Frame = new RectangleF(40, (float)FieldUser.Bounds.Height +
			                                    (float)FieldPassword.Bounds.Height +
			                                    (float)ButtonForgotPass.Bounds.Height +
			                                    (float) ButtonLogin.Bounds.Height + 130,
			                                    deviceDim.FloatForKey("width") - 80, 50);
			ButtonSignUp.BackgroundColor = UIColor.FromRGB(0, 158, 184);
			ButtonSignUp.TintColor = UIColor.FromRGB(35, 54, 69);
		}

		private void showPagoExpress() {
			this.View.BringSubviewToFront(FramePagoExpress);
		}

		private void showInicioSesion() {
			this.View.BringSubviewToFront(FrameInicioSesion);
		}
	}
}

