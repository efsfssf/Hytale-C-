using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Ecom
{
	// Token: 0x02000529 RID: 1321
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	internal struct EntitlementInternal : IGettable<Entitlement>, ISettable<Entitlement>, IDisposable
	{
		// Token: 0x17000A0D RID: 2573
		// (get) Token: 0x060022B0 RID: 8880 RVA: 0x00033560 File Offset: 0x00031760
		// (set) Token: 0x060022B1 RID: 8881 RVA: 0x00033581 File Offset: 0x00031781
		public Utf8String EntitlementName
		{
			get
			{
				Utf8String result;
				Helper.Get(this.m_EntitlementName, out result);
				return result;
			}
			set
			{
				Helper.Set(value, ref this.m_EntitlementName);
			}
		}

		// Token: 0x17000A0E RID: 2574
		// (get) Token: 0x060022B2 RID: 8882 RVA: 0x00033594 File Offset: 0x00031794
		// (set) Token: 0x060022B3 RID: 8883 RVA: 0x000335B5 File Offset: 0x000317B5
		public Utf8String EntitlementId
		{
			get
			{
				Utf8String result;
				Helper.Get(this.m_EntitlementId, out result);
				return result;
			}
			set
			{
				Helper.Set(value, ref this.m_EntitlementId);
			}
		}

		// Token: 0x17000A0F RID: 2575
		// (get) Token: 0x060022B4 RID: 8884 RVA: 0x000335C8 File Offset: 0x000317C8
		// (set) Token: 0x060022B5 RID: 8885 RVA: 0x000335E9 File Offset: 0x000317E9
		public Utf8String CatalogItemId
		{
			get
			{
				Utf8String result;
				Helper.Get(this.m_CatalogItemId, out result);
				return result;
			}
			set
			{
				Helper.Set(value, ref this.m_CatalogItemId);
			}
		}

		// Token: 0x17000A10 RID: 2576
		// (get) Token: 0x060022B6 RID: 8886 RVA: 0x000335FC File Offset: 0x000317FC
		// (set) Token: 0x060022B7 RID: 8887 RVA: 0x00033614 File Offset: 0x00031814
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

		// Token: 0x17000A11 RID: 2577
		// (get) Token: 0x060022B8 RID: 8888 RVA: 0x00033620 File Offset: 0x00031820
		// (set) Token: 0x060022B9 RID: 8889 RVA: 0x00033641 File Offset: 0x00031841
		public bool Redeemed
		{
			get
			{
				bool result;
				Helper.Get(this.m_Redeemed, out result);
				return result;
			}
			set
			{
				Helper.Set(value, ref this.m_Redeemed);
			}
		}

		// Token: 0x17000A12 RID: 2578
		// (get) Token: 0x060022BA RID: 8890 RVA: 0x00033654 File Offset: 0x00031854
		// (set) Token: 0x060022BB RID: 8891 RVA: 0x0003366C File Offset: 0x0003186C
		public long EndTimestamp
		{
			get
			{
				return this.m_EndTimestamp;
			}
			set
			{
				this.m_EndTimestamp = value;
			}
		}

		// Token: 0x060022BC RID: 8892 RVA: 0x00033678 File Offset: 0x00031878
		public void Set(ref Entitlement other)
		{
			this.m_ApiVersion = 2;
			this.EntitlementName = other.EntitlementName;
			this.EntitlementId = other.EntitlementId;
			this.CatalogItemId = other.CatalogItemId;
			this.ServerIndex = other.ServerIndex;
			this.Redeemed = other.Redeemed;
			this.EndTimestamp = other.EndTimestamp;
		}

		// Token: 0x060022BD RID: 8893 RVA: 0x000336DC File Offset: 0x000318DC
		public void Set(ref Entitlement? other)
		{
			bool flag = other != null;
			if (flag)
			{
				this.m_ApiVersion = 2;
				this.EntitlementName = other.Value.EntitlementName;
				this.EntitlementId = other.Value.EntitlementId;
				this.CatalogItemId = other.Value.CatalogItemId;
				this.ServerIndex = other.Value.ServerIndex;
				this.Redeemed = other.Value.Redeemed;
				this.EndTimestamp = other.Value.EndTimestamp;
			}
		}

		// Token: 0x060022BE RID: 8894 RVA: 0x0003377E File Offset: 0x0003197E
		public void Dispose()
		{
			Helper.Dispose(ref this.m_EntitlementName);
			Helper.Dispose(ref this.m_EntitlementId);
			Helper.Dispose(ref this.m_CatalogItemId);
		}

		// Token: 0x060022BF RID: 8895 RVA: 0x000337A5 File Offset: 0x000319A5
		public void Get(out Entitlement output)
		{
			output = default(Entitlement);
			output.Set(ref this);
		}

		// Token: 0x04000F48 RID: 3912
		private int m_ApiVersion;

		// Token: 0x04000F49 RID: 3913
		private IntPtr m_EntitlementName;

		// Token: 0x04000F4A RID: 3914
		private IntPtr m_EntitlementId;

		// Token: 0x04000F4B RID: 3915
		private IntPtr m_CatalogItemId;

		// Token: 0x04000F4C RID: 3916
		private int m_ServerIndex;

		// Token: 0x04000F4D RID: 3917
		private int m_Redeemed;

		// Token: 0x04000F4E RID: 3918
		private long m_EndTimestamp;
	}
}
