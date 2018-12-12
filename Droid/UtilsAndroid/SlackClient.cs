using System;
using Newtonsoft.Json;
using System.Collections.Specialized;
using System.Net;
using System.Text;

namespace ServipagMobile.iOS
{
		public class SlackClient
		{
			private  Uri _uri;
			private readonly Encoding _encoding = new UTF8Encoding();

			public void SetUri(string urlWithAccessToken)
			{
				_uri = new Uri(urlWithAccessToken);
			}

			public SlackClient()
			{
			
			}

			private static SlackClient instance = null;
			public static SlackClient Instance
			{
				get
				{
					if (instance == null)
						instance = new SlackClient();

					return instance;
				}
			}

			/// <summary>
			/// Posts the message.
			/// </summary>
			/// <param name="text">Text.</param>
			/// <param name="username">Username.</param>
			/// <param name="channel">Channel.</param>
			public void PostMessage(string text, string username = null, string channel = null)
			{
				SlackPayload payload = new SlackPayload()
				{
					Channel = channel,
					Username = username,
					Text = text
				};

				PostMessage(payload);
			}

			/// <summary>
			/// Posts the message.
			/// </summary>
			/// <param name="payload">Payload.</param>
			public void PostMessage(SlackPayload payload)
			{
				string payloadJson = JsonConvert.SerializeObject(payload);

				using (WebClient client = new WebClient())
				{
					NameValueCollection data = new NameValueCollection();
					data["payload"] = payloadJson;
					var response = client.UploadValues(_uri, "POST", data);
					//string responseText = _encoding.GetString(response);
				}
			}
		}
}

