using System;

namespace Epic.OnlineServices.AntiCheatCommon
{
	// Token: 0x020006C1 RID: 1729
	public struct OnMessageToClientCallbackInfo : ICallbackInfo
	{
		// Token: 0x17000D60 RID: 3424
		// (get) Token: 0x06002CE1 RID: 11489 RVA: 0x00042532 File Offset: 0x00040732
		// (set) Token: 0x06002CE2 RID: 11490 RVA: 0x0004253A File Offset: 0x0004073A
		public object ClientData { get; set; }

		// Token: 0x17000D61 RID: 3425
		// (get) Token: 0x06002CE3 RID: 11491 RVA: 0x00042543 File Offset: 0x00040743
		// (set) Token: 0x06002CE4 RID: 11492 RVA: 0x0004254B File Offset: 0x0004074B
		public IntPtr ClientHandle { get; set; }

		// Token: 0x17000D62 RID: 3426
		// (get) Token: 0x06002CE5 RID: 11493 RVA: 0x00042554 File Offset: 0x00040754
		// (set) Token: 0x06002CE6 RID: 11494 RVA: 0x0004255C File Offset: 0x0004075C
		public ArraySegment<byte> MessageData { get; set; }

		// Token: 0x06002CE7 RID: 11495 RVA: 0x00042568 File Offset: 0x00040768
		public Result? GetResultCode()
		{
			return null;
		}

		// Token: 0x06002CE8 RID: 11496 RVA: 0x00042583 File Offset: 0x00040783
		internal void Set(ref OnMessageToClientCallbackInfoInternal other)
		{
			this.ClientData = other.ClientData;
			this.ClientHandle = other.ClientHandle;
			this.MessageData = other.MessageData;
		}
	}
}
