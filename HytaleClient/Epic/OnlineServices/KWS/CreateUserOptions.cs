using System;

namespace Epic.OnlineServices.KWS
{
	// Token: 0x02000489 RID: 1161
	public struct CreateUserOptions
	{
		// Token: 0x17000899 RID: 2201
		// (get) Token: 0x06001E5F RID: 7775 RVA: 0x0002C7E8 File Offset: 0x0002A9E8
		// (set) Token: 0x06001E60 RID: 7776 RVA: 0x0002C7F0 File Offset: 0x0002A9F0
		public ProductUserId LocalUserId { get; set; }

		// Token: 0x1700089A RID: 2202
		// (get) Token: 0x06001E61 RID: 7777 RVA: 0x0002C7F9 File Offset: 0x0002A9F9
		// (set) Token: 0x06001E62 RID: 7778 RVA: 0x0002C801 File Offset: 0x0002AA01
		public Utf8String DateOfBirth { get; set; }

		// Token: 0x1700089B RID: 2203
		// (get) Token: 0x06001E63 RID: 7779 RVA: 0x0002C80A File Offset: 0x0002AA0A
		// (set) Token: 0x06001E64 RID: 7780 RVA: 0x0002C812 File Offset: 0x0002AA12
		public Utf8String ParentEmail { get; set; }
	}
}
