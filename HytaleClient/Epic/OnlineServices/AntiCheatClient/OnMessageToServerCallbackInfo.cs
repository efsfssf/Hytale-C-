using System;

namespace Epic.OnlineServices.AntiCheatClient
{
	// Token: 0x020006EC RID: 1772
	public struct OnMessageToServerCallbackInfo : ICallbackInfo
	{
		// Token: 0x17000D98 RID: 3480
		// (get) Token: 0x06002DB7 RID: 11703 RVA: 0x00043962 File Offset: 0x00041B62
		// (set) Token: 0x06002DB8 RID: 11704 RVA: 0x0004396A File Offset: 0x00041B6A
		public object ClientData { get; set; }

		// Token: 0x17000D99 RID: 3481
		// (get) Token: 0x06002DB9 RID: 11705 RVA: 0x00043973 File Offset: 0x00041B73
		// (set) Token: 0x06002DBA RID: 11706 RVA: 0x0004397B File Offset: 0x00041B7B
		public ArraySegment<byte> MessageData { get; set; }

		// Token: 0x06002DBB RID: 11707 RVA: 0x00043984 File Offset: 0x00041B84
		public Result? GetResultCode()
		{
			return null;
		}

		// Token: 0x06002DBC RID: 11708 RVA: 0x0004399F File Offset: 0x00041B9F
		internal void Set(ref OnMessageToServerCallbackInfoInternal other)
		{
			this.ClientData = other.ClientData;
			this.MessageData = other.MessageData;
		}
	}
}
