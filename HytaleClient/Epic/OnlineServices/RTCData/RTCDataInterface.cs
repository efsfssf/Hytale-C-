using System;

namespace Epic.OnlineServices.RTCData
{
	// Token: 0x020001EA RID: 490
	public sealed class RTCDataInterface : Handle
	{
		// Token: 0x06000E27 RID: 3623 RVA: 0x000149D0 File Offset: 0x00012BD0
		public RTCDataInterface()
		{
		}

		// Token: 0x06000E28 RID: 3624 RVA: 0x000149DA File Offset: 0x00012BDA
		public RTCDataInterface(IntPtr innerHandle) : base(innerHandle)
		{
		}

		// Token: 0x06000E29 RID: 3625 RVA: 0x000149E8 File Offset: 0x00012BE8
		public ulong AddNotifyDataReceived(ref AddNotifyDataReceivedOptions options, object clientData, OnDataReceivedCallback completionDelegate)
		{
			AddNotifyDataReceivedOptionsInternal addNotifyDataReceivedOptionsInternal = default(AddNotifyDataReceivedOptionsInternal);
			addNotifyDataReceivedOptionsInternal.Set(ref options);
			IntPtr zero = IntPtr.Zero;
			OnDataReceivedCallbackInternal onDataReceivedCallbackInternal = new OnDataReceivedCallbackInternal(RTCDataInterface.OnDataReceivedCallbackInternalImplementation);
			Helper.AddCallback(out zero, clientData, completionDelegate, onDataReceivedCallbackInternal, Array.Empty<Delegate>());
			ulong num = Bindings.EOS_RTCData_AddNotifyDataReceived(base.InnerHandle, ref addNotifyDataReceivedOptionsInternal, zero, onDataReceivedCallbackInternal);
			Helper.Dispose<AddNotifyDataReceivedOptionsInternal>(ref addNotifyDataReceivedOptionsInternal);
			Helper.AssignNotificationIdToCallback(zero, num);
			return num;
		}

		// Token: 0x06000E2A RID: 3626 RVA: 0x00014A54 File Offset: 0x00012C54
		public ulong AddNotifyParticipantUpdated(ref AddNotifyParticipantUpdatedOptions options, object clientData, OnParticipantUpdatedCallback completionDelegate)
		{
			AddNotifyParticipantUpdatedOptionsInternal addNotifyParticipantUpdatedOptionsInternal = default(AddNotifyParticipantUpdatedOptionsInternal);
			addNotifyParticipantUpdatedOptionsInternal.Set(ref options);
			IntPtr zero = IntPtr.Zero;
			OnParticipantUpdatedCallbackInternal onParticipantUpdatedCallbackInternal = new OnParticipantUpdatedCallbackInternal(RTCDataInterface.OnParticipantUpdatedCallbackInternalImplementation);
			Helper.AddCallback(out zero, clientData, completionDelegate, onParticipantUpdatedCallbackInternal, Array.Empty<Delegate>());
			ulong num = Bindings.EOS_RTCData_AddNotifyParticipantUpdated(base.InnerHandle, ref addNotifyParticipantUpdatedOptionsInternal, zero, onParticipantUpdatedCallbackInternal);
			Helper.Dispose<AddNotifyParticipantUpdatedOptionsInternal>(ref addNotifyParticipantUpdatedOptionsInternal);
			Helper.AssignNotificationIdToCallback(zero, num);
			return num;
		}

		// Token: 0x06000E2B RID: 3627 RVA: 0x00014ABD File Offset: 0x00012CBD
		public void RemoveNotifyDataReceived(ulong notificationId)
		{
			Bindings.EOS_RTCData_RemoveNotifyDataReceived(base.InnerHandle, notificationId);
			Helper.RemoveCallbackByNotificationId(notificationId);
		}

		// Token: 0x06000E2C RID: 3628 RVA: 0x00014AD4 File Offset: 0x00012CD4
		public void RemoveNotifyParticipantUpdated(ulong notificationId)
		{
			Bindings.EOS_RTCData_RemoveNotifyParticipantUpdated(base.InnerHandle, notificationId);
			Helper.RemoveCallbackByNotificationId(notificationId);
		}

		// Token: 0x06000E2D RID: 3629 RVA: 0x00014AEC File Offset: 0x00012CEC
		public Result SendData(ref SendDataOptions options)
		{
			SendDataOptionsInternal sendDataOptionsInternal = default(SendDataOptionsInternal);
			sendDataOptionsInternal.Set(ref options);
			Result result = Bindings.EOS_RTCData_SendData(base.InnerHandle, ref sendDataOptionsInternal);
			Helper.Dispose<SendDataOptionsInternal>(ref sendDataOptionsInternal);
			return result;
		}

		// Token: 0x06000E2E RID: 3630 RVA: 0x00014B28 File Offset: 0x00012D28
		public void UpdateReceiving(ref UpdateReceivingOptions options, object clientData, OnUpdateReceivingCallback completionDelegate)
		{
			UpdateReceivingOptionsInternal updateReceivingOptionsInternal = default(UpdateReceivingOptionsInternal);
			updateReceivingOptionsInternal.Set(ref options);
			IntPtr zero = IntPtr.Zero;
			OnUpdateReceivingCallbackInternal onUpdateReceivingCallbackInternal = new OnUpdateReceivingCallbackInternal(RTCDataInterface.OnUpdateReceivingCallbackInternalImplementation);
			Helper.AddCallback(out zero, clientData, completionDelegate, onUpdateReceivingCallbackInternal, Array.Empty<Delegate>());
			Bindings.EOS_RTCData_UpdateReceiving(base.InnerHandle, ref updateReceivingOptionsInternal, zero, onUpdateReceivingCallbackInternal);
			Helper.Dispose<UpdateReceivingOptionsInternal>(ref updateReceivingOptionsInternal);
		}

		// Token: 0x06000E2F RID: 3631 RVA: 0x00014B84 File Offset: 0x00012D84
		public void UpdateSending(ref UpdateSendingOptions options, object clientData, OnUpdateSendingCallback completionDelegate)
		{
			UpdateSendingOptionsInternal updateSendingOptionsInternal = default(UpdateSendingOptionsInternal);
			updateSendingOptionsInternal.Set(ref options);
			IntPtr zero = IntPtr.Zero;
			OnUpdateSendingCallbackInternal onUpdateSendingCallbackInternal = new OnUpdateSendingCallbackInternal(RTCDataInterface.OnUpdateSendingCallbackInternalImplementation);
			Helper.AddCallback(out zero, clientData, completionDelegate, onUpdateSendingCallbackInternal, Array.Empty<Delegate>());
			Bindings.EOS_RTCData_UpdateSending(base.InnerHandle, ref updateSendingOptionsInternal, zero, onUpdateSendingCallbackInternal);
			Helper.Dispose<UpdateSendingOptionsInternal>(ref updateSendingOptionsInternal);
		}

		// Token: 0x06000E30 RID: 3632 RVA: 0x00014BE0 File Offset: 0x00012DE0
		[MonoPInvokeCallback(typeof(OnDataReceivedCallbackInternal))]
		internal static void OnDataReceivedCallbackInternalImplementation(ref DataReceivedCallbackInfoInternal data)
		{
			OnDataReceivedCallback onDataReceivedCallback;
			DataReceivedCallbackInfo dataReceivedCallbackInfo;
			bool flag = Helper.TryGetCallback<DataReceivedCallbackInfoInternal, OnDataReceivedCallback, DataReceivedCallbackInfo>(ref data, out onDataReceivedCallback, out dataReceivedCallbackInfo);
			if (flag)
			{
				onDataReceivedCallback(ref dataReceivedCallbackInfo);
			}
		}

		// Token: 0x06000E31 RID: 3633 RVA: 0x00014C08 File Offset: 0x00012E08
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

		// Token: 0x06000E32 RID: 3634 RVA: 0x00014C30 File Offset: 0x00012E30
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

		// Token: 0x06000E33 RID: 3635 RVA: 0x00014C58 File Offset: 0x00012E58
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

		// Token: 0x0400065F RID: 1631
		public const int AddnotifydatareceivedApiLatest = 1;

		// Token: 0x04000660 RID: 1632
		public const int AddnotifyparticipantupdatedApiLatest = 1;

		// Token: 0x04000661 RID: 1633
		public const int MaxPacketSize = 1170;

		// Token: 0x04000662 RID: 1634
		public const int SenddataApiLatest = 1;

		// Token: 0x04000663 RID: 1635
		public const int UpdatereceivingApiLatest = 1;

		// Token: 0x04000664 RID: 1636
		public const int UpdatesendingApiLatest = 1;
	}
}
