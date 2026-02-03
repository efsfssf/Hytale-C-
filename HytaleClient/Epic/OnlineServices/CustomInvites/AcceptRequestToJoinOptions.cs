using System;

namespace Epic.OnlineServices.CustomInvites
{
	// Token: 0x02000576 RID: 1398
	public struct AcceptRequestToJoinOptions
	{
		// Token: 0x17000AA9 RID: 2729
		// (get) Token: 0x0600247E RID: 9342 RVA: 0x00035C27 File Offset: 0x00033E27
		// (set) Token: 0x0600247F RID: 9343 RVA: 0x00035C2F File Offset: 0x00033E2F
		public ProductUserId LocalUserId { get; set; }

		// Token: 0x17000AAA RID: 2730
		// (get) Token: 0x06002480 RID: 9344 RVA: 0x00035C38 File Offset: 0x00033E38
		// (set) Token: 0x06002481 RID: 9345 RVA: 0x00035C40 File Offset: 0x00033E40
		public ProductUserId TargetUserId { get; set; }
	}
}
