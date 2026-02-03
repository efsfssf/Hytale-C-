using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Sessions
{
	// Token: 0x0200010B RID: 267
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	internal struct GetInviteIdByIndexOptionsInternal : ISettable<GetInviteIdByIndexOptions>, IDisposable
	{
		// Token: 0x170001D5 RID: 469
		// (set) Token: 0x060008AC RID: 2220 RVA: 0x0000CA57 File Offset: 0x0000AC57
		public ProductUserId LocalUserId
		{
			set
			{
				Helper.Set(value, ref this.m_LocalUserId);
			}
		}

		// Token: 0x170001D6 RID: 470
		// (set) Token: 0x060008AD RID: 2221 RVA: 0x0000CA67 File Offset: 0x0000AC67
		public uint Index
		{
			set
			{
				this.m_Index = value;
			}
		}

		// Token: 0x060008AE RID: 2222 RVA: 0x0000CA71 File Offset: 0x0000AC71
		public void Set(ref GetInviteIdByIndexOptions other)
		{
			this.m_ApiVersion = 1;
			this.LocalUserId = other.LocalUserId;
			this.Index = other.Index;
		}

		// Token: 0x060008AF RID: 2223 RVA: 0x0000CA98 File Offset: 0x0000AC98
		public void Set(ref GetInviteIdByIndexOptions? other)
		{
			bool flag = other != null;
			if (flag)
			{
				this.m_ApiVersion = 1;
				this.LocalUserId = other.Value.LocalUserId;
				this.Index = other.Value.Index;
			}
		}

		// Token: 0x060008B0 RID: 2224 RVA: 0x0000CAE3 File Offset: 0x0000ACE3
		public void Dispose()
		{
			Helper.Dispose(ref this.m_LocalUserId);
		}

		// Token: 0x04000422 RID: 1058
		private int m_ApiVersion;

		// Token: 0x04000423 RID: 1059
		private IntPtr m_LocalUserId;

		// Token: 0x04000424 RID: 1060
		private uint m_Index;
	}
}
