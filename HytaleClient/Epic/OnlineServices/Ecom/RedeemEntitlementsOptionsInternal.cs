using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Ecom
{
	// Token: 0x0200056C RID: 1388
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	internal struct RedeemEntitlementsOptionsInternal : ISettable<RedeemEntitlementsOptions>, IDisposable
	{
		// Token: 0x17000A98 RID: 2712
		// (set) Token: 0x06002446 RID: 9286 RVA: 0x0003564D File Offset: 0x0003384D
		public EpicAccountId LocalUserId
		{
			set
			{
				Helper.Set(value, ref this.m_LocalUserId);
			}
		}

		// Token: 0x17000A99 RID: 2713
		// (set) Token: 0x06002447 RID: 9287 RVA: 0x0003565D File Offset: 0x0003385D
		public Utf8String[] EntitlementIds
		{
			set
			{
				Helper.Set<Utf8String>(value, ref this.m_EntitlementIds, out this.m_EntitlementIdCount);
			}
		}

		// Token: 0x06002448 RID: 9288 RVA: 0x00035673 File Offset: 0x00033873
		public void Set(ref RedeemEntitlementsOptions other)
		{
			this.m_ApiVersion = 2;
			this.LocalUserId = other.LocalUserId;
			this.EntitlementIds = other.EntitlementIds;
		}

		// Token: 0x06002449 RID: 9289 RVA: 0x00035698 File Offset: 0x00033898
		public void Set(ref RedeemEntitlementsOptions? other)
		{
			bool flag = other != null;
			if (flag)
			{
				this.m_ApiVersion = 2;
				this.LocalUserId = other.Value.LocalUserId;
				this.EntitlementIds = other.Value.EntitlementIds;
			}
		}

		// Token: 0x0600244A RID: 9290 RVA: 0x000356E3 File Offset: 0x000338E3
		public void Dispose()
		{
			Helper.Dispose(ref this.m_LocalUserId);
			Helper.Dispose(ref this.m_EntitlementIds);
		}

		// Token: 0x04000FE8 RID: 4072
		private int m_ApiVersion;

		// Token: 0x04000FE9 RID: 4073
		private IntPtr m_LocalUserId;

		// Token: 0x04000FEA RID: 4074
		private uint m_EntitlementIdCount;

		// Token: 0x04000FEB RID: 4075
		private IntPtr m_EntitlementIds;
	}
}
