using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.RTCAudio
{
	// Token: 0x02000253 RID: 595
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	internal struct OutputDeviceInformationInternal : IGettable<OutputDeviceInformation>, ISettable<OutputDeviceInformation>, IDisposable
	{
		// Token: 0x17000430 RID: 1072
		// (get) Token: 0x06001094 RID: 4244 RVA: 0x00017934 File Offset: 0x00015B34
		// (set) Token: 0x06001095 RID: 4245 RVA: 0x00017955 File Offset: 0x00015B55
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

		// Token: 0x17000431 RID: 1073
		// (get) Token: 0x06001096 RID: 4246 RVA: 0x00017968 File Offset: 0x00015B68
		// (set) Token: 0x06001097 RID: 4247 RVA: 0x00017989 File Offset: 0x00015B89
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

		// Token: 0x17000432 RID: 1074
		// (get) Token: 0x06001098 RID: 4248 RVA: 0x0001799C File Offset: 0x00015B9C
		// (set) Token: 0x06001099 RID: 4249 RVA: 0x000179BD File Offset: 0x00015BBD
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

		// Token: 0x0600109A RID: 4250 RVA: 0x000179CD File Offset: 0x00015BCD
		public void Set(ref OutputDeviceInformation other)
		{
			this.m_ApiVersion = 1;
			this.DefaultDevice = other.DefaultDevice;
			this.DeviceId = other.DeviceId;
			this.DeviceName = other.DeviceName;
		}

		// Token: 0x0600109B RID: 4251 RVA: 0x00017A00 File Offset: 0x00015C00
		public void Set(ref OutputDeviceInformation? other)
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

		// Token: 0x0600109C RID: 4252 RVA: 0x00017A60 File Offset: 0x00015C60
		public void Dispose()
		{
			Helper.Dispose(ref this.m_DeviceId);
			Helper.Dispose(ref this.m_DeviceName);
		}

		// Token: 0x0600109D RID: 4253 RVA: 0x00017A7B File Offset: 0x00015C7B
		public void Get(out OutputDeviceInformation output)
		{
			output = default(OutputDeviceInformation);
			output.Set(ref this);
		}

		// Token: 0x04000727 RID: 1831
		private int m_ApiVersion;

		// Token: 0x04000728 RID: 1832
		private int m_DefaultDevice;

		// Token: 0x04000729 RID: 1833
		private IntPtr m_DeviceId;

		// Token: 0x0400072A RID: 1834
		private IntPtr m_DeviceName;
	}
}
