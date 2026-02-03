using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Presence
{
	// Token: 0x020002BF RID: 703
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	internal struct CreatePresenceModificationOptionsInternal : ISettable<CreatePresenceModificationOptions>, IDisposable
	{
		// Token: 0x17000528 RID: 1320
		// (set) Token: 0x06001362 RID: 4962 RVA: 0x0001C30B File Offset: 0x0001A50B
		public EpicAccountId LocalUserId
		{
			set
			{
				Helper.Set(value, ref this.m_LocalUserId);
			}
		}

		// Token: 0x06001363 RID: 4963 RVA: 0x0001C31B File Offset: 0x0001A51B
		public void Set(ref CreatePresenceModificationOptions other)
		{
			this.m_ApiVersion = 1;
			this.LocalUserId = other.LocalUserId;
		}

		// Token: 0x06001364 RID: 4964 RVA: 0x0001C334 File Offset: 0x0001A534
		public void Set(ref CreatePresenceModificationOptions? other)
		{
			bool flag = other != null;
			if (flag)
			{
				this.m_ApiVersion = 1;
				this.LocalUserId = other.Value.LocalUserId;
			}
		}

		// Token: 0x06001365 RID: 4965 RVA: 0x0001C36A File Offset: 0x0001A56A
		public void Dispose()
		{
			Helper.Dispose(ref this.m_LocalUserId);
		}

		// Token: 0x0400087F RID: 2175
		private int m_ApiVersion;

		// Token: 0x04000880 RID: 2176
		private IntPtr m_LocalUserId;
	}
}
