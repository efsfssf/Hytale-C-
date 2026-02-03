using System;

namespace Epic.OnlineServices.Leaderboards
{
	// Token: 0x0200046C RID: 1132
	public sealed class LeaderboardsInterface : Handle
	{
		// Token: 0x06001D9D RID: 7581 RVA: 0x0002B536 File Offset: 0x00029736
		public LeaderboardsInterface()
		{
		}

		// Token: 0x06001D9E RID: 7582 RVA: 0x0002B540 File Offset: 0x00029740
		public LeaderboardsInterface(IntPtr innerHandle) : base(innerHandle)
		{
		}

		// Token: 0x06001D9F RID: 7583 RVA: 0x0002B54C File Offset: 0x0002974C
		public Result CopyLeaderboardDefinitionByIndex(ref CopyLeaderboardDefinitionByIndexOptions options, out Definition? outLeaderboardDefinition)
		{
			CopyLeaderboardDefinitionByIndexOptionsInternal copyLeaderboardDefinitionByIndexOptionsInternal = default(CopyLeaderboardDefinitionByIndexOptionsInternal);
			copyLeaderboardDefinitionByIndexOptionsInternal.Set(ref options);
			IntPtr zero = IntPtr.Zero;
			Result result = Bindings.EOS_Leaderboards_CopyLeaderboardDefinitionByIndex(base.InnerHandle, ref copyLeaderboardDefinitionByIndexOptionsInternal, ref zero);
			Helper.Dispose<CopyLeaderboardDefinitionByIndexOptionsInternal>(ref copyLeaderboardDefinitionByIndexOptionsInternal);
			Helper.Get<DefinitionInternal, Definition>(zero, out outLeaderboardDefinition);
			bool flag = outLeaderboardDefinition != null;
			if (flag)
			{
				Bindings.EOS_Leaderboards_Definition_Release(zero);
			}
			return result;
		}

		// Token: 0x06001DA0 RID: 7584 RVA: 0x0002B5AC File Offset: 0x000297AC
		public Result CopyLeaderboardDefinitionByLeaderboardId(ref CopyLeaderboardDefinitionByLeaderboardIdOptions options, out Definition? outLeaderboardDefinition)
		{
			CopyLeaderboardDefinitionByLeaderboardIdOptionsInternal copyLeaderboardDefinitionByLeaderboardIdOptionsInternal = default(CopyLeaderboardDefinitionByLeaderboardIdOptionsInternal);
			copyLeaderboardDefinitionByLeaderboardIdOptionsInternal.Set(ref options);
			IntPtr zero = IntPtr.Zero;
			Result result = Bindings.EOS_Leaderboards_CopyLeaderboardDefinitionByLeaderboardId(base.InnerHandle, ref copyLeaderboardDefinitionByLeaderboardIdOptionsInternal, ref zero);
			Helper.Dispose<CopyLeaderboardDefinitionByLeaderboardIdOptionsInternal>(ref copyLeaderboardDefinitionByLeaderboardIdOptionsInternal);
			Helper.Get<DefinitionInternal, Definition>(zero, out outLeaderboardDefinition);
			bool flag = outLeaderboardDefinition != null;
			if (flag)
			{
				Bindings.EOS_Leaderboards_Definition_Release(zero);
			}
			return result;
		}

		// Token: 0x06001DA1 RID: 7585 RVA: 0x0002B60C File Offset: 0x0002980C
		public Result CopyLeaderboardRecordByIndex(ref CopyLeaderboardRecordByIndexOptions options, out LeaderboardRecord? outLeaderboardRecord)
		{
			CopyLeaderboardRecordByIndexOptionsInternal copyLeaderboardRecordByIndexOptionsInternal = default(CopyLeaderboardRecordByIndexOptionsInternal);
			copyLeaderboardRecordByIndexOptionsInternal.Set(ref options);
			IntPtr zero = IntPtr.Zero;
			Result result = Bindings.EOS_Leaderboards_CopyLeaderboardRecordByIndex(base.InnerHandle, ref copyLeaderboardRecordByIndexOptionsInternal, ref zero);
			Helper.Dispose<CopyLeaderboardRecordByIndexOptionsInternal>(ref copyLeaderboardRecordByIndexOptionsInternal);
			Helper.Get<LeaderboardRecordInternal, LeaderboardRecord>(zero, out outLeaderboardRecord);
			bool flag = outLeaderboardRecord != null;
			if (flag)
			{
				Bindings.EOS_Leaderboards_LeaderboardRecord_Release(zero);
			}
			return result;
		}

		// Token: 0x06001DA2 RID: 7586 RVA: 0x0002B66C File Offset: 0x0002986C
		public Result CopyLeaderboardRecordByUserId(ref CopyLeaderboardRecordByUserIdOptions options, out LeaderboardRecord? outLeaderboardRecord)
		{
			CopyLeaderboardRecordByUserIdOptionsInternal copyLeaderboardRecordByUserIdOptionsInternal = default(CopyLeaderboardRecordByUserIdOptionsInternal);
			copyLeaderboardRecordByUserIdOptionsInternal.Set(ref options);
			IntPtr zero = IntPtr.Zero;
			Result result = Bindings.EOS_Leaderboards_CopyLeaderboardRecordByUserId(base.InnerHandle, ref copyLeaderboardRecordByUserIdOptionsInternal, ref zero);
			Helper.Dispose<CopyLeaderboardRecordByUserIdOptionsInternal>(ref copyLeaderboardRecordByUserIdOptionsInternal);
			Helper.Get<LeaderboardRecordInternal, LeaderboardRecord>(zero, out outLeaderboardRecord);
			bool flag = outLeaderboardRecord != null;
			if (flag)
			{
				Bindings.EOS_Leaderboards_LeaderboardRecord_Release(zero);
			}
			return result;
		}

		// Token: 0x06001DA3 RID: 7587 RVA: 0x0002B6CC File Offset: 0x000298CC
		public Result CopyLeaderboardUserScoreByIndex(ref CopyLeaderboardUserScoreByIndexOptions options, out LeaderboardUserScore? outLeaderboardUserScore)
		{
			CopyLeaderboardUserScoreByIndexOptionsInternal copyLeaderboardUserScoreByIndexOptionsInternal = default(CopyLeaderboardUserScoreByIndexOptionsInternal);
			copyLeaderboardUserScoreByIndexOptionsInternal.Set(ref options);
			IntPtr zero = IntPtr.Zero;
			Result result = Bindings.EOS_Leaderboards_CopyLeaderboardUserScoreByIndex(base.InnerHandle, ref copyLeaderboardUserScoreByIndexOptionsInternal, ref zero);
			Helper.Dispose<CopyLeaderboardUserScoreByIndexOptionsInternal>(ref copyLeaderboardUserScoreByIndexOptionsInternal);
			Helper.Get<LeaderboardUserScoreInternal, LeaderboardUserScore>(zero, out outLeaderboardUserScore);
			bool flag = outLeaderboardUserScore != null;
			if (flag)
			{
				Bindings.EOS_Leaderboards_LeaderboardUserScore_Release(zero);
			}
			return result;
		}

		// Token: 0x06001DA4 RID: 7588 RVA: 0x0002B72C File Offset: 0x0002992C
		public Result CopyLeaderboardUserScoreByUserId(ref CopyLeaderboardUserScoreByUserIdOptions options, out LeaderboardUserScore? outLeaderboardUserScore)
		{
			CopyLeaderboardUserScoreByUserIdOptionsInternal copyLeaderboardUserScoreByUserIdOptionsInternal = default(CopyLeaderboardUserScoreByUserIdOptionsInternal);
			copyLeaderboardUserScoreByUserIdOptionsInternal.Set(ref options);
			IntPtr zero = IntPtr.Zero;
			Result result = Bindings.EOS_Leaderboards_CopyLeaderboardUserScoreByUserId(base.InnerHandle, ref copyLeaderboardUserScoreByUserIdOptionsInternal, ref zero);
			Helper.Dispose<CopyLeaderboardUserScoreByUserIdOptionsInternal>(ref copyLeaderboardUserScoreByUserIdOptionsInternal);
			Helper.Get<LeaderboardUserScoreInternal, LeaderboardUserScore>(zero, out outLeaderboardUserScore);
			bool flag = outLeaderboardUserScore != null;
			if (flag)
			{
				Bindings.EOS_Leaderboards_LeaderboardUserScore_Release(zero);
			}
			return result;
		}

		// Token: 0x06001DA5 RID: 7589 RVA: 0x0002B78C File Offset: 0x0002998C
		public uint GetLeaderboardDefinitionCount(ref GetLeaderboardDefinitionCountOptions options)
		{
			GetLeaderboardDefinitionCountOptionsInternal getLeaderboardDefinitionCountOptionsInternal = default(GetLeaderboardDefinitionCountOptionsInternal);
			getLeaderboardDefinitionCountOptionsInternal.Set(ref options);
			uint result = Bindings.EOS_Leaderboards_GetLeaderboardDefinitionCount(base.InnerHandle, ref getLeaderboardDefinitionCountOptionsInternal);
			Helper.Dispose<GetLeaderboardDefinitionCountOptionsInternal>(ref getLeaderboardDefinitionCountOptionsInternal);
			return result;
		}

		// Token: 0x06001DA6 RID: 7590 RVA: 0x0002B7C8 File Offset: 0x000299C8
		public uint GetLeaderboardRecordCount(ref GetLeaderboardRecordCountOptions options)
		{
			GetLeaderboardRecordCountOptionsInternal getLeaderboardRecordCountOptionsInternal = default(GetLeaderboardRecordCountOptionsInternal);
			getLeaderboardRecordCountOptionsInternal.Set(ref options);
			uint result = Bindings.EOS_Leaderboards_GetLeaderboardRecordCount(base.InnerHandle, ref getLeaderboardRecordCountOptionsInternal);
			Helper.Dispose<GetLeaderboardRecordCountOptionsInternal>(ref getLeaderboardRecordCountOptionsInternal);
			return result;
		}

		// Token: 0x06001DA7 RID: 7591 RVA: 0x0002B804 File Offset: 0x00029A04
		public uint GetLeaderboardUserScoreCount(ref GetLeaderboardUserScoreCountOptions options)
		{
			GetLeaderboardUserScoreCountOptionsInternal getLeaderboardUserScoreCountOptionsInternal = default(GetLeaderboardUserScoreCountOptionsInternal);
			getLeaderboardUserScoreCountOptionsInternal.Set(ref options);
			uint result = Bindings.EOS_Leaderboards_GetLeaderboardUserScoreCount(base.InnerHandle, ref getLeaderboardUserScoreCountOptionsInternal);
			Helper.Dispose<GetLeaderboardUserScoreCountOptionsInternal>(ref getLeaderboardUserScoreCountOptionsInternal);
			return result;
		}

		// Token: 0x06001DA8 RID: 7592 RVA: 0x0002B840 File Offset: 0x00029A40
		public void QueryLeaderboardDefinitions(ref QueryLeaderboardDefinitionsOptions options, object clientData, OnQueryLeaderboardDefinitionsCompleteCallback completionDelegate)
		{
			QueryLeaderboardDefinitionsOptionsInternal queryLeaderboardDefinitionsOptionsInternal = default(QueryLeaderboardDefinitionsOptionsInternal);
			queryLeaderboardDefinitionsOptionsInternal.Set(ref options);
			IntPtr zero = IntPtr.Zero;
			OnQueryLeaderboardDefinitionsCompleteCallbackInternal onQueryLeaderboardDefinitionsCompleteCallbackInternal = new OnQueryLeaderboardDefinitionsCompleteCallbackInternal(LeaderboardsInterface.OnQueryLeaderboardDefinitionsCompleteCallbackInternalImplementation);
			Helper.AddCallback(out zero, clientData, completionDelegate, onQueryLeaderboardDefinitionsCompleteCallbackInternal, Array.Empty<Delegate>());
			Bindings.EOS_Leaderboards_QueryLeaderboardDefinitions(base.InnerHandle, ref queryLeaderboardDefinitionsOptionsInternal, zero, onQueryLeaderboardDefinitionsCompleteCallbackInternal);
			Helper.Dispose<QueryLeaderboardDefinitionsOptionsInternal>(ref queryLeaderboardDefinitionsOptionsInternal);
		}

		// Token: 0x06001DA9 RID: 7593 RVA: 0x0002B89C File Offset: 0x00029A9C
		public void QueryLeaderboardRanks(ref QueryLeaderboardRanksOptions options, object clientData, OnQueryLeaderboardRanksCompleteCallback completionDelegate)
		{
			QueryLeaderboardRanksOptionsInternal queryLeaderboardRanksOptionsInternal = default(QueryLeaderboardRanksOptionsInternal);
			queryLeaderboardRanksOptionsInternal.Set(ref options);
			IntPtr zero = IntPtr.Zero;
			OnQueryLeaderboardRanksCompleteCallbackInternal onQueryLeaderboardRanksCompleteCallbackInternal = new OnQueryLeaderboardRanksCompleteCallbackInternal(LeaderboardsInterface.OnQueryLeaderboardRanksCompleteCallbackInternalImplementation);
			Helper.AddCallback(out zero, clientData, completionDelegate, onQueryLeaderboardRanksCompleteCallbackInternal, Array.Empty<Delegate>());
			Bindings.EOS_Leaderboards_QueryLeaderboardRanks(base.InnerHandle, ref queryLeaderboardRanksOptionsInternal, zero, onQueryLeaderboardRanksCompleteCallbackInternal);
			Helper.Dispose<QueryLeaderboardRanksOptionsInternal>(ref queryLeaderboardRanksOptionsInternal);
		}

		// Token: 0x06001DAA RID: 7594 RVA: 0x0002B8F8 File Offset: 0x00029AF8
		public void QueryLeaderboardUserScores(ref QueryLeaderboardUserScoresOptions options, object clientData, OnQueryLeaderboardUserScoresCompleteCallback completionDelegate)
		{
			QueryLeaderboardUserScoresOptionsInternal queryLeaderboardUserScoresOptionsInternal = default(QueryLeaderboardUserScoresOptionsInternal);
			queryLeaderboardUserScoresOptionsInternal.Set(ref options);
			IntPtr zero = IntPtr.Zero;
			OnQueryLeaderboardUserScoresCompleteCallbackInternal onQueryLeaderboardUserScoresCompleteCallbackInternal = new OnQueryLeaderboardUserScoresCompleteCallbackInternal(LeaderboardsInterface.OnQueryLeaderboardUserScoresCompleteCallbackInternalImplementation);
			Helper.AddCallback(out zero, clientData, completionDelegate, onQueryLeaderboardUserScoresCompleteCallbackInternal, Array.Empty<Delegate>());
			Bindings.EOS_Leaderboards_QueryLeaderboardUserScores(base.InnerHandle, ref queryLeaderboardUserScoresOptionsInternal, zero, onQueryLeaderboardUserScoresCompleteCallbackInternal);
			Helper.Dispose<QueryLeaderboardUserScoresOptionsInternal>(ref queryLeaderboardUserScoresOptionsInternal);
		}

		// Token: 0x06001DAB RID: 7595 RVA: 0x0002B954 File Offset: 0x00029B54
		[MonoPInvokeCallback(typeof(OnQueryLeaderboardDefinitionsCompleteCallbackInternal))]
		internal static void OnQueryLeaderboardDefinitionsCompleteCallbackInternalImplementation(ref OnQueryLeaderboardDefinitionsCompleteCallbackInfoInternal data)
		{
			OnQueryLeaderboardDefinitionsCompleteCallback onQueryLeaderboardDefinitionsCompleteCallback;
			OnQueryLeaderboardDefinitionsCompleteCallbackInfo onQueryLeaderboardDefinitionsCompleteCallbackInfo;
			bool flag = Helper.TryGetAndRemoveCallback<OnQueryLeaderboardDefinitionsCompleteCallbackInfoInternal, OnQueryLeaderboardDefinitionsCompleteCallback, OnQueryLeaderboardDefinitionsCompleteCallbackInfo>(ref data, out onQueryLeaderboardDefinitionsCompleteCallback, out onQueryLeaderboardDefinitionsCompleteCallbackInfo);
			if (flag)
			{
				onQueryLeaderboardDefinitionsCompleteCallback(ref onQueryLeaderboardDefinitionsCompleteCallbackInfo);
			}
		}

		// Token: 0x06001DAC RID: 7596 RVA: 0x0002B97C File Offset: 0x00029B7C
		[MonoPInvokeCallback(typeof(OnQueryLeaderboardRanksCompleteCallbackInternal))]
		internal static void OnQueryLeaderboardRanksCompleteCallbackInternalImplementation(ref OnQueryLeaderboardRanksCompleteCallbackInfoInternal data)
		{
			OnQueryLeaderboardRanksCompleteCallback onQueryLeaderboardRanksCompleteCallback;
			OnQueryLeaderboardRanksCompleteCallbackInfo onQueryLeaderboardRanksCompleteCallbackInfo;
			bool flag = Helper.TryGetAndRemoveCallback<OnQueryLeaderboardRanksCompleteCallbackInfoInternal, OnQueryLeaderboardRanksCompleteCallback, OnQueryLeaderboardRanksCompleteCallbackInfo>(ref data, out onQueryLeaderboardRanksCompleteCallback, out onQueryLeaderboardRanksCompleteCallbackInfo);
			if (flag)
			{
				onQueryLeaderboardRanksCompleteCallback(ref onQueryLeaderboardRanksCompleteCallbackInfo);
			}
		}

		// Token: 0x06001DAD RID: 7597 RVA: 0x0002B9A4 File Offset: 0x00029BA4
		[MonoPInvokeCallback(typeof(OnQueryLeaderboardUserScoresCompleteCallbackInternal))]
		internal static void OnQueryLeaderboardUserScoresCompleteCallbackInternalImplementation(ref OnQueryLeaderboardUserScoresCompleteCallbackInfoInternal data)
		{
			OnQueryLeaderboardUserScoresCompleteCallback onQueryLeaderboardUserScoresCompleteCallback;
			OnQueryLeaderboardUserScoresCompleteCallbackInfo onQueryLeaderboardUserScoresCompleteCallbackInfo;
			bool flag = Helper.TryGetAndRemoveCallback<OnQueryLeaderboardUserScoresCompleteCallbackInfoInternal, OnQueryLeaderboardUserScoresCompleteCallback, OnQueryLeaderboardUserScoresCompleteCallbackInfo>(ref data, out onQueryLeaderboardUserScoresCompleteCallback, out onQueryLeaderboardUserScoresCompleteCallbackInfo);
			if (flag)
			{
				onQueryLeaderboardUserScoresCompleteCallback(ref onQueryLeaderboardUserScoresCompleteCallbackInfo);
			}
		}

		// Token: 0x04000CED RID: 3309
		public const int CopyleaderboarddefinitionbyindexApiLatest = 1;

		// Token: 0x04000CEE RID: 3310
		public const int CopyleaderboarddefinitionbyleaderboardidApiLatest = 1;

		// Token: 0x04000CEF RID: 3311
		public const int CopyleaderboardrecordbyindexApiLatest = 2;

		// Token: 0x04000CF0 RID: 3312
		public const int CopyleaderboardrecordbyuseridApiLatest = 2;

		// Token: 0x04000CF1 RID: 3313
		public const int CopyleaderboarduserscorebyindexApiLatest = 1;

		// Token: 0x04000CF2 RID: 3314
		public const int CopyleaderboarduserscorebyuseridApiLatest = 1;

		// Token: 0x04000CF3 RID: 3315
		public const int DefinitionApiLatest = 1;

		// Token: 0x04000CF4 RID: 3316
		public const int GetleaderboarddefinitioncountApiLatest = 1;

		// Token: 0x04000CF5 RID: 3317
		public const int GetleaderboardrecordcountApiLatest = 1;

		// Token: 0x04000CF6 RID: 3318
		public const int GetleaderboarduserscorecountApiLatest = 1;

		// Token: 0x04000CF7 RID: 3319
		public const int LeaderboardrecordApiLatest = 2;

		// Token: 0x04000CF8 RID: 3320
		public const int LeaderboarduserscoreApiLatest = 1;

		// Token: 0x04000CF9 RID: 3321
		public const int QueryleaderboarddefinitionsApiLatest = 2;

		// Token: 0x04000CFA RID: 3322
		public const int QueryleaderboardranksApiLatest = 2;

		// Token: 0x04000CFB RID: 3323
		public const int QueryleaderboarduserscoresApiLatest = 2;

		// Token: 0x04000CFC RID: 3324
		public const int TimeUndefined = -1;

		// Token: 0x04000CFD RID: 3325
		public const int UserscoresquerystatinfoApiLatest = 1;
	}
}
