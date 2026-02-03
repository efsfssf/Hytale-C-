using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Ecom
{
	// Token: 0x02000511 RID: 1297
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	internal struct CopyEntitlementByNameAndIndexOptionsInternal : ISettable<CopyEntitlementByNameAndIndexOptions>, IDisposable
	{
		// Token: 0x170009D4 RID: 2516
		// (set) Token: 0x0600220F RID: 8719 RVA: 0x00031FF9 File Offset: 0x000301F9
		public EpicAccountId LocalUserId
		{
			set
			{
				Helper.Set(value, ref this.m_LocalUserId);
			}
		}

		// Token: 0x170009D5 RID: 2517
		// (set) Token: 0x06002210 RID: 8720 RVA: 0x00032009 File Offset: 0x00030209
		public Utf8String EntitlementName
		{
			set
			{
				Helper.Set(value, ref this.m_EntitlementName);
			}
		}

		// Token: 0x170009D6 RID: 2518
		// (set) Token: 0x06002211 RID: 8721 RVA: 0x00032019 File Offset: 0x00030219
		public uint Index
		{
			set
			{
				this.m_Index = value;
			}
		}

		// Token: 0x06002212 RID: 8722 RVA: 0x00032023 File Offset: 0x00030223
		public void Set(ref CopyEntitlementByNameAndIndexOptions other)
		{
			this.m_ApiVersion = 1;
			this.LocalUserId = other.LocalUserId;
			this.EntitlementName = other.EntitlementName;
			this.Index = other.Index;
		}

		// Token: 0x06002213 RID: 8723 RVA: 0x00032054 File Offset: 0x00030254
		public void Set(ref CopyEntitlementByNameAndIndexOptions? other)
		{
			bool flag = other != null;
			if (flag)
			{
				this.m_ApiVersion = 1;
				this.LocalUserId = other.Value.LocalUserId;
				this.EntitlementName = other.Value.EntitlementName;
				this.Index = other.Value.Index;
			}
		}

		// Token: 0x06002214 RID: 8724 RVA: 0x000320B4 File Offset: 0x000302B4
		public void Dispose()
		{
			Helper.Dispose(ref this.m_LocalUserId);
			Helper.Dispose(ref this.m_EntitlementName);
		}

		// Token: 0x04000ECD RID: 3789
		private int m_ApiVersion;

		// Token: 0x04000ECE RID: 3790
		private IntPtr m_LocalUserId;

		// Token: 0x04000ECF RID: 3791
		private IntPtr m_EntitlementName;

		// Token: 0x04000ED0 RID: 3792
		private uint m_Index;
	}
}
