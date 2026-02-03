using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Ecom
{
	// Token: 0x0200050F RID: 1295
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	internal struct CopyEntitlementByIndexOptionsInternal : ISettable<CopyEntitlementByIndexOptions>, IDisposable
	{
		// Token: 0x170009CF RID: 2511
		// (set) Token: 0x06002204 RID: 8708 RVA: 0x00031F2C File Offset: 0x0003012C
		public EpicAccountId LocalUserId
		{
			set
			{
				Helper.Set(value, ref this.m_LocalUserId);
			}
		}

		// Token: 0x170009D0 RID: 2512
		// (set) Token: 0x06002205 RID: 8709 RVA: 0x00031F3C File Offset: 0x0003013C
		public uint EntitlementIndex
		{
			set
			{
				this.m_EntitlementIndex = value;
			}
		}

		// Token: 0x06002206 RID: 8710 RVA: 0x00031F46 File Offset: 0x00030146
		public void Set(ref CopyEntitlementByIndexOptions other)
		{
			this.m_ApiVersion = 1;
			this.LocalUserId = other.LocalUserId;
			this.EntitlementIndex = other.EntitlementIndex;
		}

		// Token: 0x06002207 RID: 8711 RVA: 0x00031F6C File Offset: 0x0003016C
		public void Set(ref CopyEntitlementByIndexOptions? other)
		{
			bool flag = other != null;
			if (flag)
			{
				this.m_ApiVersion = 1;
				this.LocalUserId = other.Value.LocalUserId;
				this.EntitlementIndex = other.Value.EntitlementIndex;
			}
		}

		// Token: 0x06002208 RID: 8712 RVA: 0x00031FB7 File Offset: 0x000301B7
		public void Dispose()
		{
			Helper.Dispose(ref this.m_LocalUserId);
		}

		// Token: 0x04000EC7 RID: 3783
		private int m_ApiVersion;

		// Token: 0x04000EC8 RID: 3784
		private IntPtr m_LocalUserId;

		// Token: 0x04000EC9 RID: 3785
		private uint m_EntitlementIndex;
	}
}
