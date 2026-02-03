using System;

namespace Epic.OnlineServices.Connect
{
	// Token: 0x020005C8 RID: 1480
	public sealed class ConnectInterface : Handle
	{
		// Token: 0x06002670 RID: 9840 RVA: 0x0003883D File Offset: 0x00036A3D
		public ConnectInterface()
		{
		}

		// Token: 0x06002671 RID: 9841 RVA: 0x00038847 File Offset: 0x00036A47
		public ConnectInterface(IntPtr innerHandle) : base(innerHandle)
		{
		}

		// Token: 0x06002672 RID: 9842 RVA: 0x00038854 File Offset: 0x00036A54
		public ulong AddNotifyAuthExpiration(ref AddNotifyAuthExpirationOptions options, object clientData, OnAuthExpirationCallback notification)
		{
			AddNotifyAuthExpirationOptionsInternal addNotifyAuthExpirationOptionsInternal = default(AddNotifyAuthExpirationOptionsInternal);
			addNotifyAuthExpirationOptionsInternal.Set(ref options);
			IntPtr zero = IntPtr.Zero;
			OnAuthExpirationCallbackInternal onAuthExpirationCallbackInternal = new OnAuthExpirationCallbackInternal(ConnectInterface.OnAuthExpirationCallbackInternalImplementation);
			Helper.AddCallback(out zero, clientData, notification, onAuthExpirationCallbackInternal, Array.Empty<Delegate>());
			ulong num = Bindings.EOS_Connect_AddNotifyAuthExpiration(base.InnerHandle, ref addNotifyAuthExpirationOptionsInternal, zero, onAuthExpirationCallbackInternal);
			Helper.Dispose<AddNotifyAuthExpirationOptionsInternal>(ref addNotifyAuthExpirationOptionsInternal);
			Helper.AssignNotificationIdToCallback(zero, num);
			return num;
		}

		// Token: 0x06002673 RID: 9843 RVA: 0x000388C0 File Offset: 0x00036AC0
		public ulong AddNotifyLoginStatusChanged(ref AddNotifyLoginStatusChangedOptions options, object clientData, OnLoginStatusChangedCallback notification)
		{
			AddNotifyLoginStatusChangedOptionsInternal addNotifyLoginStatusChangedOptionsInternal = default(AddNotifyLoginStatusChangedOptionsInternal);
			addNotifyLoginStatusChangedOptionsInternal.Set(ref options);
			IntPtr zero = IntPtr.Zero;
			OnLoginStatusChangedCallbackInternal onLoginStatusChangedCallbackInternal = new OnLoginStatusChangedCallbackInternal(ConnectInterface.OnLoginStatusChangedCallbackInternalImplementation);
			Helper.AddCallback(out zero, clientData, notification, onLoginStatusChangedCallbackInternal, Array.Empty<Delegate>());
			ulong num = Bindings.EOS_Connect_AddNotifyLoginStatusChanged(base.InnerHandle, ref addNotifyLoginStatusChangedOptionsInternal, zero, onLoginStatusChangedCallbackInternal);
			Helper.Dispose<AddNotifyLoginStatusChangedOptionsInternal>(ref addNotifyLoginStatusChangedOptionsInternal);
			Helper.AssignNotificationIdToCallback(zero, num);
			return num;
		}

		// Token: 0x06002674 RID: 9844 RVA: 0x0003892C File Offset: 0x00036B2C
		public Result CopyIdToken(ref CopyIdTokenOptions options, out IdToken? outIdToken)
		{
			CopyIdTokenOptionsInternal copyIdTokenOptionsInternal = default(CopyIdTokenOptionsInternal);
			copyIdTokenOptionsInternal.Set(ref options);
			IntPtr zero = IntPtr.Zero;
			Result result = Bindings.EOS_Connect_CopyIdToken(base.InnerHandle, ref copyIdTokenOptionsInternal, ref zero);
			Helper.Dispose<CopyIdTokenOptionsInternal>(ref copyIdTokenOptionsInternal);
			Helper.Get<IdTokenInternal, IdToken>(zero, out outIdToken);
			bool flag = outIdToken != null;
			if (flag)
			{
				Bindings.EOS_Connect_IdToken_Release(zero);
			}
			return result;
		}

		// Token: 0x06002675 RID: 9845 RVA: 0x0003898C File Offset: 0x00036B8C
		public Result CopyProductUserExternalAccountByAccountId(ref CopyProductUserExternalAccountByAccountIdOptions options, out ExternalAccountInfo? outExternalAccountInfo)
		{
			CopyProductUserExternalAccountByAccountIdOptionsInternal copyProductUserExternalAccountByAccountIdOptionsInternal = default(CopyProductUserExternalAccountByAccountIdOptionsInternal);
			copyProductUserExternalAccountByAccountIdOptionsInternal.Set(ref options);
			IntPtr zero = IntPtr.Zero;
			Result result = Bindings.EOS_Connect_CopyProductUserExternalAccountByAccountId(base.InnerHandle, ref copyProductUserExternalAccountByAccountIdOptionsInternal, ref zero);
			Helper.Dispose<CopyProductUserExternalAccountByAccountIdOptionsInternal>(ref copyProductUserExternalAccountByAccountIdOptionsInternal);
			Helper.Get<ExternalAccountInfoInternal, ExternalAccountInfo>(zero, out outExternalAccountInfo);
			bool flag = outExternalAccountInfo != null;
			if (flag)
			{
				Bindings.EOS_Connect_ExternalAccountInfo_Release(zero);
			}
			return result;
		}

		// Token: 0x06002676 RID: 9846 RVA: 0x000389EC File Offset: 0x00036BEC
		public Result CopyProductUserExternalAccountByAccountType(ref CopyProductUserExternalAccountByAccountTypeOptions options, out ExternalAccountInfo? outExternalAccountInfo)
		{
			CopyProductUserExternalAccountByAccountTypeOptionsInternal copyProductUserExternalAccountByAccountTypeOptionsInternal = default(CopyProductUserExternalAccountByAccountTypeOptionsInternal);
			copyProductUserExternalAccountByAccountTypeOptionsInternal.Set(ref options);
			IntPtr zero = IntPtr.Zero;
			Result result = Bindings.EOS_Connect_CopyProductUserExternalAccountByAccountType(base.InnerHandle, ref copyProductUserExternalAccountByAccountTypeOptionsInternal, ref zero);
			Helper.Dispose<CopyProductUserExternalAccountByAccountTypeOptionsInternal>(ref copyProductUserExternalAccountByAccountTypeOptionsInternal);
			Helper.Get<ExternalAccountInfoInternal, ExternalAccountInfo>(zero, out outExternalAccountInfo);
			bool flag = outExternalAccountInfo != null;
			if (flag)
			{
				Bindings.EOS_Connect_ExternalAccountInfo_Release(zero);
			}
			return result;
		}

		// Token: 0x06002677 RID: 9847 RVA: 0x00038A4C File Offset: 0x00036C4C
		public Result CopyProductUserExternalAccountByIndex(ref CopyProductUserExternalAccountByIndexOptions options, out ExternalAccountInfo? outExternalAccountInfo)
		{
			CopyProductUserExternalAccountByIndexOptionsInternal copyProductUserExternalAccountByIndexOptionsInternal = default(CopyProductUserExternalAccountByIndexOptionsInternal);
			copyProductUserExternalAccountByIndexOptionsInternal.Set(ref options);
			IntPtr zero = IntPtr.Zero;
			Result result = Bindings.EOS_Connect_CopyProductUserExternalAccountByIndex(base.InnerHandle, ref copyProductUserExternalAccountByIndexOptionsInternal, ref zero);
			Helper.Dispose<CopyProductUserExternalAccountByIndexOptionsInternal>(ref copyProductUserExternalAccountByIndexOptionsInternal);
			Helper.Get<ExternalAccountInfoInternal, ExternalAccountInfo>(zero, out outExternalAccountInfo);
			bool flag = outExternalAccountInfo != null;
			if (flag)
			{
				Bindings.EOS_Connect_ExternalAccountInfo_Release(zero);
			}
			return result;
		}

		// Token: 0x06002678 RID: 9848 RVA: 0x00038AAC File Offset: 0x00036CAC
		public Result CopyProductUserInfo(ref CopyProductUserInfoOptions options, out ExternalAccountInfo? outExternalAccountInfo)
		{
			CopyProductUserInfoOptionsInternal copyProductUserInfoOptionsInternal = default(CopyProductUserInfoOptionsInternal);
			copyProductUserInfoOptionsInternal.Set(ref options);
			IntPtr zero = IntPtr.Zero;
			Result result = Bindings.EOS_Connect_CopyProductUserInfo(base.InnerHandle, ref copyProductUserInfoOptionsInternal, ref zero);
			Helper.Dispose<CopyProductUserInfoOptionsInternal>(ref copyProductUserInfoOptionsInternal);
			Helper.Get<ExternalAccountInfoInternal, ExternalAccountInfo>(zero, out outExternalAccountInfo);
			bool flag = outExternalAccountInfo != null;
			if (flag)
			{
				Bindings.EOS_Connect_ExternalAccountInfo_Release(zero);
			}
			return result;
		}

		// Token: 0x06002679 RID: 9849 RVA: 0x00038B0C File Offset: 0x00036D0C
		public void CreateDeviceId(ref CreateDeviceIdOptions options, object clientData, OnCreateDeviceIdCallback completionDelegate)
		{
			CreateDeviceIdOptionsInternal createDeviceIdOptionsInternal = default(CreateDeviceIdOptionsInternal);
			createDeviceIdOptionsInternal.Set(ref options);
			IntPtr zero = IntPtr.Zero;
			OnCreateDeviceIdCallbackInternal onCreateDeviceIdCallbackInternal = new OnCreateDeviceIdCallbackInternal(ConnectInterface.OnCreateDeviceIdCallbackInternalImplementation);
			Helper.AddCallback(out zero, clientData, completionDelegate, onCreateDeviceIdCallbackInternal, Array.Empty<Delegate>());
			Bindings.EOS_Connect_CreateDeviceId(base.InnerHandle, ref createDeviceIdOptionsInternal, zero, onCreateDeviceIdCallbackInternal);
			Helper.Dispose<CreateDeviceIdOptionsInternal>(ref createDeviceIdOptionsInternal);
		}

		// Token: 0x0600267A RID: 9850 RVA: 0x00038B68 File Offset: 0x00036D68
		public void CreateUser(ref CreateUserOptions options, object clientData, OnCreateUserCallback completionDelegate)
		{
			CreateUserOptionsInternal createUserOptionsInternal = default(CreateUserOptionsInternal);
			createUserOptionsInternal.Set(ref options);
			IntPtr zero = IntPtr.Zero;
			OnCreateUserCallbackInternal onCreateUserCallbackInternal = new OnCreateUserCallbackInternal(ConnectInterface.OnCreateUserCallbackInternalImplementation);
			Helper.AddCallback(out zero, clientData, completionDelegate, onCreateUserCallbackInternal, Array.Empty<Delegate>());
			Bindings.EOS_Connect_CreateUser(base.InnerHandle, ref createUserOptionsInternal, zero, onCreateUserCallbackInternal);
			Helper.Dispose<CreateUserOptionsInternal>(ref createUserOptionsInternal);
		}

		// Token: 0x0600267B RID: 9851 RVA: 0x00038BC4 File Offset: 0x00036DC4
		public void DeleteDeviceId(ref DeleteDeviceIdOptions options, object clientData, OnDeleteDeviceIdCallback completionDelegate)
		{
			DeleteDeviceIdOptionsInternal deleteDeviceIdOptionsInternal = default(DeleteDeviceIdOptionsInternal);
			deleteDeviceIdOptionsInternal.Set(ref options);
			IntPtr zero = IntPtr.Zero;
			OnDeleteDeviceIdCallbackInternal onDeleteDeviceIdCallbackInternal = new OnDeleteDeviceIdCallbackInternal(ConnectInterface.OnDeleteDeviceIdCallbackInternalImplementation);
			Helper.AddCallback(out zero, clientData, completionDelegate, onDeleteDeviceIdCallbackInternal, Array.Empty<Delegate>());
			Bindings.EOS_Connect_DeleteDeviceId(base.InnerHandle, ref deleteDeviceIdOptionsInternal, zero, onDeleteDeviceIdCallbackInternal);
			Helper.Dispose<DeleteDeviceIdOptionsInternal>(ref deleteDeviceIdOptionsInternal);
		}

		// Token: 0x0600267C RID: 9852 RVA: 0x00038C20 File Offset: 0x00036E20
		public ProductUserId GetExternalAccountMapping(ref GetExternalAccountMappingsOptions options)
		{
			GetExternalAccountMappingsOptionsInternal getExternalAccountMappingsOptionsInternal = default(GetExternalAccountMappingsOptionsInternal);
			getExternalAccountMappingsOptionsInternal.Set(ref options);
			IntPtr from = Bindings.EOS_Connect_GetExternalAccountMapping(base.InnerHandle, ref getExternalAccountMappingsOptionsInternal);
			Helper.Dispose<GetExternalAccountMappingsOptionsInternal>(ref getExternalAccountMappingsOptionsInternal);
			ProductUserId result;
			Helper.Get<ProductUserId>(from, out result);
			return result;
		}

		// Token: 0x0600267D RID: 9853 RVA: 0x00038C64 File Offset: 0x00036E64
		public ProductUserId GetLoggedInUserByIndex(int index)
		{
			IntPtr from = Bindings.EOS_Connect_GetLoggedInUserByIndex(base.InnerHandle, index);
			ProductUserId result;
			Helper.Get<ProductUserId>(from, out result);
			return result;
		}

		// Token: 0x0600267E RID: 9854 RVA: 0x00038C90 File Offset: 0x00036E90
		public int GetLoggedInUsersCount()
		{
			return Bindings.EOS_Connect_GetLoggedInUsersCount(base.InnerHandle);
		}

		// Token: 0x0600267F RID: 9855 RVA: 0x00038CB0 File Offset: 0x00036EB0
		public LoginStatus GetLoginStatus(ProductUserId localUserId)
		{
			IntPtr zero = IntPtr.Zero;
			Helper.Set(localUserId, ref zero);
			return Bindings.EOS_Connect_GetLoginStatus(base.InnerHandle, zero);
		}

		// Token: 0x06002680 RID: 9856 RVA: 0x00038CE0 File Offset: 0x00036EE0
		public uint GetProductUserExternalAccountCount(ref GetProductUserExternalAccountCountOptions options)
		{
			GetProductUserExternalAccountCountOptionsInternal getProductUserExternalAccountCountOptionsInternal = default(GetProductUserExternalAccountCountOptionsInternal);
			getProductUserExternalAccountCountOptionsInternal.Set(ref options);
			uint result = Bindings.EOS_Connect_GetProductUserExternalAccountCount(base.InnerHandle, ref getProductUserExternalAccountCountOptionsInternal);
			Helper.Dispose<GetProductUserExternalAccountCountOptionsInternal>(ref getProductUserExternalAccountCountOptionsInternal);
			return result;
		}

		// Token: 0x06002681 RID: 9857 RVA: 0x00038D1C File Offset: 0x00036F1C
		public Result GetProductUserIdMapping(ref GetProductUserIdMappingOptions options, out Utf8String outBuffer)
		{
			GetProductUserIdMappingOptionsInternal getProductUserIdMappingOptionsInternal = default(GetProductUserIdMappingOptionsInternal);
			getProductUserIdMappingOptionsInternal.Set(ref options);
			int size = 257;
			IntPtr intPtr = Helper.AddAllocation(size);
			Result result = Bindings.EOS_Connect_GetProductUserIdMapping(base.InnerHandle, ref getProductUserIdMappingOptionsInternal, intPtr, ref size);
			Helper.Dispose<GetProductUserIdMappingOptionsInternal>(ref getProductUserIdMappingOptionsInternal);
			Helper.Get(intPtr, out outBuffer);
			Helper.Dispose(ref intPtr);
			return result;
		}

		// Token: 0x06002682 RID: 9858 RVA: 0x00038D78 File Offset: 0x00036F78
		public void LinkAccount(ref LinkAccountOptions options, object clientData, OnLinkAccountCallback completionDelegate)
		{
			LinkAccountOptionsInternal linkAccountOptionsInternal = default(LinkAccountOptionsInternal);
			linkAccountOptionsInternal.Set(ref options);
			IntPtr zero = IntPtr.Zero;
			OnLinkAccountCallbackInternal onLinkAccountCallbackInternal = new OnLinkAccountCallbackInternal(ConnectInterface.OnLinkAccountCallbackInternalImplementation);
			Helper.AddCallback(out zero, clientData, completionDelegate, onLinkAccountCallbackInternal, Array.Empty<Delegate>());
			Bindings.EOS_Connect_LinkAccount(base.InnerHandle, ref linkAccountOptionsInternal, zero, onLinkAccountCallbackInternal);
			Helper.Dispose<LinkAccountOptionsInternal>(ref linkAccountOptionsInternal);
		}

		// Token: 0x06002683 RID: 9859 RVA: 0x00038DD4 File Offset: 0x00036FD4
		public void Login(ref LoginOptions options, object clientData, OnLoginCallback completionDelegate)
		{
			LoginOptionsInternal loginOptionsInternal = default(LoginOptionsInternal);
			loginOptionsInternal.Set(ref options);
			IntPtr zero = IntPtr.Zero;
			OnLoginCallbackInternal onLoginCallbackInternal = new OnLoginCallbackInternal(ConnectInterface.OnLoginCallbackInternalImplementation);
			Helper.AddCallback(out zero, clientData, completionDelegate, onLoginCallbackInternal, Array.Empty<Delegate>());
			Bindings.EOS_Connect_Login(base.InnerHandle, ref loginOptionsInternal, zero, onLoginCallbackInternal);
			Helper.Dispose<LoginOptionsInternal>(ref loginOptionsInternal);
		}

		// Token: 0x06002684 RID: 9860 RVA: 0x00038E30 File Offset: 0x00037030
		public void Logout(ref LogoutOptions options, object clientData, OnLogoutCallback completionDelegate)
		{
			LogoutOptionsInternal logoutOptionsInternal = default(LogoutOptionsInternal);
			logoutOptionsInternal.Set(ref options);
			IntPtr zero = IntPtr.Zero;
			OnLogoutCallbackInternal onLogoutCallbackInternal = new OnLogoutCallbackInternal(ConnectInterface.OnLogoutCallbackInternalImplementation);
			Helper.AddCallback(out zero, clientData, completionDelegate, onLogoutCallbackInternal, Array.Empty<Delegate>());
			Bindings.EOS_Connect_Logout(base.InnerHandle, ref logoutOptionsInternal, zero, onLogoutCallbackInternal);
			Helper.Dispose<LogoutOptionsInternal>(ref logoutOptionsInternal);
		}

		// Token: 0x06002685 RID: 9861 RVA: 0x00038E8C File Offset: 0x0003708C
		public void QueryExternalAccountMappings(ref QueryExternalAccountMappingsOptions options, object clientData, OnQueryExternalAccountMappingsCallback completionDelegate)
		{
			QueryExternalAccountMappingsOptionsInternal queryExternalAccountMappingsOptionsInternal = default(QueryExternalAccountMappingsOptionsInternal);
			queryExternalAccountMappingsOptionsInternal.Set(ref options);
			IntPtr zero = IntPtr.Zero;
			OnQueryExternalAccountMappingsCallbackInternal onQueryExternalAccountMappingsCallbackInternal = new OnQueryExternalAccountMappingsCallbackInternal(ConnectInterface.OnQueryExternalAccountMappingsCallbackInternalImplementation);
			Helper.AddCallback(out zero, clientData, completionDelegate, onQueryExternalAccountMappingsCallbackInternal, Array.Empty<Delegate>());
			Bindings.EOS_Connect_QueryExternalAccountMappings(base.InnerHandle, ref queryExternalAccountMappingsOptionsInternal, zero, onQueryExternalAccountMappingsCallbackInternal);
			Helper.Dispose<QueryExternalAccountMappingsOptionsInternal>(ref queryExternalAccountMappingsOptionsInternal);
		}

		// Token: 0x06002686 RID: 9862 RVA: 0x00038EE8 File Offset: 0x000370E8
		public void QueryProductUserIdMappings(ref QueryProductUserIdMappingsOptions options, object clientData, OnQueryProductUserIdMappingsCallback completionDelegate)
		{
			QueryProductUserIdMappingsOptionsInternal queryProductUserIdMappingsOptionsInternal = default(QueryProductUserIdMappingsOptionsInternal);
			queryProductUserIdMappingsOptionsInternal.Set(ref options);
			IntPtr zero = IntPtr.Zero;
			OnQueryProductUserIdMappingsCallbackInternal onQueryProductUserIdMappingsCallbackInternal = new OnQueryProductUserIdMappingsCallbackInternal(ConnectInterface.OnQueryProductUserIdMappingsCallbackInternalImplementation);
			Helper.AddCallback(out zero, clientData, completionDelegate, onQueryProductUserIdMappingsCallbackInternal, Array.Empty<Delegate>());
			Bindings.EOS_Connect_QueryProductUserIdMappings(base.InnerHandle, ref queryProductUserIdMappingsOptionsInternal, zero, onQueryProductUserIdMappingsCallbackInternal);
			Helper.Dispose<QueryProductUserIdMappingsOptionsInternal>(ref queryProductUserIdMappingsOptionsInternal);
		}

		// Token: 0x06002687 RID: 9863 RVA: 0x00038F42 File Offset: 0x00037142
		public void RemoveNotifyAuthExpiration(ulong inId)
		{
			Bindings.EOS_Connect_RemoveNotifyAuthExpiration(base.InnerHandle, inId);
			Helper.RemoveCallbackByNotificationId(inId);
		}

		// Token: 0x06002688 RID: 9864 RVA: 0x00038F59 File Offset: 0x00037159
		public void RemoveNotifyLoginStatusChanged(ulong inId)
		{
			Bindings.EOS_Connect_RemoveNotifyLoginStatusChanged(base.InnerHandle, inId);
			Helper.RemoveCallbackByNotificationId(inId);
		}

		// Token: 0x06002689 RID: 9865 RVA: 0x00038F70 File Offset: 0x00037170
		public void TransferDeviceIdAccount(ref TransferDeviceIdAccountOptions options, object clientData, OnTransferDeviceIdAccountCallback completionDelegate)
		{
			TransferDeviceIdAccountOptionsInternal transferDeviceIdAccountOptionsInternal = default(TransferDeviceIdAccountOptionsInternal);
			transferDeviceIdAccountOptionsInternal.Set(ref options);
			IntPtr zero = IntPtr.Zero;
			OnTransferDeviceIdAccountCallbackInternal onTransferDeviceIdAccountCallbackInternal = new OnTransferDeviceIdAccountCallbackInternal(ConnectInterface.OnTransferDeviceIdAccountCallbackInternalImplementation);
			Helper.AddCallback(out zero, clientData, completionDelegate, onTransferDeviceIdAccountCallbackInternal, Array.Empty<Delegate>());
			Bindings.EOS_Connect_TransferDeviceIdAccount(base.InnerHandle, ref transferDeviceIdAccountOptionsInternal, zero, onTransferDeviceIdAccountCallbackInternal);
			Helper.Dispose<TransferDeviceIdAccountOptionsInternal>(ref transferDeviceIdAccountOptionsInternal);
		}

		// Token: 0x0600268A RID: 9866 RVA: 0x00038FCC File Offset: 0x000371CC
		public void UnlinkAccount(ref UnlinkAccountOptions options, object clientData, OnUnlinkAccountCallback completionDelegate)
		{
			UnlinkAccountOptionsInternal unlinkAccountOptionsInternal = default(UnlinkAccountOptionsInternal);
			unlinkAccountOptionsInternal.Set(ref options);
			IntPtr zero = IntPtr.Zero;
			OnUnlinkAccountCallbackInternal onUnlinkAccountCallbackInternal = new OnUnlinkAccountCallbackInternal(ConnectInterface.OnUnlinkAccountCallbackInternalImplementation);
			Helper.AddCallback(out zero, clientData, completionDelegate, onUnlinkAccountCallbackInternal, Array.Empty<Delegate>());
			Bindings.EOS_Connect_UnlinkAccount(base.InnerHandle, ref unlinkAccountOptionsInternal, zero, onUnlinkAccountCallbackInternal);
			Helper.Dispose<UnlinkAccountOptionsInternal>(ref unlinkAccountOptionsInternal);
		}

		// Token: 0x0600268B RID: 9867 RVA: 0x00039028 File Offset: 0x00037228
		public void VerifyIdToken(ref VerifyIdTokenOptions options, object clientData, OnVerifyIdTokenCallback completionDelegate)
		{
			VerifyIdTokenOptionsInternal verifyIdTokenOptionsInternal = default(VerifyIdTokenOptionsInternal);
			verifyIdTokenOptionsInternal.Set(ref options);
			IntPtr zero = IntPtr.Zero;
			OnVerifyIdTokenCallbackInternal onVerifyIdTokenCallbackInternal = new OnVerifyIdTokenCallbackInternal(ConnectInterface.OnVerifyIdTokenCallbackInternalImplementation);
			Helper.AddCallback(out zero, clientData, completionDelegate, onVerifyIdTokenCallbackInternal, Array.Empty<Delegate>());
			Bindings.EOS_Connect_VerifyIdToken(base.InnerHandle, ref verifyIdTokenOptionsInternal, zero, onVerifyIdTokenCallbackInternal);
			Helper.Dispose<VerifyIdTokenOptionsInternal>(ref verifyIdTokenOptionsInternal);
		}

		// Token: 0x0600268C RID: 9868 RVA: 0x00039084 File Offset: 0x00037284
		[MonoPInvokeCallback(typeof(OnAuthExpirationCallbackInternal))]
		internal static void OnAuthExpirationCallbackInternalImplementation(ref AuthExpirationCallbackInfoInternal data)
		{
			OnAuthExpirationCallback onAuthExpirationCallback;
			AuthExpirationCallbackInfo authExpirationCallbackInfo;
			bool flag = Helper.TryGetCallback<AuthExpirationCallbackInfoInternal, OnAuthExpirationCallback, AuthExpirationCallbackInfo>(ref data, out onAuthExpirationCallback, out authExpirationCallbackInfo);
			if (flag)
			{
				onAuthExpirationCallback(ref authExpirationCallbackInfo);
			}
		}

		// Token: 0x0600268D RID: 9869 RVA: 0x000390AC File Offset: 0x000372AC
		[MonoPInvokeCallback(typeof(OnCreateDeviceIdCallbackInternal))]
		internal static void OnCreateDeviceIdCallbackInternalImplementation(ref CreateDeviceIdCallbackInfoInternal data)
		{
			OnCreateDeviceIdCallback onCreateDeviceIdCallback;
			CreateDeviceIdCallbackInfo createDeviceIdCallbackInfo;
			bool flag = Helper.TryGetAndRemoveCallback<CreateDeviceIdCallbackInfoInternal, OnCreateDeviceIdCallback, CreateDeviceIdCallbackInfo>(ref data, out onCreateDeviceIdCallback, out createDeviceIdCallbackInfo);
			if (flag)
			{
				onCreateDeviceIdCallback(ref createDeviceIdCallbackInfo);
			}
		}

		// Token: 0x0600268E RID: 9870 RVA: 0x000390D4 File Offset: 0x000372D4
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

		// Token: 0x0600268F RID: 9871 RVA: 0x000390FC File Offset: 0x000372FC
		[MonoPInvokeCallback(typeof(OnDeleteDeviceIdCallbackInternal))]
		internal static void OnDeleteDeviceIdCallbackInternalImplementation(ref DeleteDeviceIdCallbackInfoInternal data)
		{
			OnDeleteDeviceIdCallback onDeleteDeviceIdCallback;
			DeleteDeviceIdCallbackInfo deleteDeviceIdCallbackInfo;
			bool flag = Helper.TryGetAndRemoveCallback<DeleteDeviceIdCallbackInfoInternal, OnDeleteDeviceIdCallback, DeleteDeviceIdCallbackInfo>(ref data, out onDeleteDeviceIdCallback, out deleteDeviceIdCallbackInfo);
			if (flag)
			{
				onDeleteDeviceIdCallback(ref deleteDeviceIdCallbackInfo);
			}
		}

		// Token: 0x06002690 RID: 9872 RVA: 0x00039124 File Offset: 0x00037324
		[MonoPInvokeCallback(typeof(OnLinkAccountCallbackInternal))]
		internal static void OnLinkAccountCallbackInternalImplementation(ref LinkAccountCallbackInfoInternal data)
		{
			OnLinkAccountCallback onLinkAccountCallback;
			LinkAccountCallbackInfo linkAccountCallbackInfo;
			bool flag = Helper.TryGetAndRemoveCallback<LinkAccountCallbackInfoInternal, OnLinkAccountCallback, LinkAccountCallbackInfo>(ref data, out onLinkAccountCallback, out linkAccountCallbackInfo);
			if (flag)
			{
				onLinkAccountCallback(ref linkAccountCallbackInfo);
			}
		}

		// Token: 0x06002691 RID: 9873 RVA: 0x0003914C File Offset: 0x0003734C
		[MonoPInvokeCallback(typeof(OnLoginCallbackInternal))]
		internal static void OnLoginCallbackInternalImplementation(ref LoginCallbackInfoInternal data)
		{
			OnLoginCallback onLoginCallback;
			LoginCallbackInfo loginCallbackInfo;
			bool flag = Helper.TryGetAndRemoveCallback<LoginCallbackInfoInternal, OnLoginCallback, LoginCallbackInfo>(ref data, out onLoginCallback, out loginCallbackInfo);
			if (flag)
			{
				onLoginCallback(ref loginCallbackInfo);
			}
		}

		// Token: 0x06002692 RID: 9874 RVA: 0x00039174 File Offset: 0x00037374
		[MonoPInvokeCallback(typeof(OnLoginStatusChangedCallbackInternal))]
		internal static void OnLoginStatusChangedCallbackInternalImplementation(ref LoginStatusChangedCallbackInfoInternal data)
		{
			OnLoginStatusChangedCallback onLoginStatusChangedCallback;
			LoginStatusChangedCallbackInfo loginStatusChangedCallbackInfo;
			bool flag = Helper.TryGetCallback<LoginStatusChangedCallbackInfoInternal, OnLoginStatusChangedCallback, LoginStatusChangedCallbackInfo>(ref data, out onLoginStatusChangedCallback, out loginStatusChangedCallbackInfo);
			if (flag)
			{
				onLoginStatusChangedCallback(ref loginStatusChangedCallbackInfo);
			}
		}

		// Token: 0x06002693 RID: 9875 RVA: 0x0003919C File Offset: 0x0003739C
		[MonoPInvokeCallback(typeof(OnLogoutCallbackInternal))]
		internal static void OnLogoutCallbackInternalImplementation(ref LogoutCallbackInfoInternal data)
		{
			OnLogoutCallback onLogoutCallback;
			LogoutCallbackInfo logoutCallbackInfo;
			bool flag = Helper.TryGetAndRemoveCallback<LogoutCallbackInfoInternal, OnLogoutCallback, LogoutCallbackInfo>(ref data, out onLogoutCallback, out logoutCallbackInfo);
			if (flag)
			{
				onLogoutCallback(ref logoutCallbackInfo);
			}
		}

		// Token: 0x06002694 RID: 9876 RVA: 0x000391C4 File Offset: 0x000373C4
		[MonoPInvokeCallback(typeof(OnQueryExternalAccountMappingsCallbackInternal))]
		internal static void OnQueryExternalAccountMappingsCallbackInternalImplementation(ref QueryExternalAccountMappingsCallbackInfoInternal data)
		{
			OnQueryExternalAccountMappingsCallback onQueryExternalAccountMappingsCallback;
			QueryExternalAccountMappingsCallbackInfo queryExternalAccountMappingsCallbackInfo;
			bool flag = Helper.TryGetAndRemoveCallback<QueryExternalAccountMappingsCallbackInfoInternal, OnQueryExternalAccountMappingsCallback, QueryExternalAccountMappingsCallbackInfo>(ref data, out onQueryExternalAccountMappingsCallback, out queryExternalAccountMappingsCallbackInfo);
			if (flag)
			{
				onQueryExternalAccountMappingsCallback(ref queryExternalAccountMappingsCallbackInfo);
			}
		}

		// Token: 0x06002695 RID: 9877 RVA: 0x000391EC File Offset: 0x000373EC
		[MonoPInvokeCallback(typeof(OnQueryProductUserIdMappingsCallbackInternal))]
		internal static void OnQueryProductUserIdMappingsCallbackInternalImplementation(ref QueryProductUserIdMappingsCallbackInfoInternal data)
		{
			OnQueryProductUserIdMappingsCallback onQueryProductUserIdMappingsCallback;
			QueryProductUserIdMappingsCallbackInfo queryProductUserIdMappingsCallbackInfo;
			bool flag = Helper.TryGetAndRemoveCallback<QueryProductUserIdMappingsCallbackInfoInternal, OnQueryProductUserIdMappingsCallback, QueryProductUserIdMappingsCallbackInfo>(ref data, out onQueryProductUserIdMappingsCallback, out queryProductUserIdMappingsCallbackInfo);
			if (flag)
			{
				onQueryProductUserIdMappingsCallback(ref queryProductUserIdMappingsCallbackInfo);
			}
		}

		// Token: 0x06002696 RID: 9878 RVA: 0x00039214 File Offset: 0x00037414
		[MonoPInvokeCallback(typeof(OnTransferDeviceIdAccountCallbackInternal))]
		internal static void OnTransferDeviceIdAccountCallbackInternalImplementation(ref TransferDeviceIdAccountCallbackInfoInternal data)
		{
			OnTransferDeviceIdAccountCallback onTransferDeviceIdAccountCallback;
			TransferDeviceIdAccountCallbackInfo transferDeviceIdAccountCallbackInfo;
			bool flag = Helper.TryGetAndRemoveCallback<TransferDeviceIdAccountCallbackInfoInternal, OnTransferDeviceIdAccountCallback, TransferDeviceIdAccountCallbackInfo>(ref data, out onTransferDeviceIdAccountCallback, out transferDeviceIdAccountCallbackInfo);
			if (flag)
			{
				onTransferDeviceIdAccountCallback(ref transferDeviceIdAccountCallbackInfo);
			}
		}

		// Token: 0x06002697 RID: 9879 RVA: 0x0003923C File Offset: 0x0003743C
		[MonoPInvokeCallback(typeof(OnUnlinkAccountCallbackInternal))]
		internal static void OnUnlinkAccountCallbackInternalImplementation(ref UnlinkAccountCallbackInfoInternal data)
		{
			OnUnlinkAccountCallback onUnlinkAccountCallback;
			UnlinkAccountCallbackInfo unlinkAccountCallbackInfo;
			bool flag = Helper.TryGetAndRemoveCallback<UnlinkAccountCallbackInfoInternal, OnUnlinkAccountCallback, UnlinkAccountCallbackInfo>(ref data, out onUnlinkAccountCallback, out unlinkAccountCallbackInfo);
			if (flag)
			{
				onUnlinkAccountCallback(ref unlinkAccountCallbackInfo);
			}
		}

		// Token: 0x06002698 RID: 9880 RVA: 0x00039264 File Offset: 0x00037464
		[MonoPInvokeCallback(typeof(OnVerifyIdTokenCallbackInternal))]
		internal static void OnVerifyIdTokenCallbackInternalImplementation(ref VerifyIdTokenCallbackInfoInternal data)
		{
			OnVerifyIdTokenCallback onVerifyIdTokenCallback;
			VerifyIdTokenCallbackInfo verifyIdTokenCallbackInfo;
			bool flag = Helper.TryGetAndRemoveCallback<VerifyIdTokenCallbackInfoInternal, OnVerifyIdTokenCallback, VerifyIdTokenCallbackInfo>(ref data, out onVerifyIdTokenCallback, out verifyIdTokenCallbackInfo);
			if (flag)
			{
				onVerifyIdTokenCallback(ref verifyIdTokenCallbackInfo);
			}
		}

		// Token: 0x0400109F RID: 4255
		public const int AddnotifyauthexpirationApiLatest = 1;

		// Token: 0x040010A0 RID: 4256
		public const int AddnotifyloginstatuschangedApiLatest = 1;

		// Token: 0x040010A1 RID: 4257
		public const int CopyidtokenApiLatest = 1;

		// Token: 0x040010A2 RID: 4258
		public const int CopyproductuserexternalaccountbyaccountidApiLatest = 1;

		// Token: 0x040010A3 RID: 4259
		public const int CopyproductuserexternalaccountbyaccounttypeApiLatest = 1;

		// Token: 0x040010A4 RID: 4260
		public const int CopyproductuserexternalaccountbyindexApiLatest = 1;

		// Token: 0x040010A5 RID: 4261
		public const int CopyproductuserinfoApiLatest = 1;

		// Token: 0x040010A6 RID: 4262
		public const int CreatedeviceidApiLatest = 1;

		// Token: 0x040010A7 RID: 4263
		public const int CreatedeviceidDevicemodelMaxLength = 64;

		// Token: 0x040010A8 RID: 4264
		public const int CreateuserApiLatest = 1;

		// Token: 0x040010A9 RID: 4265
		public const int CredentialsApiLatest = 1;

		// Token: 0x040010AA RID: 4266
		public const int DeletedeviceidApiLatest = 1;

		// Token: 0x040010AB RID: 4267
		public const int ExternalAccountIdMaxLength = 256;

		// Token: 0x040010AC RID: 4268
		public const int ExternalaccountinfoApiLatest = 1;

		// Token: 0x040010AD RID: 4269
		public const int GetexternalaccountmappingApiLatest = 1;

		// Token: 0x040010AE RID: 4270
		public const int GetexternalaccountmappingsApiLatest = 1;

		// Token: 0x040010AF RID: 4271
		public const int GetproductuserexternalaccountcountApiLatest = 1;

		// Token: 0x040010B0 RID: 4272
		public const int GetproductuseridmappingApiLatest = 1;

		// Token: 0x040010B1 RID: 4273
		public const int IdtokenApiLatest = 1;

		// Token: 0x040010B2 RID: 4274
		public const int LinkaccountApiLatest = 1;

		// Token: 0x040010B3 RID: 4275
		public const int LoginApiLatest = 2;

		// Token: 0x040010B4 RID: 4276
		public const int LogoutApiLatest = 1;

		// Token: 0x040010B5 RID: 4277
		public const int OnauthexpirationcallbackApiLatest = 1;

		// Token: 0x040010B6 RID: 4278
		public const int QueryexternalaccountmappingsApiLatest = 1;

		// Token: 0x040010B7 RID: 4279
		public const int QueryexternalaccountmappingsMaxAccountIds = 128;

		// Token: 0x040010B8 RID: 4280
		public const int QueryproductuseridmappingsApiLatest = 2;

		// Token: 0x040010B9 RID: 4281
		public const int TimeUndefined = -1;

		// Token: 0x040010BA RID: 4282
		public const int TransferdeviceidaccountApiLatest = 1;

		// Token: 0x040010BB RID: 4283
		public const int UnlinkaccountApiLatest = 1;

		// Token: 0x040010BC RID: 4284
		public const int UserlogininfoApiLatest = 2;

		// Token: 0x040010BD RID: 4285
		public const int UserlogininfoDisplaynameMaxLength = 32;

		// Token: 0x040010BE RID: 4286
		public const int VerifyidtokenApiLatest = 1;
	}
}
