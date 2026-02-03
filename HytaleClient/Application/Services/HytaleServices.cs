using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Hypixel.ProtoPlus;
using HytaleClient.Application.Auth;
using HytaleClient.Auth.Proto.Protocol;
using HytaleClient.Core;
using HytaleClient.Utils;
using Newtonsoft.Json;
using Newtonsoft.Json.Bson;
using Newtonsoft.Json.Linq;
using NLog;
using Org.BouncyCastle.OpenSsl;

namespace HytaleClient.Application.Services
{
	// Token: 0x02000BF7 RID: 3063
	internal class HytaleServices : Disposable
	{
		// Token: 0x06006169 RID: 24937 RVA: 0x00200678 File Offset: 0x001FE878
		public HytaleServices(App app)
		{
			HytaleServices <>4__this = this;
			this._authManager = app.AuthManager;
			this.QueueTicket = new QueueTicketStateMachine(app);
			ServicePointManager.SecurityProtocol = 3840;
			try
			{
				ServicesEndpoint endpoint = OptionsHelper.Endpoint;
				HytaleServices.Logger.Info<ServicesEndpoint>("Starting with endpoint {0}", endpoint);
				Action <>9__1;
				this._client = new ServicesClient(app, endpoint.Host, endpoint.Port, endpoint.Secure, "ws", new ServicesPacketHandler(app, this), delegate()
				{
					Disposable <>4__this;
					<>4__this.QueueTicket.HandleConnectionClose();
					Engine engine = app.Engine;
					<>4__this = <>4__this;
					Action action;
					if ((action = <>9__1) == null)
					{
						action = (<>9__1 = delegate()
						{
							app.Interface.OnServicesStateChanged(HytaleServices.ServiceState.Disconnected);
						});
					}
					engine.RunOnMainThread(<>4__this, action, true, false);
				});
			}
			catch (Exception exception)
			{
				HytaleServices.Logger.Error(exception, "Got exception when starting services client:");
			}
			this.ScheduleCallbackCleanup();
		}

		// Token: 0x0600616A RID: 24938 RVA: 0x002007DC File Offset: 0x001FE9DC
		private void ScheduleCallbackCleanup()
		{
			Task.Delay(20000, this._cancellationTokenSource.Token).ContinueWith(delegate(Task task)
			{
				bool isCanceled = task.IsCanceled;
				if (!isCanceled)
				{
					DateTime now = DateTime.Now;
					foreach (KeyValuePair<int, HytaleServices.AwaitingCallback> keyValuePair in new Dictionary<int, HytaleServices.AwaitingCallback>(this._callbacks))
					{
						bool flag = now > keyValuePair.Value.TimeoutDateTime;
						if (flag)
						{
							HytaleServices.AwaitingCallback awaitingCallback;
							this._callbacks.TryRemove(keyValuePair.Key, ref awaitingCallback);
						}
					}
					this.ScheduleCallbackCleanup();
				}
			});
		}

		// Token: 0x0600616B RID: 24939 RVA: 0x00200808 File Offset: 0x001FEA08
		private int GetNextId()
		{
			return Interlocked.Add(ref this._currentId, 1);
		}

		// Token: 0x0600616C RID: 24940 RVA: 0x00200828 File Offset: 0x001FEA28
		public void ProcessTypedCallback(int id, ProtoPacket packet)
		{
			HytaleServices.AwaitingCallback awaitingCallback;
			bool flag = this._callbacks.TryRemove(id, ref awaitingCallback);
			if (flag)
			{
				Action<ProtoPacket> successCallback = awaitingCallback.SuccessCallback;
				if (successCallback != null)
				{
					successCallback(packet);
				}
			}
		}

		// Token: 0x0600616D RID: 24941 RVA: 0x00200860 File Offset: 0x001FEA60
		public void ProcessFailureCallback(int id, ClientFailureNotification notification)
		{
			HytaleServices.AwaitingCallback awaitingCallback;
			bool flag = this._callbacks.TryRemove(id, ref awaitingCallback);
			if (flag)
			{
				Action<ClientFailureNotification> failureCallback = awaitingCallback.FailureCallback;
				if (failureCallback != null)
				{
					failureCallback(notification);
				}
			}
		}

		// Token: 0x0600616E RID: 24942 RVA: 0x00200898 File Offset: 0x001FEA98
		public bool IsConnected()
		{
			return this._client != null && this._client.IsConnected();
		}

		// Token: 0x0600616F RID: 24943 RVA: 0x002008C0 File Offset: 0x001FEAC0
		public void Ingest(ClientUser[] users)
		{
			foreach (ClientUser clientUser in users)
			{
				this.Users[clientUser.Uuid_] = new ClientUserWrapper(clientUser);
			}
		}

		// Token: 0x06006170 RID: 24944 RVA: 0x002008FD File Offset: 0x001FEAFD
		public void Ingest(ClientUser user)
		{
			this.Users[user.Uuid_] = new ClientUserWrapper(user);
		}

		// Token: 0x06006171 RID: 24945 RVA: 0x00200918 File Offset: 0x001FEB18
		public void Ingest(ClientUser[] users, List<Guid> into)
		{
			foreach (ClientUser clientUser in users)
			{
				bool flag = clientUser != null;
				if (flag)
				{
					this.Users[clientUser.Uuid_] = new ClientUserWrapper(clientUser);
					into.Add(clientUser.Uuid_);
				}
			}
		}

		// Token: 0x06006172 RID: 24946 RVA: 0x0020096C File Offset: 0x001FEB6C
		public void Ingest(ClientGuildMember[] guildMembers)
		{
			foreach (ClientGuildMember clientGuildMember in guildMembers)
			{
				this.Users[clientGuildMember.User.Uuid_] = new ClientUserWrapper(clientGuildMember.User);
			}
		}

		// Token: 0x06006173 RID: 24947 RVA: 0x002009B4 File Offset: 0x001FEBB4
		private void RegisterCallback<T>(int id, Action<ClientFailureNotification> onFailure, Action<T> onSuccess) where T : ProtoPacket
		{
			this._callbacks[id] = new HytaleServices.AwaitingCallback(onFailure, (onSuccess != null) ? delegate(ProtoPacket o)
			{
				onSuccess((T)((object)o));
			} : null);
		}

		// Token: 0x06006174 RID: 24948 RVA: 0x002009FC File Offset: 0x001FEBFC
		public void SetSkinRekey(sbyte[] pubKey, sbyte[] skinBlob, Action<ClientFailureNotification> onFailure = null, Action<ClientCertificateRefresh> onSuccess = null)
		{
			int nextId = this.GetNextId();
			this.RegisterCallback<ClientCertificateRefresh>(nextId, onFailure, onSuccess);
			this._client.Write(new ClientSetSkinRekey(pubKey, skinBlob, nextId));
		}

		// Token: 0x06006175 RID: 24949 RVA: 0x00200A30 File Offset: 0x001FEC30
		public void SendFriendRequestByUsername(string username, Action<ClientFailureNotification> onFailure = null, Action<ClientSuccessNotification> onSuccess = null)
		{
			int nextId = this.GetNextId();
			this.RegisterCallback<ClientSuccessNotification>(nextId, onFailure, onSuccess);
			this._client.Write(new ClientNamedFriendRequestServerbound(username, nextId));
		}

		// Token: 0x06006176 RID: 24950 RVA: 0x00200A64 File Offset: 0x001FEC64
		public void AnswerFriendRequest(Guid userId, bool accept, Action<ClientFailureNotification> onFailure = null, Action<ClientFriendRequestAccepted> onSuccess = null)
		{
			int nextId = this.GetNextId();
			this.RegisterCallback<ClientFriendRequestAccepted>(nextId, onFailure, onSuccess);
			this._client.Write(new ClientFriendRequestReply(userId, accept, nextId));
		}

		// Token: 0x06006177 RID: 24951 RVA: 0x00200A98 File Offset: 0x001FEC98
		public void RemoveFriend(Guid userId, Action<ClientFailureNotification> onFailure = null, Action<ClientSuccessNotification> onSuccess = null)
		{
			int nextId = this.GetNextId();
			this.RegisterCallback<ClientSuccessNotification>(nextId, onFailure, onSuccess);
			this._client.Write(new ClientRemoveFriend(userId, nextId));
		}

		// Token: 0x06006178 RID: 24952 RVA: 0x00200ACC File Offset: 0x001FECCC
		public void SendMessage(Guid recipient, string message, Action<ClientFailureNotification> onFailure = null, Action<ClientSuccessNotification> onSuccess = null)
		{
			int nextId = this.GetNextId();
			this.RegisterCallback<ClientSuccessNotification>(nextId, onFailure, onSuccess);
			this._client.Write(new ClientPrivateMessageOutbound(recipient, message, nextId));
		}

		// Token: 0x06006179 RID: 24953 RVA: 0x00200B00 File Offset: 0x001FED00
		public void SendPartyMessage(string message, Action<ClientFailureNotification> onFailure = null, Action<ClientChannelMessageInbound> onSuccess = null)
		{
			int nextId = this.GetNextId();
			this.RegisterCallback<ClientChannelMessageInbound>(nextId, onFailure, onSuccess);
			this._client.Write(new ClientChannelMessageOutbound(0, message, nextId));
		}

		// Token: 0x0600617A RID: 24954 RVA: 0x00200B34 File Offset: 0x001FED34
		public void DisbandParty(Action<ClientFailureNotification> onFailure = null, Action<ClientSetParty> onSuccess = null)
		{
			int nextId = this.GetNextId();
			this.RegisterCallback<ClientSetParty>(nextId, onFailure, onSuccess);
			this._client.Write(new ClientPartyDisband(this.PartyWrapper.PartyId, nextId));
		}

		// Token: 0x0600617B RID: 24955 RVA: 0x00200B70 File Offset: 0x001FED70
		public void LeaveParty(Action<ClientFailureNotification> onFailure = null, Action<ClientSetParty> onSuccess = null)
		{
			int nextId = this.GetNextId();
			this.RegisterCallback<ClientSetParty>(nextId, onFailure, onSuccess);
			this._client.Write(new ClientPartyLeave(this.PartyWrapper.PartyId, nextId));
		}

		// Token: 0x0600617C RID: 24956 RVA: 0x00200BAC File Offset: 0x001FEDAC
		public void RemoveMemberFromParty(Guid userId, Action<ClientFailureNotification> onFailure = null, Action<ClientPartyMembersChange> onSuccess = null)
		{
			int nextId = this.GetNextId();
			this.RegisterCallback<ClientPartyMembersChange>(nextId, onFailure, onSuccess);
			this._client.Write(new ClientRemovePartyMember(this.PartyWrapper.PartyId, userId, nextId));
		}

		// Token: 0x0600617D RID: 24957 RVA: 0x00200BEC File Offset: 0x001FEDEC
		public void CreateParty(Action<ClientFailureNotification> onFailure = null, Action<ClientSetParty> onSuccess = null)
		{
			int nextId = this.GetNextId();
			this.RegisterCallback<ClientSetParty>(nextId, onFailure, onSuccess);
			this._client.Write(new ClientCreateParty(nextId));
		}

		// Token: 0x0600617E RID: 24958 RVA: 0x00200C20 File Offset: 0x001FEE20
		public void InviteUserToParty(Guid userId, Action<ClientFailureNotification> onFailure = null, Action<ClientSuccessNotification> onSuccess = null)
		{
			bool flag = this.PartyWrapper == null;
			if (flag)
			{
				this.CreateParty(onFailure, delegate(ClientSetParty party)
				{
					this.InviteUserToParty(userId, onFailure, onSuccess);
					Action<ClientSuccessNotification> onSuccess2 = onSuccess;
					if (onSuccess2 != null)
					{
						onSuccess2(null);
					}
				});
			}
			else
			{
				int nextId = this.GetNextId();
				this.RegisterCallback<ClientSuccessNotification>(nextId, onFailure, onSuccess);
				this._client.Write(new ClientPartyInvite(this.PartyWrapper.PartyId, userId, nextId));
			}
		}

		// Token: 0x0600617F RID: 24959 RVA: 0x00200CB8 File Offset: 0x001FEEB8
		public void MakeUserPartyLeader(Guid userId, Action<ClientFailureNotification> onFailure = null, Action<ClientPartyNewLeader> onSuccess = null)
		{
			int nextId = this.GetNextId();
			this.RegisterCallback<ClientPartyNewLeader>(nextId, onFailure, onSuccess);
			this._client.Write(new ClientPartyTransfer(this.PartyWrapper.PartyId, userId, nextId));
		}

		// Token: 0x06006180 RID: 24960 RVA: 0x00200CF5 File Offset: 0x001FEEF5
		public void FollowToServer(Guid userId, Action<ClientFailureNotification> onFailure = null, Action<ClientPartyNewLeader> onSuccess = null)
		{
		}

		// Token: 0x06006181 RID: 24961 RVA: 0x00200CF8 File Offset: 0x001FEEF8
		public void JoinSharedSinglePlayerWorld(Guid worldId, Action<ClientFailureNotification> onFailure = null, Action<ClientSuccessNotification> onSuccess = null)
		{
			QueueTicketStateMachine queueTicket = this.QueueTicket;
			string str = "ssp:";
			Guid guid = worldId;
			string text;
			bool flag = queueTicket.TryQueue(str + guid.ToString(), null, out text);
			if (flag)
			{
				int nextId = this.GetNextId();
				this.RegisterCallback<ClientSuccessNotification>(nextId, delegate(ClientFailureNotification res)
				{
					Action<ClientFailureNotification> onFailure3 = onFailure;
					if (onFailure3 != null)
					{
						onFailure3(res);
					}
				}, delegate(ClientSuccessNotification res)
				{
					Action<ClientSuccessNotification> onSuccess2 = onSuccess;
					if (onSuccess2 != null)
					{
						onSuccess2(res);
					}
				});
				this._client.Write(new ClientJoinSSPWorld(worldId, text, nextId));
			}
			else
			{
				Action<ClientFailureNotification> onFailure2 = onFailure;
				if (onFailure2 != null)
				{
					onFailure2(new ClientFailureNotification(0, "TryQueue returned null!", new Dictionary<string, string>()));
				}
			}
		}

		// Token: 0x06006182 RID: 24962 RVA: 0x00200DAC File Offset: 0x001FEFAC
		public void CreateSharedSinglePlayerWorld(string name, Action<ClientFailureNotification> onFailure = null, Action<ClientSSPWorldCreated> onSuccess = null)
		{
			int nextId = this.GetNextId();
			this.RegisterCallback<ClientSSPWorldCreated>(nextId, onFailure, onSuccess);
			this._client.Write(new ClientCreateSSPWorld(name, nextId));
		}

		// Token: 0x06006183 RID: 24963 RVA: 0x00200DE0 File Offset: 0x001FEFE0
		public void AnswerPartyInvite(string partyId, bool accept, Action<ClientFailureNotification> onFailure = null, Action<ClientSetParty> onSuccess = null)
		{
			int nextId = this.GetNextId();
			this.RegisterCallback<ClientSetParty>(nextId, onFailure, onSuccess);
			this._client.Write(new ClientPartyInviteResponse(partyId, accept, nextId));
		}

		// Token: 0x06006184 RID: 24964 RVA: 0x00200E14 File Offset: 0x001FF014
		public void ToggleUserBlocked(Guid userId, bool blocked, Action<ClientFailureNotification> onFailure = null, Action<ClientBlockToggleNotification> onSuccess = null)
		{
			int nextId = this.GetNextId();
			this.RegisterCallback<ClientBlockToggleNotification>(nextId, onFailure, onSuccess);
			this._client.Write(new ClientPlayerToggleBlock(userId, blocked, nextId));
		}

		// Token: 0x06006185 RID: 24965 RVA: 0x00200E48 File Offset: 0x001FF048
		public void JoinGameQueue(string queue, sbyte[] extra = null, bool active = true, Action<ClientFailureNotification> onFailure = null, Action<ClientServerQueueReply> onSuccess = null)
		{
			string text;
			bool flag = this.QueueTicket.TryQueue(queue, extra, out text);
			if (flag)
			{
				int nextId = this.GetNextId();
				this.RegisterCallback<ClientServerQueueReply>(nextId, delegate(ClientFailureNotification res)
				{
					Action<ClientFailureNotification> onFailure3 = onFailure;
					if (onFailure3 != null)
					{
						onFailure3(res);
					}
				}, delegate(ClientServerQueueReply res)
				{
					Action<ClientServerQueueReply> onSuccess2 = onSuccess;
					if (onSuccess2 != null)
					{
						onSuccess2(res);
					}
				});
				this._client.Write(new ClientServerQueue(queue, text, extra, active, nextId));
			}
			else
			{
				Action<ClientFailureNotification> onFailure2 = onFailure;
				if (onFailure2 != null)
				{
					onFailure2(new ClientFailureNotification(0, "TryQueue returned null!", new Dictionary<string, string>()));
				}
			}
		}

		// Token: 0x06006186 RID: 24966 RVA: 0x00200EE4 File Offset: 0x001FF0E4
		public void JoinGameQueueDirect(string queue, string ticket, sbyte[] extra, bool active)
		{
			int nextId = this.GetNextId();
			this._client.Write(new ClientServerQueue(queue, ticket, extra, active, nextId));
		}

		// Token: 0x06006187 RID: 24967 RVA: 0x00200F10 File Offset: 0x001FF110
		public void LeaveGameQueue(Action<ClientFailureNotification> onFailure = null, Action<ClientSuccessNotification> onSuccess = null)
		{
			int nextId = this.GetNextId();
			this.RegisterCallback<ClientSuccessNotification>(nextId, onFailure, delegate(ClientSuccessNotification res)
			{
				this.QueueTicket.OnLeaveQueueConfirm();
				Action<ClientSuccessNotification> onSuccess2 = onSuccess;
				if (onSuccess2 != null)
				{
					onSuccess2(res);
				}
			});
			this._client.Write(new ClientLeaveQueue(nextId));
		}

		// Token: 0x06006188 RID: 24968 RVA: 0x00200F60 File Offset: 0x001FF160
		public void SetPlayerOptions(JObject metadata, Action<Exception> callback = null)
		{
			HytaleServices.Logger.Info("SetPlayerOptions() executed");
			bool flag = !this.IsConnected();
			if (flag)
			{
				Action<Exception> callback2 = callback;
				if (callback2 != null)
				{
					callback2(new Exception("Tried to set player options while not connected to services!"));
				}
			}
			else
			{
				sbyte[] skinBlob;
				using (MemoryStream memoryStream = new MemoryStream())
				{
					using (BsonDataWriter bsonDataWriter = new BsonDataWriter(memoryStream))
					{
						new JsonSerializer().Serialize(bsonDataWriter, metadata);
						skinBlob = (sbyte[])memoryStream.ToArray();
					}
				}
				sbyte[] pubKey;
				using (TextWriter textWriter = new StringWriter())
				{
					new PemWriter(textWriter).WriteObject(new MiscPemGenerator(this._authManager.Cert.GetPublicKey()));
					textWriter.Flush();
					pubKey = (sbyte[])Encoding.UTF8.GetBytes(textWriter.ToString());
				}
				this.SetSkinRekey(pubKey, skinBlob, delegate(ClientFailureNotification exception)
				{
					Action<Exception> callback3 = callback;
					if (callback3 != null)
					{
						callback3(new Exception("Failed to refresh with error " + exception.CauseLocalizable));
					}
				}, delegate(ClientCertificateRefresh refresh)
				{
					HytaleServices.Logger.Info<ClientCertificateRefresh>("Got certificate refresh {0}", refresh);
					this._authManager.UpdateCertificate((byte[])refresh.NewCertificate);
				});
			}
		}

		// Token: 0x06006189 RID: 24969 RVA: 0x002010B0 File Offset: 0x001FF2B0
		protected override void DoDispose()
		{
			this._cancellationTokenSource.Cancel();
			this._client.Close();
		}

		// Token: 0x04003C9C RID: 15516
		private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

		// Token: 0x04003C9D RID: 15517
		public readonly ConcurrentDictionary<Guid, ClientUserWrapper> Users = new ConcurrentDictionary<Guid, ClientUserWrapper>();

		// Token: 0x04003C9E RID: 15518
		public readonly ConcurrentDictionary<Guid, ClientUserState> UserStates = new ConcurrentDictionary<Guid, ClientUserState>();

		// Token: 0x04003C9F RID: 15519
		public readonly List<Guid> BlockedPlayers = new List<Guid>();

		// Token: 0x04003CA0 RID: 15520
		public readonly Dictionary<Guid, long> IncomingFriendRequests = new Dictionary<Guid, long>();

		// Token: 0x04003CA1 RID: 15521
		public readonly Dictionary<Guid, long> OutgoingFriendRequests = new Dictionary<Guid, long>();

		// Token: 0x04003CA2 RID: 15522
		public readonly List<Guid> Friends = new List<Guid>();

		// Token: 0x04003CA3 RID: 15523
		public readonly List<ClientGuildInvitationWrapper> GuildInvitations = new List<ClientGuildInvitationWrapper>();

		// Token: 0x04003CA4 RID: 15524
		public ClientPartyWrapper PartyWrapper;

		// Token: 0x04003CA5 RID: 15525
		public readonly List<ClientPartyInvitationWrapper> PartyInvitations = new List<ClientPartyInvitationWrapper>();

		// Token: 0x04003CA6 RID: 15526
		public ClientGuildWrapper GuildWrapper;

		// Token: 0x04003CA7 RID: 15527
		public List<ClientGameWrapper> Games;

		// Token: 0x04003CA8 RID: 15528
		public List<ClientSharedSinglePlayerJoinableWorldWrapper> SharedSinglePlayerJoinableWorlds = new List<ClientSharedSinglePlayerJoinableWorldWrapper>();

		// Token: 0x04003CA9 RID: 15529
		public List<ClientSharedSinglePlayerJoinableWorldWrapper> SharedSinglePlayerInvitedWorlds = new List<ClientSharedSinglePlayerJoinableWorldWrapper>();

		// Token: 0x04003CAA RID: 15530
		public readonly QueueTicketStateMachine QueueTicket;

		// Token: 0x04003CAB RID: 15531
		private readonly ServicesClient _client;

		// Token: 0x04003CAC RID: 15532
		private readonly ConcurrentDictionary<int, HytaleServices.AwaitingCallback> _callbacks = new ConcurrentDictionary<int, HytaleServices.AwaitingCallback>();

		// Token: 0x04003CAD RID: 15533
		private readonly CancellationTokenSource _cancellationTokenSource = new CancellationTokenSource();

		// Token: 0x04003CAE RID: 15534
		private int _currentId;

		// Token: 0x04003CAF RID: 15535
		private readonly AuthManager _authManager;

		// Token: 0x02001059 RID: 4185
		private class AwaitingCallback
		{
			// Token: 0x06006AE3 RID: 27363 RVA: 0x0022522C File Offset: 0x0022342C
			public AwaitingCallback(Action<ClientFailureNotification> failureCallback, Action<ProtoPacket> successCallback)
			{
				this.FailureCallback = failureCallback;
				this.SuccessCallback = successCallback;
				this.TimeoutDateTime = DateTime.Now.AddSeconds(10.0);
			}

			// Token: 0x04004DD9 RID: 19929
			public readonly Action<ClientFailureNotification> FailureCallback;

			// Token: 0x04004DDA RID: 19930
			public readonly Action<ProtoPacket> SuccessCallback;

			// Token: 0x04004DDB RID: 19931
			public readonly DateTime TimeoutDateTime;
		}

		// Token: 0x0200105A RID: 4186
		public enum ServiceState
		{
			// Token: 0x04004DDD RID: 19933
			Disconnected,
			// Token: 0x04004DDE RID: 19934
			Connected,
			// Token: 0x04004DDF RID: 19935
			Connecting,
			// Token: 0x04004DE0 RID: 19936
			Authenticating
		}
	}
}
