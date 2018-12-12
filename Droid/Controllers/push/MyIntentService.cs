using System;
using Android.App;
using Android.Content;
//using Android.Gms.Gcm;
using Android.OS;
using Android.Util;

namespace ServipagMobile.Droid {
	[Service(Exported = false), IntentFilter(new[] { "com.google.android.c2dm.intent.RECEIVE" })]
	public class MyIntentService /*: GcmListenerService*/{
		/*public override void OnMessageReceived(string from, Bundle data) {
			Log.Debug("MyGcmListenerService", "From:    " + from);
			Log.Debug("MyGcmListenerService", "Message: " + data.GetString("alert"));
			SendNotification(data.GetString("alert"));
		}

		void SendNotification(string message) {
			var intent = new Intent(this, typeof(MainActivity));
			intent.PutExtra("pushRecive", true);
			intent.AddFlags(ActivityFlags.ClearTop);
			var pendingIntent = PendingIntent.GetActivity(this, 0, intent, PendingIntentFlags.OneShot);

			var notificationBuilder = new Notification.Builder(this)
			                                          .SetSmallIcon(Resource.Mipmap.Icon)
				.SetContentTitle("Servipag")
				.SetContentText(message)
				.SetAutoCancel(true)
				.SetContentIntent(pendingIntent);

			var notificationManager = (NotificationManager)GetSystemService(Context.NotificationService);
			notificationManager.Notify(0, notificationBuilder.Build());
		}*/
	}
}
