using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.RTCAudio
{
	// Token: 0x02000269 RID: 617
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	internal struct SetInputDeviceSettingsOptionsInternal : ISettable<SetInputDeviceSettingsOptions>, IDisposable
	{
		// Token: 0x1700045B RID: 1115
		// (set) Token: 0x06001133 RID: 4403 RVA: 0x00019012 File Offset: 0x00017212
		public ProductUserId LocalUserId
		{
			set
			{
				Helper.Set(value, ref this.m_LocalUserId);
			}
		}

		// Token: 0x1700045C RID: 1116
		// (set) Token: 0x06001134 RID: 4404 RVA: 0x00019022 File Offset: 0x00017222
		public Utf8String RealDeviceId
		{
			set
			{
				Helper.Set(value, ref this.m_RealDeviceId);
			}
		}

		// Token: 0x1700045D RID: 1117
		// (set) Token: 0x06001135 RID: 4405 RVA: 0x00019032 File Offset: 0x00017232
		public bool PlatformAEC
		{
			set
			{
				Helper.Set(value, ref this.m_PlatformAEC);
			}
		}

		// Token: 0x06001136 RID: 4406 RVA: 0x00019042 File Offset: 0x00017242
		public void Set(ref SetInputDeviceSettingsOptions other)
		{
			this.m_ApiVersion = 1;
			this.LocalUserId = other.LocalUserId;
			this.RealDeviceId = other.RealDeviceId;
			this.PlatformAEC = other.PlatformAEC;
		}

		// Token: 0x06001137 RID: 4407 RVA: 0x00019074 File Offset: 0x00017274
		public void Set(ref SetInputDeviceSettingsOptions? other)
		{
			bool flag = other != null;
			if (flag)
			{
				this.m_ApiVersion = 1;
				this.LocalUserId = other.Value.LocalUserId;
				this.RealDeviceId = other.Value.RealDeviceId;
				this.PlatformAEC = other.Value.PlatformAEC;
			}
		}

		// Token: 0x06001138 RID: 4408 RVA: 0x000190D4 File Offset: 0x000172D4
		public void Dispose()
		{
			Helper.Dispose(ref this.m_LocalUserId);
			Helper.Dispose(ref this.m_RealDeviceId);
		}

		// Token: 0x0400078C RID: 1932
		private int m_ApiVersion;

		// Token: 0x0400078D RID: 1933
		private IntPtr m_LocalUserId;

		// Token: 0x0400078E RID: 1934
		private IntPtr m_RealDeviceId;

		// Token: 0x0400078F RID: 1935
		private int m_PlatformAEC;
	}
}
