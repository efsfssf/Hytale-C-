using System;

namespace Epic.OnlineServices.Sessions
{
	// Token: 0x02000173 RID: 371
	public sealed class SessionSearch : Handle
	{
		// Token: 0x06000AFF RID: 2815 RVA: 0x0000F9A1 File Offset: 0x0000DBA1
		public SessionSearch()
		{
		}

		// Token: 0x06000B00 RID: 2816 RVA: 0x0000F9AB File Offset: 0x0000DBAB
		public SessionSearch(IntPtr innerHandle) : base(innerHandle)
		{
		}

		// Token: 0x06000B01 RID: 2817 RVA: 0x0000F9B8 File Offset: 0x0000DBB8
		public Result CopySearchResultByIndex(ref SessionSearchCopySearchResultByIndexOptions options, out SessionDetails outSessionHandle)
		{
			SessionSearchCopySearchResultByIndexOptionsInternal sessionSearchCopySearchResultByIndexOptionsInternal = default(SessionSearchCopySearchResultByIndexOptionsInternal);
			sessionSearchCopySearchResultByIndexOptionsInternal.Set(ref options);
			IntPtr zero = IntPtr.Zero;
			Result result = Bindings.EOS_SessionSearch_CopySearchResultByIndex(base.InnerHandle, ref sessionSearchCopySearchResultByIndexOptionsInternal, ref zero);
			Helper.Dispose<SessionSearchCopySearchResultByIndexOptionsInternal>(ref sessionSearchCopySearchResultByIndexOptionsInternal);
			Helper.Get<SessionDetails>(zero, out outSessionHandle);
			return result;
		}

		// Token: 0x06000B02 RID: 2818 RVA: 0x0000FA04 File Offset: 0x0000DC04
		public void Find(ref SessionSearchFindOptions options, object clientData, SessionSearchOnFindCallback completionDelegate)
		{
			SessionSearchFindOptionsInternal sessionSearchFindOptionsInternal = default(SessionSearchFindOptionsInternal);
			sessionSearchFindOptionsInternal.Set(ref options);
			IntPtr zero = IntPtr.Zero;
			SessionSearchOnFindCallbackInternal sessionSearchOnFindCallbackInternal = new SessionSearchOnFindCallbackInternal(SessionSearch.OnFindCallbackInternalImplementation);
			Helper.AddCallback(out zero, clientData, completionDelegate, sessionSearchOnFindCallbackInternal, Array.Empty<Delegate>());
			Bindings.EOS_SessionSearch_Find(base.InnerHandle, ref sessionSearchFindOptionsInternal, zero, sessionSearchOnFindCallbackInternal);
			Helper.Dispose<SessionSearchFindOptionsInternal>(ref sessionSearchFindOptionsInternal);
		}

		// Token: 0x06000B03 RID: 2819 RVA: 0x0000FA60 File Offset: 0x0000DC60
		public uint GetSearchResultCount(ref SessionSearchGetSearchResultCountOptions options)
		{
			SessionSearchGetSearchResultCountOptionsInternal sessionSearchGetSearchResultCountOptionsInternal = default(SessionSearchGetSearchResultCountOptionsInternal);
			sessionSearchGetSearchResultCountOptionsInternal.Set(ref options);
			uint result = Bindings.EOS_SessionSearch_GetSearchResultCount(base.InnerHandle, ref sessionSearchGetSearchResultCountOptionsInternal);
			Helper.Dispose<SessionSearchGetSearchResultCountOptionsInternal>(ref sessionSearchGetSearchResultCountOptionsInternal);
			return result;
		}

		// Token: 0x06000B04 RID: 2820 RVA: 0x0000FA9A File Offset: 0x0000DC9A
		public void Release()
		{
			Bindings.EOS_SessionSearch_Release(base.InnerHandle);
		}

		// Token: 0x06000B05 RID: 2821 RVA: 0x0000FAAC File Offset: 0x0000DCAC
		public Result RemoveParameter(ref SessionSearchRemoveParameterOptions options)
		{
			SessionSearchRemoveParameterOptionsInternal sessionSearchRemoveParameterOptionsInternal = default(SessionSearchRemoveParameterOptionsInternal);
			sessionSearchRemoveParameterOptionsInternal.Set(ref options);
			Result result = Bindings.EOS_SessionSearch_RemoveParameter(base.InnerHandle, ref sessionSearchRemoveParameterOptionsInternal);
			Helper.Dispose<SessionSearchRemoveParameterOptionsInternal>(ref sessionSearchRemoveParameterOptionsInternal);
			return result;
		}

		// Token: 0x06000B06 RID: 2822 RVA: 0x0000FAE8 File Offset: 0x0000DCE8
		public Result SetMaxResults(ref SessionSearchSetMaxResultsOptions options)
		{
			SessionSearchSetMaxResultsOptionsInternal sessionSearchSetMaxResultsOptionsInternal = default(SessionSearchSetMaxResultsOptionsInternal);
			sessionSearchSetMaxResultsOptionsInternal.Set(ref options);
			Result result = Bindings.EOS_SessionSearch_SetMaxResults(base.InnerHandle, ref sessionSearchSetMaxResultsOptionsInternal);
			Helper.Dispose<SessionSearchSetMaxResultsOptionsInternal>(ref sessionSearchSetMaxResultsOptionsInternal);
			return result;
		}

		// Token: 0x06000B07 RID: 2823 RVA: 0x0000FB24 File Offset: 0x0000DD24
		public Result SetParameter(ref SessionSearchSetParameterOptions options)
		{
			SessionSearchSetParameterOptionsInternal sessionSearchSetParameterOptionsInternal = default(SessionSearchSetParameterOptionsInternal);
			sessionSearchSetParameterOptionsInternal.Set(ref options);
			Result result = Bindings.EOS_SessionSearch_SetParameter(base.InnerHandle, ref sessionSearchSetParameterOptionsInternal);
			Helper.Dispose<SessionSearchSetParameterOptionsInternal>(ref sessionSearchSetParameterOptionsInternal);
			return result;
		}

		// Token: 0x06000B08 RID: 2824 RVA: 0x0000FB60 File Offset: 0x0000DD60
		public Result SetSessionId(ref SessionSearchSetSessionIdOptions options)
		{
			SessionSearchSetSessionIdOptionsInternal sessionSearchSetSessionIdOptionsInternal = default(SessionSearchSetSessionIdOptionsInternal);
			sessionSearchSetSessionIdOptionsInternal.Set(ref options);
			Result result = Bindings.EOS_SessionSearch_SetSessionId(base.InnerHandle, ref sessionSearchSetSessionIdOptionsInternal);
			Helper.Dispose<SessionSearchSetSessionIdOptionsInternal>(ref sessionSearchSetSessionIdOptionsInternal);
			return result;
		}

		// Token: 0x06000B09 RID: 2825 RVA: 0x0000FB9C File Offset: 0x0000DD9C
		public Result SetTargetUserId(ref SessionSearchSetTargetUserIdOptions options)
		{
			SessionSearchSetTargetUserIdOptionsInternal sessionSearchSetTargetUserIdOptionsInternal = default(SessionSearchSetTargetUserIdOptionsInternal);
			sessionSearchSetTargetUserIdOptionsInternal.Set(ref options);
			Result result = Bindings.EOS_SessionSearch_SetTargetUserId(base.InnerHandle, ref sessionSearchSetTargetUserIdOptionsInternal);
			Helper.Dispose<SessionSearchSetTargetUserIdOptionsInternal>(ref sessionSearchSetTargetUserIdOptionsInternal);
			return result;
		}

		// Token: 0x06000B0A RID: 2826 RVA: 0x0000FBD8 File Offset: 0x0000DDD8
		[MonoPInvokeCallback(typeof(SessionSearchOnFindCallbackInternal))]
		internal static void OnFindCallbackInternalImplementation(ref SessionSearchFindCallbackInfoInternal data)
		{
			SessionSearchOnFindCallback sessionSearchOnFindCallback;
			SessionSearchFindCallbackInfo sessionSearchFindCallbackInfo;
			bool flag = Helper.TryGetAndRemoveCallback<SessionSearchFindCallbackInfoInternal, SessionSearchOnFindCallback, SessionSearchFindCallbackInfo>(ref data, out sessionSearchOnFindCallback, out sessionSearchFindCallbackInfo);
			if (flag)
			{
				sessionSearchOnFindCallback(ref sessionSearchFindCallbackInfo);
			}
		}

		// Token: 0x04000504 RID: 1284
		public const int SessionsearchCopysearchresultbyindexApiLatest = 1;

		// Token: 0x04000505 RID: 1285
		public const int SessionsearchFindApiLatest = 2;

		// Token: 0x04000506 RID: 1286
		public const int SessionsearchGetsearchresultcountApiLatest = 1;

		// Token: 0x04000507 RID: 1287
		public const int SessionsearchRemoveparameterApiLatest = 1;

		// Token: 0x04000508 RID: 1288
		public const int SessionsearchSetmaxsearchresultsApiLatest = 1;

		// Token: 0x04000509 RID: 1289
		public const int SessionsearchSetparameterApiLatest = 1;

		// Token: 0x0400050A RID: 1290
		public const int SessionsearchSetsessionidApiLatest = 1;

		// Token: 0x0400050B RID: 1291
		public const int SessionsearchSettargetuseridApiLatest = 1;
	}
}
