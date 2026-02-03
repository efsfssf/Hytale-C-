using System;

namespace Epic.OnlineServices.UI
{
	// Token: 0x02000090 RID: 144
	public struct ShowNativeProfileOptions
	{
		// Token: 0x170000F2 RID: 242
		// (get) Token: 0x060005D8 RID: 1496 RVA: 0x00008303 File Offset: 0x00006503
		// (set) Token: 0x060005D9 RID: 1497 RVA: 0x0000830B File Offset: 0x0000650B
		public EpicAccountId LocalUserId { get; set; }

		// Token: 0x170000F3 RID: 243
		// (get) Token: 0x060005DA RID: 1498 RVA: 0x00008314 File Offset: 0x00006514
		// (set) Token: 0x060005DB RID: 1499 RVA: 0x0000831C File Offset: 0x0000651C
		public EpicAccountId TargetUserId { get; set; }
	}
}
