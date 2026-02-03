using System;

namespace Epic.OnlineServices.CustomInvites
{
	// Token: 0x0200058B RID: 1419
	public struct FinalizeInviteOptions
	{
		// Token: 0x17000AB8 RID: 2744
		// (get) Token: 0x060024DE RID: 9438 RVA: 0x0003696F File Offset: 0x00034B6F
		// (set) Token: 0x060024DF RID: 9439 RVA: 0x00036977 File Offset: 0x00034B77
		public ProductUserId TargetUserId { get; set; }

		// Token: 0x17000AB9 RID: 2745
		// (get) Token: 0x060024E0 RID: 9440 RVA: 0x00036980 File Offset: 0x00034B80
		// (set) Token: 0x060024E1 RID: 9441 RVA: 0x00036988 File Offset: 0x00034B88
		public ProductUserId LocalUserId { get; set; }

		// Token: 0x17000ABA RID: 2746
		// (get) Token: 0x060024E2 RID: 9442 RVA: 0x00036991 File Offset: 0x00034B91
		// (set) Token: 0x060024E3 RID: 9443 RVA: 0x00036999 File Offset: 0x00034B99
		public Utf8String CustomInviteId { get; set; }

		// Token: 0x17000ABB RID: 2747
		// (get) Token: 0x060024E4 RID: 9444 RVA: 0x000369A2 File Offset: 0x00034BA2
		// (set) Token: 0x060024E5 RID: 9445 RVA: 0x000369AA File Offset: 0x00034BAA
		public Result ProcessingResult { get; set; }
	}
}
