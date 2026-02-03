using System;

namespace Epic.OnlineServices.CustomInvites
{
	// Token: 0x020005BE RID: 1470
	public struct SendRequestToJoinOptions
	{
		// Token: 0x17000B24 RID: 2852
		// (get) Token: 0x06002649 RID: 9801 RVA: 0x000384DB File Offset: 0x000366DB
		// (set) Token: 0x0600264A RID: 9802 RVA: 0x000384E3 File Offset: 0x000366E3
		public ProductUserId LocalUserId { get; set; }

		// Token: 0x17000B25 RID: 2853
		// (get) Token: 0x0600264B RID: 9803 RVA: 0x000384EC File Offset: 0x000366EC
		// (set) Token: 0x0600264C RID: 9804 RVA: 0x000384F4 File Offset: 0x000366F4
		public ProductUserId TargetUserId { get; set; }
	}
}
