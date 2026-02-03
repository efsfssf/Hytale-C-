using System;

namespace Epic.OnlineServices.AntiCheatCommon
{
	// Token: 0x020006BD RID: 1725
	public struct OnClientActionRequiredCallbackInfo : ICallbackInfo
	{
		// Token: 0x17000D4E RID: 3406
		// (get) Token: 0x06002CB3 RID: 11443 RVA: 0x000420B9 File Offset: 0x000402B9
		// (set) Token: 0x06002CB4 RID: 11444 RVA: 0x000420C1 File Offset: 0x000402C1
		public object ClientData { get; set; }

		// Token: 0x17000D4F RID: 3407
		// (get) Token: 0x06002CB5 RID: 11445 RVA: 0x000420CA File Offset: 0x000402CA
		// (set) Token: 0x06002CB6 RID: 11446 RVA: 0x000420D2 File Offset: 0x000402D2
		public IntPtr ClientHandle { get; set; }

		// Token: 0x17000D50 RID: 3408
		// (get) Token: 0x06002CB7 RID: 11447 RVA: 0x000420DB File Offset: 0x000402DB
		// (set) Token: 0x06002CB8 RID: 11448 RVA: 0x000420E3 File Offset: 0x000402E3
		public AntiCheatCommonClientAction ClientAction { get; set; }

		// Token: 0x17000D51 RID: 3409
		// (get) Token: 0x06002CB9 RID: 11449 RVA: 0x000420EC File Offset: 0x000402EC
		// (set) Token: 0x06002CBA RID: 11450 RVA: 0x000420F4 File Offset: 0x000402F4
		public AntiCheatCommonClientActionReason ActionReasonCode { get; set; }

		// Token: 0x17000D52 RID: 3410
		// (get) Token: 0x06002CBB RID: 11451 RVA: 0x000420FD File Offset: 0x000402FD
		// (set) Token: 0x06002CBC RID: 11452 RVA: 0x00042105 File Offset: 0x00040305
		public Utf8String ActionReasonDetailsString { get; set; }

		// Token: 0x06002CBD RID: 11453 RVA: 0x00042110 File Offset: 0x00040310
		public Result? GetResultCode()
		{
			return null;
		}

		// Token: 0x06002CBE RID: 11454 RVA: 0x0004212C File Offset: 0x0004032C
		internal void Set(ref OnClientActionRequiredCallbackInfoInternal other)
		{
			this.ClientData = other.ClientData;
			this.ClientHandle = other.ClientHandle;
			this.ClientAction = other.ClientAction;
			this.ActionReasonCode = other.ActionReasonCode;
			this.ActionReasonDetailsString = other.ActionReasonDetailsString;
		}
	}
}
