using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Friends
{
	// Token: 0x020004DE RID: 1246
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	internal struct GetFriendAtIndexOptionsInternal : ISettable<GetFriendAtIndexOptions>, IDisposable
	{
		// Token: 0x17000933 RID: 2355
		// (set) Token: 0x06002062 RID: 8290 RVA: 0x0002F9B3 File Offset: 0x0002DBB3
		public EpicAccountId LocalUserId
		{
			set
			{
				Helper.Set(value, ref this.m_LocalUserId);
			}
		}

		// Token: 0x17000934 RID: 2356
		// (set) Token: 0x06002063 RID: 8291 RVA: 0x0002F9C3 File Offset: 0x0002DBC3
		public int Index
		{
			set
			{
				this.m_Index = value;
			}
		}

		// Token: 0x06002064 RID: 8292 RVA: 0x0002F9CD File Offset: 0x0002DBCD
		public void Set(ref GetFriendAtIndexOptions other)
		{
			this.m_ApiVersion = 1;
			this.LocalUserId = other.LocalUserId;
			this.Index = other.Index;
		}

		// Token: 0x06002065 RID: 8293 RVA: 0x0002F9F4 File Offset: 0x0002DBF4
		public void Set(ref GetFriendAtIndexOptions? other)
		{
			bool flag = other != null;
			if (flag)
			{
				this.m_ApiVersion = 1;
				this.LocalUserId = other.Value.LocalUserId;
				this.Index = other.Value.Index;
			}
		}

		// Token: 0x06002066 RID: 8294 RVA: 0x0002FA3F File Offset: 0x0002DC3F
		public void Dispose()
		{
			Helper.Dispose(ref this.m_LocalUserId);
		}

		// Token: 0x04000E1E RID: 3614
		private int m_ApiVersion;

		// Token: 0x04000E1F RID: 3615
		private IntPtr m_LocalUserId;

		// Token: 0x04000E20 RID: 3616
		private int m_Index;
	}
}
