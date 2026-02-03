using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Sessions
{
	// Token: 0x0200013B RID: 315
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	internal struct QueryInvitesOptionsInternal : ISettable<QueryInvitesOptions>, IDisposable
	{
		// Token: 0x170001FE RID: 510
		// (set) Token: 0x06000993 RID: 2451 RVA: 0x0000D40B File Offset: 0x0000B60B
		public ProductUserId LocalUserId
		{
			set
			{
				Helper.Set(value, ref this.m_LocalUserId);
			}
		}

		// Token: 0x06000994 RID: 2452 RVA: 0x0000D41B File Offset: 0x0000B61B
		public void Set(ref QueryInvitesOptions other)
		{
			this.m_ApiVersion = 1;
			this.LocalUserId = other.LocalUserId;
		}

		// Token: 0x06000995 RID: 2453 RVA: 0x0000D434 File Offset: 0x0000B634
		public void Set(ref QueryInvitesOptions? other)
		{
			bool flag = other != null;
			if (flag)
			{
				this.m_ApiVersion = 1;
				this.LocalUserId = other.Value.LocalUserId;
			}
		}

		// Token: 0x06000996 RID: 2454 RVA: 0x0000D46A File Offset: 0x0000B66A
		public void Dispose()
		{
			Helper.Dispose(ref this.m_LocalUserId);
		}

		// Token: 0x04000457 RID: 1111
		private int m_ApiVersion;

		// Token: 0x04000458 RID: 1112
		private IntPtr m_LocalUserId;
	}
}
