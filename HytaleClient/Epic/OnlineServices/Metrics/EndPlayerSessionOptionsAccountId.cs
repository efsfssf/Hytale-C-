using System;

namespace Epic.OnlineServices.Metrics
{
	// Token: 0x02000353 RID: 851
	public struct EndPlayerSessionOptionsAccountId
	{
		// Token: 0x17000668 RID: 1640
		// (get) Token: 0x0600174C RID: 5964 RVA: 0x00022088 File Offset: 0x00020288
		// (set) Token: 0x0600174D RID: 5965 RVA: 0x000220A0 File Offset: 0x000202A0
		public MetricsAccountIdType AccountIdType
		{
			get
			{
				return this.m_AccountIdType;
			}
			private set
			{
				this.m_AccountIdType = value;
			}
		}

		// Token: 0x17000669 RID: 1641
		// (get) Token: 0x0600174E RID: 5966 RVA: 0x000220AC File Offset: 0x000202AC
		// (set) Token: 0x0600174F RID: 5967 RVA: 0x000220D4 File Offset: 0x000202D4
		public EpicAccountId Epic
		{
			get
			{
				EpicAccountId result;
				Helper.Get<EpicAccountId, MetricsAccountIdType>(this.m_Epic, out result, this.m_AccountIdType, MetricsAccountIdType.Epic);
				return result;
			}
			set
			{
				Helper.Set<EpicAccountId, MetricsAccountIdType>(value, ref this.m_Epic, MetricsAccountIdType.Epic, ref this.m_AccountIdType, null);
			}
		}

		// Token: 0x1700066A RID: 1642
		// (get) Token: 0x06001750 RID: 5968 RVA: 0x000220EC File Offset: 0x000202EC
		// (set) Token: 0x06001751 RID: 5969 RVA: 0x00022114 File Offset: 0x00020314
		public Utf8String External
		{
			get
			{
				Utf8String result;
				Helper.Get<Utf8String, MetricsAccountIdType>(this.m_External, out result, this.m_AccountIdType, MetricsAccountIdType.External);
				return result;
			}
			set
			{
				Helper.Set<Utf8String, MetricsAccountIdType>(value, ref this.m_External, MetricsAccountIdType.External, ref this.m_AccountIdType, null);
			}
		}

		// Token: 0x06001752 RID: 5970 RVA: 0x0002212C File Offset: 0x0002032C
		public static implicit operator EndPlayerSessionOptionsAccountId(EpicAccountId value)
		{
			return new EndPlayerSessionOptionsAccountId
			{
				Epic = value
			};
		}

		// Token: 0x06001753 RID: 5971 RVA: 0x00022150 File Offset: 0x00020350
		public static implicit operator EndPlayerSessionOptionsAccountId(Utf8String value)
		{
			return new EndPlayerSessionOptionsAccountId
			{
				External = value
			};
		}

		// Token: 0x06001754 RID: 5972 RVA: 0x00022174 File Offset: 0x00020374
		public static implicit operator EndPlayerSessionOptionsAccountId(string value)
		{
			return new EndPlayerSessionOptionsAccountId
			{
				External = value
			};
		}

		// Token: 0x06001755 RID: 5973 RVA: 0x0002219D File Offset: 0x0002039D
		internal void Set(ref EndPlayerSessionOptionsAccountIdInternal other)
		{
			this.Epic = other.Epic;
			this.External = other.External;
		}

		// Token: 0x04000A22 RID: 2594
		private MetricsAccountIdType m_AccountIdType;

		// Token: 0x04000A23 RID: 2595
		private EpicAccountId m_Epic;

		// Token: 0x04000A24 RID: 2596
		private Utf8String m_External;
	}
}
