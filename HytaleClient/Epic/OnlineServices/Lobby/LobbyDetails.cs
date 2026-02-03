using System;

namespace Epic.OnlineServices.Lobby
{
	// Token: 0x020003B4 RID: 948
	public sealed class LobbyDetails : Handle
	{
		// Token: 0x0600198F RID: 6543 RVA: 0x0002572E File Offset: 0x0002392E
		public LobbyDetails()
		{
		}

		// Token: 0x06001990 RID: 6544 RVA: 0x00025738 File Offset: 0x00023938
		public LobbyDetails(IntPtr innerHandle) : base(innerHandle)
		{
		}

		// Token: 0x06001991 RID: 6545 RVA: 0x00025744 File Offset: 0x00023944
		public Result CopyAttributeByIndex(ref LobbyDetailsCopyAttributeByIndexOptions options, out Attribute? outAttribute)
		{
			LobbyDetailsCopyAttributeByIndexOptionsInternal lobbyDetailsCopyAttributeByIndexOptionsInternal = default(LobbyDetailsCopyAttributeByIndexOptionsInternal);
			lobbyDetailsCopyAttributeByIndexOptionsInternal.Set(ref options);
			IntPtr zero = IntPtr.Zero;
			Result result = Bindings.EOS_LobbyDetails_CopyAttributeByIndex(base.InnerHandle, ref lobbyDetailsCopyAttributeByIndexOptionsInternal, ref zero);
			Helper.Dispose<LobbyDetailsCopyAttributeByIndexOptionsInternal>(ref lobbyDetailsCopyAttributeByIndexOptionsInternal);
			Helper.Get<AttributeInternal, Attribute>(zero, out outAttribute);
			bool flag = outAttribute != null;
			if (flag)
			{
				Bindings.EOS_Lobby_Attribute_Release(zero);
			}
			return result;
		}

		// Token: 0x06001992 RID: 6546 RVA: 0x000257A4 File Offset: 0x000239A4
		public Result CopyAttributeByKey(ref LobbyDetailsCopyAttributeByKeyOptions options, out Attribute? outAttribute)
		{
			LobbyDetailsCopyAttributeByKeyOptionsInternal lobbyDetailsCopyAttributeByKeyOptionsInternal = default(LobbyDetailsCopyAttributeByKeyOptionsInternal);
			lobbyDetailsCopyAttributeByKeyOptionsInternal.Set(ref options);
			IntPtr zero = IntPtr.Zero;
			Result result = Bindings.EOS_LobbyDetails_CopyAttributeByKey(base.InnerHandle, ref lobbyDetailsCopyAttributeByKeyOptionsInternal, ref zero);
			Helper.Dispose<LobbyDetailsCopyAttributeByKeyOptionsInternal>(ref lobbyDetailsCopyAttributeByKeyOptionsInternal);
			Helper.Get<AttributeInternal, Attribute>(zero, out outAttribute);
			bool flag = outAttribute != null;
			if (flag)
			{
				Bindings.EOS_Lobby_Attribute_Release(zero);
			}
			return result;
		}

		// Token: 0x06001993 RID: 6547 RVA: 0x00025804 File Offset: 0x00023A04
		public Result CopyInfo(ref LobbyDetailsCopyInfoOptions options, out LobbyDetailsInfo? outLobbyDetailsInfo)
		{
			LobbyDetailsCopyInfoOptionsInternal lobbyDetailsCopyInfoOptionsInternal = default(LobbyDetailsCopyInfoOptionsInternal);
			lobbyDetailsCopyInfoOptionsInternal.Set(ref options);
			IntPtr zero = IntPtr.Zero;
			Result result = Bindings.EOS_LobbyDetails_CopyInfo(base.InnerHandle, ref lobbyDetailsCopyInfoOptionsInternal, ref zero);
			Helper.Dispose<LobbyDetailsCopyInfoOptionsInternal>(ref lobbyDetailsCopyInfoOptionsInternal);
			Helper.Get<LobbyDetailsInfoInternal, LobbyDetailsInfo>(zero, out outLobbyDetailsInfo);
			bool flag = outLobbyDetailsInfo != null;
			if (flag)
			{
				Bindings.EOS_LobbyDetails_Info_Release(zero);
			}
			return result;
		}

		// Token: 0x06001994 RID: 6548 RVA: 0x00025864 File Offset: 0x00023A64
		public Result CopyMemberAttributeByIndex(ref LobbyDetailsCopyMemberAttributeByIndexOptions options, out Attribute? outAttribute)
		{
			LobbyDetailsCopyMemberAttributeByIndexOptionsInternal lobbyDetailsCopyMemberAttributeByIndexOptionsInternal = default(LobbyDetailsCopyMemberAttributeByIndexOptionsInternal);
			lobbyDetailsCopyMemberAttributeByIndexOptionsInternal.Set(ref options);
			IntPtr zero = IntPtr.Zero;
			Result result = Bindings.EOS_LobbyDetails_CopyMemberAttributeByIndex(base.InnerHandle, ref lobbyDetailsCopyMemberAttributeByIndexOptionsInternal, ref zero);
			Helper.Dispose<LobbyDetailsCopyMemberAttributeByIndexOptionsInternal>(ref lobbyDetailsCopyMemberAttributeByIndexOptionsInternal);
			Helper.Get<AttributeInternal, Attribute>(zero, out outAttribute);
			bool flag = outAttribute != null;
			if (flag)
			{
				Bindings.EOS_Lobby_Attribute_Release(zero);
			}
			return result;
		}

		// Token: 0x06001995 RID: 6549 RVA: 0x000258C4 File Offset: 0x00023AC4
		public Result CopyMemberAttributeByKey(ref LobbyDetailsCopyMemberAttributeByKeyOptions options, out Attribute? outAttribute)
		{
			LobbyDetailsCopyMemberAttributeByKeyOptionsInternal lobbyDetailsCopyMemberAttributeByKeyOptionsInternal = default(LobbyDetailsCopyMemberAttributeByKeyOptionsInternal);
			lobbyDetailsCopyMemberAttributeByKeyOptionsInternal.Set(ref options);
			IntPtr zero = IntPtr.Zero;
			Result result = Bindings.EOS_LobbyDetails_CopyMemberAttributeByKey(base.InnerHandle, ref lobbyDetailsCopyMemberAttributeByKeyOptionsInternal, ref zero);
			Helper.Dispose<LobbyDetailsCopyMemberAttributeByKeyOptionsInternal>(ref lobbyDetailsCopyMemberAttributeByKeyOptionsInternal);
			Helper.Get<AttributeInternal, Attribute>(zero, out outAttribute);
			bool flag = outAttribute != null;
			if (flag)
			{
				Bindings.EOS_Lobby_Attribute_Release(zero);
			}
			return result;
		}

		// Token: 0x06001996 RID: 6550 RVA: 0x00025924 File Offset: 0x00023B24
		public Result CopyMemberInfo(ref LobbyDetailsCopyMemberInfoOptions options, out LobbyDetailsMemberInfo? outLobbyDetailsMemberInfo)
		{
			LobbyDetailsCopyMemberInfoOptionsInternal lobbyDetailsCopyMemberInfoOptionsInternal = default(LobbyDetailsCopyMemberInfoOptionsInternal);
			lobbyDetailsCopyMemberInfoOptionsInternal.Set(ref options);
			IntPtr zero = IntPtr.Zero;
			Result result = Bindings.EOS_LobbyDetails_CopyMemberInfo(base.InnerHandle, ref lobbyDetailsCopyMemberInfoOptionsInternal, ref zero);
			Helper.Dispose<LobbyDetailsCopyMemberInfoOptionsInternal>(ref lobbyDetailsCopyMemberInfoOptionsInternal);
			Helper.Get<LobbyDetailsMemberInfoInternal, LobbyDetailsMemberInfo>(zero, out outLobbyDetailsMemberInfo);
			bool flag = outLobbyDetailsMemberInfo != null;
			if (flag)
			{
				Bindings.EOS_LobbyDetails_MemberInfo_Release(zero);
			}
			return result;
		}

		// Token: 0x06001997 RID: 6551 RVA: 0x00025984 File Offset: 0x00023B84
		public uint GetAttributeCount(ref LobbyDetailsGetAttributeCountOptions options)
		{
			LobbyDetailsGetAttributeCountOptionsInternal lobbyDetailsGetAttributeCountOptionsInternal = default(LobbyDetailsGetAttributeCountOptionsInternal);
			lobbyDetailsGetAttributeCountOptionsInternal.Set(ref options);
			uint result = Bindings.EOS_LobbyDetails_GetAttributeCount(base.InnerHandle, ref lobbyDetailsGetAttributeCountOptionsInternal);
			Helper.Dispose<LobbyDetailsGetAttributeCountOptionsInternal>(ref lobbyDetailsGetAttributeCountOptionsInternal);
			return result;
		}

		// Token: 0x06001998 RID: 6552 RVA: 0x000259C0 File Offset: 0x00023BC0
		public ProductUserId GetLobbyOwner(ref LobbyDetailsGetLobbyOwnerOptions options)
		{
			LobbyDetailsGetLobbyOwnerOptionsInternal lobbyDetailsGetLobbyOwnerOptionsInternal = default(LobbyDetailsGetLobbyOwnerOptionsInternal);
			lobbyDetailsGetLobbyOwnerOptionsInternal.Set(ref options);
			IntPtr from = Bindings.EOS_LobbyDetails_GetLobbyOwner(base.InnerHandle, ref lobbyDetailsGetLobbyOwnerOptionsInternal);
			Helper.Dispose<LobbyDetailsGetLobbyOwnerOptionsInternal>(ref lobbyDetailsGetLobbyOwnerOptionsInternal);
			ProductUserId result;
			Helper.Get<ProductUserId>(from, out result);
			return result;
		}

		// Token: 0x06001999 RID: 6553 RVA: 0x00025A04 File Offset: 0x00023C04
		public uint GetMemberAttributeCount(ref LobbyDetailsGetMemberAttributeCountOptions options)
		{
			LobbyDetailsGetMemberAttributeCountOptionsInternal lobbyDetailsGetMemberAttributeCountOptionsInternal = default(LobbyDetailsGetMemberAttributeCountOptionsInternal);
			lobbyDetailsGetMemberAttributeCountOptionsInternal.Set(ref options);
			uint result = Bindings.EOS_LobbyDetails_GetMemberAttributeCount(base.InnerHandle, ref lobbyDetailsGetMemberAttributeCountOptionsInternal);
			Helper.Dispose<LobbyDetailsGetMemberAttributeCountOptionsInternal>(ref lobbyDetailsGetMemberAttributeCountOptionsInternal);
			return result;
		}

		// Token: 0x0600199A RID: 6554 RVA: 0x00025A40 File Offset: 0x00023C40
		public ProductUserId GetMemberByIndex(ref LobbyDetailsGetMemberByIndexOptions options)
		{
			LobbyDetailsGetMemberByIndexOptionsInternal lobbyDetailsGetMemberByIndexOptionsInternal = default(LobbyDetailsGetMemberByIndexOptionsInternal);
			lobbyDetailsGetMemberByIndexOptionsInternal.Set(ref options);
			IntPtr from = Bindings.EOS_LobbyDetails_GetMemberByIndex(base.InnerHandle, ref lobbyDetailsGetMemberByIndexOptionsInternal);
			Helper.Dispose<LobbyDetailsGetMemberByIndexOptionsInternal>(ref lobbyDetailsGetMemberByIndexOptionsInternal);
			ProductUserId result;
			Helper.Get<ProductUserId>(from, out result);
			return result;
		}

		// Token: 0x0600199B RID: 6555 RVA: 0x00025A84 File Offset: 0x00023C84
		public uint GetMemberCount(ref LobbyDetailsGetMemberCountOptions options)
		{
			LobbyDetailsGetMemberCountOptionsInternal lobbyDetailsGetMemberCountOptionsInternal = default(LobbyDetailsGetMemberCountOptionsInternal);
			lobbyDetailsGetMemberCountOptionsInternal.Set(ref options);
			uint result = Bindings.EOS_LobbyDetails_GetMemberCount(base.InnerHandle, ref lobbyDetailsGetMemberCountOptionsInternal);
			Helper.Dispose<LobbyDetailsGetMemberCountOptionsInternal>(ref lobbyDetailsGetMemberCountOptionsInternal);
			return result;
		}

		// Token: 0x0600199C RID: 6556 RVA: 0x00025ABE File Offset: 0x00023CBE
		public void Release()
		{
			Bindings.EOS_LobbyDetails_Release(base.InnerHandle);
		}

		// Token: 0x04000B50 RID: 2896
		public const int LobbydetailsCopyattributebyindexApiLatest = 1;

		// Token: 0x04000B51 RID: 2897
		public const int LobbydetailsCopyattributebykeyApiLatest = 1;

		// Token: 0x04000B52 RID: 2898
		public const int LobbydetailsCopyinfoApiLatest = 1;

		// Token: 0x04000B53 RID: 2899
		public const int LobbydetailsCopymemberattributebyindexApiLatest = 1;

		// Token: 0x04000B54 RID: 2900
		public const int LobbydetailsCopymemberattributebykeyApiLatest = 1;

		// Token: 0x04000B55 RID: 2901
		public const int LobbydetailsCopymemberinfoApiLatest = 1;

		// Token: 0x04000B56 RID: 2902
		public const int LobbydetailsGetattributecountApiLatest = 1;

		// Token: 0x04000B57 RID: 2903
		public const int LobbydetailsGetlobbyownerApiLatest = 1;

		// Token: 0x04000B58 RID: 2904
		public const int LobbydetailsGetmemberattributecountApiLatest = 1;

		// Token: 0x04000B59 RID: 2905
		public const int LobbydetailsGetmemberbyindexApiLatest = 1;

		// Token: 0x04000B5A RID: 2906
		public const int LobbydetailsGetmembercountApiLatest = 1;

		// Token: 0x04000B5B RID: 2907
		public const int LobbydetailsInfoApiLatest = 3;

		// Token: 0x04000B5C RID: 2908
		public const int LobbydetailsMemberinfoApiLatest = 1;
	}
}
