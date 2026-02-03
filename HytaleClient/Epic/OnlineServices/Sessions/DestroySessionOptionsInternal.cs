using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Sessions
{
	// Token: 0x02000101 RID: 257
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	internal struct DestroySessionOptionsInternal : ISettable<DestroySessionOptions>, IDisposable
	{
		// Token: 0x170001C7 RID: 455
		// (set) Token: 0x06000883 RID: 2179 RVA: 0x0000C6F2 File Offset: 0x0000A8F2
		public Utf8String SessionName
		{
			set
			{
				Helper.Set(value, ref this.m_SessionName);
			}
		}

		// Token: 0x06000884 RID: 2180 RVA: 0x0000C702 File Offset: 0x0000A902
		public void Set(ref DestroySessionOptions other)
		{
			this.m_ApiVersion = 1;
			this.SessionName = other.SessionName;
		}

		// Token: 0x06000885 RID: 2181 RVA: 0x0000C71C File Offset: 0x0000A91C
		public void Set(ref DestroySessionOptions? other)
		{
			bool flag = other != null;
			if (flag)
			{
				this.m_ApiVersion = 1;
				this.SessionName = other.Value.SessionName;
			}
		}

		// Token: 0x06000886 RID: 2182 RVA: 0x0000C752 File Offset: 0x0000A952
		public void Dispose()
		{
			Helper.Dispose(ref this.m_SessionName);
		}

		// Token: 0x04000411 RID: 1041
		private int m_ApiVersion;

		// Token: 0x04000412 RID: 1042
		private IntPtr m_SessionName;
	}
}
