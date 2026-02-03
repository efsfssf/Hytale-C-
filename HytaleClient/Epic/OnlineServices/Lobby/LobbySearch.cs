using System;

namespace Epic.OnlineServices.Lobby
{
	// Token: 0x020003F0 RID: 1008
	public sealed class LobbySearch : Handle
	{
		// Token: 0x06001B2A RID: 6954 RVA: 0x00028C95 File Offset: 0x00026E95
		public LobbySearch()
		{
		}

		// Token: 0x06001B2B RID: 6955 RVA: 0x00028C9F File Offset: 0x00026E9F
		public LobbySearch(IntPtr innerHandle) : base(innerHandle)
		{
		}

		// Token: 0x06001B2C RID: 6956 RVA: 0x00028CAC File Offset: 0x00026EAC
		public Result CopySearchResultByIndex(ref LobbySearchCopySearchResultByIndexOptions options, out LobbyDetails outLobbyDetailsHandle)
		{
			LobbySearchCopySearchResultByIndexOptionsInternal lobbySearchCopySearchResultByIndexOptionsInternal = default(LobbySearchCopySearchResultByIndexOptionsInternal);
			lobbySearchCopySearchResultByIndexOptionsInternal.Set(ref options);
			IntPtr zero = IntPtr.Zero;
			Result result = Bindings.EOS_LobbySearch_CopySearchResultByIndex(base.InnerHandle, ref lobbySearchCopySearchResultByIndexOptionsInternal, ref zero);
			Helper.Dispose<LobbySearchCopySearchResultByIndexOptionsInternal>(ref lobbySearchCopySearchResultByIndexOptionsInternal);
			Helper.Get<LobbyDetails>(zero, out outLobbyDetailsHandle);
			return result;
		}

		// Token: 0x06001B2D RID: 6957 RVA: 0x00028CF8 File Offset: 0x00026EF8
		public void Find(ref LobbySearchFindOptions options, object clientData, LobbySearchOnFindCallback completionDelegate)
		{
			LobbySearchFindOptionsInternal lobbySearchFindOptionsInternal = default(LobbySearchFindOptionsInternal);
			lobbySearchFindOptionsInternal.Set(ref options);
			IntPtr zero = IntPtr.Zero;
			LobbySearchOnFindCallbackInternal lobbySearchOnFindCallbackInternal = new LobbySearchOnFindCallbackInternal(LobbySearch.OnFindCallbackInternalImplementation);
			Helper.AddCallback(out zero, clientData, completionDelegate, lobbySearchOnFindCallbackInternal, Array.Empty<Delegate>());
			Bindings.EOS_LobbySearch_Find(base.InnerHandle, ref lobbySearchFindOptionsInternal, zero, lobbySearchOnFindCallbackInternal);
			Helper.Dispose<LobbySearchFindOptionsInternal>(ref lobbySearchFindOptionsInternal);
		}

		// Token: 0x06001B2E RID: 6958 RVA: 0x00028D54 File Offset: 0x00026F54
		public uint GetSearchResultCount(ref LobbySearchGetSearchResultCountOptions options)
		{
			LobbySearchGetSearchResultCountOptionsInternal lobbySearchGetSearchResultCountOptionsInternal = default(LobbySearchGetSearchResultCountOptionsInternal);
			lobbySearchGetSearchResultCountOptionsInternal.Set(ref options);
			uint result = Bindings.EOS_LobbySearch_GetSearchResultCount(base.InnerHandle, ref lobbySearchGetSearchResultCountOptionsInternal);
			Helper.Dispose<LobbySearchGetSearchResultCountOptionsInternal>(ref lobbySearchGetSearchResultCountOptionsInternal);
			return result;
		}

		// Token: 0x06001B2F RID: 6959 RVA: 0x00028D8E File Offset: 0x00026F8E
		public void Release()
		{
			Bindings.EOS_LobbySearch_Release(base.InnerHandle);
		}

		// Token: 0x06001B30 RID: 6960 RVA: 0x00028DA0 File Offset: 0x00026FA0
		public Result RemoveParameter(ref LobbySearchRemoveParameterOptions options)
		{
			LobbySearchRemoveParameterOptionsInternal lobbySearchRemoveParameterOptionsInternal = default(LobbySearchRemoveParameterOptionsInternal);
			lobbySearchRemoveParameterOptionsInternal.Set(ref options);
			Result result = Bindings.EOS_LobbySearch_RemoveParameter(base.InnerHandle, ref lobbySearchRemoveParameterOptionsInternal);
			Helper.Dispose<LobbySearchRemoveParameterOptionsInternal>(ref lobbySearchRemoveParameterOptionsInternal);
			return result;
		}

		// Token: 0x06001B31 RID: 6961 RVA: 0x00028DDC File Offset: 0x00026FDC
		public Result SetLobbyId(ref LobbySearchSetLobbyIdOptions options)
		{
			LobbySearchSetLobbyIdOptionsInternal lobbySearchSetLobbyIdOptionsInternal = default(LobbySearchSetLobbyIdOptionsInternal);
			lobbySearchSetLobbyIdOptionsInternal.Set(ref options);
			Result result = Bindings.EOS_LobbySearch_SetLobbyId(base.InnerHandle, ref lobbySearchSetLobbyIdOptionsInternal);
			Helper.Dispose<LobbySearchSetLobbyIdOptionsInternal>(ref lobbySearchSetLobbyIdOptionsInternal);
			return result;
		}

		// Token: 0x06001B32 RID: 6962 RVA: 0x00028E18 File Offset: 0x00027018
		public Result SetMaxResults(ref LobbySearchSetMaxResultsOptions options)
		{
			LobbySearchSetMaxResultsOptionsInternal lobbySearchSetMaxResultsOptionsInternal = default(LobbySearchSetMaxResultsOptionsInternal);
			lobbySearchSetMaxResultsOptionsInternal.Set(ref options);
			Result result = Bindings.EOS_LobbySearch_SetMaxResults(base.InnerHandle, ref lobbySearchSetMaxResultsOptionsInternal);
			Helper.Dispose<LobbySearchSetMaxResultsOptionsInternal>(ref lobbySearchSetMaxResultsOptionsInternal);
			return result;
		}

		// Token: 0x06001B33 RID: 6963 RVA: 0x00028E54 File Offset: 0x00027054
		public Result SetParameter(ref LobbySearchSetParameterOptions options)
		{
			LobbySearchSetParameterOptionsInternal lobbySearchSetParameterOptionsInternal = default(LobbySearchSetParameterOptionsInternal);
			lobbySearchSetParameterOptionsInternal.Set(ref options);
			Result result = Bindings.EOS_LobbySearch_SetParameter(base.InnerHandle, ref lobbySearchSetParameterOptionsInternal);
			Helper.Dispose<LobbySearchSetParameterOptionsInternal>(ref lobbySearchSetParameterOptionsInternal);
			return result;
		}

		// Token: 0x06001B34 RID: 6964 RVA: 0x00028E90 File Offset: 0x00027090
		public Result SetTargetUserId(ref LobbySearchSetTargetUserIdOptions options)
		{
			LobbySearchSetTargetUserIdOptionsInternal lobbySearchSetTargetUserIdOptionsInternal = default(LobbySearchSetTargetUserIdOptionsInternal);
			lobbySearchSetTargetUserIdOptionsInternal.Set(ref options);
			Result result = Bindings.EOS_LobbySearch_SetTargetUserId(base.InnerHandle, ref lobbySearchSetTargetUserIdOptionsInternal);
			Helper.Dispose<LobbySearchSetTargetUserIdOptionsInternal>(ref lobbySearchSetTargetUserIdOptionsInternal);
			return result;
		}

		// Token: 0x06001B35 RID: 6965 RVA: 0x00028ECC File Offset: 0x000270CC
		[MonoPInvokeCallback(typeof(LobbySearchOnFindCallbackInternal))]
		internal static void OnFindCallbackInternalImplementation(ref LobbySearchFindCallbackInfoInternal data)
		{
			LobbySearchOnFindCallback lobbySearchOnFindCallback;
			LobbySearchFindCallbackInfo lobbySearchFindCallbackInfo;
			bool flag = Helper.TryGetAndRemoveCallback<LobbySearchFindCallbackInfoInternal, LobbySearchOnFindCallback, LobbySearchFindCallbackInfo>(ref data, out lobbySearchOnFindCallback, out lobbySearchFindCallbackInfo);
			if (flag)
			{
				lobbySearchOnFindCallback(ref lobbySearchFindCallbackInfo);
			}
		}

		// Token: 0x04000C31 RID: 3121
		public const int LobbysearchCopysearchresultbyindexApiLatest = 1;

		// Token: 0x04000C32 RID: 3122
		public const int LobbysearchFindApiLatest = 1;

		// Token: 0x04000C33 RID: 3123
		public const int LobbysearchGetsearchresultcountApiLatest = 1;

		// Token: 0x04000C34 RID: 3124
		public const int LobbysearchRemoveparameterApiLatest = 1;

		// Token: 0x04000C35 RID: 3125
		public const int LobbysearchSetlobbyidApiLatest = 1;

		// Token: 0x04000C36 RID: 3126
		public const int LobbysearchSetmaxresultsApiLatest = 1;

		// Token: 0x04000C37 RID: 3127
		public const int LobbysearchSetparameterApiLatest = 1;

		// Token: 0x04000C38 RID: 3128
		public const int LobbysearchSettargetuseridApiLatest = 1;
	}
}
