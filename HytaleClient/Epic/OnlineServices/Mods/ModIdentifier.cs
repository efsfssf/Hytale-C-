using System;

namespace Epic.OnlineServices.Mods
{
	// Token: 0x02000338 RID: 824
	public struct ModIdentifier
	{
		// Token: 0x1700062F RID: 1583
		// (get) Token: 0x06001691 RID: 5777 RVA: 0x00020F13 File Offset: 0x0001F113
		// (set) Token: 0x06001692 RID: 5778 RVA: 0x00020F1B File Offset: 0x0001F11B
		public Utf8String NamespaceId { get; set; }

		// Token: 0x17000630 RID: 1584
		// (get) Token: 0x06001693 RID: 5779 RVA: 0x00020F24 File Offset: 0x0001F124
		// (set) Token: 0x06001694 RID: 5780 RVA: 0x00020F2C File Offset: 0x0001F12C
		public Utf8String ItemId { get; set; }

		// Token: 0x17000631 RID: 1585
		// (get) Token: 0x06001695 RID: 5781 RVA: 0x00020F35 File Offset: 0x0001F135
		// (set) Token: 0x06001696 RID: 5782 RVA: 0x00020F3D File Offset: 0x0001F13D
		public Utf8String ArtifactId { get; set; }

		// Token: 0x17000632 RID: 1586
		// (get) Token: 0x06001697 RID: 5783 RVA: 0x00020F46 File Offset: 0x0001F146
		// (set) Token: 0x06001698 RID: 5784 RVA: 0x00020F4E File Offset: 0x0001F14E
		public Utf8String Title { get; set; }

		// Token: 0x17000633 RID: 1587
		// (get) Token: 0x06001699 RID: 5785 RVA: 0x00020F57 File Offset: 0x0001F157
		// (set) Token: 0x0600169A RID: 5786 RVA: 0x00020F5F File Offset: 0x0001F15F
		public Utf8String Version { get; set; }

		// Token: 0x0600169B RID: 5787 RVA: 0x00020F68 File Offset: 0x0001F168
		internal void Set(ref ModIdentifierInternal other)
		{
			this.NamespaceId = other.NamespaceId;
			this.ItemId = other.ItemId;
			this.ArtifactId = other.ArtifactId;
			this.Title = other.Title;
			this.Version = other.Version;
		}
	}
}
