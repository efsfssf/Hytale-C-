using System;
using Epic.OnlineServices.AntiCheatCommon;

namespace Epic.OnlineServices.AntiCheatServer
{
	// Token: 0x0200067B RID: 1659
	public sealed class AntiCheatServerInterface : Handle
	{
		// Token: 0x06002B16 RID: 11030 RVA: 0x0003F598 File Offset: 0x0003D798
		public AntiCheatServerInterface()
		{
		}

		// Token: 0x06002B17 RID: 11031 RVA: 0x0003F5A2 File Offset: 0x0003D7A2
		public AntiCheatServerInterface(IntPtr innerHandle) : base(innerHandle)
		{
		}

		// Token: 0x06002B18 RID: 11032 RVA: 0x0003F5B0 File Offset: 0x0003D7B0
		public ulong AddNotifyClientActionRequired(ref AddNotifyClientActionRequiredOptions options, object clientData, OnClientActionRequiredCallback notificationFn)
		{
			AddNotifyClientActionRequiredOptionsInternal addNotifyClientActionRequiredOptionsInternal = default(AddNotifyClientActionRequiredOptionsInternal);
			addNotifyClientActionRequiredOptionsInternal.Set(ref options);
			IntPtr zero = IntPtr.Zero;
			OnClientActionRequiredCallbackInternal onClientActionRequiredCallbackInternal = new OnClientActionRequiredCallbackInternal(AntiCheatServerInterface.OnClientActionRequiredCallbackInternalImplementation);
			Helper.AddCallback(out zero, clientData, notificationFn, onClientActionRequiredCallbackInternal, Array.Empty<Delegate>());
			ulong num = Bindings.EOS_AntiCheatServer_AddNotifyClientActionRequired(base.InnerHandle, ref addNotifyClientActionRequiredOptionsInternal, zero, onClientActionRequiredCallbackInternal);
			Helper.Dispose<AddNotifyClientActionRequiredOptionsInternal>(ref addNotifyClientActionRequiredOptionsInternal);
			Helper.AssignNotificationIdToCallback(zero, num);
			return num;
		}

		// Token: 0x06002B19 RID: 11033 RVA: 0x0003F61C File Offset: 0x0003D81C
		public ulong AddNotifyClientAuthStatusChanged(ref AddNotifyClientAuthStatusChangedOptions options, object clientData, OnClientAuthStatusChangedCallback notificationFn)
		{
			AddNotifyClientAuthStatusChangedOptionsInternal addNotifyClientAuthStatusChangedOptionsInternal = default(AddNotifyClientAuthStatusChangedOptionsInternal);
			addNotifyClientAuthStatusChangedOptionsInternal.Set(ref options);
			IntPtr zero = IntPtr.Zero;
			OnClientAuthStatusChangedCallbackInternal onClientAuthStatusChangedCallbackInternal = new OnClientAuthStatusChangedCallbackInternal(AntiCheatServerInterface.OnClientAuthStatusChangedCallbackInternalImplementation);
			Helper.AddCallback(out zero, clientData, notificationFn, onClientAuthStatusChangedCallbackInternal, Array.Empty<Delegate>());
			ulong num = Bindings.EOS_AntiCheatServer_AddNotifyClientAuthStatusChanged(base.InnerHandle, ref addNotifyClientAuthStatusChangedOptionsInternal, zero, onClientAuthStatusChangedCallbackInternal);
			Helper.Dispose<AddNotifyClientAuthStatusChangedOptionsInternal>(ref addNotifyClientAuthStatusChangedOptionsInternal);
			Helper.AssignNotificationIdToCallback(zero, num);
			return num;
		}

		// Token: 0x06002B1A RID: 11034 RVA: 0x0003F688 File Offset: 0x0003D888
		public ulong AddNotifyMessageToClient(ref AddNotifyMessageToClientOptions options, object clientData, OnMessageToClientCallback notificationFn)
		{
			AddNotifyMessageToClientOptionsInternal addNotifyMessageToClientOptionsInternal = default(AddNotifyMessageToClientOptionsInternal);
			addNotifyMessageToClientOptionsInternal.Set(ref options);
			IntPtr zero = IntPtr.Zero;
			OnMessageToClientCallbackInternal onMessageToClientCallbackInternal = new OnMessageToClientCallbackInternal(AntiCheatServerInterface.OnMessageToClientCallbackInternalImplementation);
			Helper.AddCallback(out zero, clientData, notificationFn, onMessageToClientCallbackInternal, Array.Empty<Delegate>());
			ulong num = Bindings.EOS_AntiCheatServer_AddNotifyMessageToClient(base.InnerHandle, ref addNotifyMessageToClientOptionsInternal, zero, onMessageToClientCallbackInternal);
			Helper.Dispose<AddNotifyMessageToClientOptionsInternal>(ref addNotifyMessageToClientOptionsInternal);
			Helper.AssignNotificationIdToCallback(zero, num);
			return num;
		}

		// Token: 0x06002B1B RID: 11035 RVA: 0x0003F6F4 File Offset: 0x0003D8F4
		public Result BeginSession(ref BeginSessionOptions options)
		{
			BeginSessionOptionsInternal beginSessionOptionsInternal = default(BeginSessionOptionsInternal);
			beginSessionOptionsInternal.Set(ref options);
			Result result = Bindings.EOS_AntiCheatServer_BeginSession(base.InnerHandle, ref beginSessionOptionsInternal);
			Helper.Dispose<BeginSessionOptionsInternal>(ref beginSessionOptionsInternal);
			return result;
		}

		// Token: 0x06002B1C RID: 11036 RVA: 0x0003F730 File Offset: 0x0003D930
		public Result EndSession(ref EndSessionOptions options)
		{
			EndSessionOptionsInternal endSessionOptionsInternal = default(EndSessionOptionsInternal);
			endSessionOptionsInternal.Set(ref options);
			Result result = Bindings.EOS_AntiCheatServer_EndSession(base.InnerHandle, ref endSessionOptionsInternal);
			Helper.Dispose<EndSessionOptionsInternal>(ref endSessionOptionsInternal);
			return result;
		}

		// Token: 0x06002B1D RID: 11037 RVA: 0x0003F76C File Offset: 0x0003D96C
		public Result GetProtectMessageOutputLength(ref GetProtectMessageOutputLengthOptions options, out uint outBufferSizeBytes)
		{
			GetProtectMessageOutputLengthOptionsInternal getProtectMessageOutputLengthOptionsInternal = default(GetProtectMessageOutputLengthOptionsInternal);
			getProtectMessageOutputLengthOptionsInternal.Set(ref options);
			outBufferSizeBytes = Helper.GetDefault<uint>();
			Result result = Bindings.EOS_AntiCheatServer_GetProtectMessageOutputLength(base.InnerHandle, ref getProtectMessageOutputLengthOptionsInternal, ref outBufferSizeBytes);
			Helper.Dispose<GetProtectMessageOutputLengthOptionsInternal>(ref getProtectMessageOutputLengthOptionsInternal);
			return result;
		}

		// Token: 0x06002B1E RID: 11038 RVA: 0x0003F7B0 File Offset: 0x0003D9B0
		public Result LogEvent(ref LogEventOptions options)
		{
			LogEventOptionsInternal logEventOptionsInternal = default(LogEventOptionsInternal);
			logEventOptionsInternal.Set(ref options);
			Result result = Bindings.EOS_AntiCheatServer_LogEvent(base.InnerHandle, ref logEventOptionsInternal);
			Helper.Dispose<LogEventOptionsInternal>(ref logEventOptionsInternal);
			return result;
		}

		// Token: 0x06002B1F RID: 11039 RVA: 0x0003F7EC File Offset: 0x0003D9EC
		public Result LogGameRoundEnd(ref LogGameRoundEndOptions options)
		{
			LogGameRoundEndOptionsInternal logGameRoundEndOptionsInternal = default(LogGameRoundEndOptionsInternal);
			logGameRoundEndOptionsInternal.Set(ref options);
			Result result = Bindings.EOS_AntiCheatServer_LogGameRoundEnd(base.InnerHandle, ref logGameRoundEndOptionsInternal);
			Helper.Dispose<LogGameRoundEndOptionsInternal>(ref logGameRoundEndOptionsInternal);
			return result;
		}

		// Token: 0x06002B20 RID: 11040 RVA: 0x0003F828 File Offset: 0x0003DA28
		public Result LogGameRoundStart(ref LogGameRoundStartOptions options)
		{
			LogGameRoundStartOptionsInternal logGameRoundStartOptionsInternal = default(LogGameRoundStartOptionsInternal);
			logGameRoundStartOptionsInternal.Set(ref options);
			Result result = Bindings.EOS_AntiCheatServer_LogGameRoundStart(base.InnerHandle, ref logGameRoundStartOptionsInternal);
			Helper.Dispose<LogGameRoundStartOptionsInternal>(ref logGameRoundStartOptionsInternal);
			return result;
		}

		// Token: 0x06002B21 RID: 11041 RVA: 0x0003F864 File Offset: 0x0003DA64
		public Result LogPlayerDespawn(ref LogPlayerDespawnOptions options)
		{
			LogPlayerDespawnOptionsInternal logPlayerDespawnOptionsInternal = default(LogPlayerDespawnOptionsInternal);
			logPlayerDespawnOptionsInternal.Set(ref options);
			Result result = Bindings.EOS_AntiCheatServer_LogPlayerDespawn(base.InnerHandle, ref logPlayerDespawnOptionsInternal);
			Helper.Dispose<LogPlayerDespawnOptionsInternal>(ref logPlayerDespawnOptionsInternal);
			return result;
		}

		// Token: 0x06002B22 RID: 11042 RVA: 0x0003F8A0 File Offset: 0x0003DAA0
		public Result LogPlayerRevive(ref LogPlayerReviveOptions options)
		{
			LogPlayerReviveOptionsInternal logPlayerReviveOptionsInternal = default(LogPlayerReviveOptionsInternal);
			logPlayerReviveOptionsInternal.Set(ref options);
			Result result = Bindings.EOS_AntiCheatServer_LogPlayerRevive(base.InnerHandle, ref logPlayerReviveOptionsInternal);
			Helper.Dispose<LogPlayerReviveOptionsInternal>(ref logPlayerReviveOptionsInternal);
			return result;
		}

		// Token: 0x06002B23 RID: 11043 RVA: 0x0003F8DC File Offset: 0x0003DADC
		public Result LogPlayerSpawn(ref LogPlayerSpawnOptions options)
		{
			LogPlayerSpawnOptionsInternal logPlayerSpawnOptionsInternal = default(LogPlayerSpawnOptionsInternal);
			logPlayerSpawnOptionsInternal.Set(ref options);
			Result result = Bindings.EOS_AntiCheatServer_LogPlayerSpawn(base.InnerHandle, ref logPlayerSpawnOptionsInternal);
			Helper.Dispose<LogPlayerSpawnOptionsInternal>(ref logPlayerSpawnOptionsInternal);
			return result;
		}

		// Token: 0x06002B24 RID: 11044 RVA: 0x0003F918 File Offset: 0x0003DB18
		public Result LogPlayerTakeDamage(ref LogPlayerTakeDamageOptions options)
		{
			LogPlayerTakeDamageOptionsInternal logPlayerTakeDamageOptionsInternal = default(LogPlayerTakeDamageOptionsInternal);
			logPlayerTakeDamageOptionsInternal.Set(ref options);
			Result result = Bindings.EOS_AntiCheatServer_LogPlayerTakeDamage(base.InnerHandle, ref logPlayerTakeDamageOptionsInternal);
			Helper.Dispose<LogPlayerTakeDamageOptionsInternal>(ref logPlayerTakeDamageOptionsInternal);
			return result;
		}

		// Token: 0x06002B25 RID: 11045 RVA: 0x0003F954 File Offset: 0x0003DB54
		public Result LogPlayerTick(ref LogPlayerTickOptions options)
		{
			LogPlayerTickOptionsInternal logPlayerTickOptionsInternal = default(LogPlayerTickOptionsInternal);
			logPlayerTickOptionsInternal.Set(ref options);
			Result result = Bindings.EOS_AntiCheatServer_LogPlayerTick(base.InnerHandle, ref logPlayerTickOptionsInternal);
			Helper.Dispose<LogPlayerTickOptionsInternal>(ref logPlayerTickOptionsInternal);
			return result;
		}

		// Token: 0x06002B26 RID: 11046 RVA: 0x0003F990 File Offset: 0x0003DB90
		public Result LogPlayerUseAbility(ref LogPlayerUseAbilityOptions options)
		{
			LogPlayerUseAbilityOptionsInternal logPlayerUseAbilityOptionsInternal = default(LogPlayerUseAbilityOptionsInternal);
			logPlayerUseAbilityOptionsInternal.Set(ref options);
			Result result = Bindings.EOS_AntiCheatServer_LogPlayerUseAbility(base.InnerHandle, ref logPlayerUseAbilityOptionsInternal);
			Helper.Dispose<LogPlayerUseAbilityOptionsInternal>(ref logPlayerUseAbilityOptionsInternal);
			return result;
		}

		// Token: 0x06002B27 RID: 11047 RVA: 0x0003F9CC File Offset: 0x0003DBCC
		public Result LogPlayerUseWeapon(ref LogPlayerUseWeaponOptions options)
		{
			LogPlayerUseWeaponOptionsInternal logPlayerUseWeaponOptionsInternal = default(LogPlayerUseWeaponOptionsInternal);
			logPlayerUseWeaponOptionsInternal.Set(ref options);
			Result result = Bindings.EOS_AntiCheatServer_LogPlayerUseWeapon(base.InnerHandle, ref logPlayerUseWeaponOptionsInternal);
			Helper.Dispose<LogPlayerUseWeaponOptionsInternal>(ref logPlayerUseWeaponOptionsInternal);
			return result;
		}

		// Token: 0x06002B28 RID: 11048 RVA: 0x0003FA08 File Offset: 0x0003DC08
		public Result ProtectMessage(ref ProtectMessageOptions options, ArraySegment<byte> outBuffer, out uint outBytesWritten)
		{
			ProtectMessageOptionsInternal protectMessageOptionsInternal = default(ProtectMessageOptionsInternal);
			protectMessageOptionsInternal.Set(ref options);
			outBytesWritten = 0U;
			IntPtr outBuffer2 = Helper.AddPinnedBuffer(outBuffer);
			Result result = Bindings.EOS_AntiCheatServer_ProtectMessage(base.InnerHandle, ref protectMessageOptionsInternal, outBuffer2, ref outBytesWritten);
			Helper.Dispose<ProtectMessageOptionsInternal>(ref protectMessageOptionsInternal);
			Helper.Dispose(ref outBuffer2);
			return result;
		}

		// Token: 0x06002B29 RID: 11049 RVA: 0x0003FA58 File Offset: 0x0003DC58
		public Result ReceiveMessageFromClient(ref ReceiveMessageFromClientOptions options)
		{
			ReceiveMessageFromClientOptionsInternal receiveMessageFromClientOptionsInternal = default(ReceiveMessageFromClientOptionsInternal);
			receiveMessageFromClientOptionsInternal.Set(ref options);
			Result result = Bindings.EOS_AntiCheatServer_ReceiveMessageFromClient(base.InnerHandle, ref receiveMessageFromClientOptionsInternal);
			Helper.Dispose<ReceiveMessageFromClientOptionsInternal>(ref receiveMessageFromClientOptionsInternal);
			return result;
		}

		// Token: 0x06002B2A RID: 11050 RVA: 0x0003FA94 File Offset: 0x0003DC94
		public Result RegisterClient(ref RegisterClientOptions options)
		{
			RegisterClientOptionsInternal registerClientOptionsInternal = default(RegisterClientOptionsInternal);
			registerClientOptionsInternal.Set(ref options);
			Result result = Bindings.EOS_AntiCheatServer_RegisterClient(base.InnerHandle, ref registerClientOptionsInternal);
			Helper.Dispose<RegisterClientOptionsInternal>(ref registerClientOptionsInternal);
			return result;
		}

		// Token: 0x06002B2B RID: 11051 RVA: 0x0003FAD0 File Offset: 0x0003DCD0
		public Result RegisterEvent(ref RegisterEventOptions options)
		{
			RegisterEventOptionsInternal registerEventOptionsInternal = default(RegisterEventOptionsInternal);
			registerEventOptionsInternal.Set(ref options);
			Result result = Bindings.EOS_AntiCheatServer_RegisterEvent(base.InnerHandle, ref registerEventOptionsInternal);
			Helper.Dispose<RegisterEventOptionsInternal>(ref registerEventOptionsInternal);
			return result;
		}

		// Token: 0x06002B2C RID: 11052 RVA: 0x0003FB0A File Offset: 0x0003DD0A
		public void RemoveNotifyClientActionRequired(ulong notificationId)
		{
			Bindings.EOS_AntiCheatServer_RemoveNotifyClientActionRequired(base.InnerHandle, notificationId);
			Helper.RemoveCallbackByNotificationId(notificationId);
		}

		// Token: 0x06002B2D RID: 11053 RVA: 0x0003FB21 File Offset: 0x0003DD21
		public void RemoveNotifyClientAuthStatusChanged(ulong notificationId)
		{
			Bindings.EOS_AntiCheatServer_RemoveNotifyClientAuthStatusChanged(base.InnerHandle, notificationId);
			Helper.RemoveCallbackByNotificationId(notificationId);
		}

		// Token: 0x06002B2E RID: 11054 RVA: 0x0003FB38 File Offset: 0x0003DD38
		public void RemoveNotifyMessageToClient(ulong notificationId)
		{
			Bindings.EOS_AntiCheatServer_RemoveNotifyMessageToClient(base.InnerHandle, notificationId);
			Helper.RemoveCallbackByNotificationId(notificationId);
		}

		// Token: 0x06002B2F RID: 11055 RVA: 0x0003FB50 File Offset: 0x0003DD50
		public Result SetClientDetails(ref SetClientDetailsOptions options)
		{
			SetClientDetailsOptionsInternal setClientDetailsOptionsInternal = default(SetClientDetailsOptionsInternal);
			setClientDetailsOptionsInternal.Set(ref options);
			Result result = Bindings.EOS_AntiCheatServer_SetClientDetails(base.InnerHandle, ref setClientDetailsOptionsInternal);
			Helper.Dispose<SetClientDetailsOptionsInternal>(ref setClientDetailsOptionsInternal);
			return result;
		}

		// Token: 0x06002B30 RID: 11056 RVA: 0x0003FB8C File Offset: 0x0003DD8C
		public Result SetClientNetworkState(ref SetClientNetworkStateOptions options)
		{
			SetClientNetworkStateOptionsInternal setClientNetworkStateOptionsInternal = default(SetClientNetworkStateOptionsInternal);
			setClientNetworkStateOptionsInternal.Set(ref options);
			Result result = Bindings.EOS_AntiCheatServer_SetClientNetworkState(base.InnerHandle, ref setClientNetworkStateOptionsInternal);
			Helper.Dispose<SetClientNetworkStateOptionsInternal>(ref setClientNetworkStateOptionsInternal);
			return result;
		}

		// Token: 0x06002B31 RID: 11057 RVA: 0x0003FBC8 File Offset: 0x0003DDC8
		public Result SetGameSessionId(ref SetGameSessionIdOptions options)
		{
			SetGameSessionIdOptionsInternal setGameSessionIdOptionsInternal = default(SetGameSessionIdOptionsInternal);
			setGameSessionIdOptionsInternal.Set(ref options);
			Result result = Bindings.EOS_AntiCheatServer_SetGameSessionId(base.InnerHandle, ref setGameSessionIdOptionsInternal);
			Helper.Dispose<SetGameSessionIdOptionsInternal>(ref setGameSessionIdOptionsInternal);
			return result;
		}

		// Token: 0x06002B32 RID: 11058 RVA: 0x0003FC04 File Offset: 0x0003DE04
		public Result UnprotectMessage(ref UnprotectMessageOptions options, ArraySegment<byte> outBuffer, out uint outBytesWritten)
		{
			UnprotectMessageOptionsInternal unprotectMessageOptionsInternal = default(UnprotectMessageOptionsInternal);
			unprotectMessageOptionsInternal.Set(ref options);
			outBytesWritten = 0U;
			IntPtr outBuffer2 = Helper.AddPinnedBuffer(outBuffer);
			Result result = Bindings.EOS_AntiCheatServer_UnprotectMessage(base.InnerHandle, ref unprotectMessageOptionsInternal, outBuffer2, ref outBytesWritten);
			Helper.Dispose<UnprotectMessageOptionsInternal>(ref unprotectMessageOptionsInternal);
			Helper.Dispose(ref outBuffer2);
			return result;
		}

		// Token: 0x06002B33 RID: 11059 RVA: 0x0003FC54 File Offset: 0x0003DE54
		public Result UnregisterClient(ref UnregisterClientOptions options)
		{
			UnregisterClientOptionsInternal unregisterClientOptionsInternal = default(UnregisterClientOptionsInternal);
			unregisterClientOptionsInternal.Set(ref options);
			Result result = Bindings.EOS_AntiCheatServer_UnregisterClient(base.InnerHandle, ref unregisterClientOptionsInternal);
			Helper.Dispose<UnregisterClientOptionsInternal>(ref unregisterClientOptionsInternal);
			return result;
		}

		// Token: 0x06002B34 RID: 11060 RVA: 0x0003FC90 File Offset: 0x0003DE90
		[MonoPInvokeCallback(typeof(OnClientActionRequiredCallbackInternal))]
		internal static void OnClientActionRequiredCallbackInternalImplementation(ref OnClientActionRequiredCallbackInfoInternal data)
		{
			OnClientActionRequiredCallback onClientActionRequiredCallback;
			OnClientActionRequiredCallbackInfo onClientActionRequiredCallbackInfo;
			bool flag = Helper.TryGetCallback<OnClientActionRequiredCallbackInfoInternal, OnClientActionRequiredCallback, OnClientActionRequiredCallbackInfo>(ref data, out onClientActionRequiredCallback, out onClientActionRequiredCallbackInfo);
			if (flag)
			{
				onClientActionRequiredCallback(ref onClientActionRequiredCallbackInfo);
			}
		}

		// Token: 0x06002B35 RID: 11061 RVA: 0x0003FCB8 File Offset: 0x0003DEB8
		[MonoPInvokeCallback(typeof(OnClientAuthStatusChangedCallbackInternal))]
		internal static void OnClientAuthStatusChangedCallbackInternalImplementation(ref OnClientAuthStatusChangedCallbackInfoInternal data)
		{
			OnClientAuthStatusChangedCallback onClientAuthStatusChangedCallback;
			OnClientAuthStatusChangedCallbackInfo onClientAuthStatusChangedCallbackInfo;
			bool flag = Helper.TryGetCallback<OnClientAuthStatusChangedCallbackInfoInternal, OnClientAuthStatusChangedCallback, OnClientAuthStatusChangedCallbackInfo>(ref data, out onClientAuthStatusChangedCallback, out onClientAuthStatusChangedCallbackInfo);
			if (flag)
			{
				onClientAuthStatusChangedCallback(ref onClientAuthStatusChangedCallbackInfo);
			}
		}

		// Token: 0x06002B36 RID: 11062 RVA: 0x0003FCE0 File Offset: 0x0003DEE0
		[MonoPInvokeCallback(typeof(OnMessageToClientCallbackInternal))]
		internal static void OnMessageToClientCallbackInternalImplementation(ref OnMessageToClientCallbackInfoInternal data)
		{
			OnMessageToClientCallback onMessageToClientCallback;
			OnMessageToClientCallbackInfo onMessageToClientCallbackInfo;
			bool flag = Helper.TryGetCallback<OnMessageToClientCallbackInfoInternal, OnMessageToClientCallback, OnMessageToClientCallbackInfo>(ref data, out onMessageToClientCallback, out onMessageToClientCallbackInfo);
			if (flag)
			{
				onMessageToClientCallback(ref onMessageToClientCallbackInfo);
			}
		}

		// Token: 0x04001276 RID: 4726
		public const int AddnotifyclientactionrequiredApiLatest = 1;

		// Token: 0x04001277 RID: 4727
		public const int AddnotifyclientauthstatuschangedApiLatest = 1;

		// Token: 0x04001278 RID: 4728
		public const int AddnotifymessagetoclientApiLatest = 1;

		// Token: 0x04001279 RID: 4729
		public const int BeginsessionApiLatest = 3;

		// Token: 0x0400127A RID: 4730
		public const int BeginsessionMaxRegistertimeout = 120;

		// Token: 0x0400127B RID: 4731
		public const int BeginsessionMinRegistertimeout = 10;

		// Token: 0x0400127C RID: 4732
		public const int EndsessionApiLatest = 1;

		// Token: 0x0400127D RID: 4733
		public const int GetprotectmessageoutputlengthApiLatest = 1;

		// Token: 0x0400127E RID: 4734
		public const int OnmessagetoclientcallbackMaxMessageSize = 512;

		// Token: 0x0400127F RID: 4735
		public const int ProtectmessageApiLatest = 1;

		// Token: 0x04001280 RID: 4736
		public const int ReceivemessagefromclientApiLatest = 1;

		// Token: 0x04001281 RID: 4737
		public const int RegisterclientApiLatest = 3;

		// Token: 0x04001282 RID: 4738
		public const int SetclientnetworkstateApiLatest = 1;

		// Token: 0x04001283 RID: 4739
		public const int UnprotectmessageApiLatest = 1;

		// Token: 0x04001284 RID: 4740
		public const int UnregisterclientApiLatest = 1;
	}
}
