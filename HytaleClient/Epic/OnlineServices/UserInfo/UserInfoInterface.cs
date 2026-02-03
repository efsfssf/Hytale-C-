using System;

namespace Epic.OnlineServices.UserInfo
{
	// Token: 0x0200004C RID: 76
	public sealed class UserInfoInterface : Handle
	{
		// Token: 0x06000470 RID: 1136 RVA: 0x00006570 File Offset: 0x00004770
		public UserInfoInterface()
		{
		}

		// Token: 0x06000471 RID: 1137 RVA: 0x0000657A File Offset: 0x0000477A
		public UserInfoInterface(IntPtr innerHandle) : base(innerHandle)
		{
		}

		// Token: 0x06000472 RID: 1138 RVA: 0x00006588 File Offset: 0x00004788
		public Result CopyBestDisplayName(ref CopyBestDisplayNameOptions options, out BestDisplayName? outBestDisplayName)
		{
			CopyBestDisplayNameOptionsInternal copyBestDisplayNameOptionsInternal = default(CopyBestDisplayNameOptionsInternal);
			copyBestDisplayNameOptionsInternal.Set(ref options);
			IntPtr zero = IntPtr.Zero;
			Result result = Bindings.EOS_UserInfo_CopyBestDisplayName(base.InnerHandle, ref copyBestDisplayNameOptionsInternal, ref zero);
			Helper.Dispose<CopyBestDisplayNameOptionsInternal>(ref copyBestDisplayNameOptionsInternal);
			Helper.Get<BestDisplayNameInternal, BestDisplayName>(zero, out outBestDisplayName);
			bool flag = outBestDisplayName != null;
			if (flag)
			{
				Bindings.EOS_UserInfo_BestDisplayName_Release(zero);
			}
			return result;
		}

		// Token: 0x06000473 RID: 1139 RVA: 0x000065E8 File Offset: 0x000047E8
		public Result CopyBestDisplayNameWithPlatform(ref CopyBestDisplayNameWithPlatformOptions options, out BestDisplayName? outBestDisplayName)
		{
			CopyBestDisplayNameWithPlatformOptionsInternal copyBestDisplayNameWithPlatformOptionsInternal = default(CopyBestDisplayNameWithPlatformOptionsInternal);
			copyBestDisplayNameWithPlatformOptionsInternal.Set(ref options);
			IntPtr zero = IntPtr.Zero;
			Result result = Bindings.EOS_UserInfo_CopyBestDisplayNameWithPlatform(base.InnerHandle, ref copyBestDisplayNameWithPlatformOptionsInternal, ref zero);
			Helper.Dispose<CopyBestDisplayNameWithPlatformOptionsInternal>(ref copyBestDisplayNameWithPlatformOptionsInternal);
			Helper.Get<BestDisplayNameInternal, BestDisplayName>(zero, out outBestDisplayName);
			bool flag = outBestDisplayName != null;
			if (flag)
			{
				Bindings.EOS_UserInfo_BestDisplayName_Release(zero);
			}
			return result;
		}

		// Token: 0x06000474 RID: 1140 RVA: 0x00006648 File Offset: 0x00004848
		public Result CopyExternalUserInfoByAccountId(ref CopyExternalUserInfoByAccountIdOptions options, out ExternalUserInfo? outExternalUserInfo)
		{
			CopyExternalUserInfoByAccountIdOptionsInternal copyExternalUserInfoByAccountIdOptionsInternal = default(CopyExternalUserInfoByAccountIdOptionsInternal);
			copyExternalUserInfoByAccountIdOptionsInternal.Set(ref options);
			IntPtr zero = IntPtr.Zero;
			Result result = Bindings.EOS_UserInfo_CopyExternalUserInfoByAccountId(base.InnerHandle, ref copyExternalUserInfoByAccountIdOptionsInternal, ref zero);
			Helper.Dispose<CopyExternalUserInfoByAccountIdOptionsInternal>(ref copyExternalUserInfoByAccountIdOptionsInternal);
			Helper.Get<ExternalUserInfoInternal, ExternalUserInfo>(zero, out outExternalUserInfo);
			bool flag = outExternalUserInfo != null;
			if (flag)
			{
				Bindings.EOS_UserInfo_ExternalUserInfo_Release(zero);
			}
			return result;
		}

		// Token: 0x06000475 RID: 1141 RVA: 0x000066A8 File Offset: 0x000048A8
		public Result CopyExternalUserInfoByAccountType(ref CopyExternalUserInfoByAccountTypeOptions options, out ExternalUserInfo? outExternalUserInfo)
		{
			CopyExternalUserInfoByAccountTypeOptionsInternal copyExternalUserInfoByAccountTypeOptionsInternal = default(CopyExternalUserInfoByAccountTypeOptionsInternal);
			copyExternalUserInfoByAccountTypeOptionsInternal.Set(ref options);
			IntPtr zero = IntPtr.Zero;
			Result result = Bindings.EOS_UserInfo_CopyExternalUserInfoByAccountType(base.InnerHandle, ref copyExternalUserInfoByAccountTypeOptionsInternal, ref zero);
			Helper.Dispose<CopyExternalUserInfoByAccountTypeOptionsInternal>(ref copyExternalUserInfoByAccountTypeOptionsInternal);
			Helper.Get<ExternalUserInfoInternal, ExternalUserInfo>(zero, out outExternalUserInfo);
			bool flag = outExternalUserInfo != null;
			if (flag)
			{
				Bindings.EOS_UserInfo_ExternalUserInfo_Release(zero);
			}
			return result;
		}

		// Token: 0x06000476 RID: 1142 RVA: 0x00006708 File Offset: 0x00004908
		public Result CopyExternalUserInfoByIndex(ref CopyExternalUserInfoByIndexOptions options, out ExternalUserInfo? outExternalUserInfo)
		{
			CopyExternalUserInfoByIndexOptionsInternal copyExternalUserInfoByIndexOptionsInternal = default(CopyExternalUserInfoByIndexOptionsInternal);
			copyExternalUserInfoByIndexOptionsInternal.Set(ref options);
			IntPtr zero = IntPtr.Zero;
			Result result = Bindings.EOS_UserInfo_CopyExternalUserInfoByIndex(base.InnerHandle, ref copyExternalUserInfoByIndexOptionsInternal, ref zero);
			Helper.Dispose<CopyExternalUserInfoByIndexOptionsInternal>(ref copyExternalUserInfoByIndexOptionsInternal);
			Helper.Get<ExternalUserInfoInternal, ExternalUserInfo>(zero, out outExternalUserInfo);
			bool flag = outExternalUserInfo != null;
			if (flag)
			{
				Bindings.EOS_UserInfo_ExternalUserInfo_Release(zero);
			}
			return result;
		}

		// Token: 0x06000477 RID: 1143 RVA: 0x00006768 File Offset: 0x00004968
		public Result CopyUserInfo(ref CopyUserInfoOptions options, out UserInfoData? outUserInfo)
		{
			CopyUserInfoOptionsInternal copyUserInfoOptionsInternal = default(CopyUserInfoOptionsInternal);
			copyUserInfoOptionsInternal.Set(ref options);
			IntPtr zero = IntPtr.Zero;
			Result result = Bindings.EOS_UserInfo_CopyUserInfo(base.InnerHandle, ref copyUserInfoOptionsInternal, ref zero);
			Helper.Dispose<CopyUserInfoOptionsInternal>(ref copyUserInfoOptionsInternal);
			Helper.Get<UserInfoDataInternal, UserInfoData>(zero, out outUserInfo);
			bool flag = outUserInfo != null;
			if (flag)
			{
				Bindings.EOS_UserInfo_Release(zero);
			}
			return result;
		}

		// Token: 0x06000478 RID: 1144 RVA: 0x000067C8 File Offset: 0x000049C8
		public uint GetExternalUserInfoCount(ref GetExternalUserInfoCountOptions options)
		{
			GetExternalUserInfoCountOptionsInternal getExternalUserInfoCountOptionsInternal = default(GetExternalUserInfoCountOptionsInternal);
			getExternalUserInfoCountOptionsInternal.Set(ref options);
			uint result = Bindings.EOS_UserInfo_GetExternalUserInfoCount(base.InnerHandle, ref getExternalUserInfoCountOptionsInternal);
			Helper.Dispose<GetExternalUserInfoCountOptionsInternal>(ref getExternalUserInfoCountOptionsInternal);
			return result;
		}

		// Token: 0x06000479 RID: 1145 RVA: 0x00006804 File Offset: 0x00004A04
		public uint GetLocalPlatformType(ref GetLocalPlatformTypeOptions options)
		{
			GetLocalPlatformTypeOptionsInternal getLocalPlatformTypeOptionsInternal = default(GetLocalPlatformTypeOptionsInternal);
			getLocalPlatformTypeOptionsInternal.Set(ref options);
			uint result = Bindings.EOS_UserInfo_GetLocalPlatformType(base.InnerHandle, ref getLocalPlatformTypeOptionsInternal);
			Helper.Dispose<GetLocalPlatformTypeOptionsInternal>(ref getLocalPlatformTypeOptionsInternal);
			return result;
		}

		// Token: 0x0600047A RID: 1146 RVA: 0x00006840 File Offset: 0x00004A40
		public void QueryUserInfo(ref QueryUserInfoOptions options, object clientData, OnQueryUserInfoCallback completionDelegate)
		{
			QueryUserInfoOptionsInternal queryUserInfoOptionsInternal = default(QueryUserInfoOptionsInternal);
			queryUserInfoOptionsInternal.Set(ref options);
			IntPtr zero = IntPtr.Zero;
			OnQueryUserInfoCallbackInternal onQueryUserInfoCallbackInternal = new OnQueryUserInfoCallbackInternal(UserInfoInterface.OnQueryUserInfoCallbackInternalImplementation);
			Helper.AddCallback(out zero, clientData, completionDelegate, onQueryUserInfoCallbackInternal, Array.Empty<Delegate>());
			Bindings.EOS_UserInfo_QueryUserInfo(base.InnerHandle, ref queryUserInfoOptionsInternal, zero, onQueryUserInfoCallbackInternal);
			Helper.Dispose<QueryUserInfoOptionsInternal>(ref queryUserInfoOptionsInternal);
		}

		// Token: 0x0600047B RID: 1147 RVA: 0x0000689C File Offset: 0x00004A9C
		public void QueryUserInfoByDisplayName(ref QueryUserInfoByDisplayNameOptions options, object clientData, OnQueryUserInfoByDisplayNameCallback completionDelegate)
		{
			QueryUserInfoByDisplayNameOptionsInternal queryUserInfoByDisplayNameOptionsInternal = default(QueryUserInfoByDisplayNameOptionsInternal);
			queryUserInfoByDisplayNameOptionsInternal.Set(ref options);
			IntPtr zero = IntPtr.Zero;
			OnQueryUserInfoByDisplayNameCallbackInternal onQueryUserInfoByDisplayNameCallbackInternal = new OnQueryUserInfoByDisplayNameCallbackInternal(UserInfoInterface.OnQueryUserInfoByDisplayNameCallbackInternalImplementation);
			Helper.AddCallback(out zero, clientData, completionDelegate, onQueryUserInfoByDisplayNameCallbackInternal, Array.Empty<Delegate>());
			Bindings.EOS_UserInfo_QueryUserInfoByDisplayName(base.InnerHandle, ref queryUserInfoByDisplayNameOptionsInternal, zero, onQueryUserInfoByDisplayNameCallbackInternal);
			Helper.Dispose<QueryUserInfoByDisplayNameOptionsInternal>(ref queryUserInfoByDisplayNameOptionsInternal);
		}

		// Token: 0x0600047C RID: 1148 RVA: 0x000068F8 File Offset: 0x00004AF8
		public void QueryUserInfoByExternalAccount(ref QueryUserInfoByExternalAccountOptions options, object clientData, OnQueryUserInfoByExternalAccountCallback completionDelegate)
		{
			QueryUserInfoByExternalAccountOptionsInternal queryUserInfoByExternalAccountOptionsInternal = default(QueryUserInfoByExternalAccountOptionsInternal);
			queryUserInfoByExternalAccountOptionsInternal.Set(ref options);
			IntPtr zero = IntPtr.Zero;
			OnQueryUserInfoByExternalAccountCallbackInternal onQueryUserInfoByExternalAccountCallbackInternal = new OnQueryUserInfoByExternalAccountCallbackInternal(UserInfoInterface.OnQueryUserInfoByExternalAccountCallbackInternalImplementation);
			Helper.AddCallback(out zero, clientData, completionDelegate, onQueryUserInfoByExternalAccountCallbackInternal, Array.Empty<Delegate>());
			Bindings.EOS_UserInfo_QueryUserInfoByExternalAccount(base.InnerHandle, ref queryUserInfoByExternalAccountOptionsInternal, zero, onQueryUserInfoByExternalAccountCallbackInternal);
			Helper.Dispose<QueryUserInfoByExternalAccountOptionsInternal>(ref queryUserInfoByExternalAccountOptionsInternal);
		}

		// Token: 0x0600047D RID: 1149 RVA: 0x00006954 File Offset: 0x00004B54
		[MonoPInvokeCallback(typeof(OnQueryUserInfoByDisplayNameCallbackInternal))]
		internal static void OnQueryUserInfoByDisplayNameCallbackInternalImplementation(ref QueryUserInfoByDisplayNameCallbackInfoInternal data)
		{
			OnQueryUserInfoByDisplayNameCallback onQueryUserInfoByDisplayNameCallback;
			QueryUserInfoByDisplayNameCallbackInfo queryUserInfoByDisplayNameCallbackInfo;
			bool flag = Helper.TryGetAndRemoveCallback<QueryUserInfoByDisplayNameCallbackInfoInternal, OnQueryUserInfoByDisplayNameCallback, QueryUserInfoByDisplayNameCallbackInfo>(ref data, out onQueryUserInfoByDisplayNameCallback, out queryUserInfoByDisplayNameCallbackInfo);
			if (flag)
			{
				onQueryUserInfoByDisplayNameCallback(ref queryUserInfoByDisplayNameCallbackInfo);
			}
		}

		// Token: 0x0600047E RID: 1150 RVA: 0x0000697C File Offset: 0x00004B7C
		[MonoPInvokeCallback(typeof(OnQueryUserInfoByExternalAccountCallbackInternal))]
		internal static void OnQueryUserInfoByExternalAccountCallbackInternalImplementation(ref QueryUserInfoByExternalAccountCallbackInfoInternal data)
		{
			OnQueryUserInfoByExternalAccountCallback onQueryUserInfoByExternalAccountCallback;
			QueryUserInfoByExternalAccountCallbackInfo queryUserInfoByExternalAccountCallbackInfo;
			bool flag = Helper.TryGetAndRemoveCallback<QueryUserInfoByExternalAccountCallbackInfoInternal, OnQueryUserInfoByExternalAccountCallback, QueryUserInfoByExternalAccountCallbackInfo>(ref data, out onQueryUserInfoByExternalAccountCallback, out queryUserInfoByExternalAccountCallbackInfo);
			if (flag)
			{
				onQueryUserInfoByExternalAccountCallback(ref queryUserInfoByExternalAccountCallbackInfo);
			}
		}

		// Token: 0x0600047F RID: 1151 RVA: 0x000069A4 File Offset: 0x00004BA4
		[MonoPInvokeCallback(typeof(OnQueryUserInfoCallbackInternal))]
		internal static void OnQueryUserInfoCallbackInternalImplementation(ref QueryUserInfoCallbackInfoInternal data)
		{
			OnQueryUserInfoCallback onQueryUserInfoCallback;
			QueryUserInfoCallbackInfo queryUserInfoCallbackInfo;
			bool flag = Helper.TryGetAndRemoveCallback<QueryUserInfoCallbackInfoInternal, OnQueryUserInfoCallback, QueryUserInfoCallbackInfo>(ref data, out onQueryUserInfoCallback, out queryUserInfoCallbackInfo);
			if (flag)
			{
				onQueryUserInfoCallback(ref queryUserInfoCallbackInfo);
			}
		}

		// Token: 0x040001C8 RID: 456
		public const int BestdisplaynameApiLatest = 1;

		// Token: 0x040001C9 RID: 457
		public const int CopybestdisplaynameApiLatest = 1;

		// Token: 0x040001CA RID: 458
		public const int CopybestdisplaynamewithplatformApiLatest = 1;

		// Token: 0x040001CB RID: 459
		public const int CopyexternaluserinfobyaccountidApiLatest = 1;

		// Token: 0x040001CC RID: 460
		public const int CopyexternaluserinfobyaccounttypeApiLatest = 1;

		// Token: 0x040001CD RID: 461
		public const int CopyexternaluserinfobyindexApiLatest = 1;

		// Token: 0x040001CE RID: 462
		public const int CopyuserinfoApiLatest = 3;

		// Token: 0x040001CF RID: 463
		public const int ExternaluserinfoApiLatest = 2;

		// Token: 0x040001D0 RID: 464
		public const int GetexternaluserinfocountApiLatest = 1;

		// Token: 0x040001D1 RID: 465
		public const int GetlocalplatformtypeApiLatest = 1;

		// Token: 0x040001D2 RID: 466
		public const int MaxDisplaynameCharacters = 16;

		// Token: 0x040001D3 RID: 467
		public const int MaxDisplaynameUtf8Length = 64;

		// Token: 0x040001D4 RID: 468
		public const int QueryuserinfoApiLatest = 1;

		// Token: 0x040001D5 RID: 469
		public const int QueryuserinfobydisplaynameApiLatest = 1;

		// Token: 0x040001D6 RID: 470
		public const int QueryuserinfobyexternalaccountApiLatest = 1;
	}
}
