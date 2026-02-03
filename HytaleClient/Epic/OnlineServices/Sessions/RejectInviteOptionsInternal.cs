using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Sessions
{
	// Token: 0x02000143 RID: 323
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	internal struct RejectInviteOptionsInternal : ISettable<RejectInviteOptions>, IDisposable
	{
		// Token: 0x17000213 RID: 531
		// (set) Token: 0x060009CA RID: 2506 RVA: 0x0000D927 File Offset: 0x0000BB27
		public ProductUserId LocalUserId
		{
			set
			{
				Helper.Set(value, ref this.m_LocalUserId);
			}
		}

		// Token: 0x17000214 RID: 532
		// (set) Token: 0x060009CB RID: 2507 RVA: 0x0000D937 File Offset: 0x0000BB37
		public Utf8String InviteId
		{
			set
			{
				Helper.Set(value, ref this.m_InviteId);
			}
		}

		// Token: 0x060009CC RID: 2508 RVA: 0x0000D947 File Offset: 0x0000BB47
		public void Set(ref RejectInviteOptions other)
		{
			this.m_ApiVersion = 1;
			this.LocalUserId = other.LocalUserId;
			this.InviteId = other.InviteId;
		}

		// Token: 0x060009CD RID: 2509 RVA: 0x0000D96C File Offset: 0x0000BB6C
		public void Set(ref RejectInviteOptions? other)
		{
			bool flag = other != null;
			if (flag)
			{
				this.m_ApiVersion = 1;
				this.LocalUserId = other.Value.LocalUserId;
				this.InviteId = other.Value.InviteId;
			}
		}

		// Token: 0x060009CE RID: 2510 RVA: 0x0000D9B7 File Offset: 0x0000BBB7
		public void Dispose()
		{
			Helper.Dispose(ref this.m_LocalUserId);
			Helper.Dispose(ref this.m_InviteId);
		}

		// Token: 0x0400046F RID: 1135
		private int m_ApiVersion;

		// Token: 0x04000470 RID: 1136
		private IntPtr m_LocalUserId;

		// Token: 0x04000471 RID: 1137
		private IntPtr m_InviteId;
	}
}
