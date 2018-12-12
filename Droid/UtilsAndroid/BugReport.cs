using System;
namespace ServipagMobile.iOS
{
	public static class BugReport
	{
		private static string hookError = Properties.GetInstance().slackHookError;
		private static string hookQa = Properties.GetInstance().slackHookDroid_QA;
		private static bool showHookError = Convert.ToBoolean(Properties.GetInstance().showErrors);
		private static bool showHookQa = Convert.ToBoolean(Properties.GetInstance().qaDroid);


		private static SlackClient client = SlackClient.Instance;


		/// <summary>
		/// Sends the bug.
		/// </summary>
		/// <param name="message">Message.</param>
		/// <param name="name">Name.</param>
		/// <param name="channel">Channel.</param>
		/// <param name="typeLog">Type log.</param>
		public static void sendBug(string message,int typeLog){
			string url = "";
			bool sendMessage = false;
			switch(typeLog){
				case 0: //errores
					sendMessage = showHookError;
					if (sendMessage) {
						url = hookError;
					}
					break;
				case 1: //QA
					sendMessage = showHookQa;
					if (sendMessage)
					{
						url = hookQa;
					}
				break;
			}
			if (sendMessage)
			{
				client.SetUri(url);
				client.PostMessage(message, null, null);
			}
		}

	}
}
