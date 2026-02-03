using System;

namespace Epic.OnlineServices.Mods
{
	// Token: 0x02000335 RID: 821
	public struct InstallModOptions
	{
		// Token: 0x17000629 RID: 1577
		// (get) Token: 0x06001685 RID: 5765 RVA: 0x00020E03 File Offset: 0x0001F003
		// (set) Token: 0x06001686 RID: 5766 RVA: 0x00020E0B File Offset: 0x0001F00B
		public EpicAccountId LocalUserId { get; set; }

		// Token: 0x1700062A RID: 1578
		// (get) Token: 0x06001687 RID: 5767 RVA: 0x00020E14 File Offset: 0x0001F014
		// (set) Token: 0x06001688 RID: 5768 RVA: 0x00020E1C File Offset: 0x0001F01C
		public ModIdentifier? Mod { get; set; }

		// Token: 0x1700062B RID: 1579
		// (get) Token: 0x06001689 RID: 5769 RVA: 0x00020E25 File Offset: 0x0001F025
		// (set) Token: 0x0600168A RID: 5770 RVA: 0x00020E2D File Offset: 0x0001F02D
		public bool RemoveAfterExit { get; set; }
	}
}
