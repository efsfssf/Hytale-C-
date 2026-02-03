using System;
using Epic.OnlineServices.AntiCheatCommon;

namespace Epic.OnlineServices.AntiCheatClient
{
	// Token: 0x020006DB RID: 1755
	public sealed class AntiCheatClientInterface : Handle
	{
		// Token: 0x06002D5D RID: 11613 RVA: 0x00042F88 File Offset: 0x00041188
		public AntiCheatClientInterface()
		{
		}

		// Token: 0x06002D5E RID: 11614 RVA: 0x00042F9E File Offset: 0x0004119E
		public AntiCheatClientInterface(IntPtr innerHandle) : base(innerHandle)
		{
		}

		// Token: 0x06002D5F RID: 11615 RVA: 0x00042FB8 File Offset: 0x000411B8
		public Result AddExternalIntegrityCatalog(ref AddExternalIntegrityCatalogOptions options)
		{
			AddExternalIntegrityCatalogOptionsInternal addExternalIntegrityCatalogOptionsInternal = default(AddExternalIntegrityCatalogOptionsInternal);
			addExternalIntegrityCatalogOptionsInternal.Set(ref options);
			Result result = Bindings.EOS_AntiCheatClient_AddExternalIntegrityCatalog(base.InnerHandle, ref addExternalIntegrityCatalogOptionsInternal);
			Helper.Dispose<AddExternalIntegrityCatalogOptionsInternal>(ref addExternalIntegrityCatalogOptionsInternal);
			return result;
		}

		// Token: 0x06002D60 RID: 11616 RVA: 0x00042FF4 File Offset: 0x000411F4
		public ulong AddNotifyClientIntegrityViolated(ref AddNotifyClientIntegrityViolatedOptions options, object clientData, OnClientIntegrityViolatedCallback notificationFn)
		{
			AddNotifyClientIntegrityViolatedOptionsInternal addNotifyClientIntegrityViolatedOptionsInternal = default(AddNotifyClientIntegrityViolatedOptionsInternal);
			addNotifyClientIntegrityViolatedOptionsInternal.Set(ref options);
			IntPtr zero = IntPtr.Zero;
			OnClientIntegrityViolatedCallbackInternal onClientIntegrityViolatedCallbackInternal = new OnClientIntegrityViolatedCallbackInternal(AntiCheatClientInterface.OnClientIntegrityViolatedCallbackInternalImplementation);
			Helper.AddCallback(out zero, clientData, notificationFn, onClientIntegrityViolatedCallbackInternal, Array.Empty<Delegate>());
			ulong num = Bindings.EOS_AntiCheatClient_AddNotifyClientIntegrityViolated(base.InnerHandle, ref addNotifyClientIntegrityViolatedOptionsInternal, zero, onClientIntegrityViolatedCallbackInternal);
			Helper.Dispose<AddNotifyClientIntegrityViolatedOptionsInternal>(ref addNotifyClientIntegrityViolatedOptionsInternal);
			Helper.AssignNotificationIdToCallback(zero, num);
			return num;
		}

		// Token: 0x06002D61 RID: 11617 RVA: 0x00043060 File Offset: 0x00041260
		public ulong AddNotifyMessageToPeer(ref AddNotifyMessageToPeerOptions options, object clientData, OnMessageToPeerCallback notificationFn)
		{
			AddNotifyMessageToPeerOptionsInternal addNotifyMessageToPeerOptionsInternal = default(AddNotifyMessageToPeerOptionsInternal);
			addNotifyMessageToPeerOptionsInternal.Set(ref options);
			IntPtr zero = IntPtr.Zero;
			OnMessageToPeerCallbackInternal onMessageToPeerCallbackInternal = new OnMessageToPeerCallbackInternal(AntiCheatClientInterface.OnMessageToPeerCallbackInternalImplementation);
			Helper.AddCallback(out zero, clientData, notificationFn, onMessageToPeerCallbackInternal, Array.Empty<Delegate>());
			ulong num = Bindings.EOS_AntiCheatClient_AddNotifyMessageToPeer(base.InnerHandle, ref addNotifyMessageToPeerOptionsInternal, zero, onMessageToPeerCallbackInternal);
			Helper.Dispose<AddNotifyMessageToPeerOptionsInternal>(ref addNotifyMessageToPeerOptionsInternal);
			Helper.AssignNotificationIdToCallback(zero, num);
			return num;
		}

		// Token: 0x06002D62 RID: 11618 RVA: 0x000430CC File Offset: 0x000412CC
		public ulong AddNotifyMessageToServer(ref AddNotifyMessageToServerOptions options, object clientData, OnMessageToServerCallback notificationFn)
		{
			AddNotifyMessageToServerOptionsInternal addNotifyMessageToServerOptionsInternal = default(AddNotifyMessageToServerOptionsInternal);
			addNotifyMessageToServerOptionsInternal.Set(ref options);
			IntPtr zero = IntPtr.Zero;
			OnMessageToServerCallbackInternal onMessageToServerCallbackInternal = new OnMessageToServerCallbackInternal(AntiCheatClientInterface.OnMessageToServerCallbackInternalImplementation);
			Helper.AddCallback(out zero, clientData, notificationFn, onMessageToServerCallbackInternal, Array.Empty<Delegate>());
			ulong num = Bindings.EOS_AntiCheatClient_AddNotifyMessageToServer(base.InnerHandle, ref addNotifyMessageToServerOptionsInternal, zero, onMessageToServerCallbackInternal);
			Helper.Dispose<AddNotifyMessageToServerOptionsInternal>(ref addNotifyMessageToServerOptionsInternal);
			Helper.AssignNotificationIdToCallback(zero, num);
			return num;
		}

		// Token: 0x06002D63 RID: 11619 RVA: 0x00043138 File Offset: 0x00041338
		public ulong AddNotifyPeerActionRequired(ref AddNotifyPeerActionRequiredOptions options, object clientData, OnPeerActionRequiredCallback notificationFn)
		{
			AddNotifyPeerActionRequiredOptionsInternal addNotifyPeerActionRequiredOptionsInternal = default(AddNotifyPeerActionRequiredOptionsInternal);
			addNotifyPeerActionRequiredOptionsInternal.Set(ref options);
			IntPtr zero = IntPtr.Zero;
			OnPeerActionRequiredCallbackInternal onPeerActionRequiredCallbackInternal = new OnPeerActionRequiredCallbackInternal(AntiCheatClientInterface.OnPeerActionRequiredCallbackInternalImplementation);
			Helper.AddCallback(out zero, clientData, notificationFn, onPeerActionRequiredCallbackInternal, Array.Empty<Delegate>());
			ulong num = Bindings.EOS_AntiCheatClient_AddNotifyPeerActionRequired(base.InnerHandle, ref addNotifyPeerActionRequiredOptionsInternal, zero, onPeerActionRequiredCallbackInternal);
			Helper.Dispose<AddNotifyPeerActionRequiredOptionsInternal>(ref addNotifyPeerActionRequiredOptionsInternal);
			Helper.AssignNotificationIdToCallback(zero, num);
			return num;
		}

		// Token: 0x06002D64 RID: 11620 RVA: 0x000431A4 File Offset: 0x000413A4
		public ulong AddNotifyPeerAuthStatusChanged(ref AddNotifyPeerAuthStatusChangedOptions options, object clientData, OnPeerAuthStatusChangedCallback notificationFn)
		{
			AddNotifyPeerAuthStatusChangedOptionsInternal addNotifyPeerAuthStatusChangedOptionsInternal = default(AddNotifyPeerAuthStatusChangedOptionsInternal);
			addNotifyPeerAuthStatusChangedOptionsInternal.Set(ref options);
			IntPtr zero = IntPtr.Zero;
			OnPeerAuthStatusChangedCallbackInternal onPeerAuthStatusChangedCallbackInternal = new OnPeerAuthStatusChangedCallbackInternal(AntiCheatClientInterface.OnPeerAuthStatusChangedCallbackInternalImplementation);
			Helper.AddCallback(out zero, clientData, notificationFn, onPeerAuthStatusChangedCallbackInternal, Array.Empty<Delegate>());
			ulong num = Bindings.EOS_AntiCheatClient_AddNotifyPeerAuthStatusChanged(base.InnerHandle, ref addNotifyPeerAuthStatusChangedOptionsInternal, zero, onPeerAuthStatusChangedCallbackInternal);
			Helper.Dispose<AddNotifyPeerAuthStatusChangedOptionsInternal>(ref addNotifyPeerAuthStatusChangedOptionsInternal);
			Helper.AssignNotificationIdToCallback(zero, num);
			return num;
		}

		// Token: 0x06002D65 RID: 11621 RVA: 0x00043210 File Offset: 0x00041410
		public Result BeginSession(ref BeginSessionOptions options)
		{
			BeginSessionOptionsInternal beginSessionOptionsInternal = default(BeginSessionOptionsInternal);
			beginSessionOptionsInternal.Set(ref options);
			Result result = Bindings.EOS_AntiCheatClient_BeginSession(base.InnerHandle, ref beginSessionOptionsInternal);
			Helper.Dispose<BeginSessionOptionsInternal>(ref beginSessionOptionsInternal);
			return result;
		}

		// Token: 0x06002D66 RID: 11622 RVA: 0x0004324C File Offset: 0x0004144C
		public Result EndSession(ref EndSessionOptions options)
		{
			EndSessionOptionsInternal endSessionOptionsInternal = default(EndSessionOptionsInternal);
			endSessionOptionsInternal.Set(ref options);
			Result result = Bindings.EOS_AntiCheatClient_EndSession(base.InnerHandle, ref endSessionOptionsInternal);
			Helper.Dispose<EndSessionOptionsInternal>(ref endSessionOptionsInternal);
			return result;
		}

		// Token: 0x06002D67 RID: 11623 RVA: 0x00043288 File Offset: 0x00041488
		public Result GetProtectMessageOutputLength(ref GetProtectMessageOutputLengthOptions options, out uint outBufferSizeBytes)
		{
			GetProtectMessageOutputLengthOptionsInternal getProtectMessageOutputLengthOptionsInternal = default(GetProtectMessageOutputLengthOptionsInternal);
			getProtectMessageOutputLengthOptionsInternal.Set(ref options);
			outBufferSizeBytes = Helper.GetDefault<uint>();
			Result result = Bindings.EOS_AntiCheatClient_GetProtectMessageOutputLength(base.InnerHandle, ref getProtectMessageOutputLengthOptionsInternal, ref outBufferSizeBytes);
			Helper.Dispose<GetProtectMessageOutputLengthOptionsInternal>(ref getProtectMessageOutputLengthOptionsInternal);
			return result;
		}

		// Token: 0x06002D68 RID: 11624 RVA: 0x000432CC File Offset: 0x000414CC
		public Result PollStatus(ref PollStatusOptions options, out AntiCheatClientViolationType outViolationType, out Utf8String outMessage)
		{
			PollStatusOptionsInternal pollStatusOptionsInternal = default(PollStatusOptionsInternal);
			pollStatusOptionsInternal.Set(ref options);
			outViolationType = Helper.GetDefault<AntiCheatClientViolationType>();
			uint outMessageLength = options.OutMessageLength;
			IntPtr intPtr = Helper.AddAllocation(outMessageLength);
			Result result = Bindings.EOS_AntiCheatClient_PollStatus(base.InnerHandle, ref pollStatusOptionsInternal, ref outViolationType, intPtr);
			Helper.Dispose<PollStatusOptionsInternal>(ref pollStatusOptionsInternal);
			Helper.Get(intPtr, out outMessage);
			Helper.Dispose(ref intPtr);
			return result;
		}

		// Token: 0x06002D69 RID: 11625 RVA: 0x00043330 File Offset: 0x00041530
		public Result ProtectMessage(ref ProtectMessageOptions options, ArraySegment<byte> outBuffer, out uint outBytesWritten)
		{
			ProtectMessageOptionsInternal protectMessageOptionsInternal = default(ProtectMessageOptionsInternal);
			protectMessageOptionsInternal.Set(ref options);
			outBytesWritten = 0U;
			IntPtr outBuffer2 = Helper.AddPinnedBuffer(outBuffer);
			Result result = Bindings.EOS_AntiCheatClient_ProtectMessage(base.InnerHandle, ref protectMessageOptionsInternal, outBuffer2, ref outBytesWritten);
			Helper.Dispose<ProtectMessageOptionsInternal>(ref protectMessageOptionsInternal);
			Helper.Dispose(ref outBuffer2);
			return result;
		}

		// Token: 0x06002D6A RID: 11626 RVA: 0x00043380 File Offset: 0x00041580
		public Result ReceiveMessageFromPeer(ref ReceiveMessageFromPeerOptions options)
		{
			ReceiveMessageFromPeerOptionsInternal receiveMessageFromPeerOptionsInternal = default(ReceiveMessageFromPeerOptionsInternal);
			receiveMessageFromPeerOptionsInternal.Set(ref options);
			Result result = Bindings.EOS_AntiCheatClient_ReceiveMessageFromPeer(base.InnerHandle, ref receiveMessageFromPeerOptionsInternal);
			Helper.Dispose<ReceiveMessageFromPeerOptionsInternal>(ref receiveMessageFromPeerOptionsInternal);
			return result;
		}

		// Token: 0x06002D6B RID: 11627 RVA: 0x000433BC File Offset: 0x000415BC
		public Result ReceiveMessageFromServer(ref ReceiveMessageFromServerOptions options)
		{
			ReceiveMessageFromServerOptionsInternal receiveMessageFromServerOptionsInternal = default(ReceiveMessageFromServerOptionsInternal);
			receiveMessageFromServerOptionsInternal.Set(ref options);
			Result result = Bindings.EOS_AntiCheatClient_ReceiveMessageFromServer(base.InnerHandle, ref receiveMessageFromServerOptionsInternal);
			Helper.Dispose<ReceiveMessageFromServerOptionsInternal>(ref receiveMessageFromServerOptionsInternal);
			return result;
		}

		// Token: 0x06002D6C RID: 11628 RVA: 0x000433F8 File Offset: 0x000415F8
		public Result RegisterPeer(ref RegisterPeerOptions options)
		{
			RegisterPeerOptionsInternal registerPeerOptionsInternal = default(RegisterPeerOptionsInternal);
			registerPeerOptionsInternal.Set(ref options);
			Result result = Bindings.EOS_AntiCheatClient_RegisterPeer(base.InnerHandle, ref registerPeerOptionsInternal);
			Helper.Dispose<RegisterPeerOptionsInternal>(ref registerPeerOptionsInternal);
			return result;
		}

		// Token: 0x06002D6D RID: 11629 RVA: 0x00043432 File Offset: 0x00041632
		public void RemoveNotifyClientIntegrityViolated(ulong notificationId)
		{
			Bindings.EOS_AntiCheatClient_RemoveNotifyClientIntegrityViolated(base.InnerHandle, notificationId);
			Helper.RemoveCallbackByNotificationId(notificationId);
		}

		// Token: 0x06002D6E RID: 11630 RVA: 0x00043449 File Offset: 0x00041649
		public void RemoveNotifyMessageToPeer(ulong notificationId)
		{
			Bindings.EOS_AntiCheatClient_RemoveNotifyMessageToPeer(base.InnerHandle, notificationId);
			Helper.RemoveCallbackByNotificationId(notificationId);
		}

		// Token: 0x06002D6F RID: 11631 RVA: 0x00043460 File Offset: 0x00041660
		public void RemoveNotifyMessageToServer(ulong notificationId)
		{
			Bindings.EOS_AntiCheatClient_RemoveNotifyMessageToServer(base.InnerHandle, notificationId);
			Helper.RemoveCallbackByNotificationId(notificationId);
		}

		// Token: 0x06002D70 RID: 11632 RVA: 0x00043477 File Offset: 0x00041677
		public void RemoveNotifyPeerActionRequired(ulong notificationId)
		{
			Bindings.EOS_AntiCheatClient_RemoveNotifyPeerActionRequired(base.InnerHandle, notificationId);
			Helper.RemoveCallbackByNotificationId(notificationId);
		}

		// Token: 0x06002D71 RID: 11633 RVA: 0x0004348E File Offset: 0x0004168E
		public void RemoveNotifyPeerAuthStatusChanged(ulong notificationId)
		{
			Bindings.EOS_AntiCheatClient_RemoveNotifyPeerAuthStatusChanged(base.InnerHandle, notificationId);
			Helper.RemoveCallbackByNotificationId(notificationId);
		}

		// Token: 0x06002D72 RID: 11634 RVA: 0x000434A8 File Offset: 0x000416A8
		public Result Reserved01(ref Reserved01Options options, out int outValue)
		{
			Reserved01OptionsInternal reserved01OptionsInternal = default(Reserved01OptionsInternal);
			reserved01OptionsInternal.Set(ref options);
			outValue = Helper.GetDefault<int>();
			Result result = Bindings.EOS_AntiCheatClient_Reserved01(base.InnerHandle, ref reserved01OptionsInternal, ref outValue);
			Helper.Dispose<Reserved01OptionsInternal>(ref reserved01OptionsInternal);
			return result;
		}

		// Token: 0x06002D73 RID: 11635 RVA: 0x000434EC File Offset: 0x000416EC
		public Result UnprotectMessage(ref UnprotectMessageOptions options, ArraySegment<byte> outBuffer, out uint outBytesWritten)
		{
			UnprotectMessageOptionsInternal unprotectMessageOptionsInternal = default(UnprotectMessageOptionsInternal);
			unprotectMessageOptionsInternal.Set(ref options);
			outBytesWritten = 0U;
			IntPtr outBuffer2 = Helper.AddPinnedBuffer(outBuffer);
			Result result = Bindings.EOS_AntiCheatClient_UnprotectMessage(base.InnerHandle, ref unprotectMessageOptionsInternal, outBuffer2, ref outBytesWritten);
			Helper.Dispose<UnprotectMessageOptionsInternal>(ref unprotectMessageOptionsInternal);
			Helper.Dispose(ref outBuffer2);
			return result;
		}

		// Token: 0x06002D74 RID: 11636 RVA: 0x0004353C File Offset: 0x0004173C
		public Result UnregisterPeer(ref UnregisterPeerOptions options)
		{
			UnregisterPeerOptionsInternal unregisterPeerOptionsInternal = default(UnregisterPeerOptionsInternal);
			unregisterPeerOptionsInternal.Set(ref options);
			Result result = Bindings.EOS_AntiCheatClient_UnregisterPeer(base.InnerHandle, ref unregisterPeerOptionsInternal);
			Helper.Dispose<UnregisterPeerOptionsInternal>(ref unregisterPeerOptionsInternal);
			return result;
		}

		// Token: 0x06002D75 RID: 11637 RVA: 0x00043578 File Offset: 0x00041778
		[MonoPInvokeCallback(typeof(OnClientIntegrityViolatedCallbackInternal))]
		internal static void OnClientIntegrityViolatedCallbackInternalImplementation(ref OnClientIntegrityViolatedCallbackInfoInternal data)
		{
			OnClientIntegrityViolatedCallback onClientIntegrityViolatedCallback;
			OnClientIntegrityViolatedCallbackInfo onClientIntegrityViolatedCallbackInfo;
			bool flag = Helper.TryGetCallback<OnClientIntegrityViolatedCallbackInfoInternal, OnClientIntegrityViolatedCallback, OnClientIntegrityViolatedCallbackInfo>(ref data, out onClientIntegrityViolatedCallback, out onClientIntegrityViolatedCallbackInfo);
			if (flag)
			{
				onClientIntegrityViolatedCallback(ref onClientIntegrityViolatedCallbackInfo);
			}
		}

		// Token: 0x06002D76 RID: 11638 RVA: 0x000435A0 File Offset: 0x000417A0
		[MonoPInvokeCallback(typeof(OnMessageToPeerCallbackInternal))]
		internal static void OnMessageToPeerCallbackInternalImplementation(ref OnMessageToClientCallbackInfoInternal data)
		{
			OnMessageToPeerCallback onMessageToPeerCallback;
			OnMessageToClientCallbackInfo onMessageToClientCallbackInfo;
			bool flag = Helper.TryGetCallback<OnMessageToClientCallbackInfoInternal, OnMessageToPeerCallback, OnMessageToClientCallbackInfo>(ref data, out onMessageToPeerCallback, out onMessageToClientCallbackInfo);
			if (flag)
			{
				onMessageToPeerCallback(ref onMessageToClientCallbackInfo);
			}
		}

		// Token: 0x06002D77 RID: 11639 RVA: 0x000435C8 File Offset: 0x000417C8
		[MonoPInvokeCallback(typeof(OnMessageToServerCallbackInternal))]
		internal static void OnMessageToServerCallbackInternalImplementation(ref OnMessageToServerCallbackInfoInternal data)
		{
			OnMessageToServerCallback onMessageToServerCallback;
			OnMessageToServerCallbackInfo onMessageToServerCallbackInfo;
			bool flag = Helper.TryGetCallback<OnMessageToServerCallbackInfoInternal, OnMessageToServerCallback, OnMessageToServerCallbackInfo>(ref data, out onMessageToServerCallback, out onMessageToServerCallbackInfo);
			if (flag)
			{
				onMessageToServerCallback(ref onMessageToServerCallbackInfo);
			}
		}

		// Token: 0x06002D78 RID: 11640 RVA: 0x000435F0 File Offset: 0x000417F0
		[MonoPInvokeCallback(typeof(OnPeerActionRequiredCallbackInternal))]
		internal static void OnPeerActionRequiredCallbackInternalImplementation(ref OnClientActionRequiredCallbackInfoInternal data)
		{
			OnPeerActionRequiredCallback onPeerActionRequiredCallback;
			OnClientActionRequiredCallbackInfo onClientActionRequiredCallbackInfo;
			bool flag = Helper.TryGetCallback<OnClientActionRequiredCallbackInfoInternal, OnPeerActionRequiredCallback, OnClientActionRequiredCallbackInfo>(ref data, out onPeerActionRequiredCallback, out onClientActionRequiredCallbackInfo);
			if (flag)
			{
				onPeerActionRequiredCallback(ref onClientActionRequiredCallbackInfo);
			}
		}

		// Token: 0x06002D79 RID: 11641 RVA: 0x00043618 File Offset: 0x00041818
		[MonoPInvokeCallback(typeof(OnPeerAuthStatusChangedCallbackInternal))]
		internal static void OnPeerAuthStatusChangedCallbackInternalImplementation(ref OnClientAuthStatusChangedCallbackInfoInternal data)
		{
			OnPeerAuthStatusChangedCallback onPeerAuthStatusChangedCallback;
			OnClientAuthStatusChangedCallbackInfo onClientAuthStatusChangedCallbackInfo;
			bool flag = Helper.TryGetCallback<OnClientAuthStatusChangedCallbackInfoInternal, OnPeerAuthStatusChangedCallback, OnClientAuthStatusChangedCallbackInfo>(ref data, out onPeerAuthStatusChangedCallback, out onClientAuthStatusChangedCallbackInfo);
			if (flag)
			{
				onPeerAuthStatusChangedCallback(ref onClientAuthStatusChangedCallbackInfo);
			}
		}

		// Token: 0x040013F5 RID: 5109
		public const int AddexternalintegritycatalogApiLatest = 1;

		// Token: 0x040013F6 RID: 5110
		public const int AddnotifyclientintegrityviolatedApiLatest = 1;

		// Token: 0x040013F7 RID: 5111
		public const int AddnotifymessagetopeerApiLatest = 1;

		// Token: 0x040013F8 RID: 5112
		public const int AddnotifymessagetoserverApiLatest = 1;

		// Token: 0x040013F9 RID: 5113
		public const int AddnotifypeeractionrequiredApiLatest = 1;

		// Token: 0x040013FA RID: 5114
		public const int AddnotifypeerauthstatuschangedApiLatest = 1;

		// Token: 0x040013FB RID: 5115
		public const int BeginsessionApiLatest = 3;

		// Token: 0x040013FC RID: 5116
		public const int EndsessionApiLatest = 1;

		// Token: 0x040013FD RID: 5117
		public const int GetprotectmessageoutputlengthApiLatest = 1;

		// Token: 0x040013FE RID: 5118
		public const int OnmessagetopeercallbackMaxMessageSize = 512;

		// Token: 0x040013FF RID: 5119
		public const int OnmessagetoservercallbackMaxMessageSize = 512;

		// Token: 0x04001400 RID: 5120
		public IntPtr PeerSelf = (IntPtr)(-1);

		// Token: 0x04001401 RID: 5121
		public const int PollstatusApiLatest = 1;

		// Token: 0x04001402 RID: 5122
		public const int ProtectmessageApiLatest = 1;

		// Token: 0x04001403 RID: 5123
		public const int ReceivemessagefrompeerApiLatest = 1;

		// Token: 0x04001404 RID: 5124
		public const int ReceivemessagefromserverApiLatest = 1;

		// Token: 0x04001405 RID: 5125
		public const int RegisterpeerApiLatest = 3;

		// Token: 0x04001406 RID: 5126
		public const int RegisterpeerMaxAuthenticationtimeout = 120;

		// Token: 0x04001407 RID: 5127
		public const int RegisterpeerMinAuthenticationtimeout = 40;

		// Token: 0x04001408 RID: 5128
		public const int Reserved01ApiLatest = 1;

		// Token: 0x04001409 RID: 5129
		public const int UnprotectmessageApiLatest = 1;

		// Token: 0x0400140A RID: 5130
		public const int UnregisterpeerApiLatest = 1;
	}
}
