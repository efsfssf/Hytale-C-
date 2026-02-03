using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Lobby
{
	// Token: 0x02000446 RID: 1094
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	internal struct RejectInviteOptionsInternal : ISettable<RejectInviteOptions>, IDisposable
	{
		// Token: 0x17000805 RID: 2053
		// (set) Token: 0x06001CB9 RID: 7353 RVA: 0x00029F54 File Offset: 0x00028154
		public Utf8String InviteId
		{
			set
			{
				Helper.Set(value, ref this.m_InviteId);
			}
		}

		// Token: 0x17000806 RID: 2054
		// (set) Token: 0x06001CBA RID: 7354 RVA: 0x00029F64 File Offset: 0x00028164
		public ProductUserId LocalUserId
		{
			set
			{
				Helper.Set(value, ref this.m_LocalUserId);
			}
		}

		// Token: 0x06001CBB RID: 7355 RVA: 0x00029F74 File Offset: 0x00028174
		public void Set(ref RejectInviteOptions other)
		{
			this.m_ApiVersion = 1;
			this.InviteId = other.InviteId;
			this.LocalUserId = other.LocalUserId;
		}

		// Token: 0x06001CBC RID: 7356 RVA: 0x00029F98 File Offset: 0x00028198
		public void Set(ref RejectInviteOptions? other)
		{
			bool flag = other != null;
			if (flag)
			{
				this.m_ApiVersion = 1;
				this.InviteId = other.Value.InviteId;
				this.LocalUserId = other.Value.LocalUserId;
			}
		}

		// Token: 0x06001CBD RID: 7357 RVA: 0x00029FE3 File Offset: 0x000281E3
		public void Dispose()
		{
			Helper.Dispose(ref this.m_InviteId);
			Helper.Dispose(ref this.m_LocalUserId);
		}

		// Token: 0x04000C85 RID: 3205
		private int m_ApiVersion;

		// Token: 0x04000C86 RID: 3206
		private IntPtr m_InviteId;

		// Token: 0x04000C87 RID: 3207
		private IntPtr m_LocalUserId;
	}
}
