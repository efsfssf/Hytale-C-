using System;

namespace Epic.OnlineServices.Auth
{
	// Token: 0x0200062D RID: 1581
	public sealed class AuthInterface : Handle
	{
		// Token: 0x060028F1 RID: 10481 RVA: 0x0003C164 File Offset: 0x0003A364
		public AuthInterface()
		{
		}

		// Token: 0x060028F2 RID: 10482 RVA: 0x0003C16E File Offset: 0x0003A36E
		public AuthInterface(IntPtr innerHandle) : base(innerHandle)
		{
		}

		// Token: 0x060028F3 RID: 10483 RVA: 0x0003C17C File Offset: 0x0003A37C
		public ulong AddNotifyLoginStatusChanged(ref AddNotifyLoginStatusChangedOptions options, object clientData, OnLoginStatusChangedCallback notification)
		{
			AddNotifyLoginStatusChangedOptionsInternal addNotifyLoginStatusChangedOptionsInternal = default(AddNotifyLoginStatusChangedOptionsInternal);
			addNotifyLoginStatusChangedOptionsInternal.Set(ref options);
			IntPtr zero = IntPtr.Zero;
			OnLoginStatusChangedCallbackInternal onLoginStatusChangedCallbackInternal = new OnLoginStatusChangedCallbackInternal(AuthInterface.OnLoginStatusChangedCallbackInternalImplementation);
			Helper.AddCallback(out zero, clientData, notification, onLoginStatusChangedCallbackInternal, Array.Empty<Delegate>());
			ulong num = Bindings.EOS_Auth_AddNotifyLoginStatusChanged(base.InnerHandle, ref addNotifyLoginStatusChangedOptionsInternal, zero, onLoginStatusChangedCallbackInternal);
			Helper.Dispose<AddNotifyLoginStatusChangedOptionsInternal>(ref addNotifyLoginStatusChangedOptionsInternal);
			Helper.AssignNotificationIdToCallback(zero, num);
			return num;
		}

		// Token: 0x060028F4 RID: 10484 RVA: 0x0003C1E8 File Offset: 0x0003A3E8
		public Result CopyIdToken(ref CopyIdTokenOptions options, out IdToken? outIdToken)
		{
			CopyIdTokenOptionsInternal copyIdTokenOptionsInternal = default(CopyIdTokenOptionsInternal);
			copyIdTokenOptionsInternal.Set(ref options);
			IntPtr zero = IntPtr.Zero;
			Result result = Bindings.EOS_Auth_CopyIdToken(base.InnerHandle, ref copyIdTokenOptionsInternal, ref zero);
			Helper.Dispose<CopyIdTokenOptionsInternal>(ref copyIdTokenOptionsInternal);
			Helper.Get<IdTokenInternal, IdToken>(zero, out outIdToken);
			bool flag = outIdToken != null;
			if (flag)
			{
				Bindings.EOS_Auth_IdToken_Release(zero);
			}
			return result;
		}

		// Token: 0x060028F5 RID: 10485 RVA: 0x0003C248 File Offset: 0x0003A448
		public Result CopyUserAuthToken(ref CopyUserAuthTokenOptions options, EpicAccountId localUserId, out Token? outUserAuthToken)
		{
			CopyUserAuthTokenOptionsInternal copyUserAuthTokenOptionsInternal = default(CopyUserAuthTokenOptionsInternal);
			copyUserAuthTokenOptionsInternal.Set(ref options);
			IntPtr zero = IntPtr.Zero;
			Helper.Set(localUserId, ref zero);
			IntPtr zero2 = IntPtr.Zero;
			Result result = Bindings.EOS_Auth_CopyUserAuthToken(base.InnerHandle, ref copyUserAuthTokenOptionsInternal, zero, ref zero2);
			Helper.Dispose<CopyUserAuthTokenOptionsInternal>(ref copyUserAuthTokenOptionsInternal);
			Helper.Get<TokenInternal, Token>(zero2, out outUserAuthToken);
			bool flag = outUserAuthToken != null;
			if (flag)
			{
				Bindings.EOS_Auth_Token_Release(zero2);
			}
			return result;
		}

		// Token: 0x060028F6 RID: 10486 RVA: 0x0003C2BC File Offset: 0x0003A4BC
		public void DeletePersistentAuth(ref DeletePersistentAuthOptions options, object clientData, OnDeletePersistentAuthCallback completionDelegate)
		{
			DeletePersistentAuthOptionsInternal deletePersistentAuthOptionsInternal = default(DeletePersistentAuthOptionsInternal);
			deletePersistentAuthOptionsInternal.Set(ref options);
			IntPtr zero = IntPtr.Zero;
			OnDeletePersistentAuthCallbackInternal onDeletePersistentAuthCallbackInternal = new OnDeletePersistentAuthCallbackInternal(AuthInterface.OnDeletePersistentAuthCallbackInternalImplementation);
			Helper.AddCallback(out zero, clientData, completionDelegate, onDeletePersistentAuthCallbackInternal, Array.Empty<Delegate>());
			Bindings.EOS_Auth_DeletePersistentAuth(base.InnerHandle, ref deletePersistentAuthOptionsInternal, zero, onDeletePersistentAuthCallbackInternal);
			Helper.Dispose<DeletePersistentAuthOptionsInternal>(ref deletePersistentAuthOptionsInternal);
		}

		// Token: 0x060028F7 RID: 10487 RVA: 0x0003C318 File Offset: 0x0003A518
		public EpicAccountId GetLoggedInAccountByIndex(int index)
		{
			IntPtr from = Bindings.EOS_Auth_GetLoggedInAccountByIndex(base.InnerHandle, index);
			EpicAccountId result;
			Helper.Get<EpicAccountId>(from, out result);
			return result;
		}

		// Token: 0x060028F8 RID: 10488 RVA: 0x0003C344 File Offset: 0x0003A544
		public int GetLoggedInAccountsCount()
		{
			return Bindings.EOS_Auth_GetLoggedInAccountsCount(base.InnerHandle);
		}

		// Token: 0x060028F9 RID: 10489 RVA: 0x0003C364 File Offset: 0x0003A564
		public LoginStatus GetLoginStatus(EpicAccountId localUserId)
		{
			IntPtr zero = IntPtr.Zero;
			Helper.Set(localUserId, ref zero);
			return Bindings.EOS_Auth_GetLoginStatus(base.InnerHandle, zero);
		}

		// Token: 0x060028FA RID: 10490 RVA: 0x0003C394 File Offset: 0x0003A594
		public EpicAccountId GetMergedAccountByIndex(EpicAccountId localUserId, uint index)
		{
			IntPtr zero = IntPtr.Zero;
			Helper.Set(localUserId, ref zero);
			IntPtr from = Bindings.EOS_Auth_GetMergedAccountByIndex(base.InnerHandle, zero, index);
			EpicAccountId result;
			Helper.Get<EpicAccountId>(from, out result);
			return result;
		}

		// Token: 0x060028FB RID: 10491 RVA: 0x0003C3D0 File Offset: 0x0003A5D0
		public uint GetMergedAccountsCount(EpicAccountId localUserId)
		{
			IntPtr zero = IntPtr.Zero;
			Helper.Set(localUserId, ref zero);
			return Bindings.EOS_Auth_GetMergedAccountsCount(base.InnerHandle, zero);
		}

		// Token: 0x060028FC RID: 10492 RVA: 0x0003C400 File Offset: 0x0003A600
		public Result GetSelectedAccountId(EpicAccountId localUserId, out EpicAccountId outSelectedAccountId)
		{
			IntPtr zero = IntPtr.Zero;
			Helper.Set(localUserId, ref zero);
			IntPtr zero2 = IntPtr.Zero;
			Result result = Bindings.EOS_Auth_GetSelectedAccountId(base.InnerHandle, zero, ref zero2);
			Helper.Get<EpicAccountId>(zero2, out outSelectedAccountId);
			return result;
		}

		// Token: 0x060028FD RID: 10493 RVA: 0x0003C440 File Offset: 0x0003A640
		public void LinkAccount(ref LinkAccountOptions options, object clientData, OnLinkAccountCallback completionDelegate)
		{
			LinkAccountOptionsInternal linkAccountOptionsInternal = default(LinkAccountOptionsInternal);
			linkAccountOptionsInternal.Set(ref options);
			IntPtr zero = IntPtr.Zero;
			OnLinkAccountCallbackInternal onLinkAccountCallbackInternal = new OnLinkAccountCallbackInternal(AuthInterface.OnLinkAccountCallbackInternalImplementation);
			Helper.AddCallback(out zero, clientData, completionDelegate, onLinkAccountCallbackInternal, Array.Empty<Delegate>());
			Bindings.EOS_Auth_LinkAccount(base.InnerHandle, ref linkAccountOptionsInternal, zero, onLinkAccountCallbackInternal);
			Helper.Dispose<LinkAccountOptionsInternal>(ref linkAccountOptionsInternal);
		}

		// Token: 0x060028FE RID: 10494 RVA: 0x0003C49C File Offset: 0x0003A69C
		public void Login(ref LoginOptions options, object clientData, OnLoginCallback completionDelegate)
		{
			LoginOptionsInternal loginOptionsInternal = default(LoginOptionsInternal);
			loginOptionsInternal.Set(ref options);
			IntPtr zero = IntPtr.Zero;
			OnLoginCallbackInternal onLoginCallbackInternal = new OnLoginCallbackInternal(AuthInterface.OnLoginCallbackInternalImplementation);
			Helper.AddCallback(out zero, clientData, completionDelegate, onLoginCallbackInternal, Array.Empty<Delegate>());
			Bindings.EOS_Auth_Login(base.InnerHandle, ref loginOptionsInternal, zero, onLoginCallbackInternal);
			Helper.Dispose<LoginOptionsInternal>(ref loginOptionsInternal);
		}

		// Token: 0x060028FF RID: 10495 RVA: 0x0003C4F8 File Offset: 0x0003A6F8
		public void Logout(ref LogoutOptions options, object clientData, OnLogoutCallback completionDelegate)
		{
			LogoutOptionsInternal logoutOptionsInternal = default(LogoutOptionsInternal);
			logoutOptionsInternal.Set(ref options);
			IntPtr zero = IntPtr.Zero;
			OnLogoutCallbackInternal onLogoutCallbackInternal = new OnLogoutCallbackInternal(AuthInterface.OnLogoutCallbackInternalImplementation);
			Helper.AddCallback(out zero, clientData, completionDelegate, onLogoutCallbackInternal, Array.Empty<Delegate>());
			Bindings.EOS_Auth_Logout(base.InnerHandle, ref logoutOptionsInternal, zero, onLogoutCallbackInternal);
			Helper.Dispose<LogoutOptionsInternal>(ref logoutOptionsInternal);
		}

		// Token: 0x06002900 RID: 10496 RVA: 0x0003C554 File Offset: 0x0003A754
		public void QueryIdToken(ref QueryIdTokenOptions options, object clientData, OnQueryIdTokenCallback completionDelegate)
		{
			QueryIdTokenOptionsInternal queryIdTokenOptionsInternal = default(QueryIdTokenOptionsInternal);
			queryIdTokenOptionsInternal.Set(ref options);
			IntPtr zero = IntPtr.Zero;
			OnQueryIdTokenCallbackInternal onQueryIdTokenCallbackInternal = new OnQueryIdTokenCallbackInternal(AuthInterface.OnQueryIdTokenCallbackInternalImplementation);
			Helper.AddCallback(out zero, clientData, completionDelegate, onQueryIdTokenCallbackInternal, Array.Empty<Delegate>());
			Bindings.EOS_Auth_QueryIdToken(base.InnerHandle, ref queryIdTokenOptionsInternal, zero, onQueryIdTokenCallbackInternal);
			Helper.Dispose<QueryIdTokenOptionsInternal>(ref queryIdTokenOptionsInternal);
		}

		// Token: 0x06002901 RID: 10497 RVA: 0x0003C5AE File Offset: 0x0003A7AE
		public void RemoveNotifyLoginStatusChanged(ulong inId)
		{
			Bindings.EOS_Auth_RemoveNotifyLoginStatusChanged(base.InnerHandle, inId);
			Helper.RemoveCallbackByNotificationId(inId);
		}

		// Token: 0x06002902 RID: 10498 RVA: 0x0003C5C8 File Offset: 0x0003A7C8
		public void VerifyIdToken(ref VerifyIdTokenOptions options, object clientData, OnVerifyIdTokenCallback completionDelegate)
		{
			VerifyIdTokenOptionsInternal verifyIdTokenOptionsInternal = default(VerifyIdTokenOptionsInternal);
			verifyIdTokenOptionsInternal.Set(ref options);
			IntPtr zero = IntPtr.Zero;
			OnVerifyIdTokenCallbackInternal onVerifyIdTokenCallbackInternal = new OnVerifyIdTokenCallbackInternal(AuthInterface.OnVerifyIdTokenCallbackInternalImplementation);
			Helper.AddCallback(out zero, clientData, completionDelegate, onVerifyIdTokenCallbackInternal, Array.Empty<Delegate>());
			Bindings.EOS_Auth_VerifyIdToken(base.InnerHandle, ref verifyIdTokenOptionsInternal, zero, onVerifyIdTokenCallbackInternal);
			Helper.Dispose<VerifyIdTokenOptionsInternal>(ref verifyIdTokenOptionsInternal);
		}

		// Token: 0x06002903 RID: 10499 RVA: 0x0003C624 File Offset: 0x0003A824
		public void VerifyUserAuth(ref VerifyUserAuthOptions options, object clientData, OnVerifyUserAuthCallback completionDelegate)
		{
			VerifyUserAuthOptionsInternal verifyUserAuthOptionsInternal = default(VerifyUserAuthOptionsInternal);
			verifyUserAuthOptionsInternal.Set(ref options);
			IntPtr zero = IntPtr.Zero;
			OnVerifyUserAuthCallbackInternal onVerifyUserAuthCallbackInternal = new OnVerifyUserAuthCallbackInternal(AuthInterface.OnVerifyUserAuthCallbackInternalImplementation);
			Helper.AddCallback(out zero, clientData, completionDelegate, onVerifyUserAuthCallbackInternal, Array.Empty<Delegate>());
			Bindings.EOS_Auth_VerifyUserAuth(base.InnerHandle, ref verifyUserAuthOptionsInternal, zero, onVerifyUserAuthCallbackInternal);
			Helper.Dispose<VerifyUserAuthOptionsInternal>(ref verifyUserAuthOptionsInternal);
		}

		// Token: 0x06002904 RID: 10500 RVA: 0x0003C680 File Offset: 0x0003A880
		[MonoPInvokeCallback(typeof(OnDeletePersistentAuthCallbackInternal))]
		internal static void OnDeletePersistentAuthCallbackInternalImplementation(ref DeletePersistentAuthCallbackInfoInternal data)
		{
			OnDeletePersistentAuthCallback onDeletePersistentAuthCallback;
			DeletePersistentAuthCallbackInfo deletePersistentAuthCallbackInfo;
			bool flag = Helper.TryGetAndRemoveCallback<DeletePersistentAuthCallbackInfoInternal, OnDeletePersistentAuthCallback, DeletePersistentAuthCallbackInfo>(ref data, out onDeletePersistentAuthCallback, out deletePersistentAuthCallbackInfo);
			if (flag)
			{
				onDeletePersistentAuthCallback(ref deletePersistentAuthCallbackInfo);
			}
		}

		// Token: 0x06002905 RID: 10501 RVA: 0x0003C6A8 File Offset: 0x0003A8A8
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

		// Token: 0x06002906 RID: 10502 RVA: 0x0003C6D0 File Offset: 0x0003A8D0
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

		// Token: 0x06002907 RID: 10503 RVA: 0x0003C6F8 File Offset: 0x0003A8F8
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

		// Token: 0x06002908 RID: 10504 RVA: 0x0003C720 File Offset: 0x0003A920
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

		// Token: 0x06002909 RID: 10505 RVA: 0x0003C748 File Offset: 0x0003A948
		[MonoPInvokeCallback(typeof(OnQueryIdTokenCallbackInternal))]
		internal static void OnQueryIdTokenCallbackInternalImplementation(ref QueryIdTokenCallbackInfoInternal data)
		{
			OnQueryIdTokenCallback onQueryIdTokenCallback;
			QueryIdTokenCallbackInfo queryIdTokenCallbackInfo;
			bool flag = Helper.TryGetAndRemoveCallback<QueryIdTokenCallbackInfoInternal, OnQueryIdTokenCallback, QueryIdTokenCallbackInfo>(ref data, out onQueryIdTokenCallback, out queryIdTokenCallbackInfo);
			if (flag)
			{
				onQueryIdTokenCallback(ref queryIdTokenCallbackInfo);
			}
		}

		// Token: 0x0600290A RID: 10506 RVA: 0x0003C770 File Offset: 0x0003A970
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

		// Token: 0x0600290B RID: 10507 RVA: 0x0003C798 File Offset: 0x0003A998
		[MonoPInvokeCallback(typeof(OnVerifyUserAuthCallbackInternal))]
		internal static void OnVerifyUserAuthCallbackInternalImplementation(ref VerifyUserAuthCallbackInfoInternal data)
		{
			OnVerifyUserAuthCallback onVerifyUserAuthCallback;
			VerifyUserAuthCallbackInfo verifyUserAuthCallbackInfo;
			bool flag = Helper.TryGetAndRemoveCallback<VerifyUserAuthCallbackInfoInternal, OnVerifyUserAuthCallback, VerifyUserAuthCallbackInfo>(ref data, out onVerifyUserAuthCallback, out verifyUserAuthCallbackInfo);
			if (flag)
			{
				onVerifyUserAuthCallback(ref verifyUserAuthCallbackInfo);
			}
		}

		// Token: 0x0600290C RID: 10508 RVA: 0x0003C7C0 File Offset: 0x0003A9C0
		public void Login(ref IOSLoginOptions options, object clientData, OnLoginCallback completionDelegate)
		{
			IOSLoginOptionsInternal iosloginOptionsInternal = default(IOSLoginOptionsInternal);
			iosloginOptionsInternal.Set(ref options);
			IntPtr zero = IntPtr.Zero;
			OnLoginCallbackInternal onLoginCallbackInternal = new OnLoginCallbackInternal(AuthInterface.OnLoginCallbackInternalImplementation);
			Helper.AddCallback(out zero, clientData, completionDelegate, onLoginCallbackInternal, Array.Empty<Delegate>());
			IOSBindings.EOS_Auth_Login(base.InnerHandle, ref iosloginOptionsInternal, zero, onLoginCallbackInternal);
			Helper.Dispose<IOSLoginOptionsInternal>(ref iosloginOptionsInternal);
		}

		// Token: 0x0600290D RID: 10509 RVA: 0x0003C81C File Offset: 0x0003AA1C
		[MonoPInvokeCallback(typeof(IOSCreateBackgroundSnapshotViewInternal))]
		internal static IntPtr IOSCreateBackgroundSnapshotViewInternalImplementation(IntPtr context)
		{
			IOSCreateBackgroundSnapshotView ioscreateBackgroundSnapshotView;
			bool flag = Helper.TryGetStaticCallback<IOSCreateBackgroundSnapshotView>("IOSCreateBackgroundSnapshotViewInternalImplementation", out ioscreateBackgroundSnapshotView);
			IntPtr result;
			if (flag)
			{
				IntPtr intPtr = ioscreateBackgroundSnapshotView(context);
				result = intPtr;
			}
			else
			{
				result = Helper.GetDefault<IntPtr>();
			}
			return result;
		}

		// Token: 0x0400118E RID: 4494
		public const int AccountfeaturerestrictedinfoApiLatest = 1;

		// Token: 0x0400118F RID: 4495
		public const int AddnotifyloginstatuschangedApiLatest = 1;

		// Token: 0x04001190 RID: 4496
		public const int CopyidtokenApiLatest = 1;

		// Token: 0x04001191 RID: 4497
		public const int CopyuserauthtokenApiLatest = 1;

		// Token: 0x04001192 RID: 4498
		public const int CredentialsApiLatest = 4;

		// Token: 0x04001193 RID: 4499
		public const int DeletepersistentauthApiLatest = 2;

		// Token: 0x04001194 RID: 4500
		public const int IdtokenApiLatest = 1;

		// Token: 0x04001195 RID: 4501
		public const int LinkaccountApiLatest = 1;

		// Token: 0x04001196 RID: 4502
		public const int LoginApiLatest = 3;

		// Token: 0x04001197 RID: 4503
		public const int LogoutApiLatest = 1;

		// Token: 0x04001198 RID: 4504
		public const int PingrantinfoApiLatest = 2;

		// Token: 0x04001199 RID: 4505
		public const int QueryidtokenApiLatest = 1;

		// Token: 0x0400119A RID: 4506
		public const int TokenApiLatest = 2;

		// Token: 0x0400119B RID: 4507
		public const int VerifyidtokenApiLatest = 1;

		// Token: 0x0400119C RID: 4508
		public const int VerifyuserauthApiLatest = 1;

		// Token: 0x0400119D RID: 4509
		public const int IosCredentialssystemauthcredentialsoptionsApiLatest = 2;
	}
}
