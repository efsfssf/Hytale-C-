using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Ecom
{
	// Token: 0x0200050D RID: 1293
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	internal struct CopyEntitlementByIdOptionsInternal : ISettable<CopyEntitlementByIdOptions>, IDisposable
	{
		// Token: 0x170009CB RID: 2507
		// (set) Token: 0x060021FB RID: 8699 RVA: 0x00031E5E File Offset: 0x0003005E
		public EpicAccountId LocalUserId
		{
			set
			{
				Helper.Set(value, ref this.m_LocalUserId);
			}
		}

		// Token: 0x170009CC RID: 2508
		// (set) Token: 0x060021FC RID: 8700 RVA: 0x00031E6E File Offset: 0x0003006E
		public Utf8String EntitlementId
		{
			set
			{
				Helper.Set(value, ref this.m_EntitlementId);
			}
		}

		// Token: 0x060021FD RID: 8701 RVA: 0x00031E7E File Offset: 0x0003007E
		public void Set(ref CopyEntitlementByIdOptions other)
		{
			this.m_ApiVersion = 2;
			this.LocalUserId = other.LocalUserId;
			this.EntitlementId = other.EntitlementId;
		}

		// Token: 0x060021FE RID: 8702 RVA: 0x00031EA4 File Offset: 0x000300A4
		public void Set(ref CopyEntitlementByIdOptions? other)
		{
			bool flag = other != null;
			if (flag)
			{
				this.m_ApiVersion = 2;
				this.LocalUserId = other.Value.LocalUserId;
				this.EntitlementId = other.Value.EntitlementId;
			}
		}

		// Token: 0x060021FF RID: 8703 RVA: 0x00031EEF File Offset: 0x000300EF
		public void Dispose()
		{
			Helper.Dispose(ref this.m_LocalUserId);
			Helper.Dispose(ref this.m_EntitlementId);
		}

		// Token: 0x04000EC2 RID: 3778
		private int m_ApiVersion;

		// Token: 0x04000EC3 RID: 3779
		private IntPtr m_LocalUserId;

		// Token: 0x04000EC4 RID: 3780
		private IntPtr m_EntitlementId;
	}
}
