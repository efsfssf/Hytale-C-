using System;

namespace Epic.OnlineServices.CustomInvites
{
	// Token: 0x0200058A RID: 1418
	public sealed class CustomInvitesInterface : Handle
	{
		// Token: 0x060024BA RID: 9402 RVA: 0x00036178 File Offset: 0x00034378
		public CustomInvitesInterface()
		{
		}

		// Token: 0x060024BB RID: 9403 RVA: 0x00036182 File Offset: 0x00034382
		public CustomInvitesInterface(IntPtr innerHandle) : base(innerHandle)
		{
		}

		// Token: 0x060024BC RID: 9404 RVA: 0x00036190 File Offset: 0x00034390
		public void AcceptRequestToJoin(ref AcceptRequestToJoinOptions options, object clientData, OnAcceptRequestToJoinCallback completionDelegate)
		{
			AcceptRequestToJoinOptionsInternal acceptRequestToJoinOptionsInternal = default(AcceptRequestToJoinOptionsInternal);
			acceptRequestToJoinOptionsInternal.Set(ref options);
			IntPtr zero = IntPtr.Zero;
			OnAcceptRequestToJoinCallbackInternal onAcceptRequestToJoinCallbackInternal = new OnAcceptRequestToJoinCallbackInternal(CustomInvitesInterface.OnAcceptRequestToJoinCallbackInternalImplementation);
			Helper.AddCallback(out zero, clientData, completionDelegate, onAcceptRequestToJoinCallbackInternal, Array.Empty<Delegate>());
			Bindings.EOS_CustomInvites_AcceptRequestToJoin(base.InnerHandle, ref acceptRequestToJoinOptionsInternal, zero, onAcceptRequestToJoinCallbackInternal);
			Helper.Dispose<AcceptRequestToJoinOptionsInternal>(ref acceptRequestToJoinOptionsInternal);
		}

		// Token: 0x060024BD RID: 9405 RVA: 0x000361EC File Offset: 0x000343EC
		public ulong AddNotifyCustomInviteAccepted(ref AddNotifyCustomInviteAcceptedOptions options, object clientData, OnCustomInviteAcceptedCallback notificationFn)
		{
			AddNotifyCustomInviteAcceptedOptionsInternal addNotifyCustomInviteAcceptedOptionsInternal = default(AddNotifyCustomInviteAcceptedOptionsInternal);
			addNotifyCustomInviteAcceptedOptionsInternal.Set(ref options);
			IntPtr zero = IntPtr.Zero;
			OnCustomInviteAcceptedCallbackInternal onCustomInviteAcceptedCallbackInternal = new OnCustomInviteAcceptedCallbackInternal(CustomInvitesInterface.OnCustomInviteAcceptedCallbackInternalImplementation);
			Helper.AddCallback(out zero, clientData, notificationFn, onCustomInviteAcceptedCallbackInternal, Array.Empty<Delegate>());
			ulong num = Bindings.EOS_CustomInvites_AddNotifyCustomInviteAccepted(base.InnerHandle, ref addNotifyCustomInviteAcceptedOptionsInternal, zero, onCustomInviteAcceptedCallbackInternal);
			Helper.Dispose<AddNotifyCustomInviteAcceptedOptionsInternal>(ref addNotifyCustomInviteAcceptedOptionsInternal);
			Helper.AssignNotificationIdToCallback(zero, num);
			return num;
		}

		// Token: 0x060024BE RID: 9406 RVA: 0x00036258 File Offset: 0x00034458
		public ulong AddNotifyCustomInviteReceived(ref AddNotifyCustomInviteReceivedOptions options, object clientData, OnCustomInviteReceivedCallback notificationFn)
		{
			AddNotifyCustomInviteReceivedOptionsInternal addNotifyCustomInviteReceivedOptionsInternal = default(AddNotifyCustomInviteReceivedOptionsInternal);
			addNotifyCustomInviteReceivedOptionsInternal.Set(ref options);
			IntPtr zero = IntPtr.Zero;
			OnCustomInviteReceivedCallbackInternal onCustomInviteReceivedCallbackInternal = new OnCustomInviteReceivedCallbackInternal(CustomInvitesInterface.OnCustomInviteReceivedCallbackInternalImplementation);
			Helper.AddCallback(out zero, clientData, notificationFn, onCustomInviteReceivedCallbackInternal, Array.Empty<Delegate>());
			ulong num = Bindings.EOS_CustomInvites_AddNotifyCustomInviteReceived(base.InnerHandle, ref addNotifyCustomInviteReceivedOptionsInternal, zero, onCustomInviteReceivedCallbackInternal);
			Helper.Dispose<AddNotifyCustomInviteReceivedOptionsInternal>(ref addNotifyCustomInviteReceivedOptionsInternal);
			Helper.AssignNotificationIdToCallback(zero, num);
			return num;
		}

		// Token: 0x060024BF RID: 9407 RVA: 0x000362C4 File Offset: 0x000344C4
		public ulong AddNotifyCustomInviteRejected(ref AddNotifyCustomInviteRejectedOptions options, object clientData, OnCustomInviteRejectedCallback notificationFn)
		{
			AddNotifyCustomInviteRejectedOptionsInternal addNotifyCustomInviteRejectedOptionsInternal = default(AddNotifyCustomInviteRejectedOptionsInternal);
			addNotifyCustomInviteRejectedOptionsInternal.Set(ref options);
			IntPtr zero = IntPtr.Zero;
			OnCustomInviteRejectedCallbackInternal onCustomInviteRejectedCallbackInternal = new OnCustomInviteRejectedCallbackInternal(CustomInvitesInterface.OnCustomInviteRejectedCallbackInternalImplementation);
			Helper.AddCallback(out zero, clientData, notificationFn, onCustomInviteRejectedCallbackInternal, Array.Empty<Delegate>());
			ulong num = Bindings.EOS_CustomInvites_AddNotifyCustomInviteRejected(base.InnerHandle, ref addNotifyCustomInviteRejectedOptionsInternal, zero, onCustomInviteRejectedCallbackInternal);
			Helper.Dispose<AddNotifyCustomInviteRejectedOptionsInternal>(ref addNotifyCustomInviteRejectedOptionsInternal);
			Helper.AssignNotificationIdToCallback(zero, num);
			return num;
		}

		// Token: 0x060024C0 RID: 9408 RVA: 0x00036330 File Offset: 0x00034530
		public ulong AddNotifyRequestToJoinAccepted(ref AddNotifyRequestToJoinAcceptedOptions options, object clientData, OnRequestToJoinAcceptedCallback notificationFn)
		{
			AddNotifyRequestToJoinAcceptedOptionsInternal addNotifyRequestToJoinAcceptedOptionsInternal = default(AddNotifyRequestToJoinAcceptedOptionsInternal);
			addNotifyRequestToJoinAcceptedOptionsInternal.Set(ref options);
			IntPtr zero = IntPtr.Zero;
			OnRequestToJoinAcceptedCallbackInternal onRequestToJoinAcceptedCallbackInternal = new OnRequestToJoinAcceptedCallbackInternal(CustomInvitesInterface.OnRequestToJoinAcceptedCallbackInternalImplementation);
			Helper.AddCallback(out zero, clientData, notificationFn, onRequestToJoinAcceptedCallbackInternal, Array.Empty<Delegate>());
			ulong num = Bindings.EOS_CustomInvites_AddNotifyRequestToJoinAccepted(base.InnerHandle, ref addNotifyRequestToJoinAcceptedOptionsInternal, zero, onRequestToJoinAcceptedCallbackInternal);
			Helper.Dispose<AddNotifyRequestToJoinAcceptedOptionsInternal>(ref addNotifyRequestToJoinAcceptedOptionsInternal);
			Helper.AssignNotificationIdToCallback(zero, num);
			return num;
		}

		// Token: 0x060024C1 RID: 9409 RVA: 0x0003639C File Offset: 0x0003459C
		public ulong AddNotifyRequestToJoinReceived(ref AddNotifyRequestToJoinReceivedOptions options, object clientData, OnRequestToJoinReceivedCallback notificationFn)
		{
			AddNotifyRequestToJoinReceivedOptionsInternal addNotifyRequestToJoinReceivedOptionsInternal = default(AddNotifyRequestToJoinReceivedOptionsInternal);
			addNotifyRequestToJoinReceivedOptionsInternal.Set(ref options);
			IntPtr zero = IntPtr.Zero;
			OnRequestToJoinReceivedCallbackInternal onRequestToJoinReceivedCallbackInternal = new OnRequestToJoinReceivedCallbackInternal(CustomInvitesInterface.OnRequestToJoinReceivedCallbackInternalImplementation);
			Helper.AddCallback(out zero, clientData, notificationFn, onRequestToJoinReceivedCallbackInternal, Array.Empty<Delegate>());
			ulong num = Bindings.EOS_CustomInvites_AddNotifyRequestToJoinReceived(base.InnerHandle, ref addNotifyRequestToJoinReceivedOptionsInternal, zero, onRequestToJoinReceivedCallbackInternal);
			Helper.Dispose<AddNotifyRequestToJoinReceivedOptionsInternal>(ref addNotifyRequestToJoinReceivedOptionsInternal);
			Helper.AssignNotificationIdToCallback(zero, num);
			return num;
		}

		// Token: 0x060024C2 RID: 9410 RVA: 0x00036408 File Offset: 0x00034608
		public ulong AddNotifyRequestToJoinRejected(ref AddNotifyRequestToJoinRejectedOptions options, object clientData, OnRequestToJoinRejectedCallback notificationFn)
		{
			AddNotifyRequestToJoinRejectedOptionsInternal addNotifyRequestToJoinRejectedOptionsInternal = default(AddNotifyRequestToJoinRejectedOptionsInternal);
			addNotifyRequestToJoinRejectedOptionsInternal.Set(ref options);
			IntPtr zero = IntPtr.Zero;
			OnRequestToJoinRejectedCallbackInternal onRequestToJoinRejectedCallbackInternal = new OnRequestToJoinRejectedCallbackInternal(CustomInvitesInterface.OnRequestToJoinRejectedCallbackInternalImplementation);
			Helper.AddCallback(out zero, clientData, notificationFn, onRequestToJoinRejectedCallbackInternal, Array.Empty<Delegate>());
			ulong num = Bindings.EOS_CustomInvites_AddNotifyRequestToJoinRejected(base.InnerHandle, ref addNotifyRequestToJoinRejectedOptionsInternal, zero, onRequestToJoinRejectedCallbackInternal);
			Helper.Dispose<AddNotifyRequestToJoinRejectedOptionsInternal>(ref addNotifyRequestToJoinRejectedOptionsInternal);
			Helper.AssignNotificationIdToCallback(zero, num);
			return num;
		}

		// Token: 0x060024C3 RID: 9411 RVA: 0x00036474 File Offset: 0x00034674
		public ulong AddNotifyRequestToJoinResponseReceived(ref AddNotifyRequestToJoinResponseReceivedOptions options, object clientData, OnRequestToJoinResponseReceivedCallback notificationFn)
		{
			AddNotifyRequestToJoinResponseReceivedOptionsInternal addNotifyRequestToJoinResponseReceivedOptionsInternal = default(AddNotifyRequestToJoinResponseReceivedOptionsInternal);
			addNotifyRequestToJoinResponseReceivedOptionsInternal.Set(ref options);
			IntPtr zero = IntPtr.Zero;
			OnRequestToJoinResponseReceivedCallbackInternal onRequestToJoinResponseReceivedCallbackInternal = new OnRequestToJoinResponseReceivedCallbackInternal(CustomInvitesInterface.OnRequestToJoinResponseReceivedCallbackInternalImplementation);
			Helper.AddCallback(out zero, clientData, notificationFn, onRequestToJoinResponseReceivedCallbackInternal, Array.Empty<Delegate>());
			ulong num = Bindings.EOS_CustomInvites_AddNotifyRequestToJoinResponseReceived(base.InnerHandle, ref addNotifyRequestToJoinResponseReceivedOptionsInternal, zero, onRequestToJoinResponseReceivedCallbackInternal);
			Helper.Dispose<AddNotifyRequestToJoinResponseReceivedOptionsInternal>(ref addNotifyRequestToJoinResponseReceivedOptionsInternal);
			Helper.AssignNotificationIdToCallback(zero, num);
			return num;
		}

		// Token: 0x060024C4 RID: 9412 RVA: 0x000364E0 File Offset: 0x000346E0
		public ulong AddNotifySendCustomNativeInviteRequested(ref AddNotifySendCustomNativeInviteRequestedOptions options, object clientData, OnSendCustomNativeInviteRequestedCallback notificationFn)
		{
			AddNotifySendCustomNativeInviteRequestedOptionsInternal addNotifySendCustomNativeInviteRequestedOptionsInternal = default(AddNotifySendCustomNativeInviteRequestedOptionsInternal);
			addNotifySendCustomNativeInviteRequestedOptionsInternal.Set(ref options);
			IntPtr zero = IntPtr.Zero;
			OnSendCustomNativeInviteRequestedCallbackInternal onSendCustomNativeInviteRequestedCallbackInternal = new OnSendCustomNativeInviteRequestedCallbackInternal(CustomInvitesInterface.OnSendCustomNativeInviteRequestedCallbackInternalImplementation);
			Helper.AddCallback(out zero, clientData, notificationFn, onSendCustomNativeInviteRequestedCallbackInternal, Array.Empty<Delegate>());
			ulong num = Bindings.EOS_CustomInvites_AddNotifySendCustomNativeInviteRequested(base.InnerHandle, ref addNotifySendCustomNativeInviteRequestedOptionsInternal, zero, onSendCustomNativeInviteRequestedCallbackInternal);
			Helper.Dispose<AddNotifySendCustomNativeInviteRequestedOptionsInternal>(ref addNotifySendCustomNativeInviteRequestedOptionsInternal);
			Helper.AssignNotificationIdToCallback(zero, num);
			return num;
		}

		// Token: 0x060024C5 RID: 9413 RVA: 0x0003654C File Offset: 0x0003474C
		public Result FinalizeInvite(ref FinalizeInviteOptions options)
		{
			FinalizeInviteOptionsInternal finalizeInviteOptionsInternal = default(FinalizeInviteOptionsInternal);
			finalizeInviteOptionsInternal.Set(ref options);
			Result result = Bindings.EOS_CustomInvites_FinalizeInvite(base.InnerHandle, ref finalizeInviteOptionsInternal);
			Helper.Dispose<FinalizeInviteOptionsInternal>(ref finalizeInviteOptionsInternal);
			return result;
		}

		// Token: 0x060024C6 RID: 9414 RVA: 0x00036588 File Offset: 0x00034788
		public void RejectRequestToJoin(ref RejectRequestToJoinOptions options, object clientData, OnRejectRequestToJoinCallback completionDelegate)
		{
			RejectRequestToJoinOptionsInternal rejectRequestToJoinOptionsInternal = default(RejectRequestToJoinOptionsInternal);
			rejectRequestToJoinOptionsInternal.Set(ref options);
			IntPtr zero = IntPtr.Zero;
			OnRejectRequestToJoinCallbackInternal onRejectRequestToJoinCallbackInternal = new OnRejectRequestToJoinCallbackInternal(CustomInvitesInterface.OnRejectRequestToJoinCallbackInternalImplementation);
			Helper.AddCallback(out zero, clientData, completionDelegate, onRejectRequestToJoinCallbackInternal, Array.Empty<Delegate>());
			Bindings.EOS_CustomInvites_RejectRequestToJoin(base.InnerHandle, ref rejectRequestToJoinOptionsInternal, zero, onRejectRequestToJoinCallbackInternal);
			Helper.Dispose<RejectRequestToJoinOptionsInternal>(ref rejectRequestToJoinOptionsInternal);
		}

		// Token: 0x060024C7 RID: 9415 RVA: 0x000365E2 File Offset: 0x000347E2
		public void RemoveNotifyCustomInviteAccepted(ulong inId)
		{
			Bindings.EOS_CustomInvites_RemoveNotifyCustomInviteAccepted(base.InnerHandle, inId);
			Helper.RemoveCallbackByNotificationId(inId);
		}

		// Token: 0x060024C8 RID: 9416 RVA: 0x000365F9 File Offset: 0x000347F9
		public void RemoveNotifyCustomInviteReceived(ulong inId)
		{
			Bindings.EOS_CustomInvites_RemoveNotifyCustomInviteReceived(base.InnerHandle, inId);
			Helper.RemoveCallbackByNotificationId(inId);
		}

		// Token: 0x060024C9 RID: 9417 RVA: 0x00036610 File Offset: 0x00034810
		public void RemoveNotifyCustomInviteRejected(ulong inId)
		{
			Bindings.EOS_CustomInvites_RemoveNotifyCustomInviteRejected(base.InnerHandle, inId);
			Helper.RemoveCallbackByNotificationId(inId);
		}

		// Token: 0x060024CA RID: 9418 RVA: 0x00036627 File Offset: 0x00034827
		public void RemoveNotifyRequestToJoinAccepted(ulong inId)
		{
			Bindings.EOS_CustomInvites_RemoveNotifyRequestToJoinAccepted(base.InnerHandle, inId);
			Helper.RemoveCallbackByNotificationId(inId);
		}

		// Token: 0x060024CB RID: 9419 RVA: 0x0003663E File Offset: 0x0003483E
		public void RemoveNotifyRequestToJoinReceived(ulong inId)
		{
			Bindings.EOS_CustomInvites_RemoveNotifyRequestToJoinReceived(base.InnerHandle, inId);
			Helper.RemoveCallbackByNotificationId(inId);
		}

		// Token: 0x060024CC RID: 9420 RVA: 0x00036655 File Offset: 0x00034855
		public void RemoveNotifyRequestToJoinRejected(ulong inId)
		{
			Bindings.EOS_CustomInvites_RemoveNotifyRequestToJoinRejected(base.InnerHandle, inId);
			Helper.RemoveCallbackByNotificationId(inId);
		}

		// Token: 0x060024CD RID: 9421 RVA: 0x0003666C File Offset: 0x0003486C
		public void RemoveNotifyRequestToJoinResponseReceived(ulong inId)
		{
			Bindings.EOS_CustomInvites_RemoveNotifyRequestToJoinResponseReceived(base.InnerHandle, inId);
			Helper.RemoveCallbackByNotificationId(inId);
		}

		// Token: 0x060024CE RID: 9422 RVA: 0x00036683 File Offset: 0x00034883
		public void RemoveNotifySendCustomNativeInviteRequested(ulong inId)
		{
			Bindings.EOS_CustomInvites_RemoveNotifySendCustomNativeInviteRequested(base.InnerHandle, inId);
			Helper.RemoveCallbackByNotificationId(inId);
		}

		// Token: 0x060024CF RID: 9423 RVA: 0x0003669C File Offset: 0x0003489C
		public void SendCustomInvite(ref SendCustomInviteOptions options, object clientData, OnSendCustomInviteCallback completionDelegate)
		{
			SendCustomInviteOptionsInternal sendCustomInviteOptionsInternal = default(SendCustomInviteOptionsInternal);
			sendCustomInviteOptionsInternal.Set(ref options);
			IntPtr zero = IntPtr.Zero;
			OnSendCustomInviteCallbackInternal onSendCustomInviteCallbackInternal = new OnSendCustomInviteCallbackInternal(CustomInvitesInterface.OnSendCustomInviteCallbackInternalImplementation);
			Helper.AddCallback(out zero, clientData, completionDelegate, onSendCustomInviteCallbackInternal, Array.Empty<Delegate>());
			Bindings.EOS_CustomInvites_SendCustomInvite(base.InnerHandle, ref sendCustomInviteOptionsInternal, zero, onSendCustomInviteCallbackInternal);
			Helper.Dispose<SendCustomInviteOptionsInternal>(ref sendCustomInviteOptionsInternal);
		}

		// Token: 0x060024D0 RID: 9424 RVA: 0x000366F8 File Offset: 0x000348F8
		public void SendRequestToJoin(ref SendRequestToJoinOptions options, object clientData, OnSendRequestToJoinCallback completionDelegate)
		{
			SendRequestToJoinOptionsInternal sendRequestToJoinOptionsInternal = default(SendRequestToJoinOptionsInternal);
			sendRequestToJoinOptionsInternal.Set(ref options);
			IntPtr zero = IntPtr.Zero;
			OnSendRequestToJoinCallbackInternal onSendRequestToJoinCallbackInternal = new OnSendRequestToJoinCallbackInternal(CustomInvitesInterface.OnSendRequestToJoinCallbackInternalImplementation);
			Helper.AddCallback(out zero, clientData, completionDelegate, onSendRequestToJoinCallbackInternal, Array.Empty<Delegate>());
			Bindings.EOS_CustomInvites_SendRequestToJoin(base.InnerHandle, ref sendRequestToJoinOptionsInternal, zero, onSendRequestToJoinCallbackInternal);
			Helper.Dispose<SendRequestToJoinOptionsInternal>(ref sendRequestToJoinOptionsInternal);
		}

		// Token: 0x060024D1 RID: 9425 RVA: 0x00036754 File Offset: 0x00034954
		public Result SetCustomInvite(ref SetCustomInviteOptions options)
		{
			SetCustomInviteOptionsInternal setCustomInviteOptionsInternal = default(SetCustomInviteOptionsInternal);
			setCustomInviteOptionsInternal.Set(ref options);
			Result result = Bindings.EOS_CustomInvites_SetCustomInvite(base.InnerHandle, ref setCustomInviteOptionsInternal);
			Helper.Dispose<SetCustomInviteOptionsInternal>(ref setCustomInviteOptionsInternal);
			return result;
		}

		// Token: 0x060024D2 RID: 9426 RVA: 0x00036790 File Offset: 0x00034990
		[MonoPInvokeCallback(typeof(OnAcceptRequestToJoinCallbackInternal))]
		internal static void OnAcceptRequestToJoinCallbackInternalImplementation(ref AcceptRequestToJoinCallbackInfoInternal data)
		{
			OnAcceptRequestToJoinCallback onAcceptRequestToJoinCallback;
			AcceptRequestToJoinCallbackInfo acceptRequestToJoinCallbackInfo;
			bool flag = Helper.TryGetAndRemoveCallback<AcceptRequestToJoinCallbackInfoInternal, OnAcceptRequestToJoinCallback, AcceptRequestToJoinCallbackInfo>(ref data, out onAcceptRequestToJoinCallback, out acceptRequestToJoinCallbackInfo);
			if (flag)
			{
				onAcceptRequestToJoinCallback(ref acceptRequestToJoinCallbackInfo);
			}
		}

		// Token: 0x060024D3 RID: 9427 RVA: 0x000367B8 File Offset: 0x000349B8
		[MonoPInvokeCallback(typeof(OnCustomInviteAcceptedCallbackInternal))]
		internal static void OnCustomInviteAcceptedCallbackInternalImplementation(ref OnCustomInviteAcceptedCallbackInfoInternal data)
		{
			OnCustomInviteAcceptedCallback onCustomInviteAcceptedCallback;
			OnCustomInviteAcceptedCallbackInfo onCustomInviteAcceptedCallbackInfo;
			bool flag = Helper.TryGetCallback<OnCustomInviteAcceptedCallbackInfoInternal, OnCustomInviteAcceptedCallback, OnCustomInviteAcceptedCallbackInfo>(ref data, out onCustomInviteAcceptedCallback, out onCustomInviteAcceptedCallbackInfo);
			if (flag)
			{
				onCustomInviteAcceptedCallback(ref onCustomInviteAcceptedCallbackInfo);
			}
		}

		// Token: 0x060024D4 RID: 9428 RVA: 0x000367E0 File Offset: 0x000349E0
		[MonoPInvokeCallback(typeof(OnCustomInviteReceivedCallbackInternal))]
		internal static void OnCustomInviteReceivedCallbackInternalImplementation(ref OnCustomInviteReceivedCallbackInfoInternal data)
		{
			OnCustomInviteReceivedCallback onCustomInviteReceivedCallback;
			OnCustomInviteReceivedCallbackInfo onCustomInviteReceivedCallbackInfo;
			bool flag = Helper.TryGetCallback<OnCustomInviteReceivedCallbackInfoInternal, OnCustomInviteReceivedCallback, OnCustomInviteReceivedCallbackInfo>(ref data, out onCustomInviteReceivedCallback, out onCustomInviteReceivedCallbackInfo);
			if (flag)
			{
				onCustomInviteReceivedCallback(ref onCustomInviteReceivedCallbackInfo);
			}
		}

		// Token: 0x060024D5 RID: 9429 RVA: 0x00036808 File Offset: 0x00034A08
		[MonoPInvokeCallback(typeof(OnCustomInviteRejectedCallbackInternal))]
		internal static void OnCustomInviteRejectedCallbackInternalImplementation(ref CustomInviteRejectedCallbackInfoInternal data)
		{
			OnCustomInviteRejectedCallback onCustomInviteRejectedCallback;
			CustomInviteRejectedCallbackInfo customInviteRejectedCallbackInfo;
			bool flag = Helper.TryGetCallback<CustomInviteRejectedCallbackInfoInternal, OnCustomInviteRejectedCallback, CustomInviteRejectedCallbackInfo>(ref data, out onCustomInviteRejectedCallback, out customInviteRejectedCallbackInfo);
			if (flag)
			{
				onCustomInviteRejectedCallback(ref customInviteRejectedCallbackInfo);
			}
		}

		// Token: 0x060024D6 RID: 9430 RVA: 0x00036830 File Offset: 0x00034A30
		[MonoPInvokeCallback(typeof(OnRejectRequestToJoinCallbackInternal))]
		internal static void OnRejectRequestToJoinCallbackInternalImplementation(ref RejectRequestToJoinCallbackInfoInternal data)
		{
			OnRejectRequestToJoinCallback onRejectRequestToJoinCallback;
			RejectRequestToJoinCallbackInfo rejectRequestToJoinCallbackInfo;
			bool flag = Helper.TryGetAndRemoveCallback<RejectRequestToJoinCallbackInfoInternal, OnRejectRequestToJoinCallback, RejectRequestToJoinCallbackInfo>(ref data, out onRejectRequestToJoinCallback, out rejectRequestToJoinCallbackInfo);
			if (flag)
			{
				onRejectRequestToJoinCallback(ref rejectRequestToJoinCallbackInfo);
			}
		}

		// Token: 0x060024D7 RID: 9431 RVA: 0x00036858 File Offset: 0x00034A58
		[MonoPInvokeCallback(typeof(OnRequestToJoinAcceptedCallbackInternal))]
		internal static void OnRequestToJoinAcceptedCallbackInternalImplementation(ref OnRequestToJoinAcceptedCallbackInfoInternal data)
		{
			OnRequestToJoinAcceptedCallback onRequestToJoinAcceptedCallback;
			OnRequestToJoinAcceptedCallbackInfo onRequestToJoinAcceptedCallbackInfo;
			bool flag = Helper.TryGetCallback<OnRequestToJoinAcceptedCallbackInfoInternal, OnRequestToJoinAcceptedCallback, OnRequestToJoinAcceptedCallbackInfo>(ref data, out onRequestToJoinAcceptedCallback, out onRequestToJoinAcceptedCallbackInfo);
			if (flag)
			{
				onRequestToJoinAcceptedCallback(ref onRequestToJoinAcceptedCallbackInfo);
			}
		}

		// Token: 0x060024D8 RID: 9432 RVA: 0x00036880 File Offset: 0x00034A80
		[MonoPInvokeCallback(typeof(OnRequestToJoinReceivedCallbackInternal))]
		internal static void OnRequestToJoinReceivedCallbackInternalImplementation(ref RequestToJoinReceivedCallbackInfoInternal data)
		{
			OnRequestToJoinReceivedCallback onRequestToJoinReceivedCallback;
			RequestToJoinReceivedCallbackInfo requestToJoinReceivedCallbackInfo;
			bool flag = Helper.TryGetCallback<RequestToJoinReceivedCallbackInfoInternal, OnRequestToJoinReceivedCallback, RequestToJoinReceivedCallbackInfo>(ref data, out onRequestToJoinReceivedCallback, out requestToJoinReceivedCallbackInfo);
			if (flag)
			{
				onRequestToJoinReceivedCallback(ref requestToJoinReceivedCallbackInfo);
			}
		}

		// Token: 0x060024D9 RID: 9433 RVA: 0x000368A8 File Offset: 0x00034AA8
		[MonoPInvokeCallback(typeof(OnRequestToJoinRejectedCallbackInternal))]
		internal static void OnRequestToJoinRejectedCallbackInternalImplementation(ref OnRequestToJoinRejectedCallbackInfoInternal data)
		{
			OnRequestToJoinRejectedCallback onRequestToJoinRejectedCallback;
			OnRequestToJoinRejectedCallbackInfo onRequestToJoinRejectedCallbackInfo;
			bool flag = Helper.TryGetCallback<OnRequestToJoinRejectedCallbackInfoInternal, OnRequestToJoinRejectedCallback, OnRequestToJoinRejectedCallbackInfo>(ref data, out onRequestToJoinRejectedCallback, out onRequestToJoinRejectedCallbackInfo);
			if (flag)
			{
				onRequestToJoinRejectedCallback(ref onRequestToJoinRejectedCallbackInfo);
			}
		}

		// Token: 0x060024DA RID: 9434 RVA: 0x000368D0 File Offset: 0x00034AD0
		[MonoPInvokeCallback(typeof(OnRequestToJoinResponseReceivedCallbackInternal))]
		internal static void OnRequestToJoinResponseReceivedCallbackInternalImplementation(ref RequestToJoinResponseReceivedCallbackInfoInternal data)
		{
			OnRequestToJoinResponseReceivedCallback onRequestToJoinResponseReceivedCallback;
			RequestToJoinResponseReceivedCallbackInfo requestToJoinResponseReceivedCallbackInfo;
			bool flag = Helper.TryGetCallback<RequestToJoinResponseReceivedCallbackInfoInternal, OnRequestToJoinResponseReceivedCallback, RequestToJoinResponseReceivedCallbackInfo>(ref data, out onRequestToJoinResponseReceivedCallback, out requestToJoinResponseReceivedCallbackInfo);
			if (flag)
			{
				onRequestToJoinResponseReceivedCallback(ref requestToJoinResponseReceivedCallbackInfo);
			}
		}

		// Token: 0x060024DB RID: 9435 RVA: 0x000368F8 File Offset: 0x00034AF8
		[MonoPInvokeCallback(typeof(OnSendCustomInviteCallbackInternal))]
		internal static void OnSendCustomInviteCallbackInternalImplementation(ref SendCustomInviteCallbackInfoInternal data)
		{
			OnSendCustomInviteCallback onSendCustomInviteCallback;
			SendCustomInviteCallbackInfo sendCustomInviteCallbackInfo;
			bool flag = Helper.TryGetAndRemoveCallback<SendCustomInviteCallbackInfoInternal, OnSendCustomInviteCallback, SendCustomInviteCallbackInfo>(ref data, out onSendCustomInviteCallback, out sendCustomInviteCallbackInfo);
			if (flag)
			{
				onSendCustomInviteCallback(ref sendCustomInviteCallbackInfo);
			}
		}

		// Token: 0x060024DC RID: 9436 RVA: 0x00036920 File Offset: 0x00034B20
		[MonoPInvokeCallback(typeof(OnSendCustomNativeInviteRequestedCallbackInternal))]
		internal static void OnSendCustomNativeInviteRequestedCallbackInternalImplementation(ref SendCustomNativeInviteRequestedCallbackInfoInternal data)
		{
			OnSendCustomNativeInviteRequestedCallback onSendCustomNativeInviteRequestedCallback;
			SendCustomNativeInviteRequestedCallbackInfo sendCustomNativeInviteRequestedCallbackInfo;
			bool flag = Helper.TryGetCallback<SendCustomNativeInviteRequestedCallbackInfoInternal, OnSendCustomNativeInviteRequestedCallback, SendCustomNativeInviteRequestedCallbackInfo>(ref data, out onSendCustomNativeInviteRequestedCallback, out sendCustomNativeInviteRequestedCallbackInfo);
			if (flag)
			{
				onSendCustomNativeInviteRequestedCallback(ref sendCustomNativeInviteRequestedCallbackInfo);
			}
		}

		// Token: 0x060024DD RID: 9437 RVA: 0x00036948 File Offset: 0x00034B48
		[MonoPInvokeCallback(typeof(OnSendRequestToJoinCallbackInternal))]
		internal static void OnSendRequestToJoinCallbackInternalImplementation(ref SendRequestToJoinCallbackInfoInternal data)
		{
			OnSendRequestToJoinCallback onSendRequestToJoinCallback;
			SendRequestToJoinCallbackInfo sendRequestToJoinCallbackInfo;
			bool flag = Helper.TryGetAndRemoveCallback<SendRequestToJoinCallbackInfoInternal, OnSendRequestToJoinCallback, SendRequestToJoinCallbackInfo>(ref data, out onSendRequestToJoinCallback, out sendRequestToJoinCallbackInfo);
			if (flag)
			{
				onSendRequestToJoinCallback(ref sendRequestToJoinCallbackInfo);
			}
		}

		// Token: 0x04001016 RID: 4118
		public const int AcceptrequesttojoinApiLatest = 1;

		// Token: 0x04001017 RID: 4119
		public const int AddnotifycustominviteacceptedApiLatest = 1;

		// Token: 0x04001018 RID: 4120
		public const int AddnotifycustominvitereceivedApiLatest = 1;

		// Token: 0x04001019 RID: 4121
		public const int AddnotifycustominviterejectedApiLatest = 1;

		// Token: 0x0400101A RID: 4122
		public const int AddnotifyrequesttojoinacceptedApiLatest = 1;

		// Token: 0x0400101B RID: 4123
		public const int AddnotifyrequesttojoinreceivedApiLatest = 1;

		// Token: 0x0400101C RID: 4124
		public const int AddnotifyrequesttojoinrejectedApiLatest = 1;

		// Token: 0x0400101D RID: 4125
		public const int AddnotifyrequesttojoinresponsereceivedApiLatest = 1;

		// Token: 0x0400101E RID: 4126
		public const int AddnotifysendcustomnativeinviterequestedApiLatest = 1;

		// Token: 0x0400101F RID: 4127
		public const int FinalizeinviteApiLatest = 1;

		// Token: 0x04001020 RID: 4128
		public const int MaxPayloadLength = 500;

		// Token: 0x04001021 RID: 4129
		public const int RejectrequesttojoinApiLatest = 1;

		// Token: 0x04001022 RID: 4130
		public const int SendcustominviteApiLatest = 1;

		// Token: 0x04001023 RID: 4131
		public const int SendrequesttojoinApiLatest = 1;

		// Token: 0x04001024 RID: 4132
		public const int SetcustominviteApiLatest = 1;
	}
}
