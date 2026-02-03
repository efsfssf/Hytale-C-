using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Ecom
{
	// Token: 0x0200053D RID: 1341
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	internal struct ItemOwnershipInternal : IGettable<ItemOwnership>, ISettable<ItemOwnership>, IDisposable
	{
		// Token: 0x17000A31 RID: 2609
		// (get) Token: 0x0600230A RID: 8970 RVA: 0x00033DF4 File Offset: 0x00031FF4
		// (set) Token: 0x0600230B RID: 8971 RVA: 0x00033E15 File Offset: 0x00032015
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

		// Token: 0x17000A32 RID: 2610
		// (get) Token: 0x0600230C RID: 8972 RVA: 0x00033E28 File Offset: 0x00032028
		// (set) Token: 0x0600230D RID: 8973 RVA: 0x00033E40 File Offset: 0x00032040
		public OwnershipStatus OwnershipStatus
		{
			get
			{
				return this.m_OwnershipStatus;
			}
			set
			{
				this.m_OwnershipStatus = value;
			}
		}

		// Token: 0x0600230E RID: 8974 RVA: 0x00033E4A File Offset: 0x0003204A
		public void Set(ref ItemOwnership other)
		{
			this.m_ApiVersion = 1;
			this.Id = other.Id;
			this.OwnershipStatus = other.OwnershipStatus;
		}

		// Token: 0x0600230F RID: 8975 RVA: 0x00033E70 File Offset: 0x00032070
		public void Set(ref ItemOwnership? other)
		{
			bool flag = other != null;
			if (flag)
			{
				this.m_ApiVersion = 1;
				this.Id = other.Value.Id;
				this.OwnershipStatus = other.Value.OwnershipStatus;
			}
		}

		// Token: 0x06002310 RID: 8976 RVA: 0x00033EBB File Offset: 0x000320BB
		public void Dispose()
		{
			Helper.Dispose(ref this.m_Id);
		}

		// Token: 0x06002311 RID: 8977 RVA: 0x00033ECA File Offset: 0x000320CA
		public void Get(out ItemOwnership output)
		{
			output = default(ItemOwnership);
			output.Set(ref this);
		}

		// Token: 0x04000F76 RID: 3958
		private int m_ApiVersion;

		// Token: 0x04000F77 RID: 3959
		private IntPtr m_Id;

		// Token: 0x04000F78 RID: 3960
		private OwnershipStatus m_OwnershipStatus;
	}
}
