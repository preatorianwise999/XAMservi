using System;
using Worklight;
using Newtonsoft.Json.Linq;
using System.Threading.Tasks;
using System.Text;
using Worklight.Push;

namespace ServipagMobile {
	public class ServiceDelegate {
		public IWorklightClient client { get; private set; }
		public IWorklightPush push { get; private set; }
		private string appRealm = "UserLogin";



		public ServiceDelegate(IWorklightClient wlc, IWorklightPush push) {
			this.client = wlc;
			this.push = push;

			SecurityCheckChallengeHandler customCH = new CustomChallengeHandler(appRealm);
			client.RegisterChallengeHandler(customCH);
			//push.Initialize();
		}

		public async Task<WorklightResult> UnprotectedInvokeAsync(string adapter, string method, string verb,
		                                                          JObject parameters) {
			var result = new WorklightResult();

			try {
				WorklightResourceRequest rr;
				WorklightResponse resp;
				StringBuilder uriBuilder = new StringBuilder()
					.Append(client.ServerUrl.AbsoluteUri) // Get the server URL
					.Append("/adapters")
					.Append("/" + adapter) //Name of the adapter
					.Append("/" + method);    // Name of the adapter procedure

				rr = client.ResourceRequest(new Uri(uriBuilder.ToString()), verb, 60000);

				if (verb == "GET") {
					resp = await rr.Send();
				} else {
					resp = await rr.Send(parameters);
				}

				result.Success = resp.Success;
				result.Message = (resp.Success) ? "Connected" : resp.Message;

				if (result.Success) {
					if (adapter.Equals("servipagProperties") || adapter.Equals("busquedaServicioBiller")) {
						result.Response = resp.ResponseJSON;
					} else {
						result.State = (JObject)resp.ResponseJSON[method + "Result"]["<header>k__BackingField"];
						result.Response = (JObject)resp.ResponseJSON[method + "Result"]["<body>k__BackingField"];
					}
				}

			} catch (Exception ex) {
				result.Success = false;
				result.Message = ex.Message;
			}

			return result;
		}

		public async Task<WorklightResult> RegisterAsync() {
			var result = new WorklightResult();

			try {
				var resp = await push.RegisterDevice(new JObject());
				result.Success = resp.Success;
				result.Message = "Registered";
				result.Response = (JObject)((resp.ResponseJSON != null) ? resp.ResponseJSON.ToString() : "");
			} catch (Exception ex) {
				result.Success = false;
				result.Message = ex.Message;
			}

			return result;
		}

		public async Task<WorklightResult> SubscribeAsync() {
			var result = new WorklightResult();

			try {
				var resp = await push.Subscribe(new string[] { "Xamarin" });
				result.Success = resp.Success;
				result.Message = "Subscribed";
				result.Response = (JObject)((resp.ResponseJSON != null) ? resp.ResponseJSON.ToString() : "");
			} catch (Exception ex) {
				result.Success = false;
				result.Message = ex.Message;
			}

			return result;
		}
	}
}
