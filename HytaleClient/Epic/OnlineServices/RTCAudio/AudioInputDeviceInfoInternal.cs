using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.RTCAudio
{
	// Token: 0x0200020B RID: 523
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	internal struct AudioInputDeviceInfoInternal : IGettable<AudioInputDeviceInfo>, ISettable<AudioInputDeviceInfo>, IDisposable
	{
		// Token: 0x170003DE RID: 990
		// (get) Token: 0x06000F1D RID: 3869 RVA: 0x00016314 File Offset: 0x00014514
		// (set) Token: 0x06000F1E RID: 3870 RVA: 0x00016335 File Offset: 0x00014535
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

		// Token: 0x170003DF RID: 991
		// (get) Token: 0x06000F1F RID: 3871 RVA: 0x00016348 File Offset: 0x00014548
		// (set) Token: 0x06000F20 RID: 3872 RVA: 0x00016369 File Offset: 0x00014569
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

		// Token: 0x170003E0 RID: 992
		// (get) Token: 0x06000F21 RID: 3873 RVA: 0x0001637C File Offset: 0x0001457C
		// (set) Token: 0x06000F22 RID: 3874 RVA: 0x0001639D File Offset: 0x0001459D
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

		// Token: 0x06000F23 RID: 3875 RVA: 0x000163AD File Offset: 0x000145AD
		public void Set(ref AudioInputDeviceInfo other)
		{
			this.m_ApiVersion = 1;
			this.DefaultDevice = other.DefaultDevice;
			this.DeviceId = other.DeviceId;
			this.DeviceName = other.DeviceName;
		}

		// Token: 0x06000F24 RID: 3876 RVA: 0x000163E0 File Offset: 0x000145E0
		public void Set(ref AudioInputDeviceInfo? other)
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

		// Token: 0x06000F25 RID: 3877 RVA: 0x00016440 File Offset: 0x00014640
		public void Dispose()
		{
			Helper.Dispose(ref this.m_DeviceId);
			Helper.Dispose(ref this.m_DeviceName);
		}

		// Token: 0x06000F26 RID: 3878 RVA: 0x0001645B File Offset: 0x0001465B
		public void Get(out AudioInputDeviceInfo output)
		{
			output = default(AudioInputDeviceInfo);
			output.Set(ref this);
		}

		// Token: 0x040006D2 RID: 1746
		private int m_ApiVersion;

		// Token: 0x040006D3 RID: 1747
		private int m_DefaultDevice;

		// Token: 0x040006D4 RID: 1748
		private IntPtr m_DeviceId;

		// Token: 0x040006D5 RID: 1749
		private IntPtr m_DeviceName;
	}
}
