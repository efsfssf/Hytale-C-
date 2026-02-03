using System;

namespace Epic.OnlineServices.RTC
{
	// Token: 0x020001D2 RID: 466
	public struct RoomStatisticsUpdatedInfo : ICallbackInfo
	{
		// Token: 0x17000349 RID: 841
		// (get) Token: 0x06000D7B RID: 3451 RVA: 0x00013935 File Offset: 0x00011B35
		// (set) Token: 0x06000D7C RID: 3452 RVA: 0x0001393D File Offset: 0x00011B3D
		public object ClientData { get; set; }

		// Token: 0x1700034A RID: 842
		// (get) Token: 0x06000D7D RID: 3453 RVA: 0x00013946 File Offset: 0x00011B46
		// (set) Token: 0x06000D7E RID: 3454 RVA: 0x0001394E File Offset: 0x00011B4E
		public ProductUserId LocalUserId { get; set; }

		// Token: 0x1700034B RID: 843
		// (get) Token: 0x06000D7F RID: 3455 RVA: 0x00013957 File Offset: 0x00011B57
		// (set) Token: 0x06000D80 RID: 3456 RVA: 0x0001395F File Offset: 0x00011B5F
		public Utf8String RoomName { get; set; }

		// Token: 0x1700034C RID: 844
		// (get) Token: 0x06000D81 RID: 3457 RVA: 0x00013968 File Offset: 0x00011B68
		// (set) Token: 0x06000D82 RID: 3458 RVA: 0x00013970 File Offset: 0x00011B70
		public Utf8String Statistic { get; set; }

		// Token: 0x06000D83 RID: 3459 RVA: 0x0001397C File Offset: 0x00011B7C
		public Result? GetResultCode()
		{
			return null;
		}

		// Token: 0x06000D84 RID: 3460 RVA: 0x00013997 File Offset: 0x00011B97
		internal void Set(ref RoomStatisticsUpdatedInfoInternal other)
		{
			this.ClientData = other.ClientData;
			this.LocalUserId = other.LocalUserId;
			this.RoomName = other.RoomName;
			this.Statistic = other.Statistic;
		}
	}
}
