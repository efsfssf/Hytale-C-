using System;

namespace Epic.OnlineServices.Mods
{
	// Token: 0x02000347 RID: 839
	public struct UninstallModOptions
	{
		// Token: 0x17000646 RID: 1606
		// (get) Token: 0x060016F9 RID: 5881 RVA: 0x000217FB File Offset: 0x0001F9FB
		// (set) Token: 0x060016FA RID: 5882 RVA: 0x00021803 File Offset: 0x0001FA03
		public EpicAccountId LocalUserId { get; set; }

		// Token: 0x17000647 RID: 1607
		// (get) Token: 0x060016FB RID: 5883 RVA: 0x0002180C File Offset: 0x0001FA0C
		// (set) Token: 0x060016FC RID: 5884 RVA: 0x00021814 File Offset: 0x0001FA14
		public ModIdentifier? Mod { get; set; }
	}
}
