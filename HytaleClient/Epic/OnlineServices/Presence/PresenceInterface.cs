using System;

namespace Epic.OnlineServices.Presence
{
	// Token: 0x020002D2 RID: 722
	public sealed class PresenceInterface : Handle
	{
		// Token: 0x060013F4 RID: 5108 RVA: 0x0001D006 File Offset: 0x0001B206
		public PresenceInterface()
		{
		}

		// Token: 0x060013F5 RID: 5109 RVA: 0x0001D010 File Offset: 0x0001B210
		public PresenceInterface(IntPtr innerHandle) : base(innerHandle)
		{
		}

		// Token: 0x060013F6 RID: 5110 RVA: 0x0001D01C File Offset: 0x0001B21C
		public ulong AddNotifyJoinGameAccepted(ref AddNotifyJoinGameAcceptedOptions options, object clientData, OnJoinGameAcceptedCallback notificationFn)
		{
			AddNotifyJoinGameAcceptedOptionsInternal addNotifyJoinGameAcceptedOptionsInternal = default(AddNotifyJoinGameAcceptedOptionsInternal);
			addNotifyJoinGameAcceptedOptionsInternal.Set(ref options);
			IntPtr zero = IntPtr.Zero;
			OnJoinGameAcceptedCallbackInternal onJoinGameAcceptedCallbackInternal = new OnJoinGameAcceptedCallbackInternal(PresenceInterface.OnJoinGameAcceptedCallbackInternalImplementation);
			Helper.AddCallback(out zero, clientData, notificationFn, onJoinGameAcceptedCallbackInternal, Array.Empty<Delegate>());
			ulong num = Bindings.EOS_Presence_AddNotifyJoinGameAccepted(base.InnerHandle, ref addNotifyJoinGameAcceptedOptionsInternal, zero, onJoinGameAcceptedCallbackInternal);
			Helper.Dispose<AddNotifyJoinGameAcceptedOptionsInternal>(ref addNotifyJoinGameAcceptedOptionsInternal);
			Helper.AssignNotificationIdToCallback(zero, num);
			return num;
		}

		// Token: 0x060013F7 RID: 5111 RVA: 0x0001D088 File Offset: 0x0001B288
		public ulong AddNotifyOnPresenceChanged(ref AddNotifyOnPresenceChangedOptions options, object clientData, OnPresenceChangedCallback notificationHandler)
		{
			AddNotifyOnPresenceChangedOptionsInternal addNotifyOnPresenceChangedOptionsInternal = default(AddNotifyOnPresenceChangedOptionsInternal);
			addNotifyOnPresenceChangedOptionsInternal.Set(ref options);
			IntPtr zero = IntPtr.Zero;
			OnPresenceChangedCallbackInternal onPresenceChangedCallbackInternal = new OnPresenceChangedCallbackInternal(PresenceInterface.OnPresenceChangedCallbackInternalImplementation);
			Helper.AddCallback(out zero, clientData, notificationHandler, onPresenceChangedCallbackInternal, Array.Empty<Delegate>());
			ulong num = Bindings.EOS_Presence_AddNotifyOnPresenceChanged(base.InnerHandle, ref addNotifyOnPresenceChangedOptionsInternal, zero, onPresenceChangedCallbackInternal);
			Helper.Dispose<AddNotifyOnPresenceChangedOptionsInternal>(ref addNotifyOnPresenceChangedOptionsInternal);
			Helper.AssignNotificationIdToCallback(zero, num);
			return num;
		}

		// Token: 0x060013F8 RID: 5112 RVA: 0x0001D0F4 File Offset: 0x0001B2F4
		public Result CopyPresence(ref CopyPresenceOptions options, out Info? outPresence)
		{
			CopyPresenceOptionsInternal copyPresenceOptionsInternal = default(CopyPresenceOptionsInternal);
			copyPresenceOptionsInternal.Set(ref options);
			IntPtr zero = IntPtr.Zero;
			Result result = Bindings.EOS_Presence_CopyPresence(base.InnerHandle, ref copyPresenceOptionsInternal, ref zero);
			Helper.Dispose<CopyPresenceOptionsInternal>(ref copyPresenceOptionsInternal);
			Helper.Get<InfoInternal, Info>(zero, out outPresence);
			bool flag = outPresence != null;
			if (flag)
			{
				Bindings.EOS_Presence_Info_Release(zero);
			}
			return result;
		}

		// Token: 0x060013F9 RID: 5113 RVA: 0x0001D154 File Offset: 0x0001B354
		public Result CreatePresenceModification(ref CreatePresenceModificationOptions options, out PresenceModification outPresenceModificationHandle)
		{
			CreatePresenceModificationOptionsInternal createPresenceModificationOptionsInternal = default(CreatePresenceModificationOptionsInternal);
			createPresenceModificationOptionsInternal.Set(ref options);
			IntPtr zero = IntPtr.Zero;
			Result result = Bindings.EOS_Presence_CreatePresenceModification(base.InnerHandle, ref createPresenceModificationOptionsInternal, ref zero);
			Helper.Dispose<CreatePresenceModificationOptionsInternal>(ref createPresenceModificationOptionsInternal);
			Helper.Get<PresenceModification>(zero, out outPresenceModificationHandle);
			return result;
		}

		// Token: 0x060013FA RID: 5114 RVA: 0x0001D1A0 File Offset: 0x0001B3A0
		public Result GetJoinInfo(ref GetJoinInfoOptions options, out Utf8String outBuffer)
		{
			GetJoinInfoOptionsInternal getJoinInfoOptionsInternal = default(GetJoinInfoOptionsInternal);
			getJoinInfoOptionsInternal.Set(ref options);
			int size = 256;
			IntPtr intPtr = Helper.AddAllocation(size);
			Result result = Bindings.EOS_Presence_GetJoinInfo(base.InnerHandle, ref getJoinInfoOptionsInternal, intPtr, ref size);
			Helper.Dispose<GetJoinInfoOptionsInternal>(ref getJoinInfoOptionsInternal);
			Helper.Get(intPtr, out outBuffer);
			Helper.Dispose(ref intPtr);
			return result;
		}

		// Token: 0x060013FB RID: 5115 RVA: 0x0001D1FC File Offset: 0x0001B3FC
		public bool HasPresence(ref HasPresenceOptions options)
		{
			HasPresenceOptionsInternal hasPresenceOptionsInternal = default(HasPresenceOptionsInternal);
			hasPresenceOptionsInternal.Set(ref options);
			int from = Bindings.EOS_Presence_HasPresence(base.InnerHandle, ref hasPresenceOptionsInternal);
			Helper.Dispose<HasPresenceOptionsInternal>(ref hasPresenceOptionsInternal);
			bool result;
			Helper.Get(from, out result);
			return result;
		}

		// Token: 0x060013FC RID: 5116 RVA: 0x0001D240 File Offset: 0x0001B440
		public void QueryPresence(ref QueryPresenceOptions options, object clientData, OnQueryPresenceCompleteCallback completionDelegate)
		{
			QueryPresenceOptionsInternal queryPresenceOptionsInternal = default(QueryPresenceOptionsInternal);
			queryPresenceOptionsInternal.Set(ref options);
			IntPtr zero = IntPtr.Zero;
			OnQueryPresenceCompleteCallbackInternal onQueryPresenceCompleteCallbackInternal = new OnQueryPresenceCompleteCallbackInternal(PresenceInterface.OnQueryPresenceCompleteCallbackInternalImplementation);
			Helper.AddCallback(out zero, clientData, completionDelegate, onQueryPresenceCompleteCallbackInternal, Array.Empty<Delegate>());
			Bindings.EOS_Presence_QueryPresence(base.InnerHandle, ref queryPresenceOptionsInternal, zero, onQueryPresenceCompleteCallbackInternal);
			Helper.Dispose<QueryPresenceOptionsInternal>(ref queryPresenceOptionsInternal);
		}

		// Token: 0x060013FD RID: 5117 RVA: 0x0001D29A File Offset: 0x0001B49A
		public void RemoveNotifyJoinGameAccepted(ulong inId)
		{
			Bindings.EOS_Presence_RemoveNotifyJoinGameAccepted(base.InnerHandle, inId);
			Helper.RemoveCallbackByNotificationId(inId);
		}

		// Token: 0x060013FE RID: 5118 RVA: 0x0001D2B1 File Offset: 0x0001B4B1
		public void RemoveNotifyOnPresenceChanged(ulong notificationId)
		{
			Bindings.EOS_Presence_RemoveNotifyOnPresenceChanged(base.InnerHandle, notificationId);
			Helper.RemoveCallbackByNotificationId(notificationId);
		}

		// Token: 0x060013FF RID: 5119 RVA: 0x0001D2C8 File Offset: 0x0001B4C8
		public void SetPresence(ref SetPresenceOptions options, object clientData, SetPresenceCompleteCallback completionDelegate)
		{
			SetPresenceOptionsInternal setPresenceOptionsInternal = default(SetPresenceOptionsInternal);
			setPresenceOptionsInternal.Set(ref options);
			IntPtr zero = IntPtr.Zero;
			SetPresenceCompleteCallbackInternal setPresenceCompleteCallbackInternal = new SetPresenceCompleteCallbackInternal(PresenceInterface.SetPresenceCompleteCallbackInternalImplementation);
			Helper.AddCallback(out zero, clientData, completionDelegate, setPresenceCompleteCallbackInternal, Array.Empty<Delegate>());
			Bindings.EOS_Presence_SetPresence(base.InnerHandle, ref setPresenceOptionsInternal, zero, setPresenceCompleteCallbackInternal);
			Helper.Dispose<SetPresenceOptionsInternal>(ref setPresenceOptionsInternal);
		}

		// Token: 0x06001400 RID: 5120 RVA: 0x0001D324 File Offset: 0x0001B524
		[MonoPInvokeCallback(typeof(OnJoinGameAcceptedCallbackInternal))]
		internal static void OnJoinGameAcceptedCallbackInternalImplementation(ref JoinGameAcceptedCallbackInfoInternal data)
		{
			OnJoinGameAcceptedCallback onJoinGameAcceptedCallback;
			JoinGameAcceptedCallbackInfo joinGameAcceptedCallbackInfo;
			bool flag = Helper.TryGetCallback<JoinGameAcceptedCallbackInfoInternal, OnJoinGameAcceptedCallback, JoinGameAcceptedCallbackInfo>(ref data, out onJoinGameAcceptedCallback, out joinGameAcceptedCallbackInfo);
			if (flag)
			{
				onJoinGameAcceptedCallback(ref joinGameAcceptedCallbackInfo);
			}
		}

		// Token: 0x06001401 RID: 5121 RVA: 0x0001D34C File Offset: 0x0001B54C
		[MonoPInvokeCallback(typeof(OnPresenceChangedCallbackInternal))]
		internal static void OnPresenceChangedCallbackInternalImplementation(ref PresenceChangedCallbackInfoInternal data)
		{
			OnPresenceChangedCallback onPresenceChangedCallback;
			PresenceChangedCallbackInfo presenceChangedCallbackInfo;
			bool flag = Helper.TryGetCallback<PresenceChangedCallbackInfoInternal, OnPresenceChangedCallback, PresenceChangedCallbackInfo>(ref data, out onPresenceChangedCallback, out presenceChangedCallbackInfo);
			if (flag)
			{
				onPresenceChangedCallback(ref presenceChangedCallbackInfo);
			}
		}

		// Token: 0x06001402 RID: 5122 RVA: 0x0001D374 File Offset: 0x0001B574
		[MonoPInvokeCallback(typeof(OnQueryPresenceCompleteCallbackInternal))]
		internal static void OnQueryPresenceCompleteCallbackInternalImplementation(ref QueryPresenceCallbackInfoInternal data)
		{
			OnQueryPresenceCompleteCallback onQueryPresenceCompleteCallback;
			QueryPresenceCallbackInfo queryPresenceCallbackInfo;
			bool flag = Helper.TryGetAndRemoveCallback<QueryPresenceCallbackInfoInternal, OnQueryPresenceCompleteCallback, QueryPresenceCallbackInfo>(ref data, out onQueryPresenceCompleteCallback, out queryPresenceCallbackInfo);
			if (flag)
			{
				onQueryPresenceCompleteCallback(ref queryPresenceCallbackInfo);
			}
		}

		// Token: 0x06001403 RID: 5123 RVA: 0x0001D39C File Offset: 0x0001B59C
		[MonoPInvokeCallback(typeof(SetPresenceCompleteCallbackInternal))]
		internal static void SetPresenceCompleteCallbackInternalImplementation(ref SetPresenceCallbackInfoInternal data)
		{
			SetPresenceCompleteCallback setPresenceCompleteCallback;
			SetPresenceCallbackInfo setPresenceCallbackInfo;
			bool flag = Helper.TryGetAndRemoveCallback<SetPresenceCallbackInfoInternal, SetPresenceCompleteCallback, SetPresenceCallbackInfo>(ref data, out setPresenceCompleteCallback, out setPresenceCallbackInfo);
			if (flag)
			{
				setPresenceCompleteCallback(ref setPresenceCallbackInfo);
			}
		}

		// Token: 0x040008B4 RID: 2228
		public const int AddnotifyjoingameacceptedApiLatest = 2;

		// Token: 0x040008B5 RID: 2229
		public const int AddnotifyonpresencechangedApiLatest = 1;

		// Token: 0x040008B6 RID: 2230
		public const int CopypresenceApiLatest = 3;

		// Token: 0x040008B7 RID: 2231
		public const int CreatepresencemodificationApiLatest = 1;

		// Token: 0x040008B8 RID: 2232
		public const int DataMaxKeyLength = 64;

		// Token: 0x040008B9 RID: 2233
		public const int DataMaxKeys = 32;

		// Token: 0x040008BA RID: 2234
		public const int DataMaxValueLength = 255;

		// Token: 0x040008BB RID: 2235
		public const int DatarecordApiLatest = 1;

		// Token: 0x040008BC RID: 2236
		public const int DeletedataApiLatest = 1;

		// Token: 0x040008BD RID: 2237
		public const int GetjoininfoApiLatest = 1;

		// Token: 0x040008BE RID: 2238
		public const int HaspresenceApiLatest = 1;

		// Token: 0x040008BF RID: 2239
		public const int InfoApiLatest = 3;

		// Token: 0x040008C0 RID: 2240
		public static readonly Utf8String KeyPlatformPresence = "EOS_PlatformPresence";

		// Token: 0x040008C1 RID: 2241
		public const int QuerypresenceApiLatest = 1;

		// Token: 0x040008C2 RID: 2242
		public const int RichTextMaxValueLength = 255;

		// Token: 0x040008C3 RID: 2243
		public const int SetdataApiLatest = 1;

		// Token: 0x040008C4 RID: 2244
		public const int SetpresenceApiLatest = 1;

		// Token: 0x040008C5 RID: 2245
		public const int SetrawrichtextApiLatest = 1;

		// Token: 0x040008C6 RID: 2246
		public const int SetstatusApiLatest = 1;
	}
}
