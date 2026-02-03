using System;

namespace Epic.OnlineServices.Sanctions
{
	// Token: 0x020001AA RID: 426
	public sealed class SanctionsInterface : Handle
	{
		// Token: 0x06000C50 RID: 3152 RVA: 0x00011EF6 File Offset: 0x000100F6
		public SanctionsInterface()
		{
		}

		// Token: 0x06000C51 RID: 3153 RVA: 0x00011F00 File Offset: 0x00010100
		public SanctionsInterface(IntPtr innerHandle) : base(innerHandle)
		{
		}

		// Token: 0x06000C52 RID: 3154 RVA: 0x00011F0C File Offset: 0x0001010C
		public Result CopyPlayerSanctionByIndex(ref CopyPlayerSanctionByIndexOptions options, out PlayerSanction? outSanction)
		{
			CopyPlayerSanctionByIndexOptionsInternal copyPlayerSanctionByIndexOptionsInternal = default(CopyPlayerSanctionByIndexOptionsInternal);
			copyPlayerSanctionByIndexOptionsInternal.Set(ref options);
			IntPtr zero = IntPtr.Zero;
			Result result = Bindings.EOS_Sanctions_CopyPlayerSanctionByIndex(base.InnerHandle, ref copyPlayerSanctionByIndexOptionsInternal, ref zero);
			Helper.Dispose<CopyPlayerSanctionByIndexOptionsInternal>(ref copyPlayerSanctionByIndexOptionsInternal);
			Helper.Get<PlayerSanctionInternal, PlayerSanction>(zero, out outSanction);
			bool flag = outSanction != null;
			if (flag)
			{
				Bindings.EOS_Sanctions_PlayerSanction_Release(zero);
			}
			return result;
		}

		// Token: 0x06000C53 RID: 3155 RVA: 0x00011F6C File Offset: 0x0001016C
		public void CreatePlayerSanctionAppeal(ref CreatePlayerSanctionAppealOptions options, object clientData, CreatePlayerSanctionAppealCallback completionDelegate)
		{
			CreatePlayerSanctionAppealOptionsInternal createPlayerSanctionAppealOptionsInternal = default(CreatePlayerSanctionAppealOptionsInternal);
			createPlayerSanctionAppealOptionsInternal.Set(ref options);
			IntPtr zero = IntPtr.Zero;
			CreatePlayerSanctionAppealCallbackInternal createPlayerSanctionAppealCallbackInternal = new CreatePlayerSanctionAppealCallbackInternal(SanctionsInterface.CreatePlayerSanctionAppealCallbackInternalImplementation);
			Helper.AddCallback(out zero, clientData, completionDelegate, createPlayerSanctionAppealCallbackInternal, Array.Empty<Delegate>());
			Bindings.EOS_Sanctions_CreatePlayerSanctionAppeal(base.InnerHandle, ref createPlayerSanctionAppealOptionsInternal, zero, createPlayerSanctionAppealCallbackInternal);
			Helper.Dispose<CreatePlayerSanctionAppealOptionsInternal>(ref createPlayerSanctionAppealOptionsInternal);
		}

		// Token: 0x06000C54 RID: 3156 RVA: 0x00011FC8 File Offset: 0x000101C8
		public uint GetPlayerSanctionCount(ref GetPlayerSanctionCountOptions options)
		{
			GetPlayerSanctionCountOptionsInternal getPlayerSanctionCountOptionsInternal = default(GetPlayerSanctionCountOptionsInternal);
			getPlayerSanctionCountOptionsInternal.Set(ref options);
			uint result = Bindings.EOS_Sanctions_GetPlayerSanctionCount(base.InnerHandle, ref getPlayerSanctionCountOptionsInternal);
			Helper.Dispose<GetPlayerSanctionCountOptionsInternal>(ref getPlayerSanctionCountOptionsInternal);
			return result;
		}

		// Token: 0x06000C55 RID: 3157 RVA: 0x00012004 File Offset: 0x00010204
		public void QueryActivePlayerSanctions(ref QueryActivePlayerSanctionsOptions options, object clientData, OnQueryActivePlayerSanctionsCallback completionDelegate)
		{
			QueryActivePlayerSanctionsOptionsInternal queryActivePlayerSanctionsOptionsInternal = default(QueryActivePlayerSanctionsOptionsInternal);
			queryActivePlayerSanctionsOptionsInternal.Set(ref options);
			IntPtr zero = IntPtr.Zero;
			OnQueryActivePlayerSanctionsCallbackInternal onQueryActivePlayerSanctionsCallbackInternal = new OnQueryActivePlayerSanctionsCallbackInternal(SanctionsInterface.OnQueryActivePlayerSanctionsCallbackInternalImplementation);
			Helper.AddCallback(out zero, clientData, completionDelegate, onQueryActivePlayerSanctionsCallbackInternal, Array.Empty<Delegate>());
			Bindings.EOS_Sanctions_QueryActivePlayerSanctions(base.InnerHandle, ref queryActivePlayerSanctionsOptionsInternal, zero, onQueryActivePlayerSanctionsCallbackInternal);
			Helper.Dispose<QueryActivePlayerSanctionsOptionsInternal>(ref queryActivePlayerSanctionsOptionsInternal);
		}

		// Token: 0x06000C56 RID: 3158 RVA: 0x00012060 File Offset: 0x00010260
		[MonoPInvokeCallback(typeof(CreatePlayerSanctionAppealCallbackInternal))]
		internal static void CreatePlayerSanctionAppealCallbackInternalImplementation(ref CreatePlayerSanctionAppealCallbackInfoInternal data)
		{
			CreatePlayerSanctionAppealCallback createPlayerSanctionAppealCallback;
			CreatePlayerSanctionAppealCallbackInfo createPlayerSanctionAppealCallbackInfo;
			bool flag = Helper.TryGetAndRemoveCallback<CreatePlayerSanctionAppealCallbackInfoInternal, CreatePlayerSanctionAppealCallback, CreatePlayerSanctionAppealCallbackInfo>(ref data, out createPlayerSanctionAppealCallback, out createPlayerSanctionAppealCallbackInfo);
			if (flag)
			{
				createPlayerSanctionAppealCallback(ref createPlayerSanctionAppealCallbackInfo);
			}
		}

		// Token: 0x06000C57 RID: 3159 RVA: 0x00012088 File Offset: 0x00010288
		[MonoPInvokeCallback(typeof(OnQueryActivePlayerSanctionsCallbackInternal))]
		internal static void OnQueryActivePlayerSanctionsCallbackInternalImplementation(ref QueryActivePlayerSanctionsCallbackInfoInternal data)
		{
			OnQueryActivePlayerSanctionsCallback onQueryActivePlayerSanctionsCallback;
			QueryActivePlayerSanctionsCallbackInfo queryActivePlayerSanctionsCallbackInfo;
			bool flag = Helper.TryGetAndRemoveCallback<QueryActivePlayerSanctionsCallbackInfoInternal, OnQueryActivePlayerSanctionsCallback, QueryActivePlayerSanctionsCallbackInfo>(ref data, out onQueryActivePlayerSanctionsCallback, out queryActivePlayerSanctionsCallbackInfo);
			if (flag)
			{
				onQueryActivePlayerSanctionsCallback(ref queryActivePlayerSanctionsCallbackInfo);
			}
		}

		// Token: 0x040005A2 RID: 1442
		public const int CopyplayersanctionbyindexApiLatest = 1;

		// Token: 0x040005A3 RID: 1443
		public const int CreateplayersanctionappealApiLatest = 1;

		// Token: 0x040005A4 RID: 1444
		public const int GetplayersanctioncountApiLatest = 1;

		// Token: 0x040005A5 RID: 1445
		public const int PlayersanctionApiLatest = 2;

		// Token: 0x040005A6 RID: 1446
		public const int QueryactiveplayersanctionsApiLatest = 2;
	}
}
