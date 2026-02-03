using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Sessions
{
	// Token: 0x02000185 RID: 389
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	internal struct SessionSearchSetSessionIdOptionsInternal : ISettable<SessionSearchSetSessionIdOptions>, IDisposable
	{
		// Token: 0x17000296 RID: 662
		// (set) Token: 0x06000B4B RID: 2891 RVA: 0x00010063 File Offset: 0x0000E263
		public Utf8String SessionId
		{
			set
			{
				Helper.Set(value, ref this.m_SessionId);
			}
		}

		// Token: 0x06000B4C RID: 2892 RVA: 0x00010073 File Offset: 0x0000E273
		public void Set(ref SessionSearchSetSessionIdOptions other)
		{
			this.m_ApiVersion = 1;
			this.SessionId = other.SessionId;
		}

		// Token: 0x06000B4D RID: 2893 RVA: 0x0001008C File Offset: 0x0000E28C
		public void Set(ref SessionSearchSetSessionIdOptions? other)
		{
			bool flag = other != null;
			if (flag)
			{
				this.m_ApiVersion = 1;
				this.SessionId = other.Value.SessionId;
			}
		}

		// Token: 0x06000B4E RID: 2894 RVA: 0x000100C2 File Offset: 0x0000E2C2
		public void Dispose()
		{
			Helper.Dispose(ref this.m_SessionId);
		}

		// Token: 0x04000525 RID: 1317
		private int m_ApiVersion;

		// Token: 0x04000526 RID: 1318
		private IntPtr m_SessionId;
	}
}
