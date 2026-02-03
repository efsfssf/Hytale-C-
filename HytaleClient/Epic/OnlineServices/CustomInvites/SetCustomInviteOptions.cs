using System;

namespace Epic.OnlineServices.CustomInvites
{
	// Token: 0x020005C0 RID: 1472
	public struct SetCustomInviteOptions
	{
		// Token: 0x17000B28 RID: 2856
		// (get) Token: 0x06002652 RID: 9810 RVA: 0x000385AA File Offset: 0x000367AA
		// (set) Token: 0x06002653 RID: 9811 RVA: 0x000385B2 File Offset: 0x000367B2
		public ProductUserId LocalUserId { get; set; }

		// Token: 0x17000B29 RID: 2857
		// (get) Token: 0x06002654 RID: 9812 RVA: 0x000385BB File Offset: 0x000367BB
		// (set) Token: 0x06002655 RID: 9813 RVA: 0x000385C3 File Offset: 0x000367C3
		public Utf8String Payload { get; set; }
	}
}
