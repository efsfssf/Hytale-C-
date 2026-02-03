using System;

namespace Epic.OnlineServices.Sessions
{
	// Token: 0x020000D9 RID: 217
	public sealed class ActiveSession : Handle
	{
		// Token: 0x060007D7 RID: 2007 RVA: 0x0000B61F File Offset: 0x0000981F
		public ActiveSession()
		{
		}

		// Token: 0x060007D8 RID: 2008 RVA: 0x0000B629 File Offset: 0x00009829
		public ActiveSession(IntPtr innerHandle) : base(innerHandle)
		{
		}

		// Token: 0x060007D9 RID: 2009 RVA: 0x0000B634 File Offset: 0x00009834
		public Result CopyInfo(ref ActiveSessionCopyInfoOptions options, out ActiveSessionInfo? outActiveSessionInfo)
		{
			ActiveSessionCopyInfoOptionsInternal activeSessionCopyInfoOptionsInternal = default(ActiveSessionCopyInfoOptionsInternal);
			activeSessionCopyInfoOptionsInternal.Set(ref options);
			IntPtr zero = IntPtr.Zero;
			Result result = Bindings.EOS_ActiveSession_CopyInfo(base.InnerHandle, ref activeSessionCopyInfoOptionsInternal, ref zero);
			Helper.Dispose<ActiveSessionCopyInfoOptionsInternal>(ref activeSessionCopyInfoOptionsInternal);
			Helper.Get<ActiveSessionInfoInternal, ActiveSessionInfo>(zero, out outActiveSessionInfo);
			bool flag = outActiveSessionInfo != null;
			if (flag)
			{
				Bindings.EOS_ActiveSession_Info_Release(zero);
			}
			return result;
		}

		// Token: 0x060007DA RID: 2010 RVA: 0x0000B694 File Offset: 0x00009894
		public ProductUserId GetRegisteredPlayerByIndex(ref ActiveSessionGetRegisteredPlayerByIndexOptions options)
		{
			ActiveSessionGetRegisteredPlayerByIndexOptionsInternal activeSessionGetRegisteredPlayerByIndexOptionsInternal = default(ActiveSessionGetRegisteredPlayerByIndexOptionsInternal);
			activeSessionGetRegisteredPlayerByIndexOptionsInternal.Set(ref options);
			IntPtr from = Bindings.EOS_ActiveSession_GetRegisteredPlayerByIndex(base.InnerHandle, ref activeSessionGetRegisteredPlayerByIndexOptionsInternal);
			Helper.Dispose<ActiveSessionGetRegisteredPlayerByIndexOptionsInternal>(ref activeSessionGetRegisteredPlayerByIndexOptionsInternal);
			ProductUserId result;
			Helper.Get<ProductUserId>(from, out result);
			return result;
		}

		// Token: 0x060007DB RID: 2011 RVA: 0x0000B6D8 File Offset: 0x000098D8
		public uint GetRegisteredPlayerCount(ref ActiveSessionGetRegisteredPlayerCountOptions options)
		{
			ActiveSessionGetRegisteredPlayerCountOptionsInternal activeSessionGetRegisteredPlayerCountOptionsInternal = default(ActiveSessionGetRegisteredPlayerCountOptionsInternal);
			activeSessionGetRegisteredPlayerCountOptionsInternal.Set(ref options);
			uint result = Bindings.EOS_ActiveSession_GetRegisteredPlayerCount(base.InnerHandle, ref activeSessionGetRegisteredPlayerCountOptionsInternal);
			Helper.Dispose<ActiveSessionGetRegisteredPlayerCountOptionsInternal>(ref activeSessionGetRegisteredPlayerCountOptionsInternal);
			return result;
		}

		// Token: 0x060007DC RID: 2012 RVA: 0x0000B712 File Offset: 0x00009912
		public void Release()
		{
			Bindings.EOS_ActiveSession_Release(base.InnerHandle);
		}

		// Token: 0x040003C4 RID: 964
		public const int ActivesessionCopyinfoApiLatest = 1;

		// Token: 0x040003C5 RID: 965
		public const int ActivesessionGetregisteredplayerbyindexApiLatest = 1;

		// Token: 0x040003C6 RID: 966
		public const int ActivesessionGetregisteredplayercountApiLatest = 1;

		// Token: 0x040003C7 RID: 967
		public const int ActivesessionInfoApiLatest = 1;
	}
}
