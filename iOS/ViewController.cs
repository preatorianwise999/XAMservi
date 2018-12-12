using System;
using Newtonsoft.Json.Linq;
using UIKit;

namespace ServipagMobile.iOS {
	public partial class ViewController : UIViewController {

		public ViewController(IntPtr handle) : base(handle) {
		}

		public override void ViewDidLoad() {
			base.ViewDidLoad();
		}

		public override void DidReceiveMemoryWarning() {
			base.DidReceiveMemoryWarning();
			// Release any cached data, images, etc that aren't in use.		
		}

		private async void OnUnprotectedInvoke() {
			JObject parametros = new JObject();

			//POST Code
			parametros.Add("username", "dhernandez@ewin.cl");
			parametros.Add("password", "12345678");
			parametros.Add("grant_type", "password");

			//var response = await MyClass.WorklightClient.UnprotectedInvokeAsync("Post", "/post", "POST", parametros);

			//JObject json = JObject.Parse(response.Response);
			//Label.Text = response.Response + response.Message + response.Success;
			//lblTest.Text = response.Response;//json.ToString();

			//GET Code
			//var response = await App.WorklightClient.UnprotectedInvokeAsync("Login", "/", "GET", null);

			//JObject json = JObject.Parse(response.Response);
			//lblTest.Text = json.ToString();
			//imgTest.Source = "https://s3.amazonaws.com/inspiringbenefits/archivos/images/" + json["result"][0]["image_file_name"];
		}
	}
}
