using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Sessions
{
	// Token: 0x02000107 RID: 263
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	internal struct EndSessionOptionsInternal : ISettable<EndSessionOptions>, IDisposable
	{
		// Token: 0x170001D0 RID: 464
		// (set) Token: 0x0600089E RID: 2206 RVA: 0x0000C946 File Offset: 0x0000AB46
		public Utf8String SessionName
		{
			set
			{
				Helper.Set(value, ref this.m_SessionName);
			}
		}

		// Token: 0x0600089F RID: 2207 RVA: 0x0000C956 File Offset: 0x0000AB56
		public void Set(ref EndSessionOptions other)
		{
			this.m_ApiVersion = 1;
			this.SessionName = other.SessionName;
		}

		// Token: 0x060008A0 RID: 2208 RVA: 0x0000C970 File Offset: 0x0000AB70
		public void Set(ref EndSessionOptions? other)
		{
			bool flag = other != null;
			if (flag)
			{
				this.m_ApiVersion = 1;
				this.SessionName = other.Value.SessionName;
			}
		}

		// Token: 0x060008A1 RID: 2209 RVA: 0x0000C9A6 File Offset: 0x0000ABA6
		public void Dispose()
		{
			Helper.Dispose(ref this.m_SessionName);
		}

		// Token: 0x0400041B RID: 1051
		private int m_ApiVersion;

		// Token: 0x0400041C RID: 1052
		private IntPtr m_SessionName;
	}
}
