using System;

namespace Epic.OnlineServices.RTCAdmin
{
	// Token: 0x02000296 RID: 662
	public sealed class RTCAdminInterface : Handle
	{
		// Token: 0x0600128F RID: 4751 RVA: 0x0001AFD8 File Offset: 0x000191D8
		public RTCAdminInterface()
		{
		}

		// Token: 0x06001290 RID: 4752 RVA: 0x0001AFE2 File Offset: 0x000191E2
		public RTCAdminInterface(IntPtr innerHandle) : base(innerHandle)
		{
		}

		// Token: 0x06001291 RID: 4753 RVA: 0x0001AFF0 File Offset: 0x000191F0
		public Result CopyUserTokenByIndex(ref CopyUserTokenByIndexOptions options, out UserToken? outUserToken)
		{
			CopyUserTokenByIndexOptionsInternal copyUserTokenByIndexOptionsInternal = default(CopyUserTokenByIndexOptionsInternal);
			copyUserTokenByIndexOptionsInternal.Set(ref options);
			IntPtr zero = IntPtr.Zero;
			Result result = Bindings.EOS_RTCAdmin_CopyUserTokenByIndex(base.InnerHandle, ref copyUserTokenByIndexOptionsInternal, ref zero);
			Helper.Dispose<CopyUserTokenByIndexOptionsInternal>(ref copyUserTokenByIndexOptionsInternal);
			Helper.Get<UserTokenInternal, UserToken>(zero, out outUserToken);
			bool flag = outUserToken != null;
			if (flag)
			{
				Bindings.EOS_RTCAdmin_UserToken_Release(zero);
			}
			return result;
		}

		// Token: 0x06001292 RID: 4754 RVA: 0x0001B050 File Offset: 0x00019250
		public Result CopyUserTokenByUserId(ref CopyUserTokenByUserIdOptions options, out UserToken? outUserToken)
		{
			CopyUserTokenByUserIdOptionsInternal copyUserTokenByUserIdOptionsInternal = default(CopyUserTokenByUserIdOptionsInternal);
			copyUserTokenByUserIdOptionsInternal.Set(ref options);
			IntPtr zero = IntPtr.Zero;
			Result result = Bindings.EOS_RTCAdmin_CopyUserTokenByUserId(base.InnerHandle, ref copyUserTokenByUserIdOptionsInternal, ref zero);
			Helper.Dispose<CopyUserTokenByUserIdOptionsInternal>(ref copyUserTokenByUserIdOptionsInternal);
			Helper.Get<UserTokenInternal, UserToken>(zero, out outUserToken);
			bool flag = outUserToken != null;
			if (flag)
			{
				Bindings.EOS_RTCAdmin_UserToken_Release(zero);
			}
			return result;
		}

		// Token: 0x06001293 RID: 4755 RVA: 0x0001B0B0 File Offset: 0x000192B0
		public void Kick(ref KickOptions options, object clientData, OnKickCompleteCallback completionDelegate)
		{
			KickOptionsInternal kickOptionsInternal = default(KickOptionsInternal);
			kickOptionsInternal.Set(ref options);
			IntPtr zero = IntPtr.Zero;
			OnKickCompleteCallbackInternal onKickCompleteCallbackInternal = new OnKickCompleteCallbackInternal(RTCAdminInterface.OnKickCompleteCallbackInternalImplementation);
			Helper.AddCallback(out zero, clientData, completionDelegate, onKickCompleteCallbackInternal, Array.Empty<Delegate>());
			Bindings.EOS_RTCAdmin_Kick(base.InnerHandle, ref kickOptionsInternal, zero, onKickCompleteCallbackInternal);
			Helper.Dispose<KickOptionsInternal>(ref kickOptionsInternal);
		}

		// Token: 0x06001294 RID: 4756 RVA: 0x0001B10C File Offset: 0x0001930C
		public void QueryJoinRoomToken(ref QueryJoinRoomTokenOptions options, object clientData, OnQueryJoinRoomTokenCompleteCallback completionDelegate)
		{
			QueryJoinRoomTokenOptionsInternal queryJoinRoomTokenOptionsInternal = default(QueryJoinRoomTokenOptionsInternal);
			queryJoinRoomTokenOptionsInternal.Set(ref options);
			IntPtr zero = IntPtr.Zero;
			OnQueryJoinRoomTokenCompleteCallbackInternal onQueryJoinRoomTokenCompleteCallbackInternal = new OnQueryJoinRoomTokenCompleteCallbackInternal(RTCAdminInterface.OnQueryJoinRoomTokenCompleteCallbackInternalImplementation);
			Helper.AddCallback(out zero, clientData, completionDelegate, onQueryJoinRoomTokenCompleteCallbackInternal, Array.Empty<Delegate>());
			Bindings.EOS_RTCAdmin_QueryJoinRoomToken(base.InnerHandle, ref queryJoinRoomTokenOptionsInternal, zero, onQueryJoinRoomTokenCompleteCallbackInternal);
			Helper.Dispose<QueryJoinRoomTokenOptionsInternal>(ref queryJoinRoomTokenOptionsInternal);
		}

		// Token: 0x06001295 RID: 4757 RVA: 0x0001B168 File Offset: 0x00019368
		public void SetParticipantHardMute(ref SetParticipantHardMuteOptions options, object clientData, OnSetParticipantHardMuteCompleteCallback completionDelegate)
		{
			SetParticipantHardMuteOptionsInternal setParticipantHardMuteOptionsInternal = default(SetParticipantHardMuteOptionsInternal);
			setParticipantHardMuteOptionsInternal.Set(ref options);
			IntPtr zero = IntPtr.Zero;
			OnSetParticipantHardMuteCompleteCallbackInternal onSetParticipantHardMuteCompleteCallbackInternal = new OnSetParticipantHardMuteCompleteCallbackInternal(RTCAdminInterface.OnSetParticipantHardMuteCompleteCallbackInternalImplementation);
			Helper.AddCallback(out zero, clientData, completionDelegate, onSetParticipantHardMuteCompleteCallbackInternal, Array.Empty<Delegate>());
			Bindings.EOS_RTCAdmin_SetParticipantHardMute(base.InnerHandle, ref setParticipantHardMuteOptionsInternal, zero, onSetParticipantHardMuteCompleteCallbackInternal);
			Helper.Dispose<SetParticipantHardMuteOptionsInternal>(ref setParticipantHardMuteOptionsInternal);
		}

		// Token: 0x06001296 RID: 4758 RVA: 0x0001B1C4 File Offset: 0x000193C4
		[MonoPInvokeCallback(typeof(OnKickCompleteCallbackInternal))]
		internal static void OnKickCompleteCallbackInternalImplementation(ref KickCompleteCallbackInfoInternal data)
		{
			OnKickCompleteCallback onKickCompleteCallback;
			KickCompleteCallbackInfo kickCompleteCallbackInfo;
			bool flag = Helper.TryGetAndRemoveCallback<KickCompleteCallbackInfoInternal, OnKickCompleteCallback, KickCompleteCallbackInfo>(ref data, out onKickCompleteCallback, out kickCompleteCallbackInfo);
			if (flag)
			{
				onKickCompleteCallback(ref kickCompleteCallbackInfo);
			}
		}

		// Token: 0x06001297 RID: 4759 RVA: 0x0001B1EC File Offset: 0x000193EC
		[MonoPInvokeCallback(typeof(OnQueryJoinRoomTokenCompleteCallbackInternal))]
		internal static void OnQueryJoinRoomTokenCompleteCallbackInternalImplementation(ref QueryJoinRoomTokenCompleteCallbackInfoInternal data)
		{
			OnQueryJoinRoomTokenCompleteCallback onQueryJoinRoomTokenCompleteCallback;
			QueryJoinRoomTokenCompleteCallbackInfo queryJoinRoomTokenCompleteCallbackInfo;
			bool flag = Helper.TryGetAndRemoveCallback<QueryJoinRoomTokenCompleteCallbackInfoInternal, OnQueryJoinRoomTokenCompleteCallback, QueryJoinRoomTokenCompleteCallbackInfo>(ref data, out onQueryJoinRoomTokenCompleteCallback, out queryJoinRoomTokenCompleteCallbackInfo);
			if (flag)
			{
				onQueryJoinRoomTokenCompleteCallback(ref queryJoinRoomTokenCompleteCallbackInfo);
			}
		}

		// Token: 0x06001298 RID: 4760 RVA: 0x0001B214 File Offset: 0x00019414
		[MonoPInvokeCallback(typeof(OnSetParticipantHardMuteCompleteCallbackInternal))]
		internal static void OnSetParticipantHardMuteCompleteCallbackInternalImplementation(ref SetParticipantHardMuteCompleteCallbackInfoInternal data)
		{
			OnSetParticipantHardMuteCompleteCallback onSetParticipantHardMuteCompleteCallback;
			SetParticipantHardMuteCompleteCallbackInfo setParticipantHardMuteCompleteCallbackInfo;
			bool flag = Helper.TryGetAndRemoveCallback<SetParticipantHardMuteCompleteCallbackInfoInternal, OnSetParticipantHardMuteCompleteCallback, SetParticipantHardMuteCompleteCallbackInfo>(ref data, out onSetParticipantHardMuteCompleteCallback, out setParticipantHardMuteCompleteCallbackInfo);
			if (flag)
			{
				onSetParticipantHardMuteCompleteCallback(ref setParticipantHardMuteCompleteCallbackInfo);
			}
		}

		// Token: 0x04000821 RID: 2081
		public const int CopyusertokenbyindexApiLatest = 2;

		// Token: 0x04000822 RID: 2082
		public const int CopyusertokenbyuseridApiLatest = 2;

		// Token: 0x04000823 RID: 2083
		public const int KickApiLatest = 1;

		// Token: 0x04000824 RID: 2084
		public const int QueryjoinroomtokenApiLatest = 2;

		// Token: 0x04000825 RID: 2085
		public const int SetparticipanthardmuteApiLatest = 1;

		// Token: 0x04000826 RID: 2086
		public const int UsertokenApiLatest = 1;
	}
}
