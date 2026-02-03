using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.RTC
{
	// Token: 0x020001BF RID: 447
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	internal struct LeaveRoomOptionsInternal : ISettable<LeaveRoomOptions>, IDisposable
	{
		// Token: 0x17000330 RID: 816
		// (set) Token: 0x06000D09 RID: 3337 RVA: 0x00013211 File Offset: 0x00011411
		public ProductUserId LocalUserId
		{
			set
			{
				Helper.Set(value, ref this.m_LocalUserId);
			}
		}

		// Token: 0x17000331 RID: 817
		// (set) Token: 0x06000D0A RID: 3338 RVA: 0x00013221 File Offset: 0x00011421
		public Utf8String RoomName
		{
			set
			{
				Helper.Set(value, ref this.m_RoomName);
			}
		}

		// Token: 0x06000D0B RID: 3339 RVA: 0x00013231 File Offset: 0x00011431
		public void Set(ref LeaveRoomOptions other)
		{
			this.m_ApiVersion = 1;
			this.LocalUserId = other.LocalUserId;
			this.RoomName = other.RoomName;
		}

		// Token: 0x06000D0C RID: 3340 RVA: 0x00013258 File Offset: 0x00011458
		public void Set(ref LeaveRoomOptions? other)
		{
			bool flag = other != null;
			if (flag)
			{
				this.m_ApiVersion = 1;
				this.LocalUserId = other.Value.LocalUserId;
				this.RoomName = other.Value.RoomName;
			}
		}

		// Token: 0x06000D0D RID: 3341 RVA: 0x000132A3 File Offset: 0x000114A3
		public void Dispose()
		{
			Helper.Dispose(ref this.m_LocalUserId);
			Helper.Dispose(ref this.m_RoomName);
		}

		// Token: 0x040005FD RID: 1533
		private int m_ApiVersion;

		// Token: 0x040005FE RID: 1534
		private IntPtr m_LocalUserId;

		// Token: 0x040005FF RID: 1535
		private IntPtr m_RoomName;
	}
}
