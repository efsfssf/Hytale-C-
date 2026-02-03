using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.RTC
{
	// Token: 0x020001AC RID: 428
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	internal struct AddNotifyDisconnectedOptionsInternal : ISettable<AddNotifyDisconnectedOptions>, IDisposable
	{
		// Token: 0x170002E2 RID: 738
		// (set) Token: 0x06000C5C RID: 3164 RVA: 0x000120D1 File Offset: 0x000102D1
		public ProductUserId LocalUserId
		{
			set
			{
				Helper.Set(value, ref this.m_LocalUserId);
			}
		}

		// Token: 0x170002E3 RID: 739
		// (set) Token: 0x06000C5D RID: 3165 RVA: 0x000120E1 File Offset: 0x000102E1
		public Utf8String RoomName
		{
			set
			{
				Helper.Set(value, ref this.m_RoomName);
			}
		}

		// Token: 0x06000C5E RID: 3166 RVA: 0x000120F1 File Offset: 0x000102F1
		public void Set(ref AddNotifyDisconnectedOptions other)
		{
			this.m_ApiVersion = 1;
			this.LocalUserId = other.LocalUserId;
			this.RoomName = other.RoomName;
		}

		// Token: 0x06000C5F RID: 3167 RVA: 0x00012118 File Offset: 0x00010318
		public void Set(ref AddNotifyDisconnectedOptions? other)
		{
			bool flag = other != null;
			if (flag)
			{
				this.m_ApiVersion = 1;
				this.LocalUserId = other.Value.LocalUserId;
				this.RoomName = other.Value.RoomName;
			}
		}

		// Token: 0x06000C60 RID: 3168 RVA: 0x00012163 File Offset: 0x00010363
		public void Dispose()
		{
			Helper.Dispose(ref this.m_LocalUserId);
			Helper.Dispose(ref this.m_RoomName);
		}

		// Token: 0x040005A9 RID: 1449
		private int m_ApiVersion;

		// Token: 0x040005AA RID: 1450
		private IntPtr m_LocalUserId;

		// Token: 0x040005AB RID: 1451
		private IntPtr m_RoomName;
	}
}
