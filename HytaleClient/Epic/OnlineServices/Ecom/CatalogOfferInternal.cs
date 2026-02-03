using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Ecom
{
	// Token: 0x02000502 RID: 1282
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	internal struct CatalogOfferInternal : IGettable<CatalogOffer>, ISettable<CatalogOffer>, IDisposable
	{
		// Token: 0x1700099B RID: 2459
		// (get) Token: 0x06002189 RID: 8585 RVA: 0x00031100 File Offset: 0x0002F300
		// (set) Token: 0x0600218A RID: 8586 RVA: 0x00031118 File Offset: 0x0002F318
		public int ServerIndex
		{
			get
			{
				return this.m_ServerIndex;
			}
			set
			{
				this.m_ServerIndex = value;
			}
		}

		// Token: 0x1700099C RID: 2460
		// (get) Token: 0x0600218B RID: 8587 RVA: 0x00031124 File Offset: 0x0002F324
		// (set) Token: 0x0600218C RID: 8588 RVA: 0x00031145 File Offset: 0x0002F345
		public Utf8String CatalogNamespace
		{
			get
			{
				Utf8String result;
				Helper.Get(this.m_CatalogNamespace, out result);
				return result;
			}
			set
			{
				Helper.Set(value, ref this.m_CatalogNamespace);
			}
		}

		// Token: 0x1700099D RID: 2461
		// (get) Token: 0x0600218D RID: 8589 RVA: 0x00031158 File Offset: 0x0002F358
		// (set) Token: 0x0600218E RID: 8590 RVA: 0x00031179 File Offset: 0x0002F379
		public Utf8String Id
		{
			get
			{
				Utf8String result;
				Helper.Get(this.m_Id, out result);
				return result;
			}
			set
			{
				Helper.Set(value, ref this.m_Id);
			}
		}

		// Token: 0x1700099E RID: 2462
		// (get) Token: 0x0600218F RID: 8591 RVA: 0x0003118C File Offset: 0x0002F38C
		// (set) Token: 0x06002190 RID: 8592 RVA: 0x000311AD File Offset: 0x0002F3AD
		public Utf8String TitleText
		{
			get
			{
				Utf8String result;
				Helper.Get(this.m_TitleText, out result);
				return result;
			}
			set
			{
				Helper.Set(value, ref this.m_TitleText);
			}
		}

		// Token: 0x1700099F RID: 2463
		// (get) Token: 0x06002191 RID: 8593 RVA: 0x000311C0 File Offset: 0x0002F3C0
		// (set) Token: 0x06002192 RID: 8594 RVA: 0x000311E1 File Offset: 0x0002F3E1
		public Utf8String DescriptionText
		{
			get
			{
				Utf8String result;
				Helper.Get(this.m_DescriptionText, out result);
				return result;
			}
			set
			{
				Helper.Set(value, ref this.m_DescriptionText);
			}
		}

		// Token: 0x170009A0 RID: 2464
		// (get) Token: 0x06002193 RID: 8595 RVA: 0x000311F4 File Offset: 0x0002F3F4
		// (set) Token: 0x06002194 RID: 8596 RVA: 0x00031215 File Offset: 0x0002F415
		public Utf8String LongDescriptionText
		{
			get
			{
				Utf8String result;
				Helper.Get(this.m_LongDescriptionText, out result);
				return result;
			}
			set
			{
				Helper.Set(value, ref this.m_LongDescriptionText);
			}
		}

		// Token: 0x170009A1 RID: 2465
		// (get) Token: 0x06002195 RID: 8597 RVA: 0x00031228 File Offset: 0x0002F428
		// (set) Token: 0x06002196 RID: 8598 RVA: 0x00031249 File Offset: 0x0002F449
		public Utf8String TechnicalDetailsText_DEPRECATED
		{
			get
			{
				Utf8String result;
				Helper.Get(this.m_TechnicalDetailsText_DEPRECATED, out result);
				return result;
			}
			set
			{
				Helper.Set(value, ref this.m_TechnicalDetailsText_DEPRECATED);
			}
		}

		// Token: 0x170009A2 RID: 2466
		// (get) Token: 0x06002197 RID: 8599 RVA: 0x0003125C File Offset: 0x0002F45C
		// (set) Token: 0x06002198 RID: 8600 RVA: 0x0003127D File Offset: 0x0002F47D
		public Utf8String CurrencyCode
		{
			get
			{
				Utf8String result;
				Helper.Get(this.m_CurrencyCode, out result);
				return result;
			}
			set
			{
				Helper.Set(value, ref this.m_CurrencyCode);
			}
		}

		// Token: 0x170009A3 RID: 2467
		// (get) Token: 0x06002199 RID: 8601 RVA: 0x00031290 File Offset: 0x0002F490
		// (set) Token: 0x0600219A RID: 8602 RVA: 0x000312A8 File Offset: 0x0002F4A8
		public Result PriceResult
		{
			get
			{
				return this.m_PriceResult;
			}
			set
			{
				this.m_PriceResult = value;
			}
		}

		// Token: 0x170009A4 RID: 2468
		// (get) Token: 0x0600219B RID: 8603 RVA: 0x000312B4 File Offset: 0x0002F4B4
		// (set) Token: 0x0600219C RID: 8604 RVA: 0x000312CC File Offset: 0x0002F4CC
		public uint OriginalPrice_DEPRECATED
		{
			get
			{
				return this.m_OriginalPrice_DEPRECATED;
			}
			set
			{
				this.m_OriginalPrice_DEPRECATED = value;
			}
		}

		// Token: 0x170009A5 RID: 2469
		// (get) Token: 0x0600219D RID: 8605 RVA: 0x000312D8 File Offset: 0x0002F4D8
		// (set) Token: 0x0600219E RID: 8606 RVA: 0x000312F0 File Offset: 0x0002F4F0
		public uint CurrentPrice_DEPRECATED
		{
			get
			{
				return this.m_CurrentPrice_DEPRECATED;
			}
			set
			{
				this.m_CurrentPrice_DEPRECATED = value;
			}
		}

		// Token: 0x170009A6 RID: 2470
		// (get) Token: 0x0600219F RID: 8607 RVA: 0x000312FC File Offset: 0x0002F4FC
		// (set) Token: 0x060021A0 RID: 8608 RVA: 0x00031314 File Offset: 0x0002F514
		public byte DiscountPercentage
		{
			get
			{
				return this.m_DiscountPercentage;
			}
			set
			{
				this.m_DiscountPercentage = value;
			}
		}

		// Token: 0x170009A7 RID: 2471
		// (get) Token: 0x060021A1 RID: 8609 RVA: 0x00031320 File Offset: 0x0002F520
		// (set) Token: 0x060021A2 RID: 8610 RVA: 0x00031338 File Offset: 0x0002F538
		public long ExpirationTimestamp
		{
			get
			{
				return this.m_ExpirationTimestamp;
			}
			set
			{
				this.m_ExpirationTimestamp = value;
			}
		}

		// Token: 0x170009A8 RID: 2472
		// (get) Token: 0x060021A3 RID: 8611 RVA: 0x00031344 File Offset: 0x0002F544
		// (set) Token: 0x060021A4 RID: 8612 RVA: 0x0003135C File Offset: 0x0002F55C
		public uint PurchasedCount_DEPRECATED
		{
			get
			{
				return this.m_PurchasedCount_DEPRECATED;
			}
			set
			{
				this.m_PurchasedCount_DEPRECATED = value;
			}
		}

		// Token: 0x170009A9 RID: 2473
		// (get) Token: 0x060021A5 RID: 8613 RVA: 0x00031368 File Offset: 0x0002F568
		// (set) Token: 0x060021A6 RID: 8614 RVA: 0x00031380 File Offset: 0x0002F580
		public int PurchaseLimit
		{
			get
			{
				return this.m_PurchaseLimit;
			}
			set
			{
				this.m_PurchaseLimit = value;
			}
		}

		// Token: 0x170009AA RID: 2474
		// (get) Token: 0x060021A7 RID: 8615 RVA: 0x0003138C File Offset: 0x0002F58C
		// (set) Token: 0x060021A8 RID: 8616 RVA: 0x000313AD File Offset: 0x0002F5AD
		public bool AvailableForPurchase
		{
			get
			{
				bool result;
				Helper.Get(this.m_AvailableForPurchase, out result);
				return result;
			}
			set
			{
				Helper.Set(value, ref this.m_AvailableForPurchase);
			}
		}

		// Token: 0x170009AB RID: 2475
		// (get) Token: 0x060021A9 RID: 8617 RVA: 0x000313C0 File Offset: 0x0002F5C0
		// (set) Token: 0x060021AA RID: 8618 RVA: 0x000313D8 File Offset: 0x0002F5D8
		public ulong OriginalPrice64
		{
			get
			{
				return this.m_OriginalPrice64;
			}
			set
			{
				this.m_OriginalPrice64 = value;
			}
		}

		// Token: 0x170009AC RID: 2476
		// (get) Token: 0x060021AB RID: 8619 RVA: 0x000313E4 File Offset: 0x0002F5E4
		// (set) Token: 0x060021AC RID: 8620 RVA: 0x000313FC File Offset: 0x0002F5FC
		public ulong CurrentPrice64
		{
			get
			{
				return this.m_CurrentPrice64;
			}
			set
			{
				this.m_CurrentPrice64 = value;
			}
		}

		// Token: 0x170009AD RID: 2477
		// (get) Token: 0x060021AD RID: 8621 RVA: 0x00031408 File Offset: 0x0002F608
		// (set) Token: 0x060021AE RID: 8622 RVA: 0x00031420 File Offset: 0x0002F620
		public uint DecimalPoint
		{
			get
			{
				return this.m_DecimalPoint;
			}
			set
			{
				this.m_DecimalPoint = value;
			}
		}

		// Token: 0x170009AE RID: 2478
		// (get) Token: 0x060021AF RID: 8623 RVA: 0x0003142C File Offset: 0x0002F62C
		// (set) Token: 0x060021B0 RID: 8624 RVA: 0x00031444 File Offset: 0x0002F644
		public long ReleaseDateTimestamp
		{
			get
			{
				return this.m_ReleaseDateTimestamp;
			}
			set
			{
				this.m_ReleaseDateTimestamp = value;
			}
		}

		// Token: 0x170009AF RID: 2479
		// (get) Token: 0x060021B1 RID: 8625 RVA: 0x00031450 File Offset: 0x0002F650
		// (set) Token: 0x060021B2 RID: 8626 RVA: 0x00031468 File Offset: 0x0002F668
		public long EffectiveDateTimestamp
		{
			get
			{
				return this.m_EffectiveDateTimestamp;
			}
			set
			{
				this.m_EffectiveDateTimestamp = value;
			}
		}

		// Token: 0x060021B3 RID: 8627 RVA: 0x00031474 File Offset: 0x0002F674
		public void Set(ref CatalogOffer other)
		{
			this.m_ApiVersion = 5;
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

		// Token: 0x060021B4 RID: 8628 RVA: 0x0003159C File Offset: 0x0002F79C
		public void Set(ref CatalogOffer? other)
		{
			bool flag = other != null;
			if (flag)
			{
				this.m_ApiVersion = 5;
				this.ServerIndex = other.Value.ServerIndex;
				this.CatalogNamespace = other.Value.CatalogNamespace;
				this.Id = other.Value.Id;
				this.TitleText = other.Value.TitleText;
				this.DescriptionText = other.Value.DescriptionText;
				this.LongDescriptionText = other.Value.LongDescriptionText;
				this.TechnicalDetailsText_DEPRECATED = other.Value.TechnicalDetailsText_DEPRECATED;
				this.CurrencyCode = other.Value.CurrencyCode;
				this.PriceResult = other.Value.PriceResult;
				this.OriginalPrice_DEPRECATED = other.Value.OriginalPrice_DEPRECATED;
				this.CurrentPrice_DEPRECATED = other.Value.CurrentPrice_DEPRECATED;
				this.DiscountPercentage = other.Value.DiscountPercentage;
				this.ExpirationTimestamp = other.Value.ExpirationTimestamp;
				this.PurchasedCount_DEPRECATED = other.Value.PurchasedCount_DEPRECATED;
				this.PurchaseLimit = other.Value.PurchaseLimit;
				this.AvailableForPurchase = other.Value.AvailableForPurchase;
				this.OriginalPrice64 = other.Value.OriginalPrice64;
				this.CurrentPrice64 = other.Value.CurrentPrice64;
				this.DecimalPoint = other.Value.DecimalPoint;
				this.ReleaseDateTimestamp = other.Value.ReleaseDateTimestamp;
				this.EffectiveDateTimestamp = other.Value.EffectiveDateTimestamp;
			}
		}

		// Token: 0x060021B5 RID: 8629 RVA: 0x0003177C File Offset: 0x0002F97C
		public void Dispose()
		{
			Helper.Dispose(ref this.m_CatalogNamespace);
			Helper.Dispose(ref this.m_Id);
			Helper.Dispose(ref this.m_TitleText);
			Helper.Dispose(ref this.m_DescriptionText);
			Helper.Dispose(ref this.m_LongDescriptionText);
			Helper.Dispose(ref this.m_TechnicalDetailsText_DEPRECATED);
			Helper.Dispose(ref this.m_CurrencyCode);
		}

		// Token: 0x060021B6 RID: 8630 RVA: 0x000317DE File Offset: 0x0002F9DE
		public void Get(out CatalogOffer output)
		{
			output = default(CatalogOffer);
			output.Set(ref this);
		}

		// Token: 0x04000E88 RID: 3720
		private int m_ApiVersion;

		// Token: 0x04000E89 RID: 3721
		private int m_ServerIndex;

		// Token: 0x04000E8A RID: 3722
		private IntPtr m_CatalogNamespace;

		// Token: 0x04000E8B RID: 3723
		private IntPtr m_Id;

		// Token: 0x04000E8C RID: 3724
		private IntPtr m_TitleText;

		// Token: 0x04000E8D RID: 3725
		private IntPtr m_DescriptionText;

		// Token: 0x04000E8E RID: 3726
		private IntPtr m_LongDescriptionText;

		// Token: 0x04000E8F RID: 3727
		private IntPtr m_TechnicalDetailsText_DEPRECATED;

		// Token: 0x04000E90 RID: 3728
		private IntPtr m_CurrencyCode;

		// Token: 0x04000E91 RID: 3729
		private Result m_PriceResult;

		// Token: 0x04000E92 RID: 3730
		private uint m_OriginalPrice_DEPRECATED;

		// Token: 0x04000E93 RID: 3731
		private uint m_CurrentPrice_DEPRECATED;

		// Token: 0x04000E94 RID: 3732
		private byte m_DiscountPercentage;

		// Token: 0x04000E95 RID: 3733
		private long m_ExpirationTimestamp;

		// Token: 0x04000E96 RID: 3734
		private uint m_PurchasedCount_DEPRECATED;

		// Token: 0x04000E97 RID: 3735
		private int m_PurchaseLimit;

		// Token: 0x04000E98 RID: 3736
		private int m_AvailableForPurchase;

		// Token: 0x04000E99 RID: 3737
		private ulong m_OriginalPrice64;

		// Token: 0x04000E9A RID: 3738
		private ulong m_CurrentPrice64;

		// Token: 0x04000E9B RID: 3739
		private uint m_DecimalPoint;

		// Token: 0x04000E9C RID: 3740
		private long m_ReleaseDateTimestamp;

		// Token: 0x04000E9D RID: 3741
		private long m_EffectiveDateTimestamp;
	}
}
