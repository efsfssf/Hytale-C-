using System;

namespace Epic.OnlineServices.KWS
{
	// Token: 0x0200048F RID: 1167
	public sealed class KWSInterface : Handle
	{
		// Token: 0x06001E7A RID: 7802 RVA: 0x0002CA51 File Offset: 0x0002AC51
		public KWSInterface()
		{
		}

		// Token: 0x06001E7B RID: 7803 RVA: 0x0002CA5B File Offset: 0x0002AC5B
		public KWSInterface(IntPtr innerHandle) : base(innerHandle)
		{
		}

		// Token: 0x06001E7C RID: 7804 RVA: 0x0002CA68 File Offset: 0x0002AC68
		public ulong AddNotifyPermissionsUpdateReceived(ref AddNotifyPermissionsUpdateReceivedOptions options, object clientData, OnPermissionsUpdateReceivedCallback notificationFn)
		{
			AddNotifyPermissionsUpdateReceivedOptionsInternal addNotifyPermissionsUpdateReceivedOptionsInternal = default(AddNotifyPermissionsUpdateReceivedOptionsInternal);
			addNotifyPermissionsUpdateReceivedOptionsInternal.Set(ref options);
			IntPtr zero = IntPtr.Zero;
			OnPermissionsUpdateReceivedCallbackInternal onPermissionsUpdateReceivedCallbackInternal = new OnPermissionsUpdateReceivedCallbackInternal(KWSInterface.OnPermissionsUpdateReceivedCallbackInternalImplementation);
			Helper.AddCallback(out zero, clientData, notificationFn, onPermissionsUpdateReceivedCallbackInternal, Array.Empty<Delegate>());
			ulong num = Bindings.EOS_KWS_AddNotifyPermissionsUpdateReceived(base.InnerHandle, ref addNotifyPermissionsUpdateReceivedOptionsInternal, zero, onPermissionsUpdateReceivedCallbackInternal);
			Helper.Dispose<AddNotifyPermissionsUpdateReceivedOptionsInternal>(ref addNotifyPermissionsUpdateReceivedOptionsInternal);
			Helper.AssignNotificationIdToCallback(zero, num);
			return num;
		}

		// Token: 0x06001E7D RID: 7805 RVA: 0x0002CAD4 File Offset: 0x0002ACD4
		public Result CopyPermissionByIndex(ref CopyPermissionByIndexOptions options, out PermissionStatus? outPermission)
		{
			CopyPermissionByIndexOptionsInternal copyPermissionByIndexOptionsInternal = default(CopyPermissionByIndexOptionsInternal);
			copyPermissionByIndexOptionsInternal.Set(ref options);
			IntPtr zero = IntPtr.Zero;
			Result result = Bindings.EOS_KWS_CopyPermissionByIndex(base.InnerHandle, ref copyPermissionByIndexOptionsInternal, ref zero);
			Helper.Dispose<CopyPermissionByIndexOptionsInternal>(ref copyPermissionByIndexOptionsInternal);
			Helper.Get<PermissionStatusInternal, PermissionStatus>(zero, out outPermission);
			bool flag = outPermission != null;
			if (flag)
			{
				Bindings.EOS_KWS_PermissionStatus_Release(zero);
			}
			return result;
		}

		// Token: 0x06001E7E RID: 7806 RVA: 0x0002CB34 File Offset: 0x0002AD34
		public void CreateUser(ref CreateUserOptions options, object clientData, OnCreateUserCallback completionDelegate)
		{
			CreateUserOptionsInternal createUserOptionsInternal = default(CreateUserOptionsInternal);
			createUserOptionsInternal.Set(ref options);
			IntPtr zero = IntPtr.Zero;
			OnCreateUserCallbackInternal onCreateUserCallbackInternal = new OnCreateUserCallbackInternal(KWSInterface.OnCreateUserCallbackInternalImplementation);
			Helper.AddCallback(out zero, clientData, completionDelegate, onCreateUserCallbackInternal, Array.Empty<Delegate>());
			Bindings.EOS_KWS_CreateUser(base.InnerHandle, ref createUserOptionsInternal, zero, onCreateUserCallbackInternal);
			Helper.Dispose<CreateUserOptionsInternal>(ref createUserOptionsInternal);
		}

		// Token: 0x06001E7F RID: 7807 RVA: 0x0002CB90 File Offset: 0x0002AD90
		public Result GetPermissionByKey(ref GetPermissionByKeyOptions options, out KWSPermissionStatus outPermission)
		{
			GetPermissionByKeyOptionsInternal getPermissionByKeyOptionsInternal = default(GetPermissionByKeyOptionsInternal);
			getPermissionByKeyOptionsInternal.Set(ref options);
			outPermission = Helper.GetDefault<KWSPermissionStatus>();
			Result result = Bindings.EOS_KWS_GetPermissionByKey(base.InnerHandle, ref getPermissionByKeyOptionsInternal, ref outPermission);
			Helper.Dispose<GetPermissionByKeyOptionsInternal>(ref getPermissionByKeyOptionsInternal);
			return result;
		}

		// Token: 0x06001E80 RID: 7808 RVA: 0x0002CBD4 File Offset: 0x0002ADD4
		public int GetPermissionsCount(ref GetPermissionsCountOptions options)
		{
			GetPermissionsCountOptionsInternal getPermissionsCountOptionsInternal = default(GetPermissionsCountOptionsInternal);
			getPermissionsCountOptionsInternal.Set(ref options);
			int result = Bindings.EOS_KWS_GetPermissionsCount(base.InnerHandle, ref getPermissionsCountOptionsInternal);
			Helper.Dispose<GetPermissionsCountOptionsInternal>(ref getPermissionsCountOptionsInternal);
			return result;
		}

		// Token: 0x06001E81 RID: 7809 RVA: 0x0002CC10 File Offset: 0x0002AE10
		public void QueryAgeGate(ref QueryAgeGateOptions options, object clientData, OnQueryAgeGateCallback completionDelegate)
		{
			QueryAgeGateOptionsInternal queryAgeGateOptionsInternal = default(QueryAgeGateOptionsInternal);
			queryAgeGateOptionsInternal.Set(ref options);
			IntPtr zero = IntPtr.Zero;
			OnQueryAgeGateCallbackInternal onQueryAgeGateCallbackInternal = new OnQueryAgeGateCallbackInternal(KWSInterface.OnQueryAgeGateCallbackInternalImplementation);
			Helper.AddCallback(out zero, clientData, completionDelegate, onQueryAgeGateCallbackInternal, Array.Empty<Delegate>());
			Bindings.EOS_KWS_QueryAgeGate(base.InnerHandle, ref queryAgeGateOptionsInternal, zero, onQueryAgeGateCallbackInternal);
			Helper.Dispose<QueryAgeGateOptionsInternal>(ref queryAgeGateOptionsInternal);
		}

		// Token: 0x06001E82 RID: 7810 RVA: 0x0002CC6C File Offset: 0x0002AE6C
		public void QueryPermissions(ref QueryPermissionsOptions options, object clientData, OnQueryPermissionsCallback completionDelegate)
		{
			QueryPermissionsOptionsInternal queryPermissionsOptionsInternal = default(QueryPermissionsOptionsInternal);
			queryPermissionsOptionsInternal.Set(ref options);
			IntPtr zero = IntPtr.Zero;
			OnQueryPermissionsCallbackInternal onQueryPermissionsCallbackInternal = new OnQueryPermissionsCallbackInternal(KWSInterface.OnQueryPermissionsCallbackInternalImplementation);
			Helper.AddCallback(out zero, clientData, completionDelegate, onQueryPermissionsCallbackInternal, Array.Empty<Delegate>());
			Bindings.EOS_KWS_QueryPermissions(base.InnerHandle, ref queryPermissionsOptionsInternal, zero, onQueryPermissionsCallbackInternal);
			Helper.Dispose<QueryPermissionsOptionsInternal>(ref queryPermissionsOptionsInternal);
		}

		// Token: 0x06001E83 RID: 7811 RVA: 0x0002CCC6 File Offset: 0x0002AEC6
		public void RemoveNotifyPermissionsUpdateReceived(ulong inId)
		{
			Bindings.EOS_KWS_RemoveNotifyPermissionsUpdateReceived(base.InnerHandle, inId);
			Helper.RemoveCallbackByNotificationId(inId);
		}

		// Token: 0x06001E84 RID: 7812 RVA: 0x0002CCE0 File Offset: 0x0002AEE0
		public void RequestPermissions(ref RequestPermissionsOptions options, object clientData, OnRequestPermissionsCallback completionDelegate)
		{
			RequestPermissionsOptionsInternal requestPermissionsOptionsInternal = default(RequestPermissionsOptionsInternal);
			requestPermissionsOptionsInternal.Set(ref options);
			IntPtr zero = IntPtr.Zero;
			OnRequestPermissionsCallbackInternal onRequestPermissionsCallbackInternal = new OnRequestPermissionsCallbackInternal(KWSInterface.OnRequestPermissionsCallbackInternalImplementation);
			Helper.AddCallback(out zero, clientData, completionDelegate, onRequestPermissionsCallbackInternal, Array.Empty<Delegate>());
			Bindings.EOS_KWS_RequestPermissions(base.InnerHandle, ref requestPermissionsOptionsInternal, zero, onRequestPermissionsCallbackInternal);
			Helper.Dispose<RequestPermissionsOptionsInternal>(ref requestPermissionsOptionsInternal);
		}

		// Token: 0x06001E85 RID: 7813 RVA: 0x0002CD3C File Offset: 0x0002AF3C
		public void UpdateParentEmail(ref UpdateParentEmailOptions options, object clientData, OnUpdateParentEmailCallback completionDelegate)
		{
			UpdateParentEmailOptionsInternal updateParentEmailOptionsInternal = default(UpdateParentEmailOptionsInternal);
			updateParentEmailOptionsInternal.Set(ref options);
			IntPtr zero = IntPtr.Zero;
			OnUpdateParentEmailCallbackInternal onUpdateParentEmailCallbackInternal = new OnUpdateParentEmailCallbackInternal(KWSInterface.OnUpdateParentEmailCallbackInternalImplementation);
			Helper.AddCallback(out zero, clientData, completionDelegate, onUpdateParentEmailCallbackInternal, Array.Empty<Delegate>());
			Bindings.EOS_KWS_UpdateParentEmail(base.InnerHandle, ref updateParentEmailOptionsInternal, zero, onUpdateParentEmailCallbackInternal);
			Helper.Dispose<UpdateParentEmailOptionsInternal>(ref updateParentEmailOptionsInternal);
		}

		// Token: 0x06001E86 RID: 7814 RVA: 0x0002CD98 File Offset: 0x0002AF98
		[MonoPInvokeCallback(typeof(OnCreateUserCallbackInternal))]
		internal static void OnCreateUserCallbackInternalImplementation(ref CreateUserCallbackInfoInternal data)
		{
			OnCreateUserCallback onCreateUserCallback;
			CreateUserCallbackInfo createUserCallbackInfo;
			bool flag = Helper.TryGetAndRemoveCallback<CreateUserCallbackInfoInternal, OnCreateUserCallback, CreateUserCallbackInfo>(ref data, out onCreateUserCallback, out createUserCallbackInfo);
			if (flag)
			{
				onCreateUserCallback(ref createUserCallbackInfo);
			}
		}

		// Token: 0x06001E87 RID: 7815 RVA: 0x0002CDC0 File Offset: 0x0002AFC0
		[MonoPInvokeCallback(typeof(OnPermissionsUpdateReceivedCallbackInternal))]
		internal static void OnPermissionsUpdateReceivedCallbackInternalImplementation(ref PermissionsUpdateReceivedCallbackInfoInternal data)
		{
			OnPermissionsUpdateReceivedCallback onPermissionsUpdateReceivedCallback;
			PermissionsUpdateReceivedCallbackInfo permissionsUpdateReceivedCallbackInfo;
			bool flag = Helper.TryGetCallback<PermissionsUpdateReceivedCallbackInfoInternal, OnPermissionsUpdateReceivedCallback, PermissionsUpdateReceivedCallbackInfo>(ref data, out onPermissionsUpdateReceivedCallback, out permissionsUpdateReceivedCallbackInfo);
			if (flag)
			{
				onPermissionsUpdateReceivedCallback(ref permissionsUpdateReceivedCallbackInfo);
			}
		}

		// Token: 0x06001E88 RID: 7816 RVA: 0x0002CDE8 File Offset: 0x0002AFE8
		[MonoPInvokeCallback(typeof(OnQueryAgeGateCallbackInternal))]
		internal static void OnQueryAgeGateCallbackInternalImplementation(ref QueryAgeGateCallbackInfoInternal data)
		{
			OnQueryAgeGateCallback onQueryAgeGateCallback;
			QueryAgeGateCallbackInfo queryAgeGateCallbackInfo;
			bool flag = Helper.TryGetAndRemoveCallback<QueryAgeGateCallbackInfoInternal, OnQueryAgeGateCallback, QueryAgeGateCallbackInfo>(ref data, out onQueryAgeGateCallback, out queryAgeGateCallbackInfo);
			if (flag)
			{
				onQueryAgeGateCallback(ref queryAgeGateCallbackInfo);
			}
		}

		// Token: 0x06001E89 RID: 7817 RVA: 0x0002CE10 File Offset: 0x0002B010
		[MonoPInvokeCallback(typeof(OnQueryPermissionsCallbackInternal))]
		internal static void OnQueryPermissionsCallbackInternalImplementation(ref QueryPermissionsCallbackInfoInternal data)
		{
			OnQueryPermissionsCallback onQueryPermissionsCallback;
			QueryPermissionsCallbackInfo queryPermissionsCallbackInfo;
			bool flag = Helper.TryGetAndRemoveCallback<QueryPermissionsCallbackInfoInternal, OnQueryPermissionsCallback, QueryPermissionsCallbackInfo>(ref data, out onQueryPermissionsCallback, out queryPermissionsCallbackInfo);
			if (flag)
			{
				onQueryPermissionsCallback(ref queryPermissionsCallbackInfo);
			}
		}

		// Token: 0x06001E8A RID: 7818 RVA: 0x0002CE38 File Offset: 0x0002B038
		[MonoPInvokeCallback(typeof(OnRequestPermissionsCallbackInternal))]
		internal static void OnRequestPermissionsCallbackInternalImplementation(ref RequestPermissionsCallbackInfoInternal data)
		{
			OnRequestPermissionsCallback onRequestPermissionsCallback;
			RequestPermissionsCallbackInfo requestPermissionsCallbackInfo;
			bool flag = Helper.TryGetAndRemoveCallback<RequestPermissionsCallbackInfoInternal, OnRequestPermissionsCallback, RequestPermissionsCallbackInfo>(ref data, out onRequestPermissionsCallback, out requestPermissionsCallbackInfo);
			if (flag)
			{
				onRequestPermissionsCallback(ref requestPermissionsCallbackInfo);
			}
		}

		// Token: 0x06001E8B RID: 7819 RVA: 0x0002CE60 File Offset: 0x0002B060
		[MonoPInvokeCallback(typeof(OnUpdateParentEmailCallbackInternal))]
		internal static void OnUpdateParentEmailCallbackInternalImplementation(ref UpdateParentEmailCallbackInfoInternal data)
		{
			OnUpdateParentEmailCallback onUpdateParentEmailCallback;
			UpdateParentEmailCallbackInfo updateParentEmailCallbackInfo;
			bool flag = Helper.TryGetAndRemoveCallback<UpdateParentEmailCallbackInfoInternal, OnUpdateParentEmailCallback, UpdateParentEmailCallbackInfo>(ref data, out onUpdateParentEmailCallback, out updateParentEmailCallbackInfo);
			if (flag)
			{
				onUpdateParentEmailCallback(ref updateParentEmailCallbackInfo);
			}
		}

		// Token: 0x04000D4E RID: 3406
		public const int AddnotifypermissionsupdatereceivedApiLatest = 1;

		// Token: 0x04000D4F RID: 3407
		public const int CopypermissionbyindexApiLatest = 1;

		// Token: 0x04000D50 RID: 3408
		public const int CreateuserApiLatest = 1;

		// Token: 0x04000D51 RID: 3409
		public const int GetpermissionbykeyApiLatest = 1;

		// Token: 0x04000D52 RID: 3410
		public const int GetpermissionscountApiLatest = 1;

		// Token: 0x04000D53 RID: 3411
		public const int MaxPermissionLength = 32;

		// Token: 0x04000D54 RID: 3412
		public const int MaxPermissions = 16;

		// Token: 0x04000D55 RID: 3413
		public const int PermissionstatusApiLatest = 1;

		// Token: 0x04000D56 RID: 3414
		public const int QueryagegateApiLatest = 1;

		// Token: 0x04000D57 RID: 3415
		public const int QuerypermissionsApiLatest = 1;

		// Token: 0x04000D58 RID: 3416
		public const int RequestpermissionsApiLatest = 1;

		// Token: 0x04000D59 RID: 3417
		public const int UpdateparentemailApiLatest = 1;
	}
}
