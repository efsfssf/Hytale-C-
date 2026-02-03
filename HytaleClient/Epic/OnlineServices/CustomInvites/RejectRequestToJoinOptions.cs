using System;

namespace Epic.OnlineServices.CustomInvites
{
	// Token: 0x020005AF RID: 1455
	public struct RejectRequestToJoinOptions
	{
		// Token: 0x17000AED RID: 2797
		// (get) Token: 0x060025C0 RID: 9664 RVA: 0x000376F3 File Offset: 0x000358F3
		// (set) Token: 0x060025C1 RID: 9665 RVA: 0x000376FB File Offset: 0x000358FB
		public ProductUserId LocalUserId { get; set; }

		// Token: 0x17000AEE RID: 2798
		// (get) Token: 0x060025C2 RID: 9666 RVA: 0x00037704 File Offset: 0x00035904
		// (set) Token: 0x060025C3 RID: 9667 RVA: 0x0003770C File Offset: 0x0003590C
		public ProductUserId TargetUserId { get; set; }
	}
}
