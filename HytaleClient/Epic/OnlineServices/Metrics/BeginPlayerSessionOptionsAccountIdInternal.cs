using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Metrics
{
	// Token: 0x02000350 RID: 848
	[StructLayout(LayoutKind.Explicit, Pack = 4)]
	internal struct BeginPlayerSessionOptionsAccountIdInternal : IGettable<BeginPlayerSessionOptionsAccountId>, ISettable<BeginPlayerSessionOptionsAccountId>, IDisposable
	{
		// Token: 0x17000664 RID: 1636
		// (get) Token: 0x0600173E RID: 5950 RVA: 0x00021ED8 File Offset: 0x000200D8
		// (set) Token: 0x0600173F RID: 5951 RVA: 0x00021F00 File Offset: 0x00020100
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

		// Token: 0x17000665 RID: 1637
		// (get) Token: 0x06001740 RID: 5952 RVA: 0x00021F24 File Offset: 0x00020124
		// (set) Token: 0x06001741 RID: 5953 RVA: 0x00021F4C File Offset: 0x0002014C
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

		// Token: 0x06001742 RID: 5954 RVA: 0x00021F6E File Offset: 0x0002016E
		public void Set(ref BeginPlayerSessionOptionsAccountId other)
		{
			this.Epic = other.Epic;
			this.External = other.External;
		}

		// Token: 0x06001743 RID: 5955 RVA: 0x00021F8C File Offset: 0x0002018C
		public void Set(ref BeginPlayerSessionOptionsAccountId? other)
		{
			bool flag = other != null;
			if (flag)
			{
				this.Epic = other.Value.Epic;
				this.External = other.Value.External;
			}
		}

		// Token: 0x06001744 RID: 5956 RVA: 0x00021FD0 File Offset: 0x000201D0
		public void Dispose()
		{
			Helper.Dispose(ref this.m_Epic);
			Helper.Dispose<MetricsAccountIdType>(ref this.m_External, this.m_AccountIdType, MetricsAccountIdType.External);
		}

		// Token: 0x06001745 RID: 5957 RVA: 0x00021FF2 File Offset: 0x000201F2
		public void Get(out BeginPlayerSessionOptionsAccountId output)
		{
			output = default(BeginPlayerSessionOptionsAccountId);
			output.Set(ref this);
		}

		// Token: 0x04000A1C RID: 2588
		[FieldOffset(0)]
		private MetricsAccountIdType m_AccountIdType;

		// Token: 0x04000A1D RID: 2589
		[FieldOffset(4)]
		private IntPtr m_Epic;

		// Token: 0x04000A1E RID: 2590
		[FieldOffset(4)]
		private IntPtr m_External;
	}
}
