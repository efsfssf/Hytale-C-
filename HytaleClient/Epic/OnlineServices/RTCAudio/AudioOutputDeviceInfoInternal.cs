using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.RTCAudio
{
	// Token: 0x0200020F RID: 527
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	internal struct AudioOutputDeviceInfoInternal : IGettable<AudioOutputDeviceInfo>, ISettable<AudioOutputDeviceInfo>, IDisposable
	{
		// Token: 0x170003ED RID: 1005
		// (get) Token: 0x06000F45 RID: 3909 RVA: 0x0001671C File Offset: 0x0001491C
		// (set) Token: 0x06000F46 RID: 3910 RVA: 0x0001673D File Offset: 0x0001493D
		public bool DefaultDevice
		{
			get
			{
				bool result;
				Helper.Get(this.m_DefaultDevice, out result);
				return result;
			}
			set
			{
				Helper.Set(value, ref this.m_DefaultDevice);
			}
		}

		// Token: 0x170003EE RID: 1006
		// (get) Token: 0x06000F47 RID: 3911 RVA: 0x00016750 File Offset: 0x00014950
		// (set) Token: 0x06000F48 RID: 3912 RVA: 0x00016771 File Offset: 0x00014971
		public Utf8String DeviceId
		{
			get
			{
				Utf8String result;
				Helper.Get(this.m_DeviceId, out result);
				return result;
			}
			set
			{
				Helper.Set(value, ref this.m_DeviceId);
			}
		}

		// Token: 0x170003EF RID: 1007
		// (get) Token: 0x06000F49 RID: 3913 RVA: 0x00016784 File Offset: 0x00014984
		// (set) Token: 0x06000F4A RID: 3914 RVA: 0x000167A5 File Offset: 0x000149A5
		public Utf8String DeviceName
		{
			get
			{
				Utf8String result;
				Helper.Get(this.m_DeviceName, out result);
				return result;
			}
			set
			{
				Helper.Set(value, ref this.m_DeviceName);
			}
		}

		// Token: 0x06000F4B RID: 3915 RVA: 0x000167B5 File Offset: 0x000149B5
		public void Set(ref AudioOutputDeviceInfo other)
		{
			this.m_ApiVersion = 1;
			this.DefaultDevice = other.DefaultDevice;
			this.DeviceId = other.DeviceId;
			this.DeviceName = other.DeviceName;
		}

		// Token: 0x06000F4C RID: 3916 RVA: 0x000167E8 File Offset: 0x000149E8
		public void Set(ref AudioOutputDeviceInfo? other)
		{
			bool flag = other != null;
			if (flag)
			{
				this.m_ApiVersion = 1;
				this.DefaultDevice = other.Value.DefaultDevice;
				this.DeviceId = other.Value.DeviceId;
				this.DeviceName = other.Value.DeviceName;
			}
		}

		// Token: 0x06000F4D RID: 3917 RVA: 0x00016848 File Offset: 0x00014A48
		public void Dispose()
		{
			Helper.Dispose(ref this.m_DeviceId);
			Helper.Dispose(ref this.m_DeviceName);
		}

		// Token: 0x06000F4E RID: 3918 RVA: 0x00016863 File Offset: 0x00014A63
		public void Get(out AudioOutputDeviceInfo output)
		{
			output = default(AudioOutputDeviceInfo);
			output.Set(ref this);
		}

		// Token: 0x040006E1 RID: 1761
		private int m_ApiVersion;

		// Token: 0x040006E2 RID: 1762
		private int m_DefaultDevice;

		// Token: 0x040006E3 RID: 1763
		private IntPtr m_DeviceId;

		// Token: 0x040006E4 RID: 1764
		private IntPtr m_DeviceName;
	}
}
