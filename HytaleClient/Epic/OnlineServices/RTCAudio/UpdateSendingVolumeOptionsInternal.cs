using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.RTCAudio
{
	// Token: 0x02000283 RID: 643
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	internal struct UpdateSendingVolumeOptionsInternal : ISettable<UpdateSendingVolumeOptions>, IDisposable
	{
		// Token: 0x170004C0 RID: 1216
		// (set) Token: 0x06001219 RID: 4633 RVA: 0x0001A6D7 File Offset: 0x000188D7
		public ProductUserId LocalUserId
		{
			set
			{
				Helper.Set(value, ref this.m_LocalUserId);
			}
		}

		// Token: 0x170004C1 RID: 1217
		// (set) Token: 0x0600121A RID: 4634 RVA: 0x0001A6E7 File Offset: 0x000188E7
		public Utf8String RoomName
		{
			set
			{
				Helper.Set(value, ref this.m_RoomName);
			}
		}

		// Token: 0x170004C2 RID: 1218
		// (set) Token: 0x0600121B RID: 4635 RVA: 0x0001A6F7 File Offset: 0x000188F7
		public float Volume
		{
			set
			{
				this.m_Volume = value;
			}
		}

		// Token: 0x0600121C RID: 4636 RVA: 0x0001A701 File Offset: 0x00018901
		public void Set(ref UpdateSendingVolumeOptions other)
		{
			this.m_ApiVersion = 1;
			this.LocalUserId = other.LocalUserId;
			this.RoomName = other.RoomName;
			this.Volume = other.Volume;
		}

		// Token: 0x0600121D RID: 4637 RVA: 0x0001A734 File Offset: 0x00018934
		public void Set(ref UpdateSendingVolumeOptions? other)
		{
			bool flag = other != null;
			if (flag)
			{
				this.m_ApiVersion = 1;
				this.LocalUserId = other.Value.LocalUserId;
				this.RoomName = other.Value.RoomName;
				this.Volume = other.Value.Volume;
			}
		}

		// Token: 0x0600121E RID: 4638 RVA: 0x0001A794 File Offset: 0x00018994
		public void Dispose()
		{
			Helper.Dispose(ref this.m_LocalUserId);
			Helper.Dispose(ref this.m_RoomName);
		}

		// Token: 0x040007F4 RID: 2036
		private int m_ApiVersion;

		// Token: 0x040007F5 RID: 2037
		private IntPtr m_LocalUserId;

		// Token: 0x040007F6 RID: 2038
		private IntPtr m_RoomName;

		// Token: 0x040007F7 RID: 2039
		private float m_Volume;
	}
}
