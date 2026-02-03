using System;

namespace Epic.OnlineServices.Metrics
{
	// Token: 0x0200034F RID: 847
	public struct BeginPlayerSessionOptionsAccountId
	{
		// Token: 0x17000661 RID: 1633
		// (get) Token: 0x06001734 RID: 5940 RVA: 0x00021DA4 File Offset: 0x0001FFA4
		// (set) Token: 0x06001735 RID: 5941 RVA: 0x00021DBC File Offset: 0x0001FFBC
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

		// Token: 0x17000662 RID: 1634
		// (get) Token: 0x06001736 RID: 5942 RVA: 0x00021DC8 File Offset: 0x0001FFC8
		// (set) Token: 0x06001737 RID: 5943 RVA: 0x00021DF0 File Offset: 0x0001FFF0
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

		// Token: 0x17000663 RID: 1635
		// (get) Token: 0x06001738 RID: 5944 RVA: 0x00021E08 File Offset: 0x00020008
		// (set) Token: 0x06001739 RID: 5945 RVA: 0x00021E30 File Offset: 0x00020030
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

		// Token: 0x0600173A RID: 5946 RVA: 0x00021E48 File Offset: 0x00020048
		public static implicit operator BeginPlayerSessionOptionsAccountId(EpicAccountId value)
		{
			return new BeginPlayerSessionOptionsAccountId
			{
				Epic = value
			};
		}

		// Token: 0x0600173B RID: 5947 RVA: 0x00021E6C File Offset: 0x0002006C
		public static implicit operator BeginPlayerSessionOptionsAccountId(Utf8String value)
		{
			return new BeginPlayerSessionOptionsAccountId
			{
				External = value
			};
		}

		// Token: 0x0600173C RID: 5948 RVA: 0x00021E90 File Offset: 0x00020090
		public static implicit operator BeginPlayerSessionOptionsAccountId(string value)
		{
			return new BeginPlayerSessionOptionsAccountId
			{
				External = value
			};
		}

		// Token: 0x0600173D RID: 5949 RVA: 0x00021EB9 File Offset: 0x000200B9
		internal void Set(ref BeginPlayerSessionOptionsAccountIdInternal other)
		{
			this.Epic = other.Epic;
			this.External = other.External;
		}

		// Token: 0x04000A19 RID: 2585
		private MetricsAccountIdType m_AccountIdType;

		// Token: 0x04000A1A RID: 2586
		private EpicAccountId m_Epic;

		// Token: 0x04000A1B RID: 2587
		private Utf8String m_External;
	}
}
