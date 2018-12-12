using System;
using Android.Webkit;

namespace ServipagMobile.Droid {
	public class WvClientTC : WebViewClient{
		public override bool ShouldOverrideUrlLoading(WebView view, IWebResourceRequest request) {
			return false;
		}
	}
}
