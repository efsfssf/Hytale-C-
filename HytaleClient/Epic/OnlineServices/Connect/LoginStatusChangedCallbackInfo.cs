using System;

namespace Epic.OnlineServices.Connect
{
	// Token: 0x020005F3 RID: 1523
	public struct LoginStatusChangedCallbackInfo : ICallbackInfo
	{
		// Token: 0x17000B8E RID: 2958
		// (get) Token: 0x0600278D RID: 10125 RVA: 0x0003A8DE File Offset: 0x00038ADE
		// (set) Token: 0x0600278E RID: 10126 RVA: 0x0003A8E6 File Offset: 0x00038AE6
		public object ClientData { get; set; }

		// Token: 0x17000B8F RID: 2959
		// (get) Token: 0x0600278F RID: 10127 RVA: 0x0003A8EF File Offset: 0x00038AEF
		// (set) Token: 0x06002790 RID: 10128 RVA: 0x0003A8F7 File Offset: 0x00038AF7
		public ProductUserId LocalUserId { get; set; }

		// Token: 0x17000B90 RID: 2960
		// (get) Token: 0x06002791 RID: 10129 RVA: 0x0003A900 File Offset: 0x00038B00
		// (set) Token: 0x06002792 RID: 10130 RVA: 0x0003A908 File Offset: 0x00038B08
		public LoginStatus PreviousStatus { get; set; }

		// Token: 0x17000B91 RID: 2961
		// (get) Token: 0x06002793 RID: 10131 RVA: 0x0003A911 File Offset: 0x00038B11
		// (set) Token: 0x06002794 RID: 10132 RVA: 0x0003A919 File Offset: 0x00038B19
		public LoginStatus CurrentStatus { get; set; }

		// Token: 0x06002795 RID: 10133 RVA: 0x0003A924 File Offset: 0x00038B24
		public Result? GetResultCode()
		{
			return null;
		}

		// Token: 0x06002796 RID: 10134 RVA: 0x0003A93F File Offset: 0x00038B3F
		internal void Set(ref LoginStatusChangedCallbackInfoInternal other)
		{
			this.ClientData = other.ClientData;
			this.LocalUserId = other.LocalUserId;
			this.PreviousStatus = other.PreviousStatus;
			this.CurrentStatus = other.CurrentStatus;
		}
	}
}
