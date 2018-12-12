using Android.OS;
using Android.Support.V4.App;
using Android.Views;
using Android.Webkit;

namespace ServipagMobile.Droid {
	public class FragmentTerminosCondiciones : Fragment {
		private WebView webViewTyC;

		public override void OnCreate(Bundle savedInstanceState) {
			base.OnCreate(savedInstanceState);
		}

		public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState) {
			return inflater.Inflate(Resource.Layout.FragmentTerminosCondiciones, container, false);
		}

		public override void OnViewCreated(View view, Bundle savedInstanceState) {
			base.OnViewCreated(view, savedInstanceState);

			webViewTyC = view.FindViewById<WebView>(Resource.Id.webViewTyC);
			WebSettings settings = webViewTyC.Settings;
			settings.JavaScriptEnabled = true;
			settings.SetAppCacheEnabled(true);
			settings.SetPluginState(WebSettings.PluginState.On);

			webViewTyC.SetWebViewClient(new WvClientTC());
			webViewTyC.Settings.LoadWithOverviewMode = true;
			webViewTyC.Settings.UseWideViewPort = true;
			webViewTyC.Settings.BuiltInZoomControls = true;
			webViewTyC.Settings.DisplayZoomControls = false;
			webViewTyC.LoadUrl("http://docs.google.com/viewer?url=https://www.servipag.com/wps/wcm/connect/672ceace-77f2-4d66-8ad6-536b5d6257a0/términos_recarga.pdf?MOD=AJPERES");
		}
	}
}
