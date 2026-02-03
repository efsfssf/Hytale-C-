using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.RTCAudio
{
	// Token: 0x02000267 RID: 615
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	internal struct SetAudioOutputSettingsOptionsInternal : ISettable<SetAudioOutputSettingsOptions>, IDisposable
	{
		// Token: 0x17000455 RID: 1109
		// (set) Token: 0x06001127 RID: 4391 RVA: 0x00018F07 File Offset: 0x00017107
		public ProductUserId LocalUserId
		{
			set
			{
				Helper.Set(value, ref this.m_LocalUserId);
			}
		}

		// Token: 0x17000456 RID: 1110
		// (set) Token: 0x06001128 RID: 4392 RVA: 0x00018F17 File Offset: 0x00017117
		public Utf8String DeviceId
		{
			set
			{
				Helper.Set(value, ref this.m_DeviceId);
			}
		}

		// Token: 0x17000457 RID: 1111
		// (set) Token: 0x06001129 RID: 4393 RVA: 0x00018F27 File Offset: 0x00017127
		public float Volume
		{
			set
			{
				this.m_Volume = value;
			}
		}

		// Token: 0x0600112A RID: 4394 RVA: 0x00018F31 File Offset: 0x00017131
		public void Set(ref SetAudioOutputSettingsOptions other)
		{
			this.m_ApiVersion = 1;
			this.LocalUserId = other.LocalUserId;
			this.DeviceId = other.DeviceId;
			this.Volume = other.Volume;
		}

		// Token: 0x0600112B RID: 4395 RVA: 0x00018F64 File Offset: 0x00017164
		public void Set(ref SetAudioOutputSettingsOptions? other)
		{
			bool flag = other != null;
			if (flag)
			{
				this.m_ApiVersion = 1;
				this.LocalUserId = other.Value.LocalUserId;
				this.DeviceId = other.Value.DeviceId;
				this.Volume = other.Value.Volume;
			}
		}

		// Token: 0x0600112C RID: 4396 RVA: 0x00018FC4 File Offset: 0x000171C4
		public void Dispose()
		{
			Helper.Dispose(ref this.m_LocalUserId);
			Helper.Dispose(ref this.m_DeviceId);
		}

		// Token: 0x04000785 RID: 1925
		private int m_ApiVersion;

		// Token: 0x04000786 RID: 1926
		private IntPtr m_LocalUserId;

		// Token: 0x04000787 RID: 1927
		private IntPtr m_DeviceId;

		// Token: 0x04000788 RID: 1928
		private float m_Volume;
	}
}
