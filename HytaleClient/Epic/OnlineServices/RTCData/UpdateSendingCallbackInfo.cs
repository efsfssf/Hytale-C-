using System;

namespace Epic.OnlineServices.RTCData
{
	// Token: 0x020001F2 RID: 498
	public struct UpdateSendingCallbackInfo : ICallbackInfo
	{
		// Token: 0x17000397 RID: 919
		// (get) Token: 0x06000E6E RID: 3694 RVA: 0x00015264 File Offset: 0x00013464
		// (set) Token: 0x06000E6F RID: 3695 RVA: 0x0001526C File Offset: 0x0001346C
		public Result ResultCode { get; set; }

		// Token: 0x17000398 RID: 920
		// (get) Token: 0x06000E70 RID: 3696 RVA: 0x00015275 File Offset: 0x00013475
		// (set) Token: 0x06000E71 RID: 3697 RVA: 0x0001527D File Offset: 0x0001347D
		public object ClientData { get; set; }

		// Token: 0x17000399 RID: 921
		// (get) Token: 0x06000E72 RID: 3698 RVA: 0x00015286 File Offset: 0x00013486
		// (set) Token: 0x06000E73 RID: 3699 RVA: 0x0001528E File Offset: 0x0001348E
		public ProductUserId LocalUserId { get; set; }

		// Token: 0x1700039A RID: 922
		// (get) Token: 0x06000E74 RID: 3700 RVA: 0x00015297 File Offset: 0x00013497
		// (set) Token: 0x06000E75 RID: 3701 RVA: 0x0001529F File Offset: 0x0001349F
		public Utf8String RoomName { get; set; }

		// Token: 0x1700039B RID: 923
		// (get) Token: 0x06000E76 RID: 3702 RVA: 0x000152A8 File Offset: 0x000134A8
		// (set) Token: 0x06000E77 RID: 3703 RVA: 0x000152B0 File Offset: 0x000134B0
		public bool DataEnabled { get; set; }

		// Token: 0x06000E78 RID: 3704 RVA: 0x000152BC File Offset: 0x000134BC
		public Result? GetResultCode()
		{
			return new Result?(this.ResultCode);
		}

		// Token: 0x06000E79 RID: 3705 RVA: 0x000152DC File Offset: 0x000134DC
		internal void Set(ref UpdateSendingCallbackInfoInternal other)
		{
			this.ResultCode = other.ResultCode;
			this.ClientData = other.ClientData;
			this.LocalUserId = other.LocalUserId;
			this.RoomName = other.RoomName;
			this.DataEnabled = other.DataEnabled;
		}
	}
}
