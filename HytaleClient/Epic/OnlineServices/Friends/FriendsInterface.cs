using System;

namespace Epic.OnlineServices.Friends
{
	// Token: 0x020004D7 RID: 1239
	public sealed class FriendsInterface : Handle
	{
		// Token: 0x0600203A RID: 8250 RVA: 0x0002F3A0 File Offset: 0x0002D5A0
		public FriendsInterface()
		{
		}

		// Token: 0x0600203B RID: 8251 RVA: 0x0002F3AA File Offset: 0x0002D5AA
		public FriendsInterface(IntPtr innerHandle) : base(innerHandle)
		{
		}

		// Token: 0x0600203C RID: 8252 RVA: 0x0002F3B8 File Offset: 0x0002D5B8
		public void AcceptInvite(ref AcceptInviteOptions options, object clientData, OnAcceptInviteCallback completionDelegate)
		{
			AcceptInviteOptionsInternal acceptInviteOptionsInternal = default(AcceptInviteOptionsInternal);
			acceptInviteOptionsInternal.Set(ref options);
			IntPtr zero = IntPtr.Zero;
			OnAcceptInviteCallbackInternal onAcceptInviteCallbackInternal = new OnAcceptInviteCallbackInternal(FriendsInterface.OnAcceptInviteCallbackInternalImplementation);
			Helper.AddCallback(out zero, clientData, completionDelegate, onAcceptInviteCallbackInternal, Array.Empty<Delegate>());
			Bindings.EOS_Friends_AcceptInvite(base.InnerHandle, ref acceptInviteOptionsInternal, zero, onAcceptInviteCallbackInternal);
			Helper.Dispose<AcceptInviteOptionsInternal>(ref acceptInviteOptionsInternal);
		}

		// Token: 0x0600203D RID: 8253 RVA: 0x0002F414 File Offset: 0x0002D614
		public ulong AddNotifyBlockedUsersUpdate(ref AddNotifyBlockedUsersUpdateOptions options, object clientData, OnBlockedUsersUpdateCallback blockedUsersUpdateHandler)
		{
			AddNotifyBlockedUsersUpdateOptionsInternal addNotifyBlockedUsersUpdateOptionsInternal = default(AddNotifyBlockedUsersUpdateOptionsInternal);
			addNotifyBlockedUsersUpdateOptionsInternal.Set(ref options);
			IntPtr zero = IntPtr.Zero;
			OnBlockedUsersUpdateCallbackInternal onBlockedUsersUpdateCallbackInternal = new OnBlockedUsersUpdateCallbackInternal(FriendsInterface.OnBlockedUsersUpdateCallbackInternalImplementation);
			Helper.AddCallback(out zero, clientData, blockedUsersUpdateHandler, onBlockedUsersUpdateCallbackInternal, Array.Empty<Delegate>());
			ulong num = Bindings.EOS_Friends_AddNotifyBlockedUsersUpdate(base.InnerHandle, ref addNotifyBlockedUsersUpdateOptionsInternal, zero, onBlockedUsersUpdateCallbackInternal);
			Helper.Dispose<AddNotifyBlockedUsersUpdateOptionsInternal>(ref addNotifyBlockedUsersUpdateOptionsInternal);
			Helper.AssignNotificationIdToCallback(zero, num);
			return num;
		}

		// Token: 0x0600203E RID: 8254 RVA: 0x0002F480 File Offset: 0x0002D680
		public ulong AddNotifyFriendsUpdate(ref AddNotifyFriendsUpdateOptions options, object clientData, OnFriendsUpdateCallback friendsUpdateHandler)
		{
			AddNotifyFriendsUpdateOptionsInternal addNotifyFriendsUpdateOptionsInternal = default(AddNotifyFriendsUpdateOptionsInternal);
			addNotifyFriendsUpdateOptionsInternal.Set(ref options);
			IntPtr zero = IntPtr.Zero;
			OnFriendsUpdateCallbackInternal onFriendsUpdateCallbackInternal = new OnFriendsUpdateCallbackInternal(FriendsInterface.OnFriendsUpdateCallbackInternalImplementation);
			Helper.AddCallback(out zero, clientData, friendsUpdateHandler, onFriendsUpdateCallbackInternal, Array.Empty<Delegate>());
			ulong num = Bindings.EOS_Friends_AddNotifyFriendsUpdate(base.InnerHandle, ref addNotifyFriendsUpdateOptionsInternal, zero, onFriendsUpdateCallbackInternal);
			Helper.Dispose<AddNotifyFriendsUpdateOptionsInternal>(ref addNotifyFriendsUpdateOptionsInternal);
			Helper.AssignNotificationIdToCallback(zero, num);
			return num;
		}

		// Token: 0x0600203F RID: 8255 RVA: 0x0002F4EC File Offset: 0x0002D6EC
		public EpicAccountId GetBlockedUserAtIndex(ref GetBlockedUserAtIndexOptions options)
		{
			GetBlockedUserAtIndexOptionsInternal getBlockedUserAtIndexOptionsInternal = default(GetBlockedUserAtIndexOptionsInternal);
			getBlockedUserAtIndexOptionsInternal.Set(ref options);
			IntPtr from = Bindings.EOS_Friends_GetBlockedUserAtIndex(base.InnerHandle, ref getBlockedUserAtIndexOptionsInternal);
			Helper.Dispose<GetBlockedUserAtIndexOptionsInternal>(ref getBlockedUserAtIndexOptionsInternal);
			EpicAccountId result;
			Helper.Get<EpicAccountId>(from, out result);
			return result;
		}

		// Token: 0x06002040 RID: 8256 RVA: 0x0002F530 File Offset: 0x0002D730
		public int GetBlockedUsersCount(ref GetBlockedUsersCountOptions options)
		{
			GetBlockedUsersCountOptionsInternal getBlockedUsersCountOptionsInternal = default(GetBlockedUsersCountOptionsInternal);
			getBlockedUsersCountOptionsInternal.Set(ref options);
			int result = Bindings.EOS_Friends_GetBlockedUsersCount(base.InnerHandle, ref getBlockedUsersCountOptionsInternal);
			Helper.Dispose<GetBlockedUsersCountOptionsInternal>(ref getBlockedUsersCountOptionsInternal);
			return result;
		}

		// Token: 0x06002041 RID: 8257 RVA: 0x0002F56C File Offset: 0x0002D76C
		public EpicAccountId GetFriendAtIndex(ref GetFriendAtIndexOptions options)
		{
			GetFriendAtIndexOptionsInternal getFriendAtIndexOptionsInternal = default(GetFriendAtIndexOptionsInternal);
			getFriendAtIndexOptionsInternal.Set(ref options);
			IntPtr from = Bindings.EOS_Friends_GetFriendAtIndex(base.InnerHandle, ref getFriendAtIndexOptionsInternal);
			Helper.Dispose<GetFriendAtIndexOptionsInternal>(ref getFriendAtIndexOptionsInternal);
			EpicAccountId result;
			Helper.Get<EpicAccountId>(from, out result);
			return result;
		}

		// Token: 0x06002042 RID: 8258 RVA: 0x0002F5B0 File Offset: 0x0002D7B0
		public int GetFriendsCount(ref GetFriendsCountOptions options)
		{
			GetFriendsCountOptionsInternal getFriendsCountOptionsInternal = default(GetFriendsCountOptionsInternal);
			getFriendsCountOptionsInternal.Set(ref options);
			int result = Bindings.EOS_Friends_GetFriendsCount(base.InnerHandle, ref getFriendsCountOptionsInternal);
			Helper.Dispose<GetFriendsCountOptionsInternal>(ref getFriendsCountOptionsInternal);
			return result;
		}

		// Token: 0x06002043 RID: 8259 RVA: 0x0002F5EC File Offset: 0x0002D7EC
		public FriendsStatus GetStatus(ref GetStatusOptions options)
		{
			GetStatusOptionsInternal getStatusOptionsInternal = default(GetStatusOptionsInternal);
			getStatusOptionsInternal.Set(ref options);
			FriendsStatus result = Bindings.EOS_Friends_GetStatus(base.InnerHandle, ref getStatusOptionsInternal);
			Helper.Dispose<GetStatusOptionsInternal>(ref getStatusOptionsInternal);
			return result;
		}

		// Token: 0x06002044 RID: 8260 RVA: 0x0002F628 File Offset: 0x0002D828
		public void QueryFriends(ref QueryFriendsOptions options, object clientData, OnQueryFriendsCallback completionDelegate)
		{
			QueryFriendsOptionsInternal queryFriendsOptionsInternal = default(QueryFriendsOptionsInternal);
			queryFriendsOptionsInternal.Set(ref options);
			IntPtr zero = IntPtr.Zero;
			OnQueryFriendsCallbackInternal onQueryFriendsCallbackInternal = new OnQueryFriendsCallbackInternal(FriendsInterface.OnQueryFriendsCallbackInternalImplementation);
			Helper.AddCallback(out zero, clientData, completionDelegate, onQueryFriendsCallbackInternal, Array.Empty<Delegate>());
			Bindings.EOS_Friends_QueryFriends(base.InnerHandle, ref queryFriendsOptionsInternal, zero, onQueryFriendsCallbackInternal);
			Helper.Dispose<QueryFriendsOptionsInternal>(ref queryFriendsOptionsInternal);
		}

		// Token: 0x06002045 RID: 8261 RVA: 0x0002F684 File Offset: 0x0002D884
		public void RejectInvite(ref RejectInviteOptions options, object clientData, OnRejectInviteCallback completionDelegate)
		{
			RejectInviteOptionsInternal rejectInviteOptionsInternal = default(RejectInviteOptionsInternal);
			rejectInviteOptionsInternal.Set(ref options);
			IntPtr zero = IntPtr.Zero;
			OnRejectInviteCallbackInternal onRejectInviteCallbackInternal = new OnRejectInviteCallbackInternal(FriendsInterface.OnRejectInviteCallbackInternalImplementation);
			Helper.AddCallback(out zero, clientData, completionDelegate, onRejectInviteCallbackInternal, Array.Empty<Delegate>());
			Bindings.EOS_Friends_RejectInvite(base.InnerHandle, ref rejectInviteOptionsInternal, zero, onRejectInviteCallbackInternal);
			Helper.Dispose<RejectInviteOptionsInternal>(ref rejectInviteOptionsInternal);
		}

		// Token: 0x06002046 RID: 8262 RVA: 0x0002F6DE File Offset: 0x0002D8DE
		public void RemoveNotifyBlockedUsersUpdate(ulong notificationId)
		{
			Bindings.EOS_Friends_RemoveNotifyBlockedUsersUpdate(base.InnerHandle, notificationId);
			Helper.RemoveCallbackByNotificationId(notificationId);
		}

		// Token: 0x06002047 RID: 8263 RVA: 0x0002F6F5 File Offset: 0x0002D8F5
		public void RemoveNotifyFriendsUpdate(ulong notificationId)
		{
			Bindings.EOS_Friends_RemoveNotifyFriendsUpdate(base.InnerHandle, notificationId);
			Helper.RemoveCallbackByNotificationId(notificationId);
		}

		// Token: 0x06002048 RID: 8264 RVA: 0x0002F70C File Offset: 0x0002D90C
		public void SendInvite(ref SendInviteOptions options, object clientData, OnSendInviteCallback completionDelegate)
		{
			SendInviteOptionsInternal sendInviteOptionsInternal = default(SendInviteOptionsInternal);
			sendInviteOptionsInternal.Set(ref options);
			IntPtr zero = IntPtr.Zero;
			OnSendInviteCallbackInternal onSendInviteCallbackInternal = new OnSendInviteCallbackInternal(FriendsInterface.OnSendInviteCallbackInternalImplementation);
			Helper.AddCallback(out zero, clientData, completionDelegate, onSendInviteCallbackInternal, Array.Empty<Delegate>());
			Bindings.EOS_Friends_SendInvite(base.InnerHandle, ref sendInviteOptionsInternal, zero, onSendInviteCallbackInternal);
			Helper.Dispose<SendInviteOptionsInternal>(ref sendInviteOptionsInternal);
		}

		// Token: 0x06002049 RID: 8265 RVA: 0x0002F768 File Offset: 0x0002D968
		[MonoPInvokeCallback(typeof(OnAcceptInviteCallbackInternal))]
		internal static void OnAcceptInviteCallbackInternalImplementation(ref AcceptInviteCallbackInfoInternal data)
		{
			OnAcceptInviteCallback onAcceptInviteCallback;
			AcceptInviteCallbackInfo acceptInviteCallbackInfo;
			bool flag = Helper.TryGetAndRemoveCallback<AcceptInviteCallbackInfoInternal, OnAcceptInviteCallback, AcceptInviteCallbackInfo>(ref data, out onAcceptInviteCallback, out acceptInviteCallbackInfo);
			if (flag)
			{
				onAcceptInviteCallback(ref acceptInviteCallbackInfo);
			}
		}

		// Token: 0x0600204A RID: 8266 RVA: 0x0002F790 File Offset: 0x0002D990
		[MonoPInvokeCallback(typeof(OnBlockedUsersUpdateCallbackInternal))]
		internal static void OnBlockedUsersUpdateCallbackInternalImplementation(ref OnBlockedUsersUpdateInfoInternal data)
		{
			OnBlockedUsersUpdateCallback onBlockedUsersUpdateCallback;
			OnBlockedUsersUpdateInfo onBlockedUsersUpdateInfo;
			bool flag = Helper.TryGetCallback<OnBlockedUsersUpdateInfoInternal, OnBlockedUsersUpdateCallback, OnBlockedUsersUpdateInfo>(ref data, out onBlockedUsersUpdateCallback, out onBlockedUsersUpdateInfo);
			if (flag)
			{
				onBlockedUsersUpdateCallback(ref onBlockedUsersUpdateInfo);
			}
		}

		// Token: 0x0600204B RID: 8267 RVA: 0x0002F7B8 File Offset: 0x0002D9B8
		[MonoPInvokeCallback(typeof(OnFriendsUpdateCallbackInternal))]
		internal static void OnFriendsUpdateCallbackInternalImplementation(ref OnFriendsUpdateInfoInternal data)
		{
			OnFriendsUpdateCallback onFriendsUpdateCallback;
			OnFriendsUpdateInfo onFriendsUpdateInfo;
			bool flag = Helper.TryGetCallback<OnFriendsUpdateInfoInternal, OnFriendsUpdateCallback, OnFriendsUpdateInfo>(ref data, out onFriendsUpdateCallback, out onFriendsUpdateInfo);
			if (flag)
			{
				onFriendsUpdateCallback(ref onFriendsUpdateInfo);
			}
		}

		// Token: 0x0600204C RID: 8268 RVA: 0x0002F7E0 File Offset: 0x0002D9E0
		[MonoPInvokeCallback(typeof(OnQueryFriendsCallbackInternal))]
		internal static void OnQueryFriendsCallbackInternalImplementation(ref QueryFriendsCallbackInfoInternal data)
		{
			OnQueryFriendsCallback onQueryFriendsCallback;
			QueryFriendsCallbackInfo queryFriendsCallbackInfo;
			bool flag = Helper.TryGetAndRemoveCallback<QueryFriendsCallbackInfoInternal, OnQueryFriendsCallback, QueryFriendsCallbackInfo>(ref data, out onQueryFriendsCallback, out queryFriendsCallbackInfo);
			if (flag)
			{
				onQueryFriendsCallback(ref queryFriendsCallbackInfo);
			}
		}

		// Token: 0x0600204D RID: 8269 RVA: 0x0002F808 File Offset: 0x0002DA08
		[MonoPInvokeCallback(typeof(OnRejectInviteCallbackInternal))]
		internal static void OnRejectInviteCallbackInternalImplementation(ref RejectInviteCallbackInfoInternal data)
		{
			OnRejectInviteCallback onRejectInviteCallback;
			RejectInviteCallbackInfo rejectInviteCallbackInfo;
			bool flag = Helper.TryGetAndRemoveCallback<RejectInviteCallbackInfoInternal, OnRejectInviteCallback, RejectInviteCallbackInfo>(ref data, out onRejectInviteCallback, out rejectInviteCallbackInfo);
			if (flag)
			{
				onRejectInviteCallback(ref rejectInviteCallbackInfo);
			}
		}

		// Token: 0x0600204E RID: 8270 RVA: 0x0002F830 File Offset: 0x0002DA30
		[MonoPInvokeCallback(typeof(OnSendInviteCallbackInternal))]
		internal static void OnSendInviteCallbackInternalImplementation(ref SendInviteCallbackInfoInternal data)
		{
			OnSendInviteCallback onSendInviteCallback;
			SendInviteCallbackInfo sendInviteCallbackInfo;
			bool flag = Helper.TryGetAndRemoveCallback<SendInviteCallbackInfoInternal, OnSendInviteCallback, SendInviteCallbackInfo>(ref data, out onSendInviteCallback, out sendInviteCallbackInfo);
			if (flag)
			{
				onSendInviteCallback(ref sendInviteCallbackInfo);
			}
		}

		// Token: 0x04000E04 RID: 3588
		public const int AcceptinviteApiLatest = 1;

		// Token: 0x04000E05 RID: 3589
		public const int AddnotifyblockedusersupdateApiLatest = 1;

		// Token: 0x04000E06 RID: 3590
		public const int AddnotifyfriendsupdateApiLatest = 1;

		// Token: 0x04000E07 RID: 3591
		public const int GetblockeduseratindexApiLatest = 1;

		// Token: 0x04000E08 RID: 3592
		public const int GetblockeduserscountApiLatest = 1;

		// Token: 0x04000E09 RID: 3593
		public const int GetfriendatindexApiLatest = 1;

		// Token: 0x04000E0A RID: 3594
		public const int GetfriendscountApiLatest = 1;

		// Token: 0x04000E0B RID: 3595
		public const int GetstatusApiLatest = 1;

		// Token: 0x04000E0C RID: 3596
		public const int QueryfriendsApiLatest = 1;

		// Token: 0x04000E0D RID: 3597
		public const int RejectinviteApiLatest = 1;

		// Token: 0x04000E0E RID: 3598
		public const int SendinviteApiLatest = 1;
	}
}
