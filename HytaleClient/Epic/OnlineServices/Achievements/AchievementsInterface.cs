using System;

namespace Epic.OnlineServices.Achievements
{
	// Token: 0x02000723 RID: 1827
	public sealed class AchievementsInterface : Handle
	{
		// Token: 0x06002F6F RID: 12143 RVA: 0x000463D3 File Offset: 0x000445D3
		public AchievementsInterface()
		{
		}

		// Token: 0x06002F70 RID: 12144 RVA: 0x000463DD File Offset: 0x000445DD
		public AchievementsInterface(IntPtr innerHandle) : base(innerHandle)
		{
		}

		// Token: 0x06002F71 RID: 12145 RVA: 0x000463E8 File Offset: 0x000445E8
		public ulong AddNotifyAchievementsUnlocked(ref AddNotifyAchievementsUnlockedOptions options, object clientData, OnAchievementsUnlockedCallback notificationFn)
		{
			AddNotifyAchievementsUnlockedOptionsInternal addNotifyAchievementsUnlockedOptionsInternal = default(AddNotifyAchievementsUnlockedOptionsInternal);
			addNotifyAchievementsUnlockedOptionsInternal.Set(ref options);
			IntPtr zero = IntPtr.Zero;
			OnAchievementsUnlockedCallbackInternal onAchievementsUnlockedCallbackInternal = new OnAchievementsUnlockedCallbackInternal(AchievementsInterface.OnAchievementsUnlockedCallbackInternalImplementation);
			Helper.AddCallback(out zero, clientData, notificationFn, onAchievementsUnlockedCallbackInternal, Array.Empty<Delegate>());
			ulong num = Bindings.EOS_Achievements_AddNotifyAchievementsUnlocked(base.InnerHandle, ref addNotifyAchievementsUnlockedOptionsInternal, zero, onAchievementsUnlockedCallbackInternal);
			Helper.Dispose<AddNotifyAchievementsUnlockedOptionsInternal>(ref addNotifyAchievementsUnlockedOptionsInternal);
			Helper.AssignNotificationIdToCallback(zero, num);
			return num;
		}

		// Token: 0x06002F72 RID: 12146 RVA: 0x00046454 File Offset: 0x00044654
		public ulong AddNotifyAchievementsUnlockedV2(ref AddNotifyAchievementsUnlockedV2Options options, object clientData, OnAchievementsUnlockedCallbackV2 notificationFn)
		{
			AddNotifyAchievementsUnlockedV2OptionsInternal addNotifyAchievementsUnlockedV2OptionsInternal = default(AddNotifyAchievementsUnlockedV2OptionsInternal);
			addNotifyAchievementsUnlockedV2OptionsInternal.Set(ref options);
			IntPtr zero = IntPtr.Zero;
			OnAchievementsUnlockedCallbackV2Internal onAchievementsUnlockedCallbackV2Internal = new OnAchievementsUnlockedCallbackV2Internal(AchievementsInterface.OnAchievementsUnlockedCallbackV2InternalImplementation);
			Helper.AddCallback(out zero, clientData, notificationFn, onAchievementsUnlockedCallbackV2Internal, Array.Empty<Delegate>());
			ulong num = Bindings.EOS_Achievements_AddNotifyAchievementsUnlockedV2(base.InnerHandle, ref addNotifyAchievementsUnlockedV2OptionsInternal, zero, onAchievementsUnlockedCallbackV2Internal);
			Helper.Dispose<AddNotifyAchievementsUnlockedV2OptionsInternal>(ref addNotifyAchievementsUnlockedV2OptionsInternal);
			Helper.AssignNotificationIdToCallback(zero, num);
			return num;
		}

		// Token: 0x06002F73 RID: 12147 RVA: 0x000464C0 File Offset: 0x000446C0
		public Result CopyAchievementDefinitionByAchievementId(ref CopyAchievementDefinitionByAchievementIdOptions options, out Definition? outDefinition)
		{
			CopyAchievementDefinitionByAchievementIdOptionsInternal copyAchievementDefinitionByAchievementIdOptionsInternal = default(CopyAchievementDefinitionByAchievementIdOptionsInternal);
			copyAchievementDefinitionByAchievementIdOptionsInternal.Set(ref options);
			IntPtr zero = IntPtr.Zero;
			Result result = Bindings.EOS_Achievements_CopyAchievementDefinitionByAchievementId(base.InnerHandle, ref copyAchievementDefinitionByAchievementIdOptionsInternal, ref zero);
			Helper.Dispose<CopyAchievementDefinitionByAchievementIdOptionsInternal>(ref copyAchievementDefinitionByAchievementIdOptionsInternal);
			Helper.Get<DefinitionInternal, Definition>(zero, out outDefinition);
			bool flag = outDefinition != null;
			if (flag)
			{
				Bindings.EOS_Achievements_Definition_Release(zero);
			}
			return result;
		}

		// Token: 0x06002F74 RID: 12148 RVA: 0x00046520 File Offset: 0x00044720
		public Result CopyAchievementDefinitionByIndex(ref CopyAchievementDefinitionByIndexOptions options, out Definition? outDefinition)
		{
			CopyAchievementDefinitionByIndexOptionsInternal copyAchievementDefinitionByIndexOptionsInternal = default(CopyAchievementDefinitionByIndexOptionsInternal);
			copyAchievementDefinitionByIndexOptionsInternal.Set(ref options);
			IntPtr zero = IntPtr.Zero;
			Result result = Bindings.EOS_Achievements_CopyAchievementDefinitionByIndex(base.InnerHandle, ref copyAchievementDefinitionByIndexOptionsInternal, ref zero);
			Helper.Dispose<CopyAchievementDefinitionByIndexOptionsInternal>(ref copyAchievementDefinitionByIndexOptionsInternal);
			Helper.Get<DefinitionInternal, Definition>(zero, out outDefinition);
			bool flag = outDefinition != null;
			if (flag)
			{
				Bindings.EOS_Achievements_Definition_Release(zero);
			}
			return result;
		}

		// Token: 0x06002F75 RID: 12149 RVA: 0x00046580 File Offset: 0x00044780
		public Result CopyAchievementDefinitionV2ByAchievementId(ref CopyAchievementDefinitionV2ByAchievementIdOptions options, out DefinitionV2? outDefinition)
		{
			CopyAchievementDefinitionV2ByAchievementIdOptionsInternal copyAchievementDefinitionV2ByAchievementIdOptionsInternal = default(CopyAchievementDefinitionV2ByAchievementIdOptionsInternal);
			copyAchievementDefinitionV2ByAchievementIdOptionsInternal.Set(ref options);
			IntPtr zero = IntPtr.Zero;
			Result result = Bindings.EOS_Achievements_CopyAchievementDefinitionV2ByAchievementId(base.InnerHandle, ref copyAchievementDefinitionV2ByAchievementIdOptionsInternal, ref zero);
			Helper.Dispose<CopyAchievementDefinitionV2ByAchievementIdOptionsInternal>(ref copyAchievementDefinitionV2ByAchievementIdOptionsInternal);
			Helper.Get<DefinitionV2Internal, DefinitionV2>(zero, out outDefinition);
			bool flag = outDefinition != null;
			if (flag)
			{
				Bindings.EOS_Achievements_DefinitionV2_Release(zero);
			}
			return result;
		}

		// Token: 0x06002F76 RID: 12150 RVA: 0x000465E0 File Offset: 0x000447E0
		public Result CopyAchievementDefinitionV2ByIndex(ref CopyAchievementDefinitionV2ByIndexOptions options, out DefinitionV2? outDefinition)
		{
			CopyAchievementDefinitionV2ByIndexOptionsInternal copyAchievementDefinitionV2ByIndexOptionsInternal = default(CopyAchievementDefinitionV2ByIndexOptionsInternal);
			copyAchievementDefinitionV2ByIndexOptionsInternal.Set(ref options);
			IntPtr zero = IntPtr.Zero;
			Result result = Bindings.EOS_Achievements_CopyAchievementDefinitionV2ByIndex(base.InnerHandle, ref copyAchievementDefinitionV2ByIndexOptionsInternal, ref zero);
			Helper.Dispose<CopyAchievementDefinitionV2ByIndexOptionsInternal>(ref copyAchievementDefinitionV2ByIndexOptionsInternal);
			Helper.Get<DefinitionV2Internal, DefinitionV2>(zero, out outDefinition);
			bool flag = outDefinition != null;
			if (flag)
			{
				Bindings.EOS_Achievements_DefinitionV2_Release(zero);
			}
			return result;
		}

		// Token: 0x06002F77 RID: 12151 RVA: 0x00046640 File Offset: 0x00044840
		public Result CopyPlayerAchievementByAchievementId(ref CopyPlayerAchievementByAchievementIdOptions options, out PlayerAchievement? outAchievement)
		{
			CopyPlayerAchievementByAchievementIdOptionsInternal copyPlayerAchievementByAchievementIdOptionsInternal = default(CopyPlayerAchievementByAchievementIdOptionsInternal);
			copyPlayerAchievementByAchievementIdOptionsInternal.Set(ref options);
			IntPtr zero = IntPtr.Zero;
			Result result = Bindings.EOS_Achievements_CopyPlayerAchievementByAchievementId(base.InnerHandle, ref copyPlayerAchievementByAchievementIdOptionsInternal, ref zero);
			Helper.Dispose<CopyPlayerAchievementByAchievementIdOptionsInternal>(ref copyPlayerAchievementByAchievementIdOptionsInternal);
			Helper.Get<PlayerAchievementInternal, PlayerAchievement>(zero, out outAchievement);
			bool flag = outAchievement != null;
			if (flag)
			{
				Bindings.EOS_Achievements_PlayerAchievement_Release(zero);
			}
			return result;
		}

		// Token: 0x06002F78 RID: 12152 RVA: 0x000466A0 File Offset: 0x000448A0
		public Result CopyPlayerAchievementByIndex(ref CopyPlayerAchievementByIndexOptions options, out PlayerAchievement? outAchievement)
		{
			CopyPlayerAchievementByIndexOptionsInternal copyPlayerAchievementByIndexOptionsInternal = default(CopyPlayerAchievementByIndexOptionsInternal);
			copyPlayerAchievementByIndexOptionsInternal.Set(ref options);
			IntPtr zero = IntPtr.Zero;
			Result result = Bindings.EOS_Achievements_CopyPlayerAchievementByIndex(base.InnerHandle, ref copyPlayerAchievementByIndexOptionsInternal, ref zero);
			Helper.Dispose<CopyPlayerAchievementByIndexOptionsInternal>(ref copyPlayerAchievementByIndexOptionsInternal);
			Helper.Get<PlayerAchievementInternal, PlayerAchievement>(zero, out outAchievement);
			bool flag = outAchievement != null;
			if (flag)
			{
				Bindings.EOS_Achievements_PlayerAchievement_Release(zero);
			}
			return result;
		}

		// Token: 0x06002F79 RID: 12153 RVA: 0x00046700 File Offset: 0x00044900
		public Result CopyUnlockedAchievementByAchievementId(ref CopyUnlockedAchievementByAchievementIdOptions options, out UnlockedAchievement? outAchievement)
		{
			CopyUnlockedAchievementByAchievementIdOptionsInternal copyUnlockedAchievementByAchievementIdOptionsInternal = default(CopyUnlockedAchievementByAchievementIdOptionsInternal);
			copyUnlockedAchievementByAchievementIdOptionsInternal.Set(ref options);
			IntPtr zero = IntPtr.Zero;
			Result result = Bindings.EOS_Achievements_CopyUnlockedAchievementByAchievementId(base.InnerHandle, ref copyUnlockedAchievementByAchievementIdOptionsInternal, ref zero);
			Helper.Dispose<CopyUnlockedAchievementByAchievementIdOptionsInternal>(ref copyUnlockedAchievementByAchievementIdOptionsInternal);
			Helper.Get<UnlockedAchievementInternal, UnlockedAchievement>(zero, out outAchievement);
			bool flag = outAchievement != null;
			if (flag)
			{
				Bindings.EOS_Achievements_UnlockedAchievement_Release(zero);
			}
			return result;
		}

		// Token: 0x06002F7A RID: 12154 RVA: 0x00046760 File Offset: 0x00044960
		public Result CopyUnlockedAchievementByIndex(ref CopyUnlockedAchievementByIndexOptions options, out UnlockedAchievement? outAchievement)
		{
			CopyUnlockedAchievementByIndexOptionsInternal copyUnlockedAchievementByIndexOptionsInternal = default(CopyUnlockedAchievementByIndexOptionsInternal);
			copyUnlockedAchievementByIndexOptionsInternal.Set(ref options);
			IntPtr zero = IntPtr.Zero;
			Result result = Bindings.EOS_Achievements_CopyUnlockedAchievementByIndex(base.InnerHandle, ref copyUnlockedAchievementByIndexOptionsInternal, ref zero);
			Helper.Dispose<CopyUnlockedAchievementByIndexOptionsInternal>(ref copyUnlockedAchievementByIndexOptionsInternal);
			Helper.Get<UnlockedAchievementInternal, UnlockedAchievement>(zero, out outAchievement);
			bool flag = outAchievement != null;
			if (flag)
			{
				Bindings.EOS_Achievements_UnlockedAchievement_Release(zero);
			}
			return result;
		}

		// Token: 0x06002F7B RID: 12155 RVA: 0x000467C0 File Offset: 0x000449C0
		public uint GetAchievementDefinitionCount(ref GetAchievementDefinitionCountOptions options)
		{
			GetAchievementDefinitionCountOptionsInternal getAchievementDefinitionCountOptionsInternal = default(GetAchievementDefinitionCountOptionsInternal);
			getAchievementDefinitionCountOptionsInternal.Set(ref options);
			uint result = Bindings.EOS_Achievements_GetAchievementDefinitionCount(base.InnerHandle, ref getAchievementDefinitionCountOptionsInternal);
			Helper.Dispose<GetAchievementDefinitionCountOptionsInternal>(ref getAchievementDefinitionCountOptionsInternal);
			return result;
		}

		// Token: 0x06002F7C RID: 12156 RVA: 0x000467FC File Offset: 0x000449FC
		public uint GetPlayerAchievementCount(ref GetPlayerAchievementCountOptions options)
		{
			GetPlayerAchievementCountOptionsInternal getPlayerAchievementCountOptionsInternal = default(GetPlayerAchievementCountOptionsInternal);
			getPlayerAchievementCountOptionsInternal.Set(ref options);
			uint result = Bindings.EOS_Achievements_GetPlayerAchievementCount(base.InnerHandle, ref getPlayerAchievementCountOptionsInternal);
			Helper.Dispose<GetPlayerAchievementCountOptionsInternal>(ref getPlayerAchievementCountOptionsInternal);
			return result;
		}

		// Token: 0x06002F7D RID: 12157 RVA: 0x00046838 File Offset: 0x00044A38
		public uint GetUnlockedAchievementCount(ref GetUnlockedAchievementCountOptions options)
		{
			GetUnlockedAchievementCountOptionsInternal getUnlockedAchievementCountOptionsInternal = default(GetUnlockedAchievementCountOptionsInternal);
			getUnlockedAchievementCountOptionsInternal.Set(ref options);
			uint result = Bindings.EOS_Achievements_GetUnlockedAchievementCount(base.InnerHandle, ref getUnlockedAchievementCountOptionsInternal);
			Helper.Dispose<GetUnlockedAchievementCountOptionsInternal>(ref getUnlockedAchievementCountOptionsInternal);
			return result;
		}

		// Token: 0x06002F7E RID: 12158 RVA: 0x00046874 File Offset: 0x00044A74
		public void QueryDefinitions(ref QueryDefinitionsOptions options, object clientData, OnQueryDefinitionsCompleteCallback completionDelegate)
		{
			QueryDefinitionsOptionsInternal queryDefinitionsOptionsInternal = default(QueryDefinitionsOptionsInternal);
			queryDefinitionsOptionsInternal.Set(ref options);
			IntPtr zero = IntPtr.Zero;
			OnQueryDefinitionsCompleteCallbackInternal onQueryDefinitionsCompleteCallbackInternal = new OnQueryDefinitionsCompleteCallbackInternal(AchievementsInterface.OnQueryDefinitionsCompleteCallbackInternalImplementation);
			Helper.AddCallback(out zero, clientData, completionDelegate, onQueryDefinitionsCompleteCallbackInternal, Array.Empty<Delegate>());
			Bindings.EOS_Achievements_QueryDefinitions(base.InnerHandle, ref queryDefinitionsOptionsInternal, zero, onQueryDefinitionsCompleteCallbackInternal);
			Helper.Dispose<QueryDefinitionsOptionsInternal>(ref queryDefinitionsOptionsInternal);
		}

		// Token: 0x06002F7F RID: 12159 RVA: 0x000468D0 File Offset: 0x00044AD0
		public void QueryPlayerAchievements(ref QueryPlayerAchievementsOptions options, object clientData, OnQueryPlayerAchievementsCompleteCallback completionDelegate)
		{
			QueryPlayerAchievementsOptionsInternal queryPlayerAchievementsOptionsInternal = default(QueryPlayerAchievementsOptionsInternal);
			queryPlayerAchievementsOptionsInternal.Set(ref options);
			IntPtr zero = IntPtr.Zero;
			OnQueryPlayerAchievementsCompleteCallbackInternal onQueryPlayerAchievementsCompleteCallbackInternal = new OnQueryPlayerAchievementsCompleteCallbackInternal(AchievementsInterface.OnQueryPlayerAchievementsCompleteCallbackInternalImplementation);
			Helper.AddCallback(out zero, clientData, completionDelegate, onQueryPlayerAchievementsCompleteCallbackInternal, Array.Empty<Delegate>());
			Bindings.EOS_Achievements_QueryPlayerAchievements(base.InnerHandle, ref queryPlayerAchievementsOptionsInternal, zero, onQueryPlayerAchievementsCompleteCallbackInternal);
			Helper.Dispose<QueryPlayerAchievementsOptionsInternal>(ref queryPlayerAchievementsOptionsInternal);
		}

		// Token: 0x06002F80 RID: 12160 RVA: 0x0004692A File Offset: 0x00044B2A
		public void RemoveNotifyAchievementsUnlocked(ulong inId)
		{
			Bindings.EOS_Achievements_RemoveNotifyAchievementsUnlocked(base.InnerHandle, inId);
			Helper.RemoveCallbackByNotificationId(inId);
		}

		// Token: 0x06002F81 RID: 12161 RVA: 0x00046944 File Offset: 0x00044B44
		public void UnlockAchievements(ref UnlockAchievementsOptions options, object clientData, OnUnlockAchievementsCompleteCallback completionDelegate)
		{
			UnlockAchievementsOptionsInternal unlockAchievementsOptionsInternal = default(UnlockAchievementsOptionsInternal);
			unlockAchievementsOptionsInternal.Set(ref options);
			IntPtr zero = IntPtr.Zero;
			OnUnlockAchievementsCompleteCallbackInternal onUnlockAchievementsCompleteCallbackInternal = new OnUnlockAchievementsCompleteCallbackInternal(AchievementsInterface.OnUnlockAchievementsCompleteCallbackInternalImplementation);
			Helper.AddCallback(out zero, clientData, completionDelegate, onUnlockAchievementsCompleteCallbackInternal, Array.Empty<Delegate>());
			Bindings.EOS_Achievements_UnlockAchievements(base.InnerHandle, ref unlockAchievementsOptionsInternal, zero, onUnlockAchievementsCompleteCallbackInternal);
			Helper.Dispose<UnlockAchievementsOptionsInternal>(ref unlockAchievementsOptionsInternal);
		}

		// Token: 0x06002F82 RID: 12162 RVA: 0x000469A0 File Offset: 0x00044BA0
		[MonoPInvokeCallback(typeof(OnAchievementsUnlockedCallbackInternal))]
		internal static void OnAchievementsUnlockedCallbackInternalImplementation(ref OnAchievementsUnlockedCallbackInfoInternal data)
		{
			OnAchievementsUnlockedCallback onAchievementsUnlockedCallback;
			OnAchievementsUnlockedCallbackInfo onAchievementsUnlockedCallbackInfo;
			bool flag = Helper.TryGetCallback<OnAchievementsUnlockedCallbackInfoInternal, OnAchievementsUnlockedCallback, OnAchievementsUnlockedCallbackInfo>(ref data, out onAchievementsUnlockedCallback, out onAchievementsUnlockedCallbackInfo);
			if (flag)
			{
				onAchievementsUnlockedCallback(ref onAchievementsUnlockedCallbackInfo);
			}
		}

		// Token: 0x06002F83 RID: 12163 RVA: 0x000469C8 File Offset: 0x00044BC8
		[MonoPInvokeCallback(typeof(OnAchievementsUnlockedCallbackV2Internal))]
		internal static void OnAchievementsUnlockedCallbackV2InternalImplementation(ref OnAchievementsUnlockedCallbackV2InfoInternal data)
		{
			OnAchievementsUnlockedCallbackV2 onAchievementsUnlockedCallbackV;
			OnAchievementsUnlockedCallbackV2Info onAchievementsUnlockedCallbackV2Info;
			bool flag = Helper.TryGetCallback<OnAchievementsUnlockedCallbackV2InfoInternal, OnAchievementsUnlockedCallbackV2, OnAchievementsUnlockedCallbackV2Info>(ref data, out onAchievementsUnlockedCallbackV, out onAchievementsUnlockedCallbackV2Info);
			if (flag)
			{
				onAchievementsUnlockedCallbackV(ref onAchievementsUnlockedCallbackV2Info);
			}
		}

		// Token: 0x06002F84 RID: 12164 RVA: 0x000469F0 File Offset: 0x00044BF0
		[MonoPInvokeCallback(typeof(OnQueryDefinitionsCompleteCallbackInternal))]
		internal static void OnQueryDefinitionsCompleteCallbackInternalImplementation(ref OnQueryDefinitionsCompleteCallbackInfoInternal data)
		{
			OnQueryDefinitionsCompleteCallback onQueryDefinitionsCompleteCallback;
			OnQueryDefinitionsCompleteCallbackInfo onQueryDefinitionsCompleteCallbackInfo;
			bool flag = Helper.TryGetAndRemoveCallback<OnQueryDefinitionsCompleteCallbackInfoInternal, OnQueryDefinitionsCompleteCallback, OnQueryDefinitionsCompleteCallbackInfo>(ref data, out onQueryDefinitionsCompleteCallback, out onQueryDefinitionsCompleteCallbackInfo);
			if (flag)
			{
				onQueryDefinitionsCompleteCallback(ref onQueryDefinitionsCompleteCallbackInfo);
			}
		}

		// Token: 0x06002F85 RID: 12165 RVA: 0x00046A18 File Offset: 0x00044C18
		[MonoPInvokeCallback(typeof(OnQueryPlayerAchievementsCompleteCallbackInternal))]
		internal static void OnQueryPlayerAchievementsCompleteCallbackInternalImplementation(ref OnQueryPlayerAchievementsCompleteCallbackInfoInternal data)
		{
			OnQueryPlayerAchievementsCompleteCallback onQueryPlayerAchievementsCompleteCallback;
			OnQueryPlayerAchievementsCompleteCallbackInfo onQueryPlayerAchievementsCompleteCallbackInfo;
			bool flag = Helper.TryGetAndRemoveCallback<OnQueryPlayerAchievementsCompleteCallbackInfoInternal, OnQueryPlayerAchievementsCompleteCallback, OnQueryPlayerAchievementsCompleteCallbackInfo>(ref data, out onQueryPlayerAchievementsCompleteCallback, out onQueryPlayerAchievementsCompleteCallbackInfo);
			if (flag)
			{
				onQueryPlayerAchievementsCompleteCallback(ref onQueryPlayerAchievementsCompleteCallbackInfo);
			}
		}

		// Token: 0x06002F86 RID: 12166 RVA: 0x00046A40 File Offset: 0x00044C40
		[MonoPInvokeCallback(typeof(OnUnlockAchievementsCompleteCallbackInternal))]
		internal static void OnUnlockAchievementsCompleteCallbackInternalImplementation(ref OnUnlockAchievementsCompleteCallbackInfoInternal data)
		{
			OnUnlockAchievementsCompleteCallback onUnlockAchievementsCompleteCallback;
			OnUnlockAchievementsCompleteCallbackInfo onUnlockAchievementsCompleteCallbackInfo;
			bool flag = Helper.TryGetAndRemoveCallback<OnUnlockAchievementsCompleteCallbackInfoInternal, OnUnlockAchievementsCompleteCallback, OnUnlockAchievementsCompleteCallbackInfo>(ref data, out onUnlockAchievementsCompleteCallback, out onUnlockAchievementsCompleteCallbackInfo);
			if (flag)
			{
				onUnlockAchievementsCompleteCallback(ref onUnlockAchievementsCompleteCallbackInfo);
			}
		}

		// Token: 0x04001526 RID: 5414
		public const int AchievementUnlocktimeUndefined = -1;

		// Token: 0x04001527 RID: 5415
		public const int AddnotifyachievementsunlockedApiLatest = 1;

		// Token: 0x04001528 RID: 5416
		public const int Addnotifyachievementsunlockedv2ApiLatest = 2;

		// Token: 0x04001529 RID: 5417
		public const int Copyachievementdefinitionv2ByachievementidApiLatest = 2;

		// Token: 0x0400152A RID: 5418
		public const int Copyachievementdefinitionv2ByindexApiLatest = 2;

		// Token: 0x0400152B RID: 5419
		public const int CopydefinitionbyachievementidApiLatest = 1;

		// Token: 0x0400152C RID: 5420
		public const int CopydefinitionbyindexApiLatest = 1;

		// Token: 0x0400152D RID: 5421
		public const int Copydefinitionv2ByachievementidApiLatest = 2;

		// Token: 0x0400152E RID: 5422
		public const int Copydefinitionv2ByindexApiLatest = 2;

		// Token: 0x0400152F RID: 5423
		public const int CopyplayerachievementbyachievementidApiLatest = 2;

		// Token: 0x04001530 RID: 5424
		public const int CopyplayerachievementbyindexApiLatest = 2;

		// Token: 0x04001531 RID: 5425
		public const int CopyunlockedachievementbyachievementidApiLatest = 1;

		// Token: 0x04001532 RID: 5426
		public const int CopyunlockedachievementbyindexApiLatest = 1;

		// Token: 0x04001533 RID: 5427
		public const int DefinitionApiLatest = 1;

		// Token: 0x04001534 RID: 5428
		public const int Definitionv2ApiLatest = 2;

		// Token: 0x04001535 RID: 5429
		public const int GetachievementdefinitioncountApiLatest = 1;

		// Token: 0x04001536 RID: 5430
		public const int GetplayerachievementcountApiLatest = 1;

		// Token: 0x04001537 RID: 5431
		public const int GetunlockedachievementcountApiLatest = 1;

		// Token: 0x04001538 RID: 5432
		public const int PlayerachievementApiLatest = 2;

		// Token: 0x04001539 RID: 5433
		public const int PlayerstatinfoApiLatest = 1;

		// Token: 0x0400153A RID: 5434
		public const int QuerydefinitionsApiLatest = 3;

		// Token: 0x0400153B RID: 5435
		public const int QueryplayerachievementsApiLatest = 2;

		// Token: 0x0400153C RID: 5436
		public const int StatthresholdApiLatest = 1;

		// Token: 0x0400153D RID: 5437
		public const int StatthresholdsApiLatest = 1;

		// Token: 0x0400153E RID: 5438
		public const int UnlockachievementsApiLatest = 1;

		// Token: 0x0400153F RID: 5439
		public const int UnlockedachievementApiLatest = 1;
	}
}
