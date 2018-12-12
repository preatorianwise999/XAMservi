using System;
using System.Collections.Generic;
using DK.Ostebaronen.Touch.SGTabbedPager;
using SidebarNavigation;
using UIKit;

namespace ServipagMobile.iOS {
	public partial class BaseController : UIViewController {
		protected SidebarController SidebarController {
			get {
				return (UIApplication.SharedApplication.Delegate as AppDelegate).RootViewController.SidebarController;
			}
		}

		protected NavController NavController {
			get {
				return (UIApplication.SharedApplication.Delegate as AppDelegate).RootViewController.NavController;
			}
		}

		public override UIStoryboard Storyboard {
			get {
				return (UIApplication.SharedApplication.Delegate as AppDelegate).RootViewController.Storyboard;
			}
		}

		public BaseController(IntPtr handle) : base(handle) {
		}

		public override void ViewDidLoad() {
			base.ViewDidLoad();
			NavigationItem.SetLeftBarButtonItem(
				new UIBarButtonItem(UIImage.FromBundle("threelines")
					, UIBarButtonItemStyle.Plain
					, (sender, args) => {
						SidebarController.ToggleMenu();
					}), true);
		}

		public override void DidReceiveMemoryWarning() {
			base.DidReceiveMemoryWarning();
			// Release any cached data, images, etc that aren't in use.
		}
	}
}

