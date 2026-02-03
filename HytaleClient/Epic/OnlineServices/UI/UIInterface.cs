using System;

namespace Epic.OnlineServices.UI
{
	// Token: 0x02000094 RID: 148
	public sealed class UIInterface : Handle
	{
		// Token: 0x060005EA RID: 1514 RVA: 0x0000849E File Offset: 0x0000669E
		public UIInterface()
		{
		}

		// Token: 0x060005EB RID: 1515 RVA: 0x000084A8 File Offset: 0x000066A8
		public UIInterface(IntPtr innerHandle) : base(innerHandle)
		{
		}

		// Token: 0x060005EC RID: 1516 RVA: 0x000084B4 File Offset: 0x000066B4
		public Result AcknowledgeEventId(ref AcknowledgeEventIdOptions options)
		{
			AcknowledgeEventIdOptionsInternal acknowledgeEventIdOptionsInternal = default(AcknowledgeEventIdOptionsInternal);
			acknowledgeEventIdOptionsInternal.Set(ref options);
			Result result = Bindings.EOS_UI_AcknowledgeEventId(base.InnerHandle, ref acknowledgeEventIdOptionsInternal);
			Helper.Dispose<AcknowledgeEventIdOptionsInternal>(ref acknowledgeEventIdOptionsInternal);
			return result;
		}

		// Token: 0x060005ED RID: 1517 RVA: 0x000084F0 File Offset: 0x000066F0
		public ulong AddNotifyDisplaySettingsUpdated(ref AddNotifyDisplaySettingsUpdatedOptions options, object clientData, OnDisplaySettingsUpdatedCallback notificationFn)
		{
			AddNotifyDisplaySettingsUpdatedOptionsInternal addNotifyDisplaySettingsUpdatedOptionsInternal = default(AddNotifyDisplaySettingsUpdatedOptionsInternal);
			addNotifyDisplaySettingsUpdatedOptionsInternal.Set(ref options);
			IntPtr zero = IntPtr.Zero;
			OnDisplaySettingsUpdatedCallbackInternal onDisplaySettingsUpdatedCallbackInternal = new OnDisplaySettingsUpdatedCallbackInternal(UIInterface.OnDisplaySettingsUpdatedCallbackInternalImplementation);
			Helper.AddCallback(out zero, clientData, notificationFn, onDisplaySettingsUpdatedCallbackInternal, Array.Empty<Delegate>());
			ulong num = Bindings.EOS_UI_AddNotifyDisplaySettingsUpdated(base.InnerHandle, ref addNotifyDisplaySettingsUpdatedOptionsInternal, zero, onDisplaySettingsUpdatedCallbackInternal);
			Helper.Dispose<AddNotifyDisplaySettingsUpdatedOptionsInternal>(ref addNotifyDisplaySettingsUpdatedOptionsInternal);
			Helper.AssignNotificationIdToCallback(zero, num);
			return num;
		}

		// Token: 0x060005EE RID: 1518 RVA: 0x0000855C File Offset: 0x0000675C
		public ulong AddNotifyMemoryMonitor(ref AddNotifyMemoryMonitorOptions options, object clientData, OnMemoryMonitorCallback notificationFn)
		{
			AddNotifyMemoryMonitorOptionsInternal addNotifyMemoryMonitorOptionsInternal = default(AddNotifyMemoryMonitorOptionsInternal);
			addNotifyMemoryMonitorOptionsInternal.Set(ref options);
			IntPtr zero = IntPtr.Zero;
			OnMemoryMonitorCallbackInternal onMemoryMonitorCallbackInternal = new OnMemoryMonitorCallbackInternal(UIInterface.OnMemoryMonitorCallbackInternalImplementation);
			Helper.AddCallback(out zero, clientData, notificationFn, onMemoryMonitorCallbackInternal, Array.Empty<Delegate>());
			ulong num = Bindings.EOS_UI_AddNotifyMemoryMonitor(base.InnerHandle, ref addNotifyMemoryMonitorOptionsInternal, zero, onMemoryMonitorCallbackInternal);
			Helper.Dispose<AddNotifyMemoryMonitorOptionsInternal>(ref addNotifyMemoryMonitorOptionsInternal);
			Helper.AssignNotificationIdToCallback(zero, num);
			return num;
		}

		// Token: 0x060005EF RID: 1519 RVA: 0x000085C8 File Offset: 0x000067C8
		public bool GetFriendsExclusiveInput(ref GetFriendsExclusiveInputOptions options)
		{
			GetFriendsExclusiveInputOptionsInternal getFriendsExclusiveInputOptionsInternal = default(GetFriendsExclusiveInputOptionsInternal);
			getFriendsExclusiveInputOptionsInternal.Set(ref options);
			int from = Bindings.EOS_UI_GetFriendsExclusiveInput(base.InnerHandle, ref getFriendsExclusiveInputOptionsInternal);
			Helper.Dispose<GetFriendsExclusiveInputOptionsInternal>(ref getFriendsExclusiveInputOptionsInternal);
			bool result;
			Helper.Get(from, out result);
			return result;
		}

		// Token: 0x060005F0 RID: 1520 RVA: 0x0000860C File Offset: 0x0000680C
		public bool GetFriendsVisible(ref GetFriendsVisibleOptions options)
		{
			GetFriendsVisibleOptionsInternal getFriendsVisibleOptionsInternal = default(GetFriendsVisibleOptionsInternal);
			getFriendsVisibleOptionsInternal.Set(ref options);
			int from = Bindings.EOS_UI_GetFriendsVisible(base.InnerHandle, ref getFriendsVisibleOptionsInternal);
			Helper.Dispose<GetFriendsVisibleOptionsInternal>(ref getFriendsVisibleOptionsInternal);
			bool result;
			Helper.Get(from, out result);
			return result;
		}

		// Token: 0x060005F1 RID: 1521 RVA: 0x00008650 File Offset: 0x00006850
		public NotificationLocation GetNotificationLocationPreference()
		{
			return Bindings.EOS_UI_GetNotificationLocationPreference(base.InnerHandle);
		}

		// Token: 0x060005F2 RID: 1522 RVA: 0x00008670 File Offset: 0x00006870
		public InputStateButtonFlags GetToggleFriendsButton(ref GetToggleFriendsButtonOptions options)
		{
			GetToggleFriendsButtonOptionsInternal getToggleFriendsButtonOptionsInternal = default(GetToggleFriendsButtonOptionsInternal);
			getToggleFriendsButtonOptionsInternal.Set(ref options);
			InputStateButtonFlags result = Bindings.EOS_UI_GetToggleFriendsButton(base.InnerHandle, ref getToggleFriendsButtonOptionsInternal);
			Helper.Dispose<GetToggleFriendsButtonOptionsInternal>(ref getToggleFriendsButtonOptionsInternal);
			return result;
		}

		// Token: 0x060005F3 RID: 1523 RVA: 0x000086AC File Offset: 0x000068AC
		public KeyCombination GetToggleFriendsKey(ref GetToggleFriendsKeyOptions options)
		{
			GetToggleFriendsKeyOptionsInternal getToggleFriendsKeyOptionsInternal = default(GetToggleFriendsKeyOptionsInternal);
			getToggleFriendsKeyOptionsInternal.Set(ref options);
			KeyCombination result = Bindings.EOS_UI_GetToggleFriendsKey(base.InnerHandle, ref getToggleFriendsKeyOptionsInternal);
			Helper.Dispose<GetToggleFriendsKeyOptionsInternal>(ref getToggleFriendsKeyOptionsInternal);
			return result;
		}

		// Token: 0x060005F4 RID: 1524 RVA: 0x000086E8 File Offset: 0x000068E8
		public void HideFriends(ref HideFriendsOptions options, object clientData, OnHideFriendsCallback completionDelegate)
		{
			HideFriendsOptionsInternal hideFriendsOptionsInternal = default(HideFriendsOptionsInternal);
			hideFriendsOptionsInternal.Set(ref options);
			IntPtr zero = IntPtr.Zero;
			OnHideFriendsCallbackInternal onHideFriendsCallbackInternal = new OnHideFriendsCallbackInternal(UIInterface.OnHideFriendsCallbackInternalImplementation);
			Helper.AddCallback(out zero, clientData, completionDelegate, onHideFriendsCallbackInternal, Array.Empty<Delegate>());
			Bindings.EOS_UI_HideFriends(base.InnerHandle, ref hideFriendsOptionsInternal, zero, onHideFriendsCallbackInternal);
			Helper.Dispose<HideFriendsOptionsInternal>(ref hideFriendsOptionsInternal);
		}

		// Token: 0x060005F5 RID: 1525 RVA: 0x00008744 File Offset: 0x00006944
		public bool IsSocialOverlayPaused(ref IsSocialOverlayPausedOptions options)
		{
			IsSocialOverlayPausedOptionsInternal isSocialOverlayPausedOptionsInternal = default(IsSocialOverlayPausedOptionsInternal);
			isSocialOverlayPausedOptionsInternal.Set(ref options);
			int from = Bindings.EOS_UI_IsSocialOverlayPaused(base.InnerHandle, ref isSocialOverlayPausedOptionsInternal);
			Helper.Dispose<IsSocialOverlayPausedOptionsInternal>(ref isSocialOverlayPausedOptionsInternal);
			bool result;
			Helper.Get(from, out result);
			return result;
		}

		// Token: 0x060005F6 RID: 1526 RVA: 0x00008788 File Offset: 0x00006988
		public bool IsValidButtonCombination(InputStateButtonFlags buttonCombination)
		{
			int from = Bindings.EOS_UI_IsValidButtonCombination(base.InnerHandle, buttonCombination);
			bool result;
			Helper.Get(from, out result);
			return result;
		}

		// Token: 0x060005F7 RID: 1527 RVA: 0x000087B4 File Offset: 0x000069B4
		public bool IsValidKeyCombination(KeyCombination keyCombination)
		{
			int from = Bindings.EOS_UI_IsValidKeyCombination(base.InnerHandle, keyCombination);
			bool result;
			Helper.Get(from, out result);
			return result;
		}

		// Token: 0x060005F8 RID: 1528 RVA: 0x000087E0 File Offset: 0x000069E0
		public Result PauseSocialOverlay(ref PauseSocialOverlayOptions options)
		{
			PauseSocialOverlayOptionsInternal pauseSocialOverlayOptionsInternal = default(PauseSocialOverlayOptionsInternal);
			pauseSocialOverlayOptionsInternal.Set(ref options);
			Result result = Bindings.EOS_UI_PauseSocialOverlay(base.InnerHandle, ref pauseSocialOverlayOptionsInternal);
			Helper.Dispose<PauseSocialOverlayOptionsInternal>(ref pauseSocialOverlayOptionsInternal);
			return result;
		}

		// Token: 0x060005F9 RID: 1529 RVA: 0x0000881C File Offset: 0x00006A1C
		public Result PrePresent(ref PrePresentOptions options)
		{
			PrePresentOptionsInternal prePresentOptionsInternal = default(PrePresentOptionsInternal);
			prePresentOptionsInternal.Set(ref options);
			Result result = Bindings.EOS_UI_PrePresent(base.InnerHandle, ref prePresentOptionsInternal);
			Helper.Dispose<PrePresentOptionsInternal>(ref prePresentOptionsInternal);
			return result;
		}

		// Token: 0x060005FA RID: 1530 RVA: 0x00008856 File Offset: 0x00006A56
		public void RemoveNotifyDisplaySettingsUpdated(ulong id)
		{
			Bindings.EOS_UI_RemoveNotifyDisplaySettingsUpdated(base.InnerHandle, id);
			Helper.RemoveCallbackByNotificationId(id);
		}

		// Token: 0x060005FB RID: 1531 RVA: 0x0000886D File Offset: 0x00006A6D
		public void RemoveNotifyMemoryMonitor(ulong id)
		{
			Bindings.EOS_UI_RemoveNotifyMemoryMonitor(base.InnerHandle, id);
			Helper.RemoveCallbackByNotificationId(id);
		}

		// Token: 0x060005FC RID: 1532 RVA: 0x00008884 File Offset: 0x00006A84
		public Result ReportInputState(ref ReportInputStateOptions options)
		{
			ReportInputStateOptionsInternal reportInputStateOptionsInternal = default(ReportInputStateOptionsInternal);
			reportInputStateOptionsInternal.Set(ref options);
			Result result = Bindings.EOS_UI_ReportInputState(base.InnerHandle, ref reportInputStateOptionsInternal);
			Helper.Dispose<ReportInputStateOptionsInternal>(ref reportInputStateOptionsInternal);
			return result;
		}

		// Token: 0x060005FD RID: 1533 RVA: 0x000088C0 File Offset: 0x00006AC0
		public Result SetDisplayPreference(ref SetDisplayPreferenceOptions options)
		{
			SetDisplayPreferenceOptionsInternal setDisplayPreferenceOptionsInternal = default(SetDisplayPreferenceOptionsInternal);
			setDisplayPreferenceOptionsInternal.Set(ref options);
			Result result = Bindings.EOS_UI_SetDisplayPreference(base.InnerHandle, ref setDisplayPreferenceOptionsInternal);
			Helper.Dispose<SetDisplayPreferenceOptionsInternal>(ref setDisplayPreferenceOptionsInternal);
			return result;
		}

		// Token: 0x060005FE RID: 1534 RVA: 0x000088FC File Offset: 0x00006AFC
		public Result SetToggleFriendsButton(ref SetToggleFriendsButtonOptions options)
		{
			SetToggleFriendsButtonOptionsInternal setToggleFriendsButtonOptionsInternal = default(SetToggleFriendsButtonOptionsInternal);
			setToggleFriendsButtonOptionsInternal.Set(ref options);
			Result result = Bindings.EOS_UI_SetToggleFriendsButton(base.InnerHandle, ref setToggleFriendsButtonOptionsInternal);
			Helper.Dispose<SetToggleFriendsButtonOptionsInternal>(ref setToggleFriendsButtonOptionsInternal);
			return result;
		}

		// Token: 0x060005FF RID: 1535 RVA: 0x00008938 File Offset: 0x00006B38
		public Result SetToggleFriendsKey(ref SetToggleFriendsKeyOptions options)
		{
			SetToggleFriendsKeyOptionsInternal setToggleFriendsKeyOptionsInternal = default(SetToggleFriendsKeyOptionsInternal);
			setToggleFriendsKeyOptionsInternal.Set(ref options);
			Result result = Bindings.EOS_UI_SetToggleFriendsKey(base.InnerHandle, ref setToggleFriendsKeyOptionsInternal);
			Helper.Dispose<SetToggleFriendsKeyOptionsInternal>(ref setToggleFriendsKeyOptionsInternal);
			return result;
		}

		// Token: 0x06000600 RID: 1536 RVA: 0x00008974 File Offset: 0x00006B74
		public void ShowBlockPlayer(ref ShowBlockPlayerOptions options, object clientData, OnShowBlockPlayerCallback completionDelegate)
		{
			ShowBlockPlayerOptionsInternal showBlockPlayerOptionsInternal = default(ShowBlockPlayerOptionsInternal);
			showBlockPlayerOptionsInternal.Set(ref options);
			IntPtr zero = IntPtr.Zero;
			OnShowBlockPlayerCallbackInternal onShowBlockPlayerCallbackInternal = new OnShowBlockPlayerCallbackInternal(UIInterface.OnShowBlockPlayerCallbackInternalImplementation);
			Helper.AddCallback(out zero, clientData, completionDelegate, onShowBlockPlayerCallbackInternal, Array.Empty<Delegate>());
			Bindings.EOS_UI_ShowBlockPlayer(base.InnerHandle, ref showBlockPlayerOptionsInternal, zero, onShowBlockPlayerCallbackInternal);
			Helper.Dispose<ShowBlockPlayerOptionsInternal>(ref showBlockPlayerOptionsInternal);
		}

		// Token: 0x06000601 RID: 1537 RVA: 0x000089D0 File Offset: 0x00006BD0
		public void ShowFriends(ref ShowFriendsOptions options, object clientData, OnShowFriendsCallback completionDelegate)
		{
			ShowFriendsOptionsInternal showFriendsOptionsInternal = default(ShowFriendsOptionsInternal);
			showFriendsOptionsInternal.Set(ref options);
			IntPtr zero = IntPtr.Zero;
			OnShowFriendsCallbackInternal onShowFriendsCallbackInternal = new OnShowFriendsCallbackInternal(UIInterface.OnShowFriendsCallbackInternalImplementation);
			Helper.AddCallback(out zero, clientData, completionDelegate, onShowFriendsCallbackInternal, Array.Empty<Delegate>());
			Bindings.EOS_UI_ShowFriends(base.InnerHandle, ref showFriendsOptionsInternal, zero, onShowFriendsCallbackInternal);
			Helper.Dispose<ShowFriendsOptionsInternal>(ref showFriendsOptionsInternal);
		}

		// Token: 0x06000602 RID: 1538 RVA: 0x00008A2C File Offset: 0x00006C2C
		public void ShowNativeProfile(ref ShowNativeProfileOptions options, object clientData, OnShowNativeProfileCallback completionDelegate)
		{
			ShowNativeProfileOptionsInternal showNativeProfileOptionsInternal = default(ShowNativeProfileOptionsInternal);
			showNativeProfileOptionsInternal.Set(ref options);
			IntPtr zero = IntPtr.Zero;
			OnShowNativeProfileCallbackInternal onShowNativeProfileCallbackInternal = new OnShowNativeProfileCallbackInternal(UIInterface.OnShowNativeProfileCallbackInternalImplementation);
			Helper.AddCallback(out zero, clientData, completionDelegate, onShowNativeProfileCallbackInternal, Array.Empty<Delegate>());
			Bindings.EOS_UI_ShowNativeProfile(base.InnerHandle, ref showNativeProfileOptionsInternal, zero, onShowNativeProfileCallbackInternal);
			Helper.Dispose<ShowNativeProfileOptionsInternal>(ref showNativeProfileOptionsInternal);
		}

		// Token: 0x06000603 RID: 1539 RVA: 0x00008A88 File Offset: 0x00006C88
		public void ShowReportPlayer(ref ShowReportPlayerOptions options, object clientData, OnShowReportPlayerCallback completionDelegate)
		{
			ShowReportPlayerOptionsInternal showReportPlayerOptionsInternal = default(ShowReportPlayerOptionsInternal);
			showReportPlayerOptionsInternal.Set(ref options);
			IntPtr zero = IntPtr.Zero;
			OnShowReportPlayerCallbackInternal onShowReportPlayerCallbackInternal = new OnShowReportPlayerCallbackInternal(UIInterface.OnShowReportPlayerCallbackInternalImplementation);
			Helper.AddCallback(out zero, clientData, completionDelegate, onShowReportPlayerCallbackInternal, Array.Empty<Delegate>());
			Bindings.EOS_UI_ShowReportPlayer(base.InnerHandle, ref showReportPlayerOptionsInternal, zero, onShowReportPlayerCallbackInternal);
			Helper.Dispose<ShowReportPlayerOptionsInternal>(ref showReportPlayerOptionsInternal);
		}

		// Token: 0x06000604 RID: 1540 RVA: 0x00008AE4 File Offset: 0x00006CE4
		[MonoPInvokeCallback(typeof(OnDisplaySettingsUpdatedCallbackInternal))]
		internal static void OnDisplaySettingsUpdatedCallbackInternalImplementation(ref OnDisplaySettingsUpdatedCallbackInfoInternal data)
		{
			OnDisplaySettingsUpdatedCallback onDisplaySettingsUpdatedCallback;
			OnDisplaySettingsUpdatedCallbackInfo onDisplaySettingsUpdatedCallbackInfo;
			bool flag = Helper.TryGetCallback<OnDisplaySettingsUpdatedCallbackInfoInternal, OnDisplaySettingsUpdatedCallback, OnDisplaySettingsUpdatedCallbackInfo>(ref data, out onDisplaySettingsUpdatedCallback, out onDisplaySettingsUpdatedCallbackInfo);
			if (flag)
			{
				onDisplaySettingsUpdatedCallback(ref onDisplaySettingsUpdatedCallbackInfo);
			}
		}

		// Token: 0x06000605 RID: 1541 RVA: 0x00008B0C File Offset: 0x00006D0C
		[MonoPInvokeCallback(typeof(OnHideFriendsCallbackInternal))]
		internal static void OnHideFriendsCallbackInternalImplementation(ref HideFriendsCallbackInfoInternal data)
		{
			OnHideFriendsCallback onHideFriendsCallback;
			HideFriendsCallbackInfo hideFriendsCallbackInfo;
			bool flag = Helper.TryGetAndRemoveCallback<HideFriendsCallbackInfoInternal, OnHideFriendsCallback, HideFriendsCallbackInfo>(ref data, out onHideFriendsCallback, out hideFriendsCallbackInfo);
			if (flag)
			{
				onHideFriendsCallback(ref hideFriendsCallbackInfo);
			}
		}

		// Token: 0x06000606 RID: 1542 RVA: 0x00008B34 File Offset: 0x00006D34
		[MonoPInvokeCallback(typeof(OnMemoryMonitorCallbackInternal))]
		internal static void OnMemoryMonitorCallbackInternalImplementation(ref MemoryMonitorCallbackInfoInternal data)
		{
			OnMemoryMonitorCallback onMemoryMonitorCallback;
			MemoryMonitorCallbackInfo memoryMonitorCallbackInfo;
			bool flag = Helper.TryGetCallback<MemoryMonitorCallbackInfoInternal, OnMemoryMonitorCallback, MemoryMonitorCallbackInfo>(ref data, out onMemoryMonitorCallback, out memoryMonitorCallbackInfo);
			if (flag)
			{
				onMemoryMonitorCallback(ref memoryMonitorCallbackInfo);
			}
		}

		// Token: 0x06000607 RID: 1543 RVA: 0x00008B5C File Offset: 0x00006D5C
		[MonoPInvokeCallback(typeof(OnShowBlockPlayerCallbackInternal))]
		internal static void OnShowBlockPlayerCallbackInternalImplementation(ref OnShowBlockPlayerCallbackInfoInternal data)
		{
			OnShowBlockPlayerCallback onShowBlockPlayerCallback;
			OnShowBlockPlayerCallbackInfo onShowBlockPlayerCallbackInfo;
			bool flag = Helper.TryGetAndRemoveCallback<OnShowBlockPlayerCallbackInfoInternal, OnShowBlockPlayerCallback, OnShowBlockPlayerCallbackInfo>(ref data, out onShowBlockPlayerCallback, out onShowBlockPlayerCallbackInfo);
			if (flag)
			{
				onShowBlockPlayerCallback(ref onShowBlockPlayerCallbackInfo);
			}
		}

		// Token: 0x06000608 RID: 1544 RVA: 0x00008B84 File Offset: 0x00006D84
		[MonoPInvokeCallback(typeof(OnShowFriendsCallbackInternal))]
		internal static void OnShowFriendsCallbackInternalImplementation(ref ShowFriendsCallbackInfoInternal data)
		{
			OnShowFriendsCallback onShowFriendsCallback;
			ShowFriendsCallbackInfo showFriendsCallbackInfo;
			bool flag = Helper.TryGetAndRemoveCallback<ShowFriendsCallbackInfoInternal, OnShowFriendsCallback, ShowFriendsCallbackInfo>(ref data, out onShowFriendsCallback, out showFriendsCallbackInfo);
			if (flag)
			{
				onShowFriendsCallback(ref showFriendsCallbackInfo);
			}
		}

		// Token: 0x06000609 RID: 1545 RVA: 0x00008BAC File Offset: 0x00006DAC
		[MonoPInvokeCallback(typeof(OnShowNativeProfileCallbackInternal))]
		internal static void OnShowNativeProfileCallbackInternalImplementation(ref ShowNativeProfileCallbackInfoInternal data)
		{
			OnShowNativeProfileCallback onShowNativeProfileCallback;
			ShowNativeProfileCallbackInfo showNativeProfileCallbackInfo;
			bool flag = Helper.TryGetAndRemoveCallback<ShowNativeProfileCallbackInfoInternal, OnShowNativeProfileCallback, ShowNativeProfileCallbackInfo>(ref data, out onShowNativeProfileCallback, out showNativeProfileCallbackInfo);
			if (flag)
			{
				onShowNativeProfileCallback(ref showNativeProfileCallbackInfo);
			}
		}

		// Token: 0x0600060A RID: 1546 RVA: 0x00008BD4 File Offset: 0x00006DD4
		[MonoPInvokeCallback(typeof(OnShowReportPlayerCallbackInternal))]
		internal static void OnShowReportPlayerCallbackInternalImplementation(ref OnShowReportPlayerCallbackInfoInternal data)
		{
			OnShowReportPlayerCallback onShowReportPlayerCallback;
			OnShowReportPlayerCallbackInfo onShowReportPlayerCallbackInfo;
			bool flag = Helper.TryGetAndRemoveCallback<OnShowReportPlayerCallbackInfoInternal, OnShowReportPlayerCallback, OnShowReportPlayerCallbackInfo>(ref data, out onShowReportPlayerCallback, out onShowReportPlayerCallbackInfo);
			if (flag)
			{
				onShowReportPlayerCallback(ref onShowReportPlayerCallbackInfo);
			}
		}

		// Token: 0x040002E2 RID: 738
		public const int AcknowledgecorrelationidApiLatest = 1;

		// Token: 0x040002E3 RID: 739
		public const int AcknowledgeeventidApiLatest = 1;

		// Token: 0x040002E4 RID: 740
		public const int AddnotifydisplaysettingsupdatedApiLatest = 1;

		// Token: 0x040002E5 RID: 741
		public const int AddnotifymemorymonitorApiLatest = 1;

		// Token: 0x040002E6 RID: 742
		public const int AddnotifymemorymonitoroptionsApiLatest = 1;

		// Token: 0x040002E7 RID: 743
		public const int EventidInvalid = 0;

		// Token: 0x040002E8 RID: 744
		public const int GetfriendsexclusiveinputApiLatest = 1;

		// Token: 0x040002E9 RID: 745
		public const int GetfriendsvisibleApiLatest = 1;

		// Token: 0x040002EA RID: 746
		public const int GettogglefriendsbuttonApiLatest = 1;

		// Token: 0x040002EB RID: 747
		public const int GettogglefriendskeyApiLatest = 1;

		// Token: 0x040002EC RID: 748
		public const int HidefriendsApiLatest = 1;

		// Token: 0x040002ED RID: 749
		public const int IssocialoverlaypausedApiLatest = 1;

		// Token: 0x040002EE RID: 750
		public const int MemorymonitorcallbackinfoApiLatest = 1;

		// Token: 0x040002EF RID: 751
		public const int PausesocialoverlayApiLatest = 1;

		// Token: 0x040002F0 RID: 752
		public const int PrepresentApiLatest = 1;

		// Token: 0x040002F1 RID: 753
		public const int RectApiLatest = 1;

		// Token: 0x040002F2 RID: 754
		public const int ReportinputstateApiLatest = 2;

		// Token: 0x040002F3 RID: 755
		public const int SetdisplaypreferenceApiLatest = 1;

		// Token: 0x040002F4 RID: 756
		public const int SettogglefriendsbuttonApiLatest = 1;

		// Token: 0x040002F5 RID: 757
		public const int SettogglefriendskeyApiLatest = 1;

		// Token: 0x040002F6 RID: 758
		public const int ShowblockplayerApiLatest = 1;

		// Token: 0x040002F7 RID: 759
		public const int ShowfriendsApiLatest = 1;

		// Token: 0x040002F8 RID: 760
		public const int ShownativeprofileApiLatest = 1;

		// Token: 0x040002F9 RID: 761
		public const int ShowreportplayerApiLatest = 1;
	}
}
