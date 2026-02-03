using System;

namespace Epic.OnlineServices.RTCAudio
{
	// Token: 0x02000202 RID: 514
	public struct AudioBeforeRenderCallbackInfo : ICallbackInfo
	{
		// Token: 0x170003BE RID: 958
		// (get) Token: 0x06000EC8 RID: 3784 RVA: 0x00015AC6 File Offset: 0x00013CC6
		// (set) Token: 0x06000EC9 RID: 3785 RVA: 0x00015ACE File Offset: 0x00013CCE
		public object ClientData { get; set; }

		// Token: 0x170003BF RID: 959
		// (get) Token: 0x06000ECA RID: 3786 RVA: 0x00015AD7 File Offset: 0x00013CD7
		// (set) Token: 0x06000ECB RID: 3787 RVA: 0x00015ADF File Offset: 0x00013CDF
		public ProductUserId LocalUserId { get; set; }

		// Token: 0x170003C0 RID: 960
		// (get) Token: 0x06000ECC RID: 3788 RVA: 0x00015AE8 File Offset: 0x00013CE8
		// (set) Token: 0x06000ECD RID: 3789 RVA: 0x00015AF0 File Offset: 0x00013CF0
		public Utf8String RoomName { get; set; }

		// Token: 0x170003C1 RID: 961
		// (get) Token: 0x06000ECE RID: 3790 RVA: 0x00015AF9 File Offset: 0x00013CF9
		// (set) Token: 0x06000ECF RID: 3791 RVA: 0x00015B01 File Offset: 0x00013D01
		public AudioBuffer? Buffer { get; set; }

		// Token: 0x170003C2 RID: 962
		// (get) Token: 0x06000ED0 RID: 3792 RVA: 0x00015B0A File Offset: 0x00013D0A
		// (set) Token: 0x06000ED1 RID: 3793 RVA: 0x00015B12 File Offset: 0x00013D12
		public ProductUserId ParticipantId { get; set; }

		// Token: 0x06000ED2 RID: 3794 RVA: 0x00015B1C File Offset: 0x00013D1C
		public Result? GetResultCode()
		{
			return null;
		}

		// Token: 0x06000ED3 RID: 3795 RVA: 0x00015B38 File Offset: 0x00013D38
		internal void Set(ref AudioBeforeRenderCallbackInfoInternal other)
		{
			this.ClientData = other.ClientData;
			this.LocalUserId = other.LocalUserId;
			this.RoomName = other.RoomName;
			this.Buffer = other.Buffer;
			this.ParticipantId = other.ParticipantId;
		}
	}
}
