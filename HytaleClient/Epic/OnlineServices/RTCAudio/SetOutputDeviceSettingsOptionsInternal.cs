using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.RTCAudio
{
	// Token: 0x0200026B RID: 619
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	internal struct SetOutputDeviceSettingsOptionsInternal : ISettable<SetOutputDeviceSettingsOptions>, IDisposable
	{
		// Token: 0x17000460 RID: 1120
		// (set) Token: 0x0600113D RID: 4413 RVA: 0x00019111 File Offset: 0x00017311
		public ProductUserId LocalUserId
		{
			set
			{
				Helper.Set(value, ref this.m_LocalUserId);
			}
		}

		// Token: 0x17000461 RID: 1121
		// (set) Token: 0x0600113E RID: 4414 RVA: 0x00019121 File Offset: 0x00017321
		public Utf8String RealDeviceId
		{
			set
			{
				Helper.Set(value, ref this.m_RealDeviceId);
			}
		}

		// Token: 0x0600113F RID: 4415 RVA: 0x00019131 File Offset: 0x00017331
		public void Set(ref SetOutputDeviceSettingsOptions other)
		{
			this.m_ApiVersion = 1;
			this.LocalUserId = other.LocalUserId;
			this.RealDeviceId = other.RealDeviceId;
		}

		// Token: 0x06001140 RID: 4416 RVA: 0x00019158 File Offset: 0x00017358
		public void Set(ref SetOutputDeviceSettingsOptions? other)
		{
			bool flag = other != null;
			if (flag)
			{
				this.m_ApiVersion = 1;
				this.LocalUserId = other.Value.LocalUserId;
				this.RealDeviceId = other.Value.RealDeviceId;
			}
		}

		// Token: 0x06001141 RID: 4417 RVA: 0x000191A3 File Offset: 0x000173A3
		public void Dispose()
		{
			Helper.Dispose(ref this.m_LocalUserId);
			Helper.Dispose(ref this.m_RealDeviceId);
		}

		// Token: 0x04000792 RID: 1938
		private int m_ApiVersion;

		// Token: 0x04000793 RID: 1939
		private IntPtr m_LocalUserId;

		// Token: 0x04000794 RID: 1940
		private IntPtr m_RealDeviceId;
	}
}
