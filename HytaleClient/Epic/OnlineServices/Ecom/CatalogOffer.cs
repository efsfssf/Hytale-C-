using System;

namespace Epic.OnlineServices.Ecom
{
	// Token: 0x02000501 RID: 1281
	public struct CatalogOffer
	{
		// Token: 0x17000986 RID: 2438
		// (get) Token: 0x0600215E RID: 8542 RVA: 0x00030E78 File Offset: 0x0002F078
		// (set) Token: 0x0600215F RID: 8543 RVA: 0x00030E80 File Offset: 0x0002F080
		public int ServerIndex { get; set; }

		// Token: 0x17000987 RID: 2439
		// (get) Token: 0x06002160 RID: 8544 RVA: 0x00030E89 File Offset: 0x0002F089
		// (set) Token: 0x06002161 RID: 8545 RVA: 0x00030E91 File Offset: 0x0002F091
		public Utf8String CatalogNamespace { get; set; }

		// Token: 0x17000988 RID: 2440
		// (get) Token: 0x06002162 RID: 8546 RVA: 0x00030E9A File Offset: 0x0002F09A
		// (set) Token: 0x06002163 RID: 8547 RVA: 0x00030EA2 File Offset: 0x0002F0A2
		public Utf8String Id { get; set; }

		// Token: 0x17000989 RID: 2441
		// (get) Token: 0x06002164 RID: 8548 RVA: 0x00030EAB File Offset: 0x0002F0AB
		// (set) Token: 0x06002165 RID: 8549 RVA: 0x00030EB3 File Offset: 0x0002F0B3
		public Utf8String TitleText { get; set; }

		// Token: 0x1700098A RID: 2442
		// (get) Token: 0x06002166 RID: 8550 RVA: 0x00030EBC File Offset: 0x0002F0BC
		// (set) Token: 0x06002167 RID: 8551 RVA: 0x00030EC4 File Offset: 0x0002F0C4
		public Utf8String DescriptionText { get; set; }

		// Token: 0x1700098B RID: 2443
		// (get) Token: 0x06002168 RID: 8552 RVA: 0x00030ECD File Offset: 0x0002F0CD
		// (set) Token: 0x06002169 RID: 8553 RVA: 0x00030ED5 File Offset: 0x0002F0D5
		public Utf8String LongDescriptionText { get; set; }

		// Token: 0x1700098C RID: 2444
		// (get) Token: 0x0600216A RID: 8554 RVA: 0x00030EDE File Offset: 0x0002F0DE
		// (set) Token: 0x0600216B RID: 8555 RVA: 0x00030EE6 File Offset: 0x0002F0E6
		internal Utf8String TechnicalDetailsText_DEPRECATED { get; set; }

		// Token: 0x1700098D RID: 2445
		// (get) Token: 0x0600216C RID: 8556 RVA: 0x00030EEF File Offset: 0x0002F0EF
		// (set) Token: 0x0600216D RID: 8557 RVA: 0x00030EF7 File Offset: 0x0002F0F7
		public Utf8String CurrencyCode { get; set; }

		// Token: 0x1700098E RID: 2446
		// (get) Token: 0x0600216E RID: 8558 RVA: 0x00030F00 File Offset: 0x0002F100
		// (set) Token: 0x0600216F RID: 8559 RVA: 0x00030F08 File Offset: 0x0002F108
		public Result PriceResult { get; set; }

		// Token: 0x1700098F RID: 2447
		// (get) Token: 0x06002170 RID: 8560 RVA: 0x00030F11 File Offset: 0x0002F111
		// (set) Token: 0x06002171 RID: 8561 RVA: 0x00030F19 File Offset: 0x0002F119
		internal uint OriginalPrice_DEPRECATED { get; set; }

		// Token: 0x17000990 RID: 2448
		// (get) Token: 0x06002172 RID: 8562 RVA: 0x00030F22 File Offset: 0x0002F122
		// (set) Token: 0x06002173 RID: 8563 RVA: 0x00030F2A File Offset: 0x0002F12A
		internal uint CurrentPrice_DEPRECATED { get; set; }

		// Token: 0x17000991 RID: 2449
		// (get) Token: 0x06002174 RID: 8564 RVA: 0x00030F33 File Offset: 0x0002F133
		// (set) Token: 0x06002175 RID: 8565 RVA: 0x00030F3B File Offset: 0x0002F13B
		public byte DiscountPercentage { get; set; }

		// Token: 0x17000992 RID: 2450
		// (get) Token: 0x06002176 RID: 8566 RVA: 0x00030F44 File Offset: 0x0002F144
		// (set) Token: 0x06002177 RID: 8567 RVA: 0x00030F4C File Offset: 0x0002F14C
		public long ExpirationTimestamp { get; set; }

		// Token: 0x17000993 RID: 2451
		// (get) Token: 0x06002178 RID: 8568 RVA: 0x00030F55 File Offset: 0x0002F155
		// (set) Token: 0x06002179 RID: 8569 RVA: 0x00030F5D File Offset: 0x0002F15D
		internal uint PurchasedCount_DEPRECATED { get; set; }

		// Token: 0x17000994 RID: 2452
		// (get) Token: 0x0600217A RID: 8570 RVA: 0x00030F66 File Offset: 0x0002F166
		// (set) Token: 0x0600217B RID: 8571 RVA: 0x00030F6E File Offset: 0x0002F16E
		public int PurchaseLimit { get; set; }

		// Token: 0x17000995 RID: 2453
		// (get) Token: 0x0600217C RID: 8572 RVA: 0x00030F77 File Offset: 0x0002F177
		// (set) Token: 0x0600217D RID: 8573 RVA: 0x00030F7F File Offset: 0x0002F17F
		public bool AvailableForPurchase { get; set; }

		// Token: 0x17000996 RID: 2454
		// (get) Token: 0x0600217E RID: 8574 RVA: 0x00030F88 File Offset: 0x0002F188
		// (set) Token: 0x0600217F RID: 8575 RVA: 0x00030F90 File Offset: 0x0002F190
		public ulong OriginalPrice64 { get; set; }

		// Token: 0x17000997 RID: 2455
		// (get) Token: 0x06002180 RID: 8576 RVA: 0x00030F99 File Offset: 0x0002F199
		// (set) Token: 0x06002181 RID: 8577 RVA: 0x00030FA1 File Offset: 0x0002F1A1
		public ulong CurrentPrice64 { get; set; }

		// Token: 0x17000998 RID: 2456
		// (get) Token: 0x06002182 RID: 8578 RVA: 0x00030FAA File Offset: 0x0002F1AA
		// (set) Token: 0x06002183 RID: 8579 RVA: 0x00030FB2 File Offset: 0x0002F1B2
		public uint DecimalPoint { get; set; }

		// Token: 0x17000999 RID: 2457
		// (get) Token: 0x06002184 RID: 8580 RVA: 0x00030FBB File Offset: 0x0002F1BB
		// (set) Token: 0x06002185 RID: 8581 RVA: 0x00030FC3 File Offset: 0x0002F1C3
		public long ReleaseDateTimestamp { get; set; }

		// Token: 0x1700099A RID: 2458
		// (get) Token: 0x06002186 RID: 8582 RVA: 0x00030FCC File Offset: 0x0002F1CC
		// (set) Token: 0x06002187 RID: 8583 RVA: 0x00030FD4 File Offset: 0x0002F1D4
		public long EffectiveDateTimestamp { get; set; }

		// Token: 0x06002188 RID: 8584 RVA: 0x00030FE0 File Offset: 0x0002F1E0
		internal void Set(ref CatalogOfferInternal other)
		{
			this.ServerIndex = other.ServerIndex;
			this.CatalogNamespace = other.CatalogNamespace;
			this.Id = other.Id;
			this.TitleText = other.TitleText;
			this.DescriptionText = other.DescriptionText;
			this.LongDescriptionText = other.LongDescriptionText;
			this.TechnicalDetailsText_DEPRECATED = other.TechnicalDetailsText_DEPRECATED;
			this.CurrencyCode = other.CurrencyCode;
			this.PriceResult = other.PriceResult;
			this.OriginalPrice_DEPRECATED = other.OriginalPrice_DEPRECATED;
			this.CurrentPrice_DEPRECATED = other.CurrentPrice_DEPRECATED;
			this.DiscountPercentage = other.DiscountPercentage;
			this.ExpirationTimestamp = other.ExpirationTimestamp;
			this.PurchasedCount_DEPRECATED = other.PurchasedCount_DEPRECATED;
			this.PurchaseLimit = other.PurchaseLimit;
			this.AvailableForPurchase = other.AvailableForPurchase;
			this.OriginalPrice64 = other.OriginalPrice64;
			this.CurrentPrice64 = other.CurrentPrice64;
			this.DecimalPoint = other.DecimalPoint;
			this.ReleaseDateTimestamp = other.ReleaseDateTimestamp;
			this.EffectiveDateTimestamp = other.EffectiveDateTimestamp;
		}
	}
}
