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
	// Token: 0x02000943 RID: 2371
	internal class YouTubeDataRequest : Disposable
	{
		// Token: 0x060048FA RID: 18682 RVA: 0x0011B7E7 File Offset: 0x001199E7
		public YouTubeDataRequest(Action<JObject, string> callback)
		{
			this._callback = callback;
		}

		// Token: 0x060048FB RID: 18683 RVA: 0x0011B7F8 File Offset: 0x001199F8
		public void GetVideo(string videoId)
		{
			bool requested = this._requested;
			if (requested)
			{
				throw new Exception("YouTubeRequest instance can not be re-used for another request!");
			}
			this._requested = true;
			this._searchQuery = videoId;
			NameValueCollection nameValueCollection = new NameValueCollection();
			nameValueCollection.Add("key", "AIzaSyAYEB4Pku4uWRAJvQxou3Pq09tNUKbxstc");
			nameValueCollection.Add("type", "video");
			nameValueCollection.Add("part", "snippet");
			nameValueCollection.Add("id", videoId);
			nameValueCollection.Add("maxResults", "1");
			NameValueCollection parameters = nameValueCollection;
			HttpUtils.RequestJson("https://www.googleapis.com/youtube/v3/videos?" + HttpUtils.BuildQueryString(parameters), new Action<JObject, Exception, HttpWebResponse>(this.OnSearchResult));
		}

		// Token: 0x060048FC RID: 18684 RVA: 0x0011B8A4 File Offset: 0x00119AA4
		public void SearchVideos(string query, int maxResults, string pageToken = null)
		{
			bool requested = this._requested;
			if (requested)
			{
				throw new Exception("YouTubeRequest instance can not be re-used for another request!");
			}
			this._requested = true;
			this._searchQuery = query;
			NameValueCollection nameValueCollection = new NameValueCollection();
			nameValueCollection.Add("key", "AIzaSyAYEB4Pku4uWRAJvQxou3Pq09tNUKbxstc");
			nameValueCollection.Add("type", "video");
			nameValueCollection.Add("part", "snippet");
			nameValueCollection.Add("q", query);
			nameValueCollection.Add("maxResults", maxResults.ToString());
			NameValueCollection nameValueCollection2 = nameValueCollection;
			bool flag = pageToken != null;
			if (flag)
			{
				nameValueCollection2["pageToken"] = pageToken;
			}
			HttpUtils.RequestJson("https://www.googleapis.com/youtube/v3/search?" + HttpUtils.BuildQueryString(nameValueCollection2), new Action<JObject, Exception, HttpWebResponse>(this.OnSearchResult));
		}

		// Token: 0x060048FD RID: 18685 RVA: 0x0011B968 File Offset: 0x00119B68
		private bool HandleErrors(JObject data, Exception ex, HttpWebResponse res)
		{
			bool flag = ex != null;
			bool result;
			if (flag)
			{
				YouTubeDataRequest.Logger.Error<Exception>(ex);
				this.RunCallback(null, "unreachable");
				result = true;
			}
			else
			{
				bool flag2 = data == null;
				if (flag2)
				{
					YouTubeDataRequest.Logger.Info<HttpStatusCode>("YouTube response is empty, status code is: {0}", res.StatusCode);
					result = true;
				}
				else
				{
					bool flag3 = data["error"] != null;
					if (flag3)
					{
						YouTubeDataRequest.Logger.Info("YouTube responded with an error: {0}", data.ToString());
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

		// Token: 0x060048FE RID: 18686 RVA: 0x0011BA00 File Offset: 0x00119C00
		private void OnSearchResult(JObject data, Exception ex, HttpWebResponse res)
		{
			bool flag = this.HandleErrors(data, ex, res);
			if (!flag)
			{
				JToken jtoken = data["nextPageToken"];
				this._nextPageToken = ((jtoken != null) ? Extensions.Value<string>(jtoken) : null);
				this._searchData = data["items"].ToObject<JArray>();
				NameValueCollection nameValueCollection = new NameValueCollection();
				nameValueCollection.Add("key", "AIzaSyAYEB4Pku4uWRAJvQxou3Pq09tNUKbxstc");
				nameValueCollection.Add("part", "id,contentDetails,statistics");
				nameValueCollection.Add("id", string.Join(",", Enumerable.ToArray<string>(Enumerable.Select<JToken, string>(this._searchData, (JToken key, int value) => (key["id"].Type == 8) ? Extensions.Value<string>(key["id"]) : Extensions.Value<string>(key["id"]["videoId"])))));
				NameValueCollection parameters = nameValueCollection;
				HttpUtils.RequestJson("https://www.googleapis.com/youtube/v3/videos?" + HttpUtils.BuildQueryString(parameters), new Action<JObject, Exception, HttpWebResponse>(this.OnSearchVideoResult));
			}
		}

		// Token: 0x060048FF RID: 18687 RVA: 0x0011BAE4 File Offset: 0x00119CE4
		private void OnSearchVideoResult(JObject data, Exception ex, HttpWebResponse res)
		{
			bool flag = this.HandleErrors(data, ex, res);
			if (!flag)
			{
				data["nextPageToken"] = this._nextPageToken;
				data["searchQuery"] = this._searchQuery;
				JArray jarray = data["items"].ToObject<JArray>();
				for (int i = 0; i < jarray.Count; i++)
				{
					JToken jtoken = jarray[i];
					foreach (JToken jtoken2 in this._searchData)
					{
						string a = (jtoken2["id"].Type == 8) ? Extensions.Value<string>(jtoken2["id"]) : Extensions.Value<string>(jtoken2["id"]["videoId"]);
						bool flag2 = a == Extensions.Value<string>(jtoken["id"]);
						if (flag2)
						{
							data["items"][i]["snippet"] = jtoken2["snippet"].ToObject<JObject>();
							break;
						}
					}
				}
				this.RunCallback(data, null);
			}
		}

		// Token: 0x06004900 RID: 18688 RVA: 0x0011BC54 File Offset: 0x00119E54
		public void OnGetPopularVideos(int maxResults)
		{
			bool requested = this._requested;
			if (requested)
			{
				throw new Exception("YouTubeRequest instance can not be re-used for another request!");
			}
			this._requested = true;
			NameValueCollection nameValueCollection = new NameValueCollection();
			nameValueCollection.Add("key", "AIzaSyAYEB4Pku4uWRAJvQxou3Pq09tNUKbxstc");
			nameValueCollection.Add("part", "id,contentDetails,statistics,snippet");
			nameValueCollection.Add("chart", "mostPopular");
			nameValueCollection.Add("maxResults", maxResults.ToString());
			NameValueCollection parameters = nameValueCollection;
			HttpUtils.RequestJson("https://www.googleapis.com/youtube/v3/videos?" + HttpUtils.BuildQueryString(parameters), delegate(JObject data, Exception ex, HttpWebResponse res)
			{
				bool flag = this.HandleErrors(data, ex, res);
				if (!flag)
				{
					this.RunCallback(data, null);
				}
			});
		}

		// Token: 0x06004901 RID: 18689 RVA: 0x0011BCEC File Offset: 0x00119EEC
		private void RunCallback(JObject data, string err)
		{
			bool disposed = base.Disposed;
			if (!disposed)
			{
				this._callback(data, err);
			}
		}

		// Token: 0x06004902 RID: 18690 RVA: 0x0011BD14 File Offset: 0x00119F14
		protected override void DoDispose()
		{
		}

		// Token: 0x04002501 RID: 9473
		private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

		// Token: 0x04002502 RID: 9474
		private const string ApiKey = "AIzaSyAYEB4Pku4uWRAJvQxou3Pq09tNUKbxstc";

		// Token: 0x04002503 RID: 9475
		private const string ApiHost = "https://www.googleapis.com/youtube/v3";

		// Token: 0x04002504 RID: 9476
		private bool _requested;

		// Token: 0x04002505 RID: 9477
		private string _searchQuery;

		// Token: 0x04002506 RID: 9478
		private string _nextPageToken;

		// Token: 0x04002507 RID: 9479
		private JArray _searchData;

		// Token: 0x04002508 RID: 9480
		private readonly Action<JObject, string> _callback;
	}
}
