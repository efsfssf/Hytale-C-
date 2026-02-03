using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.CustomInvites
{
	// Token: 0x02000587 RID: 1415
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	internal struct AddNotifySendCustomNativeInviteRequestedOptionsInternal : ISettable<AddNotifySendCustomNativeInviteRequestedOptions>, IDisposable
	{
		// Token: 0x0600249C RID: 9372 RVA: 0x00035E44 File Offset: 0x00034044
		public void Set(ref AddNotifySendCustomNativeInviteRequestedOptions other)
		{
			this.m_ApiVersion = 1;
		}

		// Token: 0x0600249D RID: 9373 RVA: 0x00035E50 File Offset: 0x00034050
		public void Set(ref AddNotifySendCustomNativeInviteRequestedOptions? other)
		{
			bool flag = other != null;
			if (flag)
			{
				this.m_ApiVersion = 1;
			}
		}

		// Token: 0x0600249E RID: 9374 RVA: 0x00035E71 File Offset: 0x00034071
		public void Dispose()
		{
		}

		// Token: 0x0400100B RID: 4107
		private int m_ApiVersion;
	}
}
