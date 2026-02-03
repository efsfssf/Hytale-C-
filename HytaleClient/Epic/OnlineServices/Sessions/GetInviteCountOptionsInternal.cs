using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Sessions
{
	// Token: 0x02000109 RID: 265
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	internal struct GetInviteCountOptionsInternal : ISettable<GetInviteCountOptions>, IDisposable
	{
		// Token: 0x170001D2 RID: 466
		// (set) Token: 0x060008A4 RID: 2212 RVA: 0x0000C9C6 File Offset: 0x0000ABC6
		public ProductUserId LocalUserId
		{
			set
			{
				Helper.Set(value, ref this.m_LocalUserId);
			}
		}

		// Token: 0x060008A5 RID: 2213 RVA: 0x0000C9D6 File Offset: 0x0000ABD6
		public void Set(ref GetInviteCountOptions other)
		{
			this.m_ApiVersion = 1;
			this.LocalUserId = other.LocalUserId;
		}

		// Token: 0x060008A6 RID: 2214 RVA: 0x0000C9F0 File Offset: 0x0000ABF0
		public void Set(ref GetInviteCountOptions? other)
		{
			bool flag = other != null;
			if (flag)
			{
				this.m_ApiVersion = 1;
				this.LocalUserId = other.Value.LocalUserId;
			}
		}

		// Token: 0x060008A7 RID: 2215 RVA: 0x0000CA26 File Offset: 0x0000AC26
		public void Dispose()
		{
			Helper.Dispose(ref this.m_LocalUserId);
		}

		// Token: 0x0400041E RID: 1054
		private int m_ApiVersion;

		// Token: 0x0400041F RID: 1055
		private IntPtr m_LocalUserId;
	}
}
