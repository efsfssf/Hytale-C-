using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Sessions
{
	// Token: 0x020000F3 RID: 243
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	internal struct CopyActiveSessionHandleOptionsInternal : ISettable<CopyActiveSessionHandleOptions>, IDisposable
	{
		// Token: 0x170001A8 RID: 424
		// (set) Token: 0x0600083B RID: 2107 RVA: 0x0000C0AF File Offset: 0x0000A2AF
		public Utf8String SessionName
		{
			set
			{
				Helper.Set(value, ref this.m_SessionName);
			}
		}

		// Token: 0x0600083C RID: 2108 RVA: 0x0000C0BF File Offset: 0x0000A2BF
		public void Set(ref CopyActiveSessionHandleOptions other)
		{
			this.m_ApiVersion = 1;
			this.SessionName = other.SessionName;
		}

		// Token: 0x0600083D RID: 2109 RVA: 0x0000C0D8 File Offset: 0x0000A2D8
		public void Set(ref CopyActiveSessionHandleOptions? other)
		{
			bool flag = other != null;
			if (flag)
			{
				this.m_ApiVersion = 1;
				this.SessionName = other.Value.SessionName;
			}
		}

		// Token: 0x0600083E RID: 2110 RVA: 0x0000C10E File Offset: 0x0000A30E
		public void Dispose()
		{
			Helper.Dispose(ref this.m_SessionName);
		}

		// Token: 0x040003EC RID: 1004
		private int m_ApiVersion;

		// Token: 0x040003ED RID: 1005
		private IntPtr m_SessionName;
	}
}
