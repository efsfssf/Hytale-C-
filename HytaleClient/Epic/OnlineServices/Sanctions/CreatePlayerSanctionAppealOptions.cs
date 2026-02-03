using System;

namespace Epic.OnlineServices.Sanctions
{
	// Token: 0x0200019D RID: 413
	public struct CreatePlayerSanctionAppealOptions
	{
		// Token: 0x170002C3 RID: 707
		// (get) Token: 0x06000C01 RID: 3073 RVA: 0x0001184A File Offset: 0x0000FA4A
		// (set) Token: 0x06000C02 RID: 3074 RVA: 0x00011852 File Offset: 0x0000FA52
		public ProductUserId LocalUserId { get; set; }

		// Token: 0x170002C4 RID: 708
		// (get) Token: 0x06000C03 RID: 3075 RVA: 0x0001185B File Offset: 0x0000FA5B
		// (set) Token: 0x06000C04 RID: 3076 RVA: 0x00011863 File Offset: 0x0000FA63
		public SanctionAppealReason Reason { get; set; }

		// Token: 0x170002C5 RID: 709
		// (get) Token: 0x06000C05 RID: 3077 RVA: 0x0001186C File Offset: 0x0000FA6C
		// (set) Token: 0x06000C06 RID: 3078 RVA: 0x00011874 File Offset: 0x0000FA74
		public Utf8String ReferenceId { get; set; }
	}
}
