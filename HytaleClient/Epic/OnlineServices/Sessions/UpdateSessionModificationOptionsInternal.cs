using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Sessions
{
	// Token: 0x02000194 RID: 404
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	internal struct UpdateSessionModificationOptionsInternal : ISettable<UpdateSessionModificationOptions>, IDisposable
	{
		// Token: 0x170002B5 RID: 693
		// (set) Token: 0x06000BD3 RID: 3027 RVA: 0x000114D0 File Offset: 0x0000F6D0
		public Utf8String SessionName
		{
			set
			{
				Helper.Set(value, ref this.m_SessionName);
			}
		}

		// Token: 0x06000BD4 RID: 3028 RVA: 0x000114E0 File Offset: 0x0000F6E0
		public void Set(ref UpdateSessionModificationOptions other)
		{
			this.m_ApiVersion = 1;
			this.SessionName = other.SessionName;
		}

		// Token: 0x06000BD5 RID: 3029 RVA: 0x000114F8 File Offset: 0x0000F6F8
		public void Set(ref UpdateSessionModificationOptions? other)
		{
			bool flag = other != null;
			if (flag)
			{
				this.m_ApiVersion = 1;
				this.SessionName = other.Value.SessionName;
			}
		}

		// Token: 0x06000BD6 RID: 3030 RVA: 0x0001152E File Offset: 0x0000F72E
		public void Dispose()
		{
			Helper.Dispose(ref this.m_SessionName);
		}

		// Token: 0x0400056C RID: 1388
		private int m_ApiVersion;

		// Token: 0x0400056D RID: 1389
		private IntPtr m_SessionName;
	}
}
