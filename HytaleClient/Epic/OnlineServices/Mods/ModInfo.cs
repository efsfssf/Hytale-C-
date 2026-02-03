using System;

namespace Epic.OnlineServices.Mods
{
	// Token: 0x0200033A RID: 826
	public struct ModInfo
	{
		// Token: 0x17000639 RID: 1593
		// (get) Token: 0x060016AA RID: 5802 RVA: 0x000211EF File Offset: 0x0001F3EF
		// (set) Token: 0x060016AB RID: 5803 RVA: 0x000211F7 File Offset: 0x0001F3F7
		public ModIdentifier[] Mods { get; set; }

		// Token: 0x1700063A RID: 1594
		// (get) Token: 0x060016AC RID: 5804 RVA: 0x00021200 File Offset: 0x0001F400
		// (set) Token: 0x060016AD RID: 5805 RVA: 0x00021208 File Offset: 0x0001F408
		public ModEnumerationType Type { get; set; }

		// Token: 0x060016AE RID: 5806 RVA: 0x00021211 File Offset: 0x0001F411
		internal void Set(ref ModInfoInternal other)
		{
			this.Mods = other.Mods;
			this.Type = other.Type;
		}
	}
}
