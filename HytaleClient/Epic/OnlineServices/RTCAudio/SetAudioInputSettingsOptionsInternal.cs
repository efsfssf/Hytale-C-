using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.RTCAudio
{
	// Token: 0x02000265 RID: 613
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	internal struct SetAudioInputSettingsOptionsInternal : ISettable<SetAudioInputSettingsOptions>, IDisposable
	{
		// Token: 0x1700044E RID: 1102
		// (set) Token: 0x0600111A RID: 4378 RVA: 0x00018DCB File Offset: 0x00016FCB
		public ProductUserId LocalUserId
		{
			set
			{
				Helper.Set(value, ref this.m_LocalUserId);
			}
		}

		// Token: 0x1700044F RID: 1103
		// (set) Token: 0x0600111B RID: 4379 RVA: 0x00018DDB File Offset: 0x00016FDB
		public Utf8String DeviceId
		{
			set
			{
				Helper.Set(value, ref this.m_DeviceId);
			}
		}

		// Token: 0x17000450 RID: 1104
		// (set) Token: 0x0600111C RID: 4380 RVA: 0x00018DEB File Offset: 0x00016FEB
		public float Volume
		{
			set
			{
				this.m_Volume = value;
			}
		}

		// Token: 0x17000451 RID: 1105
		// (set) Token: 0x0600111D RID: 4381 RVA: 0x00018DF5 File Offset: 0x00016FF5
		public bool PlatformAEC
		{
			set
			{
				Helper.Set(value, ref this.m_PlatformAEC);
			}
		}

		// Token: 0x0600111E RID: 4382 RVA: 0x00018E05 File Offset: 0x00017005
		public void Set(ref SetAudioInputSettingsOptions other)
		{
			this.m_ApiVersion = 1;
			this.LocalUserId = other.LocalUserId;
			this.DeviceId = other.DeviceId;
			this.Volume = other.Volume;
			this.PlatformAEC = other.PlatformAEC;
		}

		// Token: 0x0600111F RID: 4383 RVA: 0x00018E44 File Offset: 0x00017044
		public void Set(ref SetAudioInputSettingsOptions? other)
		{
			bool flag = other != null;
			if (flag)
			{
				this.m_ApiVersion = 1;
				this.LocalUserId = other.Value.LocalUserId;
				this.DeviceId = other.Value.DeviceId;
				this.Volume = other.Value.Volume;
				this.PlatformAEC = other.Value.PlatformAEC;
			}
		}

		// Token: 0x06001120 RID: 4384 RVA: 0x00018EB9 File Offset: 0x000170B9
		public void Dispose()
		{
			Helper.Dispose(ref this.m_LocalUserId);
			Helper.Dispose(ref this.m_DeviceId);
		}

		// Token: 0x0400077D RID: 1917
		private int m_ApiVersion;

		// Token: 0x0400077E RID: 1918
		private IntPtr m_LocalUserId;

		// Token: 0x0400077F RID: 1919
		private IntPtr m_DeviceId;

		// Token: 0x04000780 RID: 1920
		private float m_Volume;

		// Token: 0x04000781 RID: 1921
		private int m_PlatformAEC;
	}
}
