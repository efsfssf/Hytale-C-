using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Lobby
{
	// Token: 0x02000442 RID: 1090
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	internal struct QueryInvitesOptionsInternal : ISettable<QueryInvitesOptions>, IDisposable
	{
		// Token: 0x170007FB RID: 2043
		// (set) Token: 0x06001C9E RID: 7326 RVA: 0x00029CF7 File Offset: 0x00027EF7
		public ProductUserId LocalUserId
		{
			set
			{
				Helper.Set(value, ref this.m_LocalUserId);
			}
		}

		// Token: 0x06001C9F RID: 7327 RVA: 0x00029D07 File Offset: 0x00027F07
		public void Set(ref QueryInvitesOptions other)
		{
			this.m_ApiVersion = 1;
			this.LocalUserId = other.LocalUserId;
		}

		// Token: 0x06001CA0 RID: 7328 RVA: 0x00029D20 File Offset: 0x00027F20
		public void Set(ref QueryInvitesOptions? other)
		{
			bool flag = other != null;
			if (flag)
			{
				this.m_ApiVersion = 1;
				this.LocalUserId = other.Value.LocalUserId;
			}
		}

		// Token: 0x06001CA1 RID: 7329 RVA: 0x00029D56 File Offset: 0x00027F56
		public void Dispose()
		{
			Helper.Dispose(ref this.m_LocalUserId);
		}

		// Token: 0x04000C7B RID: 3195
		private int m_ApiVersion;

		// Token: 0x04000C7C RID: 3196
		private IntPtr m_LocalUserId;
	}
}
