using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Stats
{
	// Token: 0x020000C7 RID: 199
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	internal struct GetStatCountOptionsInternal : ISettable<GetStatCountOptions>, IDisposable
	{
		// Token: 0x17000161 RID: 353
		// (set) Token: 0x0600074C RID: 1868 RVA: 0x0000A8F3 File Offset: 0x00008AF3
		public ProductUserId TargetUserId
		{
			set
			{
				Helper.Set(value, ref this.m_TargetUserId);
			}
		}

		// Token: 0x0600074D RID: 1869 RVA: 0x0000A903 File Offset: 0x00008B03
		public void Set(ref GetStatCountOptions other)
		{
			this.m_ApiVersion = 1;
			this.TargetUserId = other.TargetUserId;
		}

		// Token: 0x0600074E RID: 1870 RVA: 0x0000A91C File Offset: 0x00008B1C
		public void Set(ref GetStatCountOptions? other)
		{
			bool flag = other != null;
			if (flag)
			{
				this.m_ApiVersion = 1;
				this.TargetUserId = other.Value.TargetUserId;
			}
		}

		// Token: 0x0600074F RID: 1871 RVA: 0x0000A952 File Offset: 0x00008B52
		public void Dispose()
		{
			Helper.Dispose(ref this.m_TargetUserId);
		}

		// Token: 0x04000385 RID: 901
		private int m_ApiVersion;

		// Token: 0x04000386 RID: 902
		private IntPtr m_TargetUserId;
	}
}
