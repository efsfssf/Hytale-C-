using System;

namespace Epic.OnlineServices.P2P
{
	// Token: 0x02000766 RID: 1894
	public sealed class P2PInterface : Handle
	{
		// Token: 0x06003140 RID: 12608 RVA: 0x0004939C File Offset: 0x0004759C
		public Result ReceivePacket(ref ReceivePacketOptions options, ref ProductUserId outPeerId, ref SocketId outSocketId, out byte outChannel, ArraySegment<byte> outData, out uint outBytesWritten)
		{
			bool wasCacheValid = outSocketId.PrepareForUpdate();
			IntPtr outSocketId2 = Helper.AddPinnedBuffer(outSocketId.m_AllBytes);
			IntPtr outData2 = Helper.AddPinnedBuffer(outData);
			ReceivePacketOptionsInternal receivePacketOptionsInternal = new ReceivePacketOptionsInternal(ref options);
			Result result2;
			try
			{
				IntPtr zero = IntPtr.Zero;
				outChannel = Helper.GetDefault<byte>();
				outBytesWritten = 0U;
				Result result = Bindings.EOS_P2P_ReceivePacket(base.InnerHandle, ref receivePacketOptionsInternal, ref zero, outSocketId2, ref outChannel, outData2, ref outBytesWritten);
				bool flag = outPeerId == null;
				if (flag)
				{
					Helper.Get<ProductUserId>(zero, out outPeerId);
				}
				else
				{
					bool flag2 = outPeerId.InnerHandle != zero;
					if (flag2)
					{
						outPeerId.InnerHandle = zero;
					}
				}
				outSocketId.CheckIfChanged(wasCacheValid);
				result2 = result;
			}
			finally
			{
				Helper.Dispose(ref outSocketId2);
				Helper.Dispose(ref outData2);
				receivePacketOptionsInternal.Dispose();
			}
			return result2;
		}

		// Token: 0x06003141 RID: 12609 RVA: 0x00049470 File Offset: 0x00047670
		public P2PInterface()
		{
		}

		// Token: 0x06003142 RID: 12610 RVA: 0x0004947A File Offset: 0x0004767A
		public P2PInterface(IntPtr innerHandle) : base(innerHandle)
		{
		}

		// Token: 0x06003143 RID: 12611 RVA: 0x00049488 File Offset: 0x00047688
		public Result AcceptConnection(ref AcceptConnectionOptions options)
		{
			AcceptConnectionOptionsInternal acceptConnectionOptionsInternal = default(AcceptConnectionOptionsInternal);
			acceptConnectionOptionsInternal.Set(ref options);
			Result result = Bindings.EOS_P2P_AcceptConnection(base.InnerHandle, ref acceptConnectionOptionsInternal);
			Helper.Dispose<AcceptConnectionOptionsInternal>(ref acceptConnectionOptionsInternal);
			return result;
		}

		// Token: 0x06003144 RID: 12612 RVA: 0x000494C4 File Offset: 0x000476C4
		public ulong AddNotifyIncomingPacketQueueFull(ref AddNotifyIncomingPacketQueueFullOptions options, object clientData, OnIncomingPacketQueueFullCallback incomingPacketQueueFullHandler)
		{
			AddNotifyIncomingPacketQueueFullOptionsInternal addNotifyIncomingPacketQueueFullOptionsInternal = default(AddNotifyIncomingPacketQueueFullOptionsInternal);
			addNotifyIncomingPacketQueueFullOptionsInternal.Set(ref options);
			IntPtr zero = IntPtr.Zero;
			OnIncomingPacketQueueFullCallbackInternal onIncomingPacketQueueFullCallbackInternal = new OnIncomingPacketQueueFullCallbackInternal(P2PInterface.OnIncomingPacketQueueFullCallbackInternalImplementation);
			Helper.AddCallback(out zero, clientData, incomingPacketQueueFullHandler, onIncomingPacketQueueFullCallbackInternal, Array.Empty<Delegate>());
			ulong num = Bindings.EOS_P2P_AddNotifyIncomingPacketQueueFull(base.InnerHandle, ref addNotifyIncomingPacketQueueFullOptionsInternal, zero, onIncomingPacketQueueFullCallbackInternal);
			Helper.Dispose<AddNotifyIncomingPacketQueueFullOptionsInternal>(ref addNotifyIncomingPacketQueueFullOptionsInternal);
			Helper.AssignNotificationIdToCallback(zero, num);
			return num;
		}

		// Token: 0x06003145 RID: 12613 RVA: 0x00049530 File Offset: 0x00047730
		public ulong AddNotifyPeerConnectionClosed(ref AddNotifyPeerConnectionClosedOptions options, object clientData, OnRemoteConnectionClosedCallback connectionClosedHandler)
		{
			AddNotifyPeerConnectionClosedOptionsInternal addNotifyPeerConnectionClosedOptionsInternal = default(AddNotifyPeerConnectionClosedOptionsInternal);
			addNotifyPeerConnectionClosedOptionsInternal.Set(ref options);
			IntPtr zero = IntPtr.Zero;
			OnRemoteConnectionClosedCallbackInternal onRemoteConnectionClosedCallbackInternal = new OnRemoteConnectionClosedCallbackInternal(P2PInterface.OnRemoteConnectionClosedCallbackInternalImplementation);
			Helper.AddCallback(out zero, clientData, connectionClosedHandler, onRemoteConnectionClosedCallbackInternal, Array.Empty<Delegate>());
			ulong num = Bindings.EOS_P2P_AddNotifyPeerConnectionClosed(base.InnerHandle, ref addNotifyPeerConnectionClosedOptionsInternal, zero, onRemoteConnectionClosedCallbackInternal);
			Helper.Dispose<AddNotifyPeerConnectionClosedOptionsInternal>(ref addNotifyPeerConnectionClosedOptionsInternal);
			Helper.AssignNotificationIdToCallback(zero, num);
			return num;
		}

		// Token: 0x06003146 RID: 12614 RVA: 0x0004959C File Offset: 0x0004779C
		public ulong AddNotifyPeerConnectionEstablished(ref AddNotifyPeerConnectionEstablishedOptions options, object clientData, OnPeerConnectionEstablishedCallback connectionEstablishedHandler)
		{
			AddNotifyPeerConnectionEstablishedOptionsInternal addNotifyPeerConnectionEstablishedOptionsInternal = default(AddNotifyPeerConnectionEstablishedOptionsInternal);
			addNotifyPeerConnectionEstablishedOptionsInternal.Set(ref options);
			IntPtr zero = IntPtr.Zero;
			OnPeerConnectionEstablishedCallbackInternal onPeerConnectionEstablishedCallbackInternal = new OnPeerConnectionEstablishedCallbackInternal(P2PInterface.OnPeerConnectionEstablishedCallbackInternalImplementation);
			Helper.AddCallback(out zero, clientData, connectionEstablishedHandler, onPeerConnectionEstablishedCallbackInternal, Array.Empty<Delegate>());
			ulong num = Bindings.EOS_P2P_AddNotifyPeerConnectionEstablished(base.InnerHandle, ref addNotifyPeerConnectionEstablishedOptionsInternal, zero, onPeerConnectionEstablishedCallbackInternal);
			Helper.Dispose<AddNotifyPeerConnectionEstablishedOptionsInternal>(ref addNotifyPeerConnectionEstablishedOptionsInternal);
			Helper.AssignNotificationIdToCallback(zero, num);
			return num;
		}

		// Token: 0x06003147 RID: 12615 RVA: 0x00049608 File Offset: 0x00047808
		public ulong AddNotifyPeerConnectionInterrupted(ref AddNotifyPeerConnectionInterruptedOptions options, object clientData, OnPeerConnectionInterruptedCallback connectionInterruptedHandler)
		{
			AddNotifyPeerConnectionInterruptedOptionsInternal addNotifyPeerConnectionInterruptedOptionsInternal = default(AddNotifyPeerConnectionInterruptedOptionsInternal);
			addNotifyPeerConnectionInterruptedOptionsInternal.Set(ref options);
			IntPtr zero = IntPtr.Zero;
			OnPeerConnectionInterruptedCallbackInternal onPeerConnectionInterruptedCallbackInternal = new OnPeerConnectionInterruptedCallbackInternal(P2PInterface.OnPeerConnectionInterruptedCallbackInternalImplementation);
			Helper.AddCallback(out zero, clientData, connectionInterruptedHandler, onPeerConnectionInterruptedCallbackInternal, Array.Empty<Delegate>());
			ulong num = Bindings.EOS_P2P_AddNotifyPeerConnectionInterrupted(base.InnerHandle, ref addNotifyPeerConnectionInterruptedOptionsInternal, zero, onPeerConnectionInterruptedCallbackInternal);
			Helper.Dispose<AddNotifyPeerConnectionInterruptedOptionsInternal>(ref addNotifyPeerConnectionInterruptedOptionsInternal);
			Helper.AssignNotificationIdToCallback(zero, num);
			return num;
		}

		// Token: 0x06003148 RID: 12616 RVA: 0x00049674 File Offset: 0x00047874
		public ulong AddNotifyPeerConnectionRequest(ref AddNotifyPeerConnectionRequestOptions options, object clientData, OnIncomingConnectionRequestCallback connectionRequestHandler)
		{
			AddNotifyPeerConnectionRequestOptionsInternal addNotifyPeerConnectionRequestOptionsInternal = default(AddNotifyPeerConnectionRequestOptionsInternal);
			addNotifyPeerConnectionRequestOptionsInternal.Set(ref options);
			IntPtr zero = IntPtr.Zero;
			OnIncomingConnectionRequestCallbackInternal onIncomingConnectionRequestCallbackInternal = new OnIncomingConnectionRequestCallbackInternal(P2PInterface.OnIncomingConnectionRequestCallbackInternalImplementation);
			Helper.AddCallback(out zero, clientData, connectionRequestHandler, onIncomingConnectionRequestCallbackInternal, Array.Empty<Delegate>());
			ulong num = Bindings.EOS_P2P_AddNotifyPeerConnectionRequest(base.InnerHandle, ref addNotifyPeerConnectionRequestOptionsInternal, zero, onIncomingConnectionRequestCallbackInternal);
			Helper.Dispose<AddNotifyPeerConnectionRequestOptionsInternal>(ref addNotifyPeerConnectionRequestOptionsInternal);
			Helper.AssignNotificationIdToCallback(zero, num);
			return num;
		}

		// Token: 0x06003149 RID: 12617 RVA: 0x000496E0 File Offset: 0x000478E0
		public Result ClearPacketQueue(ref ClearPacketQueueOptions options)
		{
			ClearPacketQueueOptionsInternal clearPacketQueueOptionsInternal = default(ClearPacketQueueOptionsInternal);
			clearPacketQueueOptionsInternal.Set(ref options);
			Result result = Bindings.EOS_P2P_ClearPacketQueue(base.InnerHandle, ref clearPacketQueueOptionsInternal);
			Helper.Dispose<ClearPacketQueueOptionsInternal>(ref clearPacketQueueOptionsInternal);
			return result;
		}

		// Token: 0x0600314A RID: 12618 RVA: 0x0004971C File Offset: 0x0004791C
		public Result CloseConnection(ref CloseConnectionOptions options)
		{
			CloseConnectionOptionsInternal closeConnectionOptionsInternal = default(CloseConnectionOptionsInternal);
			closeConnectionOptionsInternal.Set(ref options);
			Result result = Bindings.EOS_P2P_CloseConnection(base.InnerHandle, ref closeConnectionOptionsInternal);
			Helper.Dispose<CloseConnectionOptionsInternal>(ref closeConnectionOptionsInternal);
			return result;
		}

		// Token: 0x0600314B RID: 12619 RVA: 0x00049758 File Offset: 0x00047958
		public Result CloseConnections(ref CloseConnectionsOptions options)
		{
			CloseConnectionsOptionsInternal closeConnectionsOptionsInternal = default(CloseConnectionsOptionsInternal);
			closeConnectionsOptionsInternal.Set(ref options);
			Result result = Bindings.EOS_P2P_CloseConnections(base.InnerHandle, ref closeConnectionsOptionsInternal);
			Helper.Dispose<CloseConnectionsOptionsInternal>(ref closeConnectionsOptionsInternal);
			return result;
		}

		// Token: 0x0600314C RID: 12620 RVA: 0x00049794 File Offset: 0x00047994
		public Result GetNATType(ref GetNATTypeOptions options, out NATType outNATType)
		{
			GetNATTypeOptionsInternal getNATTypeOptionsInternal = default(GetNATTypeOptionsInternal);
			getNATTypeOptionsInternal.Set(ref options);
			outNATType = Helper.GetDefault<NATType>();
			Result result = Bindings.EOS_P2P_GetNATType(base.InnerHandle, ref getNATTypeOptionsInternal, ref outNATType);
			Helper.Dispose<GetNATTypeOptionsInternal>(ref getNATTypeOptionsInternal);
			return result;
		}

		// Token: 0x0600314D RID: 12621 RVA: 0x000497D8 File Offset: 0x000479D8
		public Result GetNextReceivedPacketSize(ref GetNextReceivedPacketSizeOptions options, out uint outPacketSizeBytes)
		{
			GetNextReceivedPacketSizeOptionsInternal getNextReceivedPacketSizeOptionsInternal = default(GetNextReceivedPacketSizeOptionsInternal);
			getNextReceivedPacketSizeOptionsInternal.Set(ref options);
			outPacketSizeBytes = Helper.GetDefault<uint>();
			Result result = Bindings.EOS_P2P_GetNextReceivedPacketSize(base.InnerHandle, ref getNextReceivedPacketSizeOptionsInternal, ref outPacketSizeBytes);
			Helper.Dispose<GetNextReceivedPacketSizeOptionsInternal>(ref getNextReceivedPacketSizeOptionsInternal);
			return result;
		}

		// Token: 0x0600314E RID: 12622 RVA: 0x0004981C File Offset: 0x00047A1C
		public Result GetPacketQueueInfo(ref GetPacketQueueInfoOptions options, out PacketQueueInfo outPacketQueueInfo)
		{
			GetPacketQueueInfoOptionsInternal getPacketQueueInfoOptionsInternal = default(GetPacketQueueInfoOptionsInternal);
			getPacketQueueInfoOptionsInternal.Set(ref options);
			PacketQueueInfoInternal @default = Helper.GetDefault<PacketQueueInfoInternal>();
			Result result = Bindings.EOS_P2P_GetPacketQueueInfo(base.InnerHandle, ref getPacketQueueInfoOptionsInternal, ref @default);
			Helper.Dispose<GetPacketQueueInfoOptionsInternal>(ref getPacketQueueInfoOptionsInternal);
			Helper.Get<PacketQueueInfoInternal, PacketQueueInfo>(ref @default, out outPacketQueueInfo);
			return result;
		}

		// Token: 0x0600314F RID: 12623 RVA: 0x00049868 File Offset: 0x00047A68
		public Result GetPortRange(ref GetPortRangeOptions options, out ushort outPort, out ushort outNumAdditionalPortsToTry)
		{
			GetPortRangeOptionsInternal getPortRangeOptionsInternal = default(GetPortRangeOptionsInternal);
			getPortRangeOptionsInternal.Set(ref options);
			outPort = Helper.GetDefault<ushort>();
			outNumAdditionalPortsToTry = Helper.GetDefault<ushort>();
			Result result = Bindings.EOS_P2P_GetPortRange(base.InnerHandle, ref getPortRangeOptionsInternal, ref outPort, ref outNumAdditionalPortsToTry);
			Helper.Dispose<GetPortRangeOptionsInternal>(ref getPortRangeOptionsInternal);
			return result;
		}

		// Token: 0x06003150 RID: 12624 RVA: 0x000498B4 File Offset: 0x00047AB4
		public Result GetRelayControl(ref GetRelayControlOptions options, out RelayControl outRelayControl)
		{
			GetRelayControlOptionsInternal getRelayControlOptionsInternal = default(GetRelayControlOptionsInternal);
			getRelayControlOptionsInternal.Set(ref options);
			outRelayControl = Helper.GetDefault<RelayControl>();
			Result result = Bindings.EOS_P2P_GetRelayControl(base.InnerHandle, ref getRelayControlOptionsInternal, ref outRelayControl);
			Helper.Dispose<GetRelayControlOptionsInternal>(ref getRelayControlOptionsInternal);
			return result;
		}

		// Token: 0x06003151 RID: 12625 RVA: 0x000498F8 File Offset: 0x00047AF8
		public void QueryNATType(ref QueryNATTypeOptions options, object clientData, OnQueryNATTypeCompleteCallback completionDelegate)
		{
			QueryNATTypeOptionsInternal queryNATTypeOptionsInternal = default(QueryNATTypeOptionsInternal);
			queryNATTypeOptionsInternal.Set(ref options);
			IntPtr zero = IntPtr.Zero;
			OnQueryNATTypeCompleteCallbackInternal onQueryNATTypeCompleteCallbackInternal = new OnQueryNATTypeCompleteCallbackInternal(P2PInterface.OnQueryNATTypeCompleteCallbackInternalImplementation);
			Helper.AddCallback(out zero, clientData, completionDelegate, onQueryNATTypeCompleteCallbackInternal, Array.Empty<Delegate>());
			Bindings.EOS_P2P_QueryNATType(base.InnerHandle, ref queryNATTypeOptionsInternal, zero, onQueryNATTypeCompleteCallbackInternal);
			Helper.Dispose<QueryNATTypeOptionsInternal>(ref queryNATTypeOptionsInternal);
		}

		// Token: 0x06003152 RID: 12626 RVA: 0x00049952 File Offset: 0x00047B52
		public void RemoveNotifyIncomingPacketQueueFull(ulong notificationId)
		{
			Bindings.EOS_P2P_RemoveNotifyIncomingPacketQueueFull(base.InnerHandle, notificationId);
			Helper.RemoveCallbackByNotificationId(notificationId);
		}

		// Token: 0x06003153 RID: 12627 RVA: 0x00049969 File Offset: 0x00047B69
		public void RemoveNotifyPeerConnectionClosed(ulong notificationId)
		{
			Bindings.EOS_P2P_RemoveNotifyPeerConnectionClosed(base.InnerHandle, notificationId);
			Helper.RemoveCallbackByNotificationId(notificationId);
		}

		// Token: 0x06003154 RID: 12628 RVA: 0x00049980 File Offset: 0x00047B80
		public void RemoveNotifyPeerConnectionEstablished(ulong notificationId)
		{
			Bindings.EOS_P2P_RemoveNotifyPeerConnectionEstablished(base.InnerHandle, notificationId);
			Helper.RemoveCallbackByNotificationId(notificationId);
		}

		// Token: 0x06003155 RID: 12629 RVA: 0x00049997 File Offset: 0x00047B97
		public void RemoveNotifyPeerConnectionInterrupted(ulong notificationId)
		{
			Bindings.EOS_P2P_RemoveNotifyPeerConnectionInterrupted(base.InnerHandle, notificationId);
			Helper.RemoveCallbackByNotificationId(notificationId);
		}

		// Token: 0x06003156 RID: 12630 RVA: 0x000499AE File Offset: 0x00047BAE
		public void RemoveNotifyPeerConnectionRequest(ulong notificationId)
		{
			Bindings.EOS_P2P_RemoveNotifyPeerConnectionRequest(base.InnerHandle, notificationId);
			Helper.RemoveCallbackByNotificationId(notificationId);
		}

		// Token: 0x06003157 RID: 12631 RVA: 0x000499C8 File Offset: 0x00047BC8
		public Result SendPacket(ref SendPacketOptions options)
		{
			SendPacketOptionsInternal sendPacketOptionsInternal = default(SendPacketOptionsInternal);
			sendPacketOptionsInternal.Set(ref options);
			Result result = Bindings.EOS_P2P_SendPacket(base.InnerHandle, ref sendPacketOptionsInternal);
			Helper.Dispose<SendPacketOptionsInternal>(ref sendPacketOptionsInternal);
			return result;
		}

		// Token: 0x06003158 RID: 12632 RVA: 0x00049A04 File Offset: 0x00047C04
		public Result SetPacketQueueSize(ref SetPacketQueueSizeOptions options)
		{
			SetPacketQueueSizeOptionsInternal setPacketQueueSizeOptionsInternal = default(SetPacketQueueSizeOptionsInternal);
			setPacketQueueSizeOptionsInternal.Set(ref options);
			Result result = Bindings.EOS_P2P_SetPacketQueueSize(base.InnerHandle, ref setPacketQueueSizeOptionsInternal);
			Helper.Dispose<SetPacketQueueSizeOptionsInternal>(ref setPacketQueueSizeOptionsInternal);
			return result;
		}

		// Token: 0x06003159 RID: 12633 RVA: 0x00049A40 File Offset: 0x00047C40
		public Result SetPortRange(ref SetPortRangeOptions options)
		{
			SetPortRangeOptionsInternal setPortRangeOptionsInternal = default(SetPortRangeOptionsInternal);
			setPortRangeOptionsInternal.Set(ref options);
			Result result = Bindings.EOS_P2P_SetPortRange(base.InnerHandle, ref setPortRangeOptionsInternal);
			Helper.Dispose<SetPortRangeOptionsInternal>(ref setPortRangeOptionsInternal);
			return result;
		}

		// Token: 0x0600315A RID: 12634 RVA: 0x00049A7C File Offset: 0x00047C7C
		public Result SetRelayControl(ref SetRelayControlOptions options)
		{
			SetRelayControlOptionsInternal setRelayControlOptionsInternal = default(SetRelayControlOptionsInternal);
			setRelayControlOptionsInternal.Set(ref options);
			Result result = Bindings.EOS_P2P_SetRelayControl(base.InnerHandle, ref setRelayControlOptionsInternal);
			Helper.Dispose<SetRelayControlOptionsInternal>(ref setRelayControlOptionsInternal);
			return result;
		}

		// Token: 0x0600315B RID: 12635 RVA: 0x00049AB8 File Offset: 0x00047CB8
		[MonoPInvokeCallback(typeof(OnIncomingConnectionRequestCallbackInternal))]
		internal static void OnIncomingConnectionRequestCallbackInternalImplementation(ref OnIncomingConnectionRequestInfoInternal data)
		{
			OnIncomingConnectionRequestCallback onIncomingConnectionRequestCallback;
			OnIncomingConnectionRequestInfo onIncomingConnectionRequestInfo;
			bool flag = Helper.TryGetCallback<OnIncomingConnectionRequestInfoInternal, OnIncomingConnectionRequestCallback, OnIncomingConnectionRequestInfo>(ref data, out onIncomingConnectionRequestCallback, out onIncomingConnectionRequestInfo);
			if (flag)
			{
				onIncomingConnectionRequestCallback(ref onIncomingConnectionRequestInfo);
			}
		}

		// Token: 0x0600315C RID: 12636 RVA: 0x00049AE0 File Offset: 0x00047CE0
		[MonoPInvokeCallback(typeof(OnIncomingPacketQueueFullCallbackInternal))]
		internal static void OnIncomingPacketQueueFullCallbackInternalImplementation(ref OnIncomingPacketQueueFullInfoInternal data)
		{
			OnIncomingPacketQueueFullCallback onIncomingPacketQueueFullCallback;
			OnIncomingPacketQueueFullInfo onIncomingPacketQueueFullInfo;
			bool flag = Helper.TryGetCallback<OnIncomingPacketQueueFullInfoInternal, OnIncomingPacketQueueFullCallback, OnIncomingPacketQueueFullInfo>(ref data, out onIncomingPacketQueueFullCallback, out onIncomingPacketQueueFullInfo);
			if (flag)
			{
				onIncomingPacketQueueFullCallback(ref onIncomingPacketQueueFullInfo);
			}
		}

		// Token: 0x0600315D RID: 12637 RVA: 0x00049B08 File Offset: 0x00047D08
		[MonoPInvokeCallback(typeof(OnPeerConnectionEstablishedCallbackInternal))]
		internal static void OnPeerConnectionEstablishedCallbackInternalImplementation(ref OnPeerConnectionEstablishedInfoInternal data)
		{
			OnPeerConnectionEstablishedCallback onPeerConnectionEstablishedCallback;
			OnPeerConnectionEstablishedInfo onPeerConnectionEstablishedInfo;
			bool flag = Helper.TryGetCallback<OnPeerConnectionEstablishedInfoInternal, OnPeerConnectionEstablishedCallback, OnPeerConnectionEstablishedInfo>(ref data, out onPeerConnectionEstablishedCallback, out onPeerConnectionEstablishedInfo);
			if (flag)
			{
				onPeerConnectionEstablishedCallback(ref onPeerConnectionEstablishedInfo);
			}
		}

		// Token: 0x0600315E RID: 12638 RVA: 0x00049B30 File Offset: 0x00047D30
		[MonoPInvokeCallback(typeof(OnPeerConnectionInterruptedCallbackInternal))]
		internal static void OnPeerConnectionInterruptedCallbackInternalImplementation(ref OnPeerConnectionInterruptedInfoInternal data)
		{
			OnPeerConnectionInterruptedCallback onPeerConnectionInterruptedCallback;
			OnPeerConnectionInterruptedInfo onPeerConnectionInterruptedInfo;
			bool flag = Helper.TryGetCallback<OnPeerConnectionInterruptedInfoInternal, OnPeerConnectionInterruptedCallback, OnPeerConnectionInterruptedInfo>(ref data, out onPeerConnectionInterruptedCallback, out onPeerConnectionInterruptedInfo);
			if (flag)
			{
				onPeerConnectionInterruptedCallback(ref onPeerConnectionInterruptedInfo);
			}
		}

		// Token: 0x0600315F RID: 12639 RVA: 0x00049B58 File Offset: 0x00047D58
		[MonoPInvokeCallback(typeof(OnQueryNATTypeCompleteCallbackInternal))]
		internal static void OnQueryNATTypeCompleteCallbackInternalImplementation(ref OnQueryNATTypeCompleteInfoInternal data)
		{
			OnQueryNATTypeCompleteCallback onQueryNATTypeCompleteCallback;
			OnQueryNATTypeCompleteInfo onQueryNATTypeCompleteInfo;
			bool flag = Helper.TryGetAndRemoveCallback<OnQueryNATTypeCompleteInfoInternal, OnQueryNATTypeCompleteCallback, OnQueryNATTypeCompleteInfo>(ref data, out onQueryNATTypeCompleteCallback, out onQueryNATTypeCompleteInfo);
			if (flag)
			{
				onQueryNATTypeCompleteCallback(ref onQueryNATTypeCompleteInfo);
			}
		}

		// Token: 0x06003160 RID: 12640 RVA: 0x00049B80 File Offset: 0x00047D80
		[MonoPInvokeCallback(typeof(OnRemoteConnectionClosedCallbackInternal))]
		internal static void OnRemoteConnectionClosedCallbackInternalImplementation(ref OnRemoteConnectionClosedInfoInternal data)
		{
			OnRemoteConnectionClosedCallback onRemoteConnectionClosedCallback;
			OnRemoteConnectionClosedInfo onRemoteConnectionClosedInfo;
			bool flag = Helper.TryGetCallback<OnRemoteConnectionClosedInfoInternal, OnRemoteConnectionClosedCallback, OnRemoteConnectionClosedInfo>(ref data, out onRemoteConnectionClosedCallback, out onRemoteConnectionClosedInfo);
			if (flag)
			{
				onRemoteConnectionClosedCallback(ref onRemoteConnectionClosedInfo);
			}
		}

		// Token: 0x040015F9 RID: 5625
		public const int AcceptconnectionApiLatest = 1;

		// Token: 0x040015FA RID: 5626
		public const int AddnotifyincomingpacketqueuefullApiLatest = 1;

		// Token: 0x040015FB RID: 5627
		public const int AddnotifypeerconnectionclosedApiLatest = 1;

		// Token: 0x040015FC RID: 5628
		public const int AddnotifypeerconnectionestablishedApiLatest = 1;

		// Token: 0x040015FD RID: 5629
		public const int AddnotifypeerconnectioninterruptedApiLatest = 1;

		// Token: 0x040015FE RID: 5630
		public const int AddnotifypeerconnectionrequestApiLatest = 1;

		// Token: 0x040015FF RID: 5631
		public const int ClearpacketqueueApiLatest = 1;

		// Token: 0x04001600 RID: 5632
		public const int CloseconnectionApiLatest = 1;

		// Token: 0x04001601 RID: 5633
		public const int CloseconnectionsApiLatest = 1;

		// Token: 0x04001602 RID: 5634
		public const int GetnattypeApiLatest = 1;

		// Token: 0x04001603 RID: 5635
		public const int GetnextreceivedpacketsizeApiLatest = 2;

		// Token: 0x04001604 RID: 5636
		public const int GetpacketqueueinfoApiLatest = 1;

		// Token: 0x04001605 RID: 5637
		public const int GetportrangeApiLatest = 1;

		// Token: 0x04001606 RID: 5638
		public const int GetrelaycontrolApiLatest = 1;

		// Token: 0x04001607 RID: 5639
		public const int MaxConnections = 32;

		// Token: 0x04001608 RID: 5640
		public const int MaxPacketSize = 1170;

		// Token: 0x04001609 RID: 5641
		public const int MaxQueueSizeUnlimited = 0;

		// Token: 0x0400160A RID: 5642
		public const int QuerynattypeApiLatest = 1;

		// Token: 0x0400160B RID: 5643
		public const int ReceivepacketApiLatest = 2;

		// Token: 0x0400160C RID: 5644
		public const int SendpacketApiLatest = 3;

		// Token: 0x0400160D RID: 5645
		public const int SetpacketqueuesizeApiLatest = 1;

		// Token: 0x0400160E RID: 5646
		public const int SetportrangeApiLatest = 1;

		// Token: 0x0400160F RID: 5647
		public const int SetrelaycontrolApiLatest = 1;

		// Token: 0x04001610 RID: 5648
		public const int SocketidApiLatest = 1;

		// Token: 0x04001611 RID: 5649
		public const int SocketidSocketnameSize = 33;
	}
}
