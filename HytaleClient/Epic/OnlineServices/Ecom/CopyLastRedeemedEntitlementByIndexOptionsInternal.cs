using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Ecom
{
	// Token: 0x02000519 RID: 1305
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	internal struct CopyLastRedeemedEntitlementByIndexOptionsInternal : ISettable<CopyLastRedeemedEntitlementByIndexOptions>, IDisposable
	{
		// Token: 0x170009E9 RID: 2537
		// (set) Token: 0x0600223A RID: 8762 RVA: 0x000323D5 File Offset: 0x000305D5
		public EpicAccountId LocalUserId
		{
			set
			{
				Helper.Set(value, ref this.m_LocalUserId);
			}
		}

		// Token: 0x170009EA RID: 2538
		// (set) Token: 0x0600223B RID: 8763 RVA: 0x000323E5 File Offset: 0x000305E5
		public uint RedeemedEntitlementIndex
		{
			set
			{
				this.m_RedeemedEntitlementIndex = value;
			}
		}

		// Token: 0x0600223C RID: 8764 RVA: 0x000323EF File Offset: 0x000305EF
		public void Set(ref CopyLastRedeemedEntitlementByIndexOptions other)
		{
			this.m_ApiVersion = 1;
			this.LocalUserId = other.LocalUserId;
			this.RedeemedEntitlementIndex = other.RedeemedEntitlementIndex;
		}

		// Token: 0x0600223D RID: 8765 RVA: 0x00032414 File Offset: 0x00030614
		public void Set(ref CopyLastRedeemedEntitlementByIndexOptions? other)
		{
			bool flag = other != null;
			if (flag)
			{
				this.m_ApiVersion = 1;
				this.LocalUserId = other.Value.LocalUserId;
				this.RedeemedEntitlementIndex = other.Value.RedeemedEntitlementIndex;
			}
		}

		// Token: 0x0600223E RID: 8766 RVA: 0x0003245F File Offset: 0x0003065F
		public void Dispose()
		{
			Helper.Dispose(ref this.m_LocalUserId);
		}

		// Token: 0x04000EE6 RID: 3814
		private int m_ApiVersion;

		// Token: 0x04000EE7 RID: 3815
		private IntPtr m_LocalUserId;

		// Token: 0x04000EE8 RID: 3816
		private uint m_RedeemedEntitlementIndex;
	}
}
