using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Auth
{
	// Token: 0x02000642 RID: 1602
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	internal struct LoginCallbackInfoInternal : ICallbackInfoInternal, IGettable<LoginCallbackInfo>, ISettable<LoginCallbackInfo>, IDisposable
	{
		// Token: 0x17000C20 RID: 3104
		// (get) Token: 0x06002989 RID: 10633 RVA: 0x0003D3A8 File Offset: 0x0003B5A8
		// (set) Token: 0x0600298A RID: 10634 RVA: 0x0003D3C0 File Offset: 0x0003B5C0
		public Result ResultCode
		{
			get
			{
				return this.m_ResultCode;
			}
			set
			{
				this.m_ResultCode = value;
			}
		}

		// Token: 0x17000C21 RID: 3105
		// (get) Token: 0x0600298B RID: 10635 RVA: 0x0003D3CC File Offset: 0x0003B5CC
		// (set) Token: 0x0600298C RID: 10636 RVA: 0x0003D3ED File Offset: 0x0003B5ED
		public object ClientData
		{
			get
			{
				object result;
				Helper.Get(this.m_ClientData, out result);
				return result;
			}
			set
			{
				Helper.Set(value, ref this.m_ClientData);
			}
		}

		// Token: 0x17000C22 RID: 3106
		// (get) Token: 0x0600298D RID: 10637 RVA: 0x0003D400 File Offset: 0x0003B600
		public IntPtr ClientDataAddress
		{
			get
			{
				return this.m_ClientData;
			}
		}

		// Token: 0x17000C23 RID: 3107
		// (get) Token: 0x0600298E RID: 10638 RVA: 0x0003D418 File Offset: 0x0003B618
		// (set) Token: 0x0600298F RID: 10639 RVA: 0x0003D439 File Offset: 0x0003B639
		public EpicAccountId LocalUserId
		{
			get
			{
				EpicAccountId result;
				Helper.Get<EpicAccountId>(this.m_LocalUserId, out result);
				return result;
			}
			set
			{
				Helper.Set(value, ref this.m_LocalUserId);
			}
		}

		// Token: 0x17000C24 RID: 3108
		// (get) Token: 0x06002990 RID: 10640 RVA: 0x0003D44C File Offset: 0x0003B64C
		// (set) Token: 0x06002991 RID: 10641 RVA: 0x0003D46D File Offset: 0x0003B66D
		public PinGrantInfo? PinGrantInfo
		{
			get
			{
				PinGrantInfo? result;
				Helper.Get<PinGrantInfoInternal, PinGrantInfo>(this.m_PinGrantInfo, out result);
				return result;
			}
			set
			{
				Helper.Set<PinGrantInfo, PinGrantInfoInternal>(ref value, ref this.m_PinGrantInfo);
			}
		}

		// Token: 0x17000C25 RID: 3109
		// (get) Token: 0x06002992 RID: 10642 RVA: 0x0003D480 File Offset: 0x0003B680
		// (set) Token: 0x06002993 RID: 10643 RVA: 0x0003D4A1 File Offset: 0x0003B6A1
		public ContinuanceToken ContinuanceToken
		{
			get
			{
				ContinuanceToken result;
				Helper.Get<ContinuanceToken>(this.m_ContinuanceToken, out result);
				return result;
			}
			set
			{
				Helper.Set(value, ref this.m_ContinuanceToken);
			}
		}

		// Token: 0x17000C26 RID: 3110
		// (get) Token: 0x06002994 RID: 10644 RVA: 0x0003D4B4 File Offset: 0x0003B6B4
		// (set) Token: 0x06002995 RID: 10645 RVA: 0x0003D4D5 File Offset: 0x0003B6D5
		public AccountFeatureRestrictedInfo? AccountFeatureRestrictedInfo_DEPRECATED
		{
			get
			{
				AccountFeatureRestrictedInfo? result;
				Helper.Get<AccountFeatureRestrictedInfoInternal, AccountFeatureRestrictedInfo>(this.m_AccountFeatureRestrictedInfo_DEPRECATED, out result);
				return result;
			}
			set
			{
				Helper.Set<AccountFeatureRestrictedInfo, AccountFeatureRestrictedInfoInternal>(ref value, ref this.m_AccountFeatureRestrictedInfo_DEPRECATED);
			}
		}

		// Token: 0x17000C27 RID: 3111
		// (get) Token: 0x06002996 RID: 10646 RVA: 0x0003D4E8 File Offset: 0x0003B6E8
		// (set) Token: 0x06002997 RID: 10647 RVA: 0x0003D509 File Offset: 0x0003B709
		public EpicAccountId SelectedAccountId
		{
			get
			{
				EpicAccountId result;
				Helper.Get<EpicAccountId>(this.m_SelectedAccountId, out result);
				return result;
			}
			set
			{
				Helper.Set(value, ref this.m_SelectedAccountId);
			}
		}

		// Token: 0x06002998 RID: 10648 RVA: 0x0003D51C File Offset: 0x0003B71C
		public void Set(ref LoginCallbackInfo other)
		{
			this.ResultCode = other.ResultCode;
			this.ClientData = other.ClientData;
			this.LocalUserId = other.LocalUserId;
			this.PinGrantInfo = other.PinGrantInfo;
			this.ContinuanceToken = other.ContinuanceToken;
			this.AccountFeatureRestrictedInfo_DEPRECATED = other.AccountFeatureRestrictedInfo_DEPRECATED;
			this.SelectedAccountId = other.SelectedAccountId;
		}

		// Token: 0x06002999 RID: 10649 RVA: 0x0003D588 File Offset: 0x0003B788
		public void Set(ref LoginCallbackInfo? other)
		{
			bool flag = other != null;
			if (flag)
			{
				this.ResultCode = other.Value.ResultCode;
				this.ClientData = other.Value.ClientData;
				this.LocalUserId = other.Value.LocalUserId;
				this.PinGrantInfo = other.Value.PinGrantInfo;
				this.ContinuanceToken = other.Value.ContinuanceToken;
				this.AccountFeatureRestrictedInfo_DEPRECATED = other.Value.AccountFeatureRestrictedInfo_DEPRECATED;
				this.SelectedAccountId = other.Value.SelectedAccountId;
			}
		}

		// Token: 0x0600299A RID: 10650 RVA: 0x0003D638 File Offset: 0x0003B838
		public void Dispose()
		{
			Helper.Dispose(ref this.m_ClientData);
			Helper.Dispose(ref this.m_LocalUserId);
			Helper.Dispose(ref this.m_PinGrantInfo);
			Helper.Dispose(ref this.m_ContinuanceToken);
			Helper.Dispose(ref this.m_AccountFeatureRestrictedInfo_DEPRECATED);
			Helper.Dispose(ref this.m_SelectedAccountId);
		}

		// Token: 0x0600299B RID: 10651 RVA: 0x0003D68E File Offset: 0x0003B88E
		public void Get(out LoginCallbackInfo output)
		{
			output = default(LoginCallbackInfo);
			output.Set(ref this);
		}

		// Token: 0x040011DF RID: 4575
		private Result m_ResultCode;

		// Token: 0x040011E0 RID: 4576
		private IntPtr m_ClientData;

		// Token: 0x040011E1 RID: 4577
		private IntPtr m_LocalUserId;

		// Token: 0x040011E2 RID: 4578
		private IntPtr m_PinGrantInfo;

		// Token: 0x040011E3 RID: 4579
		private IntPtr m_ContinuanceToken;

		// Token: 0x040011E4 RID: 4580
		private IntPtr m_AccountFeatureRestrictedInfo_DEPRECATED;

		// Token: 0x040011E5 RID: 4581
		private IntPtr m_SelectedAccountId;
	}
}
