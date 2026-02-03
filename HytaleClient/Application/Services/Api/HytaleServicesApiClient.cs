using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace HytaleClient.Application.Services.Api
{
	// Token: 0x02000BFF RID: 3071
	internal class HytaleServicesApiClient
	{
		// Token: 0x060061F9 RID: 25081 RVA: 0x00204A23 File Offset: 0x00202C23
		public HytaleServicesApiClient()
		{
			this._client = new HttpClient();
			this._client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Static", "7531cca5-d568-486f-91e9-0d8aad17d249");
		}

		// Token: 0x060061FA RID: 25082 RVA: 0x00204A58 File Offset: 0x00202C58
		[DebuggerStepThrough]
		public Task<Server[]> FetchPublicServers(HytaleServicesApiClient.FetchPublicServerQuery query, int size = 50, HytaleServicesApiClient.SortOrder sortOrder = HytaleServicesApiClient.SortOrder.Ascending, HytaleServicesApiClient.ServerSortField sortField = HytaleServicesApiClient.ServerSortField.Name)
		{
			HytaleServicesApiClient.<FetchPublicServers>d__6 <FetchPublicServers>d__ = new HytaleServicesApiClient.<FetchPublicServers>d__6();
			<FetchPublicServers>d__.<>t__builder = AsyncTaskMethodBuilder<Server[]>.Create();
			<FetchPublicServers>d__.<>4__this = this;
			<FetchPublicServers>d__.query = query;
			<FetchPublicServers>d__.size = size;
			<FetchPublicServers>d__.sortOrder = sortOrder;
			<FetchPublicServers>d__.sortField = sortField;
			<FetchPublicServers>d__.<>1__state = -1;
			<FetchPublicServers>d__.<>t__builder.Start<HytaleServicesApiClient.<FetchPublicServers>d__6>(ref <FetchPublicServers>d__);
			return <FetchPublicServers>d__.<>t__builder.Task;
		}

		// Token: 0x060061FB RID: 25083 RVA: 0x00204ABC File Offset: 0x00202CBC
		[DebuggerStepThrough]
		private Task<Server[]> FetchUserServers(Guid userUuid, string listType)
		{
			HytaleServicesApiClient.<FetchUserServers>d__7 <FetchUserServers>d__ = new HytaleServicesApiClient.<FetchUserServers>d__7();
			<FetchUserServers>d__.<>t__builder = AsyncTaskMethodBuilder<Server[]>.Create();
			<FetchUserServers>d__.<>4__this = this;
			<FetchUserServers>d__.userUuid = userUuid;
			<FetchUserServers>d__.listType = listType;
			<FetchUserServers>d__.<>1__state = -1;
			<FetchUserServers>d__.<>t__builder.Start<HytaleServicesApiClient.<FetchUserServers>d__7>(ref <FetchUserServers>d__);
			return <FetchUserServers>d__.<>t__builder.Task;
		}

		// Token: 0x060061FC RID: 25084 RVA: 0x00204B10 File Offset: 0x00202D10
		[DebuggerStepThrough]
		public Task<Server[]> FetchFavoriteServers(Guid userUuid)
		{
			HytaleServicesApiClient.<FetchFavoriteServers>d__8 <FetchFavoriteServers>d__ = new HytaleServicesApiClient.<FetchFavoriteServers>d__8();
			<FetchFavoriteServers>d__.<>t__builder = AsyncTaskMethodBuilder<Server[]>.Create();
			<FetchFavoriteServers>d__.<>4__this = this;
			<FetchFavoriteServers>d__.userUuid = userUuid;
			<FetchFavoriteServers>d__.<>1__state = -1;
			<FetchFavoriteServers>d__.<>t__builder.Start<HytaleServicesApiClient.<FetchFavoriteServers>d__8>(ref <FetchFavoriteServers>d__);
			return <FetchFavoriteServers>d__.<>t__builder.Task;
		}

		// Token: 0x060061FD RID: 25085 RVA: 0x00204B5C File Offset: 0x00202D5C
		[DebuggerStepThrough]
		public Task<Server[]> FetchRecentServers(Guid userUuid)
		{
			HytaleServicesApiClient.<FetchRecentServers>d__9 <FetchRecentServers>d__ = new HytaleServicesApiClient.<FetchRecentServers>d__9();
			<FetchRecentServers>d__.<>t__builder = AsyncTaskMethodBuilder<Server[]>.Create();
			<FetchRecentServers>d__.<>4__this = this;
			<FetchRecentServers>d__.userUuid = userUuid;
			<FetchRecentServers>d__.<>1__state = -1;
			<FetchRecentServers>d__.<>t__builder.Start<HytaleServicesApiClient.<FetchRecentServers>d__9>(ref <FetchRecentServers>d__);
			return <FetchRecentServers>d__.<>t__builder.Task;
		}

		// Token: 0x060061FE RID: 25086 RVA: 0x00204BA8 File Offset: 0x00202DA8
		[DebuggerStepThrough]
		public Task AddServerToRecents(Guid serverUuid, Guid userUuid)
		{
			HytaleServicesApiClient.<AddServerToRecents>d__10 <AddServerToRecents>d__ = new HytaleServicesApiClient.<AddServerToRecents>d__10();
			<AddServerToRecents>d__.<>t__builder = AsyncTaskMethodBuilder.Create();
			<AddServerToRecents>d__.<>4__this = this;
			<AddServerToRecents>d__.serverUuid = serverUuid;
			<AddServerToRecents>d__.userUuid = userUuid;
			<AddServerToRecents>d__.<>1__state = -1;
			<AddServerToRecents>d__.<>t__builder.Start<HytaleServicesApiClient.<AddServerToRecents>d__10>(ref <AddServerToRecents>d__);
			return <AddServerToRecents>d__.<>t__builder.Task;
		}

		// Token: 0x060061FF RID: 25087 RVA: 0x00204BFC File Offset: 0x00202DFC
		[DebuggerStepThrough]
		public Task AddServerToFavorites(Guid serverUuid, Guid userUuid)
		{
			HytaleServicesApiClient.<AddServerToFavorites>d__11 <AddServerToFavorites>d__ = new HytaleServicesApiClient.<AddServerToFavorites>d__11();
			<AddServerToFavorites>d__.<>t__builder = AsyncTaskMethodBuilder.Create();
			<AddServerToFavorites>d__.<>4__this = this;
			<AddServerToFavorites>d__.serverUuid = serverUuid;
			<AddServerToFavorites>d__.userUuid = userUuid;
			<AddServerToFavorites>d__.<>1__state = -1;
			<AddServerToFavorites>d__.<>t__builder.Start<HytaleServicesApiClient.<AddServerToFavorites>d__11>(ref <AddServerToFavorites>d__);
			return <AddServerToFavorites>d__.<>t__builder.Task;
		}

		// Token: 0x06006200 RID: 25088 RVA: 0x00204C50 File Offset: 0x00202E50
		[DebuggerStepThrough]
		public Task RemoveServerFromFavorites(Guid serverUuid, Guid userUuid)
		{
			HytaleServicesApiClient.<RemoveServerFromFavorites>d__12 <RemoveServerFromFavorites>d__ = new HytaleServicesApiClient.<RemoveServerFromFavorites>d__12();
			<RemoveServerFromFavorites>d__.<>t__builder = AsyncTaskMethodBuilder.Create();
			<RemoveServerFromFavorites>d__.<>4__this = this;
			<RemoveServerFromFavorites>d__.serverUuid = serverUuid;
			<RemoveServerFromFavorites>d__.userUuid = userUuid;
			<RemoveServerFromFavorites>d__.<>1__state = -1;
			<RemoveServerFromFavorites>d__.<>t__builder.Start<HytaleServicesApiClient.<RemoveServerFromFavorites>d__12>(ref <RemoveServerFromFavorites>d__);
			return <RemoveServerFromFavorites>d__.<>t__builder.Task;
		}

		// Token: 0x06006201 RID: 25089 RVA: 0x00204CA4 File Offset: 0x00202EA4
		[DebuggerStepThrough]
		public Task RemoveServerFromRecents(Guid serverUuid, Guid userUuid)
		{
			HytaleServicesApiClient.<RemoveServerFromRecents>d__13 <RemoveServerFromRecents>d__ = new HytaleServicesApiClient.<RemoveServerFromRecents>d__13();
			<RemoveServerFromRecents>d__.<>t__builder = AsyncTaskMethodBuilder.Create();
			<RemoveServerFromRecents>d__.<>4__this = this;
			<RemoveServerFromRecents>d__.serverUuid = serverUuid;
			<RemoveServerFromRecents>d__.userUuid = userUuid;
			<RemoveServerFromRecents>d__.<>1__state = -1;
			<RemoveServerFromRecents>d__.<>t__builder.Start<HytaleServicesApiClient.<RemoveServerFromRecents>d__13>(ref <RemoveServerFromRecents>d__);
			return <RemoveServerFromRecents>d__.<>t__builder.Task;
		}

		// Token: 0x06006202 RID: 25090 RVA: 0x00204CF8 File Offset: 0x00202EF8
		[DebuggerStepThrough]
		private Task UpdateUserListServer(Guid serverUuid, Guid userUuid, string list, HttpMethod method)
		{
			HytaleServicesApiClient.<UpdateUserListServer>d__14 <UpdateUserListServer>d__ = new HytaleServicesApiClient.<UpdateUserListServer>d__14();
			<UpdateUserListServer>d__.<>t__builder = AsyncTaskMethodBuilder.Create();
			<UpdateUserListServer>d__.<>4__this = this;
			<UpdateUserListServer>d__.serverUuid = serverUuid;
			<UpdateUserListServer>d__.userUuid = userUuid;
			<UpdateUserListServer>d__.list = list;
			<UpdateUserListServer>d__.method = method;
			<UpdateUserListServer>d__.<>1__state = -1;
			<UpdateUserListServer>d__.<>t__builder.Start<HytaleServicesApiClient.<UpdateUserListServer>d__14>(ref <UpdateUserListServer>d__);
			return <UpdateUserListServer>d__.<>t__builder.Task;
		}

		// Token: 0x06006203 RID: 25091 RVA: 0x00204D5C File Offset: 0x00202F5C
		[DebuggerStepThrough]
		public Task<Server> FetchServerDetails(Guid serverUuid)
		{
			HytaleServicesApiClient.<FetchServerDetails>d__15 <FetchServerDetails>d__ = new HytaleServicesApiClient.<FetchServerDetails>d__15();
			<FetchServerDetails>d__.<>t__builder = AsyncTaskMethodBuilder<Server>.Create();
			<FetchServerDetails>d__.<>4__this = this;
			<FetchServerDetails>d__.serverUuid = serverUuid;
			<FetchServerDetails>d__.<>1__state = -1;
			<FetchServerDetails>d__.<>t__builder.Start<HytaleServicesApiClient.<FetchServerDetails>d__15>(ref <FetchServerDetails>d__);
			return <FetchServerDetails>d__.<>t__builder.Task;
		}

		// Token: 0x06006204 RID: 25092 RVA: 0x00204DA8 File Offset: 0x00202FA8
		private static JObject DeserializeFromStream(Stream stream)
		{
			JObject result;
			using (StreamReader streamReader = new StreamReader(stream))
			{
				using (JsonTextReader jsonTextReader = new JsonTextReader(streamReader))
				{
					result = (JObject)JToken.ReadFrom(jsonTextReader);
				}
			}
			return result;
		}

		// Token: 0x06006205 RID: 25093 RVA: 0x00204E08 File Offset: 0x00203008
		private static Server CreateServerObjectFromJson(JObject serverJson)
		{
			Server server = new Server
			{
				UUID = Guid.Parse((string)serverJson["serverUuid"]["hexString"]),
				Name = (string)serverJson["name"],
				Description = (string)serverJson["description"],
				OnlinePlayers = (int)serverJson["playerCount"],
				MaxPlayers = (int)serverJson["slots"],
				Host = (string)serverJson["ip"],
				Version = (string)serverJson["version"],
				ImageUrl = (string)serverJson["imageUrl"]
			};
			server.Tags = new List<string>();
			foreach (JToken jtoken in serverJson["tags"])
			{
				string item = (string)jtoken;
				server.Tags.Add(item);
			}
			JToken jtoken2;
			bool flag = serverJson.TryGetValue("online", ref jtoken2);
			if (flag)
			{
				server.IsOnline = (bool)jtoken2;
			}
			JToken jtoken3;
			bool flag2 = serverJson.TryGetValue("languages", ref jtoken3);
			if (flag2)
			{
				server.Languages = new List<string>();
				foreach (JToken jtoken4 in jtoken3)
				{
					string item2 = (string)jtoken4;
					server.Languages.Add(item2);
				}
			}
			return server;
		}

		// Token: 0x04003CEA RID: 15594
		private const string BaseUrl = "https://stable.dal-dev.launcherapi.hytale.dev/internalapi/v1alpha1";

		// Token: 0x04003CEB RID: 15595
		private readonly HttpClient _client;

		// Token: 0x02001079 RID: 4217
		public enum SortOrder
		{
			// Token: 0x04004E21 RID: 20001
			Ascending,
			// Token: 0x04004E22 RID: 20002
			Descending
		}

		// Token: 0x0200107A RID: 4218
		public enum ServerSortField
		{
			// Token: 0x04004E24 RID: 20004
			Ping,
			// Token: 0x04004E25 RID: 20005
			PlayerCount,
			// Token: 0x04004E26 RID: 20006
			Name
		}

		// Token: 0x0200107B RID: 4219
		public struct FetchPublicServerQuery
		{
			// Token: 0x04004E27 RID: 20007
			public string Name;

			// Token: 0x04004E28 RID: 20008
			public int? SlotsMin;

			// Token: 0x04004E29 RID: 20009
			public int? SlotsMax;

			// Token: 0x04004E2A RID: 20010
			public string[] Tags;

			// Token: 0x04004E2B RID: 20011
			public string[] Languages;
		}
	}
}
