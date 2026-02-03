using System;
using System.Collections.Specialized;
using System.Linq;
using System.Net;
using HytaleClient.Core;
using HytaleClient.Utils;
using Newtonsoft.Json.Linq;
using NLog;

namespace HytaleClient.InGame.Modules.ImmersiveScreen.Data
{
	// Token: 0x02000942 RID: 2370
	internal class TwitchDataRequest : Disposable
	{
		// Token: 0x060048F3 RID: 18675 RVA: 0x0011B599 File Offset: 0x00119799
		public TwitchDataRequest(Action<JObject, string> callback)
		{
			this._callback = callback;
		}

		// Token: 0x060048F4 RID: 18676 RVA: 0x0011B5AC File Offset: 0x001197AC
		private bool HandleErrors(JObject data, Exception ex, HttpWebResponse res)
		{
			bool flag = ex != null;
			bool result;
			if (flag)
			{
				TwitchDataRequest.Logger.Error<Exception>(ex);
				this.RunCallback(null, "unreachable");
				result = true;
			}
			else
			{
				bool flag2 = data == null;
				if (flag2)
				{
					TwitchDataRequest.Logger.Info<HttpStatusCode>("Twitch response is empty, status code: {0}", res.StatusCode);
					this.RunCallback(null, "unreachable");
					result = true;
				}
				else
				{
					bool flag3 = data["error"] != null;
					if (flag3)
					{
						TwitchDataRequest.Logger.Info("Twitch responded with an error: {0}", data.ToString());
						this.RunCallback(null, "requestFailed");
						result = true;
					}
					else
					{
						result = false;
					}
				}
			}
			return result;
		}

		// Token: 0x060048F5 RID: 18677 RVA: 0x0011B650 File Offset: 0x00119850
		public void GetPopularStreams(int maxResults)
		{
			TwitchDataRequest.<>c__DisplayClass8_0 CS$<>8__locals1 = new TwitchDataRequest.<>c__DisplayClass8_0();
			CS$<>8__locals1.<>4__this = this;
			bool requested = this._requested;
			if (requested)
			{
				throw new Exception("TwitchDataRequest instance can not be re-used for another request!");
			}
			this._requested = true;
			TwitchDataRequest.<>c__DisplayClass8_0 CS$<>8__locals2 = CS$<>8__locals1;
			WebHeaderCollection webHeaderCollection = new WebHeaderCollection();
			webHeaderCollection.Add("Client-ID: f9lmcos9wdqjyzqz9jy6bs672msxgk6");
			CS$<>8__locals2.headers = webHeaderCollection;
			CS$<>8__locals1.req = HttpUtils.CreateRequest("https://api.twitch.tv/helix/streams", "GET");
			CS$<>8__locals1.req.Headers = CS$<>8__locals1.headers;
			HttpUtils.RequestJson(CS$<>8__locals1.req, null, delegate(JObject data, Exception ex, HttpWebResponse res)
			{
				bool flag = CS$<>8__locals1.<>4__this.HandleErrors(data, ex, res);
				if (!flag)
				{
					JArray streams = Extensions.Value<JArray>(data["data"]);
					string[] value = Enumerable.ToArray<string>(Enumerable.Distinct<string>(Enumerable.Select<JToken, string>(streams, (JToken stream) => "id=" + Extensions.Value<string>(stream["game_id"]))));
					CS$<>8__locals1.req = HttpUtils.CreateRequest("https://api.twitch.tv/helix/games?" + string.Join("&", value), "GET");
					CS$<>8__locals1.req.Headers = CS$<>8__locals1.headers;
					HttpUtils.RequestJson(CS$<>8__locals1.req, null, delegate(JObject data2, Exception ex2, HttpWebResponse res2)
					{
						bool flag2 = CS$<>8__locals1.<>4__this.HandleErrors(data2, ex2, res2);
						if (!flag2)
						{
							JArray jarray = Extensions.Value<JArray>(data2["data"]);
							foreach (JToken jtoken in streams)
							{
								bool flag3 = jtoken["game_id"] == null;
								if (!flag3)
								{
									foreach (JToken jtoken2 in jarray)
									{
										bool flag4 = Extensions.Value<string>(jtoken2["id"]) == Extensions.Value<string>(jtoken["game_id"]);
										if (flag4)
										{
											jtoken["game_title"] = jtoken2["name"];
											break;
										}
									}
								}
							}
							CS$<>8__locals1.<>4__this.RunCallback(data, null);
						}
					});
				}
			});
		}

		// Token: 0x060048F6 RID: 18678 RVA: 0x0011B6E0 File Offset: 0x001198E0
		public void SearchStreams(string query, int maxResults, int page)
		{
			bool requested = this._requested;
			if (requested)
			{
				throw new Exception("TwitchDataRequest instance can not be re-used for another request!");
			}
			this._requested = true;
			NameValueCollection nameValueCollection = new NameValueCollection();
			nameValueCollection.Add("client_id", "f9lmcos9wdqjyzqz9jy6bs672msxgk6");
			nameValueCollection.Add("limit", maxResults.ToString());
			nameValueCollection.Add("offset", (page * maxResults).ToString());
			nameValueCollection.Add("query", query);
			NameValueCollection parameters = nameValueCollection;
			HttpWebRequest httpWebRequest = HttpUtils.CreateRequest("https://api.twitch.tv/kraken/search/streams?" + HttpUtils.BuildQueryString(parameters), "GET");
			httpWebRequest.Accept = "application/vnd.twitchtv.v5+json";
			HttpUtils.RequestJson(httpWebRequest, null, delegate(JObject data, Exception ex, HttpWebResponse res)
			{
				bool flag = this.HandleErrors(data, ex, res);
				if (!flag)
				{
					data["searchQuery"] = query;
					this.RunCallback(data, null);
				}
			});
		}

		// Token: 0x060048F7 RID: 18679 RVA: 0x0011B7B0 File Offset: 0x001199B0
		private void RunCallback(JObject data, string err)
		{
			bool disposed = base.Disposed;
			if (!disposed)
			{
				this._callback(data, err);
			}
		}

		// Token: 0x060048F8 RID: 18680 RVA: 0x0011B7D8 File Offset: 0x001199D8
		protected override void DoDispose()
		{
		}

		// Token: 0x040024FB RID: 9467
		private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

		// Token: 0x040024FC RID: 9468
		private const string ApiClientId = "f9lmcos9wdqjyzqz9jy6bs672msxgk6";

		// Token: 0x040024FD RID: 9469
		private const string ApiHost = "https://api.twitch.tv/helix";

		// Token: 0x040024FE RID: 9470
		private const string ApiHostV5 = "https://api.twitch.tv/kraken";

		// Token: 0x040024FF RID: 9471
		private bool _requested;

		// Token: 0x04002500 RID: 9472
		private readonly Action<JObject, string> _callback;
	}
}
