using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.RTCData
{
	// Token: 0x020001ED RID: 493
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	internal struct SendDataOptionsInternal : ISettable<SendDataOptions>, IDisposable
	{
		// Token: 0x1700037F RID: 895
		// (set) Token: 0x06000E3A RID: 3642 RVA: 0x00014CB2 File Offset: 0x00012EB2
		public ProductUserId LocalUserId
		{
			set
			{
				Helper.Set(value, ref this.m_LocalUserId);
			}
		}

		// Token: 0x17000380 RID: 896
		// (set) Token: 0x06000E3B RID: 3643 RVA: 0x00014CC2 File Offset: 0x00012EC2
		public Utf8String RoomName
		{
			set
			{
				Helper.Set(value, ref this.m_RoomName);
			}
		}

		// Token: 0x17000381 RID: 897
		// (set) Token: 0x06000E3C RID: 3644 RVA: 0x00014CD2 File Offset: 0x00012ED2
		public ArraySegment<byte> Data
		{
			set
			{
				Helper.Set(value, ref this.m_Data, out this.m_DataLengthBytes);
			}
		}

		// Token: 0x06000E3D RID: 3645 RVA: 0x00014CE8 File Offset: 0x00012EE8
		public void Set(ref SendDataOptions other)
		{
			this.m_ApiVersion = 1;
			this.LocalUserId = other.LocalUserId;
			this.RoomName = other.RoomName;
			this.Data = other.Data;
		}

		// Token: 0x06000E3E RID: 3646 RVA: 0x00014D1C File Offset: 0x00012F1C
		public void Set(ref SendDataOptions? other)
		{
			bool flag = other != null;
			if (flag)
			{
				this.m_ApiVersion = 1;
				this.LocalUserId = other.Value.LocalUserId;
				this.RoomName = other.Value.RoomName;
				this.Data = other.Value.Data;
			}
		}

		// Token: 0x06000E3F RID: 3647 RVA: 0x00014D7C File Offset: 0x00012F7C
		public void Dispose()
		{
			Helper.Dispose(ref this.m_LocalUserId);
			Helper.Dispose(ref this.m_RoomName);
			Helper.Dispose(ref this.m_Data);
		}

		// Token: 0x0400066C RID: 1644
		private int m_ApiVersion;

		// Token: 0x0400066D RID: 1645
		private IntPtr m_LocalUserId;

		// Token: 0x0400066E RID: 1646
		private IntPtr m_RoomName;

		// Token: 0x0400066F RID: 1647
		private uint m_DataLengthBytes;

		// Token: 0x04000670 RID: 1648
		private IntPtr m_Data;
	}
}
