using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Friends
{
	// Token: 0x020004DA RID: 1242
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	internal struct GetBlockedUserAtIndexOptionsInternal : ISettable<GetBlockedUserAtIndexOptions>, IDisposable
	{
		// Token: 0x1700092D RID: 2349
		// (set) Token: 0x06002053 RID: 8275 RVA: 0x0002F879 File Offset: 0x0002DA79
		public EpicAccountId LocalUserId
		{
			set
			{
				Helper.Set(value, ref this.m_LocalUserId);
			}
		}

		// Token: 0x1700092E RID: 2350
		// (set) Token: 0x06002054 RID: 8276 RVA: 0x0002F889 File Offset: 0x0002DA89
		public int Index
		{
			set
			{
				this.m_Index = value;
			}
		}

		// Token: 0x06002055 RID: 8277 RVA: 0x0002F893 File Offset: 0x0002DA93
		public void Set(ref GetBlockedUserAtIndexOptions other)
		{
			this.m_ApiVersion = 1;
			this.LocalUserId = other.LocalUserId;
			this.Index = other.Index;
		}

		// Token: 0x06002056 RID: 8278 RVA: 0x0002F8B8 File Offset: 0x0002DAB8
		public void Set(ref GetBlockedUserAtIndexOptions? other)
		{
			bool flag = other != null;
			if (flag)
			{
				this.m_ApiVersion = 1;
				this.LocalUserId = other.Value.LocalUserId;
				this.Index = other.Value.Index;
			}
		}

		// Token: 0x06002057 RID: 8279 RVA: 0x0002F903 File Offset: 0x0002DB03
		public void Dispose()
		{
			Helper.Dispose(ref this.m_LocalUserId);
		}

		// Token: 0x04000E16 RID: 3606
		private int m_ApiVersion;

		// Token: 0x04000E17 RID: 3607
		private IntPtr m_LocalUserId;

		// Token: 0x04000E18 RID: 3608
		private int m_Index;
	}
}
