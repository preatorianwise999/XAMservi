using System.Timers;
using Android.Util;

namespace ServipagMobile.Droid {
	public class TimerPayment {
		private Timer timer;
		private int countSecs;
		private PagoActivity context;

		public TimerPayment(PagoActivity context) {
			this.context = context;
			timer = new Timer();
			timer.Interval = 1000;
			timer.Elapsed += OnTimedEvent;
			countSecs = 10;
			timer.Enabled = true;
		}

		private void OnTimedEvent(object sender, ElapsedEventArgs e) {
			countSecs--;

			if (countSecs <= 0) {
				timer.Stop();
				CustomAlertDialog alert = new CustomAlertDialog(context, "¿Estas ahí?",
				                                                "Confirma si aún deseas realizar el pago por favor.",
				                                                "Si, lo haré",
				                                                "No, gracias.",reRunTimer, null);
				alert.showDialog();
				Log.Debug("ASDASD", "ASDASDASDASD");
			}
		}

		private void reRunTimer() {
			timer = new Timer();
			timer.Interval = 1000;
			timer.Elapsed += OnTimedEvent;
			countSecs = 10;
			timer.Enabled = true;
		}
	}
}
