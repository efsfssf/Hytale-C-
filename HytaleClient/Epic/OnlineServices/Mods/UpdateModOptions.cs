using System;

namespace Epic.OnlineServices.Mods
{
	// Token: 0x0200034B RID: 843
	public struct UpdateModOptions
	{
		// Token: 0x17000653 RID: 1619
		// (get) Token: 0x06001719 RID: 5913 RVA: 0x00021B1B File Offset: 0x0001FD1B
		// (set) Token: 0x0600171A RID: 5914 RVA: 0x00021B23 File Offset: 0x0001FD23
		public EpicAccountId LocalUserId { get; set; }

		// Token: 0x17000654 RID: 1620
		// (get) Token: 0x0600171B RID: 5915 RVA: 0x00021B2C File Offset: 0x0001FD2C
		// (set) Token: 0x0600171C RID: 5916 RVA: 0x00021B34 File Offset: 0x0001FD34
		public ModIdentifier? Mod { get; set; }
	}
}
