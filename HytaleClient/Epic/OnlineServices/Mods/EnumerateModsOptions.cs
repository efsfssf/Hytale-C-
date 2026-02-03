using System;

namespace Epic.OnlineServices.Mods
{
	// Token: 0x02000331 RID: 817
	public struct EnumerateModsOptions
	{
		// Token: 0x1700061C RID: 1564
		// (get) Token: 0x06001665 RID: 5733 RVA: 0x00020AF7 File Offset: 0x0001ECF7
		// (set) Token: 0x06001666 RID: 5734 RVA: 0x00020AFF File Offset: 0x0001ECFF
		public EpicAccountId LocalUserId { get; set; }

		// Token: 0x1700061D RID: 1565
		// (get) Token: 0x06001667 RID: 5735 RVA: 0x00020B08 File Offset: 0x0001ED08
		// (set) Token: 0x06001668 RID: 5736 RVA: 0x00020B10 File Offset: 0x0001ED10
		public ModEnumerationType Type { get; set; }
	}
}
