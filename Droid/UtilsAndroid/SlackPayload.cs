using System;
using Newtonsoft.Json;

namespace ServipagMobile.iOS
{
	public class SlackPayload
	{
		[JsonProperty("channel")]
		public string Channel { get; set; }

		[JsonProperty("username")]
		public string Username { get; set; }

		[JsonProperty("text")]
		public string Text { get; set; }
	}
}