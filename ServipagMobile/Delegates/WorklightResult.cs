using Newtonsoft.Json.Linq;

namespace ServipagMobile
{
	public struct WorklightResult
	{
		public bool Success {get; set;}

		public string Message {get; set;}

		public JObject State { get; set;}

		public JObject Response{get; set;}
	}
}

