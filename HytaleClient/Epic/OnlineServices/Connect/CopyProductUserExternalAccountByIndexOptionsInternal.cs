using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Connect
{
	// Token: 0x020005D0 RID: 1488
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	internal struct CopyProductUserExternalAccountByIndexOptionsInternal : ISettable<CopyProductUserExternalAccountByIndexOptions>, IDisposable
	{
		// Token: 0x17000B3D RID: 2877
		// (set) Token: 0x060026B5 RID: 9909 RVA: 0x000394B4 File Offset: 0x000376B4
		public ProductUserId TargetUserId
		{
			set
			{
				Helper.Set(value, ref this.m_TargetUserId);
			}
		}

		// Token: 0x17000B3E RID: 2878
		// (set) Token: 0x060026B6 RID: 9910 RVA: 0x000394C4 File Offset: 0x000376C4
		public uint ExternalAccountInfoIndex
		{
			set
			{
				this.m_ExternalAccountInfoIndex = value;
			}
		}

		// Token: 0x060026B7 RID: 9911 RVA: 0x000394CE File Offset: 0x000376CE
		public void Set(ref CopyProductUserExternalAccountByIndexOptions other)
		{
			this.m_ApiVersion = 1;
			this.TargetUserId = other.TargetUserId;
			this.ExternalAccountInfoIndex = other.ExternalAccountInfoIndex;
		}

		// Token: 0x060026B8 RID: 9912 RVA: 0x000394F4 File Offset: 0x000376F4
		public void Set(ref CopyProductUserExternalAccountByIndexOptions? other)
		{
			bool flag = other != null;
			if (flag)
			{
				this.m_ApiVersion = 1;
				this.TargetUserId = other.Value.TargetUserId;
				this.ExternalAccountInfoIndex = other.Value.ExternalAccountInfoIndex;
			}
		}

		// Token: 0x060026B9 RID: 9913 RVA: 0x0003953F File Offset: 0x0003773F
		public void Dispose()
		{
			Helper.Dispose(ref this.m_TargetUserId);
		}

		// Token: 0x040010CE RID: 4302
		private int m_ApiVersion;

		// Token: 0x040010CF RID: 4303
		private IntPtr m_TargetUserId;

		// Token: 0x040010D0 RID: 4304
		private uint m_ExternalAccountInfoIndex;
	}
}
