using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Connect
{
	// Token: 0x0200061E RID: 1566
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	internal struct TransferDeviceIdAccountOptionsInternal : ISettable<TransferDeviceIdAccountOptions>, IDisposable
	{
		// Token: 0x17000BC4 RID: 3012
		// (set) Token: 0x0600287C RID: 10364 RVA: 0x0003B521 File Offset: 0x00039721
		public ProductUserId PrimaryLocalUserId
		{
			set
			{
				Helper.Set(value, ref this.m_PrimaryLocalUserId);
			}
		}

		// Token: 0x17000BC5 RID: 3013
		// (set) Token: 0x0600287D RID: 10365 RVA: 0x0003B531 File Offset: 0x00039731
		public ProductUserId LocalDeviceUserId
		{
			set
			{
				Helper.Set(value, ref this.m_LocalDeviceUserId);
			}
		}

		// Token: 0x17000BC6 RID: 3014
		// (set) Token: 0x0600287E RID: 10366 RVA: 0x0003B541 File Offset: 0x00039741
		public ProductUserId ProductUserIdToPreserve
		{
			set
			{
				Helper.Set(value, ref this.m_ProductUserIdToPreserve);
			}
		}

		// Token: 0x0600287F RID: 10367 RVA: 0x0003B551 File Offset: 0x00039751
		public void Set(ref TransferDeviceIdAccountOptions other)
		{
			this.m_ApiVersion = 1;
			this.PrimaryLocalUserId = other.PrimaryLocalUserId;
			this.LocalDeviceUserId = other.LocalDeviceUserId;
			this.ProductUserIdToPreserve = other.ProductUserIdToPreserve;
		}

		// Token: 0x06002880 RID: 10368 RVA: 0x0003B584 File Offset: 0x00039784
		public void Set(ref TransferDeviceIdAccountOptions? other)
		{
			bool flag = other != null;
			if (flag)
			{
				this.m_ApiVersion = 1;
				this.PrimaryLocalUserId = other.Value.PrimaryLocalUserId;
				this.LocalDeviceUserId = other.Value.LocalDeviceUserId;
				this.ProductUserIdToPreserve = other.Value.ProductUserIdToPreserve;
			}
		}

		// Token: 0x06002881 RID: 10369 RVA: 0x0003B5E4 File Offset: 0x000397E4
		public void Dispose()
		{
			Helper.Dispose(ref this.m_PrimaryLocalUserId);
			Helper.Dispose(ref this.m_LocalDeviceUserId);
			Helper.Dispose(ref this.m_ProductUserIdToPreserve);
		}

		// Token: 0x0400115D RID: 4445
		private int m_ApiVersion;

		// Token: 0x0400115E RID: 4446
		private IntPtr m_PrimaryLocalUserId;

		// Token: 0x0400115F RID: 4447
		private IntPtr m_LocalDeviceUserId;

		// Token: 0x04001160 RID: 4448
		private IntPtr m_ProductUserIdToPreserve;
	}
}
