using System;

namespace Epic.OnlineServices.Ecom
{
	// Token: 0x02000509 RID: 1289
	public struct CheckoutOptions
	{
		// Token: 0x170009C1 RID: 2497
		// (get) Token: 0x060021E8 RID: 8680 RVA: 0x00031CDB File Offset: 0x0002FEDB
		// (set) Token: 0x060021E9 RID: 8681 RVA: 0x00031CE3 File Offset: 0x0002FEE3
		public EpicAccountId LocalUserId { get; set; }

		// Token: 0x170009C2 RID: 2498
		// (get) Token: 0x060021EA RID: 8682 RVA: 0x00031CEC File Offset: 0x0002FEEC
		// (set) Token: 0x060021EB RID: 8683 RVA: 0x00031CF4 File Offset: 0x0002FEF4
		public Utf8String OverrideCatalogNamespace { get; set; }

		// Token: 0x170009C3 RID: 2499
		// (get) Token: 0x060021EC RID: 8684 RVA: 0x00031CFD File Offset: 0x0002FEFD
		// (set) Token: 0x060021ED RID: 8685 RVA: 0x00031D05 File Offset: 0x0002FF05
		public CheckoutEntry[] Entries { get; set; }

		// Token: 0x170009C4 RID: 2500
		// (get) Token: 0x060021EE RID: 8686 RVA: 0x00031D0E File Offset: 0x0002FF0E
		// (set) Token: 0x060021EF RID: 8687 RVA: 0x00031D16 File Offset: 0x0002FF16
		public CheckoutOrientation PreferredOrientation { get; set; }
	}
}
