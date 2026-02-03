using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Metrics
{
	// Token: 0x02000354 RID: 852
	[StructLayout(LayoutKind.Explicit, Pack = 4)]
	internal struct EndPlayerSessionOptionsAccountIdInternal : IGettable<EndPlayerSessionOptionsAccountId>, ISettable<EndPlayerSessionOptionsAccountId>, IDisposable
	{
		// Token: 0x1700066B RID: 1643
		// (get) Token: 0x06001756 RID: 5974 RVA: 0x000221BC File Offset: 0x000203BC
		// (set) Token: 0x06001757 RID: 5975 RVA: 0x000221E4 File Offset: 0x000203E4
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
				Helper.Set<MetricsAccountIdType>(value, ref this.m_Epic, MetricsAccountIdType.Epic, ref this.m_AccountIdType, this);
			}
		}

		// Token: 0x1700066C RID: 1644
		// (get) Token: 0x06001758 RID: 5976 RVA: 0x00022208 File Offset: 0x00020408
		// (set) Token: 0x06001759 RID: 5977 RVA: 0x00022230 File Offset: 0x00020430
		public Utf8String External
		{
			get
			{
				Utf8String result;
				Helper.Get<MetricsAccountIdType>(this.m_External, out result, this.m_AccountIdType, MetricsAccountIdType.External);
				return result;
			}
			set
			{
				Helper.Set<MetricsAccountIdType>(value, ref this.m_External, MetricsAccountIdType.External, ref this.m_AccountIdType, this);
			}
		}

		// Token: 0x0600175A RID: 5978 RVA: 0x00022252 File Offset: 0x00020452
		public void Set(ref EndPlayerSessionOptionsAccountId other)
		{
			this.Epic = other.Epic;
			this.External = other.External;
		}

		// Token: 0x0600175B RID: 5979 RVA: 0x00022270 File Offset: 0x00020470
		public void Set(ref EndPlayerSessionOptionsAccountId? other)
		{
			bool flag = other != null;
			if (flag)
			{
				this.Epic = other.Value.Epic;
				this.External = other.Value.External;
			}
		}

		// Token: 0x0600175C RID: 5980 RVA: 0x000222B4 File Offset: 0x000204B4
		public void Dispose()
		{
			Helper.Dispose(ref this.m_Epic);
			Helper.Dispose<MetricsAccountIdType>(ref this.m_External, this.m_AccountIdType, MetricsAccountIdType.External);
		}

		// Token: 0x0600175D RID: 5981 RVA: 0x000222D6 File Offset: 0x000204D6
		public void Get(out EndPlayerSessionOptionsAccountId output)
		{
			output = default(EndPlayerSessionOptionsAccountId);
			output.Set(ref this);
		}

		// Token: 0x04000A25 RID: 2597
		[FieldOffset(0)]
		private MetricsAccountIdType m_AccountIdType;

		// Token: 0x04000A26 RID: 2598
		[FieldOffset(4)]
		private IntPtr m_Epic;

		// Token: 0x04000A27 RID: 2599
		[FieldOffset(4)]
		private IntPtr m_External;
	}
}
