using System;

namespace Epic.OnlineServices.Sessions
{
	// Token: 0x02000188 RID: 392
	public sealed class SessionsInterface : Handle
	{
		// Token: 0x06000B55 RID: 2901 RVA: 0x00010151 File Offset: 0x0000E351
		public SessionsInterface()
		{
		}

		// Token: 0x06000B56 RID: 2902 RVA: 0x0001015B File Offset: 0x0000E35B
		public SessionsInterface(IntPtr innerHandle) : base(innerHandle)
		{
		}

		// Token: 0x06000B57 RID: 2903 RVA: 0x00010168 File Offset: 0x0000E368
		public ulong AddNotifyJoinSessionAccepted(ref AddNotifyJoinSessionAcceptedOptions options, object clientData, OnJoinSessionAcceptedCallback notificationFn)
		{
			AddNotifyJoinSessionAcceptedOptionsInternal addNotifyJoinSessionAcceptedOptionsInternal = default(AddNotifyJoinSessionAcceptedOptionsInternal);
			addNotifyJoinSessionAcceptedOptionsInternal.Set(ref options);
			IntPtr zero = IntPtr.Zero;
			OnJoinSessionAcceptedCallbackInternal onJoinSessionAcceptedCallbackInternal = new OnJoinSessionAcceptedCallbackInternal(SessionsInterface.OnJoinSessionAcceptedCallbackInternalImplementation);
			Helper.AddCallback(out zero, clientData, notificationFn, onJoinSessionAcceptedCallbackInternal, Array.Empty<Delegate>());
			ulong num = Bindings.EOS_Sessions_AddNotifyJoinSessionAccepted(base.InnerHandle, ref addNotifyJoinSessionAcceptedOptionsInternal, zero, onJoinSessionAcceptedCallbackInternal);
			Helper.Dispose<AddNotifyJoinSessionAcceptedOptionsInternal>(ref addNotifyJoinSessionAcceptedOptionsInternal);
			Helper.AssignNotificationIdToCallback(zero, num);
			return num;
		}

		// Token: 0x06000B58 RID: 2904 RVA: 0x000101D4 File Offset: 0x0000E3D4
		public ulong AddNotifyLeaveSessionRequested(ref AddNotifyLeaveSessionRequestedOptions options, object clientData, OnLeaveSessionRequestedCallback notificationFn)
		{
			AddNotifyLeaveSessionRequestedOptionsInternal addNotifyLeaveSessionRequestedOptionsInternal = default(AddNotifyLeaveSessionRequestedOptionsInternal);
			addNotifyLeaveSessionRequestedOptionsInternal.Set(ref options);
			IntPtr zero = IntPtr.Zero;
			OnLeaveSessionRequestedCallbackInternal onLeaveSessionRequestedCallbackInternal = new OnLeaveSessionRequestedCallbackInternal(SessionsInterface.OnLeaveSessionRequestedCallbackInternalImplementation);
			Helper.AddCallback(out zero, clientData, notificationFn, onLeaveSessionRequestedCallbackInternal, Array.Empty<Delegate>());
			ulong num = Bindings.EOS_Sessions_AddNotifyLeaveSessionRequested(base.InnerHandle, ref addNotifyLeaveSessionRequestedOptionsInternal, zero, onLeaveSessionRequestedCallbackInternal);
			Helper.Dispose<AddNotifyLeaveSessionRequestedOptionsInternal>(ref addNotifyLeaveSessionRequestedOptionsInternal);
			Helper.AssignNotificationIdToCallback(zero, num);
			return num;
		}

		// Token: 0x06000B59 RID: 2905 RVA: 0x00010240 File Offset: 0x0000E440
		public ulong AddNotifySendSessionNativeInviteRequested(ref AddNotifySendSessionNativeInviteRequestedOptions options, object clientData, OnSendSessionNativeInviteRequestedCallback notificationFn)
		{
			AddNotifySendSessionNativeInviteRequestedOptionsInternal addNotifySendSessionNativeInviteRequestedOptionsInternal = default(AddNotifySendSessionNativeInviteRequestedOptionsInternal);
			addNotifySendSessionNativeInviteRequestedOptionsInternal.Set(ref options);
			IntPtr zero = IntPtr.Zero;
			OnSendSessionNativeInviteRequestedCallbackInternal onSendSessionNativeInviteRequestedCallbackInternal = new OnSendSessionNativeInviteRequestedCallbackInternal(SessionsInterface.OnSendSessionNativeInviteRequestedCallbackInternalImplementation);
			Helper.AddCallback(out zero, clientData, notificationFn, onSendSessionNativeInviteRequestedCallbackInternal, Array.Empty<Delegate>());
			ulong num = Bindings.EOS_Sessions_AddNotifySendSessionNativeInviteRequested(base.InnerHandle, ref addNotifySendSessionNativeInviteRequestedOptionsInternal, zero, onSendSessionNativeInviteRequestedCallbackInternal);
			Helper.Dispose<AddNotifySendSessionNativeInviteRequestedOptionsInternal>(ref addNotifySendSessionNativeInviteRequestedOptionsInternal);
			Helper.AssignNotificationIdToCallback(zero, num);
			return num;
		}

		// Token: 0x06000B5A RID: 2906 RVA: 0x000102AC File Offset: 0x0000E4AC
		public ulong AddNotifySessionInviteAccepted(ref AddNotifySessionInviteAcceptedOptions options, object clientData, OnSessionInviteAcceptedCallback notificationFn)
		{
			AddNotifySessionInviteAcceptedOptionsInternal addNotifySessionInviteAcceptedOptionsInternal = default(AddNotifySessionInviteAcceptedOptionsInternal);
			addNotifySessionInviteAcceptedOptionsInternal.Set(ref options);
			IntPtr zero = IntPtr.Zero;
			OnSessionInviteAcceptedCallbackInternal onSessionInviteAcceptedCallbackInternal = new OnSessionInviteAcceptedCallbackInternal(SessionsInterface.OnSessionInviteAcceptedCallbackInternalImplementation);
			Helper.AddCallback(out zero, clientData, notificationFn, onSessionInviteAcceptedCallbackInternal, Array.Empty<Delegate>());
			ulong num = Bindings.EOS_Sessions_AddNotifySessionInviteAccepted(base.InnerHandle, ref addNotifySessionInviteAcceptedOptionsInternal, zero, onSessionInviteAcceptedCallbackInternal);
			Helper.Dispose<AddNotifySessionInviteAcceptedOptionsInternal>(ref addNotifySessionInviteAcceptedOptionsInternal);
			Helper.AssignNotificationIdToCallback(zero, num);
			return num;
		}

		// Token: 0x06000B5B RID: 2907 RVA: 0x00010318 File Offset: 0x0000E518
		public ulong AddNotifySessionInviteReceived(ref AddNotifySessionInviteReceivedOptions options, object clientData, OnSessionInviteReceivedCallback notificationFn)
		{
			AddNotifySessionInviteReceivedOptionsInternal addNotifySessionInviteReceivedOptionsInternal = default(AddNotifySessionInviteReceivedOptionsInternal);
			addNotifySessionInviteReceivedOptionsInternal.Set(ref options);
			IntPtr zero = IntPtr.Zero;
			OnSessionInviteReceivedCallbackInternal onSessionInviteReceivedCallbackInternal = new OnSessionInviteReceivedCallbackInternal(SessionsInterface.OnSessionInviteReceivedCallbackInternalImplementation);
			Helper.AddCallback(out zero, clientData, notificationFn, onSessionInviteReceivedCallbackInternal, Array.Empty<Delegate>());
			ulong num = Bindings.EOS_Sessions_AddNotifySessionInviteReceived(base.InnerHandle, ref addNotifySessionInviteReceivedOptionsInternal, zero, onSessionInviteReceivedCallbackInternal);
			Helper.Dispose<AddNotifySessionInviteReceivedOptionsInternal>(ref addNotifySessionInviteReceivedOptionsInternal);
			Helper.AssignNotificationIdToCallback(zero, num);
			return num;
		}

		// Token: 0x06000B5C RID: 2908 RVA: 0x00010384 File Offset: 0x0000E584
		public ulong AddNotifySessionInviteRejected(ref AddNotifySessionInviteRejectedOptions options, object clientData, OnSessionInviteRejectedCallback notificationFn)
		{
			AddNotifySessionInviteRejectedOptionsInternal addNotifySessionInviteRejectedOptionsInternal = default(AddNotifySessionInviteRejectedOptionsInternal);
			addNotifySessionInviteRejectedOptionsInternal.Set(ref options);
			IntPtr zero = IntPtr.Zero;
			OnSessionInviteRejectedCallbackInternal onSessionInviteRejectedCallbackInternal = new OnSessionInviteRejectedCallbackInternal(SessionsInterface.OnSessionInviteRejectedCallbackInternalImplementation);
			Helper.AddCallback(out zero, clientData, notificationFn, onSessionInviteRejectedCallbackInternal, Array.Empty<Delegate>());
			ulong num = Bindings.EOS_Sessions_AddNotifySessionInviteRejected(base.InnerHandle, ref addNotifySessionInviteRejectedOptionsInternal, zero, onSessionInviteRejectedCallbackInternal);
			Helper.Dispose<AddNotifySessionInviteRejectedOptionsInternal>(ref addNotifySessionInviteRejectedOptionsInternal);
			Helper.AssignNotificationIdToCallback(zero, num);
			return num;
		}

		// Token: 0x06000B5D RID: 2909 RVA: 0x000103F0 File Offset: 0x0000E5F0
		public Result CopyActiveSessionHandle(ref CopyActiveSessionHandleOptions options, out ActiveSession outSessionHandle)
		{
			CopyActiveSessionHandleOptionsInternal copyActiveSessionHandleOptionsInternal = default(CopyActiveSessionHandleOptionsInternal);
			copyActiveSessionHandleOptionsInternal.Set(ref options);
			IntPtr zero = IntPtr.Zero;
			Result result = Bindings.EOS_Sessions_CopyActiveSessionHandle(base.InnerHandle, ref copyActiveSessionHandleOptionsInternal, ref zero);
			Helper.Dispose<CopyActiveSessionHandleOptionsInternal>(ref copyActiveSessionHandleOptionsInternal);
			Helper.Get<ActiveSession>(zero, out outSessionHandle);
			return result;
		}

		// Token: 0x06000B5E RID: 2910 RVA: 0x0001043C File Offset: 0x0000E63C
		public Result CopySessionHandleByInviteId(ref CopySessionHandleByInviteIdOptions options, out SessionDetails outSessionHandle)
		{
			CopySessionHandleByInviteIdOptionsInternal copySessionHandleByInviteIdOptionsInternal = default(CopySessionHandleByInviteIdOptionsInternal);
			copySessionHandleByInviteIdOptionsInternal.Set(ref options);
			IntPtr zero = IntPtr.Zero;
			Result result = Bindings.EOS_Sessions_CopySessionHandleByInviteId(base.InnerHandle, ref copySessionHandleByInviteIdOptionsInternal, ref zero);
			Helper.Dispose<CopySessionHandleByInviteIdOptionsInternal>(ref copySessionHandleByInviteIdOptionsInternal);
			Helper.Get<SessionDetails>(zero, out outSessionHandle);
			return result;
		}

		// Token: 0x06000B5F RID: 2911 RVA: 0x00010488 File Offset: 0x0000E688
		public Result CopySessionHandleByUiEventId(ref CopySessionHandleByUiEventIdOptions options, out SessionDetails outSessionHandle)
		{
			CopySessionHandleByUiEventIdOptionsInternal copySessionHandleByUiEventIdOptionsInternal = default(CopySessionHandleByUiEventIdOptionsInternal);
			copySessionHandleByUiEventIdOptionsInternal.Set(ref options);
			IntPtr zero = IntPtr.Zero;
			Result result = Bindings.EOS_Sessions_CopySessionHandleByUiEventId(base.InnerHandle, ref copySessionHandleByUiEventIdOptionsInternal, ref zero);
			Helper.Dispose<CopySessionHandleByUiEventIdOptionsInternal>(ref copySessionHandleByUiEventIdOptionsInternal);
			Helper.Get<SessionDetails>(zero, out outSessionHandle);
			return result;
		}

		// Token: 0x06000B60 RID: 2912 RVA: 0x000104D4 File Offset: 0x0000E6D4
		public Result CopySessionHandleForPresence(ref CopySessionHandleForPresenceOptions options, out SessionDetails outSessionHandle)
		{
			CopySessionHandleForPresenceOptionsInternal copySessionHandleForPresenceOptionsInternal = default(CopySessionHandleForPresenceOptionsInternal);
			copySessionHandleForPresenceOptionsInternal.Set(ref options);
			IntPtr zero = IntPtr.Zero;
			Result result = Bindings.EOS_Sessions_CopySessionHandleForPresence(base.InnerHandle, ref copySessionHandleForPresenceOptionsInternal, ref zero);
			Helper.Dispose<CopySessionHandleForPresenceOptionsInternal>(ref copySessionHandleForPresenceOptionsInternal);
			Helper.Get<SessionDetails>(zero, out outSessionHandle);
			return result;
		}

		// Token: 0x06000B61 RID: 2913 RVA: 0x00010520 File Offset: 0x0000E720
		public Result CreateSessionModification(ref CreateSessionModificationOptions options, out SessionModification outSessionModificationHandle)
		{
			CreateSessionModificationOptionsInternal createSessionModificationOptionsInternal = default(CreateSessionModificationOptionsInternal);
			createSessionModificationOptionsInternal.Set(ref options);
			IntPtr zero = IntPtr.Zero;
			Result result = Bindings.EOS_Sessions_CreateSessionModification(base.InnerHandle, ref createSessionModificationOptionsInternal, ref zero);
			Helper.Dispose<CreateSessionModificationOptionsInternal>(ref createSessionModificationOptionsInternal);
			Helper.Get<SessionModification>(zero, out outSessionModificationHandle);
			return result;
		}

		// Token: 0x06000B62 RID: 2914 RVA: 0x0001056C File Offset: 0x0000E76C
		public Result CreateSessionSearch(ref CreateSessionSearchOptions options, out SessionSearch outSessionSearchHandle)
		{
			CreateSessionSearchOptionsInternal createSessionSearchOptionsInternal = default(CreateSessionSearchOptionsInternal);
			createSessionSearchOptionsInternal.Set(ref options);
			IntPtr zero = IntPtr.Zero;
			Result result = Bindings.EOS_Sessions_CreateSessionSearch(base.InnerHandle, ref createSessionSearchOptionsInternal, ref zero);
			Helper.Dispose<CreateSessionSearchOptionsInternal>(ref createSessionSearchOptionsInternal);
			Helper.Get<SessionSearch>(zero, out outSessionSearchHandle);
			return result;
		}

		// Token: 0x06000B63 RID: 2915 RVA: 0x000105B8 File Offset: 0x0000E7B8
		public void DestroySession(ref DestroySessionOptions options, object clientData, OnDestroySessionCallback completionDelegate)
		{
			DestroySessionOptionsInternal destroySessionOptionsInternal = default(DestroySessionOptionsInternal);
			destroySessionOptionsInternal.Set(ref options);
			IntPtr zero = IntPtr.Zero;
			OnDestroySessionCallbackInternal onDestroySessionCallbackInternal = new OnDestroySessionCallbackInternal(SessionsInterface.OnDestroySessionCallbackInternalImplementation);
			Helper.AddCallback(out zero, clientData, completionDelegate, onDestroySessionCallbackInternal, Array.Empty<Delegate>());
			Bindings.EOS_Sessions_DestroySession(base.InnerHandle, ref destroySessionOptionsInternal, zero, onDestroySessionCallbackInternal);
			Helper.Dispose<DestroySessionOptionsInternal>(ref destroySessionOptionsInternal);
		}

		// Token: 0x06000B64 RID: 2916 RVA: 0x00010614 File Offset: 0x0000E814
		public Result DumpSessionState(ref DumpSessionStateOptions options)
		{
			DumpSessionStateOptionsInternal dumpSessionStateOptionsInternal = default(DumpSessionStateOptionsInternal);
			dumpSessionStateOptionsInternal.Set(ref options);
			Result result = Bindings.EOS_Sessions_DumpSessionState(base.InnerHandle, ref dumpSessionStateOptionsInternal);
			Helper.Dispose<DumpSessionStateOptionsInternal>(ref dumpSessionStateOptionsInternal);
			return result;
		}

		// Token: 0x06000B65 RID: 2917 RVA: 0x00010650 File Offset: 0x0000E850
		public void EndSession(ref EndSessionOptions options, object clientData, OnEndSessionCallback completionDelegate)
		{
			EndSessionOptionsInternal endSessionOptionsInternal = default(EndSessionOptionsInternal);
			endSessionOptionsInternal.Set(ref options);
			IntPtr zero = IntPtr.Zero;
			OnEndSessionCallbackInternal onEndSessionCallbackInternal = new OnEndSessionCallbackInternal(SessionsInterface.OnEndSessionCallbackInternalImplementation);
			Helper.AddCallback(out zero, clientData, completionDelegate, onEndSessionCallbackInternal, Array.Empty<Delegate>());
			Bindings.EOS_Sessions_EndSession(base.InnerHandle, ref endSessionOptionsInternal, zero, onEndSessionCallbackInternal);
			Helper.Dispose<EndSessionOptionsInternal>(ref endSessionOptionsInternal);
		}

		// Token: 0x06000B66 RID: 2918 RVA: 0x000106AC File Offset: 0x0000E8AC
		public uint GetInviteCount(ref GetInviteCountOptions options)
		{
			GetInviteCountOptionsInternal getInviteCountOptionsInternal = default(GetInviteCountOptionsInternal);
			getInviteCountOptionsInternal.Set(ref options);
			uint result = Bindings.EOS_Sessions_GetInviteCount(base.InnerHandle, ref getInviteCountOptionsInternal);
			Helper.Dispose<GetInviteCountOptionsInternal>(ref getInviteCountOptionsInternal);
			return result;
		}

		// Token: 0x06000B67 RID: 2919 RVA: 0x000106E8 File Offset: 0x0000E8E8
		public Result GetInviteIdByIndex(ref GetInviteIdByIndexOptions options, out Utf8String outBuffer)
		{
			GetInviteIdByIndexOptionsInternal getInviteIdByIndexOptionsInternal = default(GetInviteIdByIndexOptionsInternal);
			getInviteIdByIndexOptionsInternal.Set(ref options);
			int size = 65;
			IntPtr intPtr = Helper.AddAllocation(size);
			Result result = Bindings.EOS_Sessions_GetInviteIdByIndex(base.InnerHandle, ref getInviteIdByIndexOptionsInternal, intPtr, ref size);
			Helper.Dispose<GetInviteIdByIndexOptionsInternal>(ref getInviteIdByIndexOptionsInternal);
			Helper.Get(intPtr, out outBuffer);
			Helper.Dispose(ref intPtr);
			return result;
		}

		// Token: 0x06000B68 RID: 2920 RVA: 0x00010744 File Offset: 0x0000E944
		public Result IsUserInSession(ref IsUserInSessionOptions options)
		{
			IsUserInSessionOptionsInternal isUserInSessionOptionsInternal = default(IsUserInSessionOptionsInternal);
			isUserInSessionOptionsInternal.Set(ref options);
			Result result = Bindings.EOS_Sessions_IsUserInSession(base.InnerHandle, ref isUserInSessionOptionsInternal);
			Helper.Dispose<IsUserInSessionOptionsInternal>(ref isUserInSessionOptionsInternal);
			return result;
		}

		// Token: 0x06000B69 RID: 2921 RVA: 0x00010780 File Offset: 0x0000E980
		public void JoinSession(ref JoinSessionOptions options, object clientData, OnJoinSessionCallback completionDelegate)
		{
			JoinSessionOptionsInternal joinSessionOptionsInternal = default(JoinSessionOptionsInternal);
			joinSessionOptionsInternal.Set(ref options);
			IntPtr zero = IntPtr.Zero;
			OnJoinSessionCallbackInternal onJoinSessionCallbackInternal = new OnJoinSessionCallbackInternal(SessionsInterface.OnJoinSessionCallbackInternalImplementation);
			Helper.AddCallback(out zero, clientData, completionDelegate, onJoinSessionCallbackInternal, Array.Empty<Delegate>());
			Bindings.EOS_Sessions_JoinSession(base.InnerHandle, ref joinSessionOptionsInternal, zero, onJoinSessionCallbackInternal);
			Helper.Dispose<JoinSessionOptionsInternal>(ref joinSessionOptionsInternal);
		}

		// Token: 0x06000B6A RID: 2922 RVA: 0x000107DC File Offset: 0x0000E9DC
		public void QueryInvites(ref QueryInvitesOptions options, object clientData, OnQueryInvitesCallback completionDelegate)
		{
			QueryInvitesOptionsInternal queryInvitesOptionsInternal = default(QueryInvitesOptionsInternal);
			queryInvitesOptionsInternal.Set(ref options);
			IntPtr zero = IntPtr.Zero;
			OnQueryInvitesCallbackInternal onQueryInvitesCallbackInternal = new OnQueryInvitesCallbackInternal(SessionsInterface.OnQueryInvitesCallbackInternalImplementation);
			Helper.AddCallback(out zero, clientData, completionDelegate, onQueryInvitesCallbackInternal, Array.Empty<Delegate>());
			Bindings.EOS_Sessions_QueryInvites(base.InnerHandle, ref queryInvitesOptionsInternal, zero, onQueryInvitesCallbackInternal);
			Helper.Dispose<QueryInvitesOptionsInternal>(ref queryInvitesOptionsInternal);
		}

		// Token: 0x06000B6B RID: 2923 RVA: 0x00010838 File Offset: 0x0000EA38
		public void RegisterPlayers(ref RegisterPlayersOptions options, object clientData, OnRegisterPlayersCallback completionDelegate)
		{
			RegisterPlayersOptionsInternal registerPlayersOptionsInternal = default(RegisterPlayersOptionsInternal);
			registerPlayersOptionsInternal.Set(ref options);
			IntPtr zero = IntPtr.Zero;
			OnRegisterPlayersCallbackInternal onRegisterPlayersCallbackInternal = new OnRegisterPlayersCallbackInternal(SessionsInterface.OnRegisterPlayersCallbackInternalImplementation);
			Helper.AddCallback(out zero, clientData, completionDelegate, onRegisterPlayersCallbackInternal, Array.Empty<Delegate>());
			Bindings.EOS_Sessions_RegisterPlayers(base.InnerHandle, ref registerPlayersOptionsInternal, zero, onRegisterPlayersCallbackInternal);
			Helper.Dispose<RegisterPlayersOptionsInternal>(ref registerPlayersOptionsInternal);
		}

		// Token: 0x06000B6C RID: 2924 RVA: 0x00010894 File Offset: 0x0000EA94
		public void RejectInvite(ref RejectInviteOptions options, object clientData, OnRejectInviteCallback completionDelegate)
		{
			RejectInviteOptionsInternal rejectInviteOptionsInternal = default(RejectInviteOptionsInternal);
			rejectInviteOptionsInternal.Set(ref options);
			IntPtr zero = IntPtr.Zero;
			OnRejectInviteCallbackInternal onRejectInviteCallbackInternal = new OnRejectInviteCallbackInternal(SessionsInterface.OnRejectInviteCallbackInternalImplementation);
			Helper.AddCallback(out zero, clientData, completionDelegate, onRejectInviteCallbackInternal, Array.Empty<Delegate>());
			Bindings.EOS_Sessions_RejectInvite(base.InnerHandle, ref rejectInviteOptionsInternal, zero, onRejectInviteCallbackInternal);
			Helper.Dispose<RejectInviteOptionsInternal>(ref rejectInviteOptionsInternal);
		}

		// Token: 0x06000B6D RID: 2925 RVA: 0x000108EE File Offset: 0x0000EAEE
		public void RemoveNotifyJoinSessionAccepted(ulong inId)
		{
			Bindings.EOS_Sessions_RemoveNotifyJoinSessionAccepted(base.InnerHandle, inId);
			Helper.RemoveCallbackByNotificationId(inId);
		}

		// Token: 0x06000B6E RID: 2926 RVA: 0x00010905 File Offset: 0x0000EB05
		public void RemoveNotifyLeaveSessionRequested(ulong inId)
		{
			Bindings.EOS_Sessions_RemoveNotifyLeaveSessionRequested(base.InnerHandle, inId);
			Helper.RemoveCallbackByNotificationId(inId);
		}

		// Token: 0x06000B6F RID: 2927 RVA: 0x0001091C File Offset: 0x0000EB1C
		public void RemoveNotifySendSessionNativeInviteRequested(ulong inId)
		{
			Bindings.EOS_Sessions_RemoveNotifySendSessionNativeInviteRequested(base.InnerHandle, inId);
			Helper.RemoveCallbackByNotificationId(inId);
		}

		// Token: 0x06000B70 RID: 2928 RVA: 0x00010933 File Offset: 0x0000EB33
		public void RemoveNotifySessionInviteAccepted(ulong inId)
		{
			Bindings.EOS_Sessions_RemoveNotifySessionInviteAccepted(base.InnerHandle, inId);
			Helper.RemoveCallbackByNotificationId(inId);
		}

		// Token: 0x06000B71 RID: 2929 RVA: 0x0001094A File Offset: 0x0000EB4A
		public void RemoveNotifySessionInviteReceived(ulong inId)
		{
			Bindings.EOS_Sessions_RemoveNotifySessionInviteReceived(base.InnerHandle, inId);
			Helper.RemoveCallbackByNotificationId(inId);
		}

		// Token: 0x06000B72 RID: 2930 RVA: 0x00010961 File Offset: 0x0000EB61
		public void RemoveNotifySessionInviteRejected(ulong inId)
		{
			Bindings.EOS_Sessions_RemoveNotifySessionInviteRejected(base.InnerHandle, inId);
			Helper.RemoveCallbackByNotificationId(inId);
		}

		// Token: 0x06000B73 RID: 2931 RVA: 0x00010978 File Offset: 0x0000EB78
		public void SendInvite(ref SendInviteOptions options, object clientData, OnSendInviteCallback completionDelegate)
		{
			SendInviteOptionsInternal sendInviteOptionsInternal = default(SendInviteOptionsInternal);
			sendInviteOptionsInternal.Set(ref options);
			IntPtr zero = IntPtr.Zero;
			OnSendInviteCallbackInternal onSendInviteCallbackInternal = new OnSendInviteCallbackInternal(SessionsInterface.OnSendInviteCallbackInternalImplementation);
			Helper.AddCallback(out zero, clientData, completionDelegate, onSendInviteCallbackInternal, Array.Empty<Delegate>());
			Bindings.EOS_Sessions_SendInvite(base.InnerHandle, ref sendInviteOptionsInternal, zero, onSendInviteCallbackInternal);
			Helper.Dispose<SendInviteOptionsInternal>(ref sendInviteOptionsInternal);
		}

		// Token: 0x06000B74 RID: 2932 RVA: 0x000109D4 File Offset: 0x0000EBD4
		public void StartSession(ref StartSessionOptions options, object clientData, OnStartSessionCallback completionDelegate)
		{
			StartSessionOptionsInternal startSessionOptionsInternal = default(StartSessionOptionsInternal);
			startSessionOptionsInternal.Set(ref options);
			IntPtr zero = IntPtr.Zero;
			OnStartSessionCallbackInternal onStartSessionCallbackInternal = new OnStartSessionCallbackInternal(SessionsInterface.OnStartSessionCallbackInternalImplementation);
			Helper.AddCallback(out zero, clientData, completionDelegate, onStartSessionCallbackInternal, Array.Empty<Delegate>());
			Bindings.EOS_Sessions_StartSession(base.InnerHandle, ref startSessionOptionsInternal, zero, onStartSessionCallbackInternal);
			Helper.Dispose<StartSessionOptionsInternal>(ref startSessionOptionsInternal);
		}

		// Token: 0x06000B75 RID: 2933 RVA: 0x00010A30 File Offset: 0x0000EC30
		public void UnregisterPlayers(ref UnregisterPlayersOptions options, object clientData, OnUnregisterPlayersCallback completionDelegate)
		{
			UnregisterPlayersOptionsInternal unregisterPlayersOptionsInternal = default(UnregisterPlayersOptionsInternal);
			unregisterPlayersOptionsInternal.Set(ref options);
			IntPtr zero = IntPtr.Zero;
			OnUnregisterPlayersCallbackInternal onUnregisterPlayersCallbackInternal = new OnUnregisterPlayersCallbackInternal(SessionsInterface.OnUnregisterPlayersCallbackInternalImplementation);
			Helper.AddCallback(out zero, clientData, completionDelegate, onUnregisterPlayersCallbackInternal, Array.Empty<Delegate>());
			Bindings.EOS_Sessions_UnregisterPlayers(base.InnerHandle, ref unregisterPlayersOptionsInternal, zero, onUnregisterPlayersCallbackInternal);
			Helper.Dispose<UnregisterPlayersOptionsInternal>(ref unregisterPlayersOptionsInternal);
		}

		// Token: 0x06000B76 RID: 2934 RVA: 0x00010A8C File Offset: 0x0000EC8C
		public void UpdateSession(ref UpdateSessionOptions options, object clientData, OnUpdateSessionCallback completionDelegate)
		{
			UpdateSessionOptionsInternal updateSessionOptionsInternal = default(UpdateSessionOptionsInternal);
			updateSessionOptionsInternal.Set(ref options);
			IntPtr zero = IntPtr.Zero;
			OnUpdateSessionCallbackInternal onUpdateSessionCallbackInternal = new OnUpdateSessionCallbackInternal(SessionsInterface.OnUpdateSessionCallbackInternalImplementation);
			Helper.AddCallback(out zero, clientData, completionDelegate, onUpdateSessionCallbackInternal, Array.Empty<Delegate>());
			Bindings.EOS_Sessions_UpdateSession(base.InnerHandle, ref updateSessionOptionsInternal, zero, onUpdateSessionCallbackInternal);
			Helper.Dispose<UpdateSessionOptionsInternal>(ref updateSessionOptionsInternal);
		}

		// Token: 0x06000B77 RID: 2935 RVA: 0x00010AE8 File Offset: 0x0000ECE8
		public Result UpdateSessionModification(ref UpdateSessionModificationOptions options, out SessionModification outSessionModificationHandle)
		{
			UpdateSessionModificationOptionsInternal updateSessionModificationOptionsInternal = default(UpdateSessionModificationOptionsInternal);
			updateSessionModificationOptionsInternal.Set(ref options);
			IntPtr zero = IntPtr.Zero;
			Result result = Bindings.EOS_Sessions_UpdateSessionModification(base.InnerHandle, ref updateSessionModificationOptionsInternal, ref zero);
			Helper.Dispose<UpdateSessionModificationOptionsInternal>(ref updateSessionModificationOptionsInternal);
			Helper.Get<SessionModification>(zero, out outSessionModificationHandle);
			return result;
		}

		// Token: 0x06000B78 RID: 2936 RVA: 0x00010B34 File Offset: 0x0000ED34
		[MonoPInvokeCallback(typeof(OnDestroySessionCallbackInternal))]
		internal static void OnDestroySessionCallbackInternalImplementation(ref DestroySessionCallbackInfoInternal data)
		{
			OnDestroySessionCallback onDestroySessionCallback;
			DestroySessionCallbackInfo destroySessionCallbackInfo;
			bool flag = Helper.TryGetAndRemoveCallback<DestroySessionCallbackInfoInternal, OnDestroySessionCallback, DestroySessionCallbackInfo>(ref data, out onDestroySessionCallback, out destroySessionCallbackInfo);
			if (flag)
			{
				onDestroySessionCallback(ref destroySessionCallbackInfo);
			}
		}

		// Token: 0x06000B79 RID: 2937 RVA: 0x00010B5C File Offset: 0x0000ED5C
		[MonoPInvokeCallback(typeof(OnEndSessionCallbackInternal))]
		internal static void OnEndSessionCallbackInternalImplementation(ref EndSessionCallbackInfoInternal data)
		{
			OnEndSessionCallback onEndSessionCallback;
			EndSessionCallbackInfo endSessionCallbackInfo;
			bool flag = Helper.TryGetAndRemoveCallback<EndSessionCallbackInfoInternal, OnEndSessionCallback, EndSessionCallbackInfo>(ref data, out onEndSessionCallback, out endSessionCallbackInfo);
			if (flag)
			{
				onEndSessionCallback(ref endSessionCallbackInfo);
			}
		}

		// Token: 0x06000B7A RID: 2938 RVA: 0x00010B84 File Offset: 0x0000ED84
		[MonoPInvokeCallback(typeof(OnJoinSessionAcceptedCallbackInternal))]
		internal static void OnJoinSessionAcceptedCallbackInternalImplementation(ref JoinSessionAcceptedCallbackInfoInternal data)
		{
			OnJoinSessionAcceptedCallback onJoinSessionAcceptedCallback;
			JoinSessionAcceptedCallbackInfo joinSessionAcceptedCallbackInfo;
			bool flag = Helper.TryGetCallback<JoinSessionAcceptedCallbackInfoInternal, OnJoinSessionAcceptedCallback, JoinSessionAcceptedCallbackInfo>(ref data, out onJoinSessionAcceptedCallback, out joinSessionAcceptedCallbackInfo);
			if (flag)
			{
				onJoinSessionAcceptedCallback(ref joinSessionAcceptedCallbackInfo);
			}
		}

		// Token: 0x06000B7B RID: 2939 RVA: 0x00010BAC File Offset: 0x0000EDAC
		[MonoPInvokeCallback(typeof(OnJoinSessionCallbackInternal))]
		internal static void OnJoinSessionCallbackInternalImplementation(ref JoinSessionCallbackInfoInternal data)
		{
			OnJoinSessionCallback onJoinSessionCallback;
			JoinSessionCallbackInfo joinSessionCallbackInfo;
			bool flag = Helper.TryGetAndRemoveCallback<JoinSessionCallbackInfoInternal, OnJoinSessionCallback, JoinSessionCallbackInfo>(ref data, out onJoinSessionCallback, out joinSessionCallbackInfo);
			if (flag)
			{
				onJoinSessionCallback(ref joinSessionCallbackInfo);
			}
		}

		// Token: 0x06000B7C RID: 2940 RVA: 0x00010BD4 File Offset: 0x0000EDD4
		[MonoPInvokeCallback(typeof(OnLeaveSessionRequestedCallbackInternal))]
		internal static void OnLeaveSessionRequestedCallbackInternalImplementation(ref LeaveSessionRequestedCallbackInfoInternal data)
		{
			OnLeaveSessionRequestedCallback onLeaveSessionRequestedCallback;
			LeaveSessionRequestedCallbackInfo leaveSessionRequestedCallbackInfo;
			bool flag = Helper.TryGetCallback<LeaveSessionRequestedCallbackInfoInternal, OnLeaveSessionRequestedCallback, LeaveSessionRequestedCallbackInfo>(ref data, out onLeaveSessionRequestedCallback, out leaveSessionRequestedCallbackInfo);
			if (flag)
			{
				onLeaveSessionRequestedCallback(ref leaveSessionRequestedCallbackInfo);
			}
		}

		// Token: 0x06000B7D RID: 2941 RVA: 0x00010BFC File Offset: 0x0000EDFC
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

		// Token: 0x06000B7E RID: 2942 RVA: 0x00010C24 File Offset: 0x0000EE24
		[MonoPInvokeCallback(typeof(OnRegisterPlayersCallbackInternal))]
		internal static void OnRegisterPlayersCallbackInternalImplementation(ref RegisterPlayersCallbackInfoInternal data)
		{
			OnRegisterPlayersCallback onRegisterPlayersCallback;
			RegisterPlayersCallbackInfo registerPlayersCallbackInfo;
			bool flag = Helper.TryGetAndRemoveCallback<RegisterPlayersCallbackInfoInternal, OnRegisterPlayersCallback, RegisterPlayersCallbackInfo>(ref data, out onRegisterPlayersCallback, out registerPlayersCallbackInfo);
			if (flag)
			{
				onRegisterPlayersCallback(ref registerPlayersCallbackInfo);
			}
		}

		// Token: 0x06000B7F RID: 2943 RVA: 0x00010C4C File Offset: 0x0000EE4C
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

		// Token: 0x06000B80 RID: 2944 RVA: 0x00010C74 File Offset: 0x0000EE74
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

		// Token: 0x06000B81 RID: 2945 RVA: 0x00010C9C File Offset: 0x0000EE9C
		[MonoPInvokeCallback(typeof(OnSendSessionNativeInviteRequestedCallbackInternal))]
		internal static void OnSendSessionNativeInviteRequestedCallbackInternalImplementation(ref SendSessionNativeInviteRequestedCallbackInfoInternal data)
		{
			OnSendSessionNativeInviteRequestedCallback onSendSessionNativeInviteRequestedCallback;
			SendSessionNativeInviteRequestedCallbackInfo sendSessionNativeInviteRequestedCallbackInfo;
			bool flag = Helper.TryGetCallback<SendSessionNativeInviteRequestedCallbackInfoInternal, OnSendSessionNativeInviteRequestedCallback, SendSessionNativeInviteRequestedCallbackInfo>(ref data, out onSendSessionNativeInviteRequestedCallback, out sendSessionNativeInviteRequestedCallbackInfo);
			if (flag)
			{
				onSendSessionNativeInviteRequestedCallback(ref sendSessionNativeInviteRequestedCallbackInfo);
			}
		}

		// Token: 0x06000B82 RID: 2946 RVA: 0x00010CC4 File Offset: 0x0000EEC4
		[MonoPInvokeCallback(typeof(OnSessionInviteAcceptedCallbackInternal))]
		internal static void OnSessionInviteAcceptedCallbackInternalImplementation(ref SessionInviteAcceptedCallbackInfoInternal data)
		{
			OnSessionInviteAcceptedCallback onSessionInviteAcceptedCallback;
			SessionInviteAcceptedCallbackInfo sessionInviteAcceptedCallbackInfo;
			bool flag = Helper.TryGetCallback<SessionInviteAcceptedCallbackInfoInternal, OnSessionInviteAcceptedCallback, SessionInviteAcceptedCallbackInfo>(ref data, out onSessionInviteAcceptedCallback, out sessionInviteAcceptedCallbackInfo);
			if (flag)
			{
				onSessionInviteAcceptedCallback(ref sessionInviteAcceptedCallbackInfo);
			}
		}

		// Token: 0x06000B83 RID: 2947 RVA: 0x00010CEC File Offset: 0x0000EEEC
		[MonoPInvokeCallback(typeof(OnSessionInviteReceivedCallbackInternal))]
		internal static void OnSessionInviteReceivedCallbackInternalImplementation(ref SessionInviteReceivedCallbackInfoInternal data)
		{
			OnSessionInviteReceivedCallback onSessionInviteReceivedCallback;
			SessionInviteReceivedCallbackInfo sessionInviteReceivedCallbackInfo;
			bool flag = Helper.TryGetCallback<SessionInviteReceivedCallbackInfoInternal, OnSessionInviteReceivedCallback, SessionInviteReceivedCallbackInfo>(ref data, out onSessionInviteReceivedCallback, out sessionInviteReceivedCallbackInfo);
			if (flag)
			{
				onSessionInviteReceivedCallback(ref sessionInviteReceivedCallbackInfo);
			}
		}

		// Token: 0x06000B84 RID: 2948 RVA: 0x00010D14 File Offset: 0x0000EF14
		[MonoPInvokeCallback(typeof(OnSessionInviteRejectedCallbackInternal))]
		internal static void OnSessionInviteRejectedCallbackInternalImplementation(ref SessionInviteRejectedCallbackInfoInternal data)
		{
			OnSessionInviteRejectedCallback onSessionInviteRejectedCallback;
			SessionInviteRejectedCallbackInfo sessionInviteRejectedCallbackInfo;
			bool flag = Helper.TryGetCallback<SessionInviteRejectedCallbackInfoInternal, OnSessionInviteRejectedCallback, SessionInviteRejectedCallbackInfo>(ref data, out onSessionInviteRejectedCallback, out sessionInviteRejectedCallbackInfo);
			if (flag)
			{
				onSessionInviteRejectedCallback(ref sessionInviteRejectedCallbackInfo);
			}
		}

		// Token: 0x06000B85 RID: 2949 RVA: 0x00010D3C File Offset: 0x0000EF3C
		[MonoPInvokeCallback(typeof(OnStartSessionCallbackInternal))]
		internal static void OnStartSessionCallbackInternalImplementation(ref StartSessionCallbackInfoInternal data)
		{
			OnStartSessionCallback onStartSessionCallback;
			StartSessionCallbackInfo startSessionCallbackInfo;
			bool flag = Helper.TryGetAndRemoveCallback<StartSessionCallbackInfoInternal, OnStartSessionCallback, StartSessionCallbackInfo>(ref data, out onStartSessionCallback, out startSessionCallbackInfo);
			if (flag)
			{
				onStartSessionCallback(ref startSessionCallbackInfo);
			}
		}

		// Token: 0x06000B86 RID: 2950 RVA: 0x00010D64 File Offset: 0x0000EF64
		[MonoPInvokeCallback(typeof(OnUnregisterPlayersCallbackInternal))]
		internal static void OnUnregisterPlayersCallbackInternalImplementation(ref UnregisterPlayersCallbackInfoInternal data)
		{
			OnUnregisterPlayersCallback onUnregisterPlayersCallback;
			UnregisterPlayersCallbackInfo unregisterPlayersCallbackInfo;
			bool flag = Helper.TryGetAndRemoveCallback<UnregisterPlayersCallbackInfoInternal, OnUnregisterPlayersCallback, UnregisterPlayersCallbackInfo>(ref data, out onUnregisterPlayersCallback, out unregisterPlayersCallbackInfo);
			if (flag)
			{
				onUnregisterPlayersCallback(ref unregisterPlayersCallbackInfo);
			}
		}

		// Token: 0x06000B87 RID: 2951 RVA: 0x00010D8C File Offset: 0x0000EF8C
		[MonoPInvokeCallback(typeof(OnUpdateSessionCallbackInternal))]
		internal static void OnUpdateSessionCallbackInternalImplementation(ref UpdateSessionCallbackInfoInternal data)
		{
			OnUpdateSessionCallback onUpdateSessionCallback;
			UpdateSessionCallbackInfo updateSessionCallbackInfo;
			bool flag = Helper.TryGetAndRemoveCallback<UpdateSessionCallbackInfoInternal, OnUpdateSessionCallback, UpdateSessionCallbackInfo>(ref data, out onUpdateSessionCallback, out updateSessionCallbackInfo);
			if (flag)
			{
				onUpdateSessionCallback(ref updateSessionCallbackInfo);
			}
		}

		// Token: 0x0400052A RID: 1322
		public const int AddnotifyjoinsessionacceptedApiLatest = 1;

		// Token: 0x0400052B RID: 1323
		public const int AddnotifyleavesessionrequestedApiLatest = 1;

		// Token: 0x0400052C RID: 1324
		public const int AddnotifysendsessionnativeinviterequestedApiLatest = 1;

		// Token: 0x0400052D RID: 1325
		public const int AddnotifysessioninviteacceptedApiLatest = 1;

		// Token: 0x0400052E RID: 1326
		public const int AddnotifysessioninvitereceivedApiLatest = 1;

		// Token: 0x0400052F RID: 1327
		public const int AddnotifysessioninviterejectedApiLatest = 1;

		// Token: 0x04000530 RID: 1328
		public const int AttributedataApiLatest = 1;

		// Token: 0x04000531 RID: 1329
		public const int CopyactivesessionhandleApiLatest = 1;

		// Token: 0x04000532 RID: 1330
		public const int CopysessionhandlebyinviteidApiLatest = 1;

		// Token: 0x04000533 RID: 1331
		public const int CopysessionhandlebyuieventidApiLatest = 1;

		// Token: 0x04000534 RID: 1332
		public const int CopysessionhandleforpresenceApiLatest = 1;

		// Token: 0x04000535 RID: 1333
		public const int CreatesessionmodificationApiLatest = 5;

		// Token: 0x04000536 RID: 1334
		public const int CreatesessionsearchApiLatest = 1;

		// Token: 0x04000537 RID: 1335
		public const int DestroysessionApiLatest = 1;

		// Token: 0x04000538 RID: 1336
		public const int DumpsessionstateApiLatest = 1;

		// Token: 0x04000539 RID: 1337
		public const int EndsessionApiLatest = 1;

		// Token: 0x0400053A RID: 1338
		public const int GetinvitecountApiLatest = 1;

		// Token: 0x0400053B RID: 1339
		public const int GetinviteidbyindexApiLatest = 1;

		// Token: 0x0400053C RID: 1340
		public const int InviteidMaxLength = 64;

		// Token: 0x0400053D RID: 1341
		public const int IsuserinsessionApiLatest = 1;

		// Token: 0x0400053E RID: 1342
		public const int JoinsessionApiLatest = 2;

		// Token: 0x0400053F RID: 1343
		public const int MaxSearchResults = 200;

		// Token: 0x04000540 RID: 1344
		public const int Maxregisteredplayers = 1000;

		// Token: 0x04000541 RID: 1345
		public const int QueryinvitesApiLatest = 1;

		// Token: 0x04000542 RID: 1346
		public const int RegisterplayersApiLatest = 3;

		// Token: 0x04000543 RID: 1347
		public const int RejectinviteApiLatest = 1;

		// Token: 0x04000544 RID: 1348
		public static readonly Utf8String SearchBucketId = "bucket";

		// Token: 0x04000545 RID: 1349
		public static readonly Utf8String SearchEmptyServersOnly = "emptyonly";

		// Token: 0x04000546 RID: 1350
		public static readonly Utf8String SearchMinslotsavailable = "minslotsavailable";

		// Token: 0x04000547 RID: 1351
		public static readonly Utf8String SearchNonemptyServersOnly = "nonemptyonly";

		// Token: 0x04000548 RID: 1352
		public const int SendinviteApiLatest = 1;

		// Token: 0x04000549 RID: 1353
		public const int SessionattributeApiLatest = 1;

		// Token: 0x0400054A RID: 1354
		public const int SessionattributedataApiLatest = 1;

		// Token: 0x0400054B RID: 1355
		public const int StartsessionApiLatest = 1;

		// Token: 0x0400054C RID: 1356
		public const int UnregisterplayersApiLatest = 2;

		// Token: 0x0400054D RID: 1357
		public const int UpdatesessionApiLatest = 1;

		// Token: 0x0400054E RID: 1358
		public const int UpdatesessionmodificationApiLatest = 1;
	}
}
