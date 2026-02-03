using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Sessions
{
	// Token: 0x02000196 RID: 406
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	internal struct UpdateSessionOptionsInternal : ISettable<UpdateSessionOptions>, IDisposable
	{
		// Token: 0x170002B7 RID: 695
		// (set) Token: 0x06000BD9 RID: 3033 RVA: 0x0001154E File Offset: 0x0000F74E
		public SessionModification SessionModificationHandle
		{
			set
			{
				Helper.Set(value, ref this.m_SessionModificationHandle);
			}
		}

		// Token: 0x06000BDA RID: 3034 RVA: 0x0001155E File Offset: 0x0000F75E
		public void Set(ref UpdateSessionOptions other)
		{
			this.m_ApiVersion = 1;
			this.SessionModificationHandle = other.SessionModificationHandle;
		}

		// Token: 0x06000BDB RID: 3035 RVA: 0x00011578 File Offset: 0x0000F778
		public void Set(ref UpdateSessionOptions? other)
		{
			bool flag = other != null;
			if (flag)
			{
				this.m_ApiVersion = 1;
				this.SessionModificationHandle = other.Value.SessionModificationHandle;
			}
		}

		// Token: 0x06000BDC RID: 3036 RVA: 0x000115AE File Offset: 0x0000F7AE
		public void Dispose()
		{
			Helper.Dispose(ref this.m_SessionModificationHandle);
		}

		// Token: 0x0400056F RID: 1391
		private int m_ApiVersion;

		// Token: 0x04000570 RID: 1392
		private IntPtr m_SessionModificationHandle;
	}
}
