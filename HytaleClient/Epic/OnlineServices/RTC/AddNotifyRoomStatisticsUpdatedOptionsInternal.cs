using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.RTC
{
	// Token: 0x020001B0 RID: 432
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	internal struct AddNotifyRoomStatisticsUpdatedOptionsInternal : ISettable<AddNotifyRoomStatisticsUpdatedOptions>, IDisposable
	{
		// Token: 0x170002EA RID: 746
		// (set) Token: 0x06000C6E RID: 3182 RVA: 0x0001226C File Offset: 0x0001046C
		public ProductUserId LocalUserId
		{
			set
			{
				Helper.Set(value, ref this.m_LocalUserId);
			}
		}

		// Token: 0x170002EB RID: 747
		// (set) Token: 0x06000C6F RID: 3183 RVA: 0x0001227C File Offset: 0x0001047C
		public Utf8String RoomName
		{
			set
			{
				Helper.Set(value, ref this.m_RoomName);
			}
		}

		// Token: 0x06000C70 RID: 3184 RVA: 0x0001228C File Offset: 0x0001048C
		public void Set(ref AddNotifyRoomStatisticsUpdatedOptions other)
		{
			this.m_ApiVersion = 1;
			this.LocalUserId = other.LocalUserId;
			this.RoomName = other.RoomName;
		}

		// Token: 0x06000C71 RID: 3185 RVA: 0x000122B0 File Offset: 0x000104B0
		public void Set(ref AddNotifyRoomStatisticsUpdatedOptions? other)
		{
			bool flag = other != null;
			if (flag)
			{
				this.m_ApiVersion = 1;
				this.LocalUserId = other.Value.LocalUserId;
				this.RoomName = other.Value.RoomName;
			}
		}

		// Token: 0x06000C72 RID: 3186 RVA: 0x000122FB File Offset: 0x000104FB
		public void Dispose()
		{
			Helper.Dispose(ref this.m_LocalUserId);
			Helper.Dispose(ref this.m_RoomName);
		}

		// Token: 0x040005B3 RID: 1459
		private int m_ApiVersion;

		// Token: 0x040005B4 RID: 1460
		private IntPtr m_LocalUserId;

		// Token: 0x040005B5 RID: 1461
		private IntPtr m_RoomName;
	}
}
