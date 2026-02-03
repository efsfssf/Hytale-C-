using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.RTCAudio
{
	// Token: 0x0200027B RID: 635
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	internal struct UpdateReceivingVolumeOptionsInternal : ISettable<UpdateReceivingVolumeOptions>, IDisposable
	{
		// Token: 0x1700049E RID: 1182
		// (set) Token: 0x060011CB RID: 4555 RVA: 0x00019F27 File Offset: 0x00018127
		public ProductUserId LocalUserId
		{
			set
			{
				Helper.Set(value, ref this.m_LocalUserId);
			}
		}

		// Token: 0x1700049F RID: 1183
		// (set) Token: 0x060011CC RID: 4556 RVA: 0x00019F37 File Offset: 0x00018137
		public Utf8String RoomName
		{
			set
			{
				Helper.Set(value, ref this.m_RoomName);
			}
		}

		// Token: 0x170004A0 RID: 1184
		// (set) Token: 0x060011CD RID: 4557 RVA: 0x00019F47 File Offset: 0x00018147
		public float Volume
		{
			set
			{
				this.m_Volume = value;
			}
		}

		// Token: 0x060011CE RID: 4558 RVA: 0x00019F51 File Offset: 0x00018151
		public void Set(ref UpdateReceivingVolumeOptions other)
		{
			this.m_ApiVersion = 1;
			this.LocalUserId = other.LocalUserId;
			this.RoomName = other.RoomName;
			this.Volume = other.Volume;
		}

		// Token: 0x060011CF RID: 4559 RVA: 0x00019F84 File Offset: 0x00018184
		public void Set(ref UpdateReceivingVolumeOptions? other)
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

		// Token: 0x060011D0 RID: 4560 RVA: 0x00019FE4 File Offset: 0x000181E4
		public void Dispose()
		{
			Helper.Dispose(ref this.m_LocalUserId);
			Helper.Dispose(ref this.m_RoomName);
		}

		// Token: 0x040007D2 RID: 2002
		private int m_ApiVersion;

		// Token: 0x040007D3 RID: 2003
		private IntPtr m_LocalUserId;

		// Token: 0x040007D4 RID: 2004
		private IntPtr m_RoomName;

		// Token: 0x040007D5 RID: 2005
		private float m_Volume;
	}
}
