using System;
using Foundation;
using SidebarNavigation;
using UIKit;

namespace ServipagMobile.iOS {
	public partial class RootViewController : UIViewController {
		private UIStoryboard _storyboard;
		public SidebarController SidebarController { get; private set; }
		public NavController NavController { get; private set; }
		public IntroController introController;
		public MenuController menuController;

		public override UIStoryboard Storyboard {
			get {
				if (_storyboard == null)
					_storyboard = UIStoryboard.FromName("Main", null);
				return _storyboard;
			}
		}

		public RootViewController() : base(null, null) {
		}

		public override void ViewDidLoad() {
			base.ViewDidLoad();
			var deviceDim = NSUserDefaults.StandardUserDefaults;
			var screenBounds = UIScreen.MainScreen.Bounds;

			deviceDim.SetFloat((float)screenBounds.Size.Width, "width");
			deviceDim.SetFloat((float)screenBounds.Size.Height, "height");

			introController = (IntroController)Storyboard.InstantiateViewController("IntroController");
			menuController = (MenuController)Storyboard.InstantiateViewController("MenuController");

			// create a slideout navigation controller with the top navigation controller and the menu view controller
			NavController = new NavController();
			NavController.PushViewController(introController, false);

			SidebarController = new SidebarController(this, NavController, menuController);
			SidebarController.MenuWidth = (int)(deviceDim.FloatForKey("width") / 1.5);
			SidebarController.MenuLocation = SidebarController.MenuLocations.Left;
			SidebarController.ReopenOnRotate = false;
		}

		public override void DidReceiveMemoryWarning() {
			base.DidReceiveMemoryWarning();
			// Release any cached data, images, etc that aren't in use.
		}
	}
}

