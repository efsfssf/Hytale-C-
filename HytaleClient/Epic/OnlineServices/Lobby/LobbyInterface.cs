using System;

namespace Epic.OnlineServices.Lobby
{
	// Token: 0x020003CF RID: 975
	public sealed class LobbyInterface : Handle
	{
		// Token: 0x06001A23 RID: 6691 RVA: 0x00026751 File Offset: 0x00024951
		public LobbyInterface()
		{
		}

		// Token: 0x06001A24 RID: 6692 RVA: 0x0002675B File Offset: 0x0002495B
		public LobbyInterface(IntPtr innerHandle) : base(innerHandle)
		{
		}

		// Token: 0x06001A25 RID: 6693 RVA: 0x00026768 File Offset: 0x00024968
		public ulong AddNotifyJoinLobbyAccepted(ref AddNotifyJoinLobbyAcceptedOptions options, object clientData, OnJoinLobbyAcceptedCallback notificationFn)
		{
			AddNotifyJoinLobbyAcceptedOptionsInternal addNotifyJoinLobbyAcceptedOptionsInternal = default(AddNotifyJoinLobbyAcceptedOptionsInternal);
			addNotifyJoinLobbyAcceptedOptionsInternal.Set(ref options);
			IntPtr zero = IntPtr.Zero;
			OnJoinLobbyAcceptedCallbackInternal onJoinLobbyAcceptedCallbackInternal = new OnJoinLobbyAcceptedCallbackInternal(LobbyInterface.OnJoinLobbyAcceptedCallbackInternalImplementation);
			Helper.AddCallback(out zero, clientData, notificationFn, onJoinLobbyAcceptedCallbackInternal, Array.Empty<Delegate>());
			ulong num = Bindings.EOS_Lobby_AddNotifyJoinLobbyAccepted(base.InnerHandle, ref addNotifyJoinLobbyAcceptedOptionsInternal, zero, onJoinLobbyAcceptedCallbackInternal);
			Helper.Dispose<AddNotifyJoinLobbyAcceptedOptionsInternal>(ref addNotifyJoinLobbyAcceptedOptionsInternal);
			Helper.AssignNotificationIdToCallback(zero, num);
			return num;
		}

		// Token: 0x06001A26 RID: 6694 RVA: 0x000267D4 File Offset: 0x000249D4
		public ulong AddNotifyLeaveLobbyRequested(ref AddNotifyLeaveLobbyRequestedOptions options, object clientData, OnLeaveLobbyRequestedCallback notificationFn)
		{
			AddNotifyLeaveLobbyRequestedOptionsInternal addNotifyLeaveLobbyRequestedOptionsInternal = default(AddNotifyLeaveLobbyRequestedOptionsInternal);
			addNotifyLeaveLobbyRequestedOptionsInternal.Set(ref options);
			IntPtr zero = IntPtr.Zero;
			OnLeaveLobbyRequestedCallbackInternal onLeaveLobbyRequestedCallbackInternal = new OnLeaveLobbyRequestedCallbackInternal(LobbyInterface.OnLeaveLobbyRequestedCallbackInternalImplementation);
			Helper.AddCallback(out zero, clientData, notificationFn, onLeaveLobbyRequestedCallbackInternal, Array.Empty<Delegate>());
			ulong num = Bindings.EOS_Lobby_AddNotifyLeaveLobbyRequested(base.InnerHandle, ref addNotifyLeaveLobbyRequestedOptionsInternal, zero, onLeaveLobbyRequestedCallbackInternal);
			Helper.Dispose<AddNotifyLeaveLobbyRequestedOptionsInternal>(ref addNotifyLeaveLobbyRequestedOptionsInternal);
			Helper.AssignNotificationIdToCallback(zero, num);
			return num;
		}

		// Token: 0x06001A27 RID: 6695 RVA: 0x00026840 File Offset: 0x00024A40
		public ulong AddNotifyLobbyInviteAccepted(ref AddNotifyLobbyInviteAcceptedOptions options, object clientData, OnLobbyInviteAcceptedCallback notificationFn)
		{
			AddNotifyLobbyInviteAcceptedOptionsInternal addNotifyLobbyInviteAcceptedOptionsInternal = default(AddNotifyLobbyInviteAcceptedOptionsInternal);
			addNotifyLobbyInviteAcceptedOptionsInternal.Set(ref options);
			IntPtr zero = IntPtr.Zero;
			OnLobbyInviteAcceptedCallbackInternal onLobbyInviteAcceptedCallbackInternal = new OnLobbyInviteAcceptedCallbackInternal(LobbyInterface.OnLobbyInviteAcceptedCallbackInternalImplementation);
			Helper.AddCallback(out zero, clientData, notificationFn, onLobbyInviteAcceptedCallbackInternal, Array.Empty<Delegate>());
			ulong num = Bindings.EOS_Lobby_AddNotifyLobbyInviteAccepted(base.InnerHandle, ref addNotifyLobbyInviteAcceptedOptionsInternal, zero, onLobbyInviteAcceptedCallbackInternal);
			Helper.Dispose<AddNotifyLobbyInviteAcceptedOptionsInternal>(ref addNotifyLobbyInviteAcceptedOptionsInternal);
			Helper.AssignNotificationIdToCallback(zero, num);
			return num;
		}

		// Token: 0x06001A28 RID: 6696 RVA: 0x000268AC File Offset: 0x00024AAC
		public ulong AddNotifyLobbyInviteReceived(ref AddNotifyLobbyInviteReceivedOptions options, object clientData, OnLobbyInviteReceivedCallback notificationFn)
		{
			AddNotifyLobbyInviteReceivedOptionsInternal addNotifyLobbyInviteReceivedOptionsInternal = default(AddNotifyLobbyInviteReceivedOptionsInternal);
			addNotifyLobbyInviteReceivedOptionsInternal.Set(ref options);
			IntPtr zero = IntPtr.Zero;
			OnLobbyInviteReceivedCallbackInternal onLobbyInviteReceivedCallbackInternal = new OnLobbyInviteReceivedCallbackInternal(LobbyInterface.OnLobbyInviteReceivedCallbackInternalImplementation);
			Helper.AddCallback(out zero, clientData, notificationFn, onLobbyInviteReceivedCallbackInternal, Array.Empty<Delegate>());
			ulong num = Bindings.EOS_Lobby_AddNotifyLobbyInviteReceived(base.InnerHandle, ref addNotifyLobbyInviteReceivedOptionsInternal, zero, onLobbyInviteReceivedCallbackInternal);
			Helper.Dispose<AddNotifyLobbyInviteReceivedOptionsInternal>(ref addNotifyLobbyInviteReceivedOptionsInternal);
			Helper.AssignNotificationIdToCallback(zero, num);
			return num;
		}

		// Token: 0x06001A29 RID: 6697 RVA: 0x00026918 File Offset: 0x00024B18
		public ulong AddNotifyLobbyInviteRejected(ref AddNotifyLobbyInviteRejectedOptions options, object clientData, OnLobbyInviteRejectedCallback notificationFn)
		{
			AddNotifyLobbyInviteRejectedOptionsInternal addNotifyLobbyInviteRejectedOptionsInternal = default(AddNotifyLobbyInviteRejectedOptionsInternal);
			addNotifyLobbyInviteRejectedOptionsInternal.Set(ref options);
			IntPtr zero = IntPtr.Zero;
			OnLobbyInviteRejectedCallbackInternal onLobbyInviteRejectedCallbackInternal = new OnLobbyInviteRejectedCallbackInternal(LobbyInterface.OnLobbyInviteRejectedCallbackInternalImplementation);
			Helper.AddCallback(out zero, clientData, notificationFn, onLobbyInviteRejectedCallbackInternal, Array.Empty<Delegate>());
			ulong num = Bindings.EOS_Lobby_AddNotifyLobbyInviteRejected(base.InnerHandle, ref addNotifyLobbyInviteRejectedOptionsInternal, zero, onLobbyInviteRejectedCallbackInternal);
			Helper.Dispose<AddNotifyLobbyInviteRejectedOptionsInternal>(ref addNotifyLobbyInviteRejectedOptionsInternal);
			Helper.AssignNotificationIdToCallback(zero, num);
			return num;
		}

		// Token: 0x06001A2A RID: 6698 RVA: 0x00026984 File Offset: 0x00024B84
		public ulong AddNotifyLobbyMemberStatusReceived(ref AddNotifyLobbyMemberStatusReceivedOptions options, object clientData, OnLobbyMemberStatusReceivedCallback notificationFn)
		{
			AddNotifyLobbyMemberStatusReceivedOptionsInternal addNotifyLobbyMemberStatusReceivedOptionsInternal = default(AddNotifyLobbyMemberStatusReceivedOptionsInternal);
			addNotifyLobbyMemberStatusReceivedOptionsInternal.Set(ref options);
			IntPtr zero = IntPtr.Zero;
			OnLobbyMemberStatusReceivedCallbackInternal onLobbyMemberStatusReceivedCallbackInternal = new OnLobbyMemberStatusReceivedCallbackInternal(LobbyInterface.OnLobbyMemberStatusReceivedCallbackInternalImplementation);
			Helper.AddCallback(out zero, clientData, notificationFn, onLobbyMemberStatusReceivedCallbackInternal, Array.Empty<Delegate>());
			ulong num = Bindings.EOS_Lobby_AddNotifyLobbyMemberStatusReceived(base.InnerHandle, ref addNotifyLobbyMemberStatusReceivedOptionsInternal, zero, onLobbyMemberStatusReceivedCallbackInternal);
			Helper.Dispose<AddNotifyLobbyMemberStatusReceivedOptionsInternal>(ref addNotifyLobbyMemberStatusReceivedOptionsInternal);
			Helper.AssignNotificationIdToCallback(zero, num);
			return num;
		}

		// Token: 0x06001A2B RID: 6699 RVA: 0x000269F0 File Offset: 0x00024BF0
		public ulong AddNotifyLobbyMemberUpdateReceived(ref AddNotifyLobbyMemberUpdateReceivedOptions options, object clientData, OnLobbyMemberUpdateReceivedCallback notificationFn)
		{
			AddNotifyLobbyMemberUpdateReceivedOptionsInternal addNotifyLobbyMemberUpdateReceivedOptionsInternal = default(AddNotifyLobbyMemberUpdateReceivedOptionsInternal);
			addNotifyLobbyMemberUpdateReceivedOptionsInternal.Set(ref options);
			IntPtr zero = IntPtr.Zero;
			OnLobbyMemberUpdateReceivedCallbackInternal onLobbyMemberUpdateReceivedCallbackInternal = new OnLobbyMemberUpdateReceivedCallbackInternal(LobbyInterface.OnLobbyMemberUpdateReceivedCallbackInternalImplementation);
			Helper.AddCallback(out zero, clientData, notificationFn, onLobbyMemberUpdateReceivedCallbackInternal, Array.Empty<Delegate>());
			ulong num = Bindings.EOS_Lobby_AddNotifyLobbyMemberUpdateReceived(base.InnerHandle, ref addNotifyLobbyMemberUpdateReceivedOptionsInternal, zero, onLobbyMemberUpdateReceivedCallbackInternal);
			Helper.Dispose<AddNotifyLobbyMemberUpdateReceivedOptionsInternal>(ref addNotifyLobbyMemberUpdateReceivedOptionsInternal);
			Helper.AssignNotificationIdToCallback(zero, num);
			return num;
		}

		// Token: 0x06001A2C RID: 6700 RVA: 0x00026A5C File Offset: 0x00024C5C
		public ulong AddNotifyLobbyUpdateReceived(ref AddNotifyLobbyUpdateReceivedOptions options, object clientData, OnLobbyUpdateReceivedCallback notificationFn)
		{
			AddNotifyLobbyUpdateReceivedOptionsInternal addNotifyLobbyUpdateReceivedOptionsInternal = default(AddNotifyLobbyUpdateReceivedOptionsInternal);
			addNotifyLobbyUpdateReceivedOptionsInternal.Set(ref options);
			IntPtr zero = IntPtr.Zero;
			OnLobbyUpdateReceivedCallbackInternal onLobbyUpdateReceivedCallbackInternal = new OnLobbyUpdateReceivedCallbackInternal(LobbyInterface.OnLobbyUpdateReceivedCallbackInternalImplementation);
			Helper.AddCallback(out zero, clientData, notificationFn, onLobbyUpdateReceivedCallbackInternal, Array.Empty<Delegate>());
			ulong num = Bindings.EOS_Lobby_AddNotifyLobbyUpdateReceived(base.InnerHandle, ref addNotifyLobbyUpdateReceivedOptionsInternal, zero, onLobbyUpdateReceivedCallbackInternal);
			Helper.Dispose<AddNotifyLobbyUpdateReceivedOptionsInternal>(ref addNotifyLobbyUpdateReceivedOptionsInternal);
			Helper.AssignNotificationIdToCallback(zero, num);
			return num;
		}

		// Token: 0x06001A2D RID: 6701 RVA: 0x00026AC8 File Offset: 0x00024CC8
		public ulong AddNotifyRTCRoomConnectionChanged(ref AddNotifyRTCRoomConnectionChangedOptions options, object clientData, OnRTCRoomConnectionChangedCallback notificationFn)
		{
			AddNotifyRTCRoomConnectionChangedOptionsInternal addNotifyRTCRoomConnectionChangedOptionsInternal = default(AddNotifyRTCRoomConnectionChangedOptionsInternal);
			addNotifyRTCRoomConnectionChangedOptionsInternal.Set(ref options);
			IntPtr zero = IntPtr.Zero;
			OnRTCRoomConnectionChangedCallbackInternal onRTCRoomConnectionChangedCallbackInternal = new OnRTCRoomConnectionChangedCallbackInternal(LobbyInterface.OnRTCRoomConnectionChangedCallbackInternalImplementation);
			Helper.AddCallback(out zero, clientData, notificationFn, onRTCRoomConnectionChangedCallbackInternal, Array.Empty<Delegate>());
			ulong num = Bindings.EOS_Lobby_AddNotifyRTCRoomConnectionChanged(base.InnerHandle, ref addNotifyRTCRoomConnectionChangedOptionsInternal, zero, onRTCRoomConnectionChangedCallbackInternal);
			Helper.Dispose<AddNotifyRTCRoomConnectionChangedOptionsInternal>(ref addNotifyRTCRoomConnectionChangedOptionsInternal);
			Helper.AssignNotificationIdToCallback(zero, num);
			return num;
		}

		// Token: 0x06001A2E RID: 6702 RVA: 0x00026B34 File Offset: 0x00024D34
		public ulong AddNotifySendLobbyNativeInviteRequested(ref AddNotifySendLobbyNativeInviteRequestedOptions options, object clientData, OnSendLobbyNativeInviteRequestedCallback notificationFn)
		{
			AddNotifySendLobbyNativeInviteRequestedOptionsInternal addNotifySendLobbyNativeInviteRequestedOptionsInternal = default(AddNotifySendLobbyNativeInviteRequestedOptionsInternal);
			addNotifySendLobbyNativeInviteRequestedOptionsInternal.Set(ref options);
			IntPtr zero = IntPtr.Zero;
			OnSendLobbyNativeInviteRequestedCallbackInternal onSendLobbyNativeInviteRequestedCallbackInternal = new OnSendLobbyNativeInviteRequestedCallbackInternal(LobbyInterface.OnSendLobbyNativeInviteRequestedCallbackInternalImplementation);
			Helper.AddCallback(out zero, clientData, notificationFn, onSendLobbyNativeInviteRequestedCallbackInternal, Array.Empty<Delegate>());
			ulong num = Bindings.EOS_Lobby_AddNotifySendLobbyNativeInviteRequested(base.InnerHandle, ref addNotifySendLobbyNativeInviteRequestedOptionsInternal, zero, onSendLobbyNativeInviteRequestedCallbackInternal);
			Helper.Dispose<AddNotifySendLobbyNativeInviteRequestedOptionsInternal>(ref addNotifySendLobbyNativeInviteRequestedOptionsInternal);
			Helper.AssignNotificationIdToCallback(zero, num);
			return num;
		}

		// Token: 0x06001A2F RID: 6703 RVA: 0x00026BA0 File Offset: 0x00024DA0
		public Result CopyLobbyDetailsHandle(ref CopyLobbyDetailsHandleOptions options, out LobbyDetails outLobbyDetailsHandle)
		{
			CopyLobbyDetailsHandleOptionsInternal copyLobbyDetailsHandleOptionsInternal = default(CopyLobbyDetailsHandleOptionsInternal);
			copyLobbyDetailsHandleOptionsInternal.Set(ref options);
			IntPtr zero = IntPtr.Zero;
			Result result = Bindings.EOS_Lobby_CopyLobbyDetailsHandle(base.InnerHandle, ref copyLobbyDetailsHandleOptionsInternal, ref zero);
			Helper.Dispose<CopyLobbyDetailsHandleOptionsInternal>(ref copyLobbyDetailsHandleOptionsInternal);
			Helper.Get<LobbyDetails>(zero, out outLobbyDetailsHandle);
			return result;
		}

		// Token: 0x06001A30 RID: 6704 RVA: 0x00026BEC File Offset: 0x00024DEC
		public Result CopyLobbyDetailsHandleByInviteId(ref CopyLobbyDetailsHandleByInviteIdOptions options, out LobbyDetails outLobbyDetailsHandle)
		{
			CopyLobbyDetailsHandleByInviteIdOptionsInternal copyLobbyDetailsHandleByInviteIdOptionsInternal = default(CopyLobbyDetailsHandleByInviteIdOptionsInternal);
			copyLobbyDetailsHandleByInviteIdOptionsInternal.Set(ref options);
			IntPtr zero = IntPtr.Zero;
			Result result = Bindings.EOS_Lobby_CopyLobbyDetailsHandleByInviteId(base.InnerHandle, ref copyLobbyDetailsHandleByInviteIdOptionsInternal, ref zero);
			Helper.Dispose<CopyLobbyDetailsHandleByInviteIdOptionsInternal>(ref copyLobbyDetailsHandleByInviteIdOptionsInternal);
			Helper.Get<LobbyDetails>(zero, out outLobbyDetailsHandle);
			return result;
		}

		// Token: 0x06001A31 RID: 6705 RVA: 0x00026C38 File Offset: 0x00024E38
		public Result CopyLobbyDetailsHandleByUiEventId(ref CopyLobbyDetailsHandleByUiEventIdOptions options, out LobbyDetails outLobbyDetailsHandle)
		{
			CopyLobbyDetailsHandleByUiEventIdOptionsInternal copyLobbyDetailsHandleByUiEventIdOptionsInternal = default(CopyLobbyDetailsHandleByUiEventIdOptionsInternal);
			copyLobbyDetailsHandleByUiEventIdOptionsInternal.Set(ref options);
			IntPtr zero = IntPtr.Zero;
			Result result = Bindings.EOS_Lobby_CopyLobbyDetailsHandleByUiEventId(base.InnerHandle, ref copyLobbyDetailsHandleByUiEventIdOptionsInternal, ref zero);
			Helper.Dispose<CopyLobbyDetailsHandleByUiEventIdOptionsInternal>(ref copyLobbyDetailsHandleByUiEventIdOptionsInternal);
			Helper.Get<LobbyDetails>(zero, out outLobbyDetailsHandle);
			return result;
		}

		// Token: 0x06001A32 RID: 6706 RVA: 0x00026C84 File Offset: 0x00024E84
		public void CreateLobby(ref CreateLobbyOptions options, object clientData, OnCreateLobbyCallback completionDelegate)
		{
			CreateLobbyOptionsInternal createLobbyOptionsInternal = default(CreateLobbyOptionsInternal);
			createLobbyOptionsInternal.Set(ref options);
			IntPtr zero = IntPtr.Zero;
			OnCreateLobbyCallbackInternal onCreateLobbyCallbackInternal = new OnCreateLobbyCallbackInternal(LobbyInterface.OnCreateLobbyCallbackInternalImplementation);
			Helper.AddCallback(out zero, clientData, completionDelegate, onCreateLobbyCallbackInternal, Array.Empty<Delegate>());
			Bindings.EOS_Lobby_CreateLobby(base.InnerHandle, ref createLobbyOptionsInternal, zero, onCreateLobbyCallbackInternal);
			Helper.Dispose<CreateLobbyOptionsInternal>(ref createLobbyOptionsInternal);
		}

		// Token: 0x06001A33 RID: 6707 RVA: 0x00026CE0 File Offset: 0x00024EE0
		public Result CreateLobbySearch(ref CreateLobbySearchOptions options, out LobbySearch outLobbySearchHandle)
		{
			CreateLobbySearchOptionsInternal createLobbySearchOptionsInternal = default(CreateLobbySearchOptionsInternal);
			createLobbySearchOptionsInternal.Set(ref options);
			IntPtr zero = IntPtr.Zero;
			Result result = Bindings.EOS_Lobby_CreateLobbySearch(base.InnerHandle, ref createLobbySearchOptionsInternal, ref zero);
			Helper.Dispose<CreateLobbySearchOptionsInternal>(ref createLobbySearchOptionsInternal);
			Helper.Get<LobbySearch>(zero, out outLobbySearchHandle);
			return result;
		}

		// Token: 0x06001A34 RID: 6708 RVA: 0x00026D2C File Offset: 0x00024F2C
		public void DestroyLobby(ref DestroyLobbyOptions options, object clientData, OnDestroyLobbyCallback completionDelegate)
		{
			DestroyLobbyOptionsInternal destroyLobbyOptionsInternal = default(DestroyLobbyOptionsInternal);
			destroyLobbyOptionsInternal.Set(ref options);
			IntPtr zero = IntPtr.Zero;
			OnDestroyLobbyCallbackInternal onDestroyLobbyCallbackInternal = new OnDestroyLobbyCallbackInternal(LobbyInterface.OnDestroyLobbyCallbackInternalImplementation);
			Helper.AddCallback(out zero, clientData, completionDelegate, onDestroyLobbyCallbackInternal, Array.Empty<Delegate>());
			Bindings.EOS_Lobby_DestroyLobby(base.InnerHandle, ref destroyLobbyOptionsInternal, zero, onDestroyLobbyCallbackInternal);
			Helper.Dispose<DestroyLobbyOptionsInternal>(ref destroyLobbyOptionsInternal);
		}

		// Token: 0x06001A35 RID: 6709 RVA: 0x00026D88 File Offset: 0x00024F88
		public Result GetConnectString(ref GetConnectStringOptions options, out Utf8String outBuffer)
		{
			GetConnectStringOptionsInternal getConnectStringOptionsInternal = default(GetConnectStringOptionsInternal);
			getConnectStringOptionsInternal.Set(ref options);
			uint size = 256U;
			IntPtr intPtr = Helper.AddAllocation(size);
			Result result = Bindings.EOS_Lobby_GetConnectString(base.InnerHandle, ref getConnectStringOptionsInternal, intPtr, ref size);
			Helper.Dispose<GetConnectStringOptionsInternal>(ref getConnectStringOptionsInternal);
			Helper.Get(intPtr, out outBuffer);
			Helper.Dispose(ref intPtr);
			return result;
		}

		// Token: 0x06001A36 RID: 6710 RVA: 0x00026DE4 File Offset: 0x00024FE4
		public uint GetInviteCount(ref GetInviteCountOptions options)
		{
			GetInviteCountOptionsInternal getInviteCountOptionsInternal = default(GetInviteCountOptionsInternal);
			getInviteCountOptionsInternal.Set(ref options);
			uint result = Bindings.EOS_Lobby_GetInviteCount(base.InnerHandle, ref getInviteCountOptionsInternal);
			Helper.Dispose<GetInviteCountOptionsInternal>(ref getInviteCountOptionsInternal);
			return result;
		}

		// Token: 0x06001A37 RID: 6711 RVA: 0x00026E20 File Offset: 0x00025020
		public Result GetInviteIdByIndex(ref GetInviteIdByIndexOptions options, out Utf8String outBuffer)
		{
			GetInviteIdByIndexOptionsInternal getInviteIdByIndexOptionsInternal = default(GetInviteIdByIndexOptionsInternal);
			getInviteIdByIndexOptionsInternal.Set(ref options);
			int size = 65;
			IntPtr intPtr = Helper.AddAllocation(size);
			Result result = Bindings.EOS_Lobby_GetInviteIdByIndex(base.InnerHandle, ref getInviteIdByIndexOptionsInternal, intPtr, ref size);
			Helper.Dispose<GetInviteIdByIndexOptionsInternal>(ref getInviteIdByIndexOptionsInternal);
			Helper.Get(intPtr, out outBuffer);
			Helper.Dispose(ref intPtr);
			return result;
		}

		// Token: 0x06001A38 RID: 6712 RVA: 0x00026E7C File Offset: 0x0002507C
		public Result GetRTCRoomName(ref GetRTCRoomNameOptions options, out Utf8String outBuffer)
		{
			GetRTCRoomNameOptionsInternal getRTCRoomNameOptionsInternal = default(GetRTCRoomNameOptionsInternal);
			getRTCRoomNameOptionsInternal.Set(ref options);
			uint size = 256U;
			IntPtr intPtr = Helper.AddAllocation(size);
			Result result = Bindings.EOS_Lobby_GetRTCRoomName(base.InnerHandle, ref getRTCRoomNameOptionsInternal, intPtr, ref size);
			Helper.Dispose<GetRTCRoomNameOptionsInternal>(ref getRTCRoomNameOptionsInternal);
			Helper.Get(intPtr, out outBuffer);
			Helper.Dispose(ref intPtr);
			return result;
		}

		// Token: 0x06001A39 RID: 6713 RVA: 0x00026ED8 File Offset: 0x000250D8
		public void HardMuteMember(ref HardMuteMemberOptions options, object clientData, OnHardMuteMemberCallback completionDelegate)
		{
			HardMuteMemberOptionsInternal hardMuteMemberOptionsInternal = default(HardMuteMemberOptionsInternal);
			hardMuteMemberOptionsInternal.Set(ref options);
			IntPtr zero = IntPtr.Zero;
			OnHardMuteMemberCallbackInternal onHardMuteMemberCallbackInternal = new OnHardMuteMemberCallbackInternal(LobbyInterface.OnHardMuteMemberCallbackInternalImplementation);
			Helper.AddCallback(out zero, clientData, completionDelegate, onHardMuteMemberCallbackInternal, Array.Empty<Delegate>());
			Bindings.EOS_Lobby_HardMuteMember(base.InnerHandle, ref hardMuteMemberOptionsInternal, zero, onHardMuteMemberCallbackInternal);
			Helper.Dispose<HardMuteMemberOptionsInternal>(ref hardMuteMemberOptionsInternal);
		}

		// Token: 0x06001A3A RID: 6714 RVA: 0x00026F34 File Offset: 0x00025134
		public Result IsRTCRoomConnected(ref IsRTCRoomConnectedOptions options, out bool bOutIsConnected)
		{
			IsRTCRoomConnectedOptionsInternal isRTCRoomConnectedOptionsInternal = default(IsRTCRoomConnectedOptionsInternal);
			isRTCRoomConnectedOptionsInternal.Set(ref options);
			int from = 0;
			Result result = Bindings.EOS_Lobby_IsRTCRoomConnected(base.InnerHandle, ref isRTCRoomConnectedOptionsInternal, ref from);
			Helper.Dispose<IsRTCRoomConnectedOptionsInternal>(ref isRTCRoomConnectedOptionsInternal);
			Helper.Get(from, out bOutIsConnected);
			return result;
		}

		// Token: 0x06001A3B RID: 6715 RVA: 0x00026F7C File Offset: 0x0002517C
		public void JoinLobby(ref JoinLobbyOptions options, object clientData, OnJoinLobbyCallback completionDelegate)
		{
			JoinLobbyOptionsInternal joinLobbyOptionsInternal = default(JoinLobbyOptionsInternal);
			joinLobbyOptionsInternal.Set(ref options);
			IntPtr zero = IntPtr.Zero;
			OnJoinLobbyCallbackInternal onJoinLobbyCallbackInternal = new OnJoinLobbyCallbackInternal(LobbyInterface.OnJoinLobbyCallbackInternalImplementation);
			Helper.AddCallback(out zero, clientData, completionDelegate, onJoinLobbyCallbackInternal, Array.Empty<Delegate>());
			Bindings.EOS_Lobby_JoinLobby(base.InnerHandle, ref joinLobbyOptionsInternal, zero, onJoinLobbyCallbackInternal);
			Helper.Dispose<JoinLobbyOptionsInternal>(ref joinLobbyOptionsInternal);
		}

		// Token: 0x06001A3C RID: 6716 RVA: 0x00026FD8 File Offset: 0x000251D8
		public void JoinLobbyById(ref JoinLobbyByIdOptions options, object clientData, OnJoinLobbyByIdCallback completionDelegate)
		{
			JoinLobbyByIdOptionsInternal joinLobbyByIdOptionsInternal = default(JoinLobbyByIdOptionsInternal);
			joinLobbyByIdOptionsInternal.Set(ref options);
			IntPtr zero = IntPtr.Zero;
			OnJoinLobbyByIdCallbackInternal onJoinLobbyByIdCallbackInternal = new OnJoinLobbyByIdCallbackInternal(LobbyInterface.OnJoinLobbyByIdCallbackInternalImplementation);
			Helper.AddCallback(out zero, clientData, completionDelegate, onJoinLobbyByIdCallbackInternal, Array.Empty<Delegate>());
			Bindings.EOS_Lobby_JoinLobbyById(base.InnerHandle, ref joinLobbyByIdOptionsInternal, zero, onJoinLobbyByIdCallbackInternal);
			Helper.Dispose<JoinLobbyByIdOptionsInternal>(ref joinLobbyByIdOptionsInternal);
		}

		// Token: 0x06001A3D RID: 6717 RVA: 0x00027034 File Offset: 0x00025234
		public void JoinRTCRoom(ref JoinRTCRoomOptions options, object clientData, OnJoinRTCRoomCallback completionDelegate)
		{
			JoinRTCRoomOptionsInternal joinRTCRoomOptionsInternal = default(JoinRTCRoomOptionsInternal);
			joinRTCRoomOptionsInternal.Set(ref options);
			IntPtr zero = IntPtr.Zero;
			OnJoinRTCRoomCallbackInternal onJoinRTCRoomCallbackInternal = new OnJoinRTCRoomCallbackInternal(LobbyInterface.OnJoinRTCRoomCallbackInternalImplementation);
			Helper.AddCallback(out zero, clientData, completionDelegate, onJoinRTCRoomCallbackInternal, Array.Empty<Delegate>());
			Bindings.EOS_Lobby_JoinRTCRoom(base.InnerHandle, ref joinRTCRoomOptionsInternal, zero, onJoinRTCRoomCallbackInternal);
			Helper.Dispose<JoinRTCRoomOptionsInternal>(ref joinRTCRoomOptionsInternal);
		}

		// Token: 0x06001A3E RID: 6718 RVA: 0x00027090 File Offset: 0x00025290
		public void KickMember(ref KickMemberOptions options, object clientData, OnKickMemberCallback completionDelegate)
		{
			KickMemberOptionsInternal kickMemberOptionsInternal = default(KickMemberOptionsInternal);
			kickMemberOptionsInternal.Set(ref options);
			IntPtr zero = IntPtr.Zero;
			OnKickMemberCallbackInternal onKickMemberCallbackInternal = new OnKickMemberCallbackInternal(LobbyInterface.OnKickMemberCallbackInternalImplementation);
			Helper.AddCallback(out zero, clientData, completionDelegate, onKickMemberCallbackInternal, Array.Empty<Delegate>());
			Bindings.EOS_Lobby_KickMember(base.InnerHandle, ref kickMemberOptionsInternal, zero, onKickMemberCallbackInternal);
			Helper.Dispose<KickMemberOptionsInternal>(ref kickMemberOptionsInternal);
		}

		// Token: 0x06001A3F RID: 6719 RVA: 0x000270EC File Offset: 0x000252EC
		public void LeaveLobby(ref LeaveLobbyOptions options, object clientData, OnLeaveLobbyCallback completionDelegate)
		{
			LeaveLobbyOptionsInternal leaveLobbyOptionsInternal = default(LeaveLobbyOptionsInternal);
			leaveLobbyOptionsInternal.Set(ref options);
			IntPtr zero = IntPtr.Zero;
			OnLeaveLobbyCallbackInternal onLeaveLobbyCallbackInternal = new OnLeaveLobbyCallbackInternal(LobbyInterface.OnLeaveLobbyCallbackInternalImplementation);
			Helper.AddCallback(out zero, clientData, completionDelegate, onLeaveLobbyCallbackInternal, Array.Empty<Delegate>());
			Bindings.EOS_Lobby_LeaveLobby(base.InnerHandle, ref leaveLobbyOptionsInternal, zero, onLeaveLobbyCallbackInternal);
			Helper.Dispose<LeaveLobbyOptionsInternal>(ref leaveLobbyOptionsInternal);
		}

		// Token: 0x06001A40 RID: 6720 RVA: 0x00027148 File Offset: 0x00025348
		public void LeaveRTCRoom(ref LeaveRTCRoomOptions options, object clientData, OnLeaveRTCRoomCallback completionDelegate)
		{
			LeaveRTCRoomOptionsInternal leaveRTCRoomOptionsInternal = default(LeaveRTCRoomOptionsInternal);
			leaveRTCRoomOptionsInternal.Set(ref options);
			IntPtr zero = IntPtr.Zero;
			OnLeaveRTCRoomCallbackInternal onLeaveRTCRoomCallbackInternal = new OnLeaveRTCRoomCallbackInternal(LobbyInterface.OnLeaveRTCRoomCallbackInternalImplementation);
			Helper.AddCallback(out zero, clientData, completionDelegate, onLeaveRTCRoomCallbackInternal, Array.Empty<Delegate>());
			Bindings.EOS_Lobby_LeaveRTCRoom(base.InnerHandle, ref leaveRTCRoomOptionsInternal, zero, onLeaveRTCRoomCallbackInternal);
			Helper.Dispose<LeaveRTCRoomOptionsInternal>(ref leaveRTCRoomOptionsInternal);
		}

		// Token: 0x06001A41 RID: 6721 RVA: 0x000271A4 File Offset: 0x000253A4
		public Result ParseConnectString(ref ParseConnectStringOptions options, out Utf8String outBuffer)
		{
			ParseConnectStringOptionsInternal parseConnectStringOptionsInternal = default(ParseConnectStringOptionsInternal);
			parseConnectStringOptionsInternal.Set(ref options);
			uint size = 256U;
			IntPtr intPtr = Helper.AddAllocation(size);
			Result result = Bindings.EOS_Lobby_ParseConnectString(base.InnerHandle, ref parseConnectStringOptionsInternal, intPtr, ref size);
			Helper.Dispose<ParseConnectStringOptionsInternal>(ref parseConnectStringOptionsInternal);
			Helper.Get(intPtr, out outBuffer);
			Helper.Dispose(ref intPtr);
			return result;
		}

		// Token: 0x06001A42 RID: 6722 RVA: 0x00027200 File Offset: 0x00025400
		public void PromoteMember(ref PromoteMemberOptions options, object clientData, OnPromoteMemberCallback completionDelegate)
		{
			PromoteMemberOptionsInternal promoteMemberOptionsInternal = default(PromoteMemberOptionsInternal);
			promoteMemberOptionsInternal.Set(ref options);
			IntPtr zero = IntPtr.Zero;
			OnPromoteMemberCallbackInternal onPromoteMemberCallbackInternal = new OnPromoteMemberCallbackInternal(LobbyInterface.OnPromoteMemberCallbackInternalImplementation);
			Helper.AddCallback(out zero, clientData, completionDelegate, onPromoteMemberCallbackInternal, Array.Empty<Delegate>());
			Bindings.EOS_Lobby_PromoteMember(base.InnerHandle, ref promoteMemberOptionsInternal, zero, onPromoteMemberCallbackInternal);
			Helper.Dispose<PromoteMemberOptionsInternal>(ref promoteMemberOptionsInternal);
		}

		// Token: 0x06001A43 RID: 6723 RVA: 0x0002725C File Offset: 0x0002545C
		public void QueryInvites(ref QueryInvitesOptions options, object clientData, OnQueryInvitesCallback completionDelegate)
		{
			QueryInvitesOptionsInternal queryInvitesOptionsInternal = default(QueryInvitesOptionsInternal);
			queryInvitesOptionsInternal.Set(ref options);
			IntPtr zero = IntPtr.Zero;
			OnQueryInvitesCallbackInternal onQueryInvitesCallbackInternal = new OnQueryInvitesCallbackInternal(LobbyInterface.OnQueryInvitesCallbackInternalImplementation);
			Helper.AddCallback(out zero, clientData, completionDelegate, onQueryInvitesCallbackInternal, Array.Empty<Delegate>());
			Bindings.EOS_Lobby_QueryInvites(base.InnerHandle, ref queryInvitesOptionsInternal, zero, onQueryInvitesCallbackInternal);
			Helper.Dispose<QueryInvitesOptionsInternal>(ref queryInvitesOptionsInternal);
		}

		// Token: 0x06001A44 RID: 6724 RVA: 0x000272B8 File Offset: 0x000254B8
		public void RejectInvite(ref RejectInviteOptions options, object clientData, OnRejectInviteCallback completionDelegate)
		{
			RejectInviteOptionsInternal rejectInviteOptionsInternal = default(RejectInviteOptionsInternal);
			rejectInviteOptionsInternal.Set(ref options);
			IntPtr zero = IntPtr.Zero;
			OnRejectInviteCallbackInternal onRejectInviteCallbackInternal = new OnRejectInviteCallbackInternal(LobbyInterface.OnRejectInviteCallbackInternalImplementation);
			Helper.AddCallback(out zero, clientData, completionDelegate, onRejectInviteCallbackInternal, Array.Empty<Delegate>());
			Bindings.EOS_Lobby_RejectInvite(base.InnerHandle, ref rejectInviteOptionsInternal, zero, onRejectInviteCallbackInternal);
			Helper.Dispose<RejectInviteOptionsInternal>(ref rejectInviteOptionsInternal);
		}

		// Token: 0x06001A45 RID: 6725 RVA: 0x00027312 File Offset: 0x00025512
		public void RemoveNotifyJoinLobbyAccepted(ulong inId)
		{
			Bindings.EOS_Lobby_RemoveNotifyJoinLobbyAccepted(base.InnerHandle, inId);
			Helper.RemoveCallbackByNotificationId(inId);
		}

		// Token: 0x06001A46 RID: 6726 RVA: 0x00027329 File Offset: 0x00025529
		public void RemoveNotifyLeaveLobbyRequested(ulong inId)
		{
			Bindings.EOS_Lobby_RemoveNotifyLeaveLobbyRequested(base.InnerHandle, inId);
			Helper.RemoveCallbackByNotificationId(inId);
		}

		// Token: 0x06001A47 RID: 6727 RVA: 0x00027340 File Offset: 0x00025540
		public void RemoveNotifyLobbyInviteAccepted(ulong inId)
		{
			Bindings.EOS_Lobby_RemoveNotifyLobbyInviteAccepted(base.InnerHandle, inId);
			Helper.RemoveCallbackByNotificationId(inId);
		}

		// Token: 0x06001A48 RID: 6728 RVA: 0x00027357 File Offset: 0x00025557
		public void RemoveNotifyLobbyInviteReceived(ulong inId)
		{
			Bindings.EOS_Lobby_RemoveNotifyLobbyInviteReceived(base.InnerHandle, inId);
			Helper.RemoveCallbackByNotificationId(inId);
		}

		// Token: 0x06001A49 RID: 6729 RVA: 0x0002736E File Offset: 0x0002556E
		public void RemoveNotifyLobbyInviteRejected(ulong inId)
		{
			Bindings.EOS_Lobby_RemoveNotifyLobbyInviteRejected(base.InnerHandle, inId);
			Helper.RemoveCallbackByNotificationId(inId);
		}

		// Token: 0x06001A4A RID: 6730 RVA: 0x00027385 File Offset: 0x00025585
		public void RemoveNotifyLobbyMemberStatusReceived(ulong inId)
		{
			Bindings.EOS_Lobby_RemoveNotifyLobbyMemberStatusReceived(base.InnerHandle, inId);
			Helper.RemoveCallbackByNotificationId(inId);
		}

		// Token: 0x06001A4B RID: 6731 RVA: 0x0002739C File Offset: 0x0002559C
		public void RemoveNotifyLobbyMemberUpdateReceived(ulong inId)
		{
			Bindings.EOS_Lobby_RemoveNotifyLobbyMemberUpdateReceived(base.InnerHandle, inId);
			Helper.RemoveCallbackByNotificationId(inId);
		}

		// Token: 0x06001A4C RID: 6732 RVA: 0x000273B3 File Offset: 0x000255B3
		public void RemoveNotifyLobbyUpdateReceived(ulong inId)
		{
			Bindings.EOS_Lobby_RemoveNotifyLobbyUpdateReceived(base.InnerHandle, inId);
			Helper.RemoveCallbackByNotificationId(inId);
		}

		// Token: 0x06001A4D RID: 6733 RVA: 0x000273CA File Offset: 0x000255CA
		public void RemoveNotifyRTCRoomConnectionChanged(ulong inId)
		{
			Bindings.EOS_Lobby_RemoveNotifyRTCRoomConnectionChanged(base.InnerHandle, inId);
			Helper.RemoveCallbackByNotificationId(inId);
		}

		// Token: 0x06001A4E RID: 6734 RVA: 0x000273E1 File Offset: 0x000255E1
		public void RemoveNotifySendLobbyNativeInviteRequested(ulong inId)
		{
			Bindings.EOS_Lobby_RemoveNotifySendLobbyNativeInviteRequested(base.InnerHandle, inId);
			Helper.RemoveCallbackByNotificationId(inId);
		}

		// Token: 0x06001A4F RID: 6735 RVA: 0x000273F8 File Offset: 0x000255F8
		public void SendInvite(ref SendInviteOptions options, object clientData, OnSendInviteCallback completionDelegate)
		{
			SendInviteOptionsInternal sendInviteOptionsInternal = default(SendInviteOptionsInternal);
			sendInviteOptionsInternal.Set(ref options);
			IntPtr zero = IntPtr.Zero;
			OnSendInviteCallbackInternal onSendInviteCallbackInternal = new OnSendInviteCallbackInternal(LobbyInterface.OnSendInviteCallbackInternalImplementation);
			Helper.AddCallback(out zero, clientData, completionDelegate, onSendInviteCallbackInternal, Array.Empty<Delegate>());
			Bindings.EOS_Lobby_SendInvite(base.InnerHandle, ref sendInviteOptionsInternal, zero, onSendInviteCallbackInternal);
			Helper.Dispose<SendInviteOptionsInternal>(ref sendInviteOptionsInternal);
		}

		// Token: 0x06001A50 RID: 6736 RVA: 0x00027454 File Offset: 0x00025654
		public void UpdateLobby(ref UpdateLobbyOptions options, object clientData, OnUpdateLobbyCallback completionDelegate)
		{
			UpdateLobbyOptionsInternal updateLobbyOptionsInternal = default(UpdateLobbyOptionsInternal);
			updateLobbyOptionsInternal.Set(ref options);
			IntPtr zero = IntPtr.Zero;
			OnUpdateLobbyCallbackInternal onUpdateLobbyCallbackInternal = new OnUpdateLobbyCallbackInternal(LobbyInterface.OnUpdateLobbyCallbackInternalImplementation);
			Helper.AddCallback(out zero, clientData, completionDelegate, onUpdateLobbyCallbackInternal, Array.Empty<Delegate>());
			Bindings.EOS_Lobby_UpdateLobby(base.InnerHandle, ref updateLobbyOptionsInternal, zero, onUpdateLobbyCallbackInternal);
			Helper.Dispose<UpdateLobbyOptionsInternal>(ref updateLobbyOptionsInternal);
		}

		// Token: 0x06001A51 RID: 6737 RVA: 0x000274B0 File Offset: 0x000256B0
		public Result UpdateLobbyModification(ref UpdateLobbyModificationOptions options, out LobbyModification outLobbyModificationHandle)
		{
			UpdateLobbyModificationOptionsInternal updateLobbyModificationOptionsInternal = default(UpdateLobbyModificationOptionsInternal);
			updateLobbyModificationOptionsInternal.Set(ref options);
			IntPtr zero = IntPtr.Zero;
			Result result = Bindings.EOS_Lobby_UpdateLobbyModification(base.InnerHandle, ref updateLobbyModificationOptionsInternal, ref zero);
			Helper.Dispose<UpdateLobbyModificationOptionsInternal>(ref updateLobbyModificationOptionsInternal);
			Helper.Get<LobbyModification>(zero, out outLobbyModificationHandle);
			return result;
		}

		// Token: 0x06001A52 RID: 6738 RVA: 0x000274FC File Offset: 0x000256FC
		[MonoPInvokeCallback(typeof(OnCreateLobbyCallbackInternal))]
		internal static void OnCreateLobbyCallbackInternalImplementation(ref CreateLobbyCallbackInfoInternal data)
		{
			OnCreateLobbyCallback onCreateLobbyCallback;
			CreateLobbyCallbackInfo createLobbyCallbackInfo;
			bool flag = Helper.TryGetAndRemoveCallback<CreateLobbyCallbackInfoInternal, OnCreateLobbyCallback, CreateLobbyCallbackInfo>(ref data, out onCreateLobbyCallback, out createLobbyCallbackInfo);
			if (flag)
			{
				onCreateLobbyCallback(ref createLobbyCallbackInfo);
			}
		}

		// Token: 0x06001A53 RID: 6739 RVA: 0x00027524 File Offset: 0x00025724
		[MonoPInvokeCallback(typeof(OnDestroyLobbyCallbackInternal))]
		internal static void OnDestroyLobbyCallbackInternalImplementation(ref DestroyLobbyCallbackInfoInternal data)
		{
			OnDestroyLobbyCallback onDestroyLobbyCallback;
			DestroyLobbyCallbackInfo destroyLobbyCallbackInfo;
			bool flag = Helper.TryGetAndRemoveCallback<DestroyLobbyCallbackInfoInternal, OnDestroyLobbyCallback, DestroyLobbyCallbackInfo>(ref data, out onDestroyLobbyCallback, out destroyLobbyCallbackInfo);
			if (flag)
			{
				onDestroyLobbyCallback(ref destroyLobbyCallbackInfo);
			}
		}

		// Token: 0x06001A54 RID: 6740 RVA: 0x0002754C File Offset: 0x0002574C
		[MonoPInvokeCallback(typeof(OnHardMuteMemberCallbackInternal))]
		internal static void OnHardMuteMemberCallbackInternalImplementation(ref HardMuteMemberCallbackInfoInternal data)
		{
			OnHardMuteMemberCallback onHardMuteMemberCallback;
			HardMuteMemberCallbackInfo hardMuteMemberCallbackInfo;
			bool flag = Helper.TryGetAndRemoveCallback<HardMuteMemberCallbackInfoInternal, OnHardMuteMemberCallback, HardMuteMemberCallbackInfo>(ref data, out onHardMuteMemberCallback, out hardMuteMemberCallbackInfo);
			if (flag)
			{
				onHardMuteMemberCallback(ref hardMuteMemberCallbackInfo);
			}
		}

		// Token: 0x06001A55 RID: 6741 RVA: 0x00027574 File Offset: 0x00025774
		[MonoPInvokeCallback(typeof(OnJoinLobbyAcceptedCallbackInternal))]
		internal static void OnJoinLobbyAcceptedCallbackInternalImplementation(ref JoinLobbyAcceptedCallbackInfoInternal data)
		{
			OnJoinLobbyAcceptedCallback onJoinLobbyAcceptedCallback;
			JoinLobbyAcceptedCallbackInfo joinLobbyAcceptedCallbackInfo;
			bool flag = Helper.TryGetCallback<JoinLobbyAcceptedCallbackInfoInternal, OnJoinLobbyAcceptedCallback, JoinLobbyAcceptedCallbackInfo>(ref data, out onJoinLobbyAcceptedCallback, out joinLobbyAcceptedCallbackInfo);
			if (flag)
			{
				onJoinLobbyAcceptedCallback(ref joinLobbyAcceptedCallbackInfo);
			}
		}

		// Token: 0x06001A56 RID: 6742 RVA: 0x0002759C File Offset: 0x0002579C
		[MonoPInvokeCallback(typeof(OnJoinLobbyByIdCallbackInternal))]
		internal static void OnJoinLobbyByIdCallbackInternalImplementation(ref JoinLobbyByIdCallbackInfoInternal data)
		{
			OnJoinLobbyByIdCallback onJoinLobbyByIdCallback;
			JoinLobbyByIdCallbackInfo joinLobbyByIdCallbackInfo;
			bool flag = Helper.TryGetAndRemoveCallback<JoinLobbyByIdCallbackInfoInternal, OnJoinLobbyByIdCallback, JoinLobbyByIdCallbackInfo>(ref data, out onJoinLobbyByIdCallback, out joinLobbyByIdCallbackInfo);
			if (flag)
			{
				onJoinLobbyByIdCallback(ref joinLobbyByIdCallbackInfo);
			}
		}

		// Token: 0x06001A57 RID: 6743 RVA: 0x000275C4 File Offset: 0x000257C4
		[MonoPInvokeCallback(typeof(OnJoinLobbyCallbackInternal))]
		internal static void OnJoinLobbyCallbackInternalImplementation(ref JoinLobbyCallbackInfoInternal data)
		{
			OnJoinLobbyCallback onJoinLobbyCallback;
			JoinLobbyCallbackInfo joinLobbyCallbackInfo;
			bool flag = Helper.TryGetAndRemoveCallback<JoinLobbyCallbackInfoInternal, OnJoinLobbyCallback, JoinLobbyCallbackInfo>(ref data, out onJoinLobbyCallback, out joinLobbyCallbackInfo);
			if (flag)
			{
				onJoinLobbyCallback(ref joinLobbyCallbackInfo);
			}
		}

		// Token: 0x06001A58 RID: 6744 RVA: 0x000275EC File Offset: 0x000257EC
		[MonoPInvokeCallback(typeof(OnJoinRTCRoomCallbackInternal))]
		internal static void OnJoinRTCRoomCallbackInternalImplementation(ref JoinRTCRoomCallbackInfoInternal data)
		{
			OnJoinRTCRoomCallback onJoinRTCRoomCallback;
			JoinRTCRoomCallbackInfo joinRTCRoomCallbackInfo;
			bool flag = Helper.TryGetAndRemoveCallback<JoinRTCRoomCallbackInfoInternal, OnJoinRTCRoomCallback, JoinRTCRoomCallbackInfo>(ref data, out onJoinRTCRoomCallback, out joinRTCRoomCallbackInfo);
			if (flag)
			{
				onJoinRTCRoomCallback(ref joinRTCRoomCallbackInfo);
			}
		}

		// Token: 0x06001A59 RID: 6745 RVA: 0x00027614 File Offset: 0x00025814
		[MonoPInvokeCallback(typeof(OnKickMemberCallbackInternal))]
		internal static void OnKickMemberCallbackInternalImplementation(ref KickMemberCallbackInfoInternal data)
		{
			OnKickMemberCallback onKickMemberCallback;
			KickMemberCallbackInfo kickMemberCallbackInfo;
			bool flag = Helper.TryGetAndRemoveCallback<KickMemberCallbackInfoInternal, OnKickMemberCallback, KickMemberCallbackInfo>(ref data, out onKickMemberCallback, out kickMemberCallbackInfo);
			if (flag)
			{
				onKickMemberCallback(ref kickMemberCallbackInfo);
			}
		}

		// Token: 0x06001A5A RID: 6746 RVA: 0x0002763C File Offset: 0x0002583C
		[MonoPInvokeCallback(typeof(OnLeaveLobbyCallbackInternal))]
		internal static void OnLeaveLobbyCallbackInternalImplementation(ref LeaveLobbyCallbackInfoInternal data)
		{
			OnLeaveLobbyCallback onLeaveLobbyCallback;
			LeaveLobbyCallbackInfo leaveLobbyCallbackInfo;
			bool flag = Helper.TryGetAndRemoveCallback<LeaveLobbyCallbackInfoInternal, OnLeaveLobbyCallback, LeaveLobbyCallbackInfo>(ref data, out onLeaveLobbyCallback, out leaveLobbyCallbackInfo);
			if (flag)
			{
				onLeaveLobbyCallback(ref leaveLobbyCallbackInfo);
			}
		}

		// Token: 0x06001A5B RID: 6747 RVA: 0x00027664 File Offset: 0x00025864
		[MonoPInvokeCallback(typeof(OnLeaveLobbyRequestedCallbackInternal))]
		internal static void OnLeaveLobbyRequestedCallbackInternalImplementation(ref LeaveLobbyRequestedCallbackInfoInternal data)
		{
			OnLeaveLobbyRequestedCallback onLeaveLobbyRequestedCallback;
			LeaveLobbyRequestedCallbackInfo leaveLobbyRequestedCallbackInfo;
			bool flag = Helper.TryGetCallback<LeaveLobbyRequestedCallbackInfoInternal, OnLeaveLobbyRequestedCallback, LeaveLobbyRequestedCallbackInfo>(ref data, out onLeaveLobbyRequestedCallback, out leaveLobbyRequestedCallbackInfo);
			if (flag)
			{
				onLeaveLobbyRequestedCallback(ref leaveLobbyRequestedCallbackInfo);
			}
		}

		// Token: 0x06001A5C RID: 6748 RVA: 0x0002768C File Offset: 0x0002588C
		[MonoPInvokeCallback(typeof(OnLeaveRTCRoomCallbackInternal))]
		internal static void OnLeaveRTCRoomCallbackInternalImplementation(ref LeaveRTCRoomCallbackInfoInternal data)
		{
			OnLeaveRTCRoomCallback onLeaveRTCRoomCallback;
			LeaveRTCRoomCallbackInfo leaveRTCRoomCallbackInfo;
			bool flag = Helper.TryGetAndRemoveCallback<LeaveRTCRoomCallbackInfoInternal, OnLeaveRTCRoomCallback, LeaveRTCRoomCallbackInfo>(ref data, out onLeaveRTCRoomCallback, out leaveRTCRoomCallbackInfo);
			if (flag)
			{
				onLeaveRTCRoomCallback(ref leaveRTCRoomCallbackInfo);
			}
		}

		// Token: 0x06001A5D RID: 6749 RVA: 0x000276B4 File Offset: 0x000258B4
		[MonoPInvokeCallback(typeof(OnLobbyInviteAcceptedCallbackInternal))]
		internal static void OnLobbyInviteAcceptedCallbackInternalImplementation(ref LobbyInviteAcceptedCallbackInfoInternal data)
		{
			OnLobbyInviteAcceptedCallback onLobbyInviteAcceptedCallback;
			LobbyInviteAcceptedCallbackInfo lobbyInviteAcceptedCallbackInfo;
			bool flag = Helper.TryGetCallback<LobbyInviteAcceptedCallbackInfoInternal, OnLobbyInviteAcceptedCallback, LobbyInviteAcceptedCallbackInfo>(ref data, out onLobbyInviteAcceptedCallback, out lobbyInviteAcceptedCallbackInfo);
			if (flag)
			{
				onLobbyInviteAcceptedCallback(ref lobbyInviteAcceptedCallbackInfo);
			}
		}

		// Token: 0x06001A5E RID: 6750 RVA: 0x000276DC File Offset: 0x000258DC
		[MonoPInvokeCallback(typeof(OnLobbyInviteReceivedCallbackInternal))]
		internal static void OnLobbyInviteReceivedCallbackInternalImplementation(ref LobbyInviteReceivedCallbackInfoInternal data)
		{
			OnLobbyInviteReceivedCallback onLobbyInviteReceivedCallback;
			LobbyInviteReceivedCallbackInfo lobbyInviteReceivedCallbackInfo;
			bool flag = Helper.TryGetCallback<LobbyInviteReceivedCallbackInfoInternal, OnLobbyInviteReceivedCallback, LobbyInviteReceivedCallbackInfo>(ref data, out onLobbyInviteReceivedCallback, out lobbyInviteReceivedCallbackInfo);
			if (flag)
			{
				onLobbyInviteReceivedCallback(ref lobbyInviteReceivedCallbackInfo);
			}
		}

		// Token: 0x06001A5F RID: 6751 RVA: 0x00027704 File Offset: 0x00025904
		[MonoPInvokeCallback(typeof(OnLobbyInviteRejectedCallbackInternal))]
		internal static void OnLobbyInviteRejectedCallbackInternalImplementation(ref LobbyInviteRejectedCallbackInfoInternal data)
		{
			OnLobbyInviteRejectedCallback onLobbyInviteRejectedCallback;
			LobbyInviteRejectedCallbackInfo lobbyInviteRejectedCallbackInfo;
			bool flag = Helper.TryGetCallback<LobbyInviteRejectedCallbackInfoInternal, OnLobbyInviteRejectedCallback, LobbyInviteRejectedCallbackInfo>(ref data, out onLobbyInviteRejectedCallback, out lobbyInviteRejectedCallbackInfo);
			if (flag)
			{
				onLobbyInviteRejectedCallback(ref lobbyInviteRejectedCallbackInfo);
			}
		}

		// Token: 0x06001A60 RID: 6752 RVA: 0x0002772C File Offset: 0x0002592C
		[MonoPInvokeCallback(typeof(OnLobbyMemberStatusReceivedCallbackInternal))]
		internal static void OnLobbyMemberStatusReceivedCallbackInternalImplementation(ref LobbyMemberStatusReceivedCallbackInfoInternal data)
		{
			OnLobbyMemberStatusReceivedCallback onLobbyMemberStatusReceivedCallback;
			LobbyMemberStatusReceivedCallbackInfo lobbyMemberStatusReceivedCallbackInfo;
			bool flag = Helper.TryGetCallback<LobbyMemberStatusReceivedCallbackInfoInternal, OnLobbyMemberStatusReceivedCallback, LobbyMemberStatusReceivedCallbackInfo>(ref data, out onLobbyMemberStatusReceivedCallback, out lobbyMemberStatusReceivedCallbackInfo);
			if (flag)
			{
				onLobbyMemberStatusReceivedCallback(ref lobbyMemberStatusReceivedCallbackInfo);
			}
		}

		// Token: 0x06001A61 RID: 6753 RVA: 0x00027754 File Offset: 0x00025954
		[MonoPInvokeCallback(typeof(OnLobbyMemberUpdateReceivedCallbackInternal))]
		internal static void OnLobbyMemberUpdateReceivedCallbackInternalImplementation(ref LobbyMemberUpdateReceivedCallbackInfoInternal data)
		{
			OnLobbyMemberUpdateReceivedCallback onLobbyMemberUpdateReceivedCallback;
			LobbyMemberUpdateReceivedCallbackInfo lobbyMemberUpdateReceivedCallbackInfo;
			bool flag = Helper.TryGetCallback<LobbyMemberUpdateReceivedCallbackInfoInternal, OnLobbyMemberUpdateReceivedCallback, LobbyMemberUpdateReceivedCallbackInfo>(ref data, out onLobbyMemberUpdateReceivedCallback, out lobbyMemberUpdateReceivedCallbackInfo);
			if (flag)
			{
				onLobbyMemberUpdateReceivedCallback(ref lobbyMemberUpdateReceivedCallbackInfo);
			}
		}

		// Token: 0x06001A62 RID: 6754 RVA: 0x0002777C File Offset: 0x0002597C
		[MonoPInvokeCallback(typeof(OnLobbyUpdateReceivedCallbackInternal))]
		internal static void OnLobbyUpdateReceivedCallbackInternalImplementation(ref LobbyUpdateReceivedCallbackInfoInternal data)
		{
			OnLobbyUpdateReceivedCallback onLobbyUpdateReceivedCallback;
			LobbyUpdateReceivedCallbackInfo lobbyUpdateReceivedCallbackInfo;
			bool flag = Helper.TryGetCallback<LobbyUpdateReceivedCallbackInfoInternal, OnLobbyUpdateReceivedCallback, LobbyUpdateReceivedCallbackInfo>(ref data, out onLobbyUpdateReceivedCallback, out lobbyUpdateReceivedCallbackInfo);
			if (flag)
			{
				onLobbyUpdateReceivedCallback(ref lobbyUpdateReceivedCallbackInfo);
			}
		}

		// Token: 0x06001A63 RID: 6755 RVA: 0x000277A4 File Offset: 0x000259A4
		[MonoPInvokeCallback(typeof(OnPromoteMemberCallbackInternal))]
		internal static void OnPromoteMemberCallbackInternalImplementation(ref PromoteMemberCallbackInfoInternal data)
		{
			OnPromoteMemberCallback onPromoteMemberCallback;
			PromoteMemberCallbackInfo promoteMemberCallbackInfo;
			bool flag = Helper.TryGetAndRemoveCallback<PromoteMemberCallbackInfoInternal, OnPromoteMemberCallback, PromoteMemberCallbackInfo>(ref data, out onPromoteMemberCallback, out promoteMemberCallbackInfo);
			if (flag)
			{
				onPromoteMemberCallback(ref promoteMemberCallbackInfo);
			}
		}

		// Token: 0x06001A64 RID: 6756 RVA: 0x000277CC File Offset: 0x000259CC
		[MonoPInvokeCallback(typeof(OnQueryInvitesCallbackInternal))]
		internal static void OnQueryInvitesCallbackInternalImplementation(ref QueryInvitesCallbackInfoInternal data)
		{
			OnQueryInvitesCallback onQueryInvitesCallback;
			QueryInvitesCallbackInfo queryInvitesCallbackInfo;
			bool flag = Helper.TryGetAndRemoveCallback<QueryInvitesCallbackInfoInternal, OnQueryInvitesCallback, QueryInvitesCallbackInfo>(ref data, out onQueryInvitesCallback, out queryInvitesCallbackInfo);
			if (flag)
			{
				onQueryInvitesCallback(ref queryInvitesCallbackInfo);
			}
		}

		// Token: 0x06001A65 RID: 6757 RVA: 0x000277F4 File Offset: 0x000259F4
		[MonoPInvokeCallback(typeof(OnRTCRoomConnectionChangedCallbackInternal))]
		internal static void OnRTCRoomConnectionChangedCallbackInternalImplementation(ref RTCRoomConnectionChangedCallbackInfoInternal data)
		{
			OnRTCRoomConnectionChangedCallback onRTCRoomConnectionChangedCallback;
			RTCRoomConnectionChangedCallbackInfo rtcroomConnectionChangedCallbackInfo;
			bool flag = Helper.TryGetCallback<RTCRoomConnectionChangedCallbackInfoInternal, OnRTCRoomConnectionChangedCallback, RTCRoomConnectionChangedCallbackInfo>(ref data, out onRTCRoomConnectionChangedCallback, out rtcroomConnectionChangedCallbackInfo);
			if (flag)
			{
				onRTCRoomConnectionChangedCallback(ref rtcroomConnectionChangedCallbackInfo);
			}
		}

		// Token: 0x06001A66 RID: 6758 RVA: 0x0002781C File Offset: 0x00025A1C
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

		// Token: 0x06001A67 RID: 6759 RVA: 0x00027844 File Offset: 0x00025A44
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

		// Token: 0x06001A68 RID: 6760 RVA: 0x0002786C File Offset: 0x00025A6C
		[MonoPInvokeCallback(typeof(OnSendLobbyNativeInviteRequestedCallbackInternal))]
		internal static void OnSendLobbyNativeInviteRequestedCallbackInternalImplementation(ref SendLobbyNativeInviteRequestedCallbackInfoInternal data)
		{
			OnSendLobbyNativeInviteRequestedCallback onSendLobbyNativeInviteRequestedCallback;
			SendLobbyNativeInviteRequestedCallbackInfo sendLobbyNativeInviteRequestedCallbackInfo;
			bool flag = Helper.TryGetCallback<SendLobbyNativeInviteRequestedCallbackInfoInternal, OnSendLobbyNativeInviteRequestedCallback, SendLobbyNativeInviteRequestedCallbackInfo>(ref data, out onSendLobbyNativeInviteRequestedCallback, out sendLobbyNativeInviteRequestedCallbackInfo);
			if (flag)
			{
				onSendLobbyNativeInviteRequestedCallback(ref sendLobbyNativeInviteRequestedCallbackInfo);
			}
		}

		// Token: 0x06001A69 RID: 6761 RVA: 0x00027894 File Offset: 0x00025A94
		[MonoPInvokeCallback(typeof(OnUpdateLobbyCallbackInternal))]
		internal static void OnUpdateLobbyCallbackInternalImplementation(ref UpdateLobbyCallbackInfoInternal data)
		{
			OnUpdateLobbyCallback onUpdateLobbyCallback;
			UpdateLobbyCallbackInfo updateLobbyCallbackInfo;
			bool flag = Helper.TryGetAndRemoveCallback<UpdateLobbyCallbackInfoInternal, OnUpdateLobbyCallback, UpdateLobbyCallbackInfo>(ref data, out onUpdateLobbyCallback, out updateLobbyCallbackInfo);
			if (flag)
			{
				onUpdateLobbyCallback(ref updateLobbyCallbackInfo);
			}
		}

		// Token: 0x04000B9D RID: 2973
		public const int AddnotifyjoinlobbyacceptedApiLatest = 1;

		// Token: 0x04000B9E RID: 2974
		public const int AddnotifyleavelobbyrequestedApiLatest = 1;

		// Token: 0x04000B9F RID: 2975
		public const int AddnotifylobbyinviteacceptedApiLatest = 1;

		// Token: 0x04000BA0 RID: 2976
		public const int AddnotifylobbyinvitereceivedApiLatest = 1;

		// Token: 0x04000BA1 RID: 2977
		public const int AddnotifylobbyinviterejectedApiLatest = 1;

		// Token: 0x04000BA2 RID: 2978
		public const int AddnotifylobbymemberstatusreceivedApiLatest = 1;

		// Token: 0x04000BA3 RID: 2979
		public const int AddnotifylobbymemberupdatereceivedApiLatest = 1;

		// Token: 0x04000BA4 RID: 2980
		public const int AddnotifylobbyupdatereceivedApiLatest = 1;

		// Token: 0x04000BA5 RID: 2981
		public const int AddnotifyrtcroomconnectionchangedApiLatest = 2;

		// Token: 0x04000BA6 RID: 2982
		public const int AddnotifysendlobbynativeinviterequestedApiLatest = 1;

		// Token: 0x04000BA7 RID: 2983
		public const int AttributeApiLatest = 1;

		// Token: 0x04000BA8 RID: 2984
		public const int AttributedataApiLatest = 1;

		// Token: 0x04000BA9 RID: 2985
		public const int CopylobbydetailshandleApiLatest = 1;

		// Token: 0x04000BAA RID: 2986
		public const int CopylobbydetailshandlebyinviteidApiLatest = 1;

		// Token: 0x04000BAB RID: 2987
		public const int CopylobbydetailshandlebyuieventidApiLatest = 1;

		// Token: 0x04000BAC RID: 2988
		public const int CreatelobbyApiLatest = 10;

		// Token: 0x04000BAD RID: 2989
		public const int CreatelobbysearchApiLatest = 1;

		// Token: 0x04000BAE RID: 2990
		public const int DestroylobbyApiLatest = 1;

		// Token: 0x04000BAF RID: 2991
		public const int GetconnectstringApiLatest = 1;

		// Token: 0x04000BB0 RID: 2992
		public const int GetconnectstringBufferSize = 256;

		// Token: 0x04000BB1 RID: 2993
		public const int GetinvitecountApiLatest = 1;

		// Token: 0x04000BB2 RID: 2994
		public const int GetinviteidbyindexApiLatest = 1;

		// Token: 0x04000BB3 RID: 2995
		public const int GetrtcroomnameApiLatest = 1;

		// Token: 0x04000BB4 RID: 2996
		public const int HardmutememberApiLatest = 1;

		// Token: 0x04000BB5 RID: 2997
		public const int InviteidMaxLength = 64;

		// Token: 0x04000BB6 RID: 2998
		public const int IsrtcroomconnectedApiLatest = 1;

		// Token: 0x04000BB7 RID: 2999
		public const int JoinlobbyApiLatest = 5;

		// Token: 0x04000BB8 RID: 3000
		public const int JoinlobbybyidApiLatest = 3;

		// Token: 0x04000BB9 RID: 3001
		public const int JoinrtcroomApiLatest = 1;

		// Token: 0x04000BBA RID: 3002
		public const int KickmemberApiLatest = 1;

		// Token: 0x04000BBB RID: 3003
		public const int LeavelobbyApiLatest = 1;

		// Token: 0x04000BBC RID: 3004
		public const int LeavertcroomApiLatest = 1;

		// Token: 0x04000BBD RID: 3005
		public const int LocalrtcoptionsApiLatest = 1;

		// Token: 0x04000BBE RID: 3006
		public const int MaxLobbies = 16;

		// Token: 0x04000BBF RID: 3007
		public const int MaxLobbyMembers = 64;

		// Token: 0x04000BC0 RID: 3008
		public const int MaxLobbyidoverrideLength = 60;

		// Token: 0x04000BC1 RID: 3009
		public const int MaxSearchResults = 200;

		// Token: 0x04000BC2 RID: 3010
		public const int MinLobbyidoverrideLength = 4;

		// Token: 0x04000BC3 RID: 3011
		public const int ParseconnectstringApiLatest = 1;

		// Token: 0x04000BC4 RID: 3012
		public const int ParseconnectstringBufferSize = 256;

		// Token: 0x04000BC5 RID: 3013
		public const int PromotememberApiLatest = 1;

		// Token: 0x04000BC6 RID: 3014
		public const int QueryinvitesApiLatest = 1;

		// Token: 0x04000BC7 RID: 3015
		public const int RejectinviteApiLatest = 1;

		// Token: 0x04000BC8 RID: 3016
		public static readonly Utf8String SearchBucketId = "bucket";

		// Token: 0x04000BC9 RID: 3017
		public static readonly Utf8String SearchMincurrentmembers = "mincurrentmembers";

		// Token: 0x04000BCA RID: 3018
		public static readonly Utf8String SearchMinslotsavailable = "minslotsavailable";

		// Token: 0x04000BCB RID: 3019
		public const int SendinviteApiLatest = 1;

		// Token: 0x04000BCC RID: 3020
		public const int UpdatelobbyApiLatest = 1;

		// Token: 0x04000BCD RID: 3021
		public const int UpdatelobbymodificationApiLatest = 1;
	}
}
