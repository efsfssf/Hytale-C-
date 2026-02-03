using System;

namespace Epic.OnlineServices.Stats
{
	// Token: 0x020000D8 RID: 216
	public sealed class StatsInterface : Handle
	{
		// Token: 0x060007CE RID: 1998 RVA: 0x0000B406 File Offset: 0x00009606
		public StatsInterface()
		{
		}

		// Token: 0x060007CF RID: 1999 RVA: 0x0000B410 File Offset: 0x00009610
		public StatsInterface(IntPtr innerHandle) : base(innerHandle)
		{
		}

		// Token: 0x060007D0 RID: 2000 RVA: 0x0000B41C File Offset: 0x0000961C
		public Result CopyStatByIndex(ref CopyStatByIndexOptions options, out Stat? outStat)
		{
			CopyStatByIndexOptionsInternal copyStatByIndexOptionsInternal = default(CopyStatByIndexOptionsInternal);
			copyStatByIndexOptionsInternal.Set(ref options);
			IntPtr zero = IntPtr.Zero;
			Result result = Bindings.EOS_Stats_CopyStatByIndex(base.InnerHandle, ref copyStatByIndexOptionsInternal, ref zero);
			Helper.Dispose<CopyStatByIndexOptionsInternal>(ref copyStatByIndexOptionsInternal);
			Helper.Get<StatInternal, Stat>(zero, out outStat);
			bool flag = outStat != null;
			if (flag)
			{
				Bindings.EOS_Stats_Stat_Release(zero);
			}
			return result;
		}

		// Token: 0x060007D1 RID: 2001 RVA: 0x0000B47C File Offset: 0x0000967C
		public Result CopyStatByName(ref CopyStatByNameOptions options, out Stat? outStat)
		{
			CopyStatByNameOptionsInternal copyStatByNameOptionsInternal = default(CopyStatByNameOptionsInternal);
			copyStatByNameOptionsInternal.Set(ref options);
			IntPtr zero = IntPtr.Zero;
			Result result = Bindings.EOS_Stats_CopyStatByName(base.InnerHandle, ref copyStatByNameOptionsInternal, ref zero);
			Helper.Dispose<CopyStatByNameOptionsInternal>(ref copyStatByNameOptionsInternal);
			Helper.Get<StatInternal, Stat>(zero, out outStat);
			bool flag = outStat != null;
			if (flag)
			{
				Bindings.EOS_Stats_Stat_Release(zero);
			}
			return result;
		}

		// Token: 0x060007D2 RID: 2002 RVA: 0x0000B4DC File Offset: 0x000096DC
		public uint GetStatsCount(ref GetStatCountOptions options)
		{
			GetStatCountOptionsInternal getStatCountOptionsInternal = default(GetStatCountOptionsInternal);
			getStatCountOptionsInternal.Set(ref options);
			uint result = Bindings.EOS_Stats_GetStatsCount(base.InnerHandle, ref getStatCountOptionsInternal);
			Helper.Dispose<GetStatCountOptionsInternal>(ref getStatCountOptionsInternal);
			return result;
		}

		// Token: 0x060007D3 RID: 2003 RVA: 0x0000B518 File Offset: 0x00009718
		public void IngestStat(ref IngestStatOptions options, object clientData, OnIngestStatCompleteCallback completionDelegate)
		{
			IngestStatOptionsInternal ingestStatOptionsInternal = default(IngestStatOptionsInternal);
			ingestStatOptionsInternal.Set(ref options);
			IntPtr zero = IntPtr.Zero;
			OnIngestStatCompleteCallbackInternal onIngestStatCompleteCallbackInternal = new OnIngestStatCompleteCallbackInternal(StatsInterface.OnIngestStatCompleteCallbackInternalImplementation);
			Helper.AddCallback(out zero, clientData, completionDelegate, onIngestStatCompleteCallbackInternal, Array.Empty<Delegate>());
			Bindings.EOS_Stats_IngestStat(base.InnerHandle, ref ingestStatOptionsInternal, zero, onIngestStatCompleteCallbackInternal);
			Helper.Dispose<IngestStatOptionsInternal>(ref ingestStatOptionsInternal);
		}

		// Token: 0x060007D4 RID: 2004 RVA: 0x0000B574 File Offset: 0x00009774
		public void QueryStats(ref QueryStatsOptions options, object clientData, OnQueryStatsCompleteCallback completionDelegate)
		{
			QueryStatsOptionsInternal queryStatsOptionsInternal = default(QueryStatsOptionsInternal);
			queryStatsOptionsInternal.Set(ref options);
			IntPtr zero = IntPtr.Zero;
			OnQueryStatsCompleteCallbackInternal onQueryStatsCompleteCallbackInternal = new OnQueryStatsCompleteCallbackInternal(StatsInterface.OnQueryStatsCompleteCallbackInternalImplementation);
			Helper.AddCallback(out zero, clientData, completionDelegate, onQueryStatsCompleteCallbackInternal, Array.Empty<Delegate>());
			Bindings.EOS_Stats_QueryStats(base.InnerHandle, ref queryStatsOptionsInternal, zero, onQueryStatsCompleteCallbackInternal);
			Helper.Dispose<QueryStatsOptionsInternal>(ref queryStatsOptionsInternal);
		}

		// Token: 0x060007D5 RID: 2005 RVA: 0x0000B5D0 File Offset: 0x000097D0
		[MonoPInvokeCallback(typeof(OnIngestStatCompleteCallbackInternal))]
		internal static void OnIngestStatCompleteCallbackInternalImplementation(ref IngestStatCompleteCallbackInfoInternal data)
		{
			OnIngestStatCompleteCallback onIngestStatCompleteCallback;
			IngestStatCompleteCallbackInfo ingestStatCompleteCallbackInfo;
			bool flag = Helper.TryGetAndRemoveCallback<IngestStatCompleteCallbackInfoInternal, OnIngestStatCompleteCallback, IngestStatCompleteCallbackInfo>(ref data, out onIngestStatCompleteCallback, out ingestStatCompleteCallbackInfo);
			if (flag)
			{
				onIngestStatCompleteCallback(ref ingestStatCompleteCallbackInfo);
			}
		}

		// Token: 0x060007D6 RID: 2006 RVA: 0x0000B5F8 File Offset: 0x000097F8
		[MonoPInvokeCallback(typeof(OnQueryStatsCompleteCallbackInternal))]
		internal static void OnQueryStatsCompleteCallbackInternalImplementation(ref OnQueryStatsCompleteCallbackInfoInternal data)
		{
			OnQueryStatsCompleteCallback onQueryStatsCompleteCallback;
			OnQueryStatsCompleteCallbackInfo onQueryStatsCompleteCallbackInfo;
			bool flag = Helper.TryGetAndRemoveCallback<OnQueryStatsCompleteCallbackInfoInternal, OnQueryStatsCompleteCallback, OnQueryStatsCompleteCallbackInfo>(ref data, out onQueryStatsCompleteCallback, out onQueryStatsCompleteCallbackInfo);
			if (flag)
			{
				onQueryStatsCompleteCallback(ref onQueryStatsCompleteCallbackInfo);
			}
		}

		// Token: 0x040003B9 RID: 953
		public const int CopystatbyindexApiLatest = 1;

		// Token: 0x040003BA RID: 954
		public const int CopystatbynameApiLatest = 1;

		// Token: 0x040003BB RID: 955
		public const int GetstatcountApiLatest = 1;

		// Token: 0x040003BC RID: 956
		public const int GetstatscountApiLatest = 1;

		// Token: 0x040003BD RID: 957
		public const int IngestdataApiLatest = 1;

		// Token: 0x040003BE RID: 958
		public const int IngeststatApiLatest = 3;

		// Token: 0x040003BF RID: 959
		public const int MaxIngestStats = 3000;

		// Token: 0x040003C0 RID: 960
		public const int MaxQueryStats = 1000;

		// Token: 0x040003C1 RID: 961
		public const int QuerystatsApiLatest = 3;

		// Token: 0x040003C2 RID: 962
		public const int StatApiLatest = 1;

		// Token: 0x040003C3 RID: 963
		public const int TimeUndefined = -1;
	}
}
