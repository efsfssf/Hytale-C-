using System;

namespace Epic.OnlineServices.Ecom
{
	// Token: 0x020004FF RID: 1279
	public struct CatalogItem
	{
		// Token: 0x17000972 RID: 2418
		// (get) Token: 0x06002131 RID: 8497 RVA: 0x00030946 File Offset: 0x0002EB46
		// (set) Token: 0x06002132 RID: 8498 RVA: 0x0003094E File Offset: 0x0002EB4E
		public Utf8String CatalogNamespace { get; set; }

		// Token: 0x17000973 RID: 2419
		// (get) Token: 0x06002133 RID: 8499 RVA: 0x00030957 File Offset: 0x0002EB57
		// (set) Token: 0x06002134 RID: 8500 RVA: 0x0003095F File Offset: 0x0002EB5F
		public Utf8String Id { get; set; }

		// Token: 0x17000974 RID: 2420
		// (get) Token: 0x06002135 RID: 8501 RVA: 0x00030968 File Offset: 0x0002EB68
		// (set) Token: 0x06002136 RID: 8502 RVA: 0x00030970 File Offset: 0x0002EB70
		public Utf8String EntitlementName { get; set; }

		// Token: 0x17000975 RID: 2421
		// (get) Token: 0x06002137 RID: 8503 RVA: 0x00030979 File Offset: 0x0002EB79
		// (set) Token: 0x06002138 RID: 8504 RVA: 0x00030981 File Offset: 0x0002EB81
		public Utf8String TitleText { get; set; }

		// Token: 0x17000976 RID: 2422
		// (get) Token: 0x06002139 RID: 8505 RVA: 0x0003098A File Offset: 0x0002EB8A
		// (set) Token: 0x0600213A RID: 8506 RVA: 0x00030992 File Offset: 0x0002EB92
		public Utf8String DescriptionText { get; set; }

		// Token: 0x17000977 RID: 2423
		// (get) Token: 0x0600213B RID: 8507 RVA: 0x0003099B File Offset: 0x0002EB9B
		// (set) Token: 0x0600213C RID: 8508 RVA: 0x000309A3 File Offset: 0x0002EBA3
		public Utf8String LongDescriptionText { get; set; }

		// Token: 0x17000978 RID: 2424
		// (get) Token: 0x0600213D RID: 8509 RVA: 0x000309AC File Offset: 0x0002EBAC
		// (set) Token: 0x0600213E RID: 8510 RVA: 0x000309B4 File Offset: 0x0002EBB4
		public Utf8String TechnicalDetailsText { get; set; }

		// Token: 0x17000979 RID: 2425
		// (get) Token: 0x0600213F RID: 8511 RVA: 0x000309BD File Offset: 0x0002EBBD
		// (set) Token: 0x06002140 RID: 8512 RVA: 0x000309C5 File Offset: 0x0002EBC5
		public Utf8String DeveloperText { get; set; }

		// Token: 0x1700097A RID: 2426
		// (get) Token: 0x06002141 RID: 8513 RVA: 0x000309CE File Offset: 0x0002EBCE
		// (set) Token: 0x06002142 RID: 8514 RVA: 0x000309D6 File Offset: 0x0002EBD6
		public EcomItemType ItemType { get; set; }

		// Token: 0x1700097B RID: 2427
		// (get) Token: 0x06002143 RID: 8515 RVA: 0x000309DF File Offset: 0x0002EBDF
		// (set) Token: 0x06002144 RID: 8516 RVA: 0x000309E7 File Offset: 0x0002EBE7
		public long EntitlementEndTimestamp { get; set; }

		// Token: 0x06002145 RID: 8517 RVA: 0x000309F0 File Offset: 0x0002EBF0
		internal void Set(ref CatalogItemInternal other)
		{
			this.CatalogNamespace = other.CatalogNamespace;
			this.Id = other.Id;
			this.EntitlementName = other.EntitlementName;
			this.TitleText = other.TitleText;
			this.DescriptionText = other.DescriptionText;
			this.LongDescriptionText = other.LongDescriptionText;
			this.TechnicalDetailsText = other.TechnicalDetailsText;
			this.DeveloperText = other.DeveloperText;
			this.ItemType = other.ItemType;
			this.EntitlementEndTimestamp = other.EntitlementEndTimestamp;
		}
	}
}
