using System;
using Android.Util;
using Android.Webkit;

namespace ServipagMobile.Droid {
	public class WvClient : WebViewClient {
		private Action callbackLastPage;

		public WvClient(Action callbackLastPage) {
			this.callbackLastPage = callbackLastPage;
		}

		public override void OnReceivedSslError(WebView view, SslErrorHandler handler, Android.Net.Http.SslError error) {
			handler.Proceed();
		}

		public override void OnPageStarted(WebView view, string url, Android.Graphics.Bitmap favicon) {
			base.OnPageStarted(view, url, favicon);

			Log.Debug("URL Loading", url);

			if (url.Contains("IDTRX")) {
				callbackLastPage();
			}
		}
	}
}
