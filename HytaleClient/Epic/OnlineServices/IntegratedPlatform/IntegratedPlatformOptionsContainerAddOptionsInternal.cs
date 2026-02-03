using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.IntegratedPlatform
{
	// Token: 0x020004BD RID: 1213
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	internal struct IntegratedPlatformOptionsContainerAddOptionsInternal : ISettable<IntegratedPlatformOptionsContainerAddOptions>, IDisposable
	{
		// Token: 0x170008ED RID: 2285
		// (set) Token: 0x06001F89 RID: 8073 RVA: 0x0002E386 File Offset: 0x0002C586
		public Options? Options
		{
			set
			{
				Helper.Set<Options, OptionsInternal>(ref value, ref this.m_Options);
			}
		}

		// Token: 0x06001F8A RID: 8074 RVA: 0x0002E397 File Offset: 0x0002C597
		public void Set(ref IntegratedPlatformOptionsContainerAddOptions other)
		{
			this.m_ApiVersion = 1;
			this.Options = other.Options;
		}

		// Token: 0x06001F8B RID: 8075 RVA: 0x0002E3B0 File Offset: 0x0002C5B0
		public void Set(ref IntegratedPlatformOptionsContainerAddOptions? other)
		{
			bool flag = other != null;
			if (flag)
			{
				this.m_ApiVersion = 1;
				this.Options = other.Value.Options;
			}
		}

		// Token: 0x06001F8C RID: 8076 RVA: 0x0002E3E6 File Offset: 0x0002C5E6
		public void Dispose()
		{
			Helper.Dispose(ref this.m_Options);
		}

		// Token: 0x04000DBE RID: 3518
		private int m_ApiVersion;

		// Token: 0x04000DBF RID: 3519
		private IntPtr m_Options;
	}
}
