using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.RTCData
{
	// Token: 0x020001F5 RID: 501
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	internal struct UpdateSendingOptionsInternal : ISettable<UpdateSendingOptions>, IDisposable
	{
		// Token: 0x170003A5 RID: 933
		// (set) Token: 0x06000E8F RID: 3727 RVA: 0x00015577 File Offset: 0x00013777
		public ProductUserId LocalUserId
		{
			set
			{
				Helper.Set(value, ref this.m_LocalUserId);
			}
		}

		// Token: 0x170003A6 RID: 934
		// (set) Token: 0x06000E90 RID: 3728 RVA: 0x00015587 File Offset: 0x00013787
		public Utf8String RoomName
		{
			set
			{
				Helper.Set(value, ref this.m_RoomName);
			}
		}

		// Token: 0x170003A7 RID: 935
		// (set) Token: 0x06000E91 RID: 3729 RVA: 0x00015597 File Offset: 0x00013797
		public bool DataEnabled
		{
			set
			{
				Helper.Set(value, ref this.m_DataEnabled);
			}
		}

		// Token: 0x06000E92 RID: 3730 RVA: 0x000155A7 File Offset: 0x000137A7
		public void Set(ref UpdateSendingOptions other)
		{
			this.m_ApiVersion = 1;
			this.LocalUserId = other.LocalUserId;
			this.RoomName = other.RoomName;
			this.DataEnabled = other.DataEnabled;
		}

		// Token: 0x06000E93 RID: 3731 RVA: 0x000155D8 File Offset: 0x000137D8
		public void Set(ref UpdateSendingOptions? other)
		{
			bool flag = other != null;
			if (flag)
			{
				this.m_ApiVersion = 1;
				this.LocalUserId = other.Value.LocalUserId;
				this.RoomName = other.Value.RoomName;
				this.DataEnabled = other.Value.DataEnabled;
			}
		}

		// Token: 0x06000E94 RID: 3732 RVA: 0x00015638 File Offset: 0x00013838
		public void Dispose()
		{
			Helper.Dispose(ref this.m_LocalUserId);
			Helper.Dispose(ref this.m_RoomName);
		}

		// Token: 0x04000693 RID: 1683
		private int m_ApiVersion;

		// Token: 0x04000694 RID: 1684
		private IntPtr m_LocalUserId;

		// Token: 0x04000695 RID: 1685
		private IntPtr m_RoomName;

		// Token: 0x04000696 RID: 1686
		private int m_DataEnabled;
	}
}
