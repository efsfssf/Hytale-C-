using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.RTCAudio
{
	// Token: 0x02000223 RID: 547
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	internal struct InputDeviceInformationInternal : IGettable<InputDeviceInformation>, ISettable<InputDeviceInformation>, IDisposable
	{
		// Token: 0x17000404 RID: 1028
		// (get) Token: 0x06000F91 RID: 3985 RVA: 0x00016D98 File Offset: 0x00014F98
		// (set) Token: 0x06000F92 RID: 3986 RVA: 0x00016DB9 File Offset: 0x00014FB9
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

		// Token: 0x17000405 RID: 1029
		// (get) Token: 0x06000F93 RID: 3987 RVA: 0x00016DCC File Offset: 0x00014FCC
		// (set) Token: 0x06000F94 RID: 3988 RVA: 0x00016DED File Offset: 0x00014FED
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

		// Token: 0x17000406 RID: 1030
		// (get) Token: 0x06000F95 RID: 3989 RVA: 0x00016E00 File Offset: 0x00015000
		// (set) Token: 0x06000F96 RID: 3990 RVA: 0x00016E21 File Offset: 0x00015021
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

		// Token: 0x06000F97 RID: 3991 RVA: 0x00016E31 File Offset: 0x00015031
		public void Set(ref InputDeviceInformation other)
		{
			this.m_ApiVersion = 1;
			this.DefaultDevice = other.DefaultDevice;
			this.DeviceId = other.DeviceId;
			this.DeviceName = other.DeviceName;
		}

		// Token: 0x06000F98 RID: 3992 RVA: 0x00016E64 File Offset: 0x00015064
		public void Set(ref InputDeviceInformation? other)
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

		// Token: 0x06000F99 RID: 3993 RVA: 0x00016EC4 File Offset: 0x000150C4
		public void Dispose()
		{
			Helper.Dispose(ref this.m_DeviceId);
			Helper.Dispose(ref this.m_DeviceName);
		}

		// Token: 0x06000F9A RID: 3994 RVA: 0x00016EDF File Offset: 0x000150DF
		public void Get(out InputDeviceInformation output)
		{
			output = default(InputDeviceInformation);
			output.Set(ref this);
		}

		// Token: 0x04000700 RID: 1792
		private int m_ApiVersion;

		// Token: 0x04000701 RID: 1793
		private int m_DefaultDevice;

		// Token: 0x04000702 RID: 1794
		private IntPtr m_DeviceId;

		// Token: 0x04000703 RID: 1795
		private IntPtr m_DeviceName;
	}
}
