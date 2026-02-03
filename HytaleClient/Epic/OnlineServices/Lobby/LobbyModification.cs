using System;

namespace Epic.OnlineServices.Lobby
{
	// Token: 0x020003DB RID: 987
	public sealed class LobbyModification : Handle
	{
		// Token: 0x06001AE2 RID: 6882 RVA: 0x0002858E File Offset: 0x0002678E
		public LobbyModification()
		{
		}

		// Token: 0x06001AE3 RID: 6883 RVA: 0x00028598 File Offset: 0x00026798
		public LobbyModification(IntPtr innerHandle) : base(innerHandle)
		{
		}

		// Token: 0x06001AE4 RID: 6884 RVA: 0x000285A4 File Offset: 0x000267A4
		public Result AddAttribute(ref LobbyModificationAddAttributeOptions options)
		{
			LobbyModificationAddAttributeOptionsInternal lobbyModificationAddAttributeOptionsInternal = default(LobbyModificationAddAttributeOptionsInternal);
			lobbyModificationAddAttributeOptionsInternal.Set(ref options);
			Result result = Bindings.EOS_LobbyModification_AddAttribute(base.InnerHandle, ref lobbyModificationAddAttributeOptionsInternal);
			Helper.Dispose<LobbyModificationAddAttributeOptionsInternal>(ref lobbyModificationAddAttributeOptionsInternal);
			return result;
		}

		// Token: 0x06001AE5 RID: 6885 RVA: 0x000285E0 File Offset: 0x000267E0
		public Result AddMemberAttribute(ref LobbyModificationAddMemberAttributeOptions options)
		{
			LobbyModificationAddMemberAttributeOptionsInternal lobbyModificationAddMemberAttributeOptionsInternal = default(LobbyModificationAddMemberAttributeOptionsInternal);
			lobbyModificationAddMemberAttributeOptionsInternal.Set(ref options);
			Result result = Bindings.EOS_LobbyModification_AddMemberAttribute(base.InnerHandle, ref lobbyModificationAddMemberAttributeOptionsInternal);
			Helper.Dispose<LobbyModificationAddMemberAttributeOptionsInternal>(ref lobbyModificationAddMemberAttributeOptionsInternal);
			return result;
		}

		// Token: 0x06001AE6 RID: 6886 RVA: 0x0002861A File Offset: 0x0002681A
		public void Release()
		{
			Bindings.EOS_LobbyModification_Release(base.InnerHandle);
		}

		// Token: 0x06001AE7 RID: 6887 RVA: 0x0002862C File Offset: 0x0002682C
		public Result RemoveAttribute(ref LobbyModificationRemoveAttributeOptions options)
		{
			LobbyModificationRemoveAttributeOptionsInternal lobbyModificationRemoveAttributeOptionsInternal = default(LobbyModificationRemoveAttributeOptionsInternal);
			lobbyModificationRemoveAttributeOptionsInternal.Set(ref options);
			Result result = Bindings.EOS_LobbyModification_RemoveAttribute(base.InnerHandle, ref lobbyModificationRemoveAttributeOptionsInternal);
			Helper.Dispose<LobbyModificationRemoveAttributeOptionsInternal>(ref lobbyModificationRemoveAttributeOptionsInternal);
			return result;
		}

		// Token: 0x06001AE8 RID: 6888 RVA: 0x00028668 File Offset: 0x00026868
		public Result RemoveMemberAttribute(ref LobbyModificationRemoveMemberAttributeOptions options)
		{
			LobbyModificationRemoveMemberAttributeOptionsInternal lobbyModificationRemoveMemberAttributeOptionsInternal = default(LobbyModificationRemoveMemberAttributeOptionsInternal);
			lobbyModificationRemoveMemberAttributeOptionsInternal.Set(ref options);
			Result result = Bindings.EOS_LobbyModification_RemoveMemberAttribute(base.InnerHandle, ref lobbyModificationRemoveMemberAttributeOptionsInternal);
			Helper.Dispose<LobbyModificationRemoveMemberAttributeOptionsInternal>(ref lobbyModificationRemoveMemberAttributeOptionsInternal);
			return result;
		}

		// Token: 0x06001AE9 RID: 6889 RVA: 0x000286A4 File Offset: 0x000268A4
		public Result SetAllowedPlatformIds(ref LobbyModificationSetAllowedPlatformIdsOptions options)
		{
			LobbyModificationSetAllowedPlatformIdsOptionsInternal lobbyModificationSetAllowedPlatformIdsOptionsInternal = default(LobbyModificationSetAllowedPlatformIdsOptionsInternal);
			lobbyModificationSetAllowedPlatformIdsOptionsInternal.Set(ref options);
			Result result = Bindings.EOS_LobbyModification_SetAllowedPlatformIds(base.InnerHandle, ref lobbyModificationSetAllowedPlatformIdsOptionsInternal);
			Helper.Dispose<LobbyModificationSetAllowedPlatformIdsOptionsInternal>(ref lobbyModificationSetAllowedPlatformIdsOptionsInternal);
			return result;
		}

		// Token: 0x06001AEA RID: 6890 RVA: 0x000286E0 File Offset: 0x000268E0
		public Result SetBucketId(ref LobbyModificationSetBucketIdOptions options)
		{
			LobbyModificationSetBucketIdOptionsInternal lobbyModificationSetBucketIdOptionsInternal = default(LobbyModificationSetBucketIdOptionsInternal);
			lobbyModificationSetBucketIdOptionsInternal.Set(ref options);
			Result result = Bindings.EOS_LobbyModification_SetBucketId(base.InnerHandle, ref lobbyModificationSetBucketIdOptionsInternal);
			Helper.Dispose<LobbyModificationSetBucketIdOptionsInternal>(ref lobbyModificationSetBucketIdOptionsInternal);
			return result;
		}

		// Token: 0x06001AEB RID: 6891 RVA: 0x0002871C File Offset: 0x0002691C
		public Result SetInvitesAllowed(ref LobbyModificationSetInvitesAllowedOptions options)
		{
			LobbyModificationSetInvitesAllowedOptionsInternal lobbyModificationSetInvitesAllowedOptionsInternal = default(LobbyModificationSetInvitesAllowedOptionsInternal);
			lobbyModificationSetInvitesAllowedOptionsInternal.Set(ref options);
			Result result = Bindings.EOS_LobbyModification_SetInvitesAllowed(base.InnerHandle, ref lobbyModificationSetInvitesAllowedOptionsInternal);
			Helper.Dispose<LobbyModificationSetInvitesAllowedOptionsInternal>(ref lobbyModificationSetInvitesAllowedOptionsInternal);
			return result;
		}

		// Token: 0x06001AEC RID: 6892 RVA: 0x00028758 File Offset: 0x00026958
		public Result SetMaxMembers(ref LobbyModificationSetMaxMembersOptions options)
		{
			LobbyModificationSetMaxMembersOptionsInternal lobbyModificationSetMaxMembersOptionsInternal = default(LobbyModificationSetMaxMembersOptionsInternal);
			lobbyModificationSetMaxMembersOptionsInternal.Set(ref options);
			Result result = Bindings.EOS_LobbyModification_SetMaxMembers(base.InnerHandle, ref lobbyModificationSetMaxMembersOptionsInternal);
			Helper.Dispose<LobbyModificationSetMaxMembersOptionsInternal>(ref lobbyModificationSetMaxMembersOptionsInternal);
			return result;
		}

		// Token: 0x06001AED RID: 6893 RVA: 0x00028794 File Offset: 0x00026994
		public Result SetPermissionLevel(ref LobbyModificationSetPermissionLevelOptions options)
		{
			LobbyModificationSetPermissionLevelOptionsInternal lobbyModificationSetPermissionLevelOptionsInternal = default(LobbyModificationSetPermissionLevelOptionsInternal);
			lobbyModificationSetPermissionLevelOptionsInternal.Set(ref options);
			Result result = Bindings.EOS_LobbyModification_SetPermissionLevel(base.InnerHandle, ref lobbyModificationSetPermissionLevelOptionsInternal);
			Helper.Dispose<LobbyModificationSetPermissionLevelOptionsInternal>(ref lobbyModificationSetPermissionLevelOptionsInternal);
			return result;
		}

		// Token: 0x04000BFF RID: 3071
		public const int LobbymodificationAddattributeApiLatest = 2;

		// Token: 0x04000C00 RID: 3072
		public const int LobbymodificationAddmemberattributeApiLatest = 2;

		// Token: 0x04000C01 RID: 3073
		public const int LobbymodificationMaxAttributeLength = 64;

		// Token: 0x04000C02 RID: 3074
		public const int LobbymodificationMaxAttributes = 64;

		// Token: 0x04000C03 RID: 3075
		public const int LobbymodificationRemoveattributeApiLatest = 1;

		// Token: 0x04000C04 RID: 3076
		public const int LobbymodificationRemovememberattributeApiLatest = 1;

		// Token: 0x04000C05 RID: 3077
		public const int LobbymodificationSetallowedplatformidsApiLatest = 1;

		// Token: 0x04000C06 RID: 3078
		public const int LobbymodificationSetbucketidApiLatest = 1;

		// Token: 0x04000C07 RID: 3079
		public const int LobbymodificationSetinvitesallowedApiLatest = 1;

		// Token: 0x04000C08 RID: 3080
		public const int LobbymodificationSetmaxmembersApiLatest = 1;

		// Token: 0x04000C09 RID: 3081
		public const int LobbymodificationSetpermissionlevelApiLatest = 1;
	}
}
