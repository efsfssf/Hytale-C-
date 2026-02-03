using System;

namespace Epic.OnlineServices.KWS
{
	// Token: 0x02000485 RID: 1157
	public struct CopyPermissionByIndexOptions
	{
		// Token: 0x1700088A RID: 2186
		// (get) Token: 0x06001E3B RID: 7739 RVA: 0x0002C450 File Offset: 0x0002A650
		// (set) Token: 0x06001E3C RID: 7740 RVA: 0x0002C458 File Offset: 0x0002A658
		public ProductUserId LocalUserId { get; set; }

		// Token: 0x1700088B RID: 2187
		// (get) Token: 0x06001E3D RID: 7741 RVA: 0x0002C461 File Offset: 0x0002A661
		// (set) Token: 0x06001E3E RID: 7742 RVA: 0x0002C469 File Offset: 0x0002A669
		public uint Index { get; set; }
	}
}
