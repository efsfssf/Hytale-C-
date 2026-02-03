using System;

namespace Epic.OnlineServices.AntiCheatServer
{
	// Token: 0x0200068A RID: 1674
	public struct ReceiveMessageFromClientOptions
	{
		// Token: 0x17000CB3 RID: 3251
		// (get) Token: 0x06002B73 RID: 11123 RVA: 0x0003FFFB File Offset: 0x0003E1FB
		// (set) Token: 0x06002B74 RID: 11124 RVA: 0x00040003 File Offset: 0x0003E203
		public IntPtr ClientHandle { get; set; }

		// Token: 0x17000CB4 RID: 3252
		// (get) Token: 0x06002B75 RID: 11125 RVA: 0x0004000C File Offset: 0x0003E20C
		// (set) Token: 0x06002B76 RID: 11126 RVA: 0x00040014 File Offset: 0x0003E214
		public ArraySegment<byte> Data { get; set; }
	}
}
