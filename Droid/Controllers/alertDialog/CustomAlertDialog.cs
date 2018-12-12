using System;
using Android.App;
using Android.Support.V7.View;

namespace ServipagMobile.Droid {
	public class CustomAlertDialog {
		private AlertDialog.Builder alert;

		public CustomAlertDialog(Activity context, string title, string message, string ttlBttnOne,
		                         string ttlBttnTwo, Action callbackBttnOne, Action callbackBttnTwo) {
			alert = new AlertDialog.Builder(new ContextThemeWrapper(context, Resource.Style.AlertDialogCustom));
			alert.SetTitle(title);
			alert.SetMessage(message);

			if (ttlBttnOne != "") {
				alert.SetPositiveButton(ttlBttnOne, (senderAlert, args) => {
					if (callbackBttnOne != null) {
						callbackBttnOne();
					}
				});
			}
			if (ttlBttnTwo != "") {
				alert.SetNegativeButton(ttlBttnTwo, (senderAlert, args) => {
					if (callbackBttnTwo != null) {
						callbackBttnTwo();
					}
				});
			}
		}

		public void showDialog() {
			alert.Show();
		}
	}
}
