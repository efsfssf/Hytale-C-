using System;

namespace Epic.OnlineServices.Ecom
{
	// Token: 0x02000507 RID: 1287
	public struct CheckoutEntry
	{
		// Token: 0x170009BF RID: 2495
		// (get) Token: 0x060021DF RID: 8671 RVA: 0x00031C1B File Offset: 0x0002FE1B
		// (set) Token: 0x060021E0 RID: 8672 RVA: 0x00031C23 File Offset: 0x0002FE23
		public Utf8String OfferId { get; set; }

		// Token: 0x060021E1 RID: 8673 RVA: 0x00031C2C File Offset: 0x0002FE2C
		internal void Set(ref CheckoutEntryInternal other)
		{
			this.OfferId = other.OfferId;
		}
	}
}
