using System;

namespace Epic.OnlineServices.Ecom
{
	// Token: 0x0200056D RID: 1389
	public struct SandboxIdItemOwnership
	{
		// Token: 0x17000A9A RID: 2714
		// (get) Token: 0x0600244B RID: 9291 RVA: 0x000356FE File Offset: 0x000338FE
		// (set) Token: 0x0600244C RID: 9292 RVA: 0x00035706 File Offset: 0x00033906
		public Utf8String SandboxId { get; set; }

		// Token: 0x17000A9B RID: 2715
		// (get) Token: 0x0600244D RID: 9293 RVA: 0x0003570F File Offset: 0x0003390F
		// (set) Token: 0x0600244E RID: 9294 RVA: 0x00035717 File Offset: 0x00033917
		public Utf8String[] OwnedCatalogItemIds { get; set; }

		// Token: 0x0600244F RID: 9295 RVA: 0x00035720 File Offset: 0x00033920
		internal void Set(ref SandboxIdItemOwnershipInternal other)
		{
			this.SandboxId = other.SandboxId;
			this.OwnedCatalogItemIds = other.OwnedCatalogItemIds;
		}
	}
}
