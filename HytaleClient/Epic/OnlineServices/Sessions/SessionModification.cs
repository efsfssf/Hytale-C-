using System;

namespace Epic.OnlineServices.Sessions
{
	// Token: 0x02000160 RID: 352
	public sealed class SessionModification : Handle
	{
		// Token: 0x06000ABA RID: 2746 RVA: 0x0000F2E0 File Offset: 0x0000D4E0
		public SessionModification()
		{
		}

		// Token: 0x06000ABB RID: 2747 RVA: 0x0000F2EA File Offset: 0x0000D4EA
		public SessionModification(IntPtr innerHandle) : base(innerHandle)
		{
		}

		// Token: 0x06000ABC RID: 2748 RVA: 0x0000F2F8 File Offset: 0x0000D4F8
		public Result AddAttribute(ref SessionModificationAddAttributeOptions options)
		{
			SessionModificationAddAttributeOptionsInternal sessionModificationAddAttributeOptionsInternal = default(SessionModificationAddAttributeOptionsInternal);
			sessionModificationAddAttributeOptionsInternal.Set(ref options);
			Result result = Bindings.EOS_SessionModification_AddAttribute(base.InnerHandle, ref sessionModificationAddAttributeOptionsInternal);
			Helper.Dispose<SessionModificationAddAttributeOptionsInternal>(ref sessionModificationAddAttributeOptionsInternal);
			return result;
		}

		// Token: 0x06000ABD RID: 2749 RVA: 0x0000F332 File Offset: 0x0000D532
		public void Release()
		{
			Bindings.EOS_SessionModification_Release(base.InnerHandle);
		}

		// Token: 0x06000ABE RID: 2750 RVA: 0x0000F344 File Offset: 0x0000D544
		public Result RemoveAttribute(ref SessionModificationRemoveAttributeOptions options)
		{
			SessionModificationRemoveAttributeOptionsInternal sessionModificationRemoveAttributeOptionsInternal = default(SessionModificationRemoveAttributeOptionsInternal);
			sessionModificationRemoveAttributeOptionsInternal.Set(ref options);
			Result result = Bindings.EOS_SessionModification_RemoveAttribute(base.InnerHandle, ref sessionModificationRemoveAttributeOptionsInternal);
			Helper.Dispose<SessionModificationRemoveAttributeOptionsInternal>(ref sessionModificationRemoveAttributeOptionsInternal);
			return result;
		}

		// Token: 0x06000ABF RID: 2751 RVA: 0x0000F380 File Offset: 0x0000D580
		public Result SetAllowedPlatformIds(ref SessionModificationSetAllowedPlatformIdsOptions options)
		{
			SessionModificationSetAllowedPlatformIdsOptionsInternal sessionModificationSetAllowedPlatformIdsOptionsInternal = default(SessionModificationSetAllowedPlatformIdsOptionsInternal);
			sessionModificationSetAllowedPlatformIdsOptionsInternal.Set(ref options);
			Result result = Bindings.EOS_SessionModification_SetAllowedPlatformIds(base.InnerHandle, ref sessionModificationSetAllowedPlatformIdsOptionsInternal);
			Helper.Dispose<SessionModificationSetAllowedPlatformIdsOptionsInternal>(ref sessionModificationSetAllowedPlatformIdsOptionsInternal);
			return result;
		}

		// Token: 0x06000AC0 RID: 2752 RVA: 0x0000F3BC File Offset: 0x0000D5BC
		public Result SetBucketId(ref SessionModificationSetBucketIdOptions options)
		{
			SessionModificationSetBucketIdOptionsInternal sessionModificationSetBucketIdOptionsInternal = default(SessionModificationSetBucketIdOptionsInternal);
			sessionModificationSetBucketIdOptionsInternal.Set(ref options);
			Result result = Bindings.EOS_SessionModification_SetBucketId(base.InnerHandle, ref sessionModificationSetBucketIdOptionsInternal);
			Helper.Dispose<SessionModificationSetBucketIdOptionsInternal>(ref sessionModificationSetBucketIdOptionsInternal);
			return result;
		}

		// Token: 0x06000AC1 RID: 2753 RVA: 0x0000F3F8 File Offset: 0x0000D5F8
		public Result SetHostAddress(ref SessionModificationSetHostAddressOptions options)
		{
			SessionModificationSetHostAddressOptionsInternal sessionModificationSetHostAddressOptionsInternal = default(SessionModificationSetHostAddressOptionsInternal);
			sessionModificationSetHostAddressOptionsInternal.Set(ref options);
			Result result = Bindings.EOS_SessionModification_SetHostAddress(base.InnerHandle, ref sessionModificationSetHostAddressOptionsInternal);
			Helper.Dispose<SessionModificationSetHostAddressOptionsInternal>(ref sessionModificationSetHostAddressOptionsInternal);
			return result;
		}

		// Token: 0x06000AC2 RID: 2754 RVA: 0x0000F434 File Offset: 0x0000D634
		public Result SetInvitesAllowed(ref SessionModificationSetInvitesAllowedOptions options)
		{
			SessionModificationSetInvitesAllowedOptionsInternal sessionModificationSetInvitesAllowedOptionsInternal = default(SessionModificationSetInvitesAllowedOptionsInternal);
			sessionModificationSetInvitesAllowedOptionsInternal.Set(ref options);
			Result result = Bindings.EOS_SessionModification_SetInvitesAllowed(base.InnerHandle, ref sessionModificationSetInvitesAllowedOptionsInternal);
			Helper.Dispose<SessionModificationSetInvitesAllowedOptionsInternal>(ref sessionModificationSetInvitesAllowedOptionsInternal);
			return result;
		}

		// Token: 0x06000AC3 RID: 2755 RVA: 0x0000F470 File Offset: 0x0000D670
		public Result SetJoinInProgressAllowed(ref SessionModificationSetJoinInProgressAllowedOptions options)
		{
			SessionModificationSetJoinInProgressAllowedOptionsInternal sessionModificationSetJoinInProgressAllowedOptionsInternal = default(SessionModificationSetJoinInProgressAllowedOptionsInternal);
			sessionModificationSetJoinInProgressAllowedOptionsInternal.Set(ref options);
			Result result = Bindings.EOS_SessionModification_SetJoinInProgressAllowed(base.InnerHandle, ref sessionModificationSetJoinInProgressAllowedOptionsInternal);
			Helper.Dispose<SessionModificationSetJoinInProgressAllowedOptionsInternal>(ref sessionModificationSetJoinInProgressAllowedOptionsInternal);
			return result;
		}

		// Token: 0x06000AC4 RID: 2756 RVA: 0x0000F4AC File Offset: 0x0000D6AC
		public Result SetMaxPlayers(ref SessionModificationSetMaxPlayersOptions options)
		{
			SessionModificationSetMaxPlayersOptionsInternal sessionModificationSetMaxPlayersOptionsInternal = default(SessionModificationSetMaxPlayersOptionsInternal);
			sessionModificationSetMaxPlayersOptionsInternal.Set(ref options);
			Result result = Bindings.EOS_SessionModification_SetMaxPlayers(base.InnerHandle, ref sessionModificationSetMaxPlayersOptionsInternal);
			Helper.Dispose<SessionModificationSetMaxPlayersOptionsInternal>(ref sessionModificationSetMaxPlayersOptionsInternal);
			return result;
		}

		// Token: 0x06000AC5 RID: 2757 RVA: 0x0000F4E8 File Offset: 0x0000D6E8
		public Result SetPermissionLevel(ref SessionModificationSetPermissionLevelOptions options)
		{
			SessionModificationSetPermissionLevelOptionsInternal sessionModificationSetPermissionLevelOptionsInternal = default(SessionModificationSetPermissionLevelOptionsInternal);
			sessionModificationSetPermissionLevelOptionsInternal.Set(ref options);
			Result result = Bindings.EOS_SessionModification_SetPermissionLevel(base.InnerHandle, ref sessionModificationSetPermissionLevelOptionsInternal);
			Helper.Dispose<SessionModificationSetPermissionLevelOptionsInternal>(ref sessionModificationSetPermissionLevelOptionsInternal);
			return result;
		}

		// Token: 0x040004D9 RID: 1241
		public const int SessionmodificationAddattributeApiLatest = 2;

		// Token: 0x040004DA RID: 1242
		public const int SessionmodificationMaxSessionAttributeLength = 64;

		// Token: 0x040004DB RID: 1243
		public const int SessionmodificationMaxSessionAttributes = 64;

		// Token: 0x040004DC RID: 1244
		public const int SessionmodificationMaxSessionidoverrideLength = 64;

		// Token: 0x040004DD RID: 1245
		public const int SessionmodificationMinSessionidoverrideLength = 16;

		// Token: 0x040004DE RID: 1246
		public const int SessionmodificationRemoveattributeApiLatest = 1;

		// Token: 0x040004DF RID: 1247
		public const int SessionmodificationSetallowedplatformidsApiLatest = 1;

		// Token: 0x040004E0 RID: 1248
		public const int SessionmodificationSetbucketidApiLatest = 1;

		// Token: 0x040004E1 RID: 1249
		public const int SessionmodificationSethostaddressApiLatest = 1;

		// Token: 0x040004E2 RID: 1250
		public const int SessionmodificationSetinvitesallowedApiLatest = 1;

		// Token: 0x040004E3 RID: 1251
		public const int SessionmodificationSetjoininprogressallowedApiLatest = 1;

		// Token: 0x040004E4 RID: 1252
		public const int SessionmodificationSetmaxplayersApiLatest = 1;

		// Token: 0x040004E5 RID: 1253
		public const int SessionmodificationSetpermissionlevelApiLatest = 1;
	}
}
