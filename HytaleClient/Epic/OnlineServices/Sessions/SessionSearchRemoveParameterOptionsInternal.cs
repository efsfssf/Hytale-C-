using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Sessions
{
	// Token: 0x0200017F RID: 383
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	internal struct SessionSearchRemoveParameterOptionsInternal : ISettable<SessionSearchRemoveParameterOptions>, IDisposable
	{
		// Token: 0x1700028D RID: 653
		// (set) Token: 0x06000B35 RID: 2869 RVA: 0x0000FE92 File Offset: 0x0000E092
		public Utf8String Key
		{
			set
			{
				Helper.Set(value, ref this.m_Key);
			}
		}

		// Token: 0x1700028E RID: 654
		// (set) Token: 0x06000B36 RID: 2870 RVA: 0x0000FEA2 File Offset: 0x0000E0A2
		public ComparisonOp ComparisonOp
		{
			set
			{
				this.m_ComparisonOp = value;
			}
		}

		// Token: 0x06000B37 RID: 2871 RVA: 0x0000FEAC File Offset: 0x0000E0AC
		public void Set(ref SessionSearchRemoveParameterOptions other)
		{
			this.m_ApiVersion = 1;
			this.Key = other.Key;
			this.ComparisonOp = other.ComparisonOp;
		}

		// Token: 0x06000B38 RID: 2872 RVA: 0x0000FED0 File Offset: 0x0000E0D0
		public void Set(ref SessionSearchRemoveParameterOptions? other)
		{
			bool flag = other != null;
			if (flag)
			{
				this.m_ApiVersion = 1;
				this.Key = other.Value.Key;
				this.ComparisonOp = other.Value.ComparisonOp;
			}
		}

		// Token: 0x06000B39 RID: 2873 RVA: 0x0000FF1B File Offset: 0x0000E11B
		public void Dispose()
		{
			Helper.Dispose(ref this.m_Key);
		}

		// Token: 0x04000519 RID: 1305
		private int m_ApiVersion;

		// Token: 0x0400051A RID: 1306
		private IntPtr m_Key;

		// Token: 0x0400051B RID: 1307
		private ComparisonOp m_ComparisonOp;
	}
}
