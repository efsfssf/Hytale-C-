using System;

namespace Epic.OnlineServices.RTCAudio
{
	// Token: 0x0200025F RID: 607
	public sealed class RTCAudioInterface : Handle
	{
		// Token: 0x060010CF RID: 4303 RVA: 0x00017F45 File Offset: 0x00016145
		public RTCAudioInterface()
		{
		}

		// Token: 0x060010D0 RID: 4304 RVA: 0x00017F4F File Offset: 0x0001614F
		public RTCAudioInterface(IntPtr innerHandle) : base(innerHandle)
		{
		}

		// Token: 0x060010D1 RID: 4305 RVA: 0x00017F5C File Offset: 0x0001615C
		public ulong AddNotifyAudioBeforeRender(ref AddNotifyAudioBeforeRenderOptions options, object clientData, OnAudioBeforeRenderCallback completionDelegate)
		{
			AddNotifyAudioBeforeRenderOptionsInternal addNotifyAudioBeforeRenderOptionsInternal = default(AddNotifyAudioBeforeRenderOptionsInternal);
			addNotifyAudioBeforeRenderOptionsInternal.Set(ref options);
			IntPtr zero = IntPtr.Zero;
			OnAudioBeforeRenderCallbackInternal onAudioBeforeRenderCallbackInternal = new OnAudioBeforeRenderCallbackInternal(RTCAudioInterface.OnAudioBeforeRenderCallbackInternalImplementation);
			Helper.AddCallback(out zero, clientData, completionDelegate, onAudioBeforeRenderCallbackInternal, Array.Empty<Delegate>());
			ulong num = Bindings.EOS_RTCAudio_AddNotifyAudioBeforeRender(base.InnerHandle, ref addNotifyAudioBeforeRenderOptionsInternal, zero, onAudioBeforeRenderCallbackInternal);
			Helper.Dispose<AddNotifyAudioBeforeRenderOptionsInternal>(ref addNotifyAudioBeforeRenderOptionsInternal);
			Helper.AssignNotificationIdToCallback(zero, num);
			return num;
		}

		// Token: 0x060010D2 RID: 4306 RVA: 0x00017FC8 File Offset: 0x000161C8
		public ulong AddNotifyAudioBeforeSend(ref AddNotifyAudioBeforeSendOptions options, object clientData, OnAudioBeforeSendCallback completionDelegate)
		{
			AddNotifyAudioBeforeSendOptionsInternal addNotifyAudioBeforeSendOptionsInternal = default(AddNotifyAudioBeforeSendOptionsInternal);
			addNotifyAudioBeforeSendOptionsInternal.Set(ref options);
			IntPtr zero = IntPtr.Zero;
			OnAudioBeforeSendCallbackInternal onAudioBeforeSendCallbackInternal = new OnAudioBeforeSendCallbackInternal(RTCAudioInterface.OnAudioBeforeSendCallbackInternalImplementation);
			Helper.AddCallback(out zero, clientData, completionDelegate, onAudioBeforeSendCallbackInternal, Array.Empty<Delegate>());
			ulong num = Bindings.EOS_RTCAudio_AddNotifyAudioBeforeSend(base.InnerHandle, ref addNotifyAudioBeforeSendOptionsInternal, zero, onAudioBeforeSendCallbackInternal);
			Helper.Dispose<AddNotifyAudioBeforeSendOptionsInternal>(ref addNotifyAudioBeforeSendOptionsInternal);
			Helper.AssignNotificationIdToCallback(zero, num);
			return num;
		}

		// Token: 0x060010D3 RID: 4307 RVA: 0x00018034 File Offset: 0x00016234
		public ulong AddNotifyAudioDevicesChanged(ref AddNotifyAudioDevicesChangedOptions options, object clientData, OnAudioDevicesChangedCallback completionDelegate)
		{
			AddNotifyAudioDevicesChangedOptionsInternal addNotifyAudioDevicesChangedOptionsInternal = default(AddNotifyAudioDevicesChangedOptionsInternal);
			addNotifyAudioDevicesChangedOptionsInternal.Set(ref options);
			IntPtr zero = IntPtr.Zero;
			OnAudioDevicesChangedCallbackInternal onAudioDevicesChangedCallbackInternal = new OnAudioDevicesChangedCallbackInternal(RTCAudioInterface.OnAudioDevicesChangedCallbackInternalImplementation);
			Helper.AddCallback(out zero, clientData, completionDelegate, onAudioDevicesChangedCallbackInternal, Array.Empty<Delegate>());
			ulong num = Bindings.EOS_RTCAudio_AddNotifyAudioDevicesChanged(base.InnerHandle, ref addNotifyAudioDevicesChangedOptionsInternal, zero, onAudioDevicesChangedCallbackInternal);
			Helper.Dispose<AddNotifyAudioDevicesChangedOptionsInternal>(ref addNotifyAudioDevicesChangedOptionsInternal);
			Helper.AssignNotificationIdToCallback(zero, num);
			return num;
		}

		// Token: 0x060010D4 RID: 4308 RVA: 0x000180A0 File Offset: 0x000162A0
		public ulong AddNotifyAudioInputState(ref AddNotifyAudioInputStateOptions options, object clientData, OnAudioInputStateCallback completionDelegate)
		{
			AddNotifyAudioInputStateOptionsInternal addNotifyAudioInputStateOptionsInternal = default(AddNotifyAudioInputStateOptionsInternal);
			addNotifyAudioInputStateOptionsInternal.Set(ref options);
			IntPtr zero = IntPtr.Zero;
			OnAudioInputStateCallbackInternal onAudioInputStateCallbackInternal = new OnAudioInputStateCallbackInternal(RTCAudioInterface.OnAudioInputStateCallbackInternalImplementation);
			Helper.AddCallback(out zero, clientData, completionDelegate, onAudioInputStateCallbackInternal, Array.Empty<Delegate>());
			ulong num = Bindings.EOS_RTCAudio_AddNotifyAudioInputState(base.InnerHandle, ref addNotifyAudioInputStateOptionsInternal, zero, onAudioInputStateCallbackInternal);
			Helper.Dispose<AddNotifyAudioInputStateOptionsInternal>(ref addNotifyAudioInputStateOptionsInternal);
			Helper.AssignNotificationIdToCallback(zero, num);
			return num;
		}

		// Token: 0x060010D5 RID: 4309 RVA: 0x0001810C File Offset: 0x0001630C
		public ulong AddNotifyAudioOutputState(ref AddNotifyAudioOutputStateOptions options, object clientData, OnAudioOutputStateCallback completionDelegate)
		{
			AddNotifyAudioOutputStateOptionsInternal addNotifyAudioOutputStateOptionsInternal = default(AddNotifyAudioOutputStateOptionsInternal);
			addNotifyAudioOutputStateOptionsInternal.Set(ref options);
			IntPtr zero = IntPtr.Zero;
			OnAudioOutputStateCallbackInternal onAudioOutputStateCallbackInternal = new OnAudioOutputStateCallbackInternal(RTCAudioInterface.OnAudioOutputStateCallbackInternalImplementation);
			Helper.AddCallback(out zero, clientData, completionDelegate, onAudioOutputStateCallbackInternal, Array.Empty<Delegate>());
			ulong num = Bindings.EOS_RTCAudio_AddNotifyAudioOutputState(base.InnerHandle, ref addNotifyAudioOutputStateOptionsInternal, zero, onAudioOutputStateCallbackInternal);
			Helper.Dispose<AddNotifyAudioOutputStateOptionsInternal>(ref addNotifyAudioOutputStateOptionsInternal);
			Helper.AssignNotificationIdToCallback(zero, num);
			return num;
		}

		// Token: 0x060010D6 RID: 4310 RVA: 0x00018178 File Offset: 0x00016378
		public ulong AddNotifyParticipantUpdated(ref AddNotifyParticipantUpdatedOptions options, object clientData, OnParticipantUpdatedCallback completionDelegate)
		{
			AddNotifyParticipantUpdatedOptionsInternal addNotifyParticipantUpdatedOptionsInternal = default(AddNotifyParticipantUpdatedOptionsInternal);
			addNotifyParticipantUpdatedOptionsInternal.Set(ref options);
			IntPtr zero = IntPtr.Zero;
			OnParticipantUpdatedCallbackInternal onParticipantUpdatedCallbackInternal = new OnParticipantUpdatedCallbackInternal(RTCAudioInterface.OnParticipantUpdatedCallbackInternalImplementation);
			Helper.AddCallback(out zero, clientData, completionDelegate, onParticipantUpdatedCallbackInternal, Array.Empty<Delegate>());
			ulong num = Bindings.EOS_RTCAudio_AddNotifyParticipantUpdated(base.InnerHandle, ref addNotifyParticipantUpdatedOptionsInternal, zero, onParticipantUpdatedCallbackInternal);
			Helper.Dispose<AddNotifyParticipantUpdatedOptionsInternal>(ref addNotifyParticipantUpdatedOptionsInternal);
			Helper.AssignNotificationIdToCallback(zero, num);
			return num;
		}

		// Token: 0x060010D7 RID: 4311 RVA: 0x000181E4 File Offset: 0x000163E4
		public Result CopyInputDeviceInformationByIndex(ref CopyInputDeviceInformationByIndexOptions options, out InputDeviceInformation? outInputDeviceInformation)
		{
			CopyInputDeviceInformationByIndexOptionsInternal copyInputDeviceInformationByIndexOptionsInternal = default(CopyInputDeviceInformationByIndexOptionsInternal);
			copyInputDeviceInformationByIndexOptionsInternal.Set(ref options);
			IntPtr zero = IntPtr.Zero;
			Result result = Bindings.EOS_RTCAudio_CopyInputDeviceInformationByIndex(base.InnerHandle, ref copyInputDeviceInformationByIndexOptionsInternal, ref zero);
			Helper.Dispose<CopyInputDeviceInformationByIndexOptionsInternal>(ref copyInputDeviceInformationByIndexOptionsInternal);
			Helper.Get<InputDeviceInformationInternal, InputDeviceInformation>(zero, out outInputDeviceInformation);
			bool flag = outInputDeviceInformation != null;
			if (flag)
			{
				Bindings.EOS_RTCAudio_InputDeviceInformation_Release(zero);
			}
			return result;
		}

		// Token: 0x060010D8 RID: 4312 RVA: 0x00018244 File Offset: 0x00016444
		public Result CopyOutputDeviceInformationByIndex(ref CopyOutputDeviceInformationByIndexOptions options, out OutputDeviceInformation? outOutputDeviceInformation)
		{
			CopyOutputDeviceInformationByIndexOptionsInternal copyOutputDeviceInformationByIndexOptionsInternal = default(CopyOutputDeviceInformationByIndexOptionsInternal);
			copyOutputDeviceInformationByIndexOptionsInternal.Set(ref options);
			IntPtr zero = IntPtr.Zero;
			Result result = Bindings.EOS_RTCAudio_CopyOutputDeviceInformationByIndex(base.InnerHandle, ref copyOutputDeviceInformationByIndexOptionsInternal, ref zero);
			Helper.Dispose<CopyOutputDeviceInformationByIndexOptionsInternal>(ref copyOutputDeviceInformationByIndexOptionsInternal);
			Helper.Get<OutputDeviceInformationInternal, OutputDeviceInformation>(zero, out outOutputDeviceInformation);
			bool flag = outOutputDeviceInformation != null;
			if (flag)
			{
				Bindings.EOS_RTCAudio_OutputDeviceInformation_Release(zero);
			}
			return result;
		}

		// Token: 0x060010D9 RID: 4313 RVA: 0x000182A4 File Offset: 0x000164A4
		public AudioInputDeviceInfo? GetAudioInputDeviceByIndex(ref GetAudioInputDeviceByIndexOptions options)
		{
			GetAudioInputDeviceByIndexOptionsInternal getAudioInputDeviceByIndexOptionsInternal = default(GetAudioInputDeviceByIndexOptionsInternal);
			getAudioInputDeviceByIndexOptionsInternal.Set(ref options);
			IntPtr from = Bindings.EOS_RTCAudio_GetAudioInputDeviceByIndex(base.InnerHandle, ref getAudioInputDeviceByIndexOptionsInternal);
			Helper.Dispose<GetAudioInputDeviceByIndexOptionsInternal>(ref getAudioInputDeviceByIndexOptionsInternal);
			AudioInputDeviceInfo? result;
			Helper.Get<AudioInputDeviceInfoInternal, AudioInputDeviceInfo>(from, out result);
			return result;
		}

		// Token: 0x060010DA RID: 4314 RVA: 0x000182E8 File Offset: 0x000164E8
		public uint GetAudioInputDevicesCount(ref GetAudioInputDevicesCountOptions options)
		{
			GetAudioInputDevicesCountOptionsInternal getAudioInputDevicesCountOptionsInternal = default(GetAudioInputDevicesCountOptionsInternal);
			getAudioInputDevicesCountOptionsInternal.Set(ref options);
			uint result = Bindings.EOS_RTCAudio_GetAudioInputDevicesCount(base.InnerHandle, ref getAudioInputDevicesCountOptionsInternal);
			Helper.Dispose<GetAudioInputDevicesCountOptionsInternal>(ref getAudioInputDevicesCountOptionsInternal);
			return result;
		}

		// Token: 0x060010DB RID: 4315 RVA: 0x00018324 File Offset: 0x00016524
		public AudioOutputDeviceInfo? GetAudioOutputDeviceByIndex(ref GetAudioOutputDeviceByIndexOptions options)
		{
			GetAudioOutputDeviceByIndexOptionsInternal getAudioOutputDeviceByIndexOptionsInternal = default(GetAudioOutputDeviceByIndexOptionsInternal);
			getAudioOutputDeviceByIndexOptionsInternal.Set(ref options);
			IntPtr from = Bindings.EOS_RTCAudio_GetAudioOutputDeviceByIndex(base.InnerHandle, ref getAudioOutputDeviceByIndexOptionsInternal);
			Helper.Dispose<GetAudioOutputDeviceByIndexOptionsInternal>(ref getAudioOutputDeviceByIndexOptionsInternal);
			AudioOutputDeviceInfo? result;
			Helper.Get<AudioOutputDeviceInfoInternal, AudioOutputDeviceInfo>(from, out result);
			return result;
		}

		// Token: 0x060010DC RID: 4316 RVA: 0x00018368 File Offset: 0x00016568
		public uint GetAudioOutputDevicesCount(ref GetAudioOutputDevicesCountOptions options)
		{
			GetAudioOutputDevicesCountOptionsInternal getAudioOutputDevicesCountOptionsInternal = default(GetAudioOutputDevicesCountOptionsInternal);
			getAudioOutputDevicesCountOptionsInternal.Set(ref options);
			uint result = Bindings.EOS_RTCAudio_GetAudioOutputDevicesCount(base.InnerHandle, ref getAudioOutputDevicesCountOptionsInternal);
			Helper.Dispose<GetAudioOutputDevicesCountOptionsInternal>(ref getAudioOutputDevicesCountOptionsInternal);
			return result;
		}

		// Token: 0x060010DD RID: 4317 RVA: 0x000183A4 File Offset: 0x000165A4
		public uint GetInputDevicesCount(ref GetInputDevicesCountOptions options)
		{
			GetInputDevicesCountOptionsInternal getInputDevicesCountOptionsInternal = default(GetInputDevicesCountOptionsInternal);
			getInputDevicesCountOptionsInternal.Set(ref options);
			uint result = Bindings.EOS_RTCAudio_GetInputDevicesCount(base.InnerHandle, ref getInputDevicesCountOptionsInternal);
			Helper.Dispose<GetInputDevicesCountOptionsInternal>(ref getInputDevicesCountOptionsInternal);
			return result;
		}

		// Token: 0x060010DE RID: 4318 RVA: 0x000183E0 File Offset: 0x000165E0
		public uint GetOutputDevicesCount(ref GetOutputDevicesCountOptions options)
		{
			GetOutputDevicesCountOptionsInternal getOutputDevicesCountOptionsInternal = default(GetOutputDevicesCountOptionsInternal);
			getOutputDevicesCountOptionsInternal.Set(ref options);
			uint result = Bindings.EOS_RTCAudio_GetOutputDevicesCount(base.InnerHandle, ref getOutputDevicesCountOptionsInternal);
			Helper.Dispose<GetOutputDevicesCountOptionsInternal>(ref getOutputDevicesCountOptionsInternal);
			return result;
		}

		// Token: 0x060010DF RID: 4319 RVA: 0x0001841C File Offset: 0x0001661C
		public void QueryInputDevicesInformation(ref QueryInputDevicesInformationOptions options, object clientData, OnQueryInputDevicesInformationCallback completionDelegate)
		{
			QueryInputDevicesInformationOptionsInternal queryInputDevicesInformationOptionsInternal = default(QueryInputDevicesInformationOptionsInternal);
			queryInputDevicesInformationOptionsInternal.Set(ref options);
			IntPtr zero = IntPtr.Zero;
			OnQueryInputDevicesInformationCallbackInternal onQueryInputDevicesInformationCallbackInternal = new OnQueryInputDevicesInformationCallbackInternal(RTCAudioInterface.OnQueryInputDevicesInformationCallbackInternalImplementation);
			Helper.AddCallback(out zero, clientData, completionDelegate, onQueryInputDevicesInformationCallbackInternal, Array.Empty<Delegate>());
			Bindings.EOS_RTCAudio_QueryInputDevicesInformation(base.InnerHandle, ref queryInputDevicesInformationOptionsInternal, zero, onQueryInputDevicesInformationCallbackInternal);
			Helper.Dispose<QueryInputDevicesInformationOptionsInternal>(ref queryInputDevicesInformationOptionsInternal);
		}

		// Token: 0x060010E0 RID: 4320 RVA: 0x00018478 File Offset: 0x00016678
		public void QueryOutputDevicesInformation(ref QueryOutputDevicesInformationOptions options, object clientData, OnQueryOutputDevicesInformationCallback completionDelegate)
		{
			QueryOutputDevicesInformationOptionsInternal queryOutputDevicesInformationOptionsInternal = default(QueryOutputDevicesInformationOptionsInternal);
			queryOutputDevicesInformationOptionsInternal.Set(ref options);
			IntPtr zero = IntPtr.Zero;
			OnQueryOutputDevicesInformationCallbackInternal onQueryOutputDevicesInformationCallbackInternal = new OnQueryOutputDevicesInformationCallbackInternal(RTCAudioInterface.OnQueryOutputDevicesInformationCallbackInternalImplementation);
			Helper.AddCallback(out zero, clientData, completionDelegate, onQueryOutputDevicesInformationCallbackInternal, Array.Empty<Delegate>());
			Bindings.EOS_RTCAudio_QueryOutputDevicesInformation(base.InnerHandle, ref queryOutputDevicesInformationOptionsInternal, zero, onQueryOutputDevicesInformationCallbackInternal);
			Helper.Dispose<QueryOutputDevicesInformationOptionsInternal>(ref queryOutputDevicesInformationOptionsInternal);
		}

		// Token: 0x060010E1 RID: 4321 RVA: 0x000184D4 File Offset: 0x000166D4
		public Result RegisterPlatformAudioUser(ref RegisterPlatformAudioUserOptions options)
		{
			RegisterPlatformAudioUserOptionsInternal registerPlatformAudioUserOptionsInternal = default(RegisterPlatformAudioUserOptionsInternal);
			registerPlatformAudioUserOptionsInternal.Set(ref options);
			Result result = Bindings.EOS_RTCAudio_RegisterPlatformAudioUser(base.InnerHandle, ref registerPlatformAudioUserOptionsInternal);
			Helper.Dispose<RegisterPlatformAudioUserOptionsInternal>(ref registerPlatformAudioUserOptionsInternal);
			return result;
		}

		// Token: 0x060010E2 RID: 4322 RVA: 0x00018510 File Offset: 0x00016710
		public void RegisterPlatformUser(ref RegisterPlatformUserOptions options, object clientData, OnRegisterPlatformUserCallback completionDelegate)
		{
			RegisterPlatformUserOptionsInternal registerPlatformUserOptionsInternal = default(RegisterPlatformUserOptionsInternal);
			registerPlatformUserOptionsInternal.Set(ref options);
			IntPtr zero = IntPtr.Zero;
			OnRegisterPlatformUserCallbackInternal onRegisterPlatformUserCallbackInternal = new OnRegisterPlatformUserCallbackInternal(RTCAudioInterface.OnRegisterPlatformUserCallbackInternalImplementation);
			Helper.AddCallback(out zero, clientData, completionDelegate, onRegisterPlatformUserCallbackInternal, Array.Empty<Delegate>());
			Bindings.EOS_RTCAudio_RegisterPlatformUser(base.InnerHandle, ref registerPlatformUserOptionsInternal, zero, onRegisterPlatformUserCallbackInternal);
			Helper.Dispose<RegisterPlatformUserOptionsInternal>(ref registerPlatformUserOptionsInternal);
		}

		// Token: 0x060010E3 RID: 4323 RVA: 0x0001856A File Offset: 0x0001676A
		public void RemoveNotifyAudioBeforeRender(ulong notificationId)
		{
			Bindings.EOS_RTCAudio_RemoveNotifyAudioBeforeRender(base.InnerHandle, notificationId);
			Helper.RemoveCallbackByNotificationId(notificationId);
		}

		// Token: 0x060010E4 RID: 4324 RVA: 0x00018581 File Offset: 0x00016781
		public void RemoveNotifyAudioBeforeSend(ulong notificationId)
		{
			Bindings.EOS_RTCAudio_RemoveNotifyAudioBeforeSend(base.InnerHandle, notificationId);
			Helper.RemoveCallbackByNotificationId(notificationId);
		}

		// Token: 0x060010E5 RID: 4325 RVA: 0x00018598 File Offset: 0x00016798
		public void RemoveNotifyAudioDevicesChanged(ulong notificationId)
		{
			Bindings.EOS_RTCAudio_RemoveNotifyAudioDevicesChanged(base.InnerHandle, notificationId);
			Helper.RemoveCallbackByNotificationId(notificationId);
		}

		// Token: 0x060010E6 RID: 4326 RVA: 0x000185AF File Offset: 0x000167AF
		public void RemoveNotifyAudioInputState(ulong notificationId)
		{
			Bindings.EOS_RTCAudio_RemoveNotifyAudioInputState(base.InnerHandle, notificationId);
			Helper.RemoveCallbackByNotificationId(notificationId);
		}

		// Token: 0x060010E7 RID: 4327 RVA: 0x000185C6 File Offset: 0x000167C6
		public void RemoveNotifyAudioOutputState(ulong notificationId)
		{
			Bindings.EOS_RTCAudio_RemoveNotifyAudioOutputState(base.InnerHandle, notificationId);
			Helper.RemoveCallbackByNotificationId(notificationId);
		}

		// Token: 0x060010E8 RID: 4328 RVA: 0x000185DD File Offset: 0x000167DD
		public void RemoveNotifyParticipantUpdated(ulong notificationId)
		{
			Bindings.EOS_RTCAudio_RemoveNotifyParticipantUpdated(base.InnerHandle, notificationId);
			Helper.RemoveCallbackByNotificationId(notificationId);
		}

		// Token: 0x060010E9 RID: 4329 RVA: 0x000185F4 File Offset: 0x000167F4
		public Result SendAudio(ref SendAudioOptions options)
		{
			SendAudioOptionsInternal sendAudioOptionsInternal = default(SendAudioOptionsInternal);
			sendAudioOptionsInternal.Set(ref options);
			Result result = Bindings.EOS_RTCAudio_SendAudio(base.InnerHandle, ref sendAudioOptionsInternal);
			Helper.Dispose<SendAudioOptionsInternal>(ref sendAudioOptionsInternal);
			return result;
		}

		// Token: 0x060010EA RID: 4330 RVA: 0x00018630 File Offset: 0x00016830
		public Result SetAudioInputSettings(ref SetAudioInputSettingsOptions options)
		{
			SetAudioInputSettingsOptionsInternal setAudioInputSettingsOptionsInternal = default(SetAudioInputSettingsOptionsInternal);
			setAudioInputSettingsOptionsInternal.Set(ref options);
			Result result = Bindings.EOS_RTCAudio_SetAudioInputSettings(base.InnerHandle, ref setAudioInputSettingsOptionsInternal);
			Helper.Dispose<SetAudioInputSettingsOptionsInternal>(ref setAudioInputSettingsOptionsInternal);
			return result;
		}

		// Token: 0x060010EB RID: 4331 RVA: 0x0001866C File Offset: 0x0001686C
		public Result SetAudioOutputSettings(ref SetAudioOutputSettingsOptions options)
		{
			SetAudioOutputSettingsOptionsInternal setAudioOutputSettingsOptionsInternal = default(SetAudioOutputSettingsOptionsInternal);
			setAudioOutputSettingsOptionsInternal.Set(ref options);
			Result result = Bindings.EOS_RTCAudio_SetAudioOutputSettings(base.InnerHandle, ref setAudioOutputSettingsOptionsInternal);
			Helper.Dispose<SetAudioOutputSettingsOptionsInternal>(ref setAudioOutputSettingsOptionsInternal);
			return result;
		}

		// Token: 0x060010EC RID: 4332 RVA: 0x000186A8 File Offset: 0x000168A8
		public void SetInputDeviceSettings(ref SetInputDeviceSettingsOptions options, object clientData, OnSetInputDeviceSettingsCallback completionDelegate)
		{
			SetInputDeviceSettingsOptionsInternal setInputDeviceSettingsOptionsInternal = default(SetInputDeviceSettingsOptionsInternal);
			setInputDeviceSettingsOptionsInternal.Set(ref options);
			IntPtr zero = IntPtr.Zero;
			OnSetInputDeviceSettingsCallbackInternal onSetInputDeviceSettingsCallbackInternal = new OnSetInputDeviceSettingsCallbackInternal(RTCAudioInterface.OnSetInputDeviceSettingsCallbackInternalImplementation);
			Helper.AddCallback(out zero, clientData, completionDelegate, onSetInputDeviceSettingsCallbackInternal, Array.Empty<Delegate>());
			Bindings.EOS_RTCAudio_SetInputDeviceSettings(base.InnerHandle, ref setInputDeviceSettingsOptionsInternal, zero, onSetInputDeviceSettingsCallbackInternal);
			Helper.Dispose<SetInputDeviceSettingsOptionsInternal>(ref setInputDeviceSettingsOptionsInternal);
		}

		// Token: 0x060010ED RID: 4333 RVA: 0x00018704 File Offset: 0x00016904
		public void SetOutputDeviceSettings(ref SetOutputDeviceSettingsOptions options, object clientData, OnSetOutputDeviceSettingsCallback completionDelegate)
		{
			SetOutputDeviceSettingsOptionsInternal setOutputDeviceSettingsOptionsInternal = default(SetOutputDeviceSettingsOptionsInternal);
			setOutputDeviceSettingsOptionsInternal.Set(ref options);
			IntPtr zero = IntPtr.Zero;
			OnSetOutputDeviceSettingsCallbackInternal onSetOutputDeviceSettingsCallbackInternal = new OnSetOutputDeviceSettingsCallbackInternal(RTCAudioInterface.OnSetOutputDeviceSettingsCallbackInternalImplementation);
			Helper.AddCallback(out zero, clientData, completionDelegate, onSetOutputDeviceSettingsCallbackInternal, Array.Empty<Delegate>());
			Bindings.EOS_RTCAudio_SetOutputDeviceSettings(base.InnerHandle, ref setOutputDeviceSettingsOptionsInternal, zero, onSetOutputDeviceSettingsCallbackInternal);
			Helper.Dispose<SetOutputDeviceSettingsOptionsInternal>(ref setOutputDeviceSettingsOptionsInternal);
		}

		// Token: 0x060010EE RID: 4334 RVA: 0x00018760 File Offset: 0x00016960
		public Result UnregisterPlatformAudioUser(ref UnregisterPlatformAudioUserOptions options)
		{
			UnregisterPlatformAudioUserOptionsInternal unregisterPlatformAudioUserOptionsInternal = default(UnregisterPlatformAudioUserOptionsInternal);
			unregisterPlatformAudioUserOptionsInternal.Set(ref options);
			Result result = Bindings.EOS_RTCAudio_UnregisterPlatformAudioUser(base.InnerHandle, ref unregisterPlatformAudioUserOptionsInternal);
			Helper.Dispose<UnregisterPlatformAudioUserOptionsInternal>(ref unregisterPlatformAudioUserOptionsInternal);
			return result;
		}

		// Token: 0x060010EF RID: 4335 RVA: 0x0001879C File Offset: 0x0001699C
		public void UnregisterPlatformUser(ref UnregisterPlatformUserOptions options, object clientData, OnUnregisterPlatformUserCallback completionDelegate)
		{
			UnregisterPlatformUserOptionsInternal unregisterPlatformUserOptionsInternal = default(UnregisterPlatformUserOptionsInternal);
			unregisterPlatformUserOptionsInternal.Set(ref options);
			IntPtr zero = IntPtr.Zero;
			OnUnregisterPlatformUserCallbackInternal onUnregisterPlatformUserCallbackInternal = new OnUnregisterPlatformUserCallbackInternal(RTCAudioInterface.OnUnregisterPlatformUserCallbackInternalImplementation);
			Helper.AddCallback(out zero, clientData, completionDelegate, onUnregisterPlatformUserCallbackInternal, Array.Empty<Delegate>());
			Bindings.EOS_RTCAudio_UnregisterPlatformUser(base.InnerHandle, ref unregisterPlatformUserOptionsInternal, zero, onUnregisterPlatformUserCallbackInternal);
			Helper.Dispose<UnregisterPlatformUserOptionsInternal>(ref unregisterPlatformUserOptionsInternal);
		}

		// Token: 0x060010F0 RID: 4336 RVA: 0x000187F8 File Offset: 0x000169F8
		public void UpdateParticipantVolume(ref UpdateParticipantVolumeOptions options, object clientData, OnUpdateParticipantVolumeCallback completionDelegate)
		{
			UpdateParticipantVolumeOptionsInternal updateParticipantVolumeOptionsInternal = default(UpdateParticipantVolumeOptionsInternal);
			updateParticipantVolumeOptionsInternal.Set(ref options);
			IntPtr zero = IntPtr.Zero;
			OnUpdateParticipantVolumeCallbackInternal onUpdateParticipantVolumeCallbackInternal = new OnUpdateParticipantVolumeCallbackInternal(RTCAudioInterface.OnUpdateParticipantVolumeCallbackInternalImplementation);
			Helper.AddCallback(out zero, clientData, completionDelegate, onUpdateParticipantVolumeCallbackInternal, Array.Empty<Delegate>());
			Bindings.EOS_RTCAudio_UpdateParticipantVolume(base.InnerHandle, ref updateParticipantVolumeOptionsInternal, zero, onUpdateParticipantVolumeCallbackInternal);
			Helper.Dispose<UpdateParticipantVolumeOptionsInternal>(ref updateParticipantVolumeOptionsInternal);
		}

		// Token: 0x060010F1 RID: 4337 RVA: 0x00018854 File Offset: 0x00016A54
		public void UpdateReceiving(ref UpdateReceivingOptions options, object clientData, OnUpdateReceivingCallback completionDelegate)
		{
			UpdateReceivingOptionsInternal updateReceivingOptionsInternal = default(UpdateReceivingOptionsInternal);
			updateReceivingOptionsInternal.Set(ref options);
			IntPtr zero = IntPtr.Zero;
			OnUpdateReceivingCallbackInternal onUpdateReceivingCallbackInternal = new OnUpdateReceivingCallbackInternal(RTCAudioInterface.OnUpdateReceivingCallbackInternalImplementation);
			Helper.AddCallback(out zero, clientData, completionDelegate, onUpdateReceivingCallbackInternal, Array.Empty<Delegate>());
			Bindings.EOS_RTCAudio_UpdateReceiving(base.InnerHandle, ref updateReceivingOptionsInternal, zero, onUpdateReceivingCallbackInternal);
			Helper.Dispose<UpdateReceivingOptionsInternal>(ref updateReceivingOptionsInternal);
		}

		// Token: 0x060010F2 RID: 4338 RVA: 0x000188B0 File Offset: 0x00016AB0
		public void UpdateReceivingVolume(ref UpdateReceivingVolumeOptions options, object clientData, OnUpdateReceivingVolumeCallback completionDelegate)
		{
			UpdateReceivingVolumeOptionsInternal updateReceivingVolumeOptionsInternal = default(UpdateReceivingVolumeOptionsInternal);
			updateReceivingVolumeOptionsInternal.Set(ref options);
			IntPtr zero = IntPtr.Zero;
			OnUpdateReceivingVolumeCallbackInternal onUpdateReceivingVolumeCallbackInternal = new OnUpdateReceivingVolumeCallbackInternal(RTCAudioInterface.OnUpdateReceivingVolumeCallbackInternalImplementation);
			Helper.AddCallback(out zero, clientData, completionDelegate, onUpdateReceivingVolumeCallbackInternal, Array.Empty<Delegate>());
			Bindings.EOS_RTCAudio_UpdateReceivingVolume(base.InnerHandle, ref updateReceivingVolumeOptionsInternal, zero, onUpdateReceivingVolumeCallbackInternal);
			Helper.Dispose<UpdateReceivingVolumeOptionsInternal>(ref updateReceivingVolumeOptionsInternal);
		}

		// Token: 0x060010F3 RID: 4339 RVA: 0x0001890C File Offset: 0x00016B0C
		public void UpdateSending(ref UpdateSendingOptions options, object clientData, OnUpdateSendingCallback completionDelegate)
		{
			UpdateSendingOptionsInternal updateSendingOptionsInternal = default(UpdateSendingOptionsInternal);
			updateSendingOptionsInternal.Set(ref options);
			IntPtr zero = IntPtr.Zero;
			OnUpdateSendingCallbackInternal onUpdateSendingCallbackInternal = new OnUpdateSendingCallbackInternal(RTCAudioInterface.OnUpdateSendingCallbackInternalImplementation);
			Helper.AddCallback(out zero, clientData, completionDelegate, onUpdateSendingCallbackInternal, Array.Empty<Delegate>());
			Bindings.EOS_RTCAudio_UpdateSending(base.InnerHandle, ref updateSendingOptionsInternal, zero, onUpdateSendingCallbackInternal);
			Helper.Dispose<UpdateSendingOptionsInternal>(ref updateSendingOptionsInternal);
		}

		// Token: 0x060010F4 RID: 4340 RVA: 0x00018968 File Offset: 0x00016B68
		public void UpdateSendingVolume(ref UpdateSendingVolumeOptions options, object clientData, OnUpdateSendingVolumeCallback completionDelegate)
		{
			UpdateSendingVolumeOptionsInternal updateSendingVolumeOptionsInternal = default(UpdateSendingVolumeOptionsInternal);
			updateSendingVolumeOptionsInternal.Set(ref options);
			IntPtr zero = IntPtr.Zero;
			OnUpdateSendingVolumeCallbackInternal onUpdateSendingVolumeCallbackInternal = new OnUpdateSendingVolumeCallbackInternal(RTCAudioInterface.OnUpdateSendingVolumeCallbackInternalImplementation);
			Helper.AddCallback(out zero, clientData, completionDelegate, onUpdateSendingVolumeCallbackInternal, Array.Empty<Delegate>());
			Bindings.EOS_RTCAudio_UpdateSendingVolume(base.InnerHandle, ref updateSendingVolumeOptionsInternal, zero, onUpdateSendingVolumeCallbackInternal);
			Helper.Dispose<UpdateSendingVolumeOptionsInternal>(ref updateSendingVolumeOptionsInternal);
		}

		// Token: 0x060010F5 RID: 4341 RVA: 0x000189C4 File Offset: 0x00016BC4
		[MonoPInvokeCallback(typeof(OnAudioBeforeRenderCallbackInternal))]
		internal static void OnAudioBeforeRenderCallbackInternalImplementation(ref AudioBeforeRenderCallbackInfoInternal data)
		{
			OnAudioBeforeRenderCallback onAudioBeforeRenderCallback;
			AudioBeforeRenderCallbackInfo audioBeforeRenderCallbackInfo;
			bool flag = Helper.TryGetCallback<AudioBeforeRenderCallbackInfoInternal, OnAudioBeforeRenderCallback, AudioBeforeRenderCallbackInfo>(ref data, out onAudioBeforeRenderCallback, out audioBeforeRenderCallbackInfo);
			if (flag)
			{
				onAudioBeforeRenderCallback(ref audioBeforeRenderCallbackInfo);
			}
		}

		// Token: 0x060010F6 RID: 4342 RVA: 0x000189EC File Offset: 0x00016BEC
		[MonoPInvokeCallback(typeof(OnAudioBeforeSendCallbackInternal))]
		internal static void OnAudioBeforeSendCallbackInternalImplementation(ref AudioBeforeSendCallbackInfoInternal data)
		{
			OnAudioBeforeSendCallback onAudioBeforeSendCallback;
			AudioBeforeSendCallbackInfo audioBeforeSendCallbackInfo;
			bool flag = Helper.TryGetCallback<AudioBeforeSendCallbackInfoInternal, OnAudioBeforeSendCallback, AudioBeforeSendCallbackInfo>(ref data, out onAudioBeforeSendCallback, out audioBeforeSendCallbackInfo);
			if (flag)
			{
				onAudioBeforeSendCallback(ref audioBeforeSendCallbackInfo);
			}
		}

		// Token: 0x060010F7 RID: 4343 RVA: 0x00018A14 File Offset: 0x00016C14
		[MonoPInvokeCallback(typeof(OnAudioDevicesChangedCallbackInternal))]
		internal static void OnAudioDevicesChangedCallbackInternalImplementation(ref AudioDevicesChangedCallbackInfoInternal data)
		{
			OnAudioDevicesChangedCallback onAudioDevicesChangedCallback;
			AudioDevicesChangedCallbackInfo audioDevicesChangedCallbackInfo;
			bool flag = Helper.TryGetCallback<AudioDevicesChangedCallbackInfoInternal, OnAudioDevicesChangedCallback, AudioDevicesChangedCallbackInfo>(ref data, out onAudioDevicesChangedCallback, out audioDevicesChangedCallbackInfo);
			if (flag)
			{
				onAudioDevicesChangedCallback(ref audioDevicesChangedCallbackInfo);
			}
		}

		// Token: 0x060010F8 RID: 4344 RVA: 0x00018A3C File Offset: 0x00016C3C
		[MonoPInvokeCallback(typeof(OnAudioInputStateCallbackInternal))]
		internal static void OnAudioInputStateCallbackInternalImplementation(ref AudioInputStateCallbackInfoInternal data)
		{
			OnAudioInputStateCallback onAudioInputStateCallback;
			AudioInputStateCallbackInfo audioInputStateCallbackInfo;
			bool flag = Helper.TryGetCallback<AudioInputStateCallbackInfoInternal, OnAudioInputStateCallback, AudioInputStateCallbackInfo>(ref data, out onAudioInputStateCallback, out audioInputStateCallbackInfo);
			if (flag)
			{
				onAudioInputStateCallback(ref audioInputStateCallbackInfo);
			}
		}

		// Token: 0x060010F9 RID: 4345 RVA: 0x00018A64 File Offset: 0x00016C64
		[MonoPInvokeCallback(typeof(OnAudioOutputStateCallbackInternal))]
		internal static void OnAudioOutputStateCallbackInternalImplementation(ref AudioOutputStateCallbackInfoInternal data)
		{
			OnAudioOutputStateCallback onAudioOutputStateCallback;
			AudioOutputStateCallbackInfo audioOutputStateCallbackInfo;
			bool flag = Helper.TryGetCallback<AudioOutputStateCallbackInfoInternal, OnAudioOutputStateCallback, AudioOutputStateCallbackInfo>(ref data, out onAudioOutputStateCallback, out audioOutputStateCallbackInfo);
			if (flag)
			{
				onAudioOutputStateCallback(ref audioOutputStateCallbackInfo);
			}
		}

		// Token: 0x060010FA RID: 4346 RVA: 0x00018A8C File Offset: 0x00016C8C
		[MonoPInvokeCallback(typeof(OnParticipantUpdatedCallbackInternal))]
		internal static void OnParticipantUpdatedCallbackInternalImplementation(ref ParticipantUpdatedCallbackInfoInternal data)
		{
			OnParticipantUpdatedCallback onParticipantUpdatedCallback;
			ParticipantUpdatedCallbackInfo participantUpdatedCallbackInfo;
			bool flag = Helper.TryGetCallback<ParticipantUpdatedCallbackInfoInternal, OnParticipantUpdatedCallback, ParticipantUpdatedCallbackInfo>(ref data, out onParticipantUpdatedCallback, out participantUpdatedCallbackInfo);
			if (flag)
			{
				onParticipantUpdatedCallback(ref participantUpdatedCallbackInfo);
			}
		}

		// Token: 0x060010FB RID: 4347 RVA: 0x00018AB4 File Offset: 0x00016CB4
		[MonoPInvokeCallback(typeof(OnQueryInputDevicesInformationCallbackInternal))]
		internal static void OnQueryInputDevicesInformationCallbackInternalImplementation(ref OnQueryInputDevicesInformationCallbackInfoInternal data)
		{
			OnQueryInputDevicesInformationCallback onQueryInputDevicesInformationCallback;
			OnQueryInputDevicesInformationCallbackInfo onQueryInputDevicesInformationCallbackInfo;
			bool flag = Helper.TryGetAndRemoveCallback<OnQueryInputDevicesInformationCallbackInfoInternal, OnQueryInputDevicesInformationCallback, OnQueryInputDevicesInformationCallbackInfo>(ref data, out onQueryInputDevicesInformationCallback, out onQueryInputDevicesInformationCallbackInfo);
			if (flag)
			{
				onQueryInputDevicesInformationCallback(ref onQueryInputDevicesInformationCallbackInfo);
			}
		}

		// Token: 0x060010FC RID: 4348 RVA: 0x00018ADC File Offset: 0x00016CDC
		[MonoPInvokeCallback(typeof(OnQueryOutputDevicesInformationCallbackInternal))]
		internal static void OnQueryOutputDevicesInformationCallbackInternalImplementation(ref OnQueryOutputDevicesInformationCallbackInfoInternal data)
		{
			OnQueryOutputDevicesInformationCallback onQueryOutputDevicesInformationCallback;
			OnQueryOutputDevicesInformationCallbackInfo onQueryOutputDevicesInformationCallbackInfo;
			bool flag = Helper.TryGetAndRemoveCallback<OnQueryOutputDevicesInformationCallbackInfoInternal, OnQueryOutputDevicesInformationCallback, OnQueryOutputDevicesInformationCallbackInfo>(ref data, out onQueryOutputDevicesInformationCallback, out onQueryOutputDevicesInformationCallbackInfo);
			if (flag)
			{
				onQueryOutputDevicesInformationCallback(ref onQueryOutputDevicesInformationCallbackInfo);
			}
		}

		// Token: 0x060010FD RID: 4349 RVA: 0x00018B04 File Offset: 0x00016D04
		[MonoPInvokeCallback(typeof(OnRegisterPlatformUserCallbackInternal))]
		internal static void OnRegisterPlatformUserCallbackInternalImplementation(ref OnRegisterPlatformUserCallbackInfoInternal data)
		{
			OnRegisterPlatformUserCallback onRegisterPlatformUserCallback;
			OnRegisterPlatformUserCallbackInfo onRegisterPlatformUserCallbackInfo;
			bool flag = Helper.TryGetAndRemoveCallback<OnRegisterPlatformUserCallbackInfoInternal, OnRegisterPlatformUserCallback, OnRegisterPlatformUserCallbackInfo>(ref data, out onRegisterPlatformUserCallback, out onRegisterPlatformUserCallbackInfo);
			if (flag)
			{
				onRegisterPlatformUserCallback(ref onRegisterPlatformUserCallbackInfo);
			}
		}

		// Token: 0x060010FE RID: 4350 RVA: 0x00018B2C File Offset: 0x00016D2C
		[MonoPInvokeCallback(typeof(OnSetInputDeviceSettingsCallbackInternal))]
		internal static void OnSetInputDeviceSettingsCallbackInternalImplementation(ref OnSetInputDeviceSettingsCallbackInfoInternal data)
		{
			OnSetInputDeviceSettingsCallback onSetInputDeviceSettingsCallback;
			OnSetInputDeviceSettingsCallbackInfo onSetInputDeviceSettingsCallbackInfo;
			bool flag = Helper.TryGetAndRemoveCallback<OnSetInputDeviceSettingsCallbackInfoInternal, OnSetInputDeviceSettingsCallback, OnSetInputDeviceSettingsCallbackInfo>(ref data, out onSetInputDeviceSettingsCallback, out onSetInputDeviceSettingsCallbackInfo);
			if (flag)
			{
				onSetInputDeviceSettingsCallback(ref onSetInputDeviceSettingsCallbackInfo);
			}
		}

		// Token: 0x060010FF RID: 4351 RVA: 0x00018B54 File Offset: 0x00016D54
		[MonoPInvokeCallback(typeof(OnSetOutputDeviceSettingsCallbackInternal))]
		internal static void OnSetOutputDeviceSettingsCallbackInternalImplementation(ref OnSetOutputDeviceSettingsCallbackInfoInternal data)
		{
			OnSetOutputDeviceSettingsCallback onSetOutputDeviceSettingsCallback;
			OnSetOutputDeviceSettingsCallbackInfo onSetOutputDeviceSettingsCallbackInfo;
			bool flag = Helper.TryGetAndRemoveCallback<OnSetOutputDeviceSettingsCallbackInfoInternal, OnSetOutputDeviceSettingsCallback, OnSetOutputDeviceSettingsCallbackInfo>(ref data, out onSetOutputDeviceSettingsCallback, out onSetOutputDeviceSettingsCallbackInfo);
			if (flag)
			{
				onSetOutputDeviceSettingsCallback(ref onSetOutputDeviceSettingsCallbackInfo);
			}
		}

		// Token: 0x06001100 RID: 4352 RVA: 0x00018B7C File Offset: 0x00016D7C
		[MonoPInvokeCallback(typeof(OnUnregisterPlatformUserCallbackInternal))]
		internal static void OnUnregisterPlatformUserCallbackInternalImplementation(ref OnUnregisterPlatformUserCallbackInfoInternal data)
		{
			OnUnregisterPlatformUserCallback onUnregisterPlatformUserCallback;
			OnUnregisterPlatformUserCallbackInfo onUnregisterPlatformUserCallbackInfo;
			bool flag = Helper.TryGetAndRemoveCallback<OnUnregisterPlatformUserCallbackInfoInternal, OnUnregisterPlatformUserCallback, OnUnregisterPlatformUserCallbackInfo>(ref data, out onUnregisterPlatformUserCallback, out onUnregisterPlatformUserCallbackInfo);
			if (flag)
			{
				onUnregisterPlatformUserCallback(ref onUnregisterPlatformUserCallbackInfo);
			}
		}

		// Token: 0x06001101 RID: 4353 RVA: 0x00018BA4 File Offset: 0x00016DA4
		[MonoPInvokeCallback(typeof(OnUpdateParticipantVolumeCallbackInternal))]
		internal static void OnUpdateParticipantVolumeCallbackInternalImplementation(ref UpdateParticipantVolumeCallbackInfoInternal data)
		{
			OnUpdateParticipantVolumeCallback onUpdateParticipantVolumeCallback;
			UpdateParticipantVolumeCallbackInfo updateParticipantVolumeCallbackInfo;
			bool flag = Helper.TryGetAndRemoveCallback<UpdateParticipantVolumeCallbackInfoInternal, OnUpdateParticipantVolumeCallback, UpdateParticipantVolumeCallbackInfo>(ref data, out onUpdateParticipantVolumeCallback, out updateParticipantVolumeCallbackInfo);
			if (flag)
			{
				onUpdateParticipantVolumeCallback(ref updateParticipantVolumeCallbackInfo);
			}
		}

		// Token: 0x06001102 RID: 4354 RVA: 0x00018BCC File Offset: 0x00016DCC
		[MonoPInvokeCallback(typeof(OnUpdateReceivingCallbackInternal))]
		internal static void OnUpdateReceivingCallbackInternalImplementation(ref UpdateReceivingCallbackInfoInternal data)
		{
			OnUpdateReceivingCallback onUpdateReceivingCallback;
			UpdateReceivingCallbackInfo updateReceivingCallbackInfo;
			bool flag = Helper.TryGetAndRemoveCallback<UpdateReceivingCallbackInfoInternal, OnUpdateReceivingCallback, UpdateReceivingCallbackInfo>(ref data, out onUpdateReceivingCallback, out updateReceivingCallbackInfo);
			if (flag)
			{
				onUpdateReceivingCallback(ref updateReceivingCallbackInfo);
			}
		}

		// Token: 0x06001103 RID: 4355 RVA: 0x00018BF4 File Offset: 0x00016DF4
		[MonoPInvokeCallback(typeof(OnUpdateReceivingVolumeCallbackInternal))]
		internal static void OnUpdateReceivingVolumeCallbackInternalImplementation(ref UpdateReceivingVolumeCallbackInfoInternal data)
		{
			OnUpdateReceivingVolumeCallback onUpdateReceivingVolumeCallback;
			UpdateReceivingVolumeCallbackInfo updateReceivingVolumeCallbackInfo;
			bool flag = Helper.TryGetAndRemoveCallback<UpdateReceivingVolumeCallbackInfoInternal, OnUpdateReceivingVolumeCallback, UpdateReceivingVolumeCallbackInfo>(ref data, out onUpdateReceivingVolumeCallback, out updateReceivingVolumeCallbackInfo);
			if (flag)
			{
				onUpdateReceivingVolumeCallback(ref updateReceivingVolumeCallbackInfo);
			}
		}

		// Token: 0x06001104 RID: 4356 RVA: 0x00018C1C File Offset: 0x00016E1C
		[MonoPInvokeCallback(typeof(OnUpdateSendingCallbackInternal))]
		internal static void OnUpdateSendingCallbackInternalImplementation(ref UpdateSendingCallbackInfoInternal data)
		{
			OnUpdateSendingCallback onUpdateSendingCallback;
			UpdateSendingCallbackInfo updateSendingCallbackInfo;
			bool flag = Helper.TryGetAndRemoveCallback<UpdateSendingCallbackInfoInternal, OnUpdateSendingCallback, UpdateSendingCallbackInfo>(ref data, out onUpdateSendingCallback, out updateSendingCallbackInfo);
			if (flag)
			{
				onUpdateSendingCallback(ref updateSendingCallbackInfo);
			}
		}

		// Token: 0x06001105 RID: 4357 RVA: 0x00018C44 File Offset: 0x00016E44
		[MonoPInvokeCallback(typeof(OnUpdateSendingVolumeCallbackInternal))]
		internal static void OnUpdateSendingVolumeCallbackInternalImplementation(ref UpdateSendingVolumeCallbackInfoInternal data)
		{
			OnUpdateSendingVolumeCallback onUpdateSendingVolumeCallback;
			UpdateSendingVolumeCallbackInfo updateSendingVolumeCallbackInfo;
			bool flag = Helper.TryGetAndRemoveCallback<UpdateSendingVolumeCallbackInfoInternal, OnUpdateSendingVolumeCallback, UpdateSendingVolumeCallbackInfo>(ref data, out onUpdateSendingVolumeCallback, out updateSendingVolumeCallbackInfo);
			if (flag)
			{
				onUpdateSendingVolumeCallback(ref updateSendingVolumeCallbackInfo);
			}
		}

		// Token: 0x04000745 RID: 1861
		public const int AddnotifyaudiobeforerenderApiLatest = 1;

		// Token: 0x04000746 RID: 1862
		public const int AddnotifyaudiobeforesendApiLatest = 1;

		// Token: 0x04000747 RID: 1863
		public const int AddnotifyaudiodeviceschangedApiLatest = 1;

		// Token: 0x04000748 RID: 1864
		public const int AddnotifyaudioinputstateApiLatest = 1;

		// Token: 0x04000749 RID: 1865
		public const int AddnotifyaudiooutputstateApiLatest = 1;

		// Token: 0x0400074A RID: 1866
		public const int AddnotifyparticipantupdatedApiLatest = 1;

		// Token: 0x0400074B RID: 1867
		public const int AudiobufferApiLatest = 1;

		// Token: 0x0400074C RID: 1868
		public const int AudioinputdeviceinfoApiLatest = 1;

		// Token: 0x0400074D RID: 1869
		public const int AudiooutputdeviceinfoApiLatest = 1;

		// Token: 0x0400074E RID: 1870
		public const int CopyinputdeviceinformationbyindexApiLatest = 1;

		// Token: 0x0400074F RID: 1871
		public const int CopyoutputdeviceinformationbyindexApiLatest = 1;

		// Token: 0x04000750 RID: 1872
		public const int GetaudioinputdevicebyindexApiLatest = 1;

		// Token: 0x04000751 RID: 1873
		public const int GetaudioinputdevicescountApiLatest = 1;

		// Token: 0x04000752 RID: 1874
		public const int GetaudiooutputdevicebyindexApiLatest = 1;

		// Token: 0x04000753 RID: 1875
		public const int GetaudiooutputdevicescountApiLatest = 1;

		// Token: 0x04000754 RID: 1876
		public const int GetinputdevicescountApiLatest = 1;

		// Token: 0x04000755 RID: 1877
		public const int GetoutputdevicescountApiLatest = 1;

		// Token: 0x04000756 RID: 1878
		public const int InputdeviceinformationApiLatest = 1;

		// Token: 0x04000757 RID: 1879
		public const int OutputdeviceinformationApiLatest = 1;

		// Token: 0x04000758 RID: 1880
		public const int QueryinputdevicesinformationApiLatest = 1;

		// Token: 0x04000759 RID: 1881
		public const int QueryoutputdevicesinformationApiLatest = 1;

		// Token: 0x0400075A RID: 1882
		public const int RegisterplatformaudiouserApiLatest = 1;

		// Token: 0x0400075B RID: 1883
		public const int RegisterplatformuserApiLatest = 1;

		// Token: 0x0400075C RID: 1884
		public const int SendaudioApiLatest = 1;

		// Token: 0x0400075D RID: 1885
		public const int SetaudioinputsettingsApiLatest = 1;

		// Token: 0x0400075E RID: 1886
		public const int SetaudiooutputsettingsApiLatest = 1;

		// Token: 0x0400075F RID: 1887
		public const int SetinputdevicesettingsApiLatest = 1;

		// Token: 0x04000760 RID: 1888
		public const int SetoutputdevicesettingsApiLatest = 1;

		// Token: 0x04000761 RID: 1889
		public const int UnregisterplatformaudiouserApiLatest = 1;

		// Token: 0x04000762 RID: 1890
		public const int UnregisterplatformuserApiLatest = 1;

		// Token: 0x04000763 RID: 1891
		public const int UpdateparticipantvolumeApiLatest = 1;

		// Token: 0x04000764 RID: 1892
		public const int UpdatereceivingApiLatest = 1;

		// Token: 0x04000765 RID: 1893
		public const int UpdatereceivingvolumeApiLatest = 1;

		// Token: 0x04000766 RID: 1894
		public const int UpdatesendingApiLatest = 1;

		// Token: 0x04000767 RID: 1895
		public const int UpdatesendingvolumeApiLatest = 1;
	}
}
