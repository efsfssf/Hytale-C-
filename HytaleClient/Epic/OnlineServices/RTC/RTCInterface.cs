using System;
using Epic.OnlineServices.RTCAudio;
using Epic.OnlineServices.RTCData;

namespace Epic.OnlineServices.RTC
{
	// Token: 0x020001D4 RID: 468
	public sealed class RTCInterface : Handle
	{
		// Token: 0x06000D92 RID: 3474 RVA: 0x00013B9F File Offset: 0x00011D9F
		public RTCInterface()
		{
		}

		// Token: 0x06000D93 RID: 3475 RVA: 0x00013BA9 File Offset: 0x00011DA9
		public RTCInterface(IntPtr innerHandle) : base(innerHandle)
		{
		}

		// Token: 0x06000D94 RID: 3476 RVA: 0x00013BB4 File Offset: 0x00011DB4
		public ulong AddNotifyDisconnected(ref AddNotifyDisconnectedOptions options, object clientData, OnDisconnectedCallback completionDelegate)
		{
			AddNotifyDisconnectedOptionsInternal addNotifyDisconnectedOptionsInternal = default(AddNotifyDisconnectedOptionsInternal);
			addNotifyDisconnectedOptionsInternal.Set(ref options);
			IntPtr zero = IntPtr.Zero;
			OnDisconnectedCallbackInternal onDisconnectedCallbackInternal = new OnDisconnectedCallbackInternal(RTCInterface.OnDisconnectedCallbackInternalImplementation);
			Helper.AddCallback(out zero, clientData, completionDelegate, onDisconnectedCallbackInternal, Array.Empty<Delegate>());
			ulong num = Bindings.EOS_RTC_AddNotifyDisconnected(base.InnerHandle, ref addNotifyDisconnectedOptionsInternal, zero, onDisconnectedCallbackInternal);
			Helper.Dispose<AddNotifyDisconnectedOptionsInternal>(ref addNotifyDisconnectedOptionsInternal);
			Helper.AssignNotificationIdToCallback(zero, num);
			return num;
		}

		// Token: 0x06000D95 RID: 3477 RVA: 0x00013C20 File Offset: 0x00011E20
		public ulong AddNotifyParticipantStatusChanged(ref AddNotifyParticipantStatusChangedOptions options, object clientData, OnParticipantStatusChangedCallback completionDelegate)
		{
			AddNotifyParticipantStatusChangedOptionsInternal addNotifyParticipantStatusChangedOptionsInternal = default(AddNotifyParticipantStatusChangedOptionsInternal);
			addNotifyParticipantStatusChangedOptionsInternal.Set(ref options);
			IntPtr zero = IntPtr.Zero;
			OnParticipantStatusChangedCallbackInternal onParticipantStatusChangedCallbackInternal = new OnParticipantStatusChangedCallbackInternal(RTCInterface.OnParticipantStatusChangedCallbackInternalImplementation);
			Helper.AddCallback(out zero, clientData, completionDelegate, onParticipantStatusChangedCallbackInternal, Array.Empty<Delegate>());
			ulong num = Bindings.EOS_RTC_AddNotifyParticipantStatusChanged(base.InnerHandle, ref addNotifyParticipantStatusChangedOptionsInternal, zero, onParticipantStatusChangedCallbackInternal);
			Helper.Dispose<AddNotifyParticipantStatusChangedOptionsInternal>(ref addNotifyParticipantStatusChangedOptionsInternal);
			Helper.AssignNotificationIdToCallback(zero, num);
			return num;
		}

		// Token: 0x06000D96 RID: 3478 RVA: 0x00013C8C File Offset: 0x00011E8C
		public ulong AddNotifyRoomStatisticsUpdated(ref AddNotifyRoomStatisticsUpdatedOptions options, object clientData, OnRoomStatisticsUpdatedCallback statisticsUpdateHandler)
		{
			AddNotifyRoomStatisticsUpdatedOptionsInternal addNotifyRoomStatisticsUpdatedOptionsInternal = default(AddNotifyRoomStatisticsUpdatedOptionsInternal);
			addNotifyRoomStatisticsUpdatedOptionsInternal.Set(ref options);
			IntPtr zero = IntPtr.Zero;
			OnRoomStatisticsUpdatedCallbackInternal onRoomStatisticsUpdatedCallbackInternal = new OnRoomStatisticsUpdatedCallbackInternal(RTCInterface.OnRoomStatisticsUpdatedCallbackInternalImplementation);
			Helper.AddCallback(out zero, clientData, statisticsUpdateHandler, onRoomStatisticsUpdatedCallbackInternal, Array.Empty<Delegate>());
			ulong num = Bindings.EOS_RTC_AddNotifyRoomStatisticsUpdated(base.InnerHandle, ref addNotifyRoomStatisticsUpdatedOptionsInternal, zero, onRoomStatisticsUpdatedCallbackInternal);
			Helper.Dispose<AddNotifyRoomStatisticsUpdatedOptionsInternal>(ref addNotifyRoomStatisticsUpdatedOptionsInternal);
			Helper.AssignNotificationIdToCallback(zero, num);
			return num;
		}

		// Token: 0x06000D97 RID: 3479 RVA: 0x00013CF8 File Offset: 0x00011EF8
		public void BlockParticipant(ref BlockParticipantOptions options, object clientData, OnBlockParticipantCallback completionDelegate)
		{
			BlockParticipantOptionsInternal blockParticipantOptionsInternal = default(BlockParticipantOptionsInternal);
			blockParticipantOptionsInternal.Set(ref options);
			IntPtr zero = IntPtr.Zero;
			OnBlockParticipantCallbackInternal onBlockParticipantCallbackInternal = new OnBlockParticipantCallbackInternal(RTCInterface.OnBlockParticipantCallbackInternalImplementation);
			Helper.AddCallback(out zero, clientData, completionDelegate, onBlockParticipantCallbackInternal, Array.Empty<Delegate>());
			Bindings.EOS_RTC_BlockParticipant(base.InnerHandle, ref blockParticipantOptionsInternal, zero, onBlockParticipantCallbackInternal);
			Helper.Dispose<BlockParticipantOptionsInternal>(ref blockParticipantOptionsInternal);
		}

		// Token: 0x06000D98 RID: 3480 RVA: 0x00013D54 File Offset: 0x00011F54
		public RTCAudioInterface GetAudioInterface()
		{
			IntPtr from = Bindings.EOS_RTC_GetAudioInterface(base.InnerHandle);
			RTCAudioInterface result;
			Helper.Get<RTCAudioInterface>(from, out result);
			return result;
		}

		// Token: 0x06000D99 RID: 3481 RVA: 0x00013D7C File Offset: 0x00011F7C
		public RTCDataInterface GetDataInterface()
		{
			IntPtr from = Bindings.EOS_RTC_GetDataInterface(base.InnerHandle);
			RTCDataInterface result;
			Helper.Get<RTCDataInterface>(from, out result);
			return result;
		}

		// Token: 0x06000D9A RID: 3482 RVA: 0x00013DA4 File Offset: 0x00011FA4
		public void JoinRoom(ref JoinRoomOptions options, object clientData, OnJoinRoomCallback completionDelegate)
		{
			JoinRoomOptionsInternal joinRoomOptionsInternal = default(JoinRoomOptionsInternal);
			joinRoomOptionsInternal.Set(ref options);
			IntPtr zero = IntPtr.Zero;
			OnJoinRoomCallbackInternal onJoinRoomCallbackInternal = new OnJoinRoomCallbackInternal(RTCInterface.OnJoinRoomCallbackInternalImplementation);
			Helper.AddCallback(out zero, clientData, completionDelegate, onJoinRoomCallbackInternal, Array.Empty<Delegate>());
			Bindings.EOS_RTC_JoinRoom(base.InnerHandle, ref joinRoomOptionsInternal, zero, onJoinRoomCallbackInternal);
			Helper.Dispose<JoinRoomOptionsInternal>(ref joinRoomOptionsInternal);
		}

		// Token: 0x06000D9B RID: 3483 RVA: 0x00013E00 File Offset: 0x00012000
		public void LeaveRoom(ref LeaveRoomOptions options, object clientData, OnLeaveRoomCallback completionDelegate)
		{
			LeaveRoomOptionsInternal leaveRoomOptionsInternal = default(LeaveRoomOptionsInternal);
			leaveRoomOptionsInternal.Set(ref options);
			IntPtr zero = IntPtr.Zero;
			OnLeaveRoomCallbackInternal onLeaveRoomCallbackInternal = new OnLeaveRoomCallbackInternal(RTCInterface.OnLeaveRoomCallbackInternalImplementation);
			Helper.AddCallback(out zero, clientData, completionDelegate, onLeaveRoomCallbackInternal, Array.Empty<Delegate>());
			Bindings.EOS_RTC_LeaveRoom(base.InnerHandle, ref leaveRoomOptionsInternal, zero, onLeaveRoomCallbackInternal);
			Helper.Dispose<LeaveRoomOptionsInternal>(ref leaveRoomOptionsInternal);
		}

		// Token: 0x06000D9C RID: 3484 RVA: 0x00013E5A File Offset: 0x0001205A
		public void RemoveNotifyDisconnected(ulong notificationId)
		{
			Bindings.EOS_RTC_RemoveNotifyDisconnected(base.InnerHandle, notificationId);
			Helper.RemoveCallbackByNotificationId(notificationId);
		}

		// Token: 0x06000D9D RID: 3485 RVA: 0x00013E71 File Offset: 0x00012071
		public void RemoveNotifyParticipantStatusChanged(ulong notificationId)
		{
			Bindings.EOS_RTC_RemoveNotifyParticipantStatusChanged(base.InnerHandle, notificationId);
			Helper.RemoveCallbackByNotificationId(notificationId);
		}

		// Token: 0x06000D9E RID: 3486 RVA: 0x00013E88 File Offset: 0x00012088
		public void RemoveNotifyRoomStatisticsUpdated(ulong notificationId)
		{
			Bindings.EOS_RTC_RemoveNotifyRoomStatisticsUpdated(base.InnerHandle, notificationId);
			Helper.RemoveCallbackByNotificationId(notificationId);
		}

		// Token: 0x06000D9F RID: 3487 RVA: 0x00013EA0 File Offset: 0x000120A0
		public Result SetRoomSetting(ref SetRoomSettingOptions options)
		{
			SetRoomSettingOptionsInternal setRoomSettingOptionsInternal = default(SetRoomSettingOptionsInternal);
			setRoomSettingOptionsInternal.Set(ref options);
			Result result = Bindings.EOS_RTC_SetRoomSetting(base.InnerHandle, ref setRoomSettingOptionsInternal);
			Helper.Dispose<SetRoomSettingOptionsInternal>(ref setRoomSettingOptionsInternal);
			return result;
		}

		// Token: 0x06000DA0 RID: 3488 RVA: 0x00013EDC File Offset: 0x000120DC
		public Result SetSetting(ref SetSettingOptions options)
		{
			SetSettingOptionsInternal setSettingOptionsInternal = default(SetSettingOptionsInternal);
			setSettingOptionsInternal.Set(ref options);
			Result result = Bindings.EOS_RTC_SetSetting(base.InnerHandle, ref setSettingOptionsInternal);
			Helper.Dispose<SetSettingOptionsInternal>(ref setSettingOptionsInternal);
			return result;
		}

		// Token: 0x06000DA1 RID: 3489 RVA: 0x00013F18 File Offset: 0x00012118
		[MonoPInvokeCallback(typeof(OnBlockParticipantCallbackInternal))]
		internal static void OnBlockParticipantCallbackInternalImplementation(ref BlockParticipantCallbackInfoInternal data)
		{
			OnBlockParticipantCallback onBlockParticipantCallback;
			BlockParticipantCallbackInfo blockParticipantCallbackInfo;
			bool flag = Helper.TryGetAndRemoveCallback<BlockParticipantCallbackInfoInternal, OnBlockParticipantCallback, BlockParticipantCallbackInfo>(ref data, out onBlockParticipantCallback, out blockParticipantCallbackInfo);
			if (flag)
			{
				onBlockParticipantCallback(ref blockParticipantCallbackInfo);
			}
		}

		// Token: 0x06000DA2 RID: 3490 RVA: 0x00013F40 File Offset: 0x00012140
		[MonoPInvokeCallback(typeof(OnDisconnectedCallbackInternal))]
		internal static void OnDisconnectedCallbackInternalImplementation(ref DisconnectedCallbackInfoInternal data)
		{
			OnDisconnectedCallback onDisconnectedCallback;
			DisconnectedCallbackInfo disconnectedCallbackInfo;
			bool flag = Helper.TryGetCallback<DisconnectedCallbackInfoInternal, OnDisconnectedCallback, DisconnectedCallbackInfo>(ref data, out onDisconnectedCallback, out disconnectedCallbackInfo);
			if (flag)
			{
				onDisconnectedCallback(ref disconnectedCallbackInfo);
			}
		}

		// Token: 0x06000DA3 RID: 3491 RVA: 0x00013F68 File Offset: 0x00012168
		[MonoPInvokeCallback(typeof(OnJoinRoomCallbackInternal))]
		internal static void OnJoinRoomCallbackInternalImplementation(ref JoinRoomCallbackInfoInternal data)
		{
			OnJoinRoomCallback onJoinRoomCallback;
			JoinRoomCallbackInfo joinRoomCallbackInfo;
			bool flag = Helper.TryGetAndRemoveCallback<JoinRoomCallbackInfoInternal, OnJoinRoomCallback, JoinRoomCallbackInfo>(ref data, out onJoinRoomCallback, out joinRoomCallbackInfo);
			if (flag)
			{
				onJoinRoomCallback(ref joinRoomCallbackInfo);
			}
		}

		// Token: 0x06000DA4 RID: 3492 RVA: 0x00013F90 File Offset: 0x00012190
		[MonoPInvokeCallback(typeof(OnLeaveRoomCallbackInternal))]
		internal static void OnLeaveRoomCallbackInternalImplementation(ref LeaveRoomCallbackInfoInternal data)
		{
			OnLeaveRoomCallback onLeaveRoomCallback;
			LeaveRoomCallbackInfo leaveRoomCallbackInfo;
			bool flag = Helper.TryGetAndRemoveCallback<LeaveRoomCallbackInfoInternal, OnLeaveRoomCallback, LeaveRoomCallbackInfo>(ref data, out onLeaveRoomCallback, out leaveRoomCallbackInfo);
			if (flag)
			{
				onLeaveRoomCallback(ref leaveRoomCallbackInfo);
			}
		}

		// Token: 0x06000DA5 RID: 3493 RVA: 0x00013FB8 File Offset: 0x000121B8
		[MonoPInvokeCallback(typeof(OnParticipantStatusChangedCallbackInternal))]
		internal static void OnParticipantStatusChangedCallbackInternalImplementation(ref ParticipantStatusChangedCallbackInfoInternal data)
		{
			OnParticipantStatusChangedCallback onParticipantStatusChangedCallback;
			ParticipantStatusChangedCallbackInfo participantStatusChangedCallbackInfo;
			bool flag = Helper.TryGetCallback<ParticipantStatusChangedCallbackInfoInternal, OnParticipantStatusChangedCallback, ParticipantStatusChangedCallbackInfo>(ref data, out onParticipantStatusChangedCallback, out participantStatusChangedCallbackInfo);
			if (flag)
			{
				onParticipantStatusChangedCallback(ref participantStatusChangedCallbackInfo);
			}
		}

		// Token: 0x06000DA6 RID: 3494 RVA: 0x00013FE0 File Offset: 0x000121E0
		[MonoPInvokeCallback(typeof(OnRoomStatisticsUpdatedCallbackInternal))]
		internal static void OnRoomStatisticsUpdatedCallbackInternalImplementation(ref RoomStatisticsUpdatedInfoInternal data)
		{
			OnRoomStatisticsUpdatedCallback onRoomStatisticsUpdatedCallback;
			RoomStatisticsUpdatedInfo roomStatisticsUpdatedInfo;
			bool flag = Helper.TryGetCallback<RoomStatisticsUpdatedInfoInternal, OnRoomStatisticsUpdatedCallback, RoomStatisticsUpdatedInfo>(ref data, out onRoomStatisticsUpdatedCallback, out roomStatisticsUpdatedInfo);
			if (flag)
			{
				onRoomStatisticsUpdatedCallback(ref roomStatisticsUpdatedInfo);
			}
		}

		// Token: 0x04000621 RID: 1569
		public const int AddnotifydisconnectedApiLatest = 1;

		// Token: 0x04000622 RID: 1570
		public const int AddnotifyparticipantstatuschangedApiLatest = 1;

		// Token: 0x04000623 RID: 1571
		public const int AddnotifyroomstatisticsupdatedApiLatest = 1;

		// Token: 0x04000624 RID: 1572
		public const int BlockparticipantApiLatest = 1;

		// Token: 0x04000625 RID: 1573
		public const int JoinroomApiLatest = 1;

		// Token: 0x04000626 RID: 1574
		public const int LeaveroomApiLatest = 1;

		// Token: 0x04000627 RID: 1575
		public const int OptionApiLatest = 1;

		// Token: 0x04000628 RID: 1576
		public const int OptionKeyMaxcharcount = 256;

		// Token: 0x04000629 RID: 1577
		public const int OptionValueMaxcharcount = 256;

		// Token: 0x0400062A RID: 1578
		public const int ParticipantmetadataApiLatest = 1;

		// Token: 0x0400062B RID: 1579
		public const int ParticipantmetadataKeyMaxcharcount = 256;

		// Token: 0x0400062C RID: 1580
		public const int ParticipantmetadataValueMaxcharcount = 256;

		// Token: 0x0400062D RID: 1581
		public const int SetroomsettingApiLatest = 1;

		// Token: 0x0400062E RID: 1582
		public const int SetsettingApiLatest = 1;
	}
}
