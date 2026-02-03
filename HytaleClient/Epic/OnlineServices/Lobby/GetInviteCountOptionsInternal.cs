using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Lobby
{
	// Token: 0x0200038C RID: 908
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	internal struct GetInviteCountOptionsInternal : ISettable<GetInviteCountOptions>, IDisposable
	{
		// Token: 0x170006C7 RID: 1735
		// (set) Token: 0x0600185E RID: 6238 RVA: 0x00023A77 File Offset: 0x00021C77
		public ProductUserId LocalUserId
		{
			set
			{
				Helper.Set(value, ref this.m_LocalUserId);
			}
		}

		// Token: 0x0600185F RID: 6239 RVA: 0x00023A87 File Offset: 0x00021C87
		public void Set(ref GetInviteCountOptions other)
		{
			this.m_ApiVersion = 1;
			this.LocalUserId = other.LocalUserId;
		}

		// Token: 0x06001860 RID: 6240 RVA: 0x00023AA0 File Offset: 0x00021CA0
		public void Set(ref GetInviteCountOptions? other)
		{
			bool flag = other != null;
			if (flag)
			{
				this.m_ApiVersion = 1;
				this.LocalUserId = other.Value.LocalUserId;
			}
		}

		// Token: 0x06001861 RID: 6241 RVA: 0x00023AD6 File Offset: 0x00021CD6
		public void Dispose()
		{
			Helper.Dispose(ref this.m_LocalUserId);
		}

		// Token: 0x04000AC9 RID: 2761
		private int m_ApiVersion;

		// Token: 0x04000ACA RID: 2762
		private IntPtr m_LocalUserId;
	}
}
