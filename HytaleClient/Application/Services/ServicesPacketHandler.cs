using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using Hypixel.ProtoPlus;
using HytaleClient.Application.Auth;
using HytaleClient.Auth.Proto.Protocol;
using HytaleClient.AuthHandshake.Proto.Protocol;
using HytaleClient.Core;
using HytaleClient.Utils;
using NLog;

namespace HytaleClient.Application.Services
{
	// Token: 0x02000BFD RID: 3069
	internal class ServicesPacketHandler : Disposable
	{
		// Token: 0x060061BE RID: 25022 RVA: 0x002022FB File Offset: 0x002004FB
		private void ProcessAuthDisconnect(Disconnect packet, ServicesClient client)
		{
			ServicesPacketHandler.Logger.Warn("We were disconnected while authed with message: {0}", packet.Message);
		}

		// Token: 0x060061BF RID: 25023 RVA: 0x00202314 File Offset: 0x00200514
		private void ProcessAuth0(Auth0 packet, ServicesClient client)
		{
			client.AuthState.ProcessAuth0(packet);
		}

		// Token: 0x060061C0 RID: 25024 RVA: 0x00202324 File Offset: 0x00200524
		private void ProcessAuth2(Auth2 packet, ServicesClient client)
		{
			client.AuthState.ProcessAuth2(packet);
		}

		// Token: 0x060061C1 RID: 25025 RVA: 0x00202334 File Offset: 0x00200534
		private void ProcessAuth4(Auth4 packet, ServicesClient client)
		{
			client.AuthState.ProcessAuth4(packet);
		}

		// Token: 0x060061C2 RID: 25026 RVA: 0x00202344 File Offset: 0x00200544
		private void ProcessAuthFinished(ClientAuth6 packet, ServicesClient client)
		{
			client.AuthState.ProcessAuthFinished(packet);
		}

		// Token: 0x060061C3 RID: 25027 RVA: 0x00202354 File Offset: 0x00200554
		public ServicesPacketHandler(App app, HytaleServices services)
		{
			this._app = app;
			this._services = services;
			this._threadCancellationToken = this._threadCancellationTokenSource.Token;
			this._thread = new Thread(new ThreadStart(this.ProcessPacketsThreadStart))
			{
				Name = "ServicesPacketHandler",
				IsBackground = true
			};
			this._thread.Start();
		}

		// Token: 0x060061C4 RID: 25028 RVA: 0x002023E0 File Offset: 0x002005E0
		public void Receive(ProtoPacket packet, ServicesClient client)
		{
			this._packets.Add(Tuple.Create<ProtoPacket, ServicesClient>(packet, client), this._threadCancellationToken);
		}

		// Token: 0x060061C5 RID: 25029 RVA: 0x002023FC File Offset: 0x002005FC
		protected override void DoDispose()
		{
			Debug.Assert(ThreadHelper.IsMainThread());
			this._threadCancellationTokenSource.Cancel();
			this._thread.Join();
			this._threadCancellationTokenSource.Dispose();
		}

		// Token: 0x060061C6 RID: 25030 RVA: 0x00202430 File Offset: 0x00200630
		private void ProcessAuthPacket(ProtoPacket packet, ServicesClient client)
		{
			switch (packet.GetId())
			{
			case 0:
				this.ProcessAuth0((Auth0)packet, client);
				return;
			case 1:
				this.ProcessDisconnectPacket(packet, client);
				return;
			case 3:
				this.ProcessAuth2((Auth2)packet, client);
				return;
			case 5:
				this.ProcessAuth4((Auth4)packet, client);
				return;
			}
			bool flag = this._unhandledPacketTypes.Add(packet.GetType().Name);
			if (flag)
			{
				ServicesPacketHandler.Logger.Warn("Received unhandled packet type: {0}", packet.GetType().Name);
			}
		}

		// Token: 0x060061C7 RID: 25031 RVA: 0x002024DC File Offset: 0x002006DC
		private void ProcessPacket(ProtoPacket packet, ServicesClient client)
		{
			Debug.Assert(ThreadHelper.IsOnThread(this._thread));
			int id = packet.GetId();
			int num = id;
			switch (num)
			{
			case 1:
				this.ProcessDisconnectPacket(packet, client);
				return;
			case 2:
				this.ProcessAuthFinished((ClientAuth6)packet, client);
				return;
			case 3:
				this.ProcessBlockToggleNotification((ClientBlockToggleNotification)packet, client);
				return;
			case 4:
			case 7:
			case 8:
			case 9:
			case 13:
			case 14:
			case 16:
			case 17:
			case 18:
			case 20:
			case 21:
			case 25:
			case 26:
			case 27:
			case 28:
			case 30:
			case 32:
			case 33:
			case 35:
			case 36:
			case 39:
			case 41:
			case 43:
			case 47:
			case 48:
			case 50:
			case 51:
			case 52:
			case 53:
			case 58:
				break;
			case 5:
				this.ProcessCertificateRefresh((ClientCertificateRefresh)packet, client);
				return;
			case 6:
				this.ProcessChannelMessage((ClientChannelMessageInbound)packet, client);
				return;
			case 10:
				this.ProcessFailureNotification((ClientFailureNotification)packet, client);
				return;
			case 11:
				this.ProcessFriendRemoved((ClientFriendRemoved)packet, client);
				return;
			case 12:
				this.ProcessFriendRequestAccepted((ClientFriendRequestAccepted)packet, client);
				return;
			case 15:
				return;
			case 19:
				this.ProcessGuildInviteNotification((ClientGuildInviteNotification)packet, client);
				return;
			case 22:
				this.ProcessGuildMemberAdd((ClientGuildMemberAdd)packet, client);
				return;
			case 23:
				this.ProcessGuildMemberRemove((ClientGuildMemberRemove)packet, client);
				return;
			case 24:
				this.ProcessGuildRankChange((ClientGuildRankChange)packet, client);
				return;
			case 29:
				this.ProcessLocalizedMessage((ClientLocalizedMessage)packet, client);
				return;
			case 31:
				this.ProcessNewTempLanguageMapping((ClientNewTempLanguageMapping)packet, client);
				return;
			case 34:
				this.ProcessPartyInviteNotification((ClientPartyInviteNotification)packet, client);
				return;
			case 37:
				this.ProcessPartyMembersChange((ClientPartyMembersChange)packet, client);
				return;
			case 38:
				this.ProcessPartyNewLeader((ClientPartyNewLeader)packet, client);
				return;
			case 40:
				this.ProcessPlayerStateChange((ClientPlayerStateChange)packet, client);
				return;
			case 42:
				this.ProcessPlayerMessage((ClientPrivateMessageInbound)packet, client);
				return;
			case 44:
				this.ProcessQueueRequest((ClientQueueRequest)packet, client);
				return;
			case 45:
				this.ProcessReceiveFriendRequest((ClientReceiveFriendRequest)packet, client);
				return;
			case 46:
				this.ProcessRejoinExpired((ClientRejoinExpired)packet, client);
				return;
			case 49:
				this.ProcessRequeueFailure((ClientRequeueFailure)packet, client);
				return;
			case 54:
				this.ProcessSharedSinglePlayerWorldAccessRemoved((ClientSSPWorldAccessRemoved)packet, client);
				return;
			case 55:
				this.ProcessSharedSinglePlayerWorldCreated((ClientSSPWorldCreated)packet, client);
				return;
			case 56:
				this.ProcessSharedSinglePlayerWorldInviteNotice((ClientSSPWorldInviteNotice)packet, client);
				return;
			case 57:
				this.ProcessSelfState((ClientSelfState)packet, client);
				return;
			case 59:
				this.ProcessServerQueueReply((ClientServerQueueReply)packet, client);
				return;
			case 60:
				this.ProcessSetGuild((ClientSetGuild)packet, client);
				return;
			case 61:
				this.ProcessSetParty((ClientSetParty)packet, client);
				return;
			default:
				if (num == 65)
				{
					this.ProcessSuccessNotification((ClientSuccessNotification)packet, client);
					return;
				}
				if (num == 67)
				{
					this.ProcessUserBoundChat((ClientUserBoundChat)packet, client);
					return;
				}
				break;
			}
			bool flag = this._unhandledPacketTypes.Add(packet.GetType().Name);
			if (flag)
			{
				ServicesPacketHandler.Logger.Warn("Received unhandled packet type: {0}", packet.GetType().Name);
			}
		}

		// Token: 0x060061C8 RID: 25032 RVA: 0x002028A0 File Offset: 0x00200AA0
		private void ProcessDisconnectPacket(ProtoPacket packet, ServicesClient client)
		{
			Disconnect disconnect = packet as Disconnect;
			if (disconnect == null)
			{
				ClientDisconnect clientDisconnect = packet as ClientDisconnect;
				if (clientDisconnect != null)
				{
					this.ProcessDisconnect(clientDisconnect, client);
				}
			}
			else
			{
				this.ProcessAuthDisconnect(disconnect, client);
			}
		}

		// Token: 0x060061C9 RID: 25033 RVA: 0x002028E4 File Offset: 0x00200AE4
		private void ProcessPacketsThreadStart()
		{
			Debug.Assert(ThreadHelper.IsOnThread(this._thread));
			Tuple<ProtoPacket, ServicesClient> tuple = null;
			while (!this._threadCancellationToken.IsCancellationRequested)
			{
				try
				{
					tuple = this._packets.Take(this._threadCancellationToken);
				}
				catch (OperationCanceledException)
				{
					break;
				}
				bool authed = tuple.Item2.AuthState.Authed;
				if (authed)
				{
					this.ProcessPacket(tuple.Item1, tuple.Item2);
				}
				else
				{
					this.ProcessAuthPacket(tuple.Item1, tuple.Item2);
				}
			}
		}

		// Token: 0x060061CA RID: 25034 RVA: 0x0020298C File Offset: 0x00200B8C
		private void ProcessFriendRemoved(ClientFriendRemoved packet, ServicesClient client)
		{
			this._services.Friends.Remove(packet.Player);
			this._app.Engine.RunOnMainThread(this, delegate
			{
				this._app.Interface.OnServicesFriendsRemoved(packet.Player.ToString());
			}, false, false);
			ServicesPacketHandler.Logger.Info("You are no longer friends with {0}", this._services.Users[packet.Player].Name);
			this._services.ProcessTypedCallback(packet.Token, packet);
		}

		// Token: 0x060061CB RID: 25035 RVA: 0x00202A38 File Offset: 0x00200C38
		private void ProcessReceiveFriendRequest(ClientReceiveFriendRequest packet, ServicesClient client)
		{
			ServicesPacketHandler.Logger.Info("Got friend request from {0}!", packet.FriendRequest.User.Name);
			this._services.Ingest(packet.FriendRequest.User);
			this._services.IncomingFriendRequests.Add(packet.FriendRequest.User.Uuid_, packet.FriendRequest.ExpiresAt);
			this._app.Engine.RunOnMainThread(this, delegate
			{
				this._app.Interface.TriggerEvent("services.users.updated", new ClientUserWrapper(packet.FriendRequest.User), null, null, null, null, null);
				this._app.Interface.TriggerEvent("services.friendRequests.added", packet.FriendRequest.User.Uuid_.ToString(), packet.FriendRequest.ExpiresAt.ToString(), null, null, null, null);
			}, false, false);
		}

		// Token: 0x060061CC RID: 25036 RVA: 0x00202AF4 File Offset: 0x00200CF4
		private void ProcessFriendRequestAccepted(ClientFriendRequestAccepted packet, ServicesClient client)
		{
			this._services.Ingest(packet.NewFriend);
			this._services.Friends.Add(packet.NewFriend.Uuid_);
			this._services.IncomingFriendRequests.Remove(packet.NewFriend.Uuid_);
			this._app.Engine.RunOnMainThread(this, delegate
			{
				this._app.Interface.TriggerEvent("services.users.updated", new ClientUserWrapper(packet.NewFriend), null, null, null, null, null);
				this._app.Interface.OnServicesFriendsAdded(packet.NewFriend.Uuid_.ToString());
				this._app.Interface.TriggerEvent("services.friendRequests.removed", packet.NewFriend.Uuid_.ToString(), null, null, null, null, null);
			}, false, false);
			ServicesPacketHandler.Logger.Info("You are now friends with {0}!", packet.NewFriend.Name);
			this._services.ProcessTypedCallback(packet.Token, packet);
		}

		// Token: 0x060061CD RID: 25037 RVA: 0x00202BCC File Offset: 0x00200DCC
		private void ProcessPlayerMessage(ClientPrivateMessageInbound packet, ServicesClient client)
		{
			ServicesPacketHandler.Logger.Info<string, string>("Received message from {0}: {1}", this._services.Users[packet.From].Name, packet.Message);
			this._app.Engine.RunOnMainThread(this, delegate
			{
				this._app.Interface.TriggerEvent("services.messages.received", packet.From.ToString(), packet.Message, packet.Timestamp.ToString(), null, null, null);
			}, false, false);
		}

		// Token: 0x060061CE RID: 25038 RVA: 0x00202C49 File Offset: 0x00200E49
		private void ProcessDisconnect(ClientDisconnect packet, ServicesClient client)
		{
			ServicesPacketHandler.Logger.Info("We were disconnected while authed with message: {0}", packet.Message);
		}

		// Token: 0x060061CF RID: 25039 RVA: 0x00202C64 File Offset: 0x00200E64
		private void ProcessPlayerStateChange(ClientPlayerStateChange packet, ServicesClient client)
		{
			ServicesPacketHandler.Logger.Info<ClientPlayerStateChange>("Service state change: {0}", packet);
			this._services.UserStates[packet.Uuid_] = new ClientUserState(packet.State);
			this._app.Engine.RunOnMainThread(this, delegate
			{
				this._app.Interface.OnServicesUserStateChanged(packet.Uuid_.ToString(), new ClientUserState(packet.State));
			}, false, false);
		}

		// Token: 0x060061D0 RID: 25040 RVA: 0x00202CE8 File Offset: 0x00200EE8
		private void ProcessRequeueFailure(ClientRequeueFailure packet, ServicesClient client)
		{
			ServicesPacketHandler.Logger.Info<ClientRequeueFailure.RequeueFailureCause>("Got requeue failure cause {0}", packet.Cause);
		}

		// Token: 0x060061D1 RID: 25041 RVA: 0x00202D01 File Offset: 0x00200F01
		private void ProcessRejoinExpired(ClientRejoinExpired packet, ServicesClient client)
		{
			ServicesPacketHandler.Logger.Info<Guid>("Got rejoin expired for {0}", packet.WorldId);
		}

		// Token: 0x060061D2 RID: 25042 RVA: 0x00202D1C File Offset: 0x00200F1C
		private void ProcessQueueRequest(ClientQueueRequest packet, ServicesClient client)
		{
			ServicesPacketHandler.Logger.Info("Services requests that we queue for {0}", packet.Key);
			string ticket;
			bool flag = this._services.QueueTicket.TryQueue(packet.Key, packet.Extra, out ticket);
			if (flag)
			{
				this._services.JoinGameQueueDirect(packet.Key, ticket, packet.Extra, true);
			}
			else
			{
				ServicesPacketHandler.Logger.Info<string, string>("Ignoring server queue request because we're already queued for {0} with ticket {1}", this._services.QueueTicket.Key, this._services.QueueTicket.Ticket);
			}
		}

		// Token: 0x060061D3 RID: 25043 RVA: 0x00202DB4 File Offset: 0x00200FB4
		private void ProcessSelfState(ClientSelfState packet, ServicesClient client)
		{
			ServicesPacketHandler.Logger.Info<ClientSelfState, ServicesClient>("Got self state {0} for client {1}", packet, client);
			long num = (long)(DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0)).TotalMilliseconds;
			bool flag = num + 10000L < packet.CurrentTimeMillis || num - 10000L > packet.CurrentTimeMillis;
			if (flag)
			{
				ServicesPacketHandler.Logger.Warn<long, long>("Is your clock right? The server says it's {0} but the local clock reads {1}", packet.CurrentTimeMillis, num);
			}
			else
			{
				ServicesPacketHandler.Logger.Info<long, long>("Your clock seems to be reasonably correct. Server says it's {0} and local clock reads {1}", packet.CurrentTimeMillis, num);
			}
			bool flag2 = this._services.QueueTicket != null;
			if (flag2)
			{
				this._services.QueueTicket.HandleConnectionOpen();
			}
			AuthManager authManager = this._app.AuthManager;
			this._services.Games = Enumerable.ToList<ClientGameWrapper>(Enumerable.Select<ClientGame, ClientGameWrapper>(packet.GameList.Games, (ClientGame game) => new ClientGameWrapper(game)));
			this._services.Users.Clear();
			this._services.Users[authManager.Settings.Uuid] = new ClientUserWrapper(authManager.Settings.Uuid, authManager.Settings.Username, null);
			this._services.BlockedPlayers.Clear();
			this._services.Ingest(packet.BlockedPlayers, this._services.BlockedPlayers);
			this._services.GuildInvitations.Clear();
			foreach (ClientGuildInvitation clientGuildInvitation in packet.GuildInvitations)
			{
				this._services.Ingest(clientGuildInvitation.By);
				this._services.GuildInvitations.Add(new ClientGuildInvitationWrapper(clientGuildInvitation));
			}
			this._services.IncomingFriendRequests.Clear();
			foreach (ClientFriendRequest clientFriendRequest in packet.FriendRequests)
			{
				this._services.Ingest(clientFriendRequest.User);
				this._services.IncomingFriendRequests[clientFriendRequest.User.Uuid_] = clientFriendRequest.ExpiresAt;
			}
			this._services.OutgoingFriendRequests.Clear();
			foreach (ClientFriendRequest clientFriendRequest2 in packet.FriendRequestsOutbound)
			{
				this._services.Ingest(clientFriendRequest2.User);
				this._services.OutgoingFriendRequests[clientFriendRequest2.User.Uuid_] = clientFriendRequest2.ExpiresAt;
			}
			this._services.Friends.Clear();
			foreach (KeyValuePair<ClientUser, ClientPlayerState> keyValuePair in packet.Friends)
			{
				ServicesPacketHandler.Logger.Info<ClientUser, ClientPlayerState>("Handling friend: {0}, {1}", keyValuePair.Key, keyValuePair.Value);
				this._services.Ingest(keyValuePair.Key);
				this._services.UserStates[keyValuePair.Key.Uuid_] = new ClientUserState(keyValuePair.Value);
				this._services.Friends.Add(keyValuePair.Key.Uuid_);
			}
			bool flag3 = packet.Party != null;
			if (flag3)
			{
				this._services.PartyWrapper = new ClientPartyWrapper(packet.Party);
				this._services.Ingest(packet.Party.Members);
			}
			else
			{
				this._services.PartyWrapper = null;
			}
			bool flag4 = packet.Guild != null;
			if (flag4)
			{
				this._services.GuildWrapper = new ClientGuildWrapper(packet.Guild);
				this._services.Ingest(packet.Guild.Officers);
				this._services.Ingest(packet.Guild.Members);
			}
			else
			{
				this._services.GuildWrapper = null;
			}
			this._services.SharedSinglePlayerJoinableWorlds.Clear();
			foreach (ClientSSPJoinableWorld world in packet.JoinableSspWorlds)
			{
				this._services.SharedSinglePlayerJoinableWorlds.Add(new ClientSharedSinglePlayerJoinableWorldWrapper(world));
			}
			this._services.SharedSinglePlayerInvitedWorlds.Clear();
			foreach (ClientSSPJoinableWorld world2 in packet.InvitedSspWorlds)
			{
				this._services.SharedSinglePlayerInvitedWorlds.Add(new ClientSharedSinglePlayerJoinableWorldWrapper(world2));
			}
			this._services.PartyInvitations.Clear();
			foreach (ClientPartyInvitation clientPartyInvitation in packet.PartyInvitations)
			{
				this._services.Ingest(clientPartyInvitation.InvitedBy);
				this._services.PartyInvitations.Add(new ClientPartyInvitationWrapper(clientPartyInvitation));
			}
			foreach (ClientRejoinableServer clientRejoinableServer in packet.RejoinableServers)
			{
				bool isInfoEnabled = ServicesPacketHandler.Logger.IsInfoEnabled;
				if (isInfoEnabled)
				{
					ServicesPacketHandler.Logger.Info("Found rejoinable server {0} on {1}:{2} expiring at {3} running mode {4}", new object[]
					{
						clientRejoinableServer.WorldId,
						clientRejoinableServer.Ip,
						clientRejoinableServer.Port,
						clientRejoinableServer.ExpiresAt,
						clientRejoinableServer.GameInfoJsonDebug
					});
				}
			}
			this._app.Engine.RunOnMainThread(this, delegate
			{
				this._app.Interface.OnServicesInitialized();
			}, false, false);
		}

		// Token: 0x060061D4 RID: 25044 RVA: 0x002033A8 File Offset: 0x002015A8
		private void ProcessUserBoundChat(ClientUserBoundChat packet, ServicesClient client)
		{
			this._services.Ingest(packet.From);
			bool isInfoEnabled = ServicesPacketHandler.Logger.IsInfoEnabled;
			if (isInfoEnabled)
			{
				ServicesPacketHandler.Logger.Info("[{0}] {1} ({2}): {3}", new object[]
				{
					packet.Channel,
					packet.From.Name,
					packet.From.Uuid_,
					packet.Message
				});
			}
			this._app.Engine.RunOnMainThread(this, delegate
			{
				this._app.Interface.TriggerEvent("services.users.updated", new ClientUserWrapper(packet.From), null, null, null, null, null);
				bool flag = packet.Channel == 0;
				if (flag)
				{
					this._app.Interface.TriggerEvent("services.party.messages.received", packet.From.ToString(), packet.Message, packet.Timestamp.ToString(), null, null, null);
				}
			}, false, false);
			this._services.ProcessTypedCallback(packet.Token, packet);
		}

		// Token: 0x060061D5 RID: 25045 RVA: 0x00203490 File Offset: 0x00201690
		private void ProcessChannelMessage(ClientChannelMessageInbound packet, ServicesClient client)
		{
			ServicesPacketHandler.Logger.Info<ClientChatChannel, Guid, string>("[{0}] {1}: {2}", packet.Channel, packet.From, packet.Message);
			this._app.Engine.RunOnMainThread(this, delegate
			{
				bool flag = packet.Channel == 0;
				if (flag)
				{
					this._app.Interface.TriggerEvent("services.party.messages.received", packet.From.ToString(), packet.Message, packet.Timestamp.ToString(), null, null, null);
				}
			}, false, false);
			this._services.ProcessTypedCallback(packet.Token, packet);
		}

		// Token: 0x060061D6 RID: 25046 RVA: 0x00203520 File Offset: 0x00201720
		private void ProcessLocalizedMessage(ClientLocalizedMessage packet, ServicesClient client)
		{
			ServicesPacketHandler.Logger.Info<string, Dictionary<string, string>>("Got localized message {0} with params {1}", packet.Key, packet.Params);
		}

		// Token: 0x060061D7 RID: 25047 RVA: 0x00203540 File Offset: 0x00201740
		private void ProcessBlockToggleNotification(ClientBlockToggleNotification packet, ServicesClient client)
		{
			bool blocked = packet.Blocked;
			if (blocked)
			{
				this._services.BlockedPlayers.Add(packet.Target);
				this._app.Engine.RunOnMainThread(this, delegate
				{
					this._app.Interface.TriggerEvent("services.blockedUsers.added", packet.Target.ToString(), null, null, null, null, null);
				}, false, false);
				ServicesPacketHandler.Logger.Info("You are now blocking {0}", this._services.Users[packet.Target].Name);
			}
			else
			{
				this._services.BlockedPlayers.Remove(packet.Target);
				this._app.Engine.RunOnMainThread(this, delegate
				{
					this._app.Interface.TriggerEvent("services.blockedUsers.removed", packet.Target.ToString(), null, null, null, null, null);
				}, false, false);
				ServicesPacketHandler.Logger.Info("You have unblocked {0}", this._services.Users[packet.Target].Name);
			}
			this._services.ProcessTypedCallback(packet.Token, packet);
		}

		// Token: 0x060061D8 RID: 25048 RVA: 0x0020366C File Offset: 0x0020186C
		private void ProcessNewTempLanguageMapping(ClientNewTempLanguageMapping packet, ServicesClient client)
		{
		}

		// Token: 0x060061D9 RID: 25049 RVA: 0x0020366F File Offset: 0x0020186F
		private void ProcessServerQueueReply(ClientServerQueueReply packet, ServicesClient client)
		{
			this._services.QueueTicket.HandleResponse(packet);
			this._services.ProcessTypedCallback(packet.Token, packet);
		}

		// Token: 0x060061DA RID: 25050 RVA: 0x00203697 File Offset: 0x00201897
		private void ProcessCertificateRefresh(ClientCertificateRefresh packet, ServicesClient client)
		{
			this._services.ProcessTypedCallback(packet.Token, packet);
		}

		// Token: 0x060061DB RID: 25051 RVA: 0x002036AD File Offset: 0x002018AD
		private void ProcessSuccessNotification(ClientSuccessNotification packet, ServicesClient client)
		{
			this._services.ProcessTypedCallback(packet.Token, packet);
		}

		// Token: 0x060061DC RID: 25052 RVA: 0x002036C3 File Offset: 0x002018C3
		private void ProcessFailureNotification(ClientFailureNotification packet, ServicesClient client)
		{
			this._services.ProcessFailureCallback(packet.Token, packet);
		}

		// Token: 0x060061DD RID: 25053 RVA: 0x002036DC File Offset: 0x002018DC
		private void ProcessGuildMemberAdd(ClientGuildMemberAdd packet, ServicesClient client)
		{
			bool flag = this._services.GuildWrapper != null && this._services.GuildWrapper.GuildId.Equals(packet.GuildId);
			if (flag)
			{
				ServicesPacketHandler.Logger.Info<ClientGuildMemberAdd>("Got guild member add {0}", packet);
				this._services.GuildWrapper.Members.Add(new ClientGuildMemberWrapper(packet.Member));
				this._services.Ingest(packet.Member.User);
			}
			else
			{
				ServicesPacketHandler.Logger.Warn<ClientGuildWrapper, string>("Got guild member add for a guild we're not in: {0} vs {1}", this._services.GuildWrapper, packet.GuildId);
			}
		}

		// Token: 0x060061DE RID: 25054 RVA: 0x00203789 File Offset: 0x00201989
		private void ProcessGuildInviteNotification(ClientGuildInviteNotification packet, ServicesClient client)
		{
			ServicesPacketHandler.Logger.Info<ClientGuildInvitation>("You were invited to join guild {0}", packet.Invitation);
			this._services.GuildInvitations.Add(new ClientGuildInvitationWrapper(packet.Invitation));
		}

		// Token: 0x060061DF RID: 25055 RVA: 0x002037C0 File Offset: 0x002019C0
		private void ProcessGuildMemberRemove(ClientGuildMemberRemove packet, ServicesClient client)
		{
			ServicesPacketHandler.Logger.Info<ClientUserWrapper, string>("{0} was removed from guild {1}", this._services.Users[packet.Removed], packet.GuildId);
			Predicate<ClientGuildMemberWrapper> match = (ClientGuildMemberWrapper member) => member.Uuid.Equals(packet.Removed);
			this._services.GuildWrapper.Officers.RemoveAll(match);
			this._services.GuildWrapper.Members.RemoveAll(match);
			this._services.ProcessTypedCallback(packet.Token, packet);
		}

		// Token: 0x060061E0 RID: 25056 RVA: 0x0020386C File Offset: 0x00201A6C
		private void ProcessGuildRankChange(ClientGuildRankChange packet, ServicesClient client)
		{
			bool flag = this._services.GuildWrapper != null && this._services.GuildWrapper.GuildId.Equals(packet.GuildId);
			if (flag)
			{
				ServicesPacketHandler.Logger.Info<ClientGuildRankChange>("Got guild rank change {0}", packet);
				Predicate<ClientGuildMemberWrapper> match = (ClientGuildMemberWrapper test) => test.Uuid.Equals(packet.UpdatedMember);
				this._services.GuildWrapper.Officers.RemoveAll(match);
				this._services.GuildWrapper.Members.RemoveAll(match);
				ClientUserWrapper clientUserWrapper = this._services.Users[packet.By];
				ClientGuildMemberWrapper clientGuildMemberWrapper = new ClientGuildMemberWrapper(packet.UpdatedMember, packet.NewRank);
				bool flag2 = clientGuildMemberWrapper.Rank == 0;
				if (flag2)
				{
					ClientGuildMemberWrapper leader = this._services.GuildWrapper.Leader;
					this._services.GuildWrapper.Leader = clientGuildMemberWrapper;
					this._services.GuildWrapper.Officers.Add(leader);
					ServicesPacketHandler.Logger.Info<string, string, string>("{0} promoted {1} to guild {2}", clientUserWrapper.Name, this._services.Users[clientGuildMemberWrapper.Uuid].Name, "Rank");
				}
				else
				{
					bool flag3 = clientGuildMemberWrapper.Rank == 1;
					if (flag3)
					{
						this._services.GuildWrapper.Officers.Add(clientGuildMemberWrapper);
						ServicesPacketHandler.Logger.Info<string, string, string>("{0} promoted {1} to guild {2}", clientUserWrapper.Name, this._services.Users[clientGuildMemberWrapper.Uuid].Name, "Rank");
					}
					else
					{
						bool flag4 = clientGuildMemberWrapper.Rank == 2;
						if (flag4)
						{
							this._services.GuildWrapper.Members.Add(clientGuildMemberWrapper);
							ServicesPacketHandler.Logger.Info<string, string, string>("{0} demoted {1} to guild {2}", clientUserWrapper.Name, this._services.Users[clientGuildMemberWrapper.Uuid].Name, "Rank");
						}
					}
				}
			}
			else
			{
				ServicesPacketHandler.Logger.Warn<ClientGuildWrapper, string>("Got guild rank change for a guild we're not in: {0} vs {1}", this._services.GuildWrapper, packet.GuildId);
			}
			this._services.ProcessTypedCallback(packet.Token, packet);
		}

		// Token: 0x060061E1 RID: 25057 RVA: 0x00203AE0 File Offset: 0x00201CE0
		private void ProcessSetGuild(ClientSetGuild packet, ServicesClient client)
		{
			bool flag = (this._services.GuildWrapper == null && packet.OldId == null) || (this._services.GuildWrapper != null && packet.OldId != null && packet.OldId.Equals(this._services.GuildWrapper.GuildId));
			if (flag)
			{
				bool flag2 = packet.Guild != null;
				if (flag2)
				{
					this._services.GuildWrapper = new ClientGuildWrapper(packet.Guild);
				}
				else
				{
					this._services.GuildWrapper = null;
				}
				ServicesPacketHandler.Logger.Info<ClientGuildWrapper>("Set guild to {0}", this._services.GuildWrapper);
				bool flag3 = packet.Guild != null;
				if (flag3)
				{
					this._services.Ingest(packet.Guild.Leader.User);
					this._services.Ingest(packet.Guild.Officers);
					this._services.Ingest(packet.Guild.Members);
				}
			}
			else
			{
				ServicesPacketHandler.Logger.Warn<string, ClientGuildWrapper>("Got mismatching guild IDs for {0} vs {1}", packet.OldId, this._services.GuildWrapper);
			}
			this._services.ProcessTypedCallback(packet.Token, packet);
		}

		// Token: 0x060061E2 RID: 25058 RVA: 0x00203C24 File Offset: 0x00201E24
		private void ProcessPartyMembersChange(ClientPartyMembersChange packet, ServicesClient client)
		{
			bool flag = this._services.PartyWrapper != null;
			if (flag)
			{
				bool flag2 = packet.PartyId.Equals(this._services.PartyWrapper.PartyId);
				if (flag2)
				{
					bool flag3 = packet.UserAdd != null;
					if (flag3)
					{
						this._services.Ingest(packet.UserAdd);
						this._services.PartyWrapper.Members.Add(packet.UserAdd.Uuid_);
						ServicesPacketHandler.Logger.Info("{0} joined the party", packet.UserAdd.Name);
						this._app.Engine.RunOnMainThread(this, delegate
						{
							this._app.Interface.TriggerEvent("services.users.updated", new ClientUserWrapper(packet.UserAdd), null, null, null, null, null);
							this._app.Interface.TriggerEvent("services.party.memberAdded", packet.UserAdd.Uuid_.ToString(), null, null, null, null, null);
						}, false, false);
					}
					else
					{
						this._services.PartyWrapper.Members.RemoveAll((Guid e) => e.Equals(packet.UserDel));
						ClientUserWrapper argument = this._services.Users[packet.UserDel];
						ServicesPacketHandler.Logger.Info<ClientUserWrapper>("{0} is no longer a member of the party", argument);
						this._app.Engine.RunOnMainThread(this, delegate
						{
							this._app.Interface.TriggerEvent("services.party.memberRemoved", packet.UserDel.ToString(), null, null, null, null, null);
						}, false, false);
					}
				}
				else
				{
					ServicesPacketHandler.Logger.Warn<string, string>("Received mismatching party ID {0} vs {1}", packet.PartyId, this._services.PartyWrapper.PartyId);
				}
			}
			else
			{
				ServicesPacketHandler.Logger.Warn<ClientPartyMembersChange>("Received {0} with no party set!", packet);
			}
			this._services.ProcessTypedCallback(packet.Token, packet);
		}

		// Token: 0x060061E3 RID: 25059 RVA: 0x00203DF8 File Offset: 0x00201FF8
		private void ProcessSetParty(ClientSetParty packet, ServicesClient client)
		{
			bool flag = (this._services.PartyWrapper == null && packet.OldId == null) || (this._services.PartyWrapper != null && packet.OldId.Equals(this._services.PartyWrapper.PartyId));
			if (flag)
			{
				bool flag2 = packet.CurrentParty != null;
				if (flag2)
				{
					this._services.PartyWrapper = new ClientPartyWrapper(packet.CurrentParty);
					this._services.Ingest(packet.CurrentParty.Leader);
					this._services.Ingest(packet.CurrentParty.Members);
					this._app.Engine.RunOnMainThread(this, delegate
					{
						this._app.Interface.TriggerEvent("services.users.updated", new ClientUserWrapper(packet.CurrentParty.Leader), null, null, null, null, null);
						this._app.Interface.TriggerEvent("services.users.updatedMultiple", Enumerable.ToArray<ClientUserWrapper>(Enumerable.Select<ClientUser, ClientUserWrapper>(packet.CurrentParty.Members, (ClientUser m) => new ClientUserWrapper(m))), null, null, null, null, null);
					}, false, false);
				}
				else
				{
					this._services.PartyWrapper = null;
				}
				this._app.Engine.RunOnMainThread(this, delegate
				{
					this._app.Interface.TriggerEvent("services.party.set", this._services.PartyWrapper, null, null, null, null, null);
				}, false, false);
				ServicesPacketHandler.Logger.Info<ClientPartyWrapper>("Set party to {}", this._services.PartyWrapper);
			}
			else
			{
				ServicesPacketHandler.Logger.Info<string, ClientPartyWrapper>("Got mismatching party IDs for {0} vs {1}", packet.OldId, this._services.PartyWrapper);
			}
			this._services.ProcessTypedCallback(packet.Token, packet);
		}

		// Token: 0x060061E4 RID: 25060 RVA: 0x00203F8C File Offset: 0x0020218C
		private void ProcessPartyNewLeader(ClientPartyNewLeader packet, ServicesClient client)
		{
			bool flag = this._services.PartyWrapper != null && this._services.PartyWrapper.PartyId.Equals(packet.PartyIdHex);
			if (flag)
			{
				ClientUserWrapper clientUserWrapper = this._services.Users[this._services.PartyWrapper.Leader];
				ClientUserWrapper newLeader = this._services.Users[packet.NewLeader];
				this._services.PartyWrapper.Leader = newLeader.Uuid;
				ServicesPacketHandler.Logger.Info<string, string, string>("{0} promoted {1} to the party leader of {2}", clientUserWrapper.Name, newLeader.Name, packet.PartyIdHex);
				this._app.Engine.RunOnMainThread(this, delegate
				{
					this._app.Interface.TriggerEvent("services.party.leaderChanged", newLeader.Uuid.ToString(), null, null, null, null, null);
				}, false, false);
			}
			else
			{
				ServicesPacketHandler.Logger.Info<string, ClientPartyWrapper>("Got mismaching party IDs for {0} vs {1}", packet.PartyIdHex, this._services.PartyWrapper);
			}
			this._services.ProcessTypedCallback(packet.Token, packet);
		}

		// Token: 0x060061E5 RID: 25061 RVA: 0x002040B4 File Offset: 0x002022B4
		private void ProcessPartyInviteNotification(ClientPartyInviteNotification packet, ServicesClient client)
		{
			ServicesPacketHandler.Logger.Info<string, string>("{0} invited you to party {1}", packet.PartyInvitation.InvitedBy.Name, packet.PartyInvitation.PartyIdHex);
			this._services.Ingest(packet.PartyInvitation.InvitedBy);
			this._services.PartyInvitations.Add(new ClientPartyInvitationWrapper(packet.PartyInvitation));
			this._app.Engine.RunOnMainThread(this, delegate
			{
				this._app.Interface.TriggerEvent("services.users.updated", new ClientUserWrapper(packet.PartyInvitation.InvitedBy), null, null, null, null, null);
				this._app.Interface.TriggerEvent("services.party.invitationReceived", new ClientPartyInvitationWrapper(packet.PartyInvitation), null, null, null, null, null);
			}, false, false);
		}

		// Token: 0x060061E6 RID: 25062 RVA: 0x00204168 File Offset: 0x00202368
		private void ProcessSharedSinglePlayerWorldAccessRemoved(ClientSSPWorldAccessRemoved packet, ServicesClient client)
		{
			Predicate<ClientSharedSinglePlayerJoinableWorldWrapper> match = (ClientSharedSinglePlayerJoinableWorldWrapper world) => world.WorldId.Equals(packet.WorldId);
			this._services.SharedSinglePlayerJoinableWorlds.RemoveAll(match);
			ServicesPacketHandler.Logger.Info<Guid>("World access removed for {0}", packet.WorldId);
		}

		// Token: 0x060061E7 RID: 25063 RVA: 0x002041BD File Offset: 0x002023BD
		private void ProcessSharedSinglePlayerWorldInviteNotice(ClientSSPWorldInviteNotice packet, ServicesClient client)
		{
			ServicesPacketHandler.Logger.Info<ClientSSPJoinableWorld>("Got invitation for access to SharedSinglePlayer world {0}!", packet.World);
			this._services.SharedSinglePlayerInvitedWorlds.Add(new ClientSharedSinglePlayerJoinableWorldWrapper(packet.World));
		}

		// Token: 0x060061E8 RID: 25064 RVA: 0x002041F4 File Offset: 0x002023F4
		private void ProcessSharedSinglePlayerWorldCreated(ClientSSPWorldCreated packet, ServicesClient client)
		{
			ClientSharedSinglePlayerJoinableWorldWrapper item = new ClientSharedSinglePlayerJoinableWorldWrapper(packet.CreatedWorld);
			this._services.SharedSinglePlayerJoinableWorlds.Add(item);
			this._app.Engine.RunOnMainThread(this, delegate
			{
				this._app.Interface.MainMenuView.SharedSinglePlayerPage.OnWorldsUpdated();
			}, false, false);
			ServicesPacketHandler.Logger.Info<ClientSSPJoinableWorld>("World was created: {0}!", packet.CreatedWorld);
			this._services.ProcessTypedCallback(packet.Token, packet);
		}

		// Token: 0x04003CD7 RID: 15575
		private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

		// Token: 0x04003CD8 RID: 15576
		private readonly BlockingCollection<Tuple<ProtoPacket, ServicesClient>> _packets = new BlockingCollection<Tuple<ProtoPacket, ServicesClient>>();

		// Token: 0x04003CD9 RID: 15577
		private readonly HashSet<string> _unhandledPacketTypes = new HashSet<string>();

		// Token: 0x04003CDA RID: 15578
		private readonly Thread _thread;

		// Token: 0x04003CDB RID: 15579
		private readonly CancellationTokenSource _threadCancellationTokenSource = new CancellationTokenSource();

		// Token: 0x04003CDC RID: 15580
		private readonly CancellationToken _threadCancellationToken;

		// Token: 0x04003CDD RID: 15581
		private readonly App _app;

		// Token: 0x04003CDE RID: 15582
		private readonly HytaleServices _services;
	}
}
