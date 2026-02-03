using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Sessions
{
	// Token: 0x02000103 RID: 259
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	internal struct DumpSessionStateOptionsInternal : ISettable<DumpSessionStateOptions>, IDisposable
	{
		// Token: 0x170001C9 RID: 457
		// (set) Token: 0x06000889 RID: 2185 RVA: 0x0000C772 File Offset: 0x0000A972
		public Utf8String SessionName
		{
			set
			{
				Helper.Set(value, ref this.m_SessionName);
			}
		}

		// Token: 0x0600088A RID: 2186 RVA: 0x0000C782 File Offset: 0x0000A982
		public void Set(ref DumpSessionStateOptions other)
		{
			this.m_ApiVersion = 1;
			this.SessionName = other.SessionName;
		}

		// Token: 0x0600088B RID: 2187 RVA: 0x0000C79C File Offset: 0x0000A99C
		public void Set(ref DumpSessionStateOptions? other)
		{
			bool flag = other != null;
			if (flag)
			{
				this.m_ApiVersion = 1;
				this.SessionName = other.Value.SessionName;
			}
		}

		// Token: 0x0600088C RID: 2188 RVA: 0x0000C7D2 File Offset: 0x0000A9D2
		public void Dispose()
		{
			Helper.Dispose(ref this.m_SessionName);
		}

		// Token: 0x04000414 RID: 1044
		private int m_ApiVersion;

		// Token: 0x04000415 RID: 1045
		private IntPtr m_SessionName;
	}
}
