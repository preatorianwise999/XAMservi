using System;
using Android.App;
using Com.Browser2app.Khenshin;

namespace ServipagMobile.Droid {
	[Application]
	public class Servipag : Application, IKhenshinApplication {
		public static Servipag Current { get; set; }

		public IKhenshinInterface Khenshin { get; set; }

		public Servipag(IntPtr handle, global::Android.Runtime.JniHandleOwnership transfer)
        : base(handle, transfer)
        {

		}
		public override void OnCreate() {
            
			base.OnCreate();
			Current = this;
			Khenshin = new Khenshin.KhenshinBuilder()
						.SetApplication(this)
						.SetTaskAPIUrl("https://cmr.browser2app.com/api/automata/")
						.SetDumpAPIUrl("https://cmr.browser2app.com/api/automata/")
						.SetMainButtonStyle(1)
						.SetAutomatonTimeout(90)
						.SetAllowCredentialsSaving(true)
						.SetHideWebAddressInformationInForm(false)
						.Build();
		}
	}
}
