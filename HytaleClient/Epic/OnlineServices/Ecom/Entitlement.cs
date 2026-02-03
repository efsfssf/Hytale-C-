using System;

namespace Epic.OnlineServices.Ecom
{
	// Token: 0x02000528 RID: 1320
	public struct Entitlement
	{
		// Token: 0x17000A07 RID: 2567
		// (get) Token: 0x060022A3 RID: 8867 RVA: 0x0003349B File Offset: 0x0003169B
		// (set) Token: 0x060022A4 RID: 8868 RVA: 0x000334A3 File Offset: 0x000316A3
		public Utf8String EntitlementName { get; set; }

		// Token: 0x17000A08 RID: 2568
		// (get) Token: 0x060022A5 RID: 8869 RVA: 0x000334AC File Offset: 0x000316AC
		// (set) Token: 0x060022A6 RID: 8870 RVA: 0x000334B4 File Offset: 0x000316B4
		public Utf8String EntitlementId { get; set; }

		// Token: 0x17000A09 RID: 2569
		// (get) Token: 0x060022A7 RID: 8871 RVA: 0x000334BD File Offset: 0x000316BD
		// (set) Token: 0x060022A8 RID: 8872 RVA: 0x000334C5 File Offset: 0x000316C5
		public Utf8String CatalogItemId { get; set; }

		// Token: 0x17000A0A RID: 2570
		// (get) Token: 0x060022A9 RID: 8873 RVA: 0x000334CE File Offset: 0x000316CE
		// (set) Token: 0x060022AA RID: 8874 RVA: 0x000334D6 File Offset: 0x000316D6
		public int ServerIndex { get; set; }

		// Token: 0x17000A0B RID: 2571
		// (get) Token: 0x060022AB RID: 8875 RVA: 0x000334DF File Offset: 0x000316DF
		// (set) Token: 0x060022AC RID: 8876 RVA: 0x000334E7 File Offset: 0x000316E7
		public bool Redeemed { get; set; }

		// Token: 0x17000A0C RID: 2572
		// (get) Token: 0x060022AD RID: 8877 RVA: 0x000334F0 File Offset: 0x000316F0
		// (set) Token: 0x060022AE RID: 8878 RVA: 0x000334F8 File Offset: 0x000316F8
		public long EndTimestamp { get; set; }

		// Token: 0x060022AF RID: 8879 RVA: 0x00033504 File Offset: 0x00031704
		internal void Set(ref EntitlementInternal other)
		{
			this.EntitlementName = other.EntitlementName;
			this.EntitlementId = other.EntitlementId;
			this.CatalogItemId = other.CatalogItemId;
			this.ServerIndex = other.ServerIndex;
			this.Redeemed = other.Redeemed;
			this.EndTimestamp = other.EndTimestamp;
		}
	}
}
