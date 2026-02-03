using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.RTCData
{
	// Token: 0x020001DB RID: 475
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	internal struct AddNotifyDataReceivedOptionsInternal : ISettable<AddNotifyDataReceivedOptions>, IDisposable
	{
		// Token: 0x17000360 RID: 864
		// (set) Token: 0x06000DC3 RID: 3523 RVA: 0x00014264 File Offset: 0x00012464
		public ProductUserId LocalUserId
		{
			set
			{
				Helper.Set(value, ref this.m_LocalUserId);
			}
		}

		// Token: 0x17000361 RID: 865
		// (set) Token: 0x06000DC4 RID: 3524 RVA: 0x00014274 File Offset: 0x00012474
		public Utf8String RoomName
		{
			set
			{
				Helper.Set(value, ref this.m_RoomName);
			}
		}

		// Token: 0x06000DC5 RID: 3525 RVA: 0x00014284 File Offset: 0x00012484
		public void Set(ref AddNotifyDataReceivedOptions other)
		{
			this.m_ApiVersion = 1;
			this.LocalUserId = other.LocalUserId;
			this.RoomName = other.RoomName;
		}

		// Token: 0x06000DC6 RID: 3526 RVA: 0x000142A8 File Offset: 0x000124A8
		public void Set(ref AddNotifyDataReceivedOptions? other)
		{
			bool flag = other != null;
			if (flag)
			{
				this.m_ApiVersion = 1;
				this.LocalUserId = other.Value.LocalUserId;
				this.RoomName = other.Value.RoomName;
			}
		}

		// Token: 0x06000DC7 RID: 3527 RVA: 0x000142F3 File Offset: 0x000124F3
		public void Dispose()
		{
			Helper.Dispose(ref this.m_LocalUserId);
			Helper.Dispose(ref this.m_RoomName);
		}

		// Token: 0x04000642 RID: 1602
		private int m_ApiVersion;

		// Token: 0x04000643 RID: 1603
		private IntPtr m_LocalUserId;

		// Token: 0x04000644 RID: 1604
		private IntPtr m_RoomName;
	}
}
