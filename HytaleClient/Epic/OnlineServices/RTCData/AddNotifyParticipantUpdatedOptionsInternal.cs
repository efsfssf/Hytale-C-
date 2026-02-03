using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.RTCData
{
	// Token: 0x020001DD RID: 477
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	internal struct AddNotifyParticipantUpdatedOptionsInternal : ISettable<AddNotifyParticipantUpdatedOptions>, IDisposable
	{
		// Token: 0x17000364 RID: 868
		// (set) Token: 0x06000DCC RID: 3532 RVA: 0x00014330 File Offset: 0x00012530
		public ProductUserId LocalUserId
		{
			set
			{
				Helper.Set(value, ref this.m_LocalUserId);
			}
		}

		// Token: 0x17000365 RID: 869
		// (set) Token: 0x06000DCD RID: 3533 RVA: 0x00014340 File Offset: 0x00012540
		public Utf8String RoomName
		{
			set
			{
				Helper.Set(value, ref this.m_RoomName);
			}
		}

		// Token: 0x06000DCE RID: 3534 RVA: 0x00014350 File Offset: 0x00012550
		public void Set(ref AddNotifyParticipantUpdatedOptions other)
		{
			this.m_ApiVersion = 1;
			this.LocalUserId = other.LocalUserId;
			this.RoomName = other.RoomName;
		}

		// Token: 0x06000DCF RID: 3535 RVA: 0x00014374 File Offset: 0x00012574
		public void Set(ref AddNotifyParticipantUpdatedOptions? other)
		{
			bool flag = other != null;
			if (flag)
			{
				this.m_ApiVersion = 1;
				this.LocalUserId = other.Value.LocalUserId;
				this.RoomName = other.Value.RoomName;
			}
		}

		// Token: 0x06000DD0 RID: 3536 RVA: 0x000143BF File Offset: 0x000125BF
		public void Dispose()
		{
			Helper.Dispose(ref this.m_LocalUserId);
			Helper.Dispose(ref this.m_RoomName);
		}

		// Token: 0x04000647 RID: 1607
		private int m_ApiVersion;

		// Token: 0x04000648 RID: 1608
		private IntPtr m_LocalUserId;

		// Token: 0x04000649 RID: 1609
		private IntPtr m_RoomName;
	}
}
