using System;

namespace Epic.OnlineServices.RTCAudio
{
	// Token: 0x02000204 RID: 516
	public struct AudioBeforeSendCallbackInfo : ICallbackInfo
	{
		// Token: 0x170003C9 RID: 969
		// (get) Token: 0x06000EE3 RID: 3811 RVA: 0x00015DC8 File Offset: 0x00013FC8
		// (set) Token: 0x06000EE4 RID: 3812 RVA: 0x00015DD0 File Offset: 0x00013FD0
		public object ClientData { get; set; }

		// Token: 0x170003CA RID: 970
		// (get) Token: 0x06000EE5 RID: 3813 RVA: 0x00015DD9 File Offset: 0x00013FD9
		// (set) Token: 0x06000EE6 RID: 3814 RVA: 0x00015DE1 File Offset: 0x00013FE1
		public ProductUserId LocalUserId { get; set; }

		// Token: 0x170003CB RID: 971
		// (get) Token: 0x06000EE7 RID: 3815 RVA: 0x00015DEA File Offset: 0x00013FEA
		// (set) Token: 0x06000EE8 RID: 3816 RVA: 0x00015DF2 File Offset: 0x00013FF2
		public Utf8String RoomName { get; set; }

		// Token: 0x170003CC RID: 972
		// (get) Token: 0x06000EE9 RID: 3817 RVA: 0x00015DFB File Offset: 0x00013FFB
		// (set) Token: 0x06000EEA RID: 3818 RVA: 0x00015E03 File Offset: 0x00014003
		public AudioBuffer? Buffer { get; set; }

		// Token: 0x06000EEB RID: 3819 RVA: 0x00015E0C File Offset: 0x0001400C
		public Result? GetResultCode()
		{
			return null;
		}

		// Token: 0x06000EEC RID: 3820 RVA: 0x00015E27 File Offset: 0x00014027
		internal void Set(ref AudioBeforeSendCallbackInfoInternal other)
		{
			this.ClientData = other.ClientData;
			this.LocalUserId = other.LocalUserId;
			this.RoomName = other.RoomName;
			this.Buffer = other.Buffer;
		}
	}
}
