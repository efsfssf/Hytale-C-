using System;

namespace Epic.OnlineServices.CustomInvites
{
	// Token: 0x020005B8 RID: 1464
	public struct SendCustomInviteOptions
	{
		// Token: 0x17000B0A RID: 2826
		// (get) Token: 0x0600260A RID: 9738 RVA: 0x00037E57 File Offset: 0x00036057
		// (set) Token: 0x0600260B RID: 9739 RVA: 0x00037E5F File Offset: 0x0003605F
		public ProductUserId LocalUserId { get; set; }

		// Token: 0x17000B0B RID: 2827
		// (get) Token: 0x0600260C RID: 9740 RVA: 0x00037E68 File Offset: 0x00036068
		// (set) Token: 0x0600260D RID: 9741 RVA: 0x00037E70 File Offset: 0x00036070
		public ProductUserId[] TargetUserIds { get; set; }
	}
}
