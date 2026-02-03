using System;

namespace Epic.OnlineServices.RTCAudio
{
	// Token: 0x0200020C RID: 524
	public struct AudioInputStateCallbackInfo : ICallbackInfo
	{
		// Token: 0x170003E1 RID: 993
		// (get) Token: 0x06000F27 RID: 3879 RVA: 0x0001646D File Offset: 0x0001466D
		// (set) Token: 0x06000F28 RID: 3880 RVA: 0x00016475 File Offset: 0x00014675
		public object ClientData { get; set; }

		// Token: 0x170003E2 RID: 994
		// (get) Token: 0x06000F29 RID: 3881 RVA: 0x0001647E File Offset: 0x0001467E
		// (set) Token: 0x06000F2A RID: 3882 RVA: 0x00016486 File Offset: 0x00014686
		public ProductUserId LocalUserId { get; set; }

		// Token: 0x170003E3 RID: 995
		// (get) Token: 0x06000F2B RID: 3883 RVA: 0x0001648F File Offset: 0x0001468F
		// (set) Token: 0x06000F2C RID: 3884 RVA: 0x00016497 File Offset: 0x00014697
		public Utf8String RoomName { get; set; }

		// Token: 0x170003E4 RID: 996
		// (get) Token: 0x06000F2D RID: 3885 RVA: 0x000164A0 File Offset: 0x000146A0
		// (set) Token: 0x06000F2E RID: 3886 RVA: 0x000164A8 File Offset: 0x000146A8
		public RTCAudioInputStatus Status { get; set; }

		// Token: 0x06000F2F RID: 3887 RVA: 0x000164B4 File Offset: 0x000146B4
		public Result? GetResultCode()
		{
			return null;
		}

		// Token: 0x06000F30 RID: 3888 RVA: 0x000164CF File Offset: 0x000146CF
		internal void Set(ref AudioInputStateCallbackInfoInternal other)
		{
			this.ClientData = other.ClientData;
			this.LocalUserId = other.LocalUserId;
			this.RoomName = other.RoomName;
			this.Status = other.Status;
		}
	}
}
