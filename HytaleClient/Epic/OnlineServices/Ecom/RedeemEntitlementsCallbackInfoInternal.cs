using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Ecom
{
	// Token: 0x0200056A RID: 1386
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	internal struct RedeemEntitlementsCallbackInfoInternal : ICallbackInfoInternal, IGettable<RedeemEntitlementsCallbackInfo>, ISettable<RedeemEntitlementsCallbackInfo>, IDisposable
	{
		// Token: 0x17000A91 RID: 2705
		// (get) Token: 0x06002435 RID: 9269 RVA: 0x00035490 File Offset: 0x00033690
		// (set) Token: 0x06002436 RID: 9270 RVA: 0x000354A8 File Offset: 0x000336A8
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

		// Token: 0x17000A92 RID: 2706
		// (get) Token: 0x06002437 RID: 9271 RVA: 0x000354B4 File Offset: 0x000336B4
		// (set) Token: 0x06002438 RID: 9272 RVA: 0x000354D5 File Offset: 0x000336D5
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

		// Token: 0x17000A93 RID: 2707
		// (get) Token: 0x06002439 RID: 9273 RVA: 0x000354E8 File Offset: 0x000336E8
		public IntPtr ClientDataAddress
		{
			get
			{
				return this.m_ClientData;
			}
		}

		// Token: 0x17000A94 RID: 2708
		// (get) Token: 0x0600243A RID: 9274 RVA: 0x00035500 File Offset: 0x00033700
		// (set) Token: 0x0600243B RID: 9275 RVA: 0x00035521 File Offset: 0x00033721
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

		// Token: 0x17000A95 RID: 2709
		// (get) Token: 0x0600243C RID: 9276 RVA: 0x00035534 File Offset: 0x00033734
		// (set) Token: 0x0600243D RID: 9277 RVA: 0x0003554C File Offset: 0x0003374C
		public uint RedeemedEntitlementIdsCount
		{
			get
			{
				return this.m_RedeemedEntitlementIdsCount;
			}
			set
			{
				this.m_RedeemedEntitlementIdsCount = value;
			}
		}

		// Token: 0x0600243E RID: 9278 RVA: 0x00035556 File Offset: 0x00033756
		public void Set(ref RedeemEntitlementsCallbackInfo other)
		{
			this.ResultCode = other.ResultCode;
			this.ClientData = other.ClientData;
			this.LocalUserId = other.LocalUserId;
			this.RedeemedEntitlementIdsCount = other.RedeemedEntitlementIdsCount;
		}

		// Token: 0x0600243F RID: 9279 RVA: 0x00035590 File Offset: 0x00033790
		public void Set(ref RedeemEntitlementsCallbackInfo? other)
		{
			bool flag = other != null;
			if (flag)
			{
				this.ResultCode = other.Value.ResultCode;
				this.ClientData = other.Value.ClientData;
				this.LocalUserId = other.Value.LocalUserId;
				this.RedeemedEntitlementIdsCount = other.Value.RedeemedEntitlementIdsCount;
			}
		}

		// Token: 0x06002440 RID: 9280 RVA: 0x000355FE File Offset: 0x000337FE
		public void Dispose()
		{
			Helper.Dispose(ref this.m_ClientData);
			Helper.Dispose(ref this.m_LocalUserId);
		}

		// Token: 0x06002441 RID: 9281 RVA: 0x00035619 File Offset: 0x00033819
		public void Get(out RedeemEntitlementsCallbackInfo output)
		{
			output = default(RedeemEntitlementsCallbackInfo);
			output.Set(ref this);
		}

		// Token: 0x04000FE2 RID: 4066
		private Result m_ResultCode;

		// Token: 0x04000FE3 RID: 4067
		private IntPtr m_ClientData;

		// Token: 0x04000FE4 RID: 4068
		private IntPtr m_LocalUserId;

		// Token: 0x04000FE5 RID: 4069
		private uint m_RedeemedEntitlementIdsCount;
	}
}
