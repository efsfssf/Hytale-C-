using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.RTCAudio
{
	// Token: 0x0200027F RID: 639
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	internal struct UpdateSendingOptionsInternal : ISettable<UpdateSendingOptions>, IDisposable
	{
		// Token: 0x170004AF RID: 1199
		// (set) Token: 0x060011F2 RID: 4594 RVA: 0x0001A2FF File Offset: 0x000184FF
		public ProductUserId LocalUserId
		{
			set
			{
				Helper.Set(value, ref this.m_LocalUserId);
			}
		}

		// Token: 0x170004B0 RID: 1200
		// (set) Token: 0x060011F3 RID: 4595 RVA: 0x0001A30F File Offset: 0x0001850F
		public Utf8String RoomName
		{
			set
			{
				Helper.Set(value, ref this.m_RoomName);
			}
		}

		// Token: 0x170004B1 RID: 1201
		// (set) Token: 0x060011F4 RID: 4596 RVA: 0x0001A31F File Offset: 0x0001851F
		public RTCAudioStatus AudioStatus
		{
			set
			{
				this.m_AudioStatus = value;
			}
		}

		// Token: 0x060011F5 RID: 4597 RVA: 0x0001A329 File Offset: 0x00018529
		public void Set(ref UpdateSendingOptions other)
		{
			this.m_ApiVersion = 1;
			this.LocalUserId = other.LocalUserId;
			this.RoomName = other.RoomName;
			this.AudioStatus = other.AudioStatus;
		}

		// Token: 0x060011F6 RID: 4598 RVA: 0x0001A35C File Offset: 0x0001855C
		public void Set(ref UpdateSendingOptions? other)
		{
			bool flag = other != null;
			if (flag)
			{
				this.m_ApiVersion = 1;
				this.LocalUserId = other.Value.LocalUserId;
				this.RoomName = other.Value.RoomName;
				this.AudioStatus = other.Value.AudioStatus;
			}
		}

		// Token: 0x060011F7 RID: 4599 RVA: 0x0001A3BC File Offset: 0x000185BC
		public void Dispose()
		{
			Helper.Dispose(ref this.m_LocalUserId);
			Helper.Dispose(ref this.m_RoomName);
		}

		// Token: 0x040007E3 RID: 2019
		private int m_ApiVersion;

		// Token: 0x040007E4 RID: 2020
		private IntPtr m_LocalUserId;

		// Token: 0x040007E5 RID: 2021
		private IntPtr m_RoomName;

		// Token: 0x040007E6 RID: 2022
		private RTCAudioStatus m_AudioStatus;
	}
}
